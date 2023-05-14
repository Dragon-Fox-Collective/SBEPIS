//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterContainerに格納されるParameterのクラス。
	/// </summary>
#else
	/// <summary>
	/// Class of Parameter to be stored in the ParameterContainer.
	/// </summary>
#endif
	[System.Serializable]
	public sealed partial class Parameter : ISerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを変更した時に呼ばれるデリゲート。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// Delegate called when changing Parameter.
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public delegate void DelegateOnChanged(Parameter parameter);

#if ARBOR_DOC_JA
		/// <summary>
		/// このパラメータが格納されているコンテナ。
		/// </summary>
#else
		/// <summary>
		/// Container this parameter is stored.
		/// </summary>
#endif
		public ParameterContainerInternal container;

#if ARBOR_DOC_JA
		/// <summary>
		/// ID。
		/// </summary>
#else
		/// <summary>
		/// ID.
		/// </summary>
#endif
		public int id;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの型。
		/// </summary>
#else
		/// <summary>
		/// Type.
		/// </summary>
#endif
		public Type type;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの名前。
		/// </summary>
#else
		/// <summary>
		/// Name.
		/// </summary>
#endif
		public string name;

		[SerializeField]
		internal int _ParameterIndex;

		[SerializeField]
		internal bool _IsPublicSet = true;

		[SerializeField]
		internal bool _IsPublicGet = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// objectReferenceValueやEnumの型。
		/// </summary>
#else
		/// <summary>
		/// the type of objectReferenceValue and Enum.
		/// </summary>
#endif
		public ClassTypeReference referenceType = new ClassTypeReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// このパラメータが外部グラフから設定可能かどうかを返す。(グラフ内パラメータ用)
		/// </summary>
#else
		/// <summary>
		/// Returns whether this parameter can be set from an external graph. (For parameters in graph)
		/// </summary>
