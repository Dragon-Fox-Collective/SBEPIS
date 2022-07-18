using UnityEngine;

namespace SBEPIS.Interaction
{
	public class KillPlane : MonoBehaviour
	{
		public Rigidbody player;
		public Rigidbody[] loosePlayerAttachments = new Rigidbody[0];

		private void Update()
		{
			if (player.transform.position.y < transform.position.y)
				Kill();
		}

		public void Kill()
		{
			player.transform.localPosition = Vector3.zero;

			foreach (Rigidbody attachment in loosePlayerAttachments)
				attachment.transform.position = player.transform.position;
		}
	}
}
