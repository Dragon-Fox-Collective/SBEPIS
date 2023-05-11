using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Examples
{
	using Arbor.BehaviourTree;
	using Arbor.ObjectPooling;

	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/OnTriggerEnter")]
	public sealed class OnTriggerEnterDecorator : Decorator
	{
		[SerializeField]
		private TagChecker _TagChecker = new TagChecker();

		[SerializeField]
		private OutputSlotCollider _Collider = new OutputSlotCollider();

		bool CheckTag(GameObject gameObject)
		{
			return _TagChecker.CheckTag(gameObject);
		}

		private HashSet<Collider> _Colliders = new HashSet<Collider>();
		
		void OnTriggerEnter(Collider collider)
		{
			if (CheckTag(collider.gameObject))
			{
				Debug.Log($"{gameObject.name} : OnTriggerEnter : {collider}");
				_Collider.SetValue(collider);
				_Colliders.Add(collider);
			}
		}

		void OnTriggerExit(Collider collider)
		{
			if (_Colliders.Remove(collider))
			{
				Debug.Log($"{gameObject.name} : OnTriggerExit : {collider}");
			}
		}

		protected override bool OnConditionCheck()
		{
			bool entered = false;

			foreach (var collider in _Colliders)
			{
				if (ObjectPool.IsAlive(collider))
				{
					entered = true;
					break;
				}
			}
			
			if (entered)
			{
				Debug.Log($"{gameObject.name} : OnConditionCheck");
			}
			return entered;
		}

		protected override void OnRevaluationExit()
		{
			_Colliders.Clear();
		}
	}
}