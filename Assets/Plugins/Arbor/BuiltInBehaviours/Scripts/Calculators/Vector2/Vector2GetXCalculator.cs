//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2のX成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the X component of Vector2.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.GetX")]
	[BehaviourTitle("Vector2.GetX")]
	[BuiltInBehaviour]
	public sealed class Vector2GetXCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector2
		/// </summary>
		[SerializeField] private FlexibleVector2 _Vector2 = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// X成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output X component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _X = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_X.SetValue(_Vector2.value.x);
		}
	}
}
