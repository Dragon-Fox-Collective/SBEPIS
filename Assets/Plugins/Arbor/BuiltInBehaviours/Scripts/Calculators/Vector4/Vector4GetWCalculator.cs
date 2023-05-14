//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4のW成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the W component of Vector4.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.GetW")]
	[BehaviourTitle("Vector4.GetW")]
	[BuiltInBehaviour]
	public sealed class Vector4GetWCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector4
		/// </summary>
		[SerializeField] private FlexibleVector4 _Vector4 = new FlexibleVector4();

#if ARBOR_DOC_JA
		/// <summary>
		/// W成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output W component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _W = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_W.SetValue(_Vector4.value.w);
		}
	}
}
