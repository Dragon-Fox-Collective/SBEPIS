//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の中心座標を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the center of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.GetCenter")]
	[BehaviourTitle("Rect.GetCenter")]
	[BuiltInBehaviour]
	public sealed class RectGetCenterCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 中心座標の出力
		/// </summary>
#else
		/// <summary>
		/// Output the position of the center of the rectangle.
		/// </summary>
#endif
		[FormerlySerializedAs("_Result")]
		[SerializeField] private OutputSlotVector2 _Center = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Center.SetValue(_Rect.value.center);
		}
	}
}
