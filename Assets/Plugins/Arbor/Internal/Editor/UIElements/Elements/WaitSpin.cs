//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	internal sealed class WaitSpin : VisualElement
	{
		static readonly Texture2D[] s_SpinTextures;

		static WaitSpin()
		{
			s_SpinTextures = new Texture2D[12];
			for (int index = 0; index < 12; ++index)
			{
				s_SpinTextures[index] = EditorGUIUtility.FindTexture("WaitSpin" + index.ToString("00"));
			}
		}

		private Image _Image;
		private int _Index;

		private IVisualElementScheduledItem _ExecuteSchedule;

		public WaitSpin()
		{
			_Image = new Image()
			{
				scaleMode = ScaleMode.ScaleToFit
			};
			hierarchy.Add(_Image);

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			if (_ExecuteSchedule == null)
			{
				_Index = 0;
				_Image.image = s_SpinTextures[_Index];

				_ExecuteSchedule = schedule.Execute(OnExecute).Every(100);
			}
			else
			{
				_ExecuteSchedule.Resume();
			}
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			_ExecuteSchedule?.Pause();
		}

		void OnExecute()
		{
			_Index = (_Index + 1) % s_SpinTextures.Length;
			_Image.image = s_SpinTextures[_Index];
		}
	}
}