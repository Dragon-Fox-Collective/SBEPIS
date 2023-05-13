//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	public sealed class FlexibleComponentProperty : FlexibleSceneObjectProperty
	{
		// Paths
		private const string kOverrideConstraintTypePath = "_OverrideConstraintType";

		private ClassTypeReferenceProperty _OverrideConstraintTypeProperty = null;

		public ClassTypeReferenceProperty overrideConstraintTypeProperty
		{
			get
			{
				if (_OverrideConstraintTypeProperty == null)
				{
					_OverrideConstraintTypeProperty = new ClassTypeReferenceProperty(property.FindPropertyRelative(kOverrideConstraintTypePath));
				}
				return _OverrideConstraintTypeProperty;
			}
		}

		public System.Type overrideConstraintType
		{
			get
			{
				return overrideConstraintTypeProperty.type;
			}
			set
			{
				overrideConstraintTypeProperty.type = value;
			}
		}

		public FlexibleComponentProperty(SerializedProperty property) : base(property)
		{
		}
	}
}