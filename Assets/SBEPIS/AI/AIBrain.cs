using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace SBEPIS.AI
{
	public class AIBrain : MonoBehaviour
	{
		[SerializeField] private AIAction action;
		
		[Label("Static Values (value per unit)")]
		[SerializeField] private List<AIStaticValue> staticValues = new();
		[Label("Dynamic Values (dependent unit per main unit)")]
		[SerializeField] private List<AIDynamicValue.AIDynamicValueSetup> dynamicValues = new();
		
		[SerializeField] private List<AIAction> solverActions = new();

		private Dictionary<AIValueType, AIValue> values;
		
		private void Awake()
		{
			values = staticValues.Cast<AIValue>().ToDictionary(value => value.ValueType);
			values = values.Values.Concat(dynamicValues.Select(value => value.Setup(values)).Cast<AIValue>()).ToDictionary(value => value.ValueType);
		}
		
		private void Start()
		{
			DoAction(action);
		}
		
		public void DoAction(AIAction action)
		{
			bool canComplete = action.CanComplete(solverActions, out AIValueTotalCost costs);
			float totalValue = GetTotalValue(costs);
			Debug.Log($"Doing action on {name}, can complete {canComplete}, costs {costs}, value {totalValue}");
		}
		
		public float GetTotalValue(AIValueTotalCost costs) => values.Keys.Intersect(costs).Select(valueType => valueType.InterpretValue(costs[valueType], values[valueType].Value)).Sum();
	}
	
	public interface AIValue
	{
		public AIValueType ValueType { get; }
		public float Value { get; }
	}
	
	[Serializable]
	public struct AIStaticValue : AIValue
	{
		[SerializeField] private AIValueType valueType;
		public AIValueType ValueType => valueType;
		[SerializeField] private float value;
		public float Value => value;
	}
	
	public struct AIDynamicValue : AIValue
	{
		public AIValueType ValueType { get; private set; }
		private List<(AIValue, float)> dependentValues;
		
		public float Value => dependentValues.Sum(zip => zip.Item1.Value * zip.Item2);
		
		[Serializable]
		public struct AIDynamicValueSetup
		{
			[SerializeField] private AIValueType valueType;
			[SerializeField] private List<AIStaticValue> dependentValues;
			
			public AIDynamicValue Setup(Dictionary<AIValueType, AIValue> values) => new()
			{
				ValueType = valueType,
				dependentValues = dependentValues.Select(value => (values[value.ValueType], value.Value)).ToList(),
			};
		}
	}
}
