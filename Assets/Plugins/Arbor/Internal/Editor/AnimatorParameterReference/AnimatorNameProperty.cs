//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public sealed class AnimatorNameProperty
	{
		private SerializedProperty _NameProperty;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public SerializedProperty nameProperty
		{
			get
			{
				if (_NameProperty == null)
				{
					_NameProperty = property.FindPropertyRelative("_Name");
				}
				return _NameProperty;
			}
		}

		public AnimatorNameProperty(SerializedProperty property)
		{
			this.property = property;
		}
	}
}