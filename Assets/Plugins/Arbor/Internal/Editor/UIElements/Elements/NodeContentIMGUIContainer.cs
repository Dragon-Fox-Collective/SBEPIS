//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	internal sealed class NodeContentIMGUIContainer : IMGUIContainer
	{
		private System.Action _OnGUIHandler;
		public new System.Action onGUIHandler
		{
			get
			{
				return _OnGUIHandler;
			}
			set
			{
				if (_OnGUIHandler != value)
				{
					_OnGUIHandler = value;
					MarkDirtyLayout();
					MarkDirtyRepaint();
				}
			}
		}

		public NodeContentIMGUIContainer(System.Action onGUIHandler)
		{
			base.onGUIHandler = OnGUI;
			this.onGUIHandler = onGUIHandler;
		}

		void OnGUI()
		{
			var savedMatrix = GUI.matrix;

			if (RenderTexture.active != null)
			{
				float scaling = 1f / EditorGUIUtility.pixelsPerPoint;
				Vector2 min = worldBound.min;
				Vector2 pos = -(min - min * scaling);
				GUI.matrix = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * scaling);
			}

			// When EventType.Layout, GUIClip.GetTopRect() is an invalid value, so overwrite it with BeginGroup.
			GUI.BeginGroup(contentRect);

			try
			{
				EditorGUILayout.BeginVertical();

				try
				{
					onGUIHandler?.Invoke();
				}
				catch (System.Exception ex)
				{
					if (UnityEditorBridge.GUIUtilityBridge.ShouldRethrowException(ex))
					{
#if ARBOR_DEBUG
						Debug.Log("catch ExitGUIException");
#endif
						throw;
					}
					else
					{
						Debug.LogException(ex);
					}
				}

				EditorGUILayout.Space(0); // margin

				EditorGUILayout.EndVertical();
			}
			finally
			{
				GUI.EndGroup();
				GUI.matrix = savedMatrix;
			}
		}
	}
}