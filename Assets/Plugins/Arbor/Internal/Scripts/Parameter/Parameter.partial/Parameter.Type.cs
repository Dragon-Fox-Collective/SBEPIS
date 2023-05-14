//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの型。
		/// </summary>
#else
		/// <summary>
		/// Parameter type.
		/// </summary>
#endif
		public enum Type
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// Int型。
			/// </summary>
#else
			/// <summary>
			/// Int type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(int), "Primitive/Int")]
			Int,

#if ARBOR_DOC_JA
			/// <summary>
			/// Float型。
			/// </summary>
#else
			/// <summary>
			/// Float type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(float), "Primitive/Float")]
			Float,

#if ARBOR_DOC_JA
			/// <summary>
			/// Bool型。
			/// </summary>
#else
			/// <summary>
			/// Bool type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(bool), "Primitive/Bool")]
			Bool,

#if ARBOR_DOC_JA
			/// <summary>
			/// GameObject型。
			/// </summary>
#else
			/// <summary>
			/// GameObject type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(GameObject), "UnityObject/GameObject")]
			GameObject,

#if ARBOR_DOC_JA
			/// <summary>
			/// String型。
			/// </summary>
#else
			/// <summary>
			/// String type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(string), "Primitive/String")]
			String,

#if ARBOR_DOC_JA
			/// <summary>
			/// Enum型。
			/// </summary>
#else
			/// <summary>
			/// Enum type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(System.Enum), "Primitive/Enum", useReferenceType = true)]
			Enum,

#if ARBOR_DOC_JA
			/// <summary>
			/// Vector2型。
			/// </summary>
#else
			/// <summary>
			/// Vector2 type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Vector2), "UnityStruct/Vector2")]
			Vector2 = 1000,

#if ARBOR_DOC_JA
			/// <summary>
			/// Vector3型。
			/// </summary>
#else
			/// <summary>
			/// Vector3 type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Vector3), "UnityStruct/Vector3")]
			Vector3,

#if ARBOR_DOC_JA
			/// <summary>
			/// Quaternion型。
			/// </summary>
#else
			/// <summary>
			/// Quaternion type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Quaternion), "UnityStruct/Quaternion")]
			Quaternion,

#if ARBOR_DOC_JA
			/// <summary>
			/// Rect型。
			/// </summary>
#else
			/// <summary>
			/// Rect type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Rect), "UnityStruct/Rect")]
			Rect,

#if ARBOR_DOC_JA
			/// <summary>
			/// Bounds型。
			/// </summary>
#else
			/// <summary>
			/// Bounds type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Bounds), "UnityStruct/Bounds")]
			Bounds,

#if ARBOR_DOC_JA
			/// <summary>
			/// Color型。
			/// </summary>
#else
			/// <summary>
			/// Color type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Color), "UnityStruct/Color")]
			Color,

#if ARBOR_DOC_JA
			/// <summary>
			/// Vector4型。
			/// </summary>
#else
			/// <summary>
			/// Vector4 type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Vector4), "UnityStruct/Vector4")]
			Vector4,

#if ARBOR_DOC_JA
			/// <summary>
			/// Vector2Int型。
			/// </summary>
#else
			/// <summary>
			/// Vector2Int type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Vector2Int), "UnityStruct/Vector2Int")]
			Vector2Int = 1100,

#if ARBOR_DOC_JA
			/// <summary>
			/// Vector3Int型。
			/// </summary>
#else
			/// <summary>
			/// Vector3Int type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Vector3Int), "UnityStruct/Vector3Int")]
			Vector3Int,

#if ARBOR_DOC_JA
			/// <summary>
			/// RectInt型。
			/// </summary>
#else
			/// <summary>
			/// RectInt type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(RectInt), "UnityStruct/RectInt")]
			RectInt,

#if ARBOR_DOC_JA
			/// <summary>
			/// BoundsInt型。
			/// </summary>
#else
			/// <summary>
			/// BoundsInt type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(BoundsInt), "UnityStruct/BoundsInt")]
			BoundsInt,

#if ARBOR_DOC_JA
			/// <summary>
			/// Transform型。
			/// </summary>
#else
			/// <summary>
			/// Transform type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Transform), "UnityObject/Transform")]
			Transform = 2000,

#if ARBOR_DOC_JA
			/// <summary>
			/// RectTransform型。
			/// </summary>
