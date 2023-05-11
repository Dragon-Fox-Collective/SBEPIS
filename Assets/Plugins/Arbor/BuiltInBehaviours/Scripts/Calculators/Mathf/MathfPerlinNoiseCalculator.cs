//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2Dのパーリンノイズを生成する。
	/// </summary>
#else
	/// <summary>
	/// Generate 2D Perlin noise.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.PerlinNoise")]
	[BehaviourTitle("Mathf.PerlinNoise")]
	[BuiltInBehaviour]
	public sealed class MathfPerlinNoiseCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// サンプル点のX座標。
		/// </summary>
#else
		/// <summary>
		/// X-coordinate of sample point.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _X = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// サンプル点のY座標。
		/// </summary>
#else
		/// <summary>
		/// Y-coordinate of sample point.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Y = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Result = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Mathf.PerlinNoise(_X.value, _Y.value));
		}
	}
}
