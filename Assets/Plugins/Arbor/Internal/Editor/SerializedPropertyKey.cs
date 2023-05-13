//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal struct SerializedPropertyKey : System.IEquatable<SerializedPropertyKey>
	{
		private SerializedObject _SerializedObject;
		private int[] _InstanceIDs;
		private string _PropertyPath;

		private int _HashCode;

		public SerializedPropertyKey(SerializedProperty property)
		{
			_SerializedObject = property.serializedObject;
			_PropertyPath = property.propertyPath;

			_HashCode = _PropertyPath.GetHashCode();
			var targetObjects = _SerializedObject.targetObjects;
			_InstanceIDs = new int[targetObjects.Length];
			for (int objIndex = 0; objIndex < targetObjects.Length; objIndex++)
			{
				Object obj = targetObjects[objIndex];
				_InstanceIDs[objIndex] = obj.GetInstanceID();
				_HashCode ^= obj.GetHashCode();
			}
		}

		public override int GetHashCode()
		{
			return _HashCode;
		}

		public SerializedProperty GetProperty()
		{
			try
			{
				return _SerializedObject.FindProperty(_PropertyPath);
			}
			catch
			{
				return null;
			}
		}

		bool System.IEquatable<SerializedPropertyKey>.Equals(SerializedPropertyKey other)
		{
			try
			{
				int[] otherInstanceIDs = other._InstanceIDs;

				if (_PropertyPath != other._PropertyPath ||
					_InstanceIDs == null || otherInstanceIDs == null ||
					_InstanceIDs.Length != otherInstanceIDs.Length)
				{
					return false;
				}

				for (int i = 0; i < _InstanceIDs.Length; ++i)
				{
					if (_InstanceIDs[i] != otherInstanceIDs[i])
					{
						return false;
					}
				}

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}