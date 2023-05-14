//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4のZ成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the Z component of Vector4.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.GetZ")]
	[BehaviourTitle("Vector4.GetZ")]
	[BuiltInBehaviour]
	public sealed class Vector4GetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector4
		/// </summary>
		[SerializeField] private FlexibleVector4 _Vector4 = new FlexibleVector4();

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
			_Z.SetValue(_Vector4.value.z);
		}
	}
}
