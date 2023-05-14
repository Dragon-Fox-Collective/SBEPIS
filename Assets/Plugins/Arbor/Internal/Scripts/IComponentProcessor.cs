//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// UndoなどのEditorの処理用インターフェイス。ComponentUtilityで使用する。
	/// </summary>
#else
	/// <summary>
	/// Interface for Editor processing such as Undo. Used with ComponentUtility.
	/// </summary>
#endif
	public interface IComponentProcessor
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Componentを追加する。
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		/// <param name="type">Componentの型</param>
		/// <returns>Component</returns>
#else
		/// <summary>
		/// Add component.
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		/// <param name="type">Component type</param>
		/// <returns>Component</returns>
#endif
		Component AddComponent(GameObject gameObject, System.Type type);

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectを破棄する。
		/// </summary>
		/// <param name="objectToUndo">Object</param>
#else
		/// <summary>
		/// Destroy object.
		/// </summary>
		/// <param name="objectToUndo">Object</param>
#endif
		void Destroy(Object objectToUndo);

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectを記録する。
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">名前</param>
#else
		/// <summary>
		/// Record object.
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">Name</param>
#endif
		void RecordObject(Object objectToUndo, string name);

#if ARBOR_DOC_JA
		/// <summary>
		/// 複数のObjectを記録する。
		/// </summary>
		/// <param name="objs">複数のObject</param>
		/// <param name="name">名前</param>
#else
		/// <summary>
		/// Records object.
		/// </summary>
		/// <param name="objs">Objects</param>
		/// <param name="name">Name</param>
#endif
		void RecordObjects(Object[] objs, string name);

#if ARBOR_DOC_JA
		/// <summary>
		/// UndoにObjectを登録する。
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">名前</param>
#else
		/// <summary>
		/// Register Object in Undo.
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">Name</param>
#endif
		void RegisterCompleteObjectUndo(Object objectToUndo, string name);

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectをダーティとしてマークする。
		/// </summary>
		/// <param name="obj">Object</param>
#else
		/// <summary>
		/// Marks an Object as dirty.
		/// </summary>
		/// <param name="obj">Object</param>
#endif
		void SetDirty(Object obj);

#if ARBOR_DOC_JA
		/// <summary>
		/// behaviourをnodeに移動する。
		/// </summary>
		/// <param name="node">移動先ノード</param>
		/// <param name="behaviour">移動するNodeBehaviour</param>
#else
		/// <summary>
		/// Move behavior to node.
		/// </summary>
		/// <param name="node">Moving destination node</param>
		/// <param name="behaviour">Moving NodeBehaviour</param>
#endif
		void MoveBehaviour(Node node, NodeBehaviour behaviour);

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフ内パラメータコンテナ―を移動する。Editorでのみ有効。
		/// </summary>
		/// <param name="nodeGraph">移動するnodeGraph</param>
#else
		/// <summary>
		/// Move the parameter container in the graph. Valid only in Editor.
		/// </summary>
		/// <param name="nodeGraph">Moving graph</param>
#endif
		void MoveParameterContainer(NodeGraph nodeGraph);

#if ARBOR_DOC_JA
		/// <summary>
		/// variableをparameterに移動する。Editorでのみ有効。
		/// </summary>
		/// <param name="parameter">移動先Parameter</param>
		/// <param name="variable">移動するVariableBase</param>
#else
		/// <summary>
		/// Move variable to parameter. Valid only in Editor.
		/// </summary>
		/// <param name="parameter">Moving destination Parameter</param>
		/// <param name="variable">Moving VariableBase</param>
#endif
		void MoveVariable(Parameter parameter, VariableBase variable);

#if ARBOR_DOC_JA
		/// <summary>
		/// variableListをparameterに移動する。Editorでのみ有効。
		/// </summary>
		/// <param name="parameter">移動先Parameter</param>
		/// <param name="variableList">移動するVariableListBase</param>
#else
		/// <summary>
		/// Move variable to parameter. Valid only in Editor.
		/// </summary>
		/// <param name="parameter">Moving destination Parameter</param>
		/// <param name="variableList">Moving VariableListBase</param>
#endif
		void MoveVariableList(Parameter parameter, VariableListBase variableList);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遅延破棄。
		/// </summary>
		/// <param name="obj">Object</param>
#else
		/// <summary>
		/// Delay Destroy.
		/// </summary>
		/// <param name="obj">Object</param>
#endif
		void DelayDestroy(Object obj);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遅延呼び出し
		/// </summary>
		/// <param name="delayCall">呼び出すメソッド</param>
#else
		/// <summary>
		/// Delay call.
		/// </summary>
		/// <param name="delayCall">Method to call</param>
#endif
		void DelayCall(ComponentUtility.DelayCallBack delayCall);

#if ARBOR_DOC_JA
		/// <summary>
		/// プレハブをインスタンス化する。
		/// </summary>
		/// <param name="prefab">プレハブ</param>
		/// <returns>インスタンス化したオブジェクト</returns>
#else
		/// <summary>
		/// Instantiate a prefab.
		/// </summary>
		/// <param name="prefab">Prefab</param>
		/// <returns>Instantiated object</returns>
#endif
		Object InstantiatePrefab(Object prefab);
	}
}