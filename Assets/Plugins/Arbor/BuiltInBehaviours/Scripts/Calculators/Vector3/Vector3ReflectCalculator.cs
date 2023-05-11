//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 法線で定義された平面でベクトルを反射する。
	/// </summary>
#else
	/// <summary>
	/// Reflects a vector off the plane defined by a normal.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.Reflect")]
	[BehaviourTitle("Vector3.Reflect")]
	[BuiltInBehaviour]
	public sealed class Vector3ReflectCalculator : Calculator
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
		[SerializeField] private FlexibleVector3 _InDirection = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 法線ベクトル
		/// </summary>
#else
		/// <summary>
		/// Normal vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _InNormal = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector3.Reflect(_InDirection.value, _InNormal.value));
		}
	}
}
