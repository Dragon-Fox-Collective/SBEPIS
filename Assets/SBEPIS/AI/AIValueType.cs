using System;
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
		private Dictionary<AIValueType, float> costs = new();
		
		public void AddCost(AIValueCost cost) => AddCost(cost.ValueType, cost.Cost);
		public void AddCost(AIValueType valueType, float cost)
		{
			if (costs.ContainsKey(valueType))
				costs[valueType] += cost;
			else
				costs.Add(valueType, cost);
		}
		
		public void AddCosts(AIValueTotalCost cost)
		{
			cost.costs.ForEach(pair => AddCost(pair.Key, pair.Value));
		}
		
		public void Clear()
		{
			costs.Clear();
		}
		
		public IEnumerator<AIValueType> GetEnumerator() => costs.Keys.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public float this[AIValueType type] => costs[type];
		
		public override string ToString() => costs.ToDelimString();
	}
	
	[Serializable]
	public struct AIValueCost
	{
		[SerializeField] private AIValueType valueType;
		public AIValueType ValueType => valueType;
		[SerializeField] private float cost;
		public float Cost => cost;
	}
}
