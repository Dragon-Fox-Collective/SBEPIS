//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AddBehaviourメニューでの名前を指定する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute that specifies the name of at AddBehaviour menu.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	[Internal.DocumentManual("/manual/scripting/behaviourattribute/AddBehaviourMenu.md")]
	public sealed class AddBehaviourMenu : System.Attribute
	{
		private string _MenuName;

#if ARBOR_DOC_JA
		/// <summary>
		/// ローカライズを行うフラグ。trueの場合、menuNameをキーとして各言語のワードに変換する。
		/// </summary>
#else
		/// <summary>
		/// Flag for localization. If true, convert to word of each language with menuName as key.
		/// </summary>
#endif
		public bool localization = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// メニュー名
		/// </summary>
#else
		/// <summary>
		/// Menu name
		/// </summary>
#endif
		public string menuName
		{
			get
			{
				return _MenuName;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AddBehaviourMenuのコンストラクタ
		/// </summary>
		/// <param name="menuName">
		/// メニュー名。
		/// <list type="bullet">
		/// <item><description>"/"区切りで階層化。</description></item>
		/// <item><description>Emptyかnullの場合はリストに表示されない。</description></item>
		/// </list>
		/// </param>
#else
		/// <summary>
		/// AddBehaviourMenu constructor
		/// </summary>
		/// <param name="menuName">
		/// Menu name.
		/// <list type="bullet">
		/// <item><description>Hierarchized with "/" separator.</description></item>
		/// <item><description>If it is empty or null it is not displayed in the list.</description></item>
		/// </list>
		/// </param>
#endif
		public AddBehaviourMenu(string menuName)
		{
			_MenuName = menuName;
		}
	}
}