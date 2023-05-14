//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Reflection;

namespace Arbor.ObjectPooling
{
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
	internal sealed class ClassPoolTargetAttribute : ClassTypeConstraintAttribute
	{
		public override System.Type GetBaseType(FieldInfo fieldInfo)
		{
			return typeof(Object);
		}

		public override bool IsConstraintSatisfied(System.Type type, FieldInfo fieldInfo)
		{
			return !TypeUtility.IsAssignableFrom(typeof(NodeBehaviour), type) && (TypeUtility.IsAssignableFrom(typeof(Component), type) || TypeUtility.IsAssignableFrom(typeof(GameObject), type));
		}
	}
}