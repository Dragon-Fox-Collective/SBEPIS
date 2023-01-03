using UnityEngine;
using UnityEngine.UIElements;

namespace SBEPIS.Utils
{
	public class ObjectPopupField : PopupField<Object>
	{
		public new class UxmlTraits : PopupField<Object>.UxmlTraits { }
		public new class UxmlFactory : UxmlFactory<ObjectPopupField, UxmlTraits> { }

		public ObjectPopupField()
		{
			formatListItemCallback = formatSelectedValueCallback = e => e ? e.name : "None";
		}
	}
}
