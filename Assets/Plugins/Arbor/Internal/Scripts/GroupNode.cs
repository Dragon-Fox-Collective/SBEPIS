//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// グループノードを表すクラス
	/// </summary>
#else
	/// <summary>
	/// Class that represents the group node
	/// </summary>
#endif
	[System.Serializable]
	public sealed class GroupNode : Node, ISerializeVersionCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 自動整列
		/// </summary>
#else
		/// <summary>
		/// Auto Alignment
		/// </summary>
#endif
		public enum AutoAlignment
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// なし
			/// </summary>
#else
			/// <summary>
			/// None
			/// </summary>
#endif
			None = 0,

#if ARBOR_DOC_JA
			/// <summary>
			/// 垂直方向
			/// </summary>
#else
			/// <summary>
			/// Vertical
			/// </summary>
#endif
			Vertical,

#if ARBOR_DOC_JA
			/// <summary>
			/// 水平方向
			/// </summary>
#else
			/// <summary>
			/// Horizontal
			/// </summary>
#endif
			Horizonal,
		};

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの色
		/// </summary>
#else
		/// <summary>
		/// Node color
		/// </summary>
#endif
		public Color color = Color.white;

#if ARBOR_DOC_JA
		/// <summary>
		/// オートレイアウト
		/// </summary>
#else
		/// <summary>
		/// Auto Layout
		/// </summary>
#endif
		public AutoAlignment autoAlignment = AutoAlignment.None;

		[SerializeField]
		private SerializeVersion _SerializeVersion = new SerializeVersion();

		#region old

		[SerializeField]
		[FormerlySerializedAs("_SerializeVersion")]
		private int _SerializeVersionOld = 0;

		[SerializeField]
		[FormerlySerializedAs("_IsInitialized")]
		private bool _IsInitializedOld = true;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private GroupNode() : base(null, 0)
		{
			// Initialize when calling from script.
			_SerializeVersion.Initialize(this);
		}

		internal GroupNode(NodeGraph nodeGraph, int nodeID) : base(nodeGraph, nodeID)
		{
			name = "New Group";
			// Initialize when calling from script.
			_SerializeVersion.Initialize(this);
		}

		void SerializeVer1()
		{
			color = Color.white;
		}

		#region ISerializeVersionCallbackReceiver

		int ISerializeVersionCallbackReceiver.newestVersion
		{
			get
			{
				return kCurrentSerializeVersion;
			}
		}

		void ISerializeVersionCallbackReceiver.OnInitialize()
		{
			_SerializeVersion.version = kCurrentSerializeVersion;
		}

		void ISerializeVersionCallbackReceiver.OnSerialize(int version)
		{
			switch (version)
			{
				case 0:
					SerializeVer1();
					break;
			}
		}

		void ISerializeVersionCallbackReceiver.OnVersioning()
		{
			if (_IsInitializedOld)
			{
				_SerializeVersion.version = _SerializeVersionOld;
			}
		}

		#endregion // ISerializeVersionCallbackReceiver

		#region ISerializationCallbackReceiver

#if ARBOR_DOC_JA
		/// <summary>
		/// ISerializationCallbackReceiver.OnAfterDeserializeから呼び出される。
		/// </summary>
#else
		/// <summary>
		/// Called from ISerializationCallbackReceiver.OnAfterDeserialize.
		/// </summary>
#endif
		protected override void OnAfterDeserialize()
		{
			_SerializeVersion.AfterDeserialize();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ISerializationCallbackReceiver.OnBeforeSerialize。
		/// </summary>
#else
		/// <summary>
		/// Called from ISerializationCallbackReceiver.OnBeforeSerialize.
		/// </summary>
#endif
		protected override void OnBeforeSerialize()
		{
			_SerializeVersion.BeforeDeserialize();
		}

		#endregion ISerializationCallbackReceiver
	}
}
