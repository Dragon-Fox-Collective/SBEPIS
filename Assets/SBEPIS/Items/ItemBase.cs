using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Items
{
	public class ItemBase : MonoBehaviour
	{
		public Bit[] bits;

		public Transform replaceObject;
		public Transform aeratedAttachmentPoint;

        private void Start()
        {
            foreach (Bit bit in bits)
                print(bit.name);
        }
    }
}
