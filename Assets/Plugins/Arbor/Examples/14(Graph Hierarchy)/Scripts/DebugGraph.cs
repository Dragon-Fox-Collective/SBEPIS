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
	/// Debug class of Graph
	/// </summary>
	public sealed class DebugGraph
	{
		/// <summary>
		/// NodeGraph cache
		/// </summary>
		private NodeGraph _NodeGraph;

		/// <summary>
		/// Text cache
		/// </summary>
		private Text _Text;

		/// <summary>
		/// Next button cache
		/// </summary>
		private Button _NextButton;

		/// <summary>
		/// clicked on the Next button.
		/// </summary>
		public bool isNextClick
		{
			private set;
			get;
		}

		/// <summary>
		/// DebugGraph constructor
		/// </summary>
		/// <param name="nodeGraph">NodeGraph</param>
		public DebugGraph(NodeGraph nodeGraph)
		{
			// Cache NodeGraph
			_NodeGraph = nodeGraph;

			// Get UI from the root parameter container.
			ParameterContainerInternal rootParameterContainer = nodeGraph.rootGraph.parameterContainer;
			if (rootParameterContainer == null)
			{
				return;
			}

			Parameter textParameter = rootParameterContainer.GetParam("DebugText");
			if (textParameter != null)
			{
				_Text = textParameter.componentValue as Text;
			}

			Parameter buttonParameter = rootParameterContainer.GetParam("NextButton");
			if (buttonParameter != null)
			{
				_NextButton = buttonParameter.componentValue as Button;
			}
		}

		/// <summary>
		/// Initialize OnClick event
		/// </summary>
		public void InitializeEvent()
		{
			if (_NextButton != null)
			{
				_NextButton.onClick.AddListener(OnClickEvent);
			}
		}

		/// <summary>
		/// Release OnClick event
		/// </summary>
		public void ReleaseEvent()
		{
			if (_NextButton != null)
			{
				_NextButton.onClick.RemoveListener(OnClickEvent);
			}

			isNextClick = false;
		}

		void OnClickEvent()
		{
			isNextClick = true;
		}

		/// <summary>
		/// Output log
		/// </summary>
		/// <param name="message"></param>
		public void Log(string message)
		{
			message = string.Format("{0} : {1}", _NodeGraph, message);
			if (_Text != null)
			{
				_Text.text = message;
			}
			else
			{
				Debug.Log(message, _NodeGraph);
			}
		}
	}
}
#endif