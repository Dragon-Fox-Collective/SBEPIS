//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntを中心とサイズに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose BoundsInt to size and size.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.Decompose")]
	[BehaviourTitle("BoundsInt.Decompose")]
	[BuiltInBehaviour]
	public sealed class BoundsIntDecomposeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置を出力
		/// </summary>
#else
		/// <summary>
		/// Position output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Position = new OutputSlotVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// サイズを出力
		/// </summary>
#else
		/// <summary>
		/// Size output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Size = new OutputSlotVector3Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt bounds = _BoundsInt.value;

			_Position.SetValue(bounds.position);
			_Size.SetValue(bounds.size);
		}
	}
}