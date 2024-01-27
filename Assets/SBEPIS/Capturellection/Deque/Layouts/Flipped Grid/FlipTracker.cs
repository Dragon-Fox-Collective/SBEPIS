using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.Deques
{
	public class FlipTracker : MonoBehaviour
	{
		public UnityEvent onFaceUp = new();
		public UnityEvent onFaceDown = new();
		
		private Dictionary<object, bool> faceUps = new();
		private bool currentlyFacingUp = true;
		
		public void Flip(object key, bool faceUp)
		{
			faceUps[key] = faceUp;
			if (faceUps.Values.All()) FaceUp();
			else FaceDown();
		}
		
		private void FaceUp()
		{
			if (currentlyFacingUp) return;
			currentlyFacingUp = true;
			onFaceUp.Invoke();
		}
		
		private void FaceDown()
		{
			if (!currentlyFacingUp) return;
			currentlyFacingUp = false;
			onFaceDown.Invoke();
		}
	}
}