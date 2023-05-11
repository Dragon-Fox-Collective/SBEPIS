//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ソルバーの反復回数。
	/// </summary>
#else
	/// <summary>
	/// The iteration count of the solver.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rigidbody/Rigidbody.SolverIterationCount")]
	[BehaviourTitle("Rigidbody.SolverIterationCount")]
	[BuiltInBehaviour]
	public sealed class RigidbodySolverIterationCountCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rigidbody
		/// </summary>
		[SerializeField] private FlexibleRigidbody _Rigidbody = new FlexibleRigidbody(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ソルバーの反復回数。
		/// </summary>
#else
		/// <summary>
		/// The iteration count of the solver.
		/// </summary>
#endif

		[SerializeField] private OutputSlotInt _SolverIterationCount = new OutputSlotInt();

		#endregion // Serialize fields

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			Rigidbody rigidbody = _Rigidbody.value;
			if (rigidbody != null)
			{
				_SolverIterationCount.SetValue(rigidbody.solverIterations);
			}
		}
	}
}
