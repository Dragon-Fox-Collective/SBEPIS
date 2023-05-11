//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// 法線を基準にしてベクトルの反射したベクトルを取得する。
	/// </summary>
#else
	/// <summary>
	/// Reflects a vector off the vector defined by a normal.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.Reflect")]
	[BehaviourTitle("Vector2.Reflect")]
	[BuiltInBehaviour]
	public sealed class Vector2ReflectCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入射ベクトル
		/// </summary>
#else
		/// <summary>
		/// Incident vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _InDirection = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 法線ベクトル
		/// </summary>
#else
		/// <summary>
		/// Normal vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _InNormal = new FlexibleVector2();

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
			_Result.SetValue(Vector2.Reflect(_InDirection.value, _InNormal.value));
		}
	}
}
