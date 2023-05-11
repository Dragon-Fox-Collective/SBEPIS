//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 平面に垂直な法線ベクトルによって定義される平面上にベクトルを射影する。
	/// </summary>
#else
	/// <summary>
	/// Projects a vector onto a plane defined by a normal orthogonal to the plane.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.ProjectOnPlane")]
	[BehaviourTitle("Vector3.ProjectOnPlane")]
	[BuiltInBehaviour]
	public sealed class Vector3ProjectOnPlaneCalculator : Calculator
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
		/// 平面法線
		/// </summary>
#else
		/// <summary>
		/// Plane normal
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _PlaneNormal = new FlexibleVector3();

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
			_Result.SetValue(Vector3.ProjectOnPlane(_Vector.value, _PlaneNormal.value));
		}
	}
}
