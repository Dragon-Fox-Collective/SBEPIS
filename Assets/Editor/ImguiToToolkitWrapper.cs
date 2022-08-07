using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SBEPIS.Utils
{
	[CustomEditor(typeof(MonoBehaviour), true)]
	public class ImguiToToolkitWrapper : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			var root = new VisualElement();

			var prop = serializedObject.GetIterator();
			if (prop.NextVisible(true))
			{
				do
				{
					var field = new PropertyField(prop);

					if (prop.name == "m_Script")
					{
						field.SetEnabled(false);
					}

					root.Add(field);
				}
				while (prop.NextVisible(false));
			}

			return root;
		}
	}
}
