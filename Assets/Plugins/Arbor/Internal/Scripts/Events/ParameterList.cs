//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Events
{
#if ARBOR_DOC_JA
	/// <summary>
	/// データーフローをサポートするパラメータリスト。
	/// </summary>
#else
	/// <summary>
	/// Parameter list that supports data flow.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class ParameterList
	{
		[SerializeField]
		internal List<FlexibleInt> _IntParameters = new List<FlexibleInt>();

		[SerializeField]
		internal List<FlexibleLong> _LongParameters = new List<FlexibleLong>();

		[SerializeField]
		internal List<FlexibleFloat> _FloatParameters = new List<FlexibleFloat>();

		[SerializeField]
		internal List<FlexibleBool> _BoolParameters = new List<FlexibleBool>();

		[SerializeField]
		internal List<FlexibleString> _StringParameters = new List<FlexibleString>();

		[SerializeField]
		internal List<FlexibleVector2> _Vector2Parameters = new List<FlexibleVector2>();

		[SerializeField]
		internal List<FlexibleVector3> _Vector3Parameters = new List<FlexibleVector3>();

		[SerializeField]
		internal List<FlexibleQuaternion> _QuaternionParameters = new List<FlexibleQuaternion>();

		[SerializeField]
		internal List<FlexibleRect> _RectParameters = new List<FlexibleRect>();

		[SerializeField]
		internal List<FlexibleBounds> _BoundsParameters = new List<FlexibleBounds>();

		[SerializeField]
		internal List<FlexibleColor> _ColorParameters = new List<FlexibleColor>();

		[SerializeField]
		internal List<FlexibleVector4> _Vector4Parameters = new List<FlexibleVector4>();

		[SerializeField]
		internal List<FlexibleVector2Int> _Vector2IntParameters = new List<FlexibleVector2Int>();

		[SerializeField]
		internal List<FlexibleVector3Int> _Vector3IntParameters = new List<FlexibleVector3Int>();

		[SerializeField]
		internal List<FlexibleRectInt> _RectIntParameters = new List<FlexibleRectInt>();

		[SerializeField]
		internal List<FlexibleBoundsInt> _BoundsIntParameters = new List<FlexibleBoundsInt>();

		[SerializeField]
		internal List<FlexibleGameObject> _GameObjectParameters = new List<FlexibleGameObject>();

		[SerializeField]
		internal List<FlexibleComponent> _ComponentParameters = new List<FlexibleComponent>();

		[SerializeField]
		internal List<FlexibleEnumAny> _EnumParameters = new List<FlexibleEnumAny>();

		[SerializeField]
		internal List<FlexibleAssetObject> _AssetObjectParameters = new List<FlexibleAssetObject>();

		[SerializeField]
		[HideSlotFields]
		internal List<InputSlotTypable> _InputSlotParameters = new List<InputSlotTypable>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を追加する。
		/// </summary>
		/// <param name="valueType">値の型</param>
		/// <param name="slot">設定する入力スロット</param>
		/// <returns>追加したパラメータのタイプ</returns>
#else
		/// <summary>
		/// Add an element.
		/// </summary>
		/// <param name="valueType">Value type</param>
		/// <param name="slot">Input slot to set</param>
		/// <returns>Type of parameter added</returns>
#endif
		public ParameterType AddElement(System.Type valueType, InputSlotBase slot)
		{
			bool isValidSlot = slot != null && (slot.nodeGraph != null && slot.branchID != 0);

			ParameterType parameterType = ArborEventUtility.GetParameterType(valueType, true);

			switch (parameterType)
			{
				case ParameterType.Int:
					{
						FlexibleInt flexibleInt = new FlexibleInt();
						if (isValidSlot)
						{
							flexibleInt.SetSlot(slot);
						}
						_IntParameters.Add(flexibleInt);
					}
					break;
				case ParameterType.Long:
					{
						FlexibleLong flexibleLong = new FlexibleLong();
						if (isValidSlot)
						{
							flexibleLong.SetSlot(slot);
						}
						_LongParameters.Add(flexibleLong);
					}
					break;
				case ParameterType.Float:
					{
						FlexibleFloat flexibleFloat = new FlexibleFloat();
						if (isValidSlot)
						{
							flexibleFloat.SetSlot(slot);
						}

						_FloatParameters.Add(flexibleFloat);
					}
					break;
				case ParameterType.Bool:
					{
						FlexibleBool flexibleBool = new FlexibleBool();
						if (isValidSlot)
						{
							flexibleBool.SetSlot(slot);
						}

						_BoolParameters.Add(flexibleBool);
					}
					break;
				case ParameterType.String:
					{
						FlexibleString flexibleString = new FlexibleString();
						if (isValidSlot)
						{
							flexibleString.SetSlot(slot);
						}

						_StringParameters.Add(flexibleString);
					}
					break;
				case ParameterType.Vector2:
					{
						FlexibleVector2 flexibleVector2 = new FlexibleVector2();
						if (isValidSlot)
						{
							flexibleVector2.SetSlot(slot);
						}
						_Vector2Parameters.Add(flexibleVector2);
					}
					break;
				case ParameterType.Vector3:
					{
						FlexibleVector3 flexibleVector3 = new FlexibleVector3();
						if (isValidSlot)
						{
							flexibleVector3.SetSlot(slot);
						}
						_Vector3Parameters.Add(flexibleVector3);
					}
					break;
				case ParameterType.Quaternion:
					{
						FlexibleQuaternion flexibleQuaternion = new FlexibleQuaternion();
						if (isValidSlot)
						{
							flexibleQuaternion.SetSlot(slot);
						}
						_QuaternionParameters.Add(flexibleQuaternion);
					}
					break;
				case ParameterType.Rect:
					{
						FlexibleRect flexibleRect = new FlexibleRect();
						if (isValidSlot)
						{
							flexibleRect.SetSlot(slot);
						}
						_RectParameters.Add(flexibleRect);
					}
					break;
				case ParameterType.Bounds:
					{
						FlexibleBounds flexibleBounds = new FlexibleBounds();
						if (isValidSlot)
						{
							flexibleBounds.SetSlot(slot);
						}
						_BoundsParameters.Add(flexibleBounds);
					}
					break;
				case ParameterType.Color:
					{
						FlexibleColor flexibleColor = new FlexibleColor();
						if (isValidSlot)
						{
							flexibleColor.SetSlot(slot);
						}
						_ColorParameters.Add(flexibleColor);
					}
					break;
				case ParameterType.Vector4:
					{
						FlexibleVector4 flexibleVector4 = new FlexibleVector4();
						if (isValidSlot)
						{
							flexibleVector4.SetSlot(slot);
						}
						_Vector4Parameters.Add(flexibleVector4);
					}
					break;
				case ParameterType.Vector2Int:
					{
						FlexibleVector2Int flexibleVector2Int = new FlexibleVector2Int();
						if (isValidSlot)
						{
							flexibleVector2Int.SetSlot(slot);
						}
						_Vector2IntParameters.Add(flexibleVector2Int);
					}
					break;
				case ParameterType.Vector3Int:
					{
						FlexibleVector3Int flexibleVector3Int = new FlexibleVector3Int();
						if (isValidSlot)
						{
							flexibleVector3Int.SetSlot(slot);
						}
						_Vector3IntParameters.Add(flexibleVector3Int);
					}
					break;
				case ParameterType.RectInt:
					{
						FlexibleRectInt flexibleRectInt = new FlexibleRectInt();
						if (isValidSlot)
						{
							flexibleRectInt.SetSlot(slot);
						}
						_RectIntParameters.Add(flexibleRectInt);
					}
					break;
				case ParameterType.BoundsInt:
					{
						FlexibleBoundsInt flexibleBoundsInt = new FlexibleBoundsInt();
						if (isValidSlot)
						{
							flexibleBoundsInt.SetSlot(slot);
						}
						_BoundsIntParameters.Add(flexibleBoundsInt);
					}
					break;
				case ParameterType.GameObject:
					{
						FlexibleGameObject flexibleGameObject = new FlexibleGameObject();
						if (isValidSlot)
						{
							flexibleGameObject.SetSlot(slot);
						}
						_GameObjectParameters.Add(flexibleGameObject);
					}
					break;
				case ParameterType.Component:
					{
						FlexibleComponent flexibleComponent = new FlexibleComponent();
						if (isValidSlot)
						{
							flexibleComponent.SetSlot(slot);
						}
						_ComponentParameters.Add(flexibleComponent);
					}
					break;
				case ParameterType.Enum:
					{
						FlexibleEnumAny flexibleEnumAny = new FlexibleEnumAny();
						if (isValidSlot)
						{
							flexibleEnumAny.SetSlot(slot);
						}
						_EnumParameters.Add(flexibleEnumAny);
					}
					break;
				case ParameterType.AssetObject:
					{
						FlexibleAssetObject flexibleAssetObject = new FlexibleAssetObject();
						if (isValidSlot)
						{
							flexibleAssetObject.SetSlot(slot);
						}
						_AssetObjectParameters.Add(flexibleAssetObject);
					}
					break;
				case ParameterType.Slot:
					{
						InputSlotTypable inputSlotTypable = new InputSlotTypable(valueType);
						if (isValidSlot)
						{
							inputSlotTypable.Copy(slot);
						}
						_InputSlotParameters.Add(inputSlotTypable);
					}
					break;
			}

			return parameterType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したparameterTypeのすべての要素に対して型制約を上書きする
		/// </summary>
		/// <param name="parameterType">パラメータのタイプ</param>
		/// <param name="type">制約する型</param>
#else
		/// <summary>
		/// Override type constraints on all elements of the specified parameterType
		/// </summary>
		/// <param name="parameterType">Parameter type</param>
		/// <param name="type">Constraint type</param>
#endif
		public void SetOverrideConstraint(ParameterType parameterType, System.Type type)
		{
			switch (parameterType)
			{
				case ParameterType.Component:
				case ParameterType.Enum:
				case ParameterType.AssetObject:
					{
						var parameterList = GetParameterList(parameterType);
						if (parameterList != null && parameterList.Count > 0)
						{
							var typeConstraint = type != null ? new ClassConstraintInfo() { baseType = type } : null;
							for (int i = 0; i < parameterList.Count; i++)
							{
								var flexibleField = parameterList[i] as IOverrideConstraint;
								flexibleField.overrideConstraint = typeConstraint;
							}
						}
					}
					break;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータのリストを取得する。
		/// </summary>
		/// <param name="parameterType">パラメータのタイプ</param>
		/// <returns>パラメータのリスト</returns>
#else
		/// <summary>
		/// Get a list of parameters.
		/// </summary>
		/// <param name="parameterType">Parameter type</param>
		/// <returns>List of parameters</returns>
#endif
		public IList GetParameterList(ParameterType parameterType)
		{
			switch (parameterType)
			{
				case ParameterType.Int:
					return _IntParameters;
				case ParameterType.Long:
					return _LongParameters;
				case ParameterType.Float:
					return _FloatParameters;
				case ParameterType.Bool:
					return _BoolParameters;
				case ParameterType.String:
					return _StringParameters;
				case ParameterType.Vector2:
					return _Vector2Parameters;
				case ParameterType.Vector3:
					return _Vector3Parameters;
				case ParameterType.Quaternion:
					return _QuaternionParameters;
				case ParameterType.Rect:
					return _RectParameters;
				case ParameterType.Bounds:
					return _BoundsParameters;
				case ParameterType.Color:
					return _ColorParameters;
				case ParameterType.Vector4:
					return _Vector4Parameters;
				case ParameterType.Vector2Int:
					return _Vector2IntParameters;
				case ParameterType.Vector3Int:
					return _Vector3IntParameters;
				case ParameterType.RectInt:
					return _RectIntParameters;
				case ParameterType.BoundsInt:
					return _BoundsIntParameters;
				case ParameterType.GameObject:
					return _GameObjectParameters;
				case ParameterType.Component:
					return _ComponentParameters;
				case ParameterType.Enum:
					return _EnumParameters;
				case ParameterType.AssetObject:
					return _AssetObjectParameters;
				case ParameterType.Slot:
					return _InputSlotParameters;
			}

			return null;
		}
	}
}