//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if !(ENABLE_IL2CPP || NET_STANDARD_2_0 || NETFX_CORE || ARBOR_DLL)
#define ENABLE_DELEGATE_CALL
#endif

using System.Collections.Generic;
using System.Reflection;

#if ENABLE_DELEGATE_CALL
using System.Reflection.Emit;
#endif

namespace Arbor.DynamicReflection
{
#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドへ直接的にアクセスするクラス。
	/// </summary>
	/// <remarks>AOTやIL2CPP環境ではReflectionによるアクセスとなる。</remarks>
#else
	/// <summary>
	/// A class that accesses Field directly.
	/// </summary>
	/// <remarks>In AOT and IL2CPP environments, it is access by Reflection.</remarks>
#endif
	public abstract class DynamicField
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 参照しているFieldInfo
		/// </summary>
#else
		/// <summary>
		/// Referencing FieldInfo
		/// </summary>
#endif
		public FieldInfo fieldInfo
		{
			get;
			protected set;
		}

		private void Create(FieldInfo fieldInfo)
		{
			this.fieldInfo = fieldInfo;

			OnCreate(fieldInfo);
		}

		internal abstract void OnCreate(FieldInfo fieldInfo);

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドの値を返す。
		/// </summary>
		/// <param name="instance">フィールドを持っているインスタンス</param>
		/// <returns>フィールドの値</returns>
#else
		/// <summary>
		/// Returns the value of the field.
		/// </summary>
		/// <param name="instance">An instance that has a field</param>
		/// <returns>Field value</returns>
#endif
		public abstract object GetValue(object instance);

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドの値を設定する。
		/// </summary>
		/// <param name="instance">フィールドを持っているインスタンス</param>
		/// <param name="value">フィールドの値</param>
#else
		/// <summary>
		/// Set the value of the field.
		/// </summary>
		/// <param name="instance">An instance that has a field</param>
		/// <param name="value">Field value</param>
#endif
		public abstract void SetValue(object instance, object value);

		private static Dictionary<FieldInfo, DynamicField> s_FieldCache = new Dictionary<FieldInfo, DynamicField>();

#if ARBOR_DOC_JA
		/// <summary>
		/// DynamicFieldを返す。
		/// </summary>
		/// <param name="fieldInfo">参照するFieldInfo</param>
		/// <returns>指定したFieldInfoを参照したDynamicField</returns>
#else
		/// <summary>
		/// Returns DynamicField.
		/// </summary>
		/// <param name="fieldInfo">Reference FieldInfo</param>
		/// <returns>DynamicField referencing the specified FieldInfo</returns>
#endif
		public static DynamicField GetField(FieldInfo fieldInfo)
		{
			DynamicField dynamicField = null;

			if (fieldInfo != null && !s_FieldCache.TryGetValue(fieldInfo, out dynamicField))
			{
#if ENABLE_DELEGATE_CALL
				try
				{
					if (fieldInfo.IsLiteral)
					{
						dynamicField = new ConstField();
					}
					else
					{
						dynamicField = new DelegatedField();
					}
					dynamicField.Create(fieldInfo);
				}
				catch (System.NotSupportedException)
				{
					dynamicField = new DefaultField();
					dynamicField.Create(fieldInfo);
				}
				catch (System.Exception ex)
				{
					UnityEngine.Debug.LogException(ex);

					dynamicField = new DefaultField();
					dynamicField.Create(fieldInfo);
				}
#else
				if (fieldInfo.IsLiteral)
				{
					dynamicField = new ConstField();
				}
				else
				{
					dynamicField = new DefaultField();
				}
				dynamicField.Create(fieldInfo);
#endif

				s_FieldCache.Add(fieldInfo, dynamicField);
			}

			return dynamicField;
		}

#if ENABLE_DELEGATE_CALL
		internal sealed class DelegatedField : DynamicField
		{
			private delegate object GetValueDelegate(object instance);
			private delegate void SetValueDelegate(object instance, object value);

			private GetValueDelegate _GetDelegate = null;
			private SetValueDelegate _SetDelegate = null;

