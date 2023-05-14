//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// オイラー角としての角度
	/// </summary>
#else
	/// <summary>
	/// The rotation as Euler angles in degrees.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/Transform.EulerAngles")]
	[BehaviourTitle("Transform.EulerAngles")]
	[BuiltInBehaviour]
	public sealed class TransformEulerAnglesCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Transform
		/// </summary>
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// オイラー角としての角度
		/// </summary>
#else
		/// <summary>
		/// The rotation as Euler angles in degrees.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _EulerAngles = new OutputSlotVector3();

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
				_EulerAngles.SetValue(transform.eulerAngles);
			}
		}
	}
}
