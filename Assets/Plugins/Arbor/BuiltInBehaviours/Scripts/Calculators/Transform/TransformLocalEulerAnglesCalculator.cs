//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 親の Transform オブジェクトから見た相対的なオイラー角としての回転値
	/// </summary>
#else
	/// <summary>
	/// The rotation as Euler angles in degrees relative to the parent transform's rotation.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.LocalEulerAngles")]
	[BehaviourTitle("Transform.LocalEulerAngles")]
	[BuiltInBehaviour]
	public sealed class TransformLocalEulerAnglesCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 相対的なオイラー角としての回転値
		/// </summary>
#else
		/// <summary>
		/// The rotation as Euler angles in degrees relative.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _LocalEulerAngles = new OutputSlotVector3();

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
				_LocalEulerAngles.SetValue(transform.localEulerAngles);
			}
		}
	}
}
