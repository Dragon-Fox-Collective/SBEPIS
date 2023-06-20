using System;
using System.Collections.Generic;
using System.Linq;
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
			
			List<AIEdge> edges = new();
			edges.AddRange(startNode.Edges);
			
			AINode solution = null;
			while (edges.Count > 0)
			{
				AIEdge edge = edges.Max();
				edges.Remove(edge);
				
				//Debug.Log($"{edge} (best is {(solution == null ? "null" : solution)})");
				
				AINode node = edge.NewDestination();
				
				if (node.point == goal && (solution == null || solution.currentValue < node.currentValue))
					solution = node;
				
				const float sunkCostBreakOff = 5;
				if (solution != null && node.currentValue < solution.currentValue - sunkCostBreakOff)
					break;
				
				edges.AddRange(node.Edges);
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
	
	public struct AIEdge : IComparable<AIEdge>
	{
		public AINode origin;
		public Point destination;
		public float value;
		public AIState state;
		
		public float TotalValue => origin.currentValue + value;
		
		public AINode NewDestination() => new()
		{
			point = destination,
			previousNode = origin,
			currentValue = TotalValue,
			state = state,
		};
		
		public int CompareTo(AIEdge other) => TotalValue.CompareTo(other.TotalValue);
		
		public override string ToString() => $"{origin} ==> {destination}{{{TotalValue:0.##} value, {state}}}";
	}
	
	public class AINode
	{
		public Point point;
		public AINode previousNode;
		public float currentValue;
		public AIState state;
		
		public IEnumerable<AIEdge> Edges => point.GetEdges(this);
		
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
		
		public override string ToString() => $"{point}{{{currentValue:0.##} value, {state}}}";
	}
	
	public struct AIState
	{
		public float steps;
		public float cash;
		
		public override string ToString() => $"{steps} steps taken, ${cash} left";
	}
	
	public class Point
	{
		private string name;
		private Vector2 position;
		private List<AIEdgeDestinationGenerator> accessiblePoints = new();
		
		public Point(string name, float x, float y) : this(name, new Vector2(x, y)) { }
		public Point(string name, Vector2 position)
		{
			this.name = name;
			this.position = position;
		}
		
		public IEnumerable<AIEdge> GetEdges(AINode origin) => accessiblePoints.Select(edge => new AIEdge()
		{
			origin = origin,
			destination = edge.destination,
			value = edge.valueGenerator(origin.state),
			state = edge.stateGenerator(origin.state),
		});
		
		public void Connect(Point point) => Connect(point, state => -Vector2.Distance(position, point.position) - state.steps, TakeStep);
		public void Connect(Point point, float value) => Connect(point, state => value - state.steps, TakeStep);
		public void Connect(Point point, Func<AIState, float> valueGenerator, Func<AIState, AIState> stateGenerator) => accessiblePoints.Add(new AIEdgeDestinationGenerator
		{
			destination = point,
			valueGenerator = valueGenerator,
			stateGenerator = stateGenerator,
		});
		
		private static AIState TakeStep(AIState state)
		{
			state.steps++;
			return state;
		}
		
		public override string ToString() => name;
		
		private struct AIEdgeDestinationGenerator
		{
			public Point destination;
			public Func<AIState, float> valueGenerator;
			public Func<AIState, AIState> stateGenerator;
		}
	}
}
