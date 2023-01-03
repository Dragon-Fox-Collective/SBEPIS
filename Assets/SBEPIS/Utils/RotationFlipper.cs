using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
