//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	public static class Styles
	{
		// EditorResources/EditorSkin
		public static readonly GUIStyle addBehaviourButton;
		public static readonly GUIStyle nodeCommentField;
		public static readonly GUIStyle dropField;
		public static readonly GUIStyle nodeDataOutPin;
		public static readonly GUIStyle nodeDataInPin;
		public static readonly GUIStyle nodeDataArrayOutPin;
		public static readonly GUIStyle nodeDataArrayInPin;
		public static readonly GUIStyle popupIconButton;
		public static readonly GUIStyle visibilityToggle;
		public static readonly GUIStyle dataLinkSlot;
		public static readonly GUIStyle dataLinkSlotActive;

		private static GUISkin _Skin = null;
		private static readonly Dictionary<string, GUIStyle> _StyleCache;

		private static GUISkin skin
		{
			get
			{
				if (_Skin == null)
				{
					_Skin = EditorResources.Load<GUISkin>("EditorSkin", ".guiskin");
				}
				return _Skin;
			}
		}

		static Styles()
		{
			_StyleCache = new Dictionary<string, GUIStyle>();

			// EditorResources/EditorSkin
			addBehaviourButton = GetStyle("add behaviour button");
			nodeCommentField = GetStyle("node comment");
			dropField = GetStyle("drop field");
			nodeDataOutPin = GetStyle("node data out pin");
			nodeDataInPin = GetStyle("node data in pin");
			nodeDataArrayOutPin = GetStyle("node data array out pin");
			nodeDataArrayInPin = GetStyle("node data array in pin");
			dataLinkSlot = GetStyle("data link slot");
			dataLinkSlotActive = GetStyle("data link slot active");
			
			popupIconButton = new GUIStyle(GUIStyle.none);
			popupIconButton.alignment = TextAnchor.MiddleCenter;
			popupIconButton.fixedHeight = 16f;
			popupIconButton.fixedWidth = 16f;

			visibilityToggle = new GUIStyle();
			visibilityToggle.normal.background = EditorContents.visibilityToggleOffTexture;
			visibilityToggle.onNormal.background = EditorContents.visibilityToggleOnTexture;

			visibilityToggle.fixedHeight = EditorContents.visibilityToggleOffTexture.height;
			visibilityToggle.fixedWidth = EditorContents.visibilityToggleOffTexture.width;
			visibilityToggle.border = new RectOffset(2, 2, 2, 2);
			visibilityToggle.padding = new RectOffset(3, 3, 3, 3);
			visibilityToggle.overflow = new RectOffset(-1, 1, -2, 2);
		}

		public static GUIStyle GetStyle(string name)
		{
			GUIStyle style = null;
			if (!_StyleCache.TryGetValue(name, out style))
			{
				style = skin.FindStyle(name);
				if (style != null)
				{
					_StyleCache.Add(name, style);
				}
			}
			return style;
		}

		public static string GetNodeFrameClassName(bool on, bool active)
		{
			return string.Format("node-frame{0}{1}", (!on) ? string.Empty : "-on", (!active) ? string.Empty : "-active");
		}

		public static string GetNodeBaseClassName(Styles.BaseColor color)
		{
			return string.Format("node-base-{0}", (int)color);
		}

		public static string GetNodeHeaderClassName(Styles.Color color)
		{
			return string.Format("node-header-{0}", (int)color);
		}

		public enum BaseColor
		{
			Gray = 0,
			Grey = 0,
			White = 1,
		}

		public enum Color
		{
			Gray = 0,
			Grey = 0,
			Blue = 1,
			Aqua = 2,
			Green = 3,
			Yellow = 4,
			Orange = 5,
			Red = 6,
			Purple = 7,
			White = 8,
		}

		private static readonly Dictionary<Styles.Color, UnityEngine.Color> s_ColorDictionary = new Dictionary<Color, UnityEngine.Color>()
		{
			{ Styles.Color.Gray,new UnityEngine.Color(0.8f, 0.8f, 0.8f) },
			{ Styles.Color.Blue,new UnityEngine.Color(0.2f, 0.7f, 1.0f) },
			{ Styles.Color.Aqua,new UnityEngine.Color(0.5f, 1.0f, 1.0f) },
			{ Styles.Color.Green,UnityEngine.Color.green },
			{ Styles.Color.Yellow,UnityEngine.Color.yellow },
			{ Styles.Color.Orange,new UnityEngine.Color(1.0f, 0.5f, 0.0f) },
			{ Styles.Color.Red,UnityEngine.Color.red },
			{ Styles.Color.Purple,new UnityEngine.Color(0.9f,0.5f, 1.0f) },
			{ Styles.Color.White,UnityEngine.Color.white },
		};

		public static UnityEngine.Color GetColor(Styles.Color styleColor)
		{
			UnityEngine.Color color = UnityEngine.Color.white;
			if (s_ColorDictionary.TryGetValue(styleColor, out color))
			{
				return color;
			}
			return UnityEngine.Color.white;
		}

		public static GUIStyle GetDataInPin(System.Type type)
		{
			if (type != null && DataSlotGUIUtility.IsList(type))
			{
				return nodeDataArrayInPin;
			}
			return nodeDataInPin;
		}

		public static GUIStyle GetDataOutPin(System.Type type)
		{
			if (type != null && DataSlotGUIUtility.IsList(type))
			{
				return nodeDataArrayOutPin;
			}
			return nodeDataOutPin;
		}
	}
}
