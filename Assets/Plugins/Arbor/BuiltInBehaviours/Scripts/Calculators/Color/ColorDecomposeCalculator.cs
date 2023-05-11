//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Colorを分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.Decompose")]
	[BehaviourTitle("Color.Decompose")]
	[BuiltInBehaviour]
	public sealed class ColorDecomposeCalculator : Calculator
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
		/// R成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of R component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _R = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// G成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of G component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _G = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// B成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of B component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _B = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// A成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of A component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _A = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Color color = _Input.value;
			_R.SetValue(color.r);
			_G.SetValue(color.g);
			_B.SetValue(color.b);
			_A.SetValue(color.a);
		}
	}
}
