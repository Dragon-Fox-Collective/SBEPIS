//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntを作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.Compose")]
	[BehaviourTitle("BoundsInt.Compose")]
	[BuiltInBehaviour]
	public sealed class BoundsIntComposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置
		/// </summary>
#else
		/// <summary>
		/// Position
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Position = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// サイズ
		/// </summary>
#else
		/// <summary>
		/// Size
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3Int _Size = new FlexibleVector3Int();

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
			_Result.SetValue(new BoundsInt(_Position.value, _Size.value));
		}
	}
}