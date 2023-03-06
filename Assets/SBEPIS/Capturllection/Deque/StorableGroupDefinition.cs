using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class StorableGroupDefinition : MonoBehaviour
	{
		public DequeRuleset ruleset;
		public int maxStorables;
		public StorableGroupDefinition subdefinition;

		public Storable GetNewStorable(Transform parent)
		{
			GameObject childGameObject = new("Storable");
			Transform child = childGameObject.transform;
			child.SetParent(parent);
			child.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			
			return subdefinition != null ? new StorableGroup(child, subdefinition) : new StorableSlot(child);
		}
	}
}
