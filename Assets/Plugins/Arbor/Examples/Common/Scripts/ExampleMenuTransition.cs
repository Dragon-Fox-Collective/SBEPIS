//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.Examples
{
	/// <summary>
	/// Example of transition by menu (Legacy GUI)
	/// </summary>
	[AddComponentMenu("")]
	[AddBehaviourMenu("Examples/Menu")]
	public sealed class ExampleMenuTransition : StateBehaviour
	{
		/// <summary>
		/// Menu name
		/// </summary>
		[Multiline(3)]
		[SerializeField]
		private string _MenuName = string.Empty;

		/// <summary>
		/// Menu item
		/// </summary>
		[System.Serializable]
		private sealed class Item
		{
			/// <summary>
			/// Menu item name
			/// </summary>
			public string name = "";

			/// <summary>
			/// Transition link
			/// </summary>
			public StateLink link = new StateLink();
		}

		/// <summary>
		/// Menu item list
		/// </summary>
		[SerializeField]
		private List<Item> _Items = new List<Item>();

		void OnGUI()
		{
			// Display menu name
			GUILayout.Label(_MenuName);

			int itemCount = _Items.Count;
			for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
			{
				Item item = _Items[itemIndex];
				// Press the button to make a transition.
				if (GUILayout.Button(item.name))
				{
					Transition(item.link);
				}
			}
		}
	}
}
