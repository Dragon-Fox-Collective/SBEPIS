//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorをHSVへ分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Color into HSV.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.DecomposeHSV")]
	[BehaviourTitle("Color.DecomposeHSV")]
	[BuiltInBehaviour]
	public sealed class ColorDecomposeHSVCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力値
		/// </summary>
#else
		/// <summary>
		/// Input value
		/// </summary>
#endif
		[SerializeField] private FlexibleColor _Input = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// H成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of H component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _H = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// S成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of S component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _S = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// V成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of V component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _V = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Color color = _Input.value;
			float h, s, v;
			Color.RGBToHSV(color, out h, out s, out v);
			_H.SetValue(h);
			_S.SetValue(s);
			_V.SetValue(v);
		}
	}
}
