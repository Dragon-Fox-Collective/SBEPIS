//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// クラスの制約情報
	/// </summary>
#else
	/// <summary>
	/// Class constraint information
	/// </summary>
#endif
	public sealed class ClassConstraintInfo
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 基本型
		/// </summary>
#else
		/// <summary>
		/// Base type
		/// </summary>
#endif
		public System.Type baseType;

		/// <summary>
		/// ClassTypeConstraintAttribute
		/// </summary>
		public ClassTypeConstraintAttribute constraintAttribute;

#if ARBOR_DOC_JA
		/// <summary>
		/// ClassTypeConstraintAttributeが付与されているフィールドのFieldInfo
		/// </summary>
#else
		/// <summary>
		/// FieldInfo of the field to which ClassTypeConstraintAttribute is given
		/// </summary>
#endif
		public System.Reflection.FieldInfo constraintFieldInfo;

		/// <summary>
		/// SlotTypeAttribute
		/// </summary>
		public SlotTypeAttribute slotTypeAttribute;

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約を満たすか判定する
		/// </summary>
		/// <param name="type">判定する型</param>
		/// <returns>制約を満たしているときtrueを返す。</returns>
#else
		/// <summary>
		/// Determine whether the constraint is satisfied
		/// </summary>
		/// <param name="type">Determining type</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public bool IsConstraintSatisfied(System.Type type)
		{
			bool isAny = baseType == null || baseType == typeof(object);

			if (!isAny)
			{
				return TypeUtility.IsAssignableFrom(baseType, type);
			}

			if (constraintAttribute != null)
			{
				return constraintAttribute.IsConstraintSatisfied(type, constraintFieldInfo);
			}

			if (slotTypeAttribute != null)
			{
				System.Type connectableType = slotTypeAttribute.connectableType;

				bool isAnySlotType = connectableType == null || connectableType == typeof(object);
				if (!isAnySlotType)
				{
					return TypeUtility.IsAssignableFrom(connectableType, type);
				}
			}

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型を取得する。
		/// </summary>
		/// <returns>制約の基本型を返す</returns>
#else
		/// <summary>
		/// Get the base type of constraint.
		/// </summary>
		/// <returns>Return base type of constraint</returns>
#endif
		public System.Type GetConstraintBaseType()
		{
			if (baseType != null)
			{
				return baseType;
			}

			if (constraintAttribute != null)
			{
				return constraintAttribute.GetBaseType(constraintFieldInfo);
			}

			if (slotTypeAttribute != null)
			{
				return slotTypeAttribute.connectableType;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の型名を返す。
		/// </summary>
		/// <returns>制約の型名。制約がない場合はAnyを返す。</returns>
#else
		/// <summary>
		/// Returns the type name of the constraint.
		/// </summary>
		/// <returns>Type name of the constraint. Returns Any if there are no constraints.</returns>
#endif
		public string GetConstraintTypeName()
		{
			if (baseType != null)
			{
				return TypeUtility.GetSlotTypeName(baseType);
			}

			if (constraintAttribute != null)
			{
				return constraintAttribute.GetTypeName(constraintFieldInfo);
			}

			if (slotTypeAttribute != null)
			{
				return TypeUtility.GetSlotTypeName(slotTypeAttribute.connectableType);
			}

			return "Any";
		}
	}
}