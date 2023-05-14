//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Reflection;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なシーンオブジェクト(GameObject, Component)型を扱うクラス。継承して使用する。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible scene objects (GameObject, Component) type reference method there is more than one. Inherit and use it.
	/// </summary>
#endif
	public abstract class FlexibleSceneObjectBase : IFlexibleField, IAssignFieldReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値の指定タイプ
		/// </summary>
#else
		/// <summary>
		/// Specified type of value
		/// </summary>
#endif
		[SerializeField]
		protected FlexibleSceneObjectType _Type = FlexibleSceneObjectType.Constant;

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeが<see cref="FlexibleSceneObjectType.Hierarchy"/>である時に指定するHierarchyの参照タイプ。
		/// </summary>
#else
		/// <summary>
		/// Reference type of Hierarchy specified when Type is FlexibleSceneObjectType.Hierarchy.
		/// </summary>
#endif
		[SerializeField]
		protected FlexibleHierarchyType _HierarchyType = FlexibleHierarchyType.Self;

#if ARBOR_DOC_JA
		/// <summary>
		/// このインスタンスを所有しているオブジェクトを返す
		/// </summary>
#else
		/// <summary>
		/// Return the object that owns this instance
		/// </summary>
#endif
		public Object ownerObject
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このインスタンスを所有しているFieldInfoを返す
		/// </summary>
#else
		/// <summary>
		/// Return the FieldInfo that owns this instance
		/// </summary>
#endif
		public FieldInfo fieldInfo
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSceneObjectType.Hierarchyの場合に参照する対象のNodeGraphを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns the NodeGraph to be referenced in case of FlexibleSceneObjectType.Hierarchy.
		/// </summary>
#endif
		public NodeGraph targetGraph
		{
			get
			{
				if (_Type != FlexibleSceneObjectType.Hierarchy)
				{
					return null;
				}

				NodeBehaviour ownerComponent = ownerObject as NodeBehaviour;
				if (ownerComponent == null)
				{
					return null;
				}

				NodeGraph nodeGraph = ownerComponent.nodeGraph;
				if (nodeGraph == null)
				{
					return null;
				}

				switch (_HierarchyType)
				{
					case FlexibleHierarchyType.Self:
						return nodeGraph;
					case FlexibleHierarchyType.RootGraph:
						return nodeGraph.rootGraph;
					case FlexibleHierarchyType.ParentGraph:
						return nodeGraph.parentGraph;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSceneObjectType.Hierarchyの場合に参照する対象のGameObjectを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns the GameObject to be referenced in case of FlexibleSceneObjectType.Hierarchy.
		/// </summary>
#endif
		public GameObject targetGameObject
		{
			get
			{
				NodeGraph graph = targetGraph;
				if (graph != null)
				{
					return graph.gameObject;
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeを返す
		/// </summary>
#else
		/// <summary>
		/// It returns a type
		/// </summary>
#endif
		public FlexibleSceneObjectType type
		{
			get
			{
				return _Type;
			}
			set
			{
				_Type = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSceneObjectType.Hierarchyの場合、どのオブジェクトを参照するかを表すFlexibleHierarchyTypeを返す
		/// </summary>
#else
		/// <summary>
		/// In the case of FlexibleSceneObjectType.Hierarchy, return FlexibleHierarchyType representing which object to reference
		/// </summary>
#endif
		public FlexibleHierarchyType hierarchyType
		{
			get
			{
				return _HierarchyType;
			}
			set
			{
				_HierarchyType = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値をobjectで返す。
		/// </summary>
		/// <returns>値のobject</returns>
#else
		/// <summary>
		/// Return the value as object.
		/// </summary>
		/// <returns>The value object</returns>
#endif
		public abstract object GetValueObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSceneObjectType.ConstantのObjectを返す。
		/// </summary>
		/// <returns>Constantの時のObject値</returns>
#else
		/// <summary>
		/// Returns an Object of FlexibleSceneObjectType.Constant.
		/// </summary>
		/// <returns>Object value at Constant</returns>
#endif
		public abstract Object GetConstantObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSceneObjectType.Constantであり参照しているオブジェクトがnullの場合に、FlexibleSceneObjectType.Hierarchyへ変更する。
		/// </summary>
		/// <param name="hierarchyType">設定するFlexibleHierarchyType</param>
		/// <returns>変更した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Change to FlexibleSceneObjectType.Hierarchy if FlexibleSceneObjectType.Constant and the referencing object is null.
		/// </summary>
		/// <param name="hierarchyType">FlexibleHierarchyType to set</param>
		/// <returns>Return true if changed.</returns>
#endif
		public bool SetHierarchyIfConstantNull(FlexibleHierarchyType hierarchyType = FlexibleHierarchyType.Self)
		{
			if (_Type == FlexibleSceneObjectType.Constant)
			{
				Object constantObject = GetConstantObject();

				bool isConstantNull = false;
				try
				{
					isConstantNull = (constantObject == null);
				}
				catch
				{
					// An exception occurs if constantObject is null when loading a scene.
					// If it is not null, no exception will occur, so force it to be true.
					isConstantNull = true;
				}

				if (isConstantNull)
				{
					_Type = FlexibleSceneObjectType.Hierarchy;
					_HierarchyType = hierarchyType;
					return true;
				}
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		public abstract void Disconnect();

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviour下のフィールドに割り当てられたときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when assigned to a field under NodeBehaviour.
		/// </summary>
#endif
		protected virtual void OnAssignedField()
		{
		}

		void IAssignFieldReceiver.OnAssignField(Object ownerObject, FieldInfo fieldInfo)
		{
			this.ownerObject = ownerObject;
			this.fieldInfo = fieldInfo;

			OnAssignedField();
		}
	}
}