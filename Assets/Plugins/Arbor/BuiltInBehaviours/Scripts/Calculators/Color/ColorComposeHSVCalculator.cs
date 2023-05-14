//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorをHSVから作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose Color from HSV.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.ComposeHSV")]
	[BehaviourTitle("Color.ComposeHSV")]
	[BuiltInBehaviour]
	public sealed class ColorComposeHSVCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// H成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of H component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _H = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// S成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of S component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _S = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// V成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of V component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _V = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// HDRカラーを出力するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to output HDR color.
		/// </summary>
#endif
		[SerializeField] private FlexibleBool _HDR = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotColor _Result = new OutputSlotColor();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Color.HSVToRGB(_H.value, _S.value, _V.value, _HDR.value));
		}
	}
}
