//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 設定した位置から一番近い、Bounds オブジェクトの最近接点を求める。
	/// </summary>
#else
	/// <summary>
	/// The closest point to the bounding box of the attached collider.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Collider/Collider.ClosestPointOnBounds")]
	[BehaviourTitle("Collider.ClosestPointOnBounds")]
	[BuiltInBehaviour]
	public sealed class ColliderClosestPointOnBoundsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Collider
		/// </summary>
		[SerializeField] private InputSlotCollider _Collider = new InputSlotCollider();

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置
		/// </summary>
#else
		/// <summary>
		/// Position
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Position = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
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
			Collider collider = null;
			_Collider.GetValue(ref collider);
			if (collider != null)
			{
				_Result.SetValue(collider.ClosestPointOnBounds(_Position.value));
			}
		}
	}
}
