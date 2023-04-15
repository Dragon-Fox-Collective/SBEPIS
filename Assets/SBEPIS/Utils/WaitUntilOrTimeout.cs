using System;
using UnityEngine;

namespace SBEPIS.Utils
{
	public sealed class WaitUntilOrTimeout : CustomYieldInstruction
	{
		private Func<bool> predicate;
		private float time = 0;
		private float timeout;
		
		public override bool keepWaiting
		{
			get
			{
				time += Time.deltaTime;
				return !(time >= timeout || predicate());
			}
		}
		
		public WaitUntilOrTimeout(Func<bool> predicate, float timeout)
		{
			this.predicate = predicate;
			this.timeout = timeout;
		}
	}
}