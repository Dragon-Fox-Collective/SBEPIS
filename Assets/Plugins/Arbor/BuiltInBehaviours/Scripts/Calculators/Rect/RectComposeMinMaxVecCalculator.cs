//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxからRectを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create Rect from Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.ComposeMinMaxVec")]
	[BehaviourTitle("Rect.ComposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class RectComposeMinMaxVecCalculator : Calculator
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
		[SerializeField] private FlexibleVector2 _Min = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum value.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Max = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotRect _Result = new OutputSlotRect();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Vector2 min = _Min.value;
			Vector2 max = _Max.value;
			_Result.SetValue(Rect.MinMaxRect(min.x, min.y, max.x, max.y));
		}
	}
}
