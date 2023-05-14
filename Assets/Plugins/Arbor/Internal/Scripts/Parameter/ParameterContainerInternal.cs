//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// パラメータコンテナ。
	/// GameObjectにアタッチして使用する。
	/// </summary>
#else
	/// <summary>
	/// ParameterContainer.
	/// Is used by attaching to GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region Serialize fields

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private Object _Owner = null;

#if ARBOR_DOC_JA
		/// <summary>Paramerterの一覧。<br/>
		/// <list type="bullet">
		/// <item><description>+ボタンから、パラメータの型を選択して作成。</description></item>
		/// <item><description>パラメータを選択し、-ボタンをクリックで削除。</description></item>
		/// </list>
		/// </summary>
#else
		/// <summary>List of parameters.<br/>
		/// <list type="bullet">
		/// <item><description>From the + button, select the type of parameter to create.</description></item>
		/// <item><description>Select the parameter and delete it by clicking the - button.</description></item>
		/// </list>
		/// </summary>
#endif
		[SerializeField]
		private List<Parameter> _Parameters = new List<Parameter>();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Parameters")]
		private List<ParameterLegacy> _OldParameters = new List<ParameterLegacy>();

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		[System.NonSerialized]
		private Dictionary<int, Parameter> _DicParameters = new Dictionary<int, Parameter>();

#if ARBOR_DOC_JA
		/// <summary>
		/// このParameterContainerの所有者であるObject
		/// </summary>
#else
		/// <summary>
		/// Object own this ParameterContainer
		/// </summary>
