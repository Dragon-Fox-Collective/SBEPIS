using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class Storable
	{
		public abstract Vector3 position { get; set; }
		public abstract Quaternion rotation { get; set; }
		
		public abstract bool CanFetch(DequeStorable card);
		public abstract (DequeStorable, Capturellectainer) Store(Capturllectable item, out Capturllectable ejectedItem);
		public abstract Capturllectable Fetch(DequeStorable card);
	}
}
