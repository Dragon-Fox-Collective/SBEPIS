//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Editor用Componentユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Component utility class for Editor
	/// </summary>
#endif
	public static class ComponentUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 遅延呼び出しされるメソッドのデリゲート
		/// </summary>
#else
		/// <summary>
		/// Delegate of delayed invoked method
		/// </summary>
#endif
		public delegate void DelayCallBack();

#if ARBOR_DOC_JA
		/// <summary>
		/// Editorでのコンポーネントプロセッサ
		/// </summary>
#else
		/// <summary>
		/// Component processor in Editor
		/// </summary>
#endif
		public static IComponentProcessor editorProcessor = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// editorProcessorを使うフラグ。
		/// </summary>
#else
		/// <summary>
		/// Flag using editorProcessor.
		/// </summary>
#endif
		public static bool useEditorProcessor = true;

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
		public static Component AddComponent(GameObject gameObject, System.Type type)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				return editorProcessor.AddComponent(gameObject, type);
			}
			return gameObject.AddComponent(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Componentを追加する。
		/// </summary>
		/// <typeparam name="Type">Componentの型</typeparam>
		/// <param name="gameObject">GameObject</param>
		/// <returns>Component</returns>
#else
		/// <summary>
		/// Add component.
		/// </summary>
		/// <typeparam name="Type">Component type</typeparam>
		/// <param name="gameObject">GameObject</param>
		/// <returns>Component</returns>
#endif
		public static Type AddComponent<Type>(GameObject gameObject) where Type : Component
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				return editorProcessor.AddComponent(gameObject, typeof(Type)) as Type;
			}
			return gameObject.AddComponent<Type>();
		}

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
		public static void Destroy(Object objectToUndo)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				editorProcessor.Destroy(objectToUndo);
			}
			else if (Application.isEditor && !Application.isPlaying)
			{
				Object.DestroyImmediate(objectToUndo);
			}
			else
			{
				Object.Destroy(objectToUndo);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectを記録する。Editorでのみ有効。
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">名前</param>
#else
		/// <summary>
		/// Record object. Valid only in Editor.
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">Name</param>
#endif
		public static void RecordObject(Object objectToUndo, string name)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				editorProcessor.RecordObject(objectToUndo, name);
				return;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 複数のObjectを記録する。Editorでのみ有効。
		/// </summary>
		/// <param name="objs">複数のObject</param>
		/// <param name="name">名前</param>
#else
		/// <summary>
		/// Records object. Valid only in Editor.
		/// </summary>
		/// <param name="objs">Objects</param>
		/// <param name="name">Name</param>
#endif
		public static void RecordObjects(Object[] objs, string name)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				editorProcessor.RecordObjects(objs, name);
				return;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// UndoにObjectを登録する。Editorでのみ有効。
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">名前</param>
#else
		/// <summary>
		/// Register Object in Undo. Valid only in Editor.
		/// </summary>
		/// <param name="objectToUndo">Object</param>
		/// <param name="name">Name</param>
#endif
		public static void RegisterCompleteObjectUndo(Object objectToUndo, string name)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				editorProcessor.RegisterCompleteObjectUndo(objectToUndo, name);
				return;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectをダーティとしてマークする。Editorでのみ有効。
		/// </summary>
		/// <param name="obj">Object</param>
#else
		/// <summary>
		/// Marks an Object as dirty. Valid only in Editor.
		/// </summary>
		/// <param name="obj">Object</param>
#endif
		public static void SetDirty(Object obj)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				editorProcessor.SetDirty(obj);
				return;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// behaviourをnodeに移動する。Editorでのみ有効。
		/// </summary>
		/// <param name="node">移動先ノード</param>
		/// <param name="behaviour">移動するNodeBehaviour</param>
#else
		/// <summary>
		/// Move behavior to node. Valid only in Editor.
		/// </summary>
		/// <param name="node">Moving destination node</param>
		/// <param name="behaviour">Moving NodeBehaviour</param>
#endif
		public static void MoveBehaviour(Node node, NodeBehaviour behaviour)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null && node != null)
			{
				editorProcessor.MoveBehaviour(node, behaviour);
				return;
			}
		}

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
		public static void MoveParameterContainer(NodeGraph nodeGraph)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null && nodeGraph != null)
			{
				editorProcessor.MoveParameterContainer(nodeGraph);
				return;
			}
		}

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
		public static void MoveVariable(Parameter parameter, VariableBase variable)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null && parameter != null)
			{
				editorProcessor.MoveVariable(parameter, variable);
				return;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// variableListをparameterに移動する。Editorでのみ有効。
		/// </summary>
		/// <param name="parameter">移動先Parameter</param>
		/// <param name="variableList">移動するVariableListBase</param>
#else
		/// <summary>
		/// Move variableList to parameter. Valid only in Editor.
		/// </summary>
		/// <param name="parameter">Moving destination Parameter</param>
		/// <param name="variableList">Moving VariableListBase</param>
#endif
		public static void MoveVariableList(Parameter parameter, VariableListBase variableList)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null && parameter != null)
			{
				editorProcessor.MoveVariableList(parameter, variableList);
				return;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphをRefreshする。Editorでのみ有効。
		/// </summary>
		/// <param name="nodeGraph">NodeGraph</param>
#else
		/// <summary>
		/// Refresh NodeGraph. Valid only in Editor.
		/// </summary>
		/// <param name="nodeGraph">NodeGraph</param>
#endif
		public static void RefreshNodeGraph(NodeGraph nodeGraph)
		{
			if (useEditorProcessor && Application.isEditor && nodeGraph != null)
			{
				bool cachedEnabled = useEditorProcessor;
				useEditorProcessor = false;

				nodeGraph.Refresh();

				useEditorProcessor = cachedEnabled;
				return;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectが有効かチェックする。
		/// </summary>
		/// <param name="obj">Object</param>
		/// <returns>Objectが有効である場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Check if Object is valid.
		/// </summary>
		/// <param name="obj">Object</param>
		/// <returns>Returns true if Object is valid.</returns>
#endif
		public static bool IsValidObject(Object obj)
		{
			return obj != null && (obj is MonoBehaviour || obj is ScriptableObject);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 遅延破棄。Editorでのみ遅延。
		/// </summary>
		/// <param name="obj">Object</param>
#else
		/// <summary>
		/// Delay Destroy. Delayed only in Editor.
		/// </summary>
		/// <param name="obj">Object</param>
#endif
		public static void DelayDestroy(Object obj)
		{
			if (useEditorProcessor && Application.isEditor && editorProcessor != null)
			{
				editorProcessor.DelayDestroy(obj);
			}
			else
			{
				Destroy(obj);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 遅延呼び出し。Editorでのみ遅延。
		/// </summary>
		/// <param name="delayCall">呼び出すメソッド</param>
#else
		/// <summary>
		/// Delay call. Delayed only in Editor.
		/// </summary>
		/// <param name="delayCall">Method to call</param>
#endif
		public static void DelayCall(DelayCallBack delayCall)
		{
			if (useEditorProcessor && editorProcessor != null)
			{
				editorProcessor.DelayCall(delayCall);
			}
			else
			{
				delayCall();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プレハブをインスタンス化する。
		/// </summary>
		/// <param name="prefab">プレハブ</param>
		/// <returns>インスタンス化したオブジェクト</returns>
		/// <remarks>
		/// UnityEditor上から編集中に呼び出した場合はプレハブが紐づいたインスタンスとしてシーンに配置される。
		/// </remarks>
#else
		/// <summary>
		/// Instantiate a prefab.
		/// </summary>
		/// <param name="prefab">Prefab</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// When called from the Unity Editor during editing, the prefab is placed in the scene as a linked instance.
		/// </remarks>
#endif
		public static Object InstantiatePrefab(Object prefab)
		{
			if (useEditorProcessor && Application.isEditor && !Application.isPlaying && editorProcessor != null)
			{
				return editorProcessor.InstantiatePrefab(prefab);
			}
			else
			{
				return Object.Instantiate(prefab);
			}
		}
	}
}