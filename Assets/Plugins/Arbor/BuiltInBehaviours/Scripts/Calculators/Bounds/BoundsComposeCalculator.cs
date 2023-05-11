//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsを作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.Compose")]
	[BehaviourTitle("Bounds.Compose")]
	[BuiltInBehaviour]
	public sealed class BoundsComposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 中心
		/// </summary>
#else
		/// <summary>
		/// Center
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Center = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// サイズ
		/// </summary>
#else
		/// <summary>
		/// Size
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Size = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBounds _Result = new OutputSlotBounds();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(new Bounds(_Center.value, _Size.value));
		}
	}
}
