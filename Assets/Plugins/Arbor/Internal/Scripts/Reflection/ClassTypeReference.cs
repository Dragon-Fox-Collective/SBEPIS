//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// シリアライズ可能な型の参照用クラス。
	/// </summary>
	/// <remarks>Editorから選択可能な型を制限するにはClassTypeConstraintAttributeを継承している属性を指定する。</remarks>
#else
	/// <summary>
	/// A reference class for serializable types.
	/// </summary>
	/// <remarks>To restrict selectable types from the Editor, specify an attribute that inherits ClassTypeConstraintAttribute.</remarks>
#endif
	[Serializable]
	public sealed class ClassTypeReference : System.IEquatable<ClassTypeReference>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private string _AssemblyTypeName;

		[NonSerialized]
		private Type _Type = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// 型を変更したときに呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when changing type
		/// </summary>
#endif
		public System.Action onTypeChanged;

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照している型
		/// </summary>
#else
		/// <summary>
		/// Reference type
		/// </summary>
#endif
		public Type type
		{
			get
			{
				return _Type;
			}
			set
			{
				if (_Type != value)
				{
					_Type = value;
					_AssemblyTypeName = TypeUtility.TidyAssemblyTypeName(_Type);

					onTypeChanged?.Invoke();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シリアライズ可能な形式の型名
		/// </summary>
#else
		/// <summary>
		/// Serializable format type name
		/// </summary>
#endif
		public string assemblyTypeName
		{
			get
			{
				return _AssemblyTypeName;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 何も指定しないでClassTypeReferenceを作成する。
		/// </summary>
#else
		/// <summary>
		/// Create a ClassTypeReference without specifying anything.
		/// </summary>
#endif
		public ClassTypeReference()
		{
			this.type = null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeを指定してClassTypeReferenceを作成する。
		/// </summary>
		/// <param name="type">参照する型</param>
#else
		/// <summary>
		/// To create a ClassTypeReference specify the Type.
		/// </summary>
		/// <param name="type">Reference type</param>
#endif
		public ClassTypeReference(Type type)
		{
			this.type = type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り当て可能な型かどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <returns>割り当て可能な型の場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It is judged whether or not it is assignable type.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <returns>Returns true if it is an assignable type.</returns>
#endif
		public bool IsAssignableFrom(Type type)
		{
			return TypeUtility.IsAssignableFrom(this._Type, type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceが等しいかを返す。
		/// </summary>
		/// <param name="x">ClassTypeReference</param>
		/// <param name="y">ClassTypeReference</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether ClassTypeReference is equal.
		/// </summary>
		/// <param name="x">ClassTypeReference</param>
		/// <param name="y">ClassTypeReference</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public static bool operator ==(ClassTypeReference x, ClassTypeReference y)
		{
			if (ReferenceEquals(x, y))
			{
				return true;
			}

			if ((object)x == null || (object)y == null)
			{
				return false;
			}

			return x.Equals(y);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceが等しくないかを返す。
		/// </summary>
		/// <param name="x">ClassTypeReference</param>
		/// <param name="y">ClassTypeReference</param>
		/// <returns>等しくない場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether ClassTypeReference are not equal.
		/// </summary>
		/// <param name="x">ClassTypeReference</param>
		/// <param name="y">ClassTypeReference</param>
		/// <returns>Returns true if it is not equal.</returns>
#endif
		public static bool operator !=(ClassTypeReference x, ClassTypeReference y)
		{
			return !(x == y);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceとTypeが等しいかを返す。
		/// </summary>
		/// <param name="typeRef">ClassTypeReferenceの値</param>
		/// <param name="type">Typeの値</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether ClassTypeReference is equal to Type.
		/// </summary>
		/// <param name="typeRef">Value of ClassTypeReference</param>
		/// <param name="type">Type value</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public static bool operator ==(ClassTypeReference typeRef, Type type)
		{
			if ((object)typeRef == null || (object)type == null)
			{
				return false;
			}

			return typeRef._Type == type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceとTypeが等しくないかを返す。
		/// </summary>
		/// <param name="typeRef">ClassTypeReferenceの値</param>
		/// <param name="type">Typeの値</param>
		/// <returns>等しくない場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether ClassTypeReference and Type are not equal.
		/// </summary>
		/// <param name="typeRef">Value of ClassTypeReference</param>
		/// <param name="type">Type value</param>
		/// <returns>Returns true if it is not equal.</returns>
#endif
		public static bool operator !=(ClassTypeReference typeRef, Type type)
		{
			return !(typeRef == type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceとTypeが等しいかを返す。
		/// </summary>
		/// <param name="type">Typeの値</param>
		/// <param name="typeRef">ClassTypeReferenceの値</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether ClassTypeReference is equal to Type.
		/// </summary>
		/// <param name="type">Type value</param>
		/// <param name="typeRef">Value of ClassTypeReference</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public static bool operator ==(Type type, ClassTypeReference typeRef)
		{
			if ((object)type == null || (object)typeRef == null)
			{
				return false;
			}

			return type == typeRef._Type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceとTypeが等しくないかを返す。
		/// </summary>
		/// <param name="type">Typeの値</param>
		/// <param name="typeRef">ClassTypeReferenceの値</param>
		/// <returns>等しくない場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether ClassTypeReference and Type are not equal.
		/// </summary>
		/// <param name="type">Type value</param>
		/// <param name="typeRef">Value of ClassTypeReference</param>
		/// <returns>Returns true if it is not equal.</returns>
#endif
		public static bool operator !=(Type type, ClassTypeReference typeRef)
		{
			return !(type == typeRef);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceと等しいかを返す。
		/// </summary>
		/// <param name="typeRef">ClassTypeReferenceの値</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether this is equal to ClassTypeReference.
		/// </summary>
		/// <param name="typeRef">Value of ClassTypeReference</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public bool Equals(ClassTypeReference typeRef)
		{
			if ((object)typeRef == null)
			{
				return false;
			}

			if (_Type != null && typeRef._Type != null)
			{
				return _Type == typeRef._Type;
			}

			return _AssemblyTypeName == typeRef._AssemblyTypeName;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeと等しいかを返す。
		/// </summary>
		/// <param name="type">Typeの値</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether this is equal to Type.
		/// </summary>
		/// <param name="type">Type value</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public bool Equals(Type type)
		{
			if ((object)type == null)
			{
				return false;
			}

			if (_Type != null)
			{
				return _Type == type;
			}

			return _AssemblyTypeName == TypeUtility.TidyAssemblyTypeName(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// objectと等しいかを返す。
		/// </summary>
		/// <param name="obj">objectの値</param>
		/// <returns>等しい場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether this is equal to object.
		/// </summary>
		/// <param name="obj">object value</param>
		/// <returns>Returns true if they are equal.</returns>
#endif
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			ClassTypeReference typeRef = obj as ClassTypeReference;
			if ((object)typeRef != null)
			{
				return Equals(typeRef);
			}

			Type type = obj as Type;
			if ((object)type != null)
			{
				return Equals(type);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ハッシュ値を取得する。
		/// </summary>
		/// <returns>ハッシュ値</returns>
#else
		/// <summary>
		/// Get a hash code.
		/// </summary>
		/// <returns>Hash code</returns>
#endif
		public override int GetHashCode()
		{
			return (_Type != null) ? _Type.GetHashCode() : 0;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeReferenceからTypeへキャストする。
		/// </summary>
		/// <param name="typeRef">ClassTypeReferenceの値</param>
		/// <returns>Typeにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast from ClassTypeReference to Type.
		/// </summary>
		/// <param name="typeRef">Value of ClassTypeReference</param>
		/// <returns>Returns the result of casting to Type.</returns>
#endif
		public static implicit operator Type(ClassTypeReference typeRef)
		{
			return typeRef._Type;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TypeからClassTypeReferenceへキャストする。
		/// </summary>
		/// <param name="type">Typeの値</param>
		/// <returns>ClassTypeReferenceにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast from Type to ClassTypeReference.
		/// </summary>
		/// <param name="type">Value of Type</param>
		/// <returns>Returns the result of casting to ClassTypeReference.</returns>
#endif
		public static implicit operator ClassTypeReference(Type type)
		{
			return new ClassTypeReference(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 文字列に変換する。
		/// </summary>
		/// <returns>変換した文字列</returns>
#else
		/// <summary>
		/// Convert to a string.
		/// </summary>
		/// <returns>Converted string</returns>
#endif
		public override string ToString()
		{
			if (_Type != null)
			{
				return _Type.ToString();
			}
			else if (!string.IsNullOrEmpty(_AssemblyTypeName))
			{
				return "(Missing)" + _AssemblyTypeName;
			}
			else
			{
				return "(None)";
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			var type = TypeUtility.GetAssemblyType(_AssemblyTypeName);
			if (!string.IsNullOrEmpty(_AssemblyTypeName) && type == null)
			{
				int index = _AssemblyTypeName.IndexOf(',');
				string oldTypeName = (index >= 0) ? _AssemblyTypeName.Substring(0, index) : _AssemblyTypeName;
				type = TypeUtility.GetTypeRenamedFrom(oldTypeName);
				if (type == null)
				{
					type = TypeUtility.FindType(oldTypeName);
				}
			}

			this.type = type;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Typeをシリアライズ可能な文字列に変換する。
		/// </summary>
		/// <param name="type">Typeの値</param>
		/// <returns>シリアライズ可能な文字列</returns>
#else
		/// <summary>
		/// Convert Type to a serializable string.
		/// </summary>
		/// <param name="type">Type value</param>
		/// <returns>Serializable string</returns>
#endif
		[Obsolete("use TypeUtility.TidyAssemblyTypeName (Type)")]
		public static string TidyAssemblyTypeName(Type type)
		{
			return TypeUtility.TidyAssemblyTypeName(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 文字列からSystem.Typeを取得する。
		/// </summary>
		/// <param name="assemblyTypeName">型名</param>
		/// <returns>System.Type。assemblyTypeNameが空かnullの場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get System.Type from string.
		/// </summary>
		/// <param name="assemblyTypeName">Type name</param>
		/// <returns>System.Type. If assemblyTypeName is empty or null, it returns null.</returns>
#endif
		[Obsolete("use TypeUtility.GetAssemblyType(string)")]
		public static Type GetAssemblyType(string assemblyTypeName)
		{
			return TypeUtility.GetAssemblyType(assemblyTypeName);
		}
	}
}
