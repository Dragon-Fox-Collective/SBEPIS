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
	/// Example behaviour that uses the DataLink attribute
	/// </summary>
	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/DataLinkExampleBehaviour")]
	public sealed class DataLinkExampleBehaviour : StateBehaviour
	{
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
		/// Data field with DataLink attribute
		/// </summary>
		[SerializeField]
		[DataLink]
		private DataLinkExampleData _ExampleData = new DataLinkExampleData();

		/// <summary>
		/// Called when entering a state.
		/// </summary>
		public override void OnStateBegin()
		{
			// Set the current state name to StateText.
			_StateText.text = state.name;
		}

		/// <summary>
		/// Called when updating a state.
		/// </summary>
		public override void OnStateUpdate()
		{
			// Set the current example data to ValueText.
			_ValueText.text = _ExampleData.ToString();
		}
	}
}
#endif