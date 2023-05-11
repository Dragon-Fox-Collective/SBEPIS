//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	[System.Serializable]
	public sealed class CalculatorConditionList : ISerializationCallbackReceiver
	{
		[System.Serializable]
		public sealed class IntParameter
		{
			[SerializeField]
			public FlexibleInt value1 = new FlexibleInt();

			[SerializeField]
			public FlexibleInt value2 = new FlexibleInt();

			public bool Compare(CompareType compareType)
			{
				return CompareUtility.Compare(value1.value, value2.value, compareType);
			}
		}

		[System.Serializable]
		public sealed class FloatParameter
		{
			[SerializeField]
			public FlexibleFloat value1 = new FlexibleFloat();

			[SerializeField]
			public FlexibleFloat value2 = new FlexibleFloat();

			public bool Compare(CompareType compareType)
			{
				return CompareUtility.Compare(value1.value, value2.value, compareType);
			}
		}

		[System.Serializable]
		public sealed class BoolParameter
		{
			[SerializeField]
			public FlexibleBool value1 = new FlexibleBool(false);

			[SerializeField]
			public FlexibleBool value2 = new FlexibleBool(true);

			public bool Compare()
			{
				return (value1.value == value2.value);
			}
		}

		[SerializeField]
		internal List<CalculatorCondition> _Conditions = new List<CalculatorCondition>();

		[SerializeField]
		internal List<IntParameter> _IntParameters = new List<IntParameter>();

		[SerializeField]
		internal List<FloatParameter> _FloatParameters = new List<FloatParameter>();

		[SerializeField]
		internal List<BoolParameter> _BoolParameters = new List<BoolParameter>();

		internal bool Compare(CalculatorCondition.Type type, int parameterIndex, CompareType compareType)
		{
			switch (type)
			{
				case CalculatorCondition.Type.Int:
					IntParameter intParameter = _IntParameters[parameterIndex];
					return intParameter.Compare(compareType);
				case CalculatorCondition.Type.Float:
					FloatParameter floatParameter = _FloatParameters[parameterIndex];
					return floatParameter.Compare(compareType);
				case CalculatorCondition.Type.Bool:
					BoolParameter boolParameter = _BoolParameters[parameterIndex];
					return boolParameter.Compare();
			}

			return false;
		}

		public void ImportLegacy(List<CalculatorConditionLegacy> legacyList)
		{
			for (int index = 0; index < legacyList.Count; index++)
			{
				CalculatorConditionLegacy legacy = legacyList[index];
				var condition = new CalculatorCondition(legacy._Type);
				condition._CompareType = legacy._CompareType;

				CalculatorCondition.Type type = condition._Type;

				switch (type)
				{
					case CalculatorCondition.Type.Int:
						IntParameter intParameter = new IntParameter();
						intParameter.value1 = legacy._IntValue1;
						intParameter.value2 = legacy._IntValue2;
						_IntParameters.Add(intParameter);
						condition._ParameterIndex = _IntParameters.Count - 1;
						break;
					case CalculatorCondition.Type.Float:
						FloatParameter floatParameter = new FloatParameter();
						floatParameter.value1 = legacy._FloatValue1;
						floatParameter.value2 = legacy._FloatValue2;
						_FloatParameters.Add(floatParameter);
						condition._ParameterIndex = _FloatParameters.Count - 1;
						break;
					case CalculatorCondition.Type.Bool:
						BoolParameter boolParameter = new BoolParameter();
						boolParameter.value1 = legacy._BoolValue1;
						boolParameter.value2 = legacy._BoolValue2;
						_BoolParameters.Add(boolParameter);
						condition._ParameterIndex = _BoolParameters.Count - 1;
						break;
				}

				condition.owner = this;

				_Conditions.Add(condition);
			}
		}

		public bool CheckCondition()
		{
			int conditionCount = _Conditions.Count;
			for (int conditionIndex = 0; conditionIndex < conditionCount; conditionIndex++)
			{
				CalculatorCondition condition = _Conditions[conditionIndex];
				condition.ClearConditionResult();
			}

			bool first = true;
			bool result = false;
			bool skipAnd = false;
			
			for (int conditionIndex = 0; conditionIndex < conditionCount; conditionIndex++)
			{
				CalculatorCondition condition = _Conditions[conditionIndex];
				if (first)
				{
					first = false;
					result = condition.Compare();
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
							result &= condition.Compare();
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
							result |= condition.Compare();
							break;
					}
				}
			}

			return result;
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			for (int i = 0, count = _Conditions.Count; i < count; i++)
			{
				CalculatorCondition condition = _Conditions[i];
				condition.owner = this;
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}