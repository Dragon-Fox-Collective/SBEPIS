//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector2IntのX成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the X component of Vector2Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2Int/Vector2Int.GetX")]
	[BehaviourTitle("Vector2Int.GetX")]
	[BuiltInBehaviour]
	public sealed class Vector2IntGetXCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector2Int
		/// </summary>
		[SerializeField] private FlexibleVector2Int _Vector2Int = new FlexibleVector2Int();

#if ARBOR_DOC_JA
		/// <summary>
		/// X成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output X component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _X = new OutputSlotInt();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_X.SetValue(_Vector2Int.value.x);
		}
	}
}