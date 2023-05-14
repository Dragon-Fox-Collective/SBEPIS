//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.UI;

namespace Arbor.Examples
{
	/// <summary>
	/// Example of behaviour using data flow
	/// </summary>
	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/DataFlowExampleBehaviour")]
	public sealed class DataFlowExampleBehaviour : StateBehaviour
	{
		/// <summary>
		/// Input timing.
		/// </summary>
		public enum InputTiming
		{
			/// <summary>
			/// When you enter the state
			/// </summary>
			Enter,

			/// <summary>
			/// Always input
			/// </summary>
			Always,
		}

		/// <summary>
		/// Text that displays the current state
		/// </summary>
		[SerializeField]
		private Text _StateText = null;

		/// <summary>
		/// Text to display the current value
		/// </summary>
		[SerializeField]
		private Text _ValueText = null;

		/// <summary>
		/// Input timing.
		/// </summary>
		[SerializeField]
		private InputTiming _InputTiming = InputTiming.Enter;

		/// <summary>
		/// Data input slot
		/// </summary>
		[SerializeField]
		private InputSlotDataFlowExampleData _Input = new InputSlotDataFlowExampleData();

		private DataFlowExampleData _ExampleDataCache = new DataFlowExampleData();

		/// <summary>
		/// Update of input data
		/// </summary>
		/// <param name="isEnter">Is it a call from OnStateBegin?</param>
		void UpdateInputData(bool isEnter)
		{
			// OnStateBegin || (OnStateUpdate && _InputTiming == InputTiming.Always)
			if (isEnter || _InputTiming == InputTiming.Always)
			{
				// Calculator is re-calculated by the call of GetValue.
				_Input.GetValue(ref _ExampleDataCache);
			}
		}

		/// <summary>
		/// Called when entering a state.
		/// </summary>
		public override void OnStateBegin()
		{
			// Set the current state name to StateText.
			_StateText.text = state.name;

			// Update data when entering a state
			UpdateInputData(true);
		}

		/// <summary>
		/// Called when updating a state.
		/// </summary>
		public override void OnStateUpdate()
		{
			// Data update at state update timing
			UpdateInputData(false);

			// Set the current example data to ValueText.
			_ValueText.text = _ExampleDataCache.ToString();
		}
	}
}
#endif