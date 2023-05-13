//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.Internal;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listのユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// List utility class
	/// </summary>
#endif
	public static class ListUtility
	{
		internal const string ArgumentOutOfRange_NeedNonNegNum = "Non-negative number required.";
		internal const string ArgumentOutOfRange_BiggerThanCollection = "Larger than collection size.";
		
		static Dictionary<System.Type, System.Type> s_IListTypeCache = new Dictionary<System.Type, System.Type>();
		static Dictionary<System.Type, System.Type> s_ICollectionTypeCache = new Dictionary<System.Type, System.Type>();
		static Dictionary<System.Type, System.Type> s_ListTypeCache = new Dictionary<System.Type, System.Type>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素の型を取得する。
		/// </summary>
		/// <param name="listType">リストの型</param>
		/// <returns>要素の型を返す。</returns>
#else
		/// <summary>
		/// Get the element type.
		/// </summary>
		/// <param name="listType">List type</param>
		/// <returns>Returns the element type.</returns>
#endif
		public static System.Type GetElementType(System.Type listType)
		{
			if (TypeUtility.IsGeneric(listType, typeof(IList<>)) || TypeUtility.IsGeneric(listType, typeof(List<>)))
			{
				return TypeUtility.GetGenericArguments(listType)[0];
			}
			else if (listType.IsArray && listType.GetArrayRank() == 1)
			{
				return listType.GetElementType();
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IList&lt;elementType&gt;の型を取得する。
		/// </summary>
		/// <param name="elementType">要素の型。</param>
		/// <returns>IList&lt;elementType&gt;の型を返す。</returns>
#else
		/// <summary>
		/// Get the type of IList&lt;elementType&gt;.
		/// </summary>
		/// <param name="elementType">Element type</param>
		/// <returns>Returns the type of IList&lt;elementType&gt;.</returns>
#endif
		public static System.Type GetIListType(System.Type elementType)
		{
			if (elementType == null)
			{
				return null;
			}

			System.Type type = null;
			if (!s_IListTypeCache.TryGetValue(elementType, out type))
			{
				type = typeof(IList<>).MakeGenericType(elementType);
				s_IListTypeCache.Add(elementType, type);
			}

			return type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ICollection&lt;elementType&gt;の型を取得する。
		/// </summary>
		/// <param name="elementType">要素の型。</param>
		/// <returns>ICollection&lt;elementType&gt;の型を返す。</returns>
#else
		/// <summary>
		/// Get the type of ICollection&lt;elementType&gt;.
		/// </summary>
		/// <param name="elementType">Element type</param>
		/// <returns>Returns the type of ICollection&lt;elementType&gt;.</returns>
#endif
		public static System.Type GetICollectionType(System.Type elementType)
		{
			if (elementType == null)
			{
				return null;
			}

			System.Type type = null;
			if (!s_ICollectionTypeCache.TryGetValue(elementType, out type))
			{
				type = typeof(ICollection<>).MakeGenericType(elementType);
				s_ICollectionTypeCache.Add(elementType, type);
			}

			return type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// List&lt;elementType&gt;の型を取得する。
		/// </summary>
		/// <param name="elementType">要素の型。</param>
		/// <returns>List&lt;elementType&gt;の型を返す。</returns>
#else
		/// <summary>
		/// Get the type of List&lt;elementType&gt;.
		/// </summary>
		/// <param name="elementType">Element type</param>
		/// <returns>Returns the type of List&lt;elementType&gt;.</returns>
#endif
		public static System.Type GetListType(System.Type elementType)
		{
			if (elementType == null)
			{
				return null;
			}

			System.Type type = null;
			if (!s_ListTypeCache.TryGetValue(elementType, out type))
			{
				type = typeof(List<>).MakeGenericType(elementType);
				s_ListTypeCache.Add(elementType, type);
			}

			return type;
		}

		internal static void CheckInstance(IList list, string instanceName)
		{
			if (list == null)
			{
				throw new System.ArgumentNullException(instanceName);
			}
		}

		internal static void CheckIndex(IList list, int index)
		{
			if (index < 0)
			{
				throw new System.ArgumentOutOfRangeException("index", ArgumentOutOfRange_NeedNonNegNum);
			}

			int count = list.Count;
			if (index >= count)
			{
				throw new System.ArgumentOutOfRangeException("index", ArgumentOutOfRange_BiggerThanCollection);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IListが等しいかどうかを判断する。
		/// </summary>
		/// <param name="a">判定するIList</param>
		/// <param name="b">判定するIList</param>
		/// <param name="comparer">等価比較を行うインターフェイス。nullを指定した場合はデフォルトの判定を使用する。</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if IList are equal.
		/// </summary>
		/// <param name="a">IList to judge</param>
		/// <param name="b">IList to judge</param>
		/// <param name="comparer">Interface for equality comparison. When null is specified, default judgment is used.</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public static bool Equals(IList a, IList b, IEqualityComparer comparer = null)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}

			if (a == null || b == null)
			{
				return false;
			}

			CheckInstance(a, "a");
			CheckInstance(b, "b");

			int aCount = a.Count;
			int bCount = b.Count;
			if (aCount != bCount)
			{
				return false;
			}

			for (int i = 0; i < aCount; i++)
			{
				object aValue = a[i];
				object bValue = b[i];
				bool equals = comparer != null ? comparer.Equals(aValue, bValue) : object.Equals(aValue, bValue);
				if (!equals)
				{
					return false;
				}
			}

			return true;
		}

		private static int Internal_IndexOf(IList instance, object element, int startIndex, int count)
		{
			int listCount = instance.Count;
			if (listCount == 0)
			{
				return -1;
			}

			CheckIndex(instance, startIndex);
			if (count < 0)
			{
				throw new System.ArgumentOutOfRangeException("count", ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count > listCount - startIndex)
			{
				throw new System.ArgumentOutOfRangeException("count", ArgumentOutOfRange_BiggerThanCollection);
			}

			int num = startIndex + count;
			for (int i = startIndex; i < num; ++i)
			{
				var value = instance[i];
				if (object.Equals(value, element))
				{
					return i;
				}
			}

			return -1;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素が格納されているインデックスを取得する。
		/// </summary>
		/// <param name="instance">IList型のインスタンス</param>
		/// <param name="element">要素</param>
		/// <returns>要素が格納されているインデックス。要素がなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Gets the index where the element is stored.
		/// </summary>
		/// <param name="instance">Instance of type IList</param>
		/// <param name="element">Element</param>
		/// <returns>The index where the element is stored. Returns -1 if there is no element.</returns>
#endif
		public static int IndexOf(IList instance, object element)
		{
			CheckInstance(instance, "instance");

			return Internal_IndexOf(instance, element, 0, instance.Count);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素が格納されているインデックスを取得する。
		/// </summary>
		/// <param name="instance">IList型のインスタンス</param>
		/// <param name="element">要素</param>
		/// <param name="startIndex">開始インデックス</param>
		/// <returns>要素が格納されているインデックス。要素がなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Gets the index where the element is stored.
		/// </summary>
		/// <param name="instance">Instance of type IList</param>
		/// <param name="element">Element</param>
		/// <param name="startIndex">Start index</param>
		/// <returns>The index where the element is stored. Returns -1 if there is no element.</returns>
#endif
		public static int IndexOf(IList instance, object element, int startIndex)
		{
			CheckInstance(instance, "instance");

			return Internal_IndexOf(instance, element, startIndex, instance.Count - startIndex);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素が格納されているインデックスを取得する。
		/// </summary>
		/// <param name="instance">IList型のインスタンス</param>
		/// <param name="element">要素</param>
		/// <param name="startIndex">開始インデックス</param>
		/// <param name="count">個数</param>
		/// <returns>要素が格納されているインデックス。要素がなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Gets the index where the element is stored.
		/// </summary>
		/// <param name="instance">Instance of type IList</param>
		/// <param name="element">Element</param>
		/// <param name="startIndex">Start index</param>
		/// <param name="count">Count</param>
		/// <returns>The index where the element is stored. Returns -1 if there is no element.</returns>
#endif
		public static int IndexOf(IList instance, object element, int startIndex, int count)
		{
			CheckInstance(instance, "instance");

			return Internal_IndexOf(instance, element, startIndex, count);
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
				throw new System.ArgumentOutOfRangeException("index", ArgumentOutOfRange_NeedNonNegNum);
			}
			if (startIndex >= listCount)
			{
				throw new System.ArgumentOutOfRangeException("index", ArgumentOutOfRange_BiggerThanCollection);
			}

			if (count < 0)
			{
				throw new System.ArgumentOutOfRangeException("count", ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count > startIndex + 1)
			{
				throw new System.ArgumentOutOfRangeException("count", ArgumentOutOfRange_BiggerThanCollection);
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

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素が格納されているインデックスを末尾から検索する。
		/// </summary>
		/// <param name="instance">IList型のインスタンス</param>
		/// <param name="element">要素</param>
		/// <returns>要素が格納されているインデックス。要素がなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Search the index where the element is stored from the end.
		/// </summary>
		/// <param name="instance">Instance of type IList</param>
		/// <param name="element">Element</param>
		/// <returns>The index where the element is stored. Returns -1 if there is no element.</returns>
#endif
		public static int LastIndexOf(IList instance, object element)
		{
			CheckInstance(instance, "instance");

			int count = instance.Count;

			return Internal_LastIndexOf(instance, element, count - 1, count);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素が格納されているインデックスを末尾から検索する。
		/// </summary>
		/// <param name="instance">IList型のインスタンス</param>
		/// <param name="element">要素</param>
		/// <param name="startIndex">開始インデックス</param>
		/// <returns>要素が格納されているインデックス。要素がなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Search the index where the element is stored from the end.
		/// </summary>
		/// <param name="instance">Instance of type IList</param>
		/// <param name="element">Element</param>
		/// <param name="startIndex">Start index</param>
		/// <returns>The index where the element is stored. Returns -1 if there is no element.</returns>
#endif
		public static int LastIndexOf(IList instance, object element, int startIndex)
		{
			CheckInstance(instance, "instance");

			return Internal_LastIndexOf(instance, element, startIndex, startIndex + 1);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素が格納されているインデックスを末尾から検索する。
		/// </summary>
		/// <param name="instance">IList型のインスタンス</param>
		/// <param name="element">要素</param>
		/// <param name="startIndex">開始インデックス</param>
		/// <param name="count">個数</param>
		/// <returns>要素が格納されているインデックス。要素がなかった場合は-1を返す。</returns>
#else
		/// <summary>
		/// Search the index where the element is stored from the end.
		/// </summary>
		/// <param name="instance">Instance of type IList</param>
		/// <param name="element">Element</param>
		/// <param name="startIndex">Start index</param>
		/// <param name="count">Count</param>
		/// <returns>The index where the element is stored. Returns -1 if there is no element.</returns>
#endif
		public static int LastIndexOf(IList instance, object element, int startIndex, int count)
		{
			CheckInstance(instance, "instance");

			return Internal_LastIndexOf(instance, element, startIndex, count);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IList&lt;T&gt;が等しいかどうかを判断する。
		/// </summary>
		/// <typeparam name="T">要素の型</typeparam>
		/// <param name="a">判定するIList&lt;T&gt;</param>
		/// <param name="b">判定するIList&lt;T&gt;</param>
		/// <param name="comparer">等価比較を行うインターフェイス。nullを指定した場合はデフォルトの判定を使用する。</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if IList&lt;T&gt; are equal.
		/// </summary>
		/// <typeparam name="T">Element type</typeparam>
		/// <param name="a">IList&lt;T&gt; to judge</param>
		/// <param name="b">IList&lt;T&gt; to judge</param>
		/// <param name="comparer">Interface for equality comparison. When null is specified, default judgment is used.</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public static bool Equals<T>(IList<T> a, IList<T> b, IEqualityComparer<T> comparer = null)
		{
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}

			if (a == null || b == null || a.Count != b.Count)
			{
				return false;
			}

			if (comparer == null)
			{
				comparer = EqualityComparerEx<T>.Default;
			}

			for (int i = 0; i < a.Count; i++)
			{
				if (!comparer.Equals(a[i], b[i]))
				{
					return false;
				}
			}

			return true;
		}

		internal static void CheckInstance<T>(IList<T> list, string instanceName)
		{
			if (list == null)
			{
				throw new System.ArgumentNullException(instanceName);
			}
		}

		internal static void CheckIndex<T>(IList<T> list, int index)
		{
			if (index < 0)
			{
				throw new System.ArgumentOutOfRangeException("index", ListUtility.ArgumentOutOfRange_NeedNonNegNum);
			}

			int count = list.Count;
			if (index >= count)
			{
				throw new System.ArgumentOutOfRangeException("index", ListUtility.ArgumentOutOfRange_BiggerThanCollection);
			}
		}

		internal static void Copy<T>(IList<T> sourceList, int sourceIndex, IList<T> destinationList, int destinationIndex, int length)
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
				destinationList[i + destinationIndex] = sourceList[i + sourceIndex];
			}
		}

		internal static void AddRange<T>(IList<T> list, IList<T> addToList)
		{
			int count = addToList.Count;
			for (int i = 0; i < count; i++)
			{
				list.Add(addToList[i]);
			}
		}

		private static T[] Internal_ToArray<T>(IList<T> list)
		{
			int count = list.Count;
			T[] newArray = new T[count];
			Copy(list, 0, newArray, 0, count);
			return newArray;
		}

		internal static T[] ToArray<T>(IList<T> list)
		{
			CheckInstance(list, "list");

			return Internal_ToArray(list);
		}

		private static List<T> Internal_ToList<T>(IList<T> list)
		{
			return new List<T>(list);
		}

		internal static List<T> ToList<T>(IList<T> list)
		{
			CheckInstance(list, "list");

			return Internal_ToList(list);
		}

		internal static T[] AddToArray<T>(IList<T> source, T value)
		{
			int count = source.Count;
			T[] newArray = new T[count + 1];

			Copy(source, 0, newArray, 0, count);

			newArray[count] = value;

			return newArray;
		}

		internal static IList<T> AddElement<T>(IList<T> list, T value, ListInstanceType instanceType)
		{
			CheckInstance(list, "list");

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						list.Add(value);
						return list;
					}
				case ListInstanceType.NewArray:
					{
						return AddToArray(list, value);
					}
				case ListInstanceType.NewList:
					{
						List<T> newList = new List<T>(list);

						newList.Add(value);

						return newList;
					}
			}

			return list;
		}

		internal static IList<T> SetElement<T>(IList<T> list, int index, T value, ListInstanceType instanceType)
		{
			CheckInstance(list, "list");
			CheckIndex(list, index);

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						list[index] = value;
						return list;
					}
				case ListInstanceType.NewArray:
					{
						T[] newArray = Internal_ToArray(list);
						newArray[index] = value;
						return newArray;
					}
				case ListInstanceType.NewList:
					{
						List<T> newList = new List<T>(list);
						newList[index] = value;
						return newList;
					}
			}

			return list;
		}

		internal static T[] InsertToArray<T>(IList<T> source, int index, T value)
		{
			int count = source.Count;
			T[] newArray = new T[count + 1];

			Copy(source, 0, newArray, 0, index);
			newArray[index] = value;
			if (index < count)
			{
				Copy(source, index, newArray, index + 1, count - index);
			}

			return newArray;
		}

		internal static IList<T> InsertElement<T>(IList<T> list, int index, T value, ListInstanceType instanceType)
		{
			CheckInstance(list, "list");

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						list.Insert(index, value);
						return list;
					}
				case ListInstanceType.NewArray:
					{
						return InsertToArray(list, index, value);
					}
				case ListInstanceType.NewList:
					{
						List<T> newList = new List<T>(list);
						newList.Insert(index, value);
						return newList;
					}
			}

			return list;
		}

		internal static T[] RemoveAtToArray<T>(IList<T> source, int index)
		{
			CheckIndex(source, index);

			int count = source.Count;
			T[] newArray = new T[count - 1];
			Copy(source, 0, newArray, 0, index);
			if (index + 1 < count)
			{
				Copy(source, index + 1, newArray, index, count - (index + 1));
			}
			return newArray;
		}

		private static IList<T> Internal_RemoveAt<T>(IList<T> list, int index, ListInstanceType instanceType)
		{
			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						list.RemoveAt(index);
						return list;
					}
				case ListInstanceType.NewArray:
					{
						return RemoveAtToArray(list, index);
					}
				case ListInstanceType.NewList:
					{
						List<T> newList = new List<T>(list);
						newList.RemoveAt(index);
						return newList;
					}
			}

			return list;
		}

		internal static IList<T> RemoveElement<T>(IList<T> list, T value, ListInstanceType instanceType)
		{
			CheckInstance(list, "list");

			int index = list.IndexOf(value);
			if (index >= 0)
			{
				return Internal_RemoveAt(list, index, instanceType);
			}
			else
			{
				switch (instanceType)
				{
					case ListInstanceType.Keep:
						return list;
					case ListInstanceType.NewArray:
						return Internal_ToArray(list);
					case ListInstanceType.NewList:
						return new List<T>(list);
				}
			}

			return list;
		}

		internal static IList<T> RemoveAt<T>(IList<T> list, int index, ListInstanceType instanceType)
		{
			CheckInstance(list, "list");

			return Internal_RemoveAt(list, index, instanceType);
		}

		internal static IList<T> Clear<T>(IList<T> list, ListInstanceType instanceType)
		{
			CheckInstance(list, "list");

			switch (instanceType)
			{
				case ListInstanceType.Keep:
					{
						list.Clear();
						return list;
					}
				case ListInstanceType.NewArray:
					{
						int count = list.Count;
						return new T[count];
					}
				case ListInstanceType.NewList:
					{
						return new List<T>();
					}
			}

			return list;
		}

		private static int Internal_LastIndexOf<T>(IList<T> list, T value, int startIndex, int count)
		{
			int listCount = list.Count;
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
				if (EqualityComparerEx<T>.Default.Equals(value, list[i]))
				{
					return i;
				}
			}

			return -1;
		}

		internal static int LastIndexOf<T>(IList<T> list, T value)
		{
			CheckInstance(list, "list");

			int count = list.Count;

			return Internal_LastIndexOf(list, value, count - 1, count);
		}

		internal static int LastIndexOf<T>(IList<T> list, T value, int startIndex)
		{
			CheckInstance(list, "list");

			return Internal_LastIndexOf(list, value, startIndex, startIndex + 1);
		}

		internal static int LastIndexOf<T>(IList<T> list, T value, int startIndex, int count)
		{
			CheckInstance(list, "list");

			return Internal_LastIndexOf(list, value, startIndex, count);
		}
	}
}