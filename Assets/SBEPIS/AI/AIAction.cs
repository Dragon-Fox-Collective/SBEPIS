using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.AI
{
	[CreateAssetMenu]
	public class AIAction : ScriptableObject
	{
		[SerializeField] private List<AIValueCost> costs = new();
		
		[SerializeField] private List<AIActionConstraint> constraints = new();
		
		[SerializeField] private List<AIActionConstraint> solvedConstraints = new();
		
		public bool CanComplete(List<AIAction> possibleActions, out AIValueTotalCost costs)
		{
			costs = new AIValueTotalCost();
			
			foreach (AIActionConstraint constraint in constraints)
			{
				bool satisfied = false;
				foreach (AIAction possibleAction in possibleActions)
				{
					if (possibleAction.CanSatisfy(constraint, possibleActions, out AIValueTotalCost actionCosts))
					{
						satisfied = true;
						costs.AddCosts(actionCosts);
						break;
					}
				}
				
				if (!satisfied)
				{
					costs.Clear();
					return false;
				}
			}
			
			return true;
		}
		
		public bool CanSatisfy(AIActionConstraint constraint, List<AIAction> possibleActions, out AIValueTotalCost costs)
		{
			if (!CanComplete(possibleActions, out costs))
				return false;
			
			foreach (AIActionConstraint solvedConstraint in solvedConstraints)
			{
				if (constraint == solvedConstraint)
				{
					this.costs.ForEach(costs.AddCost);
					return true;
				}
			}
			
			costs.Clear();
			return false;
		}
	}
}
