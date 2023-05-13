//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Arbor
{
	using Arbor.DynamicReflection;
	using Arbor.Serialization;

	internal static class EachFieldUtility
	{
		public static bool IsIgnoreType(System.Type type)
		{
			return TypeUtility.IsPrimitive(type) ||
				TypeUtility.IsEnum(type) ||
				type == typeof(string) ||
				TypeUtility.IsAssignableFrom(typeof(Object), type);
		}

		public static bool IsIgnoreDeclaringType(System.Type type)
		{
			return type == typeof(object) ||
				type == typeof(Object) ||
				type == typeof(Component) ||
				type == typeof(MonoBehaviour) ||
				type == typeof(ScriptableObject);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// シリアライズ可能な各フィールドを見つける
	/// </summary>
	/// <typeparam name="T">見つける型</typeparam>
#else
	/// <summary>
	/// Find each serializable field
	/// </summary>
	/// <typeparam name="T">Type to find</typeparam>
#endif
	public sealed class EachField<T>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 見つかった時のコールバック
		/// </summary>
		/// <param name="value">見つかった値</param>
#else
		/// <summary>
		/// Callback when found.
		/// </summary>
		/// <param name="value">find value</param>
#endif
		public delegate void OnFind(T value);

#if ARBOR_DOC_JA
		/// <summary>
		/// 見つかった時のコールバック
		/// </summary>
		/// <param name="value">見つかった値</param>
		/// <param name="fieldInfo">ValueのFieldInfo</param>
#else
		/// <summary>
		/// Callback when found.
		/// </summary>
		/// <param name="value">find value</param>
		/// <param name="fieldInfo">FieldInfo of Value</param>
#endif
		public delegate void OnFindEx(T value, FieldInfo fieldInfo);

#if ARBOR_DOC_JA
		/// <summary>
		/// 見つけた値を受け取るインターフェイス
		/// </summary>
#else
		/// <summary>
		/// Interface to receive the found value
		/// </summary>
#endif
		public interface IFindReceiver
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 見つかった時のコールバック
			/// </summary>
			/// <param name="value">見つかった値</param>
			/// <param name="fieldInfo">ValueのFieldInfo</param>
#else
			/// <summary>
			/// Callback when found.
			/// </summary>
			/// <param name="value">find value</param>
			/// <param name="fieldInfo">FieldInfo of Value</param>
#endif
			void OnFind(T value, FieldInfo fieldInfo);
		}

		static Dictionary<System.Type, TypeElement> s_Types = new Dictionary<System.Type, TypeElement>();

		static TypeElement GetTypeElement(System.Type type)
		{
			if (EachFieldUtility.IsIgnoreDeclaringType(type))
			{
				return null;
			}

			TypeElement typeElement = null;
			if (!s_Types.TryGetValue(type, out typeElement))
			{
				typeElement = new TypeElement(type);
				s_Types.Add(type, typeElement);
			}

			return typeElement;
		}

		static bool HasFields(System.Type type)
		{
			for (TypeElement typeElement = GetTypeElement(type); typeElement != null; typeElement = typeElement.baseType)
			{
				if (typeElement.fields.Count > 0)
				{
					return true;
				}
			}

			return false;
		}

		static bool IsTargetType(System.Type type)
		{
			return TypeUtility.IsAssignableFrom(typeof(T), type);
		}

		private sealed class FieldElement
		{
			public enum FieldType
			{
				Target,
				Array,
				InnerField,
			}

			public FieldType fieldType
			{
				get;
				private set;
			}

			public DynamicField dynamicField
			{
				get;
				private set;
			}

			public System.Type elementType
			{
				get;
				private set;
			}

			public FieldInfo fieldInfo
			{
				get
				{
					return dynamicField.fieldInfo;
				}
			}

			public FieldElement arrayElement
			{
				get;
				private set;
			}

			private FieldElement(FieldInfo fieldInfo, System.Type elementType, FieldType fieldType)
			{
				this.dynamicField = DynamicField.GetField(fieldInfo);
				this.fieldType = fieldType;
				this.elementType = elementType;

				switch (fieldType)
				{
					case FieldType.Target:
						{
							this.arrayElement = null;
						}
						break;
					case FieldType.Array:
						{
							this.arrayElement = Create(fieldInfo, elementType);
						}
						break;
					case FieldType.InnerField:
						{
							this.arrayElement = null;
						}
						break;
				}
			}

			public object GetValue(object instance)
			{
				return dynamicField.GetValue(instance);
			}

			static bool IsValidFieldType(System.Type fieldType)
			{
				return !EachFieldUtility.IsIgnoreType(fieldType) &&
					TypeUtility.IsSerializable(fieldType) &&
					HasFields(fieldType);
			}

			static FieldElement Create(FieldInfo fieldInfo, System.Type fieldType)
			{
				if (IsTargetType(fieldType))
				{
					return new FieldElement(fieldInfo, fieldType, FieldType.Target);
				}
				else if (SerializationUtility.IsSupportedArray(fieldType))
				{
					System.Type elementType = SerializationUtility.ElementTypeOfArray(fieldType);

					if (IsTargetType(elementType) || IsValidFieldType(elementType))
					{
						return new FieldElement(fieldInfo, elementType, FieldType.Array);
					}
				}
				else if (IsValidFieldType(fieldType))
				{
					return new FieldElement(fieldInfo, fieldType, FieldType.InnerField);
				}

				return null;
			}

			public static FieldElement Create(FieldInfo fieldInfo)
			{
				return Create(fieldInfo, fieldInfo.FieldType);
			}
		}

		private sealed class TypeElement
		{
			private List<FieldElement> _Fields = new List<FieldElement>();

			public List<FieldElement> fields
			{
				get
				{
					return _Fields;
				}
			}

			public TypeElement baseType
			{
				get;
				private set;
			}

			public TypeElement(System.Type type)
			{
				FieldInfo[] fields = FieldCache.GetFields(type);

				for (int i = 0, count = fields.Length; i < count; i++)
				{
					FieldInfo fieldInfo = fields[i];
					FieldElement fieldElement = FieldElement.Create(fieldInfo);

					if (fieldElement != null)
					{
						_Fields.Add(fieldElement);
					}
				}

				System.Type baseType = TypeUtility.GetBaseType(type);

				if (baseType != null)
				{
					this.baseType = GetTypeElement(baseType);
				}
				else
				{
					this.baseType = null;
				}
			}
		}

		private class FindFieldBase
		{
			public virtual void OnFindField(T obj, FieldInfo fieldInfo)
			{
			}

			void FindField(object fieldObj, FieldElement fieldElement)
			{
				if (fieldObj == null)
				{
					return;
				}

				FieldInfo fieldInfo = fieldElement.fieldInfo;

				switch (fieldElement.fieldType)
				{
					case FieldElement.FieldType.Target:
						{
							OnFindField((T)fieldObj, fieldInfo);
						}
						break;
					case FieldElement.FieldType.Array:
						{
							IList list = (IList)fieldObj;
							FieldElement arrayElement = fieldElement.arrayElement;
							if (list != null && arrayElement != null)
							{
								int itemCount = list.Count;
								for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
								{
									object itemObj = list[itemIndex];
									FindField(itemObj, arrayElement);
								}
							}
						}
						break;
					case FieldElement.FieldType.InnerField:
						{
							var elementType = fieldElement.elementType;
							if (TypeUtility.IsClass(elementType) && AttributeHelper.HasAttribute<SerializeReference>(fieldInfo))
							{
								if( fieldObj == null )
								{
									break;
								}

								elementType = fieldObj.GetType();
							}
							FindFields(fieldObj, elementType);
						}
						break;
				}
			}

			void FindFields(object obj, System.Type type)
			{
				for (TypeElement typeElement = GetTypeElement(type); typeElement != null; typeElement = typeElement.baseType)
				{
					List<FieldElement> fields = typeElement.fields;
					int fieldCount = fields.Count;
					for (int i = 0; i < fieldCount; i++)
					{
						FieldElement field = fields[i];

						FindField(field.GetValue(obj), field);
					}
				}
			}

			public void Find(object obj, System.Type type, bool ignoreRoot)
			{
				if (!ignoreRoot && IsTargetType(type))
				{
					OnFindField((T)obj, null);
				}
				else if (SerializationUtility.IsSupportedArray(type))
				{
					System.Type elementType = SerializationUtility.ElementTypeOfArray(type);

					IList list = (IList)obj;
					if (list != null)
					{
						int itemCount = list.Count;
						for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
						{
							object itemObj = list[itemIndex];
							if (IsTargetType(elementType))
							{
								OnFindField((T)itemObj, null);
							}
							else
							{
								FindFields(itemObj, elementType);
							}
						}
					}
				}
				else
				{
					FindFields(obj, type);
				}
			}
		}

		private sealed class FindField : FindFieldBase
		{
			public OnFind onFind;

			public override void OnFindField(T obj, FieldInfo fieldInfo)
			{
				onFind(obj);
			}
		}

		private sealed class FindFieldEx : FindFieldBase
		{
			public OnFindEx onFind;

			public override void OnFindField(T obj, FieldInfo fieldInfo)
			{
				onFind(obj, fieldInfo);
			}
		}

		private sealed class FindFieldList : FindFieldBase
		{
			public List<T> list;

			public override void OnFindField(T obj, FieldInfo fieldInfo)
			{
				list.Add(obj);
			}
		}

		private sealed class FindFieldReceiver : FindFieldBase
		{
			public IFindReceiver receiver;

			public override void OnFindField(T obj, FieldInfo fieldInfo)
			{
				receiver.OnFind(obj, fieldInfo);
			}
		}

		static FindField s_Find = new FindField();
		static FindFieldEx s_FindEx = new FindFieldEx();
		static FindFieldList s_FindList = new FindFieldList();
		static FindFieldReceiver s_FindReceiver = new FindFieldReceiver();

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを取得する。
		/// </summary>
		/// <param name="type">オブジェクトの型</param>
		/// <returns>フィールドの列挙子</returns>
#else
		/// <summary>
		/// Get Fields
		/// </summary>
		/// <param name="type">Object type</param>
		/// <returns>Field enumerator</returns>
#endif
		public static IEnumerable<DynamicField> GetFields(System.Type type)
		{
			for (TypeElement typeElement = GetTypeElement(type); typeElement != null; typeElement = typeElement.baseType)
			{
				List<FieldElement> fields = typeElement.fields;
				int fieldCount = fields.Count;
				for (int i = 0; i < fieldCount; i++)
				{
					FieldElement field = fields[i];

					yield return field.dynamicField;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つける。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="onFind">見つけた場合のコールバック</param>
#else
		/// <summary>
		/// Find the field.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="onFind">Callback If you find</param>
#endif
		public static void Find(object rootObj, System.Type type, OnFind onFind)
		{
			Find(rootObj, type, onFind, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つける。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="onFind">見つけた場合のコールバック</param>
		/// <param name="ignoreRoot">rootObjを対象外とするフラグ</param>
#else
		/// <summary>
		/// Find the field.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="onFind">Callback If you find</param>
		/// <param name="ignoreRoot">Flags that exclude rootObj</param>
#endif
		public static void Find(object rootObj, System.Type type, OnFind onFind, bool ignoreRoot)
		{
			s_Find.onFind = onFind;
			s_Find.Find(rootObj, type, ignoreRoot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つける。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="onFind">見つけた場合のコールバック</param>
#else
		/// <summary>
		/// Find the field.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="onFind">Callback If you find</param>
#endif
		public static void Find(object rootObj, System.Type type, OnFindEx onFind)
		{
			Find(rootObj, type, onFind, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つける。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="onFind">見つけた場合のコールバック</param>
		/// <param name="ignoreRoot">rootObjを対象外とするフラグ</param>
#else
		/// <summary>
		/// Find the field.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="onFind">Callback If you find</param>
		/// <param name="ignoreRoot">Flags that exclude rootObj</param>
#endif
		public static void Find(object rootObj, System.Type type, OnFindEx onFind, bool ignoreRoot)
		{
			s_FindEx.onFind = onFind;
			s_FindEx.Find(rootObj, type, ignoreRoot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つけListで返す。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <returns>見つけた値のリスト</returns>
#else
		/// <summary>
		/// Find the field and return it as a List.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <returns>List of values found</returns>
#endif
		public static List<T> Find(object rootObj, System.Type type)
		{
			return Find(rootObj, type, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つけListで返す。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="ignoreRoot">rootObjを対象外とするフラグ</param>
		/// <returns>見つけた値のリスト</returns>
#else
		/// <summary>
		/// Find the field and return it as a List.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="ignoreRoot">Flags that exclude rootObj</param>
		/// <returns>List of values found</returns>
#endif
		public static List<T> Find(object rootObj, System.Type type, bool ignoreRoot)
		{
			s_FindList.list = new List<T>();
			s_FindList.Find(rootObj, type, ignoreRoot);
			return s_FindList.list;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つけListに追加する。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="list">格納するList</param>
#else
		/// <summary>
		/// Find the field and add it to the List.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="list">List to store</param>
#endif
		public static void Find(object rootObj, System.Type type,List<T> list)
		{
			Find(rootObj, type, list, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つけListに追加する。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="list">格納するList</param>
		/// <param name="ignoreRoot">rootObjを対象外とするフラグ</param>
#else
		/// <summary>
		/// Find the field and add it to the List.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="list">List to store</param>
		/// <param name="ignoreRoot">Flags that exclude rootObj</param>
#endif
		public static void Find(object rootObj, System.Type type, List<T> list, bool ignoreRoot)
		{
			s_FindList.list = list;
			s_FindList.Find(rootObj, type, ignoreRoot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つけIFindReceiverのOnFindを呼ぶ。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="receiver">受取先</param>
#else
		/// <summary>
		/// Find the field and call OnFind of IFindReceiver.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="receiver">Receiver</param>
#endif
		public static void Find(object rootObj, System.Type type, IFindReceiver receiver)
		{
			Find(rootObj, type, receiver, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドを見つけIFindReceiverのOnFindを呼ぶ。
		/// </summary>
		/// <param name="rootObj">オブジェクト</param>
		/// <param name="type">オブジェクトの型</param>
		/// <param name="receiver">受取先</param>
		/// <param name="ignoreRoot">rootObjを対象外とするフラグ</param>
#else
		/// <summary>
		/// Find the field and call OnFind of IFindReceiver.
		/// </summary>
		/// <param name="rootObj">Object</param>
		/// <param name="type">Type of object</param>
		/// <param name="receiver">Receiver</param>
		/// <param name="ignoreRoot">Flags that exclude rootObj</param>
#endif
		public static void Find(object rootObj, System.Type type, IFindReceiver receiver, bool ignoreRoot)
		{
			s_FindReceiver.receiver = receiver;
			s_FindReceiver.Find(rootObj, type, ignoreRoot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// キャッシュをクリア
		/// </summary>
#else
		/// <summary>
		/// Clear cache
		/// </summary>
#endif
		public static void ClearCache()
		{
			s_Types.Clear();
		}

		//public static void DebugList()
		//{
		//	System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//	foreach (KeyValuePair<System.Type, TypeElement> pair in s_Types)
		//	{
		//		System.Type type = pair.Key;
		//		TypeElement element = pair.Value;

		//		if (element != null)
		//		{
		//			sb.AppendLine(type.AssemblyQualifiedName);

		//			var fields = element.fields;
		//			for (int fieldIndex = 0; fieldIndex < fields.Count; fieldIndex++)
		//			{
		//				FieldElement field = fields[fieldIndex];
		//				sb.AppendFormat("\t{0} {1}\n", field.fieldInfo.FieldType.Name, field.fieldInfo.Name);
		//			}
		//		}
		//	}

		//	Debug.Log(sb.ToString());
		//}
	}
}