#endif
		public Object owner
		{
			get
			{
				return _Owner;
			}
			set
			{
				_Owner = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの配列を取得。
		/// </summary>
#else
		/// <summary>
		/// Get an array of parameters.
		/// </summary>
#endif
		[System.Obsolete("use parameterCount and GetParameterFromIndex()")]
		public Parameter[] parameters
		{
			get
			{
				return _Parameters.ToArray();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of Parameter.
		/// </summary>
#endif
		public int parameterCount
		{
			get
			{
				return _Parameters.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>Parameter</returns>
#else
		/// <summary>
		/// Get Parameter from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Parameter</returns>
#endif
		public Parameter GetParameterFromIndex(int index)
		{
			return _Parameters[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ParameterContainerを作成する。
		/// </summary>
		/// <param name="gameObject">追加先のGameObject</param>
		/// <param name="type">ParameterContainerの型</param>
		/// <returns>作成されたParamreterContainer</returns>
#else
		/// <summary>
		/// Create ParameterContainer.
		/// </summary>
		/// <param name="gameObject">The GameObject to add to</param>
		/// <param name="type">ParameterContainer type</param>
		/// <returns>The created ParamreterContainer</returns>
#endif
		public static ParameterContainerInternal Create(GameObject gameObject, System.Type type)
		{
			return Create(gameObject, type, null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ParameterContainerを作成する。
		/// </summary>
		/// <param name="gameObject">追加先のGameObject</param>
		/// <param name="type">ParameterContainerの型</param>
		/// <param name="owner">所有者</param>
		/// <returns>作成されたParamreterContainer</returns>
#else
		/// <summary>
		/// Create ParameterContainer.
		/// </summary>
		/// <param name="gameObject">The GameObject to add to</param>
		/// <param name="type">ParameterContainer type</param>
		/// <param name="owner">Owner</param>
		/// <returns>The created ParamreterContainer</returns>
#endif
		public static ParameterContainerInternal Create(GameObject gameObject, System.Type type, Object owner)
		{
			if (!TypeUtility.IsSubclassOf(type, typeof(ParameterContainerInternal)))
			{
				return null;
			}

			s_CreatingOwner = owner;

			ParameterContainerInternal container = ComponentUtility.AddComponent(gameObject, type) as ParameterContainerInternal;

			s_CreatingOwner = null;

			return container;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Unityから呼び出される開始メソッド
		/// </summary>
#else
		/// <summary>
		/// Start method called from Unity
		/// </summary>
#endif
		protected virtual void Start()
		{
			Refresh();
		}

		private bool _IsEditor = false;

		bool IsEditor()
		{
			return _IsEditor;
		}

		bool IsMove()
		{
			if (IsEditor())
			{
				return false;
			}

			int count = parameterCount;
			for (int index = 0; index < count; index++)
			{
				Parameter parameter = GetParameterFromIndex(index);

				if (parameter.container != this)
				{
					return true;
				}
				else
				{
					switch (parameter.type)
					{
						case Parameter.Type.Variable:
							{
								VariableBase variable = parameter.variableObject;
								if (variable != null && variable.parameterContainer != this)
								{
									return true;
								}
							}
							break;
						case Parameter.Type.VariableList:
							{
								VariableListBase variable = parameter.variableListObject;
								if (variable != null && variable.parameterContainer != this)
								{
									return true;
								}
							}
							break;
					}
				}
			}

			return false;
		}

		void DestroyUnusedVariable(InternalVariableBase variable)
		{
			if (variable.parameterContainer != this)
			{
				return;
			}

			Parameter parameter = FindParameterContainsVariable(variable);
			if (parameter == null)
			{
#if ARBOR_DEBUG
				Debug.Log("Destroy Unused Variable : " + variable);
#endif
				ComponentUtility.Destroy(variable);
			}
		}

		void DestroyUnusedVariable()
		{
			if (this == null)
			{
				return;
			}

			var variables = this.GetComponentsTemp<InternalVariableBase>();
			for (int variableIndex = 0; variableIndex < variables.Count; variableIndex++)
			{
				InternalVariableBase variable = variables[variableIndex];
				DestroyUnusedVariable(variable);
			}
		}

		private bool _IsValidateDelay = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトがロードされた時やインスペクターの値が変更されたときに呼び出される（この呼出はエディター上のみ）
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script is loaded or when the inspector value changes (this call is only in the editor)
		/// </summary>
#endif
		protected virtual void OnValidate()
		{
			if (IsMove())
			{
				for (int count = parameterCount, parameterIndex = 0; parameterIndex < count; parameterIndex++)
				{
					Parameter parameter = GetParameterFromIndex(parameterIndex);
					parameter.ChangeContainer(this);
				}
			}

			if (!_IsValidateDelay)
			{
				_IsValidateDelay = true;
				ComponentUtility.DelayCall(OnValidateDelay);
			}

			if (Application.isPlaying)
			{
				for (int count = parameterCount, parameterIndex = 0; parameterIndex < count; parameterIndex++)
				{
					Parameter parameter = GetParameterFromIndex(parameterIndex);
					parameter.DoChanged();
				}
			}

			Refresh();
		}

		/// <summary>
		/// Editor only
		/// </summary>
		public void Refresh()
		{
#if !ARBOR_DEBUG
			if (owner == null)
			{
				hideFlags &= ~(HideFlags.HideInInspector);
			}
			else
			{
				hideFlags |= HideFlags.HideInInspector | HideFlags.HideInHierarchy;
			}
#endif
			for (int count = parameterCount, parameterIndex = 0; parameterIndex < count; parameterIndex++)
			{
				Parameter parameter = GetParameterFromIndex(parameterIndex);

				if (parameter.type != Parameter.Type.Variable && parameter.type != Parameter.Type.VariableList)
				{
					continue;
				}

				Object variableObj = parameter.Internal_GetObject();

				if (ComponentUtility.IsValidObject(variableObj))
				{
#if !ARBOR_DEBUG
					variableObj.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
#endif
				}
			}
		}

		void OnValidateDelay()
		{
			DestroyUnusedVariable();

			_IsValidateDelay = false;
		}

		IList GetParameterList(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Int:
				case Parameter.Type.Enum:
					return _IntParameters;
				case Parameter.Type.Long:
					return _LongParameters;
				case Parameter.Type.Float:
					return _FloatParameters;
				case Parameter.Type.Bool:
					return _BoolParameters;
				case Parameter.Type.String:
					return _StringParameters;
				case Parameter.Type.Vector2:
					return _Vector2Parameters;
				case Parameter.Type.Vector3:
					return _Vector3Parameters;
				case Parameter.Type.Quaternion:
					return _QuaternionParameters;
				case Parameter.Type.Rect:
					return _RectParameters;
				case Parameter.Type.Bounds:
					return _BoundsParameters;
				case Parameter.Type.Color:
					return _ColorParameters;
				case Parameter.Type.Vector4:
					return _Vector4Parameters;
				case Parameter.Type.Vector2Int:
					return _Vector2IntParameters;
				case Parameter.Type.Vector3Int:
					return _Vector3IntParameters;
				case Parameter.Type.RectInt:
					return _RectIntParameters;
				case Parameter.Type.BoundsInt:
					return _BoundsIntParameters;
				case Parameter.Type.GameObject:
				case Parameter.Type.Component:
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.AssetObject:
				case Parameter.Type.Variable:
				case Parameter.Type.VariableList:
					return _ObjectParameters;
				case Parameter.Type.IntList:
					return _IntListParameters;
				case Parameter.Type.LongList:
					return _LongListParameters;
				case Parameter.Type.FloatList:
					return _FloatListParameters;
				case Parameter.Type.BoolList:
					return _BoolListParameters;
				case Parameter.Type.StringList:
					return _StringListParameters;
				case Parameter.Type.EnumList:
					return _EnumListParameters;
				case Parameter.Type.Vector2List:
					return _Vector2ListParameters;
				case Parameter.Type.Vector3List:
					return _Vector3ListParameters;
				case Parameter.Type.QuaternionList:
					return _QuaternionListParameters;
				case Parameter.Type.RectList:
					return _RectListParameters;
				case Parameter.Type.BoundsList:
					return _BoundsListParameters;
				case Parameter.Type.ColorList:
					return _ColorListParameters;
				case Parameter.Type.Vector4List:
					return _Vector4ListParameters;
				case Parameter.Type.Vector2IntList:
					return _Vector2IntListParameters;
				case Parameter.Type.Vector3IntList:
					return _Vector3IntListParameters;
				case Parameter.Type.RectIntList:
					return _RectIntListParameters;
				case Parameter.Type.BoundsIntList:
					return _BoundsIntListParameters;
				case Parameter.Type.GameObjectList:
					return _GameObjectListParameters;
				case Parameter.Type.ComponentList:
					return _ComponentListParameters;
				case Parameter.Type.AssetObjectList:
					return _AssetObjectListParameters;
			}

			throw new System.NotImplementedException("It is an unimplemented Parameter type(" + parameterType + ")");
		}

		static void AddParameterValue<T>(Parameter parameter, IList<T> list, T value)
		{
			list.Add(value);
			parameter._ParameterIndex = list.Count - 1;
		}

		Parameter AddParamInternal(int id, string name, Parameter.Type type)
		{
			Parameter parameter = new Parameter();
			parameter.container = this;
			parameter.id = id;
			parameter.name = name;
			parameter.type = type;

			_Parameters.Add(parameter);
			_DicParameters.Add(parameter.id, parameter);

			switch (type)
			{
				case Parameter.Type.Int:
				case Parameter.Type.Enum:
					AddParameterValue(parameter, _IntParameters, 0);
					break;
				case Parameter.Type.Long:
					AddParameterValue(parameter, _LongParameters, 0L);
					break;
				case Parameter.Type.Float:
					AddParameterValue(parameter, _FloatParameters, 0f);
					break;
				case Parameter.Type.Bool:
					AddParameterValue(parameter, _BoolParameters, false);
					break;
				case Parameter.Type.String:
					AddParameterValue(parameter, _StringParameters, "");
					break;
				case Parameter.Type.Vector2:
					AddParameterValue(parameter, _Vector2Parameters, Vector2.zero);
					break;
				case Parameter.Type.Vector3:
					AddParameterValue(parameter, _Vector3Parameters, Vector3.zero);
					break;
				case Parameter.Type.Quaternion:
					AddParameterValue(parameter, _QuaternionParameters, Quaternion.identity);
					break;
				case Parameter.Type.Rect:
					AddParameterValue(parameter, _RectParameters, new Rect());
					break;
				case Parameter.Type.Bounds:
					AddParameterValue(parameter, _BoundsParameters, new Bounds());
					break;
				case Parameter.Type.Color:
					AddParameterValue(parameter, _ColorParameters, Color.white);
					break;
				case Parameter.Type.Vector4:
					AddParameterValue(parameter, _Vector4Parameters, Vector4.zero);
					break;
				case Parameter.Type.Vector2Int:
					AddParameterValue(parameter, _Vector2IntParameters, Vector2Int.zero);
					break;
				case Parameter.Type.Vector3Int:
					AddParameterValue(parameter, _Vector3IntParameters, Vector3Int.zero);
					break;
				case Parameter.Type.RectInt:
					AddParameterValue(parameter, _RectIntParameters, RectIntExtensions.zero);
					break;
				case Parameter.Type.BoundsInt:
					AddParameterValue(parameter, _BoundsIntParameters, BoundsIntExtensions.zero);
					break;
				case Parameter.Type.Component:
					AddParameterValue(parameter, _ObjectParameters, null);
					parameter.referenceType = typeof(Component);
					break;
				case Parameter.Type.AssetObject:
					AddParameterValue(parameter, _ObjectParameters, null);
					parameter.referenceType = typeof(Object);
					break;
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Variable:
				case Parameter.Type.VariableList:
				case Parameter.Type.GameObject:
					AddParameterValue(parameter, _ObjectParameters, null);
					break;
				case Parameter.Type.IntList:
					AddParameterValue(parameter, _IntListParameters, new IntListParameter());
					break;
				case Parameter.Type.LongList:
					AddParameterValue(parameter, _LongListParameters, new LongListParameter());
					break;
				case Parameter.Type.FloatList:
					AddParameterValue(parameter, _FloatListParameters, new FloatListParameter());
					break;
				case Parameter.Type.BoolList:
					AddParameterValue(parameter, _BoolListParameters, new BoolListParameter());
					break;
				case Parameter.Type.StringList:
					AddParameterValue(parameter, _StringListParameters, new StringListParameter());
					break;
				case Parameter.Type.EnumList:
					{
						var enumListParameter = new EnumListParameter();
						parameter.referenceType.type = null;
						enumListParameter.OnAfterDeserialize(parameter.referenceType.type);

						AddParameterValue(parameter, _EnumListParameters, enumListParameter);
					}
					break;
				case Parameter.Type.Vector2List:
					AddParameterValue(parameter, _Vector2ListParameters, new Vector2ListParameter());
					break;
				case Parameter.Type.Vector3List:
					AddParameterValue(parameter, _Vector3ListParameters, new Vector3ListParameter());
					break;
				case Parameter.Type.QuaternionList:
					AddParameterValue(parameter, _QuaternionListParameters, new QuaternionListParameter());
					break;
				case Parameter.Type.RectList:
					AddParameterValue(parameter, _RectListParameters, new RectListParameter());
					break;
				case Parameter.Type.BoundsList:
					AddParameterValue(parameter, _BoundsListParameters, new BoundsListParameter());
					break;
				case Parameter.Type.ColorList:
					AddParameterValue(parameter, _ColorListParameters, new ColorListParameter());
					break;
				case Parameter.Type.Vector4List:
					AddParameterValue(parameter, _Vector4ListParameters, new Vector4ListParameter());
					break;
				case Parameter.Type.Vector2IntList:
					AddParameterValue(parameter, _Vector2IntListParameters, new Vector2IntListParameter());
					break;
				case Parameter.Type.Vector3IntList:
					AddParameterValue(parameter, _Vector3IntListParameters, new Vector3IntListParameter());
					break;
				case Parameter.Type.RectIntList:
					AddParameterValue(parameter, _RectIntListParameters, new RectIntListParameter());
					break;
				case Parameter.Type.BoundsIntList:
					AddParameterValue(parameter, _BoundsIntListParameters, new BoundsIntListParameter());
					break;
				case Parameter.Type.GameObjectList:
					AddParameterValue(parameter, _GameObjectListParameters, new GameObjectListParameter());
					break;
				case Parameter.Type.ComponentList:
					{
						var componentListParameter = new ComponentListParameter();
						parameter.referenceType.type = typeof(Component);
						componentListParameter.OnAfterDeserialize(parameter.referenceType.type ?? typeof(Component));

						AddParameterValue(parameter, _ComponentListParameters, componentListParameter);
					}
					break;
				case Parameter.Type.AssetObjectList:
					{
						var listParameter = new AssetObjectListParameter();
						parameter.referenceType.type = typeof(Object);
						listParameter.OnAfterDeserialize(parameter.referenceType.type ?? typeof(Object));

						AddParameterValue(parameter, _AssetObjectListParameters, listParameter);
					}
					break;
				default:
					throw new System.NotImplementedException("It is an unimplemented Parameter type(" + type + ")");
			}

			return parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを追加する。
		/// </summary>
		/// <param name="name">名前。nameが重複していた場合はユニークな名前に変更される。</param>
		/// <param name="type">型。</param>
		/// <returns>追加されたパラメータ。</returns>
#else
		/// <summary>
		/// Add a parameter.
		/// </summary>
		/// <param name="name">Name. It is changed to a unique name if the name had been duplicated.</param>
		/// <param name="type">Type.</param>
		/// <returns>It added parameters.</returns>
#endif
		public Parameter AddParam(string name, Parameter.Type type)
		{
			int id = MakeUniqueParamID();
			name = MakeUniqueName(name);
			return AddParamInternal(id, name, type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前からパラメータを取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータ。存在しなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the parameters from the name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Parameters. Return null if you did not exist.</returns>
#endif
		public Parameter GetParam(string name)
		{
			for (int i = 0, count = _Parameters.Count; i < count; i++)
			{
				Parameter parameter = _Parameters[i];
				if (parameter == null)
				{
					continue;
				}

				if (parameter.name == name)
				{
					return parameter;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前からパラメータのIDを取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータのID。存在しなかった場合は0を返す。</returns>
#else
		/// <summary>
		/// Get the parameters ID from the name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>Parameters ID. Return 0 if you did not exist.</returns>
#endif
		public int GetParamID(string name)
		{
			Parameter parameter = GetParam(name);
			if (parameter != null)
			{
				return parameter.id;
			}

			return 0;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IDからパラメータを取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータ。存在しなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the parameters from the ID.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>Parameters. Return null if you did not exist.</returns>
#endif
		public Parameter GetParam(int id)
		{
			Parameter result = null;
			if (_DicParameters.TryGetValue(id, out result))
			{
				return result;
			}

			return null;
		}

		int MakeUniqueParamID()
		{
			int count = _Parameters.Count;

			System.Random random = new System.Random(count);

			while (true)
			{
				int id = random.Next();

				if (id != 0 && GetParam(id) == null)
				{
					return id;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを削除する。
		/// </summary>
		/// <param name="parameter">パラメータ。</param>
#else
		/// <summary>
		/// Delete a parameter.
		/// </summary>
		/// <param name="parameter">Parameter.</param>
#endif
		public void DeleteParam(Parameter parameter)
		{
			if (parameter == null)
			{
				return;
			}

			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			_Parameters.Remove(parameter);
			_DicParameters.Remove(parameter.id);

			parameter.OnDestroy();

			IList parameterList = GetParameterList(parameter.type);

			int parameterIndex = parameter._ParameterIndex;

			if (parameterList != null && 0 <= parameterIndex && parameterIndex < parameterList.Count)
			{
				parameterList.RemoveAt(parameterIndex);

				for (int i = 0, count = parameterCount; i < count; i++)
				{
					Parameter p = GetParameterFromIndex(i);
					IList pList = GetParameterList(p.type);
					if (pList == parameterList && p._ParameterIndex > parameterIndex)
					{
						p._ParameterIndex--;
					}
				}
			}

			ComponentUtility.SetDirty(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前を指定してパラメータを削除する。
		/// </summary>
		/// <param name="name">名前。</param>
#else
		/// <summary>
		/// Delete the parameters by name.
		/// </summary>
		/// <param name="name">Name.</param>
#endif
		public void DeleteParam(string name)
		{
			Parameter parameter = GetParam(name);
			DeleteParam(parameter);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IDを指定してパラメータを削除する。
		/// </summary>
		/// <param name="id">ID。</param>
#else
		/// <summary>
		/// Delete the parameters by ID.
		/// </summary>
		/// <param name="id">ID.</param>
#endif
		public void DeleteParam(int id)
		{
			Parameter parameter = GetParam(id);
			DeleteParam(parameter);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 重複しない名前を生成する。
		/// </summary>
		/// <param name="name">元の名前。</param>
		/// <returns>結果の名前。</returns>
#else
		/// <summary>
		/// It generates a name that does not overlap.
		/// </summary>
		/// <param name="name">The original name.</param>
		/// <returns>Result.</returns>
#endif
		public string MakeUniqueName(string name)
		{
			string searchName = name;
			int count = 0;
			while (true)
			{
				if (GetParam(searchName) == null)
				{
					break;
				}

				searchName = name + " " + count;
				count++;
			}

			return searchName;
		}

		private bool IsParameterType(Parameter parameter, Parameter.Type type)
		{
			return parameter != null && parameter.type == type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの型を判定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="type">パラメータの型。</param>
		/// <returns>パラメータの型がtypeと一致する場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine the type of parameter.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="type">Parameter type.</param>
		/// <returns>Returns true if the parameter type matches type.</returns>
#endif
		public bool IsParameterType(string name, Parameter.Type type)
		{
			Parameter parameter = GetParam(name);
			return IsParameterType(parameter, type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの型を判定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="type">パラメータの型。</param>
		/// <returns>パラメータの型がtypeと一致する場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine the type of parameter.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="type">Parameter type.</param>
		/// <returns>Returns true if the parameter type matches type.</returns>
#endif
		public bool IsParameterType(int id, Parameter.Type type)
		{
			Parameter parameter = GetParam(id);
			return IsParameterType(parameter, type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// VariableBaseが属しているParameterの取得。
		/// </summary>
		/// <param name="variable">VariableBase</param>
		/// <returns>VariableBaseが属しているParameter。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Acquisition of parameters VariableBase belongs.
		/// </summary>
		/// <param name="variable">VariableBase</param>
		/// <returns>Parameters VariableBase belongs. Return null if not.</returns>
#endif
		public Parameter FindParameterContainsVariable(InternalVariableBase variable)
		{
			for (int count = _Parameters.Count, index = 0; index < count; index++)
			{
				Parameter parameter = _Parameters[index];
				if (parameter.type != Parameter.Type.Variable && parameter.type != Parameter.Type.VariableList)
				{
					continue;
				}

				Object variableObj = parameter.Internal_GetObject();
				if (variableObj == variable)
				{
					return parameter;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 内部的に使用するメソッド。特に呼び出す必要はありません。
		/// </summary>
#else
		/// <summary>
		/// Method to be used internally. In particular there is no need to call.
		/// </summary>
#endif
		public void DestroySubComponents()
		{
			int parameterCount = _Parameters.Count;
			for (int parameterIndex = 0; parameterIndex < parameterCount; ++parameterIndex)
			{
				Parameter parameter = _Parameters[parameterIndex];

				parameter.OnDestroy();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はMonoBehaviourが破棄されるときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when MonoBehaivour will be destroyed.
		/// </summary>
#endif
		protected virtual void OnDestroy()
		{
			DestroySubComponents();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ParameterContainerInternalの破棄
		/// </summary>
		/// <param name="parameterContainer">ParameterContainerInternal</param>
#else
		/// <summary>
		/// Destroy ParameterContainerInternal
		/// </summary>
		/// <param name="parameterContainer">ParameterContainerInternal</param>
#endif
		public static void Destroy(ParameterContainerInternal parameterContainer)
		{
			if (!Application.isPlaying)
			{
				parameterContainer.DestroySubComponents();
			}
			ComponentUtility.Destroy(parameterContainer);
		}

		private static Object s_CreatingOwner = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// デフォルト値にリセットする。
		/// </summary>
#else
		/// <summary>
		/// Reset to default values.
		/// </summary>
#endif
		protected virtual void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
			owner = s_CreatingOwner;
		}

		void SerializeVer1()
		{
			_Parameters.Clear();
			_DicParameters.Clear();
			
			for (int oldParameterIndex = 0; oldParameterIndex < _OldParameters.Count; oldParameterIndex++)
			{
				ParameterLegacy p = _OldParameters[oldParameterIndex];
				Parameter newParameter = AddParamInternal(p.id, p.name, p.type);
				int parameterIndex = newParameter._ParameterIndex;
				switch (p.type)
				{
					case Parameter.Type.Int:
						_IntParameters[parameterIndex] = p._IntValue;
						break;
					case Parameter.Type.Long:
						_LongParameters[parameterIndex] = p._LongValue;
						break;
					case Parameter.Type.Float:
						_FloatParameters[parameterIndex] = p._FloatValue;
						break;
					case Parameter.Type.Bool:
						_BoolParameters[parameterIndex] = p._BoolValue;
						break;
					case Parameter.Type.String:
						_StringParameters[parameterIndex] = p._StringValue;
						break;
					case Parameter.Type.Enum:
						_IntParameters[parameterIndex] = p._IntValue;
						newParameter.referenceType = new ClassTypeReference(p.referenceType);
						break;
					case Parameter.Type.Vector2:
						_Vector2Parameters[parameterIndex] = p._Vector2Value;
						break;
					case Parameter.Type.Vector3:
						_Vector3Parameters[parameterIndex] = p._Vector3Value;
						break;
					case Parameter.Type.Quaternion:
						_QuaternionParameters[parameterIndex] = p._QuaternionValue;
						break;
					case Parameter.Type.Rect:
						_RectParameters[parameterIndex] = p._RectValue;
						break;
					case Parameter.Type.Bounds:
						_BoundsParameters[parameterIndex] = p._BoundsValue;
						break;
					case Parameter.Type.Color:
						_ColorParameters[parameterIndex] = p._ColorValue;
						break;
					case Parameter.Type.GameObject:
						_ObjectParameters[parameterIndex] = p._GameObjectValue;
						break;
					case Parameter.Type.Component:
					case Parameter.Type.AssetObject:
						_ObjectParameters[parameterIndex] = p._ObjectReferenceValue;
						newParameter.referenceType = new ClassTypeReference(p.referenceType);
						break;
					case Parameter.Type.Transform:
					case Parameter.Type.RectTransform:
					case Parameter.Type.Rigidbody:
					case Parameter.Type.Rigidbody2D:
					case Parameter.Type.Variable:
						_ObjectParameters[parameterIndex] = p._ObjectReferenceValue;
						break;
				}
			}

			_OldParameters.Clear();
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デシリアライズ済みかどうかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether or not deserialization has been done.
		/// </summary>
#endif
		public bool isDeserialized
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デシリアライズ後のコールバック
		/// </summary>
#else
		/// <summary>
		/// Callback after deserialization
		/// </summary>
#endif
		public event System.Action onAfterDeserialize;

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();

			for (int i = 0, count = _Parameters.Count; i < count; i++)
			{
				Parameter parameter = _Parameters[i];
				if (parameter == null)
				{
					continue;
				}

				switch (parameter.type)
				{
					case Parameter.Type.EnumList:
						_EnumListParameters[parameter._ParameterIndex].OnBeforeSerialize();
						break;
					case Parameter.Type.ComponentList:
						_ComponentListParameters[parameter._ParameterIndex].OnBeforeSerialize();
						break;
					case Parameter.Type.AssetObjectList:
						_AssetObjectListParameters[parameter._ParameterIndex].OnBeforeSerialize();
						break;
				}
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();

			_DicParameters.Clear();
			for (int i = 0, count = _Parameters.Count; i < count; i++)
			{
				Parameter parameter = _Parameters[i];
				if (parameter == null)
				{
					continue;
				}

				_DicParameters.Add(parameter.id, parameter);

				switch (parameter.type)
				{
					case Parameter.Type.EnumList:
						_EnumListParameters[parameter._ParameterIndex].OnAfterDeserialize(parameter.referenceType.type);
						break;
					case Parameter.Type.ComponentList:
						_ComponentListParameters[parameter._ParameterIndex].OnAfterDeserialize(parameter.referenceType.type ?? typeof(Component));
						break;
					case Parameter.Type.AssetObjectList:
						_AssetObjectListParameters[parameter._ParameterIndex].OnAfterDeserialize(parameter.referenceType.type ?? typeof(Object));
						break;

				}
			}

			onAfterDeserialize?.Invoke();
			onAfterDeserialize = null;

			isDeserialized = true;
		}
	}
}
