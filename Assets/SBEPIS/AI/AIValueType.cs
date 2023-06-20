using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	[CreateAssetMenu]
	public class AIValueType : ScriptableObject
	{
		public float InterpretValue(float cost, float value) => cost * value;
	}
	
	public class AIValueTotalCost : IEnumerable<AIValueType>
	{
		public static AIValueTotalCost Zero => new();
		
		private Dictionary<AIValueType, float> costs = new();
		
		private void AddCost(AIValue cost) => AddCost(cost.ValueType, cost.Value);
		private void AddCost(AIValueType valueType, float cost)
		{
			if (costs.ContainsKey(valueType))
				costs[valueType] += cost;
			else
				costs.Add(valueType, cost);
		}
		
		private void AddCosts(AIValueTotalCost cost)
		{
			cost.costs.ForEach(pair => AddCost(pair.Key, pair.Value));
		}
		
		public IEnumerator<AIValueType> GetEnumerator() => costs.Keys.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public float this[AIValueType type] => costs[type];
		
		public static AIValueTotalCost operator +(AIValueTotalCost costs, AIValue cost)
		{
			AIValueTotalCost newCosts = Zero;
			newCosts.AddCosts(costs);
			newCosts.AddCost(cost);
			return newCosts;
		}
		
		public static AIValueTotalCost operator +(AIValueTotalCost costs, AIValueTotalCost cost)
		{
			AIValueTotalCost newCosts = Zero;
			newCosts.AddCosts(costs);
			newCosts.AddCosts(cost);
			return newCosts;
		}
		
		public override string ToString() => costs.ToDelimString();
	}
	
	public static class AIValueCostLINQ
	{
		public static AIValueTotalCost Sum(this IEnumerable<AIValue> costs) => costs.Aggregate(AIValueTotalCost.Zero, (currentCosts, cost) => currentCosts + cost);
		public static AIValueTotalCost Sum(this IEnumerable<AIValueTotalCost> costs) => costs.Aggregate(AIValueTotalCost.Zero, (currentCosts, cost) => currentCosts + cost);
	}
}
