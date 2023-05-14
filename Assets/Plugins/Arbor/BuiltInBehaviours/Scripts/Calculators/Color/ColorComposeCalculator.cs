//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Colorを作成する。
	/// </summary>
#else
	/// <summary>
	/// Compose Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.Compose")]
	[BehaviourTitle("Color.Compose")]
	[BuiltInBehaviour]
	public sealed class ColorComposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// R成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of R component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _R = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// G成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of G component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _G = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// B成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of B component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _B = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// A成分の値
		/// </summary>
#else
		/// <summary>
		/// Value of A component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _A = new FlexibleFloat();

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
			_Result.SetValue(new Color(_R.value, _G.value, _B.value, _A.value));
		}
	}
}
