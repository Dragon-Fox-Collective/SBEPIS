using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public class AIBrain : MonoBehaviour
	{
		[SerializeField] private AIAction action;
		
		[Label("Values (value per unit)")]
		[SerializeField] private List<AIValue> values = new();
		
		[SerializeField] private List<AIValuedAction> solverActions = new();
		
		private Dictionary<AIValueType, AIValue> valuesDict;
		
		private void Awake()
		{
			valuesDict = values.ToDictionary(value => value.ValueType);
		}
		
		private void Start()
		{
			DoAction(action);
		}
		
		public void DoAction(AIAction action)
		{
			IEnumerable<AIAction> performedActions = action.GetBestRouteToComplete(values, solverActions, out float totalValue);
			Debug.Log($"Doing {action.name} on {name} via {performedActions.ToDelimString()}, value {totalValue}");
		}
		
		public float GetTotalValue(AIValueTotalCost costs) => valuesDict.Keys.Intersect(costs).Select(valueType => valueType.InterpretValue(costs[valueType], valuesDict[valueType].Value)).Sum();
	}
	
	[Serializable]
	public struct AIValue
	{
		[SerializeField] private AIValueType valueType;
		public AIValueType ValueType => valueType;
		[SerializeField] private float value;
		public float Value => value;
		
		public static AIValue operator *(AIValue value, float multipler) => new(){ valueType = value.valueType, value = value.value * multipler };
	}
	
	[Serializable]
	public struct AICostConversion
	{
		[SerializeField] private AIValueType valueType;
		
		[Label("Conversions (converted unit per main unit)")]
		[SerializeField] private List<AIValue> conversions;
		
		public AIValueTotalCost AttemptConvert(AIValueType valueType, float cost) => valueType != this.valueType ? new AIValueTotalCost() : conversions.Select(conversionCost => conversionCost * cost).Sum();
	}
	
	[Serializable]
	public struct AIValuedAction
	{
		[SerializeField] private AIAction action;
		public AIAction Action => action;
		
		[SerializeField] private List<AICostConversion> conversions;
		
		public AIValueTotalCost Convert(AIValueTotalCost costs)
		{
			List<AICostConversion> conversions = this.conversions;
			return costs.SelectMany(valueType => conversions.Select(conversion => conversion.AttemptConvert(valueType, costs[valueType]))).Sum();
		}
	}
}
