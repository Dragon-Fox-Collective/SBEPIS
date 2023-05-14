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
	/// メソッドへ直接的にアクセスするクラス。
	/// </summary>
	/// <remarks>AOTやIL2CPP環境ではReflectionによるアクセスとなる。</remarks>
#else
	/// <summary>
	/// A class that accesses Method directly.
	/// </summary>
	/// <remarks>In AOT and IL2CPP environments, it is access by Reflection.</remarks>
#endif
	public abstract class DynamicMethod
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 参照しているMethodInfo
		/// </summary>
#else
		/// <summary>
		/// Referencing MethodInfo
		/// </summary>
#endif
		public MethodInfo methodInfo
		{
			get;
			protected set;
		}

		private void Create(MethodInfo methodInfo)
		{
			this.methodInfo = methodInfo;

			OnCreate(methodInfo);
		}

		internal abstract void OnCreate(MethodInfo methodInfo);

#if ARBOR_DOC_JA
		/// <summary>
		/// メソッドを呼び出す
		/// </summary>
		/// <param name="instance">メソッドを持っているインスタンス</param>
		/// <param name="arguments">引数リスト</param>
		/// <returns>メソッドの戻り値</returns>
#else
		/// <summary>
		/// Call method
		/// </summary>
		/// <param name="instance">An instance that has a method</param>
		/// <param name="arguments">Argument list</param>
		/// <returns>Method return value</returns>
#endif
		public abstract object Invoke(object instance, object[] arguments);

		private static Dictionary<MethodInfo, DynamicMethod> s_MethodCache = new Dictionary<MethodInfo, DynamicMethod>();

#if ARBOR_DOC_JA
		/// <summary>
		/// DynamicMethodを返す。
		/// </summary>
		/// <param name="methodInfo">参照するMethodInfo</param>
		/// <returns>指定したMethodInfoを参照したDynamicMethod</returns>
#else
		/// <summary>
		/// Returns DynamicMethod.
		/// </summary>
		/// <param name="methodInfo">Reference MethodInfo</param>
		/// <returns>DynamicMethod referencing the specified MethodInfo</returns>
#endif
		public static DynamicMethod GetMethod(MethodInfo methodInfo)
		{
			DynamicMethod dynamicMethod = null;

			if (methodInfo != null && !s_MethodCache.TryGetValue(methodInfo, out dynamicMethod))
			{
#if ENABLE_DELEGATE_CALL
				try
				{
					dynamicMethod = new DelegatedMethod();
					dynamicMethod.Create(methodInfo);
				}
				catch (System.NotSupportedException)
				{
					dynamicMethod = new DefaultMethod();
					dynamicMethod.Create(methodInfo);
				}
				catch (System.Exception ex)
				{
					UnityEngine.Debug.LogException(ex);

					dynamicMethod = new DefaultMethod();
					dynamicMethod.Create(methodInfo);
				}
#else
				dynamicMethod = new DefaultMethod();
				dynamicMethod.Create(methodInfo);
#endif

				s_MethodCache.Add(methodInfo, dynamicMethod);
			}

			return dynamicMethod;
		}
	}

