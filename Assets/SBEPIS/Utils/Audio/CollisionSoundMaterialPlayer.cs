using UnityEngine;

namespace SBEPIS.Utils.Audio
{
	public class CollisionSoundMaterialPlayer : MonoBehaviour
	{
		[SerializeField] private float impulseThreshold = 0.1f;
		
		private void OnCollisionEnter(Collision collision)
		{
			float impulse = collision.impulse.magnitude;
			if (impulse >= impulseThreshold)
			{
				ContactPoint contact = collision.contacts.MaxBy(contact => contact.impulse.magnitude);
				if (contact.thisCollider.TryGetComponent(out CollisionSoundMaterial material))
					material.Play(contact.point, impulse);
			}
		}
	}
}