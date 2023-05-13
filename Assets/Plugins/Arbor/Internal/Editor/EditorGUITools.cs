//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.UnityEditorBridge.UIElements;

	public static class EditorGUITools
	{
		public const float kDropDownWidth = 13f;
		public const float kSubtractDropdownWidth = 14f;

		public const float kPopupWidth = 16f;
		public const float kSubtrackPopupWidth = 20f;

		public static GUIContent GetHelpBoxContent(string message, MessageType type)
		{
			GUIContent content = GUIContentCaches.Get(message);
			content.image = UnityEditorBridge.EditorGUIUtilityBridge.GetHelpIcon(type);

			return content;
		}

		public static float GetHelpBoxHeight(string message, MessageType type, float width)
		{
			GUIContent content = GetHelpBoxContent(message, type);

			return EditorStyles.helpBox.CalcHeight(content, width);
		}

		public static float GetHelpBoxHeight(string message, MessageType type)
		{
			GUIContent content = GetHelpBoxContent(message, type);

			Vector2 contentSize = EditorStyles.helpBox.CalcSize(content);
			return EditorStyles.helpBox.CalcScreenSize(contentSize).y;
		}

		public static string DropdownSearchField(string searchWord)
		{
			Rect rect;
			
			const float kBorderWidth = 1f;
			rect = GUILayoutUtility.GetRect(0, BuiltInStyles.toolbarSearchField.fixedHeight, BuiltInStyles.toolbarSearchField);

			rect.height = BuiltInStyles.toolbarSearchField.fixedHeight;
			rect.xMin += kBorderWidth;
			rect.xMax -= kBorderWidth;
			rect.yMin += kBorderWidth;

			return UnityEditorBridge.EditorGUIBridge.ToolbarSearchField(rect, searchWord, false);
		}

		public static Texture2D FindTexture(System.Type type)
		{
			Texture2D tex = UnityEditorBridge.EditorGUIUtilityBridge.FindTexture(type);
			if (tex != null)
			{
				return tex;
			}

			return AssetPreview.GetMiniTypeThumbnail(type);
		}

		[System.Obsolete("use GUIContentCaches.Get")]
		public static GUIContent GetTextContent(string key)
		{
			return GUIContentCaches.Get(key);
		}

		private static Dictionary<Object, GUIContent> _ThumbnailContents = new Dictionary<Object, GUIContent>();

		[InitializeOnEnterPlayMode]
		static void OnEnterPlayMode()
		{
			_ThumbnailContents.Clear();
		}

		public static GUIContent GetThumbnailContent(Object obj)
		{
			if (obj == null)
			{
				return GUIContent.none;
			}

			GUIContent content = null;
			if (!_ThumbnailContents.TryGetValue(obj, out content))
			{
				content = new GUIContent(AssetPreview.GetMiniThumbnail(obj));
				_ThumbnailContents.Add(obj, content);
			}

			return content;
		}

		private static Dictionary<string, string> _NicifyVariableNames = new Dictionary<string, string>();
		public static string NicifyVariableName(string name)
		{
			string value;
			if (!_NicifyVariableNames.TryGetValue(name, out value))
			{
				value = ObjectNames.NicifyVariableName(name);
				_NicifyVariableNames.Add(name, value);
			}

			return value;
		}

		public static Color GetColorOnGUI(Color color)
		{
			return color * GUI.color;
		}

		public static void DrawLines(Texture2D tex, Color color, float width, params Vector3[] points)
		{
			if (Event.current.type != EventType.Repaint)
				return;

			Color tempColor = Handles.color;
			Handles.color = GetColorOnGUI(color);
			Handles.DrawAAPolyLine(tex, width, points);
			Handles.color = tempColor;
		}

		public const float kBranchArrowWidth = 16.0f;

		const int s_QuadVertexNum = 4;

		static void GenerateSolidQuad(Vector3 center, float width, float angle, Color color, List<Vector3> vertices, List<Color> colors, List<ushort> triangles, List<Vector2> texcoords)
		{
			Quaternion rotate = Quaternion.Euler(0, 0, angle);

			int vertCount = vertices.Count;

			vertices.Add(center + (rotate * new Vector2(-0.5f, -0.5f)) * width);
			vertices.Add(center + (rotate * new Vector2(0.5f, -0.5f)) * width);
			vertices.Add(center + (rotate * new Vector2(-0.5f, 0.5f)) * width);
			vertices.Add(center + (rotate * new Vector2(0.5f, 0.5f)) * width);

			texcoords.Add(new Vector2(0.0f, 1.0f));
			texcoords.Add(new Vector2(1.0f, 1.0f));
			texcoords.Add(new Vector2(0.0f, 0.0f));
			texcoords.Add(new Vector2(1.0f, 0.0f));

			colors.Add(color);
			colors.Add(color);
			colors.Add(color);
			colors.Add(color);

			triangles.Add((ushort)(vertCount + 0));
			triangles.Add((ushort)(vertCount + 1));
			triangles.Add((ushort)(vertCount + 2));
			triangles.Add((ushort)(vertCount + 2));
			triangles.Add((ushort)(vertCount + 1));
			triangles.Add((ushort)(vertCount + 3));
		}

		public class MeshData
		{
			public List<Vector3> vertices = new List<Vector3>();
			public List<Color> colors = new List<Color>();
			public List<Vector2> texcoords = new List<Vector2>();
			public List<ushort> triangles = new List<ushort>();

			public void Clear()
			{
				vertices.Clear();
				colors.Clear();
				texcoords.Clear();
				triangles.Clear();
			}

			public void DrawGL()
			{
				GL.Begin(GL.TRIANGLES);
				
				for (int count = triangles.Count, i = 0; i < count; i++)
				{
					ushort index = triangles[i];

					GL.Color(colors[index]);
					GL.TexCoord(texcoords[index]);
					GL.Vertex(vertices[index]);
				}

				GL.End();
			}

			public void Allocate(MeshGenerationContext mgc, Texture tex)
			{
				var md = mgc.Allocate(vertices.Count, triangles.Count, tex);

#if !UNITY_2023_1_OR_NEWER
				Rect uvRegion = md.uvRegion;
#endif

				for (int count = md.vertexCount, i = 0; i < count; i++)
				{
					Vector3 position = vertices[i];
					position.z = Vertex.nearZ;

					Vector2 texcoord = texcoords[i];
#if !UNITY_2023_1_OR_NEWER
					texcoord.x *= uvRegion.width;
					texcoord.y *= uvRegion.height;
					texcoord += uvRegion.min;
#endif

					md.SetNextVertex(new Vertex()
					{
						position = position,
						uv = texcoord,
						tint = colors[i] * UIElementsUtilityBridge.editorPlayModeTintColor,
					});
				}

				for (int count = md.indexCount, i = 0; i < count; i++)
				{
					md.SetNextIndex(triangles[i]);
				}
			}
		}

		public static void GeneratePolyLineMesh(IList<Vector2> points, Color lineColor, float edgeWidth, bool loop, MeshData meshData)
		{
			using (new ProfilerScope("GeneratePolyLineMesh"))
			{
				if (points.Count < 2)
				{
					return;
				}

				float halfWidth = edgeWidth * 0.5f;

				Vector2 prev = points[0];
				int count = points.Count;
				if (loop)
				{
					prev = points[points.Count - 1];
					count++;
				}

				int vertexCount = count * 2;
				int indexCount = (vertexCount - 2) * 3;

				for (int i = 0; i < count; i++)
				{
					int index = i % points.Count;
					Vector2 point = points[index];
					
					Vector2 next = point;
					if (index + 1 < points.Count)
					{
						next = points[index + 1];
					}
					else if(loop)
					{
						next = points[0];
					}
					Vector2 prevDir = (point - prev).normalized;
					Vector2 nextDir = (next - point).normalized;

					Vector2 tangent = (nextDir + prevDir).normalized;
					Vector2 normal = Vector2.Perpendicular(tangent);
					Color color = lineColor;

					Vector2 edge = (index + 1 < points.Count || loop)?Vector2.Perpendicular(nextDir) : Vector2.Perpendicular(prevDir);
					float d = Vector2.Dot(normal, edge);
					float width = halfWidth / d;
					
					meshData.colors.Add(color);
					meshData.texcoords.Add(new Vector2(0.5f, 0.0f));
					meshData.vertices.Add(point + normal * width);

					meshData.colors.Add(color);
					meshData.texcoords.Add(new Vector2(0.5f, 1.0f));
					meshData.vertices.Add(point - normal * width);

					prev = point;
				}

				for (uint i = 0; i < vertexCount - 2; ++i)
				{
					if ((i & 0x01) == 0)
					{
						meshData.triangles.Add((ushort)i);
						meshData.triangles.Add((ushort)(i + 1));
						meshData.triangles.Add((ushort)(i + 2));
					}
					else
					{
						meshData.triangles.Add((ushort)i);
						meshData.triangles.Add((ushort)(i + 2));
						meshData.triangles.Add((ushort)(i + 1));
					}
				}
			}
		}

		public static void GenerateBezierMesh(Bezier2D bezier, Vector2 offset, Color startColor, Color endColor, float edgeWidth, int division, MeshData meshData)
		{
			using (new ProfilerScope("GenerateBezierMesh"))
			{
				float halfWidth = edgeWidth * 0.5f;

				int vertexCount = division * 2;
				int indexCount = (vertexCount - 2) * 3;

				for (int i = 0; i < division; i++)
				{
					float t = (float)i / (float)(division - 1);
					Vector2 point = bezier.GetPoint(t);
					Vector2 tangent = bezier.GetTangent(t);
					Vector2 edge = Vector2.Perpendicular(tangent);
					Color color = Color.Lerp(startColor, endColor, t);

					meshData.colors.Add(color);
					meshData.texcoords.Add(new Vector2(0.5f, 0.0f));
					meshData.vertices.Add(point + edge * halfWidth + offset);

					meshData.colors.Add(color);
					meshData.texcoords.Add(new Vector2(0.5f, 1.0f));
					meshData.vertices.Add(point - edge * halfWidth + offset);
				}

				for (uint i = 0; i < vertexCount - 2; ++i)
				{
					if ((i & 0x01) == 0)
					{
						meshData.triangles.Add((ushort)i);
						meshData.triangles.Add((ushort)(i + 1));
						meshData.triangles.Add((ushort)(i + 2));
					}
					else
					{
						meshData.triangles.Add((ushort)i);
						meshData.triangles.Add((ushort)(i + 2));
						meshData.triangles.Add((ushort)(i + 1));
					}
				}
			}
		}

		public static void GenerateBezierDottedQuadMesh(Bezier2D bezier, Color startColor, Color endColor, float edgeLength, float space, bool shadow, Vector2 shadowOffset, Color shadowColor, MeshData meshData)
		{
			using (new ProfilerScope("GenerateBezierDottedQuadMesh"))
			{
				meshData.Clear();

				int vertexPerQuad = s_QuadVertexNum;
				if (shadow)
				{
					vertexPerQuad *= 2;
				}
				int maxVertex = 65000;
				int maxQuadCount = maxVertex / vertexPerQuad;

				float bezierLength = bezier.length;

				int dotCount = (int)(bezierLength / space);
				if (dotCount >= maxQuadCount)
				{
					space = bezierLength / maxQuadCount;
				}

				for (float l = 0.0f; l <= bezierLength; l += space)
				{
					float tl = l / bezierLength;
					float t = bezier.LinearToInterpolationParam(tl);
					Vector2 point = bezier.GetPoint(t);
					Vector2 tangent = bezier.GetTangent(t);

					float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

					if (shadow)
					{
						GenerateSolidQuad(point + shadowOffset, edgeLength, angle, shadowColor, meshData.vertices, meshData.colors, meshData.triangles, meshData.texcoords);
					}
					GenerateSolidQuad(point, edgeLength, angle, Color.Lerp(startColor, endColor, tl), meshData.vertices, meshData.colors, meshData.triangles, meshData.texcoords);
				}
			}
		}

		static readonly MeshData s_GenerateMeshData = new MeshData();

		public static void GeneratePolyLine(MeshGenerationContext mgc, IList<Vector2> points, Color lineColor, float edgeWidth, bool loop,　Texture tex)
		{
			s_GenerateMeshData.Clear();

			GeneratePolyLineMesh(points, lineColor, edgeWidth, loop, s_GenerateMeshData);

			s_GenerateMeshData.Allocate(mgc, tex);
		}

		public static void GenerateBezier(MeshGenerationContext mgc, Bezier2D bezier, Vector2 offset, Color startColor, Color endColor, float edgeWidth, int division, Texture tex)
		{
			s_GenerateMeshData.Clear();

			GenerateBezierMesh(bezier, offset, startColor, endColor, edgeWidth, division, s_GenerateMeshData);

			s_GenerateMeshData.Allocate(mgc, tex);
		}

		public static void GenerateDottedBezier(MeshGenerationContext mgc, Bezier2D bezier, Color startColor, Color endColor, float edgeLength, float space, bool shadow, Vector2 shadowOffset, Color shadowColor, Texture tex)
		{
			s_GenerateMeshData.Clear();

			GenerateBezierDottedQuadMesh(bezier, startColor, endColor, edgeLength, space, shadow, shadowOffset, shadowColor, s_GenerateMeshData);

			s_GenerateMeshData.Allocate(mgc, tex);
		}

		static readonly Color s_IntColor = Color.cyan;
		static readonly Color s_BoolColor = Color.red;
		static readonly Color s_StringColor = Color.magenta;
		static readonly Color s_FloatColor = new Color(0.5f, 1, 0.5f);
		static readonly Color s_Vector2Color = new Color(1, 0.5f, 0);
		static readonly Color s_Vector3Color = Color.yellow;
		static readonly Color s_QuaternionColor = new Color(0.5f, 1.0f, 0);
		static readonly Color s_UnityObjectColor = new Color(0, 0.5f, 1);
		static readonly Color s_AnyColor = Color.white;
		static readonly Color s_OtherColor = new Color(0.7f, 0.5f, 1.0f);

		public static Color GetTypeColor(System.Type type)
		{
			if (type != null && TypeUtility.IsGeneric(type, typeof(System.Nullable<>)))
			{
				type = type.GetGenericArguments()[0];
			}

			if (type != null)
			{
				type = DataSlotGUIUtility.ElementType(type);
			}

			if (type == null || type == typeof(object))
			{
				return s_AnyColor;
			}
			else if (type == typeof(int) || type == typeof(long))
			{
				return s_IntColor;
			}
			else if (type == typeof(bool))
			{
				return s_BoolColor;
			}
			else if (type == typeof(string))
			{
				return s_StringColor;
			}
			else if (type == typeof(float))
			{
				return s_FloatColor;
			}
			else if (type == typeof(Vector2))
			{
				return s_Vector2Color;
			}
			else if (type == typeof(Vector3))
			{
				return s_Vector3Color;
			}
			else if (type == typeof(Quaternion))
			{
				return s_QuaternionColor;
			}
			else if (type == typeof(Vector2Int))
			{
				return s_Vector2Color;
			}
			else if (type == typeof(Vector3Int))
			{
				return s_Vector3Color;
			}
			else if (typeof(Object).IsAssignableFrom(type))
			{
				return s_UnityObjectColor;
			}

			return s_OtherColor;
		}

		static readonly Color s_SlotBackgroundDarkColor = new Color(0.25f, 0.25f, 0.25f, 1f);
		static readonly Color s_SlotBackgroundLightColor = Color.white;

		public static Color GetSlotBackgroundColor(Color slotColor, bool isActive, bool on)
		{
			slotColor.a = 1f;

			if (isActive)
			{
				return Color.Lerp(slotColor, s_SlotBackgroundLightColor, 0.8f);
			}

			if (!on)
			{
				return Color.Lerp(slotColor, s_SlotBackgroundDarkColor, 0.5f);
			}

			return slotColor;
		}

		static readonly Color kSplitColorDark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
		static readonly Color kSplitColorLight = new Color(0.6f, 0.6f, 0.6f, 1.333f);

		public static Color GetSplitColor()
		{
			return EditorGUIUtility.isProSkin ? kSplitColorDark : kSplitColorLight;
		}

		private static Color kIconColorDark = new Color(0.77f, 0.77f, 0.77f, 1.0f);
		private static Color kIconColorLight = new Color(0.33f, 0.33f, 0.33f, 1.0f);

		public static Color GetIconColor()
		{
			return EditorGUIUtility.isProSkin ? kIconColorDark : kIconColorLight;
		}

		public static Color GetConditionColor(ConditionResult condition)
		{
			switch (condition)
			{
				case ConditionResult.None:
					return Color.gray;
				case ConditionResult.Success:
					return Color.green;
				case ConditionResult.Failure:
					return Color.red;
			}

			return Color.clear;
		}

		public static void DrawSeparator()
		{
			Rect rect = GUILayoutUtility.GetRect(0.0f, 1.0f);

			if (Event.current.type == EventType.Repaint)
			{
				EditorGUI.DrawRect(rect, GetSplitColor());
			}
		}

		public static readonly Color gridMinorColor = new Color(0.5f, 0.5f, 0.5f, 0.18f);
		public static readonly Color gridMajorColor = new Color(0.5f, 0.5f, 0.5f, 0.28f);

		public const float kStateBezierTargetOffsetY = 12.0f;
		public const float kBezierTangent = 50f;
		public static readonly Vector2 kBezierTangentOffset = new Vector2(kBezierTangent, 0.0f);

		public static Rect GetDropdownRect(Rect position)
		{
			position.xMin = position.xMax - kDropDownWidth;
			position.height = EditorGUIUtility.singleLineHeight;
			return position;
		}

		public static Rect SubtractDropdownWidth(Rect position)
		{
			position.width -= kSubtractDropdownWidth;
			return position;
		}

		public static Rect GetPopupRect(Rect position)
		{
			position.xMin = position.xMax - kPopupWidth;
			position.height = EditorGUIUtility.singleLineHeight;
			return position;
		}

		public static Rect SubtractPopupWidth(Rect position)
		{
			position.width -= kSubtrackPopupWidth;
			return position;
		}

		public static Rect PrefixLabel(Rect totalPosition, GUIContent label)
		{
			Rect labelPosition = EditorGUI.IndentedRect(new Rect(totalPosition.x, totalPosition.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight));
			Rect rect = new Rect(totalPosition.x + EditorGUIUtility.labelWidth, totalPosition.y, totalPosition.width - EditorGUIUtility.labelWidth, totalPosition.height);
			EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, 0, EditorStyles.label);
			return rect;
		}

		public static MonoScript GetMonoScript(Object obj)
		{
			MonoBehaviour monoBehaviour = obj as MonoBehaviour;
			if (monoBehaviour != null)
			{
				return MonoScript.FromMonoBehaviour(monoBehaviour);
			}
			ScriptableObject scriptableObject = obj as ScriptableObject;
			if (scriptableObject != null)
			{
				return MonoScript.FromScriptableObject(scriptableObject);
			}

			if (obj != null && !(obj is Editor))
			{
				using (SerializedObject serializedObject = new SerializedObject(obj))
				{
					var scriptProperty = serializedObject.FindProperty("m_Script");
					return scriptProperty.objectReferenceValue as MonoScript;
				}
			}

			return null;
		}

		public static float SnapToGrid(float x)
		{
			if (ArborSettings.showGrid && ArborSettings.snapGrid)
			{
				float gridSizeMinor = ArborSettings.gridSize / (float)ArborSettings.gridSplitNum;
				int num1 = Mathf.FloorToInt(x / gridSizeMinor);
				x = num1 * gridSizeMinor;
			}
			return x;
		}

		public static Vector2 SnapToGrid(Vector2 position)
		{
			if (ArborSettings.showGrid && ArborSettings.snapGrid)
			{
				float gridSizeMinor = ArborSettings.gridSize / (float)ArborSettings.gridSplitNum;
				int num1 = Mathf.FloorToInt(position.x / gridSizeMinor);
				int num2 = Mathf.FloorToInt(position.y / gridSizeMinor);
				position.x = num1 * gridSizeMinor;
				position.y = num2 * gridSizeMinor;
			}
			return position;
		}

		public static float GetSnapSpace()
		{
			float space = 10f;
			if (ArborSettings.showGrid && ArborSettings.snapGrid)
			{
				space = ArborSettings.gridSize / (float)ArborSettings.gridSplitNum;
			}

			return space;
		}

		public static Rect SnapPositionToGrid(Rect position)
		{
			position.position = SnapToGrid(position.position);
			return position;
		}

		private static void DrawLine(Vector2 p1, Vector2 p2)
		{
			GL.Vertex((Vector3)p1);
			GL.Vertex((Vector3)p2);
		}

		private static void DrawGridLines(Rect rect, float gridSize, Color gridColor)
		{
			GL.Color(gridColor * GUI.color);

			float xMin = rect.xMin - rect.xMin % gridSize;
			int gridWidth = Mathf.CeilToInt(rect.width / gridSize);
			for (int i = 0; i <= gridWidth; i++)
			{
				float x = xMin + i * gridSize;
				DrawLine(new Vector2(x, rect.yMin), new Vector2(x, rect.yMax));
			}

			GL.Color(gridColor * GUI.color);
			float yMin = rect.yMin - rect.yMin % gridSize;
			int gridHeight = Mathf.CeilToInt(rect.height / gridSize);
			for (int i = 0; i < gridHeight; i++)
			{
				float y = yMin + i * gridSize;
				DrawLine(new Vector2(rect.xMin, y), new Vector2(rect.xMax, y));
			}
		}

		public static void DrawGrid(Rect rect, float zoomLevel, float gridSize, int gridSplitNum)
		{
			using (new ProfilerScope("DrawGrid"))
			{
				UnityEditorBridge.HandleUtilityBridge.ApplyWireMaterial();
				GL.PushMatrix();
				GL.Begin(GL.LINES);

				float t = Mathf.InverseLerp(0.1f, 1, zoomLevel);

				if (gridSplitNum > 1 && t > 0f)
				{
					DrawGridLines(rect, gridSize / (float)gridSplitNum, Color.Lerp(Color.clear, gridMinorColor, t));
				}
				DrawGridLines(rect, gridSize, Color.Lerp(gridMinorColor, gridMajorColor, t));

				GL.End();
				GL.PopMatrix();
			}
		}

		public static void DrawGrid(Rect rect, float zoomLevel)
		{
			DrawGrid(rect, zoomLevel, ArborSettings.gridSize, ArborSettings.gridSplitNum);
		}

		public static void OpenAssetStore(string storeURL)
		{
			storeURL = storeURL.Substring(storeURL.IndexOf("content", System.StringComparison.Ordinal));
			UnityEditorInternal.AssetStore.Open(storeURL);
		}

		public static TEnum EnumPopup<TEnum>(Rect position, GUIContent label, TEnum selected, GUIStyle style) where TEnum : System.Enum
		{
			var enumType = typeof(TEnum);

			if (!enumType.IsEnum)
			{
				throw new System.ArgumentException("Parameter selected must be of type System.Enum", "selected");
			}

			TEnum[] values = EnumUtility.GetValues<TEnum>();
			int selectedIndex = EnumUtility.GetIndexFromValue<TEnum>(selected);
			GUIContent[] displayOptions = EnumUtility.GetContents<TEnum>();

			selectedIndex = EditorGUI.Popup(position, label, selectedIndex, displayOptions, style);
			if (0 <= selectedIndex && selectedIndex < values.Length)
			{
				selected = EnumUtility.GetValueFromIndex<TEnum>(selectedIndex);
			}

			return selected;
		}

		public static TEnum EnumPopupUnIndent<TEnum>(Rect position, GUIContent label, TEnum selected, GUIStyle style) where TEnum : System.Enum
		{
			int tempIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			selected = EnumPopup(position, label, selected, style);

			EditorGUI.indentLevel = tempIndentLevel;
			return selected;
		}

		public static bool VisibilityToggle(Rect position, GUIContent label, bool toggle)
		{
			Rect toggleRect = position;
			toggleRect.width = Styles.visibilityToggle.fixedWidth;

			toggle = EditorGUI.Toggle(toggleRect, GUIContent.none, toggle, Styles.visibilityToggle);

			Rect setLabelRect = position;
			setLabelRect.xMin = toggleRect.xMax + 2f;

			EditorGUI.BeginDisabledGroup(!toggle);

			EditorGUI.LabelField(setLabelRect, label);

			EditorGUI.EndDisabledGroup();

			return toggle;
		}

		public static void TagField(Rect rect, SerializedProperty property, GUIContent label)
		{
			TagField(rect, property, label, EditorStyles.popup);
		}

		public static void TagField(Rect rect, SerializedProperty property, GUIContent label, GUIStyle style)
		{
			label = EditorGUI.BeginProperty(rect, label, property);

			EditorGUI.BeginChangeCheck();
			string tag = EditorGUI.TagField(rect, label, property.stringValue, style);
			if (EditorGUI.EndChangeCheck())
			{
				property.stringValue = tag;
			}

			EditorGUI.EndProperty();
		}

		public static bool ButtonForceEnabled(string text, params GUILayoutOption[] options)
		{
			return ButtonForceEnabled(text, GUI.skin.button, options);
		}

		public static bool ButtonForceEnabled(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			bool guiEnabled = GUI.enabled;
			GUI.enabled = true;

			bool button = GUILayout.Button(text, style, options);

			GUI.enabled = guiEnabled;

			return button;
		}

		static long RoundToLong(float f)
		{
			return (long)System.Math.Round(f);
		}

		public static long LongSlider(Rect position, long value, long leftValue, long rightValue, GUIContent label)
		{
			return RoundToLong(EditorGUI.Slider(position, label, value, leftValue, rightValue));
		}

		public static void LongSlider(Rect position, SerializedProperty property, long leftValue, long rightValue, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			EditorGUI.BeginChangeCheck();

			long newValue = LongSlider(position, property.longValue, leftValue, rightValue, label);

			if (EditorGUI.EndChangeCheck())
			{
				property.longValue = newValue;
			}

			EditorGUI.EndProperty();
		}
	}
}
