using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Utils.ECS;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public static class AISolver
	{
		public static bool Solve(Point start, Point goal, AIState initialState, out Point[] path)
		{
			AINode startNode = new()
			{
				point = start,
				previousNode = null,
				currentValue = 0,
				state = initialState,
			};
			
			List<AINode> nodesToExplore = new();
			nodesToExplore.AddRange(startNode.ConnectedNodes);

			int numIterations = 0;
			AINode solution = null;
			while (nodesToExplore.Count > 0)
			{
				const int iterationCap = 1_000_000;
				if (numIterations++ >= iterationCap)
				{
					Debug.LogError($"Solver capped at {iterationCap} iterations!");
					break;
				}
				
				AINode node = nodesToExplore.Max();
				nodesToExplore.Remove(node);
				
				Debug.Log($"{node.previousNode} ==> {node} (best is {(solution == null ? "null" : solution)})");
				
				const float sunkCostBreakOff = 5;
				if (solution != null && node.currentValue < solution.currentValue - sunkCostBreakOff)
					break;
				
				if (node.point == goal && (solution == null || solution.currentValue < node.currentValue))
					solution = node;
				
				nodesToExplore.AddRange(node.ConnectedNodes);
			}
			
			if (solution != null)
			{
				path = solution.PathBack.Reverse().ToArray();
				return true;
			}
			else
			{
				path = null;
				return false;
			}
		}
	}
	
	public class AINode : IComparable<AINode>
	{
		public Point point;
		public AINode previousNode;
		public float currentValue;
		public AIState state;
		
		public IEnumerable<AINode> ConnectedNodes => point.GetConnectedNodes(this);
		
		public IEnumerable<Point> PathBack
		{
			get
			{
				yield return point;
				
				AINode iterNode = previousNode;
				while (iterNode != null)
				{
					yield return iterNode.point;
					iterNode = iterNode.previousNode;
				}
			}
		}
		
		public int CompareTo(AINode other) => currentValue.CompareTo(other.currentValue);
		
		public override string ToString() => $"{point}{{{currentValue:0.##} value, {state}}}";
	}
	
	public class AIState : ECSEntity<AIStateComponent>
	{
		public float Value => this.Select(component => component.GetValue()).Sum();
		
		public AIState() { }
		public AIState(Dictionary<Type, AIStateComponent> components) : base(components) { }
		
		public AIState StepState() => new(Components.Select(pair => (pair.Key, Value: pair.Value.StepState())).ToDictionary(pair => pair.Key, pair => pair.Value));
	}
	
	public interface AIStateComponent
	{
		public float GetValue();
		public AIStateComponent StepState();
	}
	
	public class Point
	{
		private string name;
		public AIPointState state;
		private List<Edge> connectedPoints = new();
		
		public Point(string name, AIPointState state)
		{
			this.name = name;
			this.state = state;
		}
		
		public IEnumerable<AINode> GetConnectedNodes(AINode origin) => connectedPoints.Select(edge =>
		{
			AIState connectedState = edge.stateModifiers.ProcessOn(origin.state.StepState());
			return new AINode
			{
				currentValue = origin.currentValue + connectedState.Value + edge.valueCalculators.Select(calc => calc(connectedState)).Sum(),
				point = edge.destination,
				previousNode = origin,
				state = connectedState,
			};
		});
		
		public void Connect(Point point, float value) => Connect(point, Array.Empty<Action<AIState>>(), new Func<AIState, float>[]{ _ => value });
		public void Connect<TStateComponent>(Point point, Func<TStateComponent, TStateComponent> stateModifier, Func<TStateComponent, float> valueCalculator) where TStateComponent : struct, AIStateComponent => Connect(point, state => state.Set(stateModifier(state.Get<TStateComponent>())), state => valueCalculator(state.Get<TStateComponent>()));
		public void Connect(Point point, Action<AIState> stateModifier, Func<AIState, float> valueCalculator) => Connect(point, new[]{ stateModifier }, new[]{ valueCalculator });
		public void Connect(Point point, Action<AIState>[] stateModifiers, Func<AIState, float>[] valueCalculators) => connectedPoints.Add(new Edge
		{
			destination = point,
			stateModifiers = stateModifiers,
			valueCalculators = valueCalculators,
		});
		
		public override string ToString() => name;
		
		private struct Edge
		{
			public Point destination;
			public Action<AIState>[] stateModifiers;
			public Func<AIState, float>[] valueCalculators;
		}
	}
	
	public class AIPointState : ECSEntity<AIPointComponent>
	{
		
	}
	
	public interface AIPointComponent
	{
		
	}
}
