//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ワールド空間からローカル空間へ Vector を変換する。
	/// </summary>
#else
	/// <summary>
	/// Transforms a Vector from world space to local space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.InverseTransformVector")]
	[BehaviourTitle("Transform.InverseTransformVector")]
	[BuiltInBehaviour]
	public sealed class TransformInverseTransformVectorCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ワールド空間でのベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector in world space
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Vector = new FlexibleVector3();

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

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Transform transform = _Transform.value;
			if (transform != null)
			{
				_Result.SetValue(transform.InverseTransformVector(_Vector.value));
			}
		}
	}
}
