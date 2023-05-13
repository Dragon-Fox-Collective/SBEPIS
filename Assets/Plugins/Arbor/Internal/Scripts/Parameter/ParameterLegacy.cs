//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor
{
	[System.Serializable]
	internal sealed class ParameterLegacy
	{
		public ParameterContainerInternal container = null;

		public int id = 0;

		public Parameter.Type type = Parameter.Type.Int;

		public string name = "";

		[SerializeField]
		[FormerlySerializedAs("intValue")]
		internal int _IntValue = 0;

		[SerializeField]
		[FormerlySerializedAs("longValue")]
		internal long _LongValue = 0L;

		[SerializeField]
		[FormerlySerializedAs("floatValue")]
		internal float _FloatValue = 0f;

		[SerializeField]
		[FormerlySerializedAs("boolValue")]
		internal bool _BoolValue = false;

		[SerializeField]
		[FormerlySerializedAs("stringValue")]
		internal string _StringValue = "";

		[SerializeField]
		[FormerlySerializedAs("gameObjectValue")]
		internal GameObject _GameObjectValue = null;

		[SerializeField]
		[FormerlySerializedAs("vector2Value")]
		internal Vector2 _Vector2Value = Vector2.zero;

		[SerializeField]
		[FormerlySerializedAs("vector3Value")]
		internal Vector3 _Vector3Value = Vector3.zero;

		[SerializeField]
		[EulerAngles]
		[FormerlySerializedAs("quaternionValue")]
		internal Quaternion _QuaternionValue = Quaternion.identity;

		[SerializeField]
		[FormerlySerializedAs("rectValue")]
		internal Rect _RectValue = new Rect();

		[SerializeField]
		[FormerlySerializedAs("boundsValue")]
		internal Bounds _BoundsValue = new Bounds();

		[SerializeField]
		[FormerlySerializedAs("colorValue")]
		internal Color _ColorValue = Color.white;

		[SerializeField]
		[FormerlySerializedAs("objectReferenceValue")]
		internal Object _ObjectReferenceValue = null;

		public ClassTypeReference referenceType = new ClassTypeReference();
	}
}
