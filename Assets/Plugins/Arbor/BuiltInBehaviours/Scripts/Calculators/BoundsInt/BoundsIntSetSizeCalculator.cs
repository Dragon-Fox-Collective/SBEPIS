//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのサイズを設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the size of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.SetSize")]
	[BehaviourTitle("BoundsInt.SetSize")]
	[BuiltInBehaviour]
	public sealed class BoundsIntSetSizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntのサイズ
		/// </summary>
#else
		/// <summary>
		/// the size of the BoundsInt.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Size = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBoundsInt _Result = new OutputSlotBoundsInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt value = _BoundsInt.value;
			value.size = _Size.value;
			_Result.SetValue(value);
		}
	}
}