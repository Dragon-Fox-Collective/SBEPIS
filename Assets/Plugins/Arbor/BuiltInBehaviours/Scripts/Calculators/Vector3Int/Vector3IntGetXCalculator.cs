//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3IntのX成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the X component of Vector3Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.GetX")]
	[BehaviourTitle("Vector3Int.GetX")]
	[BuiltInBehaviour]
	public sealed class Vector3IntGetXCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3Int
		/// </summary>
		[SerializeField] private FlexibleVector3Int _Vector3Int = new FlexibleVector3Int();

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
			_X.SetValue(_Vector3Int.value.x);
		}
	}
}