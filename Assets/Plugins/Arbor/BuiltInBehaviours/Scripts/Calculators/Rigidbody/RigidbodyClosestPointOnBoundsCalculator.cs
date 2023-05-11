//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// アタッチされたColliderのバウンディングボックスにもっとも近い位置
	/// </summary>
#else
	/// <summary>
	/// The closest point to the bounding box of the attached colliders.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.ClosestPointOnBounds")]
	[BehaviourTitle("Rigidbody.ClosestPointOnBounds")]
	[BuiltInBehaviour]
	public sealed class RigidbodyClosestPointOnBoundsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 最近接点を計算する位置
		/// </summary>
#else
		/// <summary>
		/// Position to calculate the closest point
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Position = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody rigidbody = _Rigidbody.value;
			if (rigidbody != null)
			{
				_Result.SetValue(rigidbody.ClosestPointOnBounds(_Position.value));
			}
		}
	}
}
