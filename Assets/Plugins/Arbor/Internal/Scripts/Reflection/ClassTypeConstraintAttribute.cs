//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System;
using System.Reflection;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対して型の制約を行う基本Attributeクラス。
	/// </summary>
	/// <remarks>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// A basic Attribute class that imposes a type constraint on a field.
	/// </summary>
	/// <remarks>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public abstract class ClassTypeConstraintAttribute : Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public virtual Type GetBaseType(FieldInfo fieldInfo)
		{
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の型名を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の型名</returns>
#else
		/// <summary>
		/// Returns the type name of the constraint.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Type name of constraint</returns>
#endif
		public virtual string GetTypeName(FieldInfo fieldInfo)
		{
			Type type = GetBaseType(fieldInfo);
			return TypeUtility.GetSlotTypeName(type);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public abstract bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo);
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対して指定した型から継承しているクラスのみに制限する属性。
	/// </summary>
	/// <remarks>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Attribute to restrict to classes inherited from the type specified for the field.
	/// </summary>
	/// <remarks>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassExtendsAttribute : ClassTypeConstraintAttribute
	{
		private Type _BaseType;

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			return _BaseType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassExtendsAttributeを型を指定して作成する。
		/// </summary>
		/// <param name="baseType">基本となる型</param>
#else
		/// <summary>
		/// Create a ClassExtendsAttribute with a specified type.
		/// </summary>
		/// <param name="baseType">Basic type</param>
#endif
		public ClassExtendsAttribute(Type baseType)
		{
			_BaseType = baseType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return TypeUtility.IsAssignableFrom(_BaseType, type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してUnityEngine.Objectから継承しているクラスのみに制限する属性。
	/// </summary>
	/// <remarks>
	/// NodeBehaviourから継承しているクラスは除く。<br/>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Attribute to restrict to classes inherited from UnityEngine.Object for fields.
	/// </summary>
	/// <remarks>
	/// Except for classes inherited from NodeBehaviour. <br/>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassUnityObjectAttribute : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			return typeof(UnityEngine.Object);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return TypeUtility.IsAssignableFrom(typeof(UnityEngine.Object), type) && !TypeUtility.IsAssignableFrom(typeof(NodeBehaviour), type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してUnityEngine.Objectから継承していない型のみに制限する属性。
	/// </summary>
	/// <remarks>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// An attribute that restricts the field to only types that are not inherited from UnityEngine.Object.
	/// </summary>
	/// <remarks>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassNotUnityObjectAttribute : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			return typeof(object);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return !TypeUtility.IsAssignableFrom(typeof(UnityEngine.Object), type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してUnityEngine.Object(アセット)から継承しているクラスのみに制限する属性。
	/// </summary>
	/// <remarks>
	/// Componentから継承しているクラスかGameObjectは除く。<br/>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Attribute to restrict to classes inherited from UnityEngine.Object(asset) for fields.
	/// </summary>
	/// <remarks>
	/// Except for classes inherited from Component or GameObject. <br/>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassAssetObjectAttribute : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			return typeof(UnityEngine.Object);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			if (!TypeUtility.IsAssignableFrom(typeof(UnityEngine.Object), type))
			{
				return false;
			}

			return !(TypeUtility.IsAssignableFrom(typeof(Component), type) || TypeUtility.IsAssignableFrom(typeof(GameObject), type));
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してUnityEngine.Componentから継承しているクラスのみに制限する属性。
	/// </summary>
	/// <remarks>
	/// NodeBehaviourから継承しているクラスは除く。<br/>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Attribute to restrict to classes inherited from UnityEngine.Component for fields.
	/// </summary>
	/// <remarks>
	/// Except for classes inherited from NodeBehaviour. <br/>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassComponentAttribute : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			return typeof(Component);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return TypeUtility.IsAssignableFrom(typeof(Component), type) && !TypeUtility.IsAssignableFrom(typeof(NodeBehaviour), type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してNodeBehaviour以外から継承しているクラスのみに制限する属性。
	/// </summary>
	/// <remarks>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// An attribute that restricts the field to only classes that inherit from other than NodeBehaviour.
	/// </summary>
	/// <remarks>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassNotNodeBehaviourAttribute : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			return typeof(object);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return !TypeUtility.IsAssignableFrom(typeof(NodeBehaviour), type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してUnityEngine.ScriptableObjectから継承しているクラスのみに制限する属性。
	/// </summary>
	/// <remarks>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Attribute to restrict to classes inherited from UnityEngine.ScriptableObject for fields.
	/// </summary>
	/// <remarks>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassScriptableObjectAttribute : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			return typeof(ScriptableObject);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return TypeUtility.IsAssignableFrom(typeof(ScriptableObject), type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してジェネリックの引数に指定されている型のみに制限する属性。
	/// </summary>
	/// <remarks>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Attribute to restrict to field only for types specified as generic arguments.
	/// </summary>
	/// <remarks>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeReference"/></description></item>
	/// <item><description><see cref="AnyParameterReference"/></description></item>
	/// <item><description><see cref="ComponentParameterReference"/></description></item>
	/// <item><description><see cref="InputSlotAny"/></description></item>
	/// <item><description><see cref="InputSlotComponent"/></description></item>
	/// <item><description><see cref="InputSlotUnityObject"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassGenericArgumentAttribute : ClassTypeConstraintAttribute
	{
		private int _GenericArgumentIndex = 0;

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassExtendsAttributeを型を指定して作成する。
		/// </summary>
		/// <param name="genericArgumentIndex">ジェネリック引数のインデックス</param>
#else
		/// <summary>
		/// Create a ClassExtendsAttribute with a specified type.
		/// </summary>
		/// <param name="genericArgumentIndex">Index of generic argument</param>
#endif
		public ClassGenericArgumentAttribute(int genericArgumentIndex)
		{
			_GenericArgumentIndex = genericArgumentIndex;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の基本型</returns>
#else
		/// <summary>
		/// It returns the base type of constraints.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>The base type of constraints</returns>
#endif
		public override Type GetBaseType(FieldInfo fieldInfo)
		{
			Type declaringType = fieldInfo.DeclaringType;
			if (!TypeUtility.IsGenericType(declaringType))
			{
				return null;
			}

			Type[] arguments = TypeUtility.GetGenericArguments(declaringType);

			if (arguments == null || _GenericArgumentIndex < 0 || arguments.Length <= _GenericArgumentIndex)
			{
				return null;
			}

			Type constraintType = arguments[_GenericArgumentIndex];

			return constraintType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			Type declaringType = fieldInfo.DeclaringType;
			if (!TypeUtility.IsGenericType(declaringType))
			{
				return false;
			}

			Type[] arguments = TypeUtility.GetGenericArguments(declaringType);

			if (arguments == null || _GenericArgumentIndex < 0 || arguments.Length <= _GenericArgumentIndex)
			{
				return false;
			}

			Type constraintType = arguments[_GenericArgumentIndex];

			return TypeUtility.IsAssignableFrom(constraintType, type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してenum型のみに制約する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute to restrict to enum types only for fields.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassEnumFieldConstraint : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の型名を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の型名</returns>
#else
		/// <summary>
		/// Returns the type name of the constraint.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Type name of constraint</returns>
#endif
		public override string GetTypeName(FieldInfo fieldInfo)
		{
			Type type = GetBaseType(fieldInfo);
			if (type != null)
			{
				return TypeUtility.GetSlotTypeName(type);
			}
			return "Enum";
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return EnumFieldUtility.IsEnum(type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してenum型(System.FlagsAttributeあり)のみに制約する属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute to restrict to enum types (With System.FlagsAttribute) only for fields.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassEnumFlagsConstraint : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の型名を返す。
		/// </summary>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約の型名</returns>
#else
		/// <summary>
		/// Returns the type name of the constraint.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Type name of constraint</returns>
#endif
		public override string GetTypeName(FieldInfo fieldInfo)
		{
			Type type = GetBaseType(fieldInfo);
			if (type != null)
			{
				return TypeUtility.GetSlotTypeName(type);
			}
			return "EnumFlags";
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return EnumFieldUtility.IsEnumFlags(type);
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// フィールドに対してstaticクラス以外に制約する属性。
	/// </summary>
#else
	/// <summary>
	/// Attributes that restrict fields other than static classes.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ClassNotStaticConstraint : ClassTypeConstraintAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約が満たされるかどうかを判定する。
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <param name="fieldInfo">この属性が指定されているFieldInfo</param>
		/// <returns>制約が満たされる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the constraint is satisfied is judged.
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <param name="fieldInfo">FieldInfo with this attribute specified</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Type type, FieldInfo fieldInfo)
		{
			return !TypeUtility.IsStatic(type);
		}
	}
}