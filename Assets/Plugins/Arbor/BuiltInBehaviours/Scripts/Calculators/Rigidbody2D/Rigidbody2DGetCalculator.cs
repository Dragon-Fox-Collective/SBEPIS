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
	/// GameObjectにアタッチされているRigidbody2Dを取得する。
	/// </summary>
#else
	/// <summary>
	/// Gets the Rigidbody2D attached to GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.Get")]
	[BehaviourTitle("Rigidbody2D.Get")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DGetCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// GameObject
		/// </summary>
		[SerializeField] private FlexibleGameObject _GameObject = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 取得したRigidbody2D
		/// </summary>
#else
		/// <summary>
		/// Get the Rigidbody2D
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody2D _Rigidbody2D = new OutputSlotRigidbody2D();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			GameObject gameObject = _GameObject.value;
			if (gameObject != null)
			{
				Rigidbody2D rigidbody2D = null;
				if (!gameObject.TryGetComponent<Rigidbody2D>(out rigidbody2D))
				{
					rigidbody2D = null;
				}
				_Rigidbody2D.SetValue(rigidbody2D);
			}
		}
	}
}
