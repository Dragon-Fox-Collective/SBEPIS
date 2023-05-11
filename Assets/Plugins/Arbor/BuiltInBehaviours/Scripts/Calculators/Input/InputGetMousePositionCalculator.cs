//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// マウスの位置を取得する
	/// </summary>
#else
	/// <summary>
	/// Get the mouse position
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Input/Input.GetMousePosition")]
	[BehaviourTitle("Input.GetMousePosition")]
	[BuiltInBehaviour]
	public sealed class InputGetMousePositionCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 取得したマウスの位置を出力する。
		/// </summary>
#else
		/// <summary>
		/// Output the mouse position.
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotVector3 _Output = new OutputSlotVector3();

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(Input.mousePosition);
		}
	}
}
#endif