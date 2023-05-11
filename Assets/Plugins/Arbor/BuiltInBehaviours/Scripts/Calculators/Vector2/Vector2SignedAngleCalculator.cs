//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromとToの間の符号付き角度を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates the signed angle in degrees between from and to.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.SignedAngle")]
	[BehaviourTitle("Vector2.SignedAngle")]
	[BuiltInBehaviour]
	public sealed class Vector2SignedAngleCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始ベクトル
		/// </summary>
#else
		/// <summary>
		/// Starting vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _From = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了ベクトル
		/// </summary>
#else
		/// <summary>
		/// End vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _To = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Result = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector2.SignedAngle(_From.value, _To.value));
		}
	}
}
