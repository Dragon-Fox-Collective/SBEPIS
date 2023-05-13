//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor
{
	using DynamicReflection;

#if ARBOR_DOC_JA
	/// <summary>
	/// UnityObjectのListParameter
	/// </summary>
	/// <typeparam name="T">UnityObjectの型</typeparam>
#else
	/// <summary>
	/// ListParameter of UnityObject
	/// </summary>
	/// <typeparam name="T">UnityObject type</typeparam>
#endif
	[System.Serializable]
	public abstract class ObjectListParameterBase<T> : ListParameterBase<T> where T : Object
	{
		[System.NonSerialized]
		private System.Type _ObjectType;

		[System.NonSerialized]
		private ListParameterBase _ObjectListParameter;

#if ARBOR_DOC_JA
		/// <summary>
		/// Listのインスタンスオブジェクト
		/// </summary>
#else
		/// <summary>
		/// List instance object
		/// </summary>
#endif
		public override sealed object listObject
		{
			get
			{
				if (_ObjectType == typeof(T))
				{
					return base.listObject;
				}

				if (_ObjectListParameter != null)
				{
					return _ObjectListParameter.listObject;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Listを設定する。
		/// </summary>
		/// <param name="value">設定するList</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set the List.
		/// </summary>
		/// <param name="value">The List to set</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public override bool SetList(IList value)
		{
			if (_ObjectType == typeof(T))
			{
				return base.SetList(value);
			}

			if (_ObjectListParameter == null)
			{
				return false;
			}

			if (_ObjectListParameter.SetList(value))
			{
				OnBeforeSerialize();
				return true;
			}

			return false;
		}

		internal void OnAfterDeserialize(System.Type valueType)
		{
			ListAccessor accessor = ListAccessor.Get(valueType);

			if (valueType != _ObjectType)
			{
				_ObjectType = valueType;

				if (_ObjectType != typeof(T))
				{
					_ObjectListParameter = ListParameterUtility.CreateInstance(valueType);
				}
				else
				{
					_ObjectListParameter = null;
				}
			}

			if (_ObjectListParameter == null)
			{
				return;
			}

			object listObject = _ObjectListParameter.listObject;

			IList listInstance = listObject as IList;
			if (listInstance == null)
			{
				return;
			}

			accessor.Clear(listInstance, ListInstanceType.Keep);

			for (int i = 0; i < list.Count; i++)
			{
				accessor.AddElement(listInstance, DynamicUtility.Cast(list[i], valueType, ExceptionMode.Ignore), ListInstanceType.Keep);
			}
		}

		internal void OnBeforeSerialize()
		{
			if (_ObjectType == typeof(T) && _ObjectListParameter == null)
			{
				return;
			}

			list.Clear();

			if (_ObjectListParameter == null || _ObjectType == null)
			{
				Debug.LogWarning("_ComponentListParameter == null || _ComponentType == null");
				return;
			}

			object listObject = _ObjectListParameter.listObject;
			IList listInstance = listObject as IList;
			if (listInstance == null)
			{
				return;
			}

			int count = listInstance.Count;
			for (int i = 0; i < count; i++)
			{
				object value = listInstance[i];
				list.Add(value as T);
			}
		}
	}
}