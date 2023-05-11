//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3IntのZ成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the Z component of Vector3Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.GetZ")]
	[BehaviourTitle("Vector3Int.GetZ")]
	[BuiltInBehaviour]
	public sealed class Vector3IntGetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3Int
		/// </summary>
		[SerializeField] private FlexibleVector3Int _Vector3Int = new FlexibleVector3Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Z component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Z = new OutputSlotInt();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Z.SetValue(_Vector3Int.value.z);
		}
	}
}