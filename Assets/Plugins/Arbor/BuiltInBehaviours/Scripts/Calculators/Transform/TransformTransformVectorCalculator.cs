//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ローカル空間からワールド空間へ Vector を変換します。
	/// </summary>
#else
	/// <summary>
	/// Transforms Vector from local space to world space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.TransformVector")]
	[BehaviourTitle("Transform.TransformVector")]
	[BuiltInBehaviour]
	public sealed class TransformTransformVectorCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ローカル空間でのベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector in local space
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
				_Result.SetValue(transform.TransformVector(_Vector.value));
			}
		}
	}
}
