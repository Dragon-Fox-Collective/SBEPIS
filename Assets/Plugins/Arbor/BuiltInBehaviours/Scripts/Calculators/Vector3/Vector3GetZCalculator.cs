//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3のZ成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the Z component of Vector3.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.GetZ")]
	[BehaviourTitle("Vector3.GetZ")]
	[BuiltInBehaviour]
	public sealed class Vector3GetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3
		/// </summary>
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Z component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Z = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Z.SetValue(_Vector3.value.z);
		}
	}
}
