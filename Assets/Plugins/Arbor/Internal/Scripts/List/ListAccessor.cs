//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ランタイムに生成したIList&lt;&gt;へのアクセスを行う。
	/// </summary>
#else
	/// <summary>
	/// Access IList&lt;&gt; generated at runtime.
	/// </summary>
#endif
	public sealed class ListAccessor
	{
		private System.Type _ElementType;
		
		internal ListAccessor(System.Type elementType)
		{
			_ElementType = elementType;
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
		public IList NewList()
		{
			return System.Activator.CreateInstance(ListUtility.GetListType(_ElementType)) as IList;
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
		public IList NewArray(int count)
		{
			return System.Array.CreateInstance(_ElementType, count);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を設定する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="index">インデックス</param>
		/// <param name="element">設定する要素</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Set the element.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="index">Index</param>
		/// <param name="element">The element to set</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public IList SetElement(IList instance, int index, object element, ListInstanceType instanceType)
		{
			ListUtility.CheckInstance(instance, "instance");
			ListUtility.CheckIndex(instance, index);

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						instance[index] = element;
						return instance;
					}
				case ListInstanceType.NewArray:
					{
						IList newArray = Internal_ToArray(instance);
						newArray[index] = element;
						return newArray;
					}
				case ListInstanceType.NewList:
					{
						IList newList = Internal_ToList(instance);
						newList[index] = element;
						return newList;
					}
			}

			return instance;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を追加する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="element">追加する要素</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Add an element.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="element">The element to add</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public IList AddElement(IList instance, object element, ListInstanceType instanceType)
		{
			ListUtility.CheckInstance(instance, "instance");

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						instance.Add(element);
						return instance;
					}
				case ListInstanceType.NewArray:
					{
						int count = instance.Count;
						IList newArray = NewArray(count + 1);

						Copy(instance, 0, newArray, 0, count);

						newArray[count] = element;
						return newArray;
					}
				case ListInstanceType.NewList:
					{
						IList newList = Internal_ToList(instance);

						newList.Add(element);
						return newList;
					}
			}

			return instance;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を挿入する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="index">インデックス</param>
		/// <param name="element">挿入する要素</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Insert an element.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="index">Index</param>
		/// <param name="element">The element to insert</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public IList InsertElement(IList instance, int index, object element, ListInstanceType instanceType)
		{
			ListUtility.CheckInstance(instance, "instance");

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						instance.Insert(index, element);
						return instance;
					}
				case ListInstanceType.NewArray:
					{
						int count = instance.Count;
						IList newArray = NewArray(count + 1);
						Copy(instance, 0, newArray, 0, index);
						newArray[index] = element;
						if (index < count)
						{
							Copy(instance, index, newArray, index + 1, count - index);
						}

						return newArray;
					}
				case ListInstanceType.NewList:
					{
						IList newList = Internal_ToList(instance);
						newList.Insert(index, element);
						return newList;
					}
			}

			return instance;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を削除する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="element">削除する要素</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Remove an element.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="element">The element to remove</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public IList RemoveElement(IList instance, object element, ListInstanceType instanceType)
		{
			ListUtility.CheckInstance(instance, "instance");

			int index = ListUtility.IndexOf(instance, element);
			if (index >= 0)
			{
				return Internal_RemoveAtIndex(instance, index, instanceType);
			}
			else
			{
				switch (instanceType)
				{
					case ListInstanceType.Keep:
						return instance;
					case ListInstanceType.NewArray:
						return Internal_ToArray(instance);
					case ListInstanceType.NewList:
						return Internal_ToList(instance);
				}
			}

			return instance;
		}

		private void Copy(IList sourceList, int sourceIndex, IList destinationList, int destinationIndex, int length)
		{
			int sourceCount = sourceList.Count;
			if (sourceIndex < 0)
			{
				throw new System.ArgumentOutOfRangeException("sourceIndex", ListUtility.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (sourceIndex >= sourceCount)
			{
				throw new System.ArgumentOutOfRangeException("sourceIndex", ListUtility.ArgumentOutOfRange_BiggerThanCollection);
			}

			int destinationCount = destinationList.Count;
			if (destinationIndex < 0)
			{
				throw new System.ArgumentOutOfRangeException("destinationIndex", ListUtility.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (destinationIndex >= destinationCount)
			{
				throw new System.ArgumentOutOfRangeException("destinationIndex", ListUtility.ArgumentOutOfRange_BiggerThanCollection);
			}

			if (length < 0)
			{
				throw new System.ArgumentOutOfRangeException("length", ListUtility.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (length > sourceCount - sourceIndex ||
				length > destinationCount - destinationIndex)
			{
				throw new System.ArgumentOutOfRangeException("length", ListUtility.ArgumentOutOfRange_BiggerThanCollection);
			}

			for (int i = 0; i < length; i++)
			{
				var element = sourceList[i + sourceIndex];
				destinationList[i + destinationIndex] = element;
			}
		}

		private IList Internal_RemoveAtIndex(IList instance, int index, ListInstanceType instanceType)
		{
			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						instance.RemoveAt(index);
						return instance;
					}
				case ListInstanceType.NewArray:
					{
						ListUtility.CheckIndex(instance, index);

						int count = instance.Count;
						int newCount = count - 1;
						IList array = NewArray(newCount);
						Copy(instance, 0, array, 0, index);
						if (index + 1 < count)
						{
							Copy(instance, index + 1, array, index, count - (index + 1));
						}

						return array;
					}
				case ListInstanceType.NewList:
					{
						IList newList = Internal_ToList(instance);
						newList.RemoveAt(index);
						return newList;
					}
			}

			return instance;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素を削除する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="index">インデックス</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Remove an element.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="index">Index</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public IList RemoveAtIndex(IList instance, int index, ListInstanceType instanceType)
		{
			ListUtility.CheckInstance(instance, "instance");

			return Internal_RemoveAtIndex(instance, index, instanceType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素をすべて削除する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <param name="instanceType">インスタンスの変更方法</param>
		/// <returns>変更した結果のインスタンス</returns>
#else
		/// <summary>
		/// Remove all elements.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <param name="instanceType">How to change the instance</param>
		/// <returns>The resulting instance of the change</returns>
#endif
		public IList Clear(IList instance, ListInstanceType instanceType)
		{
			ListUtility.CheckInstance(instance, "instance");

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						instance.Clear();
						return instance;
					}
				case ListInstanceType.NewArray:
					{
						int count = instance.Count;
						return NewArray(count);
					}
				case ListInstanceType.NewList:
					{
						return NewList();
					}
			}

			return instance;
		}

		private static int Internal_LastIndexOf(IList instance, object element, int startIndex, int count)
		{
			int listCount = instance.Count;
			if (listCount == 0)
			{
				return -1;
			}

			if (startIndex < 0)
			{
				throw new System.ArgumentOutOfRangeException("index", ListUtility.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (startIndex >= listCount)
			{
				throw new System.ArgumentOutOfRangeException("index", ListUtility.ArgumentOutOfRange_BiggerThanCollection);
			}

			if (count < 0)
			{
				throw new System.ArgumentOutOfRangeException("count", ListUtility.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count > startIndex + 1)
			{
				throw new System.ArgumentOutOfRangeException("count", ListUtility.ArgumentOutOfRange_BiggerThanCollection);
			}

			int num = startIndex - count + 1;
			for (int i = startIndex; i >= num; --i)
			{
				object value = instance[i];
				if (object.Equals(value, element))
				{
					return i;
				}
			}

			return -1;
		}

		private IList Internal_ToArray(IList instance)
		{
			int count = instance.Count;
			var newArray = NewArray(count);
			Copy(instance, 0, newArray, 0, count);

			return newArray;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 配列に変換する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <returns>変換結果の配列</returns>
#else
		/// <summary>
		/// Convert to an array.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <returns>Array of conversion results</returns>
#endif
		public IList ToArray(IList instance)
		{
			ListUtility.CheckInstance(instance, "instance");

			return Internal_ToArray(instance);
		}

		private IList Internal_ToList(IList instance)
		{
			return System.Activator.CreateInstance(ListUtility.GetListType(_ElementType), instance) as IList;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Listに変換する。
		/// </summary>
		/// <param name="instance">IList&lt;elementType&gt;型のインスタンス</param>
		/// <returns>変換結果のList</returns>
#else
		/// <summary>
		/// Convert to List.
		/// </summary>
		/// <param name="instance">Instance of type IList&lt;elementType&gt;</param>
		/// <returns>List of conversion results</returns>
#endif
		public IList ToList(IList instance)
		{
			ListUtility.CheckInstance(instance, "instance");

			return Internal_ToList(instance);
		}

		static Dictionary<System.Type, ListAccessor> s_Accessors = new Dictionary<System.Type, ListAccessor>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した要素の型のListAccessorを取得する。
		/// </summary>
		/// <param name="elementType">要素の型</param>
		/// <returns>IList&lt;elementType&gt;にアクセスするListAccessor</returns>
#else
		/// <summary>
		/// Get ListAccessor of specified element type.
		/// </summary>
		/// <param name="elementType">Element type</param>
		/// <returns>ListAccessor to access IList&lt;elementType&gt;</returns>
#endif
		public static ListAccessor Get(System.Type elementType)
		{
			ListAccessor accessor = null;
			if (elementType != null && !s_Accessors.TryGetValue(elementType, out accessor))
			{
				accessor = new ListAccessor(elementType);
				s_Accessors.Add(elementType, accessor);
			}

			return accessor;
		}
	}
}