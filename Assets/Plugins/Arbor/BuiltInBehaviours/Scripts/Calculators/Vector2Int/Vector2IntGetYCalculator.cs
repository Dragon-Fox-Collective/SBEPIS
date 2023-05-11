//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2IntのY成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the Y component of Vector2Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.GetY")]
	[BehaviourTitle("Vector2Int.GetY")]
	[BuiltInBehaviour]
	public sealed class Vector2IntGetYCalculator : Calculator
	{
#region Serialize fields

		/// <summary>
		/// Vector2Int
		/// </summary>
		[SerializeField] private FlexibleVector2Int _Vector2Int = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Y component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Y = new OutputSlotInt();

#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Y.SetValue(_Vector2Int.value.y);
		}
	}
}