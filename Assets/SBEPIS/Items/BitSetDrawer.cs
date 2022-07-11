using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SBEPIS.Bits
{
	[CustomPropertyDrawer(typeof(BitSet))]
	public class BitSetDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			BitSetField field = new(property.displayName)
			{
				bindingPath = property.propertyPath,
			};
			return field;
		}
	}
}
