//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// タグの判定
	/// </summary>
#else
	/// <summary>
	/// Tag check
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public class TagChecker
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// タグをチェックするかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to check the tag.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _IsCheckTag = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// チェックするタグ。
		/// </summary>
#else
		/// <summary>
		/// Tag to be checked.
		/// </summary>
#endif
		[SerializeField]
		[TagSelector]
		private FlexibleString _Tag = new FlexibleString("Untagged");

		public TagChecker() : this(false, "Untagged")
		{
		}

		public TagChecker(bool isCheckTag, string tag)
		{
			_IsCheckTag = new FlexibleBool(isCheckTag);
			_Tag = new FlexibleString(tag);
		}

		public bool CheckTag(GameObject gameObject)
		{
			return !_IsCheckTag.value || gameObject.CompareTag(_Tag.value);
		}

		public bool CheckTag(Component component)
		{
			return CheckTag(component.gameObject);
		}
	}
}