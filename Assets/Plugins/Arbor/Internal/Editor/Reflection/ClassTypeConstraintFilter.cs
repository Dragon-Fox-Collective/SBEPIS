//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor
{
	using Arbor;

	public sealed class ClassTypeConstraintFilter : IDefinableType
	{
		private ClassTypeConstraintAttribute _Constraint;
		private System.Reflection.FieldInfo _FieldInfo;

		public ClassTypeConstraintFilter(ClassTypeConstraintAttribute attribute, System.Reflection.FieldInfo fieldInfo)
		{
			_Constraint = attribute;
			_FieldInfo = fieldInfo;
		}

		public bool IsDefinableType(System.Type type)
		{
			return _Constraint.IsConstraintSatisfied(type, _FieldInfo);
		}
	}
}