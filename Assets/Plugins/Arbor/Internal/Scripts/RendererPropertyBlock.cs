//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rendererへ割り当てられているMaterialPropertyBlockのラッパークラス。
	/// </summary>
#else
	/// <summary>
	/// A wrapper class for the MaterialPropertyBlock assigned to the Renderer.
	/// </summary>
#endif
	public sealed class RendererPropertyBlock
	{
		private readonly struct BlockKey : IEquatable<BlockKey>
		{
			private readonly int _InstanceID;
			private readonly int _MaterialIndex;

			public BlockKey(int instanceID, int materialIndex)
			{
				_InstanceID = instanceID;
				_MaterialIndex = materialIndex;
			}

			public override bool Equals(object obj)
			{
				return (obj is BlockKey other) && this.Equals(other);
			}

			public bool Equals(BlockKey other)
			{
				return _InstanceID == other._InstanceID && _MaterialIndex == other._MaterialIndex;
			}

			public override int GetHashCode()
			{
				return (_InstanceID, _MaterialIndex).GetHashCode();
			}

			public static bool operator ==(BlockKey l, BlockKey r)
			{
				return l.Equals(r);
			}

			public static bool operator !=(BlockKey l, BlockKey r)
			{
				return !(l == r);
			}
		}

		private static Dictionary<BlockKey, RendererPropertyBlock> s_Blocks = new Dictionary<BlockKey, RendererPropertyBlock>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnBeforeSceneLoad()
		{
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}

		static void OnSceneUnloaded(Scene scene)
		{
			List<BlockKey> removeBlocks = new List<BlockKey>();

			foreach (var pair in s_Blocks)
			{
				var renderer = pair.Key;
				if (renderer == null)
				{
					removeBlocks.Add(renderer);
				}
			}

			for (int rendererIndex = 0; rendererIndex < removeBlocks.Count; rendererIndex++)
			{
				BlockKey key = removeBlocks[rendererIndex];
				s_Blocks.Remove(key);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rendererへ割り当てられているRendererPropertyBlockを取得する。
		/// </summary>
		/// <param name="renderer">Renderer</param>
		/// <param name="materialIndex">マテリアルのインデックス</param>
		/// <returns>RendererPropertyBlock</returns>
#else
		/// <summary>
		/// Get the RendererPropertyBlock assigned to the Renderer.
		/// </summary>
		/// <param name="renderer">Renderer</param>
		/// <param name="materialIndex">Material index</param>
		/// <returns>RendererPropertyBlock</returns>
#endif
		public static RendererPropertyBlock Get(Renderer renderer, int materialIndex)
		{
			if (renderer == null)
			{
				return null;
			}

			int instanceID = renderer.GetInstanceID();

			BlockKey key = new BlockKey(instanceID, materialIndex);

			RendererPropertyBlock block = null;
			if (!s_Blocks.TryGetValue(key, out block))
			{
				block = new RendererPropertyBlock(renderer, materialIndex);
				s_Blocks.Add(key, block);
			}

			return block;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rendererへ割り当てられているRendererPropertyBlockを取得する。
		/// </summary>
		/// <param name="renderer">Renderer</param>
		/// <returns>RendererPropertyBlock</returns>
#else
		/// <summary>
		/// Get the RendererPropertyBlock assigned to the Renderer.
		/// </summary>
		/// <param name="renderer">Renderer</param>
		/// <returns>RendererPropertyBlock</returns>
#endif
		public static RendererPropertyBlock Get(Renderer renderer)
		{
			return Get(renderer, 0);
		}

		private Renderer _Renderer;
		private int _MaterialIndex;
		private HashSet<int> _HasProperties = new HashSet<int>();
		private MaterialPropertyBlock _Block;

		private RendererPropertyBlock(Renderer renderer, int materialIndex)
		{
			_Renderer = renderer;
			_MaterialIndex = materialIndex;
			_Block = new MaterialPropertyBlock();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RendererからMaterialPropertyBlockを更新する。
		/// </summary>
#else
		/// <summary>
		/// Update MaterialPropertyBlock from Renderer.
		/// </summary>
#endif
		public void Update()
		{
			_Renderer.GetPropertyBlock(_Block, _MaterialIndex);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RendererへMaterialPropertyBlockを適用する。
		/// </summary>
#else
		/// <summary>
		/// Apply MaterialPropertyBlock to Renderer.
		/// </summary>
#endif
		public void Apply()
		{
			_Renderer.SetPropertyBlock(_Block, _MaterialIndex);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Materialのプロパティ値をクリアする。
		/// </summary>
#else
		/// <summary>
		/// Clear material property values.
		/// </summary>
#endif
		public void Clear()
		{
			_Block.Clear();
			_HasProperties.Clear();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// PropertyBlockがプロパティ値を持っているかどうかを返す。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <returns>プロパティ値が設定されている場合にtrueを返す。</returns>
		/// <remarks>MaterialPropertyBlockを直接変更した場合は反映されません。必ずRendererPropertyBlockのSetメソッドを使用して下さい。</remarks>
#else
		/// <summary>
		/// Returns whether the PropertyBlock has a property value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <returns>Returns true if the property value is set.</returns>
		/// <remarks>If you change the MaterialPropertyBlock directly, it will not be reflected. Be sure to use the Set method of RendererPropertyBlock.</remarks>
#endif
		public bool HasProperty(int nameID)
		{
			return _HasProperties.Contains(nameID);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// PropertyBlockがプロパティ値を持っているかどうかを返す。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>プロパティ値が設定されている場合にtrueを返す。</returns>
		/// <remarks>MaterialPropertyBlockを直接変更した場合は反映されません。必ずRendererPropertyBlockのSetメソッドを使用して下さい。</remarks>
#else
		/// <summary>
		/// Returns whether the PropertyBlock has a property value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>Returns true if the property value is set.</returns>
		/// <remarks>If you change the MaterialPropertyBlock directly, it will not be reflected. Be sure to use the Set method of RendererPropertyBlock.</remarks>
#endif
		public bool HasProperty(string name)
		{
			return HasProperty(Shader.PropertyToID(name));
		}

		Material GetMaterial()
		{
			using (Arbor.Pool.ListPool<Material>.Get(out var materials))
			{
				_Renderer.GetSharedMaterials(materials);

				return materials[_MaterialIndex];
			}
		}

		#region Float

#if ARBOR_DOC_JA
		/// <summary>
		/// float値をゲットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <returns>float値</returns>
#else
		/// <summary>
		/// Get the float value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <returns>float value</returns>
#endif
		public float GetFloat(int nameID)
		{
			if (HasProperty(nameID))
			{
				return _Block.GetFloat(nameID);
			}
			else
			{
				return GetMaterial().GetFloat(nameID);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// float値をゲットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>float値</returns>
#else
		/// <summary>
		/// Get the float value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>float value</returns>
#endif
		public float GetFloat(string name)
		{
			return GetFloat(Shader.PropertyToID(name));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// float値をセットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the float value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <param name="value">Value</param>
#endif
		public void SetFloat(int nameID, float value)
		{
			_Block.SetFloat(nameID, value);
			_HasProperties.Add(nameID);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// float値をセットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the float value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Value</param>
#endif
		public void SetFloat(string name, float value)
		{
			SetFloat(Shader.PropertyToID(name), value);
		}

		#endregion // Float

		#region Color

#if ARBOR_DOC_JA
		/// <summary>
		/// Color値をゲットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <returns>Color値</returns>
#else
		/// <summary>
		/// Get the Color value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <returns>Color value</returns>
#endif
		public Color GetColor(int nameID)
		{
			if (HasProperty(nameID))
			{
				return _Block.GetColor(nameID);
			}
			else
			{
				return GetMaterial().GetColor(nameID);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color値をゲットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>Color値</returns>
#else
		/// <summary>
		/// Get the Color value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>Color value</returns>
#endif
		public Color GetColor(string name)
		{
			return GetColor(Shader.PropertyToID(name));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color値をセットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Color value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <param name="value">Value</param>
#endif
		public void SetColor(int nameID, Color value)
		{
			_Block.SetColor(nameID, value);
			_HasProperties.Add(nameID);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color値をセットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Color value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Value</param>
#endif
		public void SetColor(string name, Color value)
		{
			SetColor(Shader.PropertyToID(name), value);
		}

		#endregion // Color

		#region Matrix

#if ARBOR_DOC_JA
		/// <summary>
		/// Matrix4x4値をゲットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <returns>Matrix4x4値</returns>
#else
		/// <summary>
		/// Get the Matrix4x4 value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <returns>Matrix4x4 value</returns>
#endif
		public Matrix4x4 GetMatrix(int nameID)
		{
			if (HasProperty(nameID))
			{
				return _Block.GetMatrix(nameID);
			}
			else
			{
				return GetMaterial().GetMatrix(nameID);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Matrix4x4値をゲットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>Matrix4x4値</returns>
#else
		/// <summary>
		/// Get the Matrix4x4 value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>Matrix4x4 value</returns>
#endif
		public Matrix4x4 GetMatrix(string name)
		{
			return GetMatrix(Shader.PropertyToID(name));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Matrix4x4値をセットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Matrix4x4 value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <param name="value">Value</param>
#endif
		public void SetMatrix(int nameID, Matrix4x4 value)
		{
			_Block.SetMatrix(nameID, value);
			_HasProperties.Add(nameID);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Matrix4x4値をセットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Matrix4x4 value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Value</param>
#endif
		public void SetMatrix(string name, Matrix4x4 value)
		{
			SetMatrix(Shader.PropertyToID(name), value);
		}

		#endregion // Matrix

		#region Texture

#if ARBOR_DOC_JA
		/// <summary>
		/// Texture値をゲットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <returns>Texture値</returns>
#else
		/// <summary>
		/// Get the Texture value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <returns>Texture value</returns>
#endif
		public Texture GetTexture(int nameID)
		{
			if (HasProperty(nameID))
			{
				return _Block.GetTexture(nameID);
			}
			else
			{
				return GetMaterial().GetTexture(nameID);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Texture値をゲットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>Texture値</returns>
#else
		/// <summary>
		/// Get the Texture value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>Texture value</returns>
#endif
		public Texture GetTexture(string name)
		{
			return GetTexture(Shader.PropertyToID(name));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Texture値をセットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Texture value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <param name="value">Value</param>
#endif
		public void SetTexture(int nameID, Texture value)
		{
			_Block.SetTexture(nameID, value);
			_HasProperties.Add(nameID);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Texture値をセットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Texture value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Value</param>
#endif
		public void SetTexture(string name, Texture value)
		{
			SetTexture(Shader.PropertyToID(name), value);
		}

		#endregion // Texture

		#region TextureScale

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureScale値を取得する。
		/// </summary>
		/// <param name="nameID">プロパティ名のID。<br/>TextureScaleが格納されているベクトルプロパティのIDを指定する。</param>
		/// <returns>TextureScale値</returns>
#else
		/// <summary>
		/// Get TextureScale value./
		/// </summary>
		/// <param name="nameID">Property name ID.<br/>Specify the ID of the vector property that stores TextureScale.</param>
		/// <returns>TextureScale value</returns>
#endif
		public Vector2 GetTextureScale(int nameID)
		{
			Vector4 vector = GetVector(nameID);
			return new Vector2(vector.x, vector.y);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureScale値を取得する。
		/// </summary>
		/// <param name="name">プロパティ名。<br/>TextureScaleが格納されているベクトルプロパティの名前を指定する。</param>
		/// <returns>TextureOffset値</returns>
#else
		/// <summary>
		/// Get TextureScale value./
		/// </summary>
		/// <param name="name">Property name.<br/>Specify the name of the vector property that stores TextureScale.</param>
		/// <returns>TextureScale value</returns>
#endif
		public Vector2 GetTextureScale(string name)
		{
			return GetTextureScale(Shader.PropertyToID(name));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureScale値を設定する。
		/// </summary>
		/// <param name="nameID">プロパティ名のID。<br/>TextureScaleが格納されているベクトルプロパティのIDを指定する。</param>
		/// <param name="scale">設定する値</param>
#else
		/// <summary>
		/// Set TextureScale value.
		/// </summary>
		/// <param name="nameID">Property name ID.<br/>Specify the ID of the vector property that stores TextureScale.</param>
		/// <param name="scale">Value to set</param>
#endif
		public void SetTextureScale(int nameID, Vector2 scale)
		{
			Vector4 vector = GetVector(nameID);
			vector.x = scale.x;
			vector.y = scale.y;
			SetVector(nameID, vector);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureScale値を設定する。
		/// </summary>
		/// <param name="name">プロパティ名。<br/>TextureScaleが格納されているベクトルプロパティの名前を指定する。</param>
		/// <param name="scale">設定する値</param>
#else
		/// <summary>
		/// Set TextureScale value.
		/// </summary>
		/// <param name="name">Property name.<br/>Specify the name of the vector property that stores TextureScale.</param>
		/// <param name="scale">Value to set</param>
#endif
		public void SetTextureScale(string name, Vector2 scale)
		{
			SetTextureScale(Shader.PropertyToID(name), scale);
		}

		#endregion // TextureScale

		#region TextureOffset

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureOffset値を取得する。
		/// </summary>
		/// <param name="nameID">プロパティ名のID。<br/>TextureOffsetが格納されているベクトルプロパティのIDを指定する。</param>
		/// <returns>TextureOffset値</returns>
#else
		/// <summary>
		/// Get TextureOffset value./
		/// </summary>
		/// <param name="nameID">Property name ID.<br/>Specify the ID of the vector property that stores TextureOffset.</param>
		/// <returns>TextureOffset value</returns>
#endif
		public Vector2 GetTextureOffset(int nameID)
		{
			Vector4 vector = GetVector(nameID);
			return new Vector2(vector.z, vector.w);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureOffset値を取得する。
		/// </summary>
		/// <param name="name">プロパティ名。<br/>TextureOffsetが格納されているベクトルプロパティの名前を指定する。</param>
		/// <returns>TextureOffset値</returns>
#else
		/// <summary>
		/// Get TextureOffset value./
		/// </summary>
		/// <param name="name">Property name.<br/>Specify the name of the vector property that stores TextureOffset.</param>
		/// <returns>TextureOffset value</returns>
#endif
		public Vector2 GetTextureOffset(string name)
		{
			return GetTextureOffset(Shader.PropertyToID(name));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureOffset値を設定する。
		/// </summary>
		/// <param name="nameID">プロパティ名のID。<br/>TextureOffsetが格納されているベクトルプロパティのIDを指定する。</param>
		/// <param name="offset">設定する値</param>
#else
		/// <summary>
		/// Set TextureOffset value.
		/// </summary>
		/// <param name="nameID">Property name ID.<br/>Specify the ID of the vector property that stores TextureOffset.</param>
		/// <param name="offset">Value to set</param>
#endif
		public void SetTextureOffset(int nameID, Vector2 offset)
		{
			Vector4 vector = GetVector(nameID);
			vector.z = offset.x;
			vector.w = offset.y;
			SetVector(nameID, vector);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TextureOffset値を設定する。
		/// </summary>
		/// <param name="name">プロパティ名。<br/>TextureOffsetが格納されているベクトルプロパティの名前を指定する。</param>
		/// <param name="offset">設定する値</param>
#else
		/// <summary>
		/// Set TextureOffset value.
		/// </summary>
		/// <param name="name">Property name.<br/>Specify the name of the vector property that stores TextureOffset.</param>
		/// <param name="offset">Value to set</param>
#endif
		public void SetTextureOffset(string name, Vector2 offset)
		{
			SetTextureOffset(Shader.PropertyToID(name), offset);
		}

		#endregion // TextureOffset

		#region Vector

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4値をゲットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <returns>Vector4値</returns>
#else
		/// <summary>
		/// Get the Vector4 value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <returns>Vector4 value</returns>
#endif
		public Vector4 GetVector(int nameID)
		{
			if (HasProperty(nameID))
			{
				return _Block.GetVector(nameID);
			}
			else
			{
				return GetMaterial().GetVector(nameID);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4値をゲットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <returns>Vector4値</returns>
#else
		/// <summary>
		/// Get the Vector4 value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <returns>Vector4 value</returns>
#endif
		public Vector4 GetVector(string name)
		{
			return GetVector(Shader.PropertyToID(name));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4値をセットする。
		/// </summary>
		/// <param name="nameID">プロパティ名のID</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Vector4 value.
		/// </summary>
		/// <param name="nameID">Property name ID</param>
		/// <param name="value">Value</param>
#endif
		public void SetVector(int nameID, Vector4 value)
		{
			_Block.SetVector(nameID, value);
			_HasProperties.Add(nameID);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4値をセットする。
		/// </summary>
		/// <param name="name">プロパティ名</param>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the Vector4 value.
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Value</param>
#endif
		public void SetVector(string name, Vector4 value)
		{
			SetVector(Shader.PropertyToID(name), value);
		}

		#endregion // Vector
	}
}