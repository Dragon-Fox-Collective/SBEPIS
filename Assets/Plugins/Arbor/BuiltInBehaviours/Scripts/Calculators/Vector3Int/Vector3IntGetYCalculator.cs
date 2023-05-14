//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3IntのY成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the Y component of Vector3Int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3Int/Vector3Int.GetY")]
	[BehaviourTitle("Vector3Int.GetY")]
	[BuiltInBehaviour]
	public sealed class Vector3IntGetYCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3Int
		/// </summary>
		[SerializeField] private FlexibleVector3Int _Vector3Int = new FlexibleVector3Int();

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
			_Y.SetValue(_Vector3Int.value.y);
		}
	}
}