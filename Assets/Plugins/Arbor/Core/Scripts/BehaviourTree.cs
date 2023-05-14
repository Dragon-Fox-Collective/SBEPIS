//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ビヘイビアツリーのコア部分。<br/>
	/// GameObjectにアタッチして使用する。
	/// </summary>
	/// <param name="OpenEditor">Arbor Editorウィンドウで開く</param>
#else
	/// <summary>
	/// Core part of BehaviourTree.<br/>
	/// Is used by attaching to GameObject.
	/// </summary>
	/// <param name="OpenEditor">Open with Arbor Editor Window</param>
#endif
	[AddComponentMenu("Arbor/BehaviourTree", 10)]
	[AddBehaviourMenu("BehaviourTree")]
	[BuiltInComponent]
	[HelpURL(ArborReferenceUtility.componentUrl + "Arbor/behaviourtree.html")]
	[ExcludeFromPreset]
	[Internal.DocumentManual("/manual/behaviourtree/_index.md")]
	public sealed class BehaviourTree : BehaviourTreeInternal
	{
	}
}