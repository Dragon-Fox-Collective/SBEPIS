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
		public bool offsetXFromEnd = false;
		public float offsetX = 0.1f;
		public bool offsetYFromEnd = false;
		public float offsetY = 0.1f;
		
		public void Layout<TState>(IList<Storable> inventory, TState state) where TState : DirectionState, TreeDictionaryState
		{
			Vector3 rightDirection = state.Direction;
			Vector3 absRightDirection = rightDirection.Select(Mathf.Abs);
			Vector3 downDirection = Quaternion.Euler(0, 0, -90) * rightDirection;
			Vector3 absDownDirection = downDirection.Select(Mathf.Abs);
			
			foreach (Storable storable in inventory)
				storable.Layout(Quaternion.Euler(0, 0, -60) * rightDirection);
			
			float layerLength = state.Tree.Any() ? state.Tree.Layers.Select(layer => layer.Select(zip => Vector3.Project(zip.Item2.MaxPossibleSize, absRightDirection).magnitude).Aggregate(Mathf.Max)).Aggregate(Mathf.Max) : 0;
			List<int> layerCounts = state.Tree.Layers.Select((_, depth) => (int)Mathf.Pow(2, depth)).ToList();
			List<float> layerOffsets = layerCounts.AsEnumerable().Reverse().Select(count => count * offsetX).ToList();
			List<float> layerLengthSums = layerCounts.Zip(layerOffsets, (layerCount, layerOffset) => offsetXFromEnd
				? layerOffset * (layerCount - 1) + layerCount * layerLength
				: layerOffset * (layerCount - 1)).ToList();
			
			List<float> layerHeights = state.Tree.Layers.Select(layer => layer.Select(zip => Vector3.Project(zip.Item2.MaxPossibleSize, absDownDirection).magnitude).Aggregate(Mathf.Max)).ToList();
			float heightSum = offsetYFromEnd
				? offsetY * (state.Tree.Layers.Count() - 1) + layerHeights.Aggregate(ExtensionMethods.Add)
				: offsetY * (state.Tree.Layers.Count() - 1);
			
			Vector3 up = -heightSum / 2 * downDirection;
			foreach ((IEnumerable<(int, Storable)> layer, float layerLengthSum, float layerHeight, float layerOffset) in state.Tree.Layers.Zip(layerLengthSums, layerHeights, layerOffsets))
			{
				up += downDirection * (offsetYFromEnd ? layerHeight / 2 : 0);
				
				Vector3 right = -layerLengthSum / 2 * rightDirection;
				int lastIndex = -1;
				foreach ((int index, Storable storable) in layer)
				{
					right += rightDirection * (index - lastIndex - 1) * (layerOffset + (offsetXFromEnd ? layerLength : 0));
					right += rightDirection * (offsetXFromEnd ? layerLength / 2 : 0);
					
					storable.Position = right + up;
					storable.Rotation = DequeLayout.GetOffsetRotation(rightDirection);
					
					right += rightDirection * (layerOffset + (offsetXFromEnd ? layerLength / 2 : 0));
					lastIndex = index;
				}
				
				up += downDirection * (offsetY + (offsetYFromEnd ? layerHeight / 2 : 0));
			}
			
			List<Storable> spares = inventory.Except(state.Tree).ToList();
			float sparesHeight = spares.Select(spare => Vector3.Project(spare.MaxPossibleSize, absDownDirection).magnitude).Aggregate(Mathf.Max);
			foreach (Storable storable in spares)
			{
				storable.Position = -(heightSum / 2 + offsetY + sparesHeight / 2) * downDirection;
				storable.Rotation = DequeLayout.GetOffsetRotation(rightDirection);
			}
		}
	}
	
	[Serializable]
	public class TreeLayoutSettings : LayoutSettings<TreeLayout>
	{
		[SerializeField] private TreeLayout layout;
		public TreeLayout Layout => layout;
	}
}
