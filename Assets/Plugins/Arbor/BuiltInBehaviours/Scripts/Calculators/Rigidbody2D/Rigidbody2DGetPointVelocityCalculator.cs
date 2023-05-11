//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド座標における、Rigidbody2Dオブジェクトの速度を取得します
	/// </summary>
#else
	/// <summary>
	/// The velocity of the Rigidbody2D at the point Point in global space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody2D/Rigidbody2D.GetPointVelocity")]
	[BehaviourTitle("Rigidbody2D.GetPointVelocity")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DGetPointVelocityCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody2D
		/// </summary>
		[SerializeField] private FlexibleRigidbody2D _Rigidbody2D = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ワールド座標
		/// </summary>
#else
		/// <summary>
		/// the point worldPoint in global space.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Point = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody2D rigidbody2D = _Rigidbody2D.value;
			if (rigidbody2D != null)
			{
				_Result.SetValue(rigidbody2D.GetPointVelocity(_Point.value));
			}
		}
	}
}
