//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector4を分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Vector4.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector4/Vector4.Decompose")]
	[BehaviourTitle("Vector4.Decompose")]
	[BuiltInBehaviour]
	public sealed class Vector4DecomposeCalculator : Calculator
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
		[SerializeField] private FlexibleVector4 _Input = new FlexibleVector4();

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

#if ARBOR_DOC_JA
		/// <summary>
		/// W座標の値
		/// </summary>
#else
		/// <summary>
		/// W coordinate value
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _W = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector4 v = _Input.value;
			_X.SetValue(v.x);
			_Y.SetValue(v.y);
			_Z.SetValue(v.z);
			_W.SetValue(v.w);
		}
	}
}
