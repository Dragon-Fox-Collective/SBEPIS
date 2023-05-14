//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	public sealed class ClassTypeReferenceProperty
	{
		private const string kAssemblyTypeNamePath = "_AssemblyTypeName";

		private SerializedProperty _AssemblyTypeName;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public SerializedProperty assemblyTypeName
		{
			get
			{
				if (_AssemblyTypeName == null)
				{
					_AssemblyTypeName = property.FindPropertyRelative(kAssemblyTypeNamePath);
				}
				return _AssemblyTypeName;
			}
		}

		public System.Type type
		{
			get
			{
				return TypeUtility.GetAssemblyType(assemblyTypeName.stringValue);
			}
			set
			{
				assemblyTypeName.stringValue = TypeUtility.TidyAssemblyTypeName(value);
			}
		}

		public ClassTypeReferenceProperty(SerializedProperty property)
		{
			this.property = property;
		}

		public void SetConstraint(ClassTypeConstraintAttribute constraint)
		{
			property.SetStateData(constraint);
		}

		public void SetTypeFilter(TypeFilterAttribute filter)
		{
			property.SetStateData(filter);
		}

		public void Clear()
		{
			assemblyTypeName.stringValue = "";
		}
	}
}