//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのサイズを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the size of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetSize")]
	[BehaviourTitle("BoundsInt.GetSize")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetSizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntのサイズの出力。
		/// </summary>
#else
		/// <summary>
		/// Result the size of the BoundsInt.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Size = new OutputSlotVector3Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Size.SetValue(_BoundsInt.value.size);
		}
	}
}