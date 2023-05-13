//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateBehaviourの表示するタイトルを指定する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute that specifies the title to display the StateBehaviour.
	/// </summary>
#endif
	[Obsolete("use BehaviourTitle")]
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class CalculatorTitle : Attribute
	{
		private string _TitleName;

#if ARBOR_DOC_JA
		/// <summary>
		/// タイトル名
		/// </summary>
#else
		/// <summary>
		/// Title name
		/// </summary>
#endif
		public string titleName
		{
			get
			{
				return _TitleName;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CalculatorTitleのコンストラクタ
		/// </summary>
		/// <param name="titleName">タイトル名</param>
#else
		/// <summary>
		/// CalculatorTitle constructor
		/// </summary>
		/// <param name="titleName">Title name</param>
#endif
		public CalculatorTitle(string titleName)
		{
			_TitleName = titleName;
		}
	}
}
