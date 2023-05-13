//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;
	using Arbor.Serialization;

	internal sealed class FlexibleComponentOfTPropertyEditor : FlexibleSceneObjectPropertyEditor
	{
		private static Dictionary<System.Type, System.Type> s_ConstantObjectTypes = new Dictionary<System.Type, System.Type>();

		protected override System.Type GetConstantObjectType()
		{
			System.Type fieldType = SerializationUtility.ElementType(fieldInfo.FieldType);

			System.Type objType = typeof(Component);
			if (!s_ConstantObjectTypes.TryGetValue(fieldType, out objType))
			{
				for (System.Type type = fieldType; type != null && type != typeof(FlexibleComponentBase); type = type.BaseType)
				{
					if (TypeUtility.IsGeneric(type, typeof(FlexibleComponent<>)))
					{
						objType = type.GetGenericArguments()[0];
						break;
					}
				}

				s_ConstantObjectTypes.Add(fieldType, objType);
			}

			return objType;
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleComponentBase), true)]
	internal sealed class FlexibleComponentOfTPropertyDrawer : PropertyEditorDrawer<FlexibleComponentOfTPropertyEditor>
	{
	}
}