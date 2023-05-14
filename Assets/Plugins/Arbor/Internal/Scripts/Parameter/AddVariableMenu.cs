//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterContainerのパラメータ追加メニューでのパスを指定する属性。
	/// </summary>
#else
	/// <summary>
	/// AAttribute that specifies the path in ParameterContainer's parameter addition menu.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class AddVariableMenu : System.Attribute
	{
		private string _MenuName;

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
		/// AddVariableMenuのコンストラクタ
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
		/// AddVariableMenu constructor
		/// </summary>
		/// <param name="menuName">
		/// Menu name.
		/// <list type="bullet">
		/// <item><description>Hierarchized with "/" separator.</description></item>
		/// <item><description>If it is empty or null it is not displayed in the list.</description></item>
		/// </list>
		/// </param>
#endif
		public AddVariableMenu(string menuName)
		{
			_MenuName = menuName;
		}
	}
}