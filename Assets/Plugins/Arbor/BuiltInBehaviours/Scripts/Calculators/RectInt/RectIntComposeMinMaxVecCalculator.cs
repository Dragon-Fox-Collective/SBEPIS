//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxのベクトルからRectIntを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create RectInt from Min and Max vector.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.ComposeMinMaxVec")]
	[BehaviourTitle("RectInt.ComposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class RectIntComposeMinMaxVecCalculator : Calculator
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
		[SerializeField] private FlexibleVector2Int _Min = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2Int _Max = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotRectInt _Result = new OutputSlotRectInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt rectInt = new RectInt();
			rectInt.SetMinMax(_Min.value, _Max.value);
			_Result.SetValue(rectInt);
		}
	}
}