#else
			/// <summary>
			/// RectTransform type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(RectTransform), "UnityObject/RectTransform")]
			RectTransform,

#if ARBOR_DOC_JA
			/// <summary>
			/// Rigidbody型。
			/// </summary>
#else
			/// <summary>
			/// Rigidbody type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Rigidbody), "UnityObject/Rigidbody")]
			Rigidbody,

#if ARBOR_DOC_JA
			/// <summary>
			/// Rigidbody2D型。
			/// </summary>
#else
			/// <summary>
			/// Rigidbody2D type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Rigidbody2D), "UnityObject/Rigidbody2D")]
			Rigidbody2D,

#if ARBOR_DOC_JA
			/// <summary>
			/// Component型。
			/// </summary>
#else
			/// <summary>
			/// Component type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(Component), "UnityObject/Component", useReferenceType = true)]
			Component,

#if ARBOR_DOC_JA
			/// <summary>
			/// Long型。
			/// </summary>
#else
			/// <summary>
			/// Long type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(long), "Primitive/Long")]
			Long,

#if ARBOR_DOC_JA
			/// <summary>
			/// Object型(Asset)。
			/// </summary>
#else
			/// <summary>
			/// Object type(Asset).
			/// </summary>
#endif
			[ParameterTypeField(typeof(Object), "UnityObject/AssetObject", useReferenceType = true)]
			AssetObject,

#if ARBOR_DOC_JA
			/// <summary>
			/// Variable型。
			/// </summary>
#else
			/// <summary>
			/// Variable type.
			/// </summary>
#endif
			[ParameterTypeField(null, "Variable", useReferenceType = true)]
			Variable = 3000,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;int&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;int&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<int>), "PrimitiveList/IntList")]
			IntList = 4000,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;long&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;long&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<long>), "PrimitiveList/LongList")]
			LongList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;float&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;float&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<float>), "PrimitiveList/FloatList")]
			FloatList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;bool&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;bool&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<bool>), "PrimitiveList/BoolList")]
			BoolList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;string&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;string&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<string>), "PrimitiveList/StringList")]
			StringList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Enum&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Enum&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<System.Enum>), "PrimitiveList/EnumList", useReferenceType = true, toList = true)]
			EnumList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Vector2&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Vector2&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Vector2>), "UnityStructList/Vector2List")]
			Vector2List = 5000,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Vector3&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Vector3&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Vector3>), "UnityStructList/Vector3List")]
			Vector3List,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Quaternion&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Quaternion&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Quaternion>), "UnityStructList/QuaternionList")]
			QuaternionList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Rect&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Rect&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Rect>), "UnityStructList/RectList")]
			RectList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Bounds&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Bounds&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Bounds>), "UnityStructList/BoundsList")]
			BoundsList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Color&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Color&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Color>), "UnityStructList/ColorList")]
			ColorList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Vector4&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Vector4&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Vector4>), "UnityStructList/Vector4List")]
			Vector4List,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Vector2Int&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Vector2Int&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Vector2Int>), "UnityStructList/Vector2IntList")]
			Vector2IntList = 5100,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Vector3Int&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Vector3Int&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Vector3Int>), "UnityStructList/Vector3IntList")]
			Vector3IntList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;RectInt&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;RectInt&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<RectInt>), "UnityStructList/RectIntList")]
			RectIntList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;BoundsInt&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;BoundsInt&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<BoundsInt>), "UnityStructList/BoundsIntList")]
			BoundsIntList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;GameObject&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;GameObject&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<GameObject>), "UnityObjectList/GameObjectList")]
			GameObjectList = 6000,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Component&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Component&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Component>), "UnityObjectList/ComponentList", useReferenceType = true, toList = true)]
			ComponentList,

#if ARBOR_DOC_JA
			/// <summary>
			/// List&lt;Object(Asset)&gt;型。
			/// </summary>
#else
			/// <summary>
			/// List&lt;Object(Asset)&gt; type.
			/// </summary>
#endif
			[ParameterTypeField(typeof(IList<Object>), "UnityObjectList/AssetObjectList", useReferenceType = true, toList = true)]
			AssetObjectList,

#if ARBOR_DOC_JA
			/// <summary>
			/// VariableList型。
			/// </summary>
#else
			/// <summary>
			/// VariableList type.
			/// </summary>
#endif
			[ParameterTypeField(null, "VariableList", useReferenceType = true, toList = true)]
			VariableList = 7000,
		}
	}
}