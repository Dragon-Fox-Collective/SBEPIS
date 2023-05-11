//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxのベクトルからBoundsIntを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create BoundsInt from Min and Max vector.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.ComposeMinMaxVec")]
	[BehaviourTitle("BoundsInt.ComposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class BoundsIntComposeMinMaxVecCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Min = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Max = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBoundsInt _Result = new OutputSlotBoundsInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt boundsInt = new BoundsInt();
			boundsInt.SetMinMax(_Min.value, _Max.value);
			_Result.SetValue(boundsInt);
		}
	}
}