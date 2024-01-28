using UnityEngine;

namespace SBEPIS.Items.Modules
{
	public class Jet : MonoBehaviour
	{
		[SerializeField] private float force = 50;
		
		private new Rigidbody rigidbody;
		
		private void Start()
		{
			rigidbody = GetComponentInParent<Rigidbody>();
		}
		
		private void FixedUpdate()
		{
			if (rigidbody)
				rigidbody.AddForceAtPosition(-transform.up * force, transform.position);
		}
	}
}