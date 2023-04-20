using KBCore.Refs;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class Deque : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private StorableGroupDefinition definition;
		public StorableGroupDefinition Definition => definition;
	}
}
