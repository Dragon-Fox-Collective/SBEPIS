using UnityEditor;
using UnityEngine;

namespace SBEPIS.Bits
{
	[CustomPropertyDrawer(typeof(BitSet))]
	public class BitSetDrawer : PropertyDrawer
	{
		private static float lineHeight => EditorGUIUtility.singleLineHeight;
		private static float listHeight => lineHeight * BitManager.instance.bits.numBitsInCharacterGeneral * Mathf.Min(BitManager.instance.bits.numCharactersInCode, 2.5f);
		
		private Vector2 scrollPosition;
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return property.serializedObject.isEditingMultipleObjects ? 0 : lineHeight + (property.isExpanded ? listHeight : 0);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.serializedObject.isEditingMultipleObjects)
				return;
			
			if (property.boxedValue is null)
			{
				property.boxedValue = new BitSet();
				property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
			}
			
			BitList bitList = BitManager.instance.bits;
			BitSet bitSet = property.boxedValue as BitSet;
			string code = bitList.BitSetToCode(bitSet);
			
			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, lineHeight, lineHeight), property.isExpanded, GUIContent.none);

			string newCode = EditorGUI.DelayedTextField(new Rect(position.x + lineHeight, position.y, position.width - lineHeight, lineHeight), code);
			if (code != newCode)
			{
				property.boxedValue = bitList.BitSetFromCode(newCode);
				property.serializedObject.ApplyModifiedProperties();
			}

			if (property.isExpanded)
			{
				scrollPosition = GUI.BeginScrollView(new Rect(position.x, position.y + lineHeight, position.width, listHeight), scrollPosition, new Rect(0, 0, position.width - 14, lineHeight * BitManager.instance.bits.numBitsInCharacterGeneral * BitManager.instance.bits.numCharactersInCode));
				{
					for (int charIndex = 0; charIndex < bitList.numCharactersInCode; charIndex++)
					{
						GUI.Box(new Rect(0, lineHeight * bitList.numBitsInCharacterGeneral * charIndex, position.width, lineHeight * bitList.numBitsInCharacterGeneral), GUIContent.none);
						for (int bitIndex = 0; bitIndex < bitList.NumBitsInCharacterAt(charIndex); bitIndex++)
						{
							int index = charIndex * bitList.numCharactersInCode + bitIndex;
							Bit bit = bitList[index];
							bool hasBit = bitSet.Has(bit);
							bool newHasBit = EditorGUI.ToggleLeft(new Rect(4, lineHeight * index, position.width, lineHeight), bit.bitName, hasBit);
							if (hasBit != newHasBit)
							{
								if (newHasBit)
									property.boxedValue = property.boxedValue as BitSet | bit;
								else
									property.boxedValue = property.boxedValue as BitSet & (BitManager.instance.bits - bit);
								property.serializedObject.ApplyModifiedProperties();
							}
						}
					}
				}
				GUI.EndScrollView();
			}

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}
