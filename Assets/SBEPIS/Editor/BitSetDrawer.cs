using UnityEditor;
using UnityEngine;

namespace SBEPIS.Bits
{
	[CustomPropertyDrawer(typeof(BitSet))]
	public class BitSetDrawer : PropertyDrawer
	{
		private static float LineHeight => EditorGUIUtility.singleLineHeight;
		private static float ListHeight => LineHeight * BitManager.instance.Bits.NumBitsInCharacterGeneral * Mathf.Min(BitManager.instance.Bits.NumCharactersInCode, 2.5f);
		
		private Vector2 scrollPosition;
		
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return property.serializedObject.isEditingMultipleObjects ? 0 : LineHeight + (property.isExpanded ? ListHeight : 0);
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
			
			BitList bitList = BitManager.instance.Bits;
			BitSet bitSet = (BitSet)property.boxedValue;
			string code = bitList.BitSetToCode(bitSet);
			
			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, LineHeight, LineHeight), property.isExpanded, GUIContent.none);
			
			string newCode = EditorGUI.DelayedTextField(new Rect(position.x + LineHeight, position.y, position.width - LineHeight, LineHeight), code);
			if (code != newCode)
			{
				property.boxedValue = bitList.BitSetFromCode(newCode);
				property.serializedObject.ApplyModifiedProperties();
			}
			
			if (property.isExpanded)
			{
				scrollPosition = GUI.BeginScrollView(new Rect(position.x, position.y + LineHeight, position.width, ListHeight), scrollPosition, new Rect(0, 0, position.width - 14, LineHeight * BitManager.instance.Bits.NumBitsInCharacterGeneral * BitManager.instance.Bits.NumCharactersInCode));
				{
					for (int charIndex = 0; charIndex < bitList.NumCharactersInCode; charIndex++)
					{
						GUI.Box(new Rect(0, LineHeight * bitList.NumBitsInCharacterGeneral * charIndex, position.width, LineHeight * bitList.NumBitsInCharacterGeneral), GUIContent.none);
						for (int bitIndex = 0; bitIndex < bitList.NumBitsInCharacterAt(charIndex); bitIndex++)
						{
							int index = charIndex * bitList.NumCharactersInCode + bitIndex;
							Bit bit = bitList[index];
							bool hasBit = bitSet.Has(bit);
							bool newHasBit = EditorGUI.ToggleLeft(new Rect(4, LineHeight * index, position.width, LineHeight), bit.BitName, hasBit);
							if (hasBit != newHasBit)
							{
								if (newHasBit)
									property.boxedValue = (BitSet)property.boxedValue | bit;
								else
									property.boxedValue = (BitSet)property.boxedValue & BitManager.instance.Bits.Not(bit);
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
