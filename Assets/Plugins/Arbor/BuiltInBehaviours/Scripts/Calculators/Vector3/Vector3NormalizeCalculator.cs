//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3を正規化したベクトル
	/// </summary>
#else
	/// <summary>
	/// Vector3 normalized vector
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.Normalize")]
	[BehaviourTitle("Vector3.Normalize")]
	[BuiltInBehaviour]
	public sealed class Vector3NormalizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3
		/// </summary>
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

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
			_Result.SetValue(_Vector3.value.normalized);
		}
	}
}
