//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3のY成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the Y component of Vector3.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.SetY")]
	[BehaviourTitle("Vector3.SetY")]
	[BuiltInBehaviour]
	public sealed class Vector3SetYCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3
		/// </summary>
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y成分
		/// </summary>
#else
		/// <summary>
		/// Y component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Y = new FlexibleFloat();

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
			value.y = _Y.value;
			_Result.SetValue(value);
		}
	}
}
