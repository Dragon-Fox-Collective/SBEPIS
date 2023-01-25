using UnityEngine;

namespace SBEPIS.Utils
{
	public class RotationFlipper : MonoBehaviour
	{
		private bool flipped;

		public void Flip()
		{
			flipped = !flipped;
			transform.Rotate(0, 180, 0);
		}

		public void ResetFlip()
		{
			if (flipped)
				Flip();
		}
	}
}