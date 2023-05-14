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
	/// Behavior of setting VariableExampleData to UI
	/// </summary>
	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/VariableExampleBehaviour")]
	public sealed class VariableExampleBehaviour : StateBehaviour
	{
		/// <summary>
		/// Image component displaying Icon
		/// </summary>
		[SerializeField]
		private Image _IconImage = null;

		/// <summary>
		/// Text component to display Name
		/// </summary>
		[SerializeField]
		private Text _NameText = null;

		/// <summary>
		/// Input field of VariableExampleData
		/// </summary>
		[SerializeField]
		private FlexibleVariableExampleData _ExampleData = new FlexibleVariableExampleData(new VariableExampleData());

		/// <summary>
		/// Called when entering a state.
		/// </summary>
		public override void OnStateBegin()
		{
			// Get VariableExample from FlexibleVariableExampleData
			VariableExampleData exampleData = _ExampleData.value;
			if (exampleData != null)
			{
				// Set icon and name.
				_IconImage.sprite = exampleData.icon;
				_NameText.text = exampleData.name;
			}
		}
	}
}
#endif