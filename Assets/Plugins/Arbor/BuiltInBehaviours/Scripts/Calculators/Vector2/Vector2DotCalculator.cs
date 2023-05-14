//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのベクトルの内積
	/// </summary>
#else
	/// <summary>
	/// Dot Product of two vectors.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.Dot")]
	[BehaviourTitle("Vector2.Dot")]
	[BuiltInBehaviour]
	public sealed class Vector2DotCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 左側の値
		/// </summary>
#else
		/// <summary>
		/// Left side value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Lhs = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 右側の値
		/// </summary>
#else
		/// <summary>
		/// Right side value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Rhs = new FlexibleVector2();

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
			_Result.SetValue(Vector2.Dot(_Lhs.value, _Rhs.value));
		}
	}
}
