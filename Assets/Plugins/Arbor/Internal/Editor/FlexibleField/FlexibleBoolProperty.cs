//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	public sealed class FlexibleBoolProperty : FlexiblePrimitiveProperty
	{
		private const string kProbabilityPath = "_Probability";

		private SerializedProperty _ProbabilityProperty;

		public SerializedProperty probabilityProperty
		{
			get
			{
				if (_ProbabilityProperty == null)
				{
					_ProbabilityProperty = property.FindPropertyRelative(kProbabilityPath);
				}
				return _ProbabilityProperty;
			}
		}

		public FlexibleBoolProperty(SerializedProperty property) : base(property)
		{
		}

		protected override void OnClear()
		{
			probabilityProperty.Clear();
		}
	}
}