//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor
{
	using Arbor;
	public static class ClassTypeConstraintEditorUtility
	{
		public static readonly ClassUnityObjectAttribute unityObject = new ClassUnityObjectAttribute();
		public static readonly ClassAssetObjectAttribute asset = new ClassAssetObjectAttribute();
		public static readonly ClassComponentAttribute component = new ClassComponentAttribute();
		public static readonly ClassScriptableObjectAttribute scriptableObject = new ClassScriptableObjectAttribute();
		public static readonly ClassEnumFieldConstraint enumField = new ClassEnumFieldConstraint();
		public static readonly ClassEnumFlagsConstraint enumFlags = new ClassEnumFlagsConstraint();

		public static bool GetParameterTypeConstraint(Parameter.Type parameterType, out ClassTypeConstraintAttribute constraint)
		{
			switch (parameterType)
			{
				case Parameter.Type.Component:
					constraint = component;
					return true;
				case Parameter.Type.Enum:
					constraint = enumField;
					return true;
				case Parameter.Type.AssetObject:
					constraint = asset;
					return true;
			}

			constraint = null;
			return false;
		}
	}
}