using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	[CreateAssetMenu]
	public class AIAction : ScriptableObject
	{
		[SerializeField] private List<AIValue> costs = new();
		
		[SerializeField] private List<AIActionConstraint> constraints = new();
		
		[SerializeField] private List<AIActionConstraint> solvedConstraints = new();
		
		public IEnumerable<AIAction> GetBestRouteToComplete(List<AIValuedAction> possibleActions, Func<AIValueTotalCost, float> valueConverter, out float totalValue)
		{
			throw new NotImplementedException();
			/*
			foreach (AIActionConstraint constraint in constraints)
			{
				(IEnumerable<AIAction> performedActions, float value) = possibleActions.Select(possibleAction => (satisfied: possibleAction.Action.CanSatisfy(constraint, possibleActions, out AIValueTotalCost actionCosts, out IEnumerable<AIAction> subPerformedActions), actionCosts, performedActions: subPerformedActions.Prepend(possibleAction.Action)))
					.Where(zip => zip.satisfied)
					.Aggregate((Enumerable.Empty<AIAction>(), 0f), (currentMax, zip) =>
					{
						float newValue = valueConverter(zip.actionCosts);
						return newValue > currentMax.Item2 ? (zip.performedActions, newValue) : currentMax;
					});
				totalValue = value;
				return performedActions;
			}
			*/
		}
		
		public bool CanSatisfy(AIActionConstraint constraint, List<AIValuedAction> possibleActions, out AIValueTotalCost costs, out IEnumerable<AIAction> performedActions)
		{
			throw new NotImplementedException();
			/*
			if (!GetRoutesToComplete(possibleActions, out costs, out performedActions))
				return false;
			
			if (solvedConstraints.Contains(constraint))
			{
				costs += this.costs.Sum();
				return true;
			}
			else
			{
				costs = AIValueTotalCost.Zero;
				performedActions = Enumerable.Empty<AIAction>();
				return false;
			}
			*/
		}
	}
}