			static GetValueDelegate CreateGetDelegateIL(FieldInfo fieldInfo)
			{
				System.Reflection.Emit.DynamicMethod dm = new System.Reflection.Emit.DynamicMethod("", typeof(object), new[] { typeof(object) }, true);

				ILGenerator generator = dm.GetILGenerator();

				bool isStatic = fieldInfo.IsStatic;
				System.Type fieldType = fieldInfo.FieldType;

				System.Type declaringType = fieldInfo.DeclaringType;
				bool isValueType = declaringType != null && TypeUtility.IsValueType(declaringType);

				LocalBuilder localInstance = null;

				if (!isStatic && isValueType)
				{
					localInstance = generator.DeclareLocal(declaringType);
				}

				if (!isStatic)
				{
					// load instance
					generator.Emit(OpCodes.Ldarg_0);

					if (isValueType)
					{
						generator.Emit(OpCodes.Unbox_Any, declaringType);
						generator.Emit(OpCodes.Stloc, localInstance);
						generator.Emit(OpCodes.Ldloca_S, localInstance);
					}
					else
					{
						generator.Emit(OpCodes.Castclass, declaringType);
					}
				}

				// get value
				if (isStatic)
				{
					generator.Emit(OpCodes.Ldsfld, fieldInfo);
				}
				else
				{
					generator.Emit(OpCodes.Ldfld, fieldInfo);
				}

				if (TypeUtility.IsValueType(fieldType))
				{
					generator.Emit(OpCodes.Box, fieldType);
				}

				// return
				generator.Emit(OpCodes.Ret);

				return (GetValueDelegate)dm.CreateDelegate(typeof(GetValueDelegate));
			}

			static SetValueDelegate CreateSetDelegateIL(FieldInfo fieldInfo)
			{
				System.Reflection.Emit.DynamicMethod dm = new System.Reflection.Emit.DynamicMethod("", typeof(void), new[] { typeof(object), typeof(object) }, true);

				ILGenerator generator = dm.GetILGenerator();

				bool isStatic = fieldInfo.IsStatic;
				System.Type fieldType = fieldInfo.FieldType;

				System.Type declaringType = fieldInfo.DeclaringType;
				bool isValueType = declaringType != null && TypeUtility.IsValueType(declaringType);

				if (!isStatic)
				{
					// load instance
					generator.Emit(OpCodes.Ldarg_0);

					if (isValueType)
					{
						generator.Emit(OpCodes.Unbox, declaringType);
					}
					else
					{
						generator.Emit(OpCodes.Castclass, declaringType);
					}
				}

				// load value
				generator.Emit(OpCodes.Ldarg_1);
				if (TypeUtility.IsValueType(fieldType))
				{
					generator.Emit(OpCodes.Unbox_Any, fieldType);
				}
				else
				{
					generator.Emit(OpCodes.Castclass, fieldType);
				}

				// set value
				if (isStatic)
				{
					generator.Emit(OpCodes.Stsfld, fieldInfo);
				}
				else
				{
					generator.Emit(OpCodes.Stfld, fieldInfo);
				}

				// return
				generator.Emit(OpCodes.Ret);

				return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
			}

			internal override void OnCreate(FieldInfo fieldInfo)
			{
				_GetDelegate = CreateGetDelegateIL(fieldInfo);

				if (fieldInfo.IsInitOnly)// readonly
				{
					_SetDelegate = null;
				}
				else
				{
					_SetDelegate = CreateSetDelegateIL(fieldInfo);
				}
			}

			public override object GetValue(object instance)
			{
				if (_GetDelegate != null)
				{
					return _GetDelegate(instance);
				}

				return null;
			}

			public override void SetValue(object instance, object value)
			{
				_SetDelegate?.Invoke(instance, value);
			}
		}
#endif

		internal sealed class ConstField : DynamicField
		{
			private object _Value;

			internal override void OnCreate(FieldInfo fieldInfo)
			{
				_Value = fieldInfo.GetValue(null);
			}

			public override object GetValue(object instance)
			{
				return _Value;
			}

			public override void SetValue(object instance, object value)
			{
			}
		}

		internal sealed class DefaultField : DynamicField
		{
			internal override void OnCreate(FieldInfo fieldInfo)
			{
			}

			public override object GetValue(object instance)
			{
				if (fieldInfo != null)
				{
					return fieldInfo.GetValue(instance);
				}

				return null;
			}

			public override void SetValue(object instance, object value)
			{
				if (fieldInfo != null && !fieldInfo.IsInitOnly)
				{
					fieldInfo.SetValue(instance, value);
				}
			}
		}
	}
}