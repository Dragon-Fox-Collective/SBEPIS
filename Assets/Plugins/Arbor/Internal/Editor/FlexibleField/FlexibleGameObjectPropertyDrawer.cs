//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class FlexibleGameObjectPropertyEditor : FlexibleSceneObjectPropertyEditor
	{
		protected override System.Type GetConstantObjectType()
		{
			return typeof(GameObject);
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleGameObject))]
	internal sealed class FlexibleGameObjectPropertyDrawer : PropertyEditorDrawer<FlexibleGameObjectPropertyEditor>
	{
	}
}