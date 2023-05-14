//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 値のやりとりを仲介するクラス。ランタイムに型が決定する箇所において、極力ボックス化を回避するために使用する。
	/// </summary>
#else
	/// <summary>
	/// A class that mediates the exchange of values. It is used to avoid boxing as much as possible where the type is determined at runtime.
	/// </summary>
#endif
	public class ValueMediator
	{
		private static Dictionary<System.Type, ValueMediator> s_Mediators = new Dictionary<System.Type, ValueMediator>();
		
		static ValueMediator()
		{
			Register<sbyte>();
			Register<byte>();
			Register<short>();
			Register<ushort>();
			Register<int>();
			Register<uint>();
			Register<long>();
			Register<ulong>();
			Register<char>();
			Register<float>();
			Register<double>();
			Register<bool>();
			Register<decimal>();

			Register<Vector2>();
			Register<Vector3>();
			Register<Vector4>();
			Register<Quaternion>();
			Register<Rect>();
			Register<Bounds>();
			Register<Color>();
			Register<Color32>();
			Register<Matrix4x4>();
			Register<Vector2Int>();
			Register<Vector3Int>();
			Register<RectInt>();
			Register<BoundsInt>();
			Register<Ray>();
			Register<Ray2D>();
			Register<RaycastHit>();
			Register<RaycastHit2D>();

			var types = TypeUtility.GetRuntimeTypes();
			if (types != null)
			{
				for (int typeIndex = 0; typeIndex < types.Count; typeIndex++)
				{
					System.Type type = types[typeIndex];

					var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++)
					{
						var method = methods[methodIndex];

						if (!AttributeHelper.HasAttribute<ValueMediatorInitializeOnLoadMethod>(method))
						{
							continue;
						}

						method.Invoke(null, null);
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 登録している型の配列を取得する。
		/// </summary>
		/// <returns>登録している型の配列を返す。</returns>
#else
		/// <summary>
		/// Get an array of registered types.
		/// </summary>
		/// <returns>Returns an array of registered types.</returns>
#endif
		public static System.Type[] GetRegisteredTypes()
		{
			var keys = s_Mediators.Keys;
			var types = new System.Type[keys.Count];
			keys.CopyTo(types, 0);
			return types;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ValueMediatorを登録する。
		/// </summary>
		/// <typeparam name="T">仲介したい値の型</typeparam>
		/// <remarks>ValueMediatorInitializeOnLoadMethod属性をメソッドへ指定することで、初期化時に任意の型のValueMediatorを登録できる。</remarks>
#else
		/// <summary>
		/// Register ValueMediator.
		/// </summary>
		/// <typeparam name="T">The type of value you want to broker.</typeparam>
		/// <remarks>By specifying the ValueMediatorInitializeOnLoadMethod attribute to a method, a ValueMediator of any type can be registered at the time of initialization.</remarks>
#endif
		public static void Register<T>() where T : struct
		{
			System.Type type = typeof(T);
			if (s_Mediators.ContainsKey(type))
			{
				return;
			}
			s_Mediators.Add(type, new ValueMediator<T>());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型をValueMediatorに登録する。
		/// </summary>
		/// <typeparam name="T">仲介したい値の型</typeparam>
		/// <remarks>ValueMediatorInitializeOnLoadMethod属性をメソッドへ指定することで、初期化時に任意の型のValueMediatorを登録できる。</remarks>
#else
		/// <summary>
		/// Register Enum type in ValueMediator.
		/// </summary>
		/// <typeparam name="T">The type of value you want to broker.</typeparam>
		/// <remarks>By specifying the ValueMediatorInitializeOnLoadMethod attribute to a method, a ValueMediator of any type can be registered at the time of initialization.</remarks>
#endif
		public static void RegisterEnum<T>() where T : struct, System.Enum
		{
			System.Type type = typeof(T);
			if (s_Mediators.ContainsKey(type))
			{
				return;
			}

			s_Mediators.Add(type, new EnumMediator<T>());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ValueMediatorを所持しているかどうかを確認する。
		/// </summary>
		/// <param name="valueType">値の型</param>
		/// <returns>ValueMediatorがある場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Check to see if you have ValueMediator in your possession.
		/// </summary>
		/// <param name="valueType">Value type</param>
		/// <returns>Returns true if there is a ValueMediator.</returns>
#endif
		public static bool HasMediator(System.Type valueType)
		{
			return s_Mediators.ContainsKey(valueType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した型のValueMediatorを取得する。
		/// </summary>
		/// <param name="type">値の型</param>
		/// <returns>見つかったValueMediator。未登録の型の場合はボックス化される可能性のあるValueMediatorを返す。</returns>
#else
		/// <summary>
		/// Gets the ValueMediator of the specified type.
		/// </summary>
		/// <param name="type">Value type</param>
		/// <returns>ValueMediator found; for unregistered types, returns the ValueMediator that may be boxed.</returns>
#endif
		public static ValueMediator Get(System.Type type)
		{
			ValueMediator mediator = null;
			if (s_Mediators.TryGetValue(type, out mediator))
			{
				return mediator;
			}

			mediator = new ValueMediator(type, new ListMediator(type));
			s_Mediators.Add(type, mediator);

			return mediator;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値の型
		/// </summary>
#else
		/// <summary>
		/// Value type
		/// </summary>
#endif
		public readonly System.Type valueType;

#if ARBOR_DOC_JA
		/// <summary>
		/// リスト用の仲介クラス
		/// </summary>
#else
		/// <summary>
		/// Mediator class for lists
		/// </summary>
#endif
		public readonly ListMediator listMediator;

		internal ValueMediator(System.Type valueType, ListMediator listMediator)
		{
			this.valueType = valueType;
			this.listMediator = listMediator;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IValueContainerを作成する。
		/// </summary>
		/// <returns>作成したIValueContainer。未登録の型の場合にボックス化される可能性のあるIValueContainerを返す。</returns>
#else
		/// <summary>
		/// Create an IValueContainer.
		/// </summary>
		/// <returns>IValueContainer created; returns the IValueContainer that may be boxed in the case of an unregistered type.</returns>
#endif
		public virtual IValueContainer CreateContainer()
		{
			return new ValueContainer<object>();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値を出力する。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		/// <param name="setter">パラメータの値を受け取るIValueSetter</param>
#else
		/// <summary>
		/// Exports the value of the parameter.
		/// </summary>
		/// <param name="parameter">Parameter</param>
		/// <param name="setter">IValueSetter to receive the value of a parameter.</param>
#endif
		public virtual void ExportParameter(Parameter parameter, IValueSetter setter)
		{
			setter.SetValueObject(parameter.value);
		}
	}
}