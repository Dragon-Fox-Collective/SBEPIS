//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3を分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Vector3.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.Decompose")]
	[BehaviourTitle("Vector3.Decompose")]
	[BuiltInBehaviour]
	public sealed class Vector3DecomposeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力値
		/// </summary>
#else
		/// <summary>
		/// Input value
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Input = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// X座標の値
		/// </summary>
#else
		/// <summary>
		/// X coordinate value
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _X = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y座標の値
		/// </summary>
#else
		/// <summary>
		/// Y coordinate value
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Y = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z座標の値
		/// </summary>
#else
		/// <summary>
		/// Z coordinate value
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Z = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector3 v = _Input.value;
			_X.SetValue(v.x);
			_Y.SetValue(v.y);
			_Z.SetValue(v.z);
		}
	}
}