#if ENABLE_DELEGATE_CALL
	internal sealed class DelegatedMethod : DynamicMethod
	{
		private delegate object ReturnValueDelegate(object instance, object[] arguments);
		private delegate void VoidDelegate(object instance, object[] arguments);

		private ReturnValueDelegate _Delegate = null;

		static ReturnValueDelegate CreateDelegateIL(MethodInfo methodInfo)
		{
			System.Reflection.Emit.DynamicMethod dm = new System.Reflection.Emit.DynamicMethod("", typeof(object), new[] { typeof(object), typeof(object[]) }, true);

			ILGenerator generator = dm.GetILGenerator();

			ParameterInfo[] parameters = methodInfo.GetParameters();
			int parameterCount = parameters.Length;
			LocalBuilder[] locals = new LocalBuilder[parameterCount];
			bool[] isByRefs = new bool[parameterCount];
			System.Type[] parameterTypes = new System.Type[parameterCount];
			LocalBuilder ret = null;
			LocalBuilder exception = null;

			System.Type declaringType = methodInfo.DeclaringType;

			bool isValueType = TypeUtility.IsValueType(declaringType);

			bool isStatic = methodInfo.IsStatic;

			// declare locals (ParameterInfo.IsByRef)
			for (int i = 0; i < parameterCount; ++i)
			{
				ParameterInfo parameter = parameters[i];

				System.Type parameterType = parameter.ParameterType;
				bool isByRef = parameterType.IsByRef;
				if (isByRef)
				{
					parameterType = parameterType.GetElementType();
					locals[i] = generator.DeclareLocal(parameterType);
				}

				parameterTypes[i] = parameterType;
				isByRefs[i] = isByRef;
			}

			// declare local (return)
			System.Type returnType = methodInfo.ReturnType;
			bool isReturn = returnType != typeof(void);

			if (isReturn)
			{
				ret = generator.DeclareLocal(returnType);
			}

			exception = generator.DeclareLocal(typeof(System.Exception));

			// arguments to locals
			for (int i = 0; i < parameterCount; ++i)
			{
				if (!isByRefs[i])
				{
					continue;
				}

				ParameterInfo parameter = parameters[i];

				System.Type parameterType = parameterTypes[i];

				if (parameter.IsOut)
				{
					if (TypeUtility.IsValueType(parameterType))
					{
						generator.Emit(OpCodes.Ldloca_S, locals[i]);
						generator.Emit(OpCodes.Initobj, parameterType);
						generator.Emit(OpCodes.Ldloc, locals[i]);
					}
					else
					{
						generator.Emit(OpCodes.Ldnull);
					}
				}
				else
				{
					generator.Emit(OpCodes.Ldarg_1);
					generator.Emit(OpCodes.Ldc_I4, i);
					generator.Emit(OpCodes.Ldelem_Ref);
					if (TypeUtility.IsValueType(parameterType))
					{
						generator.Emit(OpCodes.Unbox_Any, parameterType);
					}
					else
					{
						generator.Emit(OpCodes.Castclass, parameterType);
					}
				}

				generator.Emit(OpCodes.Stloc, locals[i]);
			}

			// begin call try
			Label callExceptionLabel = generator.BeginExceptionBlock();

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

			// load arguments
			for (int i = 0; i < parameterCount; ++i)
			{
				ParameterInfo parameter = parameters[i];

				System.Type parameterType = parameter.ParameterType;
				if (parameterType.IsByRef)
				{
					generator.Emit(OpCodes.Ldloca_S, locals[i]);
				}
				else
				{
					generator.Emit(OpCodes.Ldarg_1);
					generator.Emit(OpCodes.Ldc_I4, i);
					generator.Emit(OpCodes.Ldelem_Ref);
					if (TypeUtility.IsValueType(parameterTypes[i]))
					{
						generator.Emit(OpCodes.Unbox_Any, parameterTypes[i]);
					}
					else
					{
						generator.Emit(OpCodes.Castclass, parameterTypes[i]);
					}
				}
			}

			// call method
			generator.Emit((!isValueType && !isStatic) ? OpCodes.Callvirt : OpCodes.Call, methodInfo);

			// return value to local
			if (isReturn)
			{
				generator.Emit(OpCodes.Stloc_S, ret);
			}

			generator.Emit(OpCodes.Leave, callExceptionLabel);

			// catch(System.Exception)
			generator.BeginCatchBlock(typeof(System.Exception));

			generator.Emit(OpCodes.Stloc_S, exception);
			generator.Emit(OpCodes.Ldloc_S, exception);
			generator.Emit(OpCodes.Newobj, typeof(TargetInvocationException).GetConstructor(new[] { typeof(System.Exception) }));
			generator.Emit(OpCodes.Throw);

			generator.Emit(OpCodes.Leave, callExceptionLabel);

			generator.EndExceptionBlock();

			// locals to arguments
			for (int i = 0; i < parameterCount; ++i)
			{
				if (!isByRefs[i])
				{
					continue;
				}

				// local to argument (ParameterInfo.IsByRef)
				generator.Emit(OpCodes.Ldarg_1);
				generator.Emit(OpCodes.Ldc_I4, i);
				generator.Emit(OpCodes.Ldloc, locals[i]);
				if (TypeUtility.IsValueType(parameterTypes[i]))
				{
					generator.Emit(OpCodes.Box, parameterTypes[i]);
				}
				generator.Emit(OpCodes.Stelem_Ref);
			}

			// load return value
			if (isReturn)
			{
				if (TypeUtility.IsValueType(returnType))
				{
					generator.Emit(OpCodes.Ldloc_S, ret);
					generator.Emit(OpCodes.Box, returnType);
				}
				else
				{
					generator.Emit(OpCodes.Ldloc, ret);
				}
			}
			else
			{
				generator.Emit(OpCodes.Ldnull);
			}

			// return
			generator.Emit(OpCodes.Ret);

			return (ReturnValueDelegate)dm.CreateDelegate(typeof(ReturnValueDelegate));
		}

		internal override void OnCreate(MethodInfo methodInfo)
		{
			_Delegate = CreateDelegateIL(methodInfo);
		}

		public override object Invoke(object instance, object[] arguments)
		{
			if (_Delegate != null)
			{
				return _Delegate(instance, arguments);
			}

			return null;
		}
	}
#endif

	internal sealed class DefaultMethod : DynamicMethod
	{
		internal override void OnCreate(MethodInfo methodInfo)
		{
		}

		public override object Invoke(object instance, object[] arguments)
		{
			if (methodInfo != null)
			{
				return methodInfo.Invoke(instance, arguments);
			}

			return null;
		}
	}
}