//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2 つのベクトルの各成分を乗算する。
	/// </summary>
#else
	/// <summary>
	/// Multiplies two vectors component-wise.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.Scale")]
	[BehaviourTitle("Vector2.Scale")]
	[BuiltInBehaviour]
	public sealed class Vector2ScaleCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector2
		/// </summary>
		[SerializeField] private FlexibleVector2 _Vector2 = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 乗算するベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector to multiply
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Scale = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector2.Scale(_Vector2.value, _Scale.value));
		}
	}
}
