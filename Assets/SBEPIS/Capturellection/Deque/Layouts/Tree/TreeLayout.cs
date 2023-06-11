using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	[Serializable]
	public class TreeLayout : DequeLayoutBase
	{
		public bool offsetFromEnd = false;
		public float offset = 0.1f;
		
		public void Layout<TState>(IList<Storable> inventory, TState state) where TState : DirectionState, TreeDictionaryState
		{
			Vector3 rightDirection = state.Direction;
			Vector3 absRightDirection = rightDirection.Select(Mathf.Abs);
			Vector3 downDirection = Quaternion.Euler(0, 0, -90) * rightDirection;
			Vector3 absDownDirection = downDirection.Select(Mathf.Abs);
			
			foreach (Storable storable in inventory)
				storable.Layout(Quaternion.Euler(0, 0, -60) * rightDirection);
			
			List<float> layerLengths = state.Tree.Layers.Select(layer => layer.Select(zip => Vector3.Project(zip.Item2.MaxPossibleSize, absRightDirection).magnitude).Aggregate(Mathf.Max)).ToList();
			List<float> layerLengthSums = state.Tree.Layers.Zip(layerLengths).Select((zip, depth) =>
			{
				int count = (int)Mathf.Pow(2, depth);
				return offsetFromEnd
					? offset * (count - 1) + count * zip.Item2
					: offset * (count - 1);
			}).ToList();
			
			List<float> layerHeights = state.Tree.Layers.Select(layer => layer.Select(zip => Vector3.Project(zip.Item2.MaxPossibleSize, absDownDirection).magnitude).Aggregate(Mathf.Max)).ToList();
			float heightSum = offsetFromEnd
				? offset * (state.Tree.Layers.Count() - 1) + layerHeights.Aggregate(ExtensionMethods.Add)
				: offset * (state.Tree.Layers.Count() - 1);
			
			Vector3 up = -heightSum / 2 * downDirection;
			foreach ((IEnumerable<(int, Storable)> layer, float layerLength, float layerLengthSum, float layerHeight) in state.Tree.Layers.Zip(layerLengths, layerLengthSums, layerHeights))
			{
				up += downDirection * (offsetFromEnd ? layerHeight / 2 : 0);
				
				Vector3 right = -layerLengthSum / 2 * rightDirection;
				int lastIndex = -1;
				foreach ((int index, Storable storable) in layer)
				{
					right += rightDirection * (index - lastIndex - 1) * offset;
					right += rightDirection * (offsetFromEnd ? layerLength / 2 : 0);
					
					storable.Position = right + up;
					storable.Rotation = DequeLayout.GetOffsetRotation(rightDirection);
					
					right += rightDirection * (offset + (offsetFromEnd ? layerLength / 2 : 0));
					lastIndex = index;
				}
				
				up += downDirection * (offset + (offsetFromEnd ? layerHeight / 2 : 0));
			}
			
			List<Storable> spares = inventory.Except(state.Tree).ToList();
			float sparesHeight = spares.Select(spare => Vector3.Project(spare.MaxPossibleSize, absDownDirection).magnitude).Aggregate(Mathf.Max);
			foreach (Storable storable in spares)
			{
				storable.Position = -(heightSum / 2 + offset + sparesHeight / 2) * downDirection;
				storable.Rotation = DequeLayout.GetOffsetRotation(rightDirection);
			}
		}
	}
}
