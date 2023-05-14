//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.Events
{
	using Arbor;

	internal sealed class ValueTypeRepair : InvalidRepair
	{
		private PersistentCallProperty _Call;
		private System.Type _ValueType;

		public ValueTypeRepair(PersistentCallProperty call, System.Type valueType)
		{
			_Call = call;
			_ValueType = valueType;
		}

		public override string GetMessage()
		{
			return string.Format(Localization.GetWord("ArborEvent.InvalidRepair.ValueType"), TypeUtility.GetTypeName(_ValueType));
		}

		public override void OnRepair()
		{
			ArgumentProperty argumentProperty = _Call.argumentProperties[0];

			SerializedProperty oldParametersProperty = _Call.GetParametersProperty(argumentProperty.parameterType);
			oldParametersProperty.DeleteArrayElementAtIndex(argumentProperty.parameterIndex);

			_Call.argumentsProperty.DeleteArrayElementAtIndex(0);

			_Call.AddArgument(_ValueType);
		}
	}
}