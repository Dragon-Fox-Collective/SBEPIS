//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// パラメータを格納するためのコンポーネント。<br/>
	/// GameObjectにアタッチして使用する。
	/// </summary>
	/// <remarks>
	/// [EnumList, AssetObjectList, ComponentList]<br />
	/// IL2CPPなどのAOT環境では、List&lt;指定した型&gt;がコード上で使用していないと正常動作しない可能性があります。<br />
	/// 詳しくは、<a href="https://arbor-docs.caitsithware.com/ja/manual/dataflow/list.html#AOTRestrictions">事前コンパイル(AOT)での制限</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// ParameterContainer。<br />
	/// Is used by attaching to GameObject.
	/// </summary>
	/// <remarks>
	/// [EnumList, AssetObjectList, ComponentList]<br />
	/// In an AOT environment such as IL2CPP, List&lt;specified type&gt; may not work properly unless it is used in the code.<br />
	/// See <a href="https://arbor-docs.caitsithware.com/en/manual/dataflow/list.html#AOTRestrictions">Ahead-of-Time (AOT) Restrictions</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("Arbor/ParameterContainer", 20)]
	[BuiltInComponent]
	[HelpURL(ArborReferenceUtility.componentUrl + "Arbor/parametercontainer.html")]
	[ExcludeFromPreset]
	[Internal.DocumentManual("/manual/parametercontainer/_index.md")]
	public sealed class ParameterContainer : ParameterContainerInternal
	{
	}
}