#endif
		public bool isPublicSet
		{
			get
			{
				return _IsPublicSet;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このパラメータが外部グラフから取得可能かどうかを返す。(グラフ内パラメータ用)
		/// </summary>
#else
		/// <summary>
		/// Returns whether this parameter can be get from an external graph. (For parameters in graph)
		/// </summary>
#endif
		public bool isPublicGet
		{
			get
			{
				return _IsPublicGet;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したParameter.Typeで扱う値の型がIList&lt;&gt;であるかを判断する。
		/// </summary>
		/// <param name="type">パラメータのタイプ</param>
		/// <returns>値の型がIList&lt;&gt;であればtrueを返す。</returns>
#else
		/// <summary>
		/// Judges whether the type of the value handled by the specified Parameter.Type is IList&lt;&gt;.
		/// </summary>
		/// <param name="type">Parameter type</param>
		/// <returns>Returns true if the value type is IListIList&lt;&gt;.</returns>
#endif
		public static bool IsListType(Parameter.Type type)
		{
			var valueType = ParameterUtility.GetValueType(type);
			return TypeUtility.IsGeneric(valueType, typeof(IList<>));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値の型を取得する。
		/// </summary>
		/// <param name="type">パラメータのタイプ</param>
		/// <param name="referenceType">参照する型（Enum, Component, Variableで使用）</param>
		/// <returns>パラメータの値の型</returns>
#else
		/// <summary>
		/// Get the value type of the parameter.
		/// </summary>
		/// <param name="type">Type of parameter</param>
		/// <param name="referenceType">Reference type (used for Enum, Component, Variable)</param>
		/// <returns>Value type of the parameter</returns>
#endif
		[System.Obsolete("use ParameterTypeUtility.GetValueType()")]
		public static System.Type GetValueType(Parameter.Type type, System.Type referenceType = null)
		{
			return ParameterUtility.GetValueType(type, referenceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値の型を取得する。
		/// </summary>
#else
		/// <summary>
		/// Get value type.
		/// </summary>
#endif
		public System.Type valueType
		{
			get
			{
				System.Type referenceType = null;
				switch (type)
				{
					case Type.Variable:
						{
							VariableBase variable = Internal_GetObject() as VariableBase;
							if (variable != null)
							{
								return variable.valueType;
							}
						}
						break;
					case Type.VariableList:
						{
							VariableListBase variable = Internal_GetObject() as VariableListBase;
							if (variable != null)
							{
								return variable.valueType;
							}
						}
						break;
					default:
						referenceType = this.referenceType;
						break;
				}

				return ParameterUtility.GetValueType(type, referenceType);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// タイプに応じた値を取得する。
		/// </summary>
#else
		/// <summary>
		/// Get values according to type.
		/// </summary>
#endif
		public object value
		{
			get
			{
				switch (type)
				{
					case Type.Int:
						return intValue;
					case Type.Long:
						return longValue;
					case Type.Float:
						return floatValue;
					case Type.Bool:
						return boolValue;
					case Type.GameObject:
						return gameObjectValue;
					case Type.String:
						return stringValue;
					case Type.Enum:
						{
							System.Type enumType = referenceType.type;
							if (EnumFieldUtility.IsEnum(enumType))
							{
								return enumValue;
							}

							return enumIntValue;
						}
					case Type.Vector2:
						return vector2Value;
					case Type.Vector3:
						return vector3Value;
					case Type.Quaternion:
						return quaternionValue;
					case Type.Rect:
						return rectValue;
					case Type.Bounds:
						return boundsValue;
					case Type.Color:
						return colorValue;
					case Type.Vector4:
						return vector4Value;
					case Type.Vector2Int:
						return vector2IntValue;
					case Type.Vector3Int:
						return vector3IntValue;
					case Type.RectInt:
						return rectIntValue;
					case Type.BoundsInt:
						return boundsIntValue;
					case Type.Transform:
						return transformValue;
					case Type.RectTransform:
						return rectTransformValue;
					case Type.Rigidbody:
						return rigidbodyValue;
					case Type.Rigidbody2D:
						return rigidbody2DValue;
					case Type.Component:
						return componentValue;
					case Type.AssetObject:
						return objectReferenceValue;
					case Type.Variable:
						return variableValue;
					case Type.IntList:
						return intListValue;
					case Type.LongList:
						return longListValue;
					case Type.FloatList:
						return floatListValue;
					case Type.BoolList:
						return boolListValue;
					case Type.StringList:
						return stringListValue;
					case Type.EnumList:
						return Internal_GetEnumList();
					case Type.Vector2List:
						return vector2ListValue;
					case Type.Vector3List:
						return vector3ListValue;
					case Type.QuaternionList:
						return quaternionListValue;
					case Type.RectList:
						return rectListValue;
					case Type.BoundsList:
						return boundsListValue;
					case Type.ColorList:
						return colorListValue;
					case Type.Vector4List:
						return vector4ListValue;
					case Type.Vector2IntList:
						return vector2IntListValue;
					case Type.Vector3IntList:
						return vector3IntListValue;
					case Type.RectIntList:
						return rectIntListValue;
					case Type.BoundsIntList:
						return boundsIntListValue;
					case Type.GameObjectList:
						return gameObjectListValue;
					case Type.ComponentList:
						return Internal_GetComponentList();
					case Type.AssetObjectList:
						return Internal_GetAssetObjectList();
					case Type.VariableList:
						return variableListValue;
				}

				throw new System.NotImplementedException("It is an unimplemented Parameter type(" + type + ")");
			}
			set
			{
				try
				{
					switch (type)
					{
						case Type.Int:
							intValue = (int)value;
							break;
						case Type.Long:
							longValue = (long)value;
							break;
						case Type.Float:
							floatValue = (float)value;
							break;
						case Type.Bool:
							boolValue = (bool)value;
							break;
						case Type.GameObject:
							gameObjectValue = (GameObject)value;
							break;
						case Type.String:
							stringValue = (string)value;
							break;
						case Type.Enum:
							if (value != null)
							{
								System.Type valueType = value.GetType();
								if (EnumFieldUtility.IsEnum(valueType))
								{
									enumValue = (System.Enum)value;
								}
								else if (valueType == typeof(int))
								{
									enumIntValue = System.Convert.ToInt32(value);
								}
							}
							break;
						case Type.Vector2:
							vector2Value = (Vector2)value;
							break;
						case Type.Vector3:
							vector3Value = (Vector3)value;
							break;
						case Type.Quaternion:
							quaternionValue = (Quaternion)value;
							break;
						case Type.Rect:
							rectValue = (Rect)value;
							break;
						case Type.Bounds:
							boundsValue = (Bounds)value;
							break;
						case Type.Color:
							colorValue = (Color)value;
							break;
						case Type.Vector4:
							vector4Value = (Vector4)value;
							break;
						case Type.Vector2Int:
							vector2IntValue = (Vector2Int)value;
							break;
						case Type.Vector3Int:
							vector3IntValue = (Vector3Int)value;
							break;
						case Type.RectInt:
							rectIntValue = (RectInt)value;
							break;
						case Type.BoundsInt:
							boundsIntValue = (BoundsInt)value;
							break;
						case Type.Transform:
							transformValue = (Transform)value;
							break;
						case Type.RectTransform:
							rectTransformValue = (RectTransform)value;
							break;
						case Type.Rigidbody:
							rigidbodyValue = (Rigidbody)value;
							break;
						case Type.Rigidbody2D:
							rigidbody2DValue = (Rigidbody2D)value;
							break;
						case Type.Component:
							componentValue = (Component)value;
							break;
						case Type.AssetObject:
							objectReferenceValue = (Object)value;
							break;
						case Type.Variable:
							variableValue = value;
							break;
						case Type.IntList:
							intListValue = (IList<int>)value;
							break;
						case Type.LongList:
							longListValue = (IList<long>)value;
							break;
						case Type.FloatList:
							floatListValue = (IList<float>)value;
							break;
						case Type.BoolList:
							boolListValue = (IList<bool>)value;
							break;
						case Type.StringList:
							stringListValue = (IList<string>)value;
							break;
						case Type.EnumList:
							Internal_SetEnumList((IList)value, true);
							break;
						case Type.Vector2List:
							vector2ListValue = (IList<Vector2>)value;
							break;
						case Type.Vector3List:
							vector3ListValue = (IList<Vector3>)value;
							break;
						case Type.QuaternionList:
							quaternionListValue = (IList<Quaternion>)value;
							break;
						case Type.RectList:
							rectListValue = (IList<Rect>)value;
							break;
						case Type.BoundsList:
							boundsListValue = (IList<Bounds>)value;
							break;
						case Type.ColorList:
							colorListValue = (IList<Color>)value;
							break;
						case Type.Vector4List:
							vector4ListValue = (IList<Vector4>)value;
							break;
						case Type.Vector2IntList:
							vector2IntListValue = (IList<Vector2Int>)value;
							break;
						case Type.Vector3IntList:
							vector3IntListValue = (IList<Vector3Int>)value;
							break;
						case Type.RectIntList:
							rectIntListValue = (IList<RectInt>)value;
							break;
						case Type.BoundsIntList:
							boundsIntListValue = (IList<BoundsInt>)value;
							break;
						case Type.GameObjectList:
							gameObjectListValue = (IList<GameObject>)value;
							break;
						case Type.ComponentList:
							Internal_SetComponentList((IList)value, true);
							break;
						case Type.AssetObjectList:
							Internal_SetAssetObjectList((IList)value, true);
							break;
						case Type.VariableList:
							variableListValue = value;
							break;
						default:
							throw new System.NotImplementedException("It is an unimplemented Parameter type(" + type + ")");
					}
				}
				catch (System.InvalidCastException ex)
				{
					Debug.LogException(ex);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値が変更された際に呼び出されるコールバック関数。
		/// </summary>
#else
		/// <summary>
		/// Callback function to be called when the value is changed.
		/// </summary>
#endif
		public event DelegateOnChanged onChanged;

		internal void DoChanged()
		{
			onChanged?.Invoke(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を変更した際に呼び出す。
		/// </summary>
#else
		/// <summary>
		/// Call when you change the value.
		/// </summary>
#endif
		[System.Obsolete("There is no need to call it.")]
		public void OnChanged()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を文字列形式に変換する。
		/// </summary>
		/// <returns>変換した文字列</returns>
#else
		/// <summary>
		/// Convert value to string format.
		/// </summary>
		/// <returns>Converted string</returns>
#endif
		public override string ToString()
		{
			return System.Convert.ToString(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を文字列形式に変換する。
		/// </summary>
		/// <param name="format">数値書式指定フォーマット(Int,Long,Floatのみ)</param>
		/// <returns>変換した文字列</returns>
		/// <remarks>数値書式指定フォーマットの詳細については、次を参照してください。<a href="https://msdn.microsoft.com/ja-jp/library/dwhawy9k(v=vs.110).aspx">標準の数値書式指定文字列</a>、<a href="https://msdn.microsoft.com/ja-jp/library/0c899ak8(v=vs.110).aspx">カスタム数値書式指定文字列</a></remarks>
#else
		/// <summary>
		/// Convert value to string format.
		/// </summary>
		/// <param name="format">Numeric format string (Int, Long, Float only)</param>
		/// <returns>Converted string</returns>
		/// <remarks>For more information about numeric format specifiers, see <a href="https://msdn.microsoft.com/en-us/library/dwhawy9k(v=vs.110).aspx">Standard Numeric Format Strings</a> and <a href="https://msdn.microsoft.com/en-us/library/0c899ak8(v=vs.110).aspx">Custom Numeric Format Strings</a>.</remarks>
#endif
		public string ToString(string format)
		{
			string s;

			switch (type)
			{
				case Parameter.Type.Int:
					s = intValue.ToString(format);
					break;
				case Parameter.Type.Long:
					s = longValue.ToString(format);
					break;
				case Parameter.Type.Float:
					s = floatValue.ToString(format);
					break;
				case Parameter.Type.String:
					s = stringValue;
					break;
				default:
					s = System.Convert.ToString(value);
					break;
			}

			return s;
		}

		internal void ChangeContainer(ParameterContainerInternal container)
		{
			if (!Application.isEditor || Application.isPlaying)
			{
				throw new System.NotSupportedException();
			}

			this.container = container;

			switch (type)
			{
				case Type.Variable:
					{
						VariableBase sourceVariable = Internal_GetObject() as VariableBase;
						Internal_SetObject(null);

						ComponentUtility.MoveVariable(this, sourceVariable);
					}
					break;
				case Type.VariableList:
					{
						VariableListBase sourceVariable = Internal_GetObject() as VariableListBase;
						Internal_SetObject(null);

						ComponentUtility.MoveVariableList(this, sourceVariable);
					}
					break;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値を設定する。
		/// </summary>
		/// <typeparam name="T">設定する値の型</typeparam>
		/// <param name="value">設定する値</param>
		/// <returns>設定に成功した場合にtrueを返す</returns>
#else
		/// <summary>
		/// Set the value of the parameter.
		/// </summary>
		/// <typeparam name="T">Type of value to set</typeparam>
		/// <param name="value">Value to set</param>
		/// <returns>Returns true if the setting is successful</returns>
#endif
		public bool SetValue<T>(T value)
		{
			var accessor = ParameterAccessor.GetAccessor(type);

			var genericAccessor = accessor as IParameterAccessor<T>;
			if (genericAccessor != null)
			{
				return genericAccessor.SetValue(this, value);
			}
			else if (accessor != null)
			{
				return accessor.SetValue(this, value);
			}

			switch (type)
			{
				case Type.Variable:
					return SetVariable<T>(value);
			}

			this.value = value;
			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値を設定する。
		/// </summary>
		/// <typeparam name="T">設定する値の型</typeparam>
		/// <param name="valueGetter">設定する値を取得してくるインターフェイス</param>
		/// <returns>設定に成功した場合にtrueを返す</returns>
#else
		/// <summary>
		/// Set the value of the parameter.
		/// </summary>
		/// <typeparam name="T">Type of value to set</typeparam>
		/// <param name="valueGetter">Interface to get the value to set</param>
		/// <returns>Returns true if the setting is successful</returns>
#endif
		public bool SetValue<T>(IValueGetter valueGetter)
		{
			T value = default(T);

			IValueGetter<T> container = valueGetter as IValueGetter<T>;
			if (container != null)
			{
				value = container.GetValue();
			}
			else if (!valueGetter.TryGetValue<T>(out value))
			{
				return false;
			}

			return SetValue<T>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値を設定する。
		/// </summary>
		/// <param name="valueGetter">設定する値を取得してくるインターフェイス</param>
		/// <returns>設定に成功した場合にtrueを返す</returns>
#else
		/// <summary>
		/// Set the value of the parameter.
		/// </summary>
		/// <param name="valueGetter">Interface to get the value to set</param>
		/// <returns>Returns true if the setting is successful</returns>
#endif
		public bool SetValue(IValueGetter valueGetter)
		{
			switch (type)
			{
				case Type.Int:
					if (SetValue<int>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Bool:
					if (SetValue<bool>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Float:
					if (SetValue<float>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Long:
					if (SetValue<long>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Enum:
					if (SetValue<int>(valueGetter))
					{
						return true;
					}
					if (SetValue<System.Enum>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Vector2:
					if (SetValue<Vector2>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Vector3:
					if (SetValue<Vector3>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Vector4:
					if (SetValue<Vector4>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Quaternion:
					if (SetValue<Quaternion>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Rect:
					if (SetValue<Rect>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Bounds:
					if (SetValue<Bounds>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Color:
					if (SetValue<Color>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Vector2Int:
					if (SetValue<Vector2Int>(valueGetter))
					{
						return true;
					}
					break;
				case Type.Vector3Int:
					if (SetValue<Vector3Int>(valueGetter))
					{
						return true;
					}
					break;
				case Type.RectInt:
					if (SetValue<RectInt>(valueGetter))
					{
						return true;
					}
					break;
				case Type.BoundsInt:
					if (SetValue<BoundsInt>(valueGetter))
					{
						return true;
					}
					break;
			}

			object value = valueGetter.GetValueObject();

			var valueType = this.valueType;

			if (TypeUtility.IsValueType(valueType) && value == null)
			{
				return false;
			}

			if (value != null)
			{
				System.Type type = value.GetType();
				if (type != valueType)
				{
					value = DynamicReflection.DynamicUtility.Cast(value, valueType);
				}
			}

			this.value = value;

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値の取得を試みる。
		/// </summary>
		/// <typeparam name="T">取得する値の型</typeparam>
		/// <param name="outValue">値の出力引数</param>
		/// <returns>取得に成功した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Try to get the value of the parameter.
		/// </summary>
		/// <typeparam name="T">Type of value to get</typeparam>
		/// <param name="outValue">Value output argument</param>
		/// <returns>Returns true if the get is successful.</returns>
#endif
		public bool TryGetValue<T>(out T outValue)
		{
			var accessor = ParameterAccessor.GetAccessor(type);

			var genericAccessor = accessor as IParameterAccessor<T>;
			if (genericAccessor != null)
			{
				return genericAccessor.TryGetValue(this, out outValue);
			}
			else if (accessor != null)
			{
				object value;
				if (accessor.TryGetValue(this, out value))
				{
					outValue = (T)value;
					return true;
				}
				outValue = default(T);
				return false;
			}

			switch (type)
			{
				case Type.Variable:
					return TryGetVariable<T>(out outValue);
			}

			outValue = (T)this.value;
			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する。
		/// </summary>
		/// <typeparam name="T">値の型。</typeparam>
		/// <returns>取得した値。取得できなかった場合はdefault(T)を返す。</returns>
#else
		/// <summary>
		/// Get the value.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <returns>Get the value. If you can not get returns the default (T).</returns>
#endif
		public T GetValue<T>()
		{
			T value;
			if (TryGetValue<T>(out value))
			{
				return value;
			}
			return default(T);
		}

		internal event System.Action onAfterDeserialize;
		internal event System.Action onDestroy;

		internal void OnDestroy()
		{
			onDestroy?.Invoke();
			onDestroy = null;
			
			if (type == Type.Variable || type == Type.VariableList)
			{
				Object variableObj = Internal_GetObject();
				if (variableObj != null)
				{
					ComponentUtility.Destroy(variableObj);
				}
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			onAfterDeserialize?.Invoke();
		}
	}
}
