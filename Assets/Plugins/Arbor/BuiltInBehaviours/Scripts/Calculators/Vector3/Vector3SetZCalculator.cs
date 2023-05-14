//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3のZ成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the Z component of Vector3.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.SetZ")]
	[BehaviourTitle("Vector3.SetZ")]
	[BuiltInBehaviour]
	public sealed class Vector3SetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3
		/// </summary>
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z成分
		/// </summary>
#else
		/// <summary>
		/// Z component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Z = new FlexibleFloat();

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
			Vector3 value = _Vector3.value;
			value.z = _Z.value;
			_Result.SetValue(value);
		}
	}
}
