//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor
{
	using Arbor;

	internal static class ParameterContainerEditorUtility
	{
		public static readonly System.Type ParameterContainerType;
		private static readonly System.Reflection.FieldInfo s_ParameterContainerField;

		static ParameterContainerEditorUtility()
		{
			ParameterContainerType = AssemblyHelper.GetTypeByName("Arbor.ParameterContainer");
			s_ParameterContainerField = typeof(NodeGraph).GetField("_ParameterContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		}

		public static void SetParameterContainer(NodeGraph nodeGraph, ParameterContainerInternal parameterContainer)
		{
			s_ParameterContainerField.SetValue(nodeGraph, parameterContainer);
		}
	}
}