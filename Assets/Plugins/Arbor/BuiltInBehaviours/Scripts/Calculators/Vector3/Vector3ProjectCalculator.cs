//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ベクトルを別のベクトルに投影する。
	/// </summary>
#else
	/// <summary>
	/// Calculate so that the vector is normalized and orthogonal to other vectors.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.Project")]
	[BehaviourTitle("Vector3.Project")]
	[BuiltInBehaviour]
	public sealed class Vector3ProjectCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Vector = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 法線ベクトル
		/// </summary>
#else
		/// <summary>
		/// Normal vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _OnNormal = new FlexibleVector3();

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
			_Result.SetValue(Vector3.Project(_Vector.value, _OnNormal.value));
		}
	}
}
