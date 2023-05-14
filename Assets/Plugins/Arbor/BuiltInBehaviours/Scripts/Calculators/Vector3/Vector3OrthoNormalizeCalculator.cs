//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ベクトルが正規化され他のベクトルと直交するように計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate so that the vector is normalized and orthogonal to other vectors.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.OrthoNormalize")]
	[BehaviourTitle("Vector3.OrthoNormalize")]
	[BuiltInBehaviour]
	public sealed class Vector3OrthoNormalizeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力する法線ベクトル
		/// </summary>
#else
		/// <summary>
		/// Input normal vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _InputNormal = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力するTangentベクトル
		/// </summary>
#else
		/// <summary>
		/// Input tangent vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _InputTangent = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 法線ベクトル出力
		/// </summary>
#else
		/// <summary>
		/// Output normal vector
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _OutputNormal = new OutputSlotVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// Tangentベクトル出力
		/// </summary>
#else
		/// <summary>
		/// Output tangent vector
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _OutputTangent = new OutputSlotVector3();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector3 normal = _InputNormal.value;
			Vector3 tangent = _InputTangent.value;

			Vector3.OrthoNormalize(ref normal, ref tangent);

			_OutputNormal.SetValue(normal);
			_OutputTangent.SetValue(tangent);
		}
	}
}
