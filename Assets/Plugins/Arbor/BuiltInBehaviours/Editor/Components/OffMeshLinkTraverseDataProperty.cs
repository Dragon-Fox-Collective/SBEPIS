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
	using Arbor;

	public sealed class OffMeshLinkTraverseDataProperty
	{
		private AnimatorNameProperty _ParameterProperty;
		private SerializedProperty _AngularSpeedProperty;
		private SerializedProperty _JumpHeightProperty;
		private SerializedProperty _MinSpeedProperty;
		private SerializedProperty _StartWaitProperty;
		private SerializedProperty _EndWaitProperty;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public AnimatorNameProperty parameterProperty
		{
			get
			{
				if (_ParameterProperty == null)
				{
					_ParameterProperty = new AnimatorNameProperty(property.FindPropertyRelative(nameof(OffMeshLinkTraverseData.parameter)));
				}
				return _ParameterProperty;
			}
		}

		public SerializedProperty angularSpeedProperty
		{
			get
			{
				if (_AngularSpeedProperty == null)
				{
					_AngularSpeedProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseData.angularSpeed));
				}
				return _AngularSpeedProperty;
			}
		}

		public SerializedProperty jumpHeightProperty
		{
			get
			{
				if (_JumpHeightProperty == null)
				{
					_JumpHeightProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseData.jumpHeight));
				}
				return _JumpHeightProperty;
			}
		}

		public SerializedProperty minSpeedProperty
		{
			get
			{
				if (_MinSpeedProperty == null)
				{
					_MinSpeedProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseData.minSpeed));
				}
				return _MinSpeedProperty;
			}
		}

		public SerializedProperty startWaitProperty
		{
			get
			{
				if (_StartWaitProperty == null)
				{
					_StartWaitProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseData.startWait));
				}
				return _StartWaitProperty;
			}
		}

		public SerializedProperty endWaitProperty
		{
			get
			{
				if (_EndWaitProperty == null)
				{
					_EndWaitProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseData.endWait));
				}
				return _EndWaitProperty;
			}
		}

		public OffMeshLinkTraverseDataProperty(SerializedProperty property)
		{
			this.property = property;
		}
	}
}