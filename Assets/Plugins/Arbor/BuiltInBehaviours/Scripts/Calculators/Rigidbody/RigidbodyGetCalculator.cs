//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectにアタッチされているRigidbodyを取得する。
	/// </summary>
#else
	/// <summary>
	/// Gets the Rigidbody attached to GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.Get")]
	[BehaviourTitle("Rigidbody.Get")]
	[BuiltInBehaviour]
	public sealed class RigidbodyGetCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// GameObject
		/// </summary>
		[SerializeField] private FlexibleGameObject _GameObject = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 取得したRigidbody
		/// </summary>
#else
		/// <summary>
		/// Get the Rigidbody
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody _Rigidbody = new OutputSlotRigidbody();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			GameObject gameObject = _GameObject.value;
			if (gameObject != null)
			{
				Rigidbody rigidbody = null;
				if (!gameObject.TryGetComponent<Rigidbody>(out rigidbody))
				{
					rigidbody = null;
				}
				_Rigidbody.SetValue(rigidbody);
			}
		}
	}
}
