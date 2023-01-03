using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Physics
{
	public class FixedDeltaTimeAdjuster : MonoBehaviour
	{
		public int mostDeltaTimes = 30;
		
		private readonly Queue<float> deltaTimes = new();

		private void Start()
		{
			print($"Starting fixedDeltaTime {Time.fixedDeltaTime}");
		}

		private void Update()
		{
			print("UPDATE");
			
			while (deltaTimes.Count >= mostDeltaTimes)
				deltaTimes.Dequeue();
			
			deltaTimes.Enqueue(Time.deltaTime);

			Time.fixedDeltaTime = deltaTimes.Aggregate(0f, (a, b) => a + b) / deltaTimes.Count;
			print(deltaTimes.ToDelimString());
			print(Time.fixedDeltaTime);
		}

		private void FixedUpdate()
		{
			
			print("FIXED UPDATE");
		}
	}
}