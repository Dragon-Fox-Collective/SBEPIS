using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Utils.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	public static class AISolver
	{
		public static bool Solve(Point start, Point goal, AIState initialState, out Point[] path)
		{
			ModifyInitialState(initialState);
			
			AINode startNode = new()
			{
				point = start,
				previousNode = null,
				currentValue = 0,
				state = initialState,
			};
			
			List<AINode> nodesToExplore = new();
			nodesToExplore.AddRange(startNode.ConnectedNodes);
			
			AINode solution = null;
			while (nodesToExplore.Count > 0)
			{
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
		
		private static void ModifyInitialState(AIState state)
		{
			state.Add(new StepsState());
		}
		
		private struct StepsState : AIStateComponent
		{
			private int steps;
			
			public float GetValue() => -steps;
			
			public AIStateComponent StepState()
			{
				StepsState nextState = this;
				nextState.steps++;
				return nextState;
			}
			
			public override string ToString() => $"{steps} steps taken";
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
	
	public class AIState : IEnumerable<AIStateComponent>
	{
		private Dictionary<Type, AIStateComponent> stateComponents = new();
		
		public float Value => this.Select(component => component.GetValue()).Sum();
		
		public void Add<TStateComponent>(TStateComponent stateComponent) where TStateComponent : AIStateComponent => stateComponents.Add(typeof(TStateComponent), stateComponent);
		
		public TStateComponent Get<TStateComponent>() => (TStateComponent)stateComponents[typeof(TStateComponent)];
		public void Set<TStateComponent>(TStateComponent component) where TStateComponent : AIStateComponent => stateComponents[typeof(TStateComponent)] = component;
		
		public AIState StepState() => new(){ stateComponents = stateComponents.Select(pair => (pair.Key, Value: pair.Value.StepState())).ToDictionary(pair => pair.Key, pair => pair.Value) };
		
		public IEnumerator<AIStateComponent> GetEnumerator() => stateComponents.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		public override string ToString() => ", ".Join(stateComponents.Values);
	}
	
	public interface AIStateComponent
	{
		public float GetValue();
		public AIStateComponent StepState();
	}
	
	public class Point
	{
		private string name;
		private Vector2 position;
		private List<Edge> connectedPoints = new();
		
		public Point(string name, float x, float y) : this(name, new Vector2(x, y)) { }
		public Point(string name, Vector2 position)
		{
			this.name = name;
			this.position = position;
		}
		
		public IEnumerable<AINode> GetConnectedNodes(AINode origin) => connectedPoints.Select(edge =>
		{
			AIState state = edge.stateModifiers.ProcessOn(origin.state.StepState());
			return new AINode
			{
				currentValue = origin.currentValue + state.Value + edge.valueCalculators.Select(calc => calc(state)).Sum(),
				point = edge.destination,
				previousNode = origin,
				state = state,
			};
		});
		
		public void ConnectDistance(Point point) => Connect(point, -Vector2.Distance(position, point.position));
		
		public void Connect(Point point, float value) => Connect(point, new Func<AIState, float>[]{ _ => value }, Array.Empty<Action<AIState>>());
		public void Connect<TStateComponent>(Point point, Func<TStateComponent, TStateComponent> stateModifier) where TStateComponent : struct, AIStateComponent => Connect(point, state => state.Set(stateModifier(state.Get<TStateComponent>())));
		public void Connect(Point point, Action<AIState> stateModifier) => Connect(point, Array.Empty<Func<AIState, float>>(), new[]{ stateModifier });
		public void Connect(Point point, Func<AIState, float>[] valueCalculators, Action<AIState>[] stateModifiers) => connectedPoints.Add(new Edge
		{
			destination = point,
			valueCalculators = valueCalculators,
			stateModifiers = stateModifiers,
		});
		
		public override string ToString() => name;
		
		private struct Edge
		{
			public Point destination;
			public Func<AIState, float>[] valueCalculators;
			public Action<AIState>[] stateModifiers;
		}
	}
}
