//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// IListインスタンスとIValueSetter, IValueGetterのアクセスを仲介するクラス。
	/// </summary>
#else
	/// <summary>
	/// A class that mediates access between the IList instance and IValueSetter and IValueGetter.
	/// </summary>
#endif
	public class ListMediator
	{
		private System.Type _ValueType;
		private ListAccessor _Accessor;

		internal ListMediator(System.Type valueType)
		{
			_ValueType = valueType;
			_Accessor = ListAccessor.Get(_ValueType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素の値をIValueSetterへ設定する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="index">要素のインデックス</param>
		/// <param name="container">設定先IValueSetter</param>
#else
		/// <summary>
		/// Set the value of the element to IValueSetter.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="index">Element index</param>
		/// <param name="container">Setting destination IValueSetter</param>
#endif
		public virtual void GetElement(IList instance, int index, IValueSetter container)
		{
			ListUtility.CheckInstance(instance, "instnace");
			ListUtility.CheckIndex(instance, index);

			container.SetValueObject(instance[index]);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素の値の文字列を取得する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="index">要素のインデックス</param>
		/// <returns>値を変換した文字列</returns>
#else
		/// <summary>
		/// Gets the string of element values.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="index">Element index</param>
		/// <returns>Value converted string</returns>
#endif
		public virtual string GetElementString(IList instance, int index)
		{
			ListUtility.CheckInstance(instance, "instnace");
			ListUtility.CheckIndex(instance, index);

			var value = instance[index];
			if (value == null)
			{
				return "null";
			}
			return value.ToString();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IValueGetterから取得した値がIListに含まれているか確認する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="container">値を取得するIValueGetter</param>
		/// <returns>値が含まれている場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Check if the value retrieved from IValueGetter is contained in the IList.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="container">IValueGetter to get the value</param>
		/// <returns>Returns true if it contains a value.</returns>
#endif
		public virtual bool Contains(IList instance, IValueGetter container)
		{
			ListUtility.CheckInstance(instance, "instance");

			return instance.Contains(container.GetValueObject());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IValueGetterから取得した値のあるインデックスを見つける。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="container">値を取得するIValueGetter</param>
		/// <returns>見つかったインデックス。見つからなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Find the index where the value obtained from IValueGetter is located.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="container">IValueGetter to get a value.</param>
		/// <returns>Index found. If not found, -1 is returned.</returns>
#endif
		public virtual int IndexOf(IList instance, IValueGetter container)
		{
			return ListUtility.IndexOf(instance, container.GetValueObject());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IValueGetterから取得した値のあるインデックスをリストの終端から見つける。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="container">値を取得するIValueGetter</param>
		/// <returns>見つかったインデックス。見つからなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Find the index with the value retrieved from IValueGetter from the end of the list.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="container">IValueGetter to get a value.</param>
		/// <returns>Index found. If not found, -1 is returned.</returns>
#endif
		public virtual int LastIndexOf(IList instance, IValueGetter container)
		{
			return ListUtility.LastIndexOf(instance, container.GetValueObject());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 配列インスタンスを生成する。
		/// </summary>
		/// <param name="count">要素の数</param>
		/// <returns>配列インスタンス</returns>
#else
		/// <summary>
		/// Create a array instance.
		/// </summary>
		/// <param name="count">Element count</param>
		/// <returns>Array instance</returns>
#endif
		public virtual IList NewArray(int count)
		{
			return _Accessor.NewArray(count);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Listインスタンスを生成する。
		/// </summary>
		/// <returns>Listインスタンス</returns>
#else
		/// <summary>
		/// Create a List instance.
		/// </summary>
		/// <returns>List instance</returns>
#endif
		public virtual IList NewList()
		{
			return _Accessor.NewList();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// List{T}へ変換する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <returns>変換した結果を返す。</returns>
#else
		/// <summary>
		/// Convert to List{T}.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <returns>Return the result of the conversion.</returns>
#endif
		public virtual IList ToList(IList instance)
		{
			return _Accessor.ToList(instance);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 配列へ変換する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <returns>変換した結果を返す。</returns>
#else
		/// <summary>
		/// Convert to array.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <returns>Return the result of the conversion.</returns>
#endif
		public virtual IList ToArray(IList instance)
		{
			return _Accessor.ToArray(instance);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を削除する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="index">インデックス</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Remove an element.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="index">Index</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public virtual IList RemoveAtIndex(IList instance, int index, ListInstanceType instanceType)
		{
			return _Accessor.RemoveAtIndex(instance, index, instanceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素をすべて削除する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Remove all elements.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public virtual IList Clear(IList instance, ListInstanceType instanceType)
		{
			return _Accessor.Clear(instance, instanceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IListが等しいかどうかを判断する。
		/// </summary>
		/// <param name="a">判定するIList</param>
		/// <param name="b">判定するIList</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if IList are equal.
		/// </summary>
		/// <param name="a">IList to judge</param>
		/// <param name="b">IList to judge</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public virtual bool EqualsList(IList a, IList b)
		{
			return ListUtility.Equals(a, b);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// リストの範囲を追加。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="addToList">追加するリスト</param>
#else
		/// <summary>
		/// Add a range of lists.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="addToList">List to add</param>
#endif
		public virtual void AddRange(IList instance, IList addToList)
		{
			ListUtility.CheckInstance(instance, "instance");
			ListUtility.CheckInstance(addToList, "addToList");

			int count = addToList.Count;
			for (int i = 0; i < count; i++)
			{
				instance.Add(addToList[i]);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を設定する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="index">インデックス</param>
		/// <param name="valueContainer">設定するcontainer</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Set the container.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="index">Index</param>
		/// <param name="valueContainer">The container to set</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public virtual IList SetElement(IList instance, int index, IValueGetter valueContainer, ListInstanceType instanceType)
		{
			return _Accessor.SetElement(instance, index, valueContainer.GetValueObject(), instanceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を追加する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="container">追加するcontainer</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Add an container.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="container">The container to add</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public virtual IList AddElement(IList instance, IValueGetter container, ListInstanceType instanceType)
		{
			return _Accessor.AddElement(instance, container.GetValueObject(), instanceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を挿入する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="index">インデックス</param>
		/// <param name="container">挿入するcontainer</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Insert an container.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="index">Index</param>
		/// <param name="container">The container to insert</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public virtual IList InsertElement(IList instance, int index, IValueGetter container, ListInstanceType instanceType)
		{
			return _Accessor.InsertElement(instance, index, container.GetValueObject(), instanceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を削除する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="container">削除するcontainer</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Remove an container.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="container">The container to remove</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public virtual IList RemoveElement(IList instance, IValueGetter container, ListInstanceType instanceType)
		{
			return _Accessor.RemoveElement(instance, container.GetValueObject(), instanceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IListとIValueGetterで共通している要素の型を取得する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="container">値を取得するIValueGetter</param>
		/// <returns>共通している要素の型を返す。共通していない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the type of an element that is common to IList and IValueGetter.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="container">IValueGetter to get a value.</param>
		/// <returns>Returns the type of the common element. If it is not common, null is returned.</returns>
#endif
		public static System.Type GetElementType(IList instance, IValueGetter container)
		{
			if (instance == null || container == null)
			{
				return null;
			}

			var listElementType = ListUtility.GetElementType(instance.GetType());
			if (listElementType == null)
			{
				return null;
			}

			var containerType = container.GetType();

			System.Type valueType = null;
			if (TypeUtility.IsGeneric(containerType, typeof(IValueGetter<>)))
			{
				valueType = TypeUtility.GetGenericArguments(containerType)[0];
			}

			if (listElementType == valueType)
			{
				return listElementType;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IListとIValueSetterで共通している要素の型を取得する。
		/// </summary>
		/// <param name="instance">IListインスタンス</param>
		/// <param name="container">値を設定するIValueSetter</param>
		/// <returns>共通している要素の型を返す。共通していない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the type of an element that is common to IList and IValueSetter.
		/// </summary>
		/// <param name="instance">IList instance</param>
		/// <param name="container">IValueStter to set a value.</param>
		/// <returns>Returns the type of the common element. If it is not common, null is returned.</returns>
#endif
		public static System.Type GetElementType(IList instance, IValueSetter container)
		{
			if (instance == null || container == null)
			{
				return null;
			}

			var listElementType = ListUtility.GetElementType(instance.GetType());
			if (listElementType == null)
			{
				return null;
			}

			var containerType = container.GetType();

			System.Type valueType = null;
			if (TypeUtility.IsGeneric(containerType, typeof(IValueSetter<>)))
			{
				valueType = TypeUtility.GetGenericArguments(containerType)[0];
			}

			if (listElementType == valueType)
			{
				return listElementType;
			}

			return null;
		}
	}
}