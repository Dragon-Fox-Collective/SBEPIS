//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	public sealed class FlexibleNumericProperty : FlexiblePrimitiveProperty
	{
		// Paths
		private const string kMinRangePath = "_MinRange";
		private const string kMaxRangePath = "_MaxRange";

		private SerializedProperty _MinRange;
		private SerializedProperty _MaxRange;

		public SerializedProperty minRangeProperty
		{
			get
			{
				if (_MinRange == null)
				{
					_MinRange = property.FindPropertyRelative(kMinRangePath);
				}
				return _MinRange;
			}
		}

		public SerializedProperty maxRangeProperty
		{
			get
			{
				if (_MaxRange == null)
				{
					_MaxRange = property.FindPropertyRelative(kMaxRangePath);
				}
				return _MaxRange;
			}
		}

		public FlexibleNumericProperty(SerializedProperty property) : base(property)
		{
		}

		protected override void OnClear()
		{
			minRangeProperty.Clear();
			maxRangeProperty.Clear();
		}
	}
}