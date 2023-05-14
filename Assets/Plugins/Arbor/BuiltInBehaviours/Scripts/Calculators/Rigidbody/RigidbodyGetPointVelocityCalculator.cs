//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド座標における、Rigidbodyオブジェクトの速度を取得します
	/// </summary>
#else
	/// <summary>
	/// The velocity of the rigidbody at the point worldPoint in global space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.GetPointVelocity")]
	[BehaviourTitle("Rigidbody.GetPointVelocity")]
	[BuiltInBehaviour]
	public sealed class RigidbodyGetPointVelocityCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ワールド座標
		/// </summary>
#else
		/// <summary>
		/// the point worldPoint in global space.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _WorldPoint = new FlexibleVector3();

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
				_Result.SetValue(rigidbody.GetPointVelocity(_WorldPoint.value));
			}
		}
	}
}
