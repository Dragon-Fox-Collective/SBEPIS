//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 型選択ポップアップで表示するフィルターのフラグ
	/// </summary>
#else
	/// <summary>
	/// Filter flag displayed in the type selection popup
	/// </summary>
#endif
	[System.Flags]
	public enum TypeFilterFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内のオブジェクト(GameObjectかComponent)
		/// </summary>
#else
		/// <summary>
		/// Object in the scene (GameObject or Component)
		/// </summary>
#endif
		SceneObject = 1 << 0,

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン以外のオブジェクト
		/// </summary>
#else
		/// <summary>
		/// Objects other than the scene
		/// </summary>
#endif
		AssetObject = 1 << 1,

#if ARBOR_DOC_JA
		/// <summary>
		/// クラス
		/// </summary>
#else
		/// <summary>
		/// Class
		/// </summary>
#endif
		Class = 1 << 2,

#if ARBOR_DOC_JA
		/// <summary>
		/// 構造体
		/// </summary>
#else
		/// <summary>
		/// Struct
		/// </summary>
#endif
		Struct = 1 << 3,

#if ARBOR_DOC_JA
		/// <summary>
		/// インターフェイス
		/// </summary>
#else
		/// <summary>
		/// Interface
		/// </summary>
#endif
		Interface = 1 << 4,

#if ARBOR_DOC_JA
		/// <summary>
		/// 列挙型
		/// </summary>
#else
		/// <summary>
		/// Enum
		/// </summary>
#endif
		Enum = 1 << 5,

#if ARBOR_DOC_JA
		/// <summary>
		/// プリミティブ(intなどの基本型)
		/// </summary>
#else
		/// <summary>
		/// Primitive (base type such as int)
		/// </summary>
#endif
		Primitive = 1 << 6,

#if ARBOR_DOC_JA
		/// <summary>
		/// デリゲート
		/// </summary>
#else
		/// <summary>
		/// Delegate
		/// </summary>
#endif
		Delegate = 1 << 7,

#if ARBOR_DOC_JA
		/// <summary>
		/// スタティッククラス
		/// </summary>
#else
		/// <summary>
		/// Static class
		/// </summary>
#endif
		Static = 1 << 8,
	}
}