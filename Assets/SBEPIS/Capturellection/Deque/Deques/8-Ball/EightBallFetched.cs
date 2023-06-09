using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class EightBallFetched : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private CaptureContainer container;
		public CaptureContainer Container => container;
		[SerializeField, Self] private Capturellectable capturellectable;
		public Capturellectable Capturellectable => capturellectable;
		
		public void Break()
		{
			Capturellectable item = container.Fetch();
			if (item) item.transform.position = transform.position;
			Destroy(gameObject);
		}
	}
}
