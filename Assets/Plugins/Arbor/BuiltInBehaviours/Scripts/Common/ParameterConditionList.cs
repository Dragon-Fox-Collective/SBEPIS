//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	[System.Serializable]
	public sealed class ParameterConditionList : ISerializationCallbackReceiver
	{
		[SerializeField]
		internal List<ParameterCondition> _Conditions = new List<ParameterCondition>();

		[SerializeField]
		internal List<FlexibleInt> _IntParameters = new List<FlexibleInt>();

		[SerializeField]
		internal List<FlexibleLong> _LongParameters = new List<FlexibleLong>();

		[SerializeField]
		internal List<FlexibleFloat> _FloatParameters = new List<FlexibleFloat>();

		[SerializeField]
		internal List<FlexibleBool> _BoolParameters = new List<FlexibleBool>();

		[SerializeField]
		internal List<FlexibleEnumAny> _EnumParameters = new List<FlexibleEnumAny>();

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
		internal List<FlexibleAssetObject> _AssetObjectParameters = new List<FlexibleAssetObject>();

		public int conditionCount
		{
			get
			{
				return _Conditions.Count;
			}
		}

		public ParameterCondition GetCondition(int index)
		{
			return _Conditions[index];
		}

		public bool CheckCondition()
		{
			int conditionCount = _Conditions.Count;
			for (int conditionIndex = 0; conditionIndex < conditionCount; conditionIndex++)
			{
				ParameterCondition condition = _Conditions[conditionIndex];
				condition.conditionResult = ConditionResult.None;
			}

			bool first = true;
			bool result = false;
			bool skipAnd = false;

			for (int conditionIndex = 0; conditionIndex < conditionCount; conditionIndex++)
			{
				ParameterCondition condition = _Conditions[conditionIndex];
				if (first)
				{
					first = false;
					result = condition.CheckCondition();
				}
				else
				{
					switch (condition._LogicalCondition.logicalOperation)
					{
						case LogicalOperation.And:
							if (!result)
							{
								skipAnd = true;
							}
							if (skipAnd)
							{
								continue;
							}
							result &= condition.CheckCondition();
							break;
						case LogicalOperation.Or:
							if (skipAnd)
							{
								skipAnd = false;
							}
							if (result)
							{
								return true;
							}
							result |= condition.CheckCondition();
							break;
					}
				}
			}

			return result;
		}

		void ImportLegacy(ParameterConditionLegacy legacy)
		{
			ParameterReference parameterReference = legacy._Reference;
			Parameter parameter = parameterReference.parameter;
			Parameter.Type parameterType = parameter != null ? parameter.type : legacy._ParameterType;

			var condition = new ParameterCondition();
			condition._Reference = legacy._Reference;
			condition._ParameterType = parameterType;
			condition._ReferenceType = legacy._ReferenceType;
			condition._CompareType = legacy._CompareType;

			switch (parameterType)
			{
				case Parameter.Type.Int:
					_IntParameters.Add(legacy._IntValue);
					condition._ParameterIndex = _IntParameters.Count - 1;
					break;
				case Parameter.Type.Long:
					_LongParameters.Add(legacy._LongValue);
					condition._ParameterIndex = _LongParameters.Count - 1;
					break;
				case Parameter.Type.Float:
					_FloatParameters.Add(legacy._FloatValue);
					condition._ParameterIndex = _FloatParameters.Count - 1;
					break;
				case Parameter.Type.Bool:
					_BoolParameters.Add(legacy._BoolValue);
					condition._ParameterIndex = _BoolParameters.Count - 1;
					break;
				case Parameter.Type.String:
					_StringParameters.Add(legacy._StringValue);
					condition._ParameterIndex = _StringParameters.Count - 1;
					break;
				case Parameter.Type.Enum:
					_EnumParameters.Add(legacy._EnumValue);
					condition._ParameterIndex = _EnumParameters.Count - 1;
					break;
				case Parameter.Type.GameObject:
					_GameObjectParameters.Add(legacy._GameObjectValue);
					condition._ParameterIndex = _GameObjectParameters.Count - 1;
					break;
				case Parameter.Type.Vector2:
					_Vector2Parameters.Add(legacy._Vector2Value);
					condition._ParameterIndex = _Vector2Parameters.Count - 1;
					break;
				case Parameter.Type.Vector3:
					_Vector3Parameters.Add(legacy._Vector3Value);
					condition._ParameterIndex = _Vector3Parameters.Count - 1;
					break;
				case Parameter.Type.Quaternion:
					_QuaternionParameters.Add(legacy._QuaternionValue);
					condition._ParameterIndex = _QuaternionParameters.Count - 1;
					break;
				case Parameter.Type.Rect:
					_RectParameters.Add(legacy._RectValue);
					condition._ParameterIndex = _RectParameters.Count - 1;
					break;
				case Parameter.Type.Bounds:
					_BoundsParameters.Add(legacy._BoundsValue);
					condition._ParameterIndex = _BoundsParameters.Count - 1;
					break;
				case Parameter.Type.Color:
					_ColorParameters.Add(legacy._ColorValue);
					condition._ParameterIndex = _ColorParameters.Count - 1;
					break;
				case Parameter.Type.Transform:
					_ComponentParameters.Add(legacy._TransformValue.ToFlexibleComponent());
					condition._ParameterIndex = _ComponentParameters.Count - 1;
					break;
				case Parameter.Type.RectTransform:
					_ComponentParameters.Add(legacy._RectTransformValue.ToFlexibleComponent());
					condition._ParameterIndex = _ComponentParameters.Count - 1;
					break;
				case Parameter.Type.Rigidbody:
					_ComponentParameters.Add(legacy._RigidbodyValue.ToFlexibleComponent());
					condition._ParameterIndex = _ComponentParameters.Count - 1;
					break;
				case Parameter.Type.Rigidbody2D:
					_ComponentParameters.Add(legacy._Rigidbody2DValue.ToFlexibleComponent());
					condition._ParameterIndex = _ComponentParameters.Count - 1;
					break;
				case Parameter.Type.Component:
					_ComponentParameters.Add(legacy._ComponentValue);
					condition._ParameterIndex = _ComponentParameters.Count - 1;
					break;
				case Parameter.Type.AssetObject:
					_AssetObjectParameters.Add(legacy._AssetObjectValue);
					condition._ParameterIndex = _AssetObjectParameters.Count - 1;
					break;
				case Parameter.Type.Variable:
				case Parameter.Type.VariableList:
					break;
			}

			condition.owner = this;

			_Conditions.Add(condition);
		}


		internal void ImportLegacy(List<ParameterConditionLegacy> legacyList)
		{
			for (int i = 0; i < legacyList.Count; i++)
			{
				ParameterConditionLegacy legacy = legacyList[i];

				ParameterReference parameterReference = legacy._Reference;
				if (parameterReference.type == ParameterReferenceType.Constant)
				{
					ParameterContainerBase containerBase = parameterReference.container;
					if (containerBase != null)
					{
						ParameterContainerInternal parameterContainer = containerBase.container;

						if (parameterContainer != null)
						{
							if (!parameterContainer.isDeserialized)
							{
								// ParameterContainer has not been deserialized yet
								parameterContainer.onAfterDeserialize += () =>
								{
									ImportLegacy(legacy);
								};
								continue;
							}
						}
					}
				}

				ImportLegacy(legacy);
			}
		}

		public void Destroy()
		{
			for (int i = 0; i < _Conditions.Count; i++)
			{
				_Conditions[i].Destroy();
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			for (int i = 0, count = _Conditions.Count; i < count; i++)
			{
				ParameterCondition condition = _Conditions[i];
				condition.owner = this;

				condition.Constraint();
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}