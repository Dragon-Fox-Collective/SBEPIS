//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ローカル空間からワールド空間へ Direction を変換する
	/// </summary>
#else
	/// <summary>
	/// Transforms Direction from local space to world space.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.TransformDirection")]
	[BehaviourTitle("Transform.TransformDirection")]
	[BuiltInBehaviour]
	public sealed class TransformTransformDirectionCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ローカル空間での方向
		/// </summary>
#else
		/// <summary>
		/// Direction in local space
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Direction = new FlexibleVector3();

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
				_Result.SetValue(transform.TransformDirection(_Direction.value));
			}
		}
	}
}
