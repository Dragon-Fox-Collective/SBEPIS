//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// シリアライズやデシリアライズ時にコールバックを受信するインターフェース
	/// </summary>
	/// <remarks>NodeBehaviourを継承しているクラスに使用する。</remarks>
#else
	/// <summary>
	/// Interface to receive callbacks upon serialization and deserialization.
	/// </summary>
	/// <remarks>Used for classes that inherit from NodeBehaviour.</remarks>
#endif
	[Internal.DocumentManual("/manual/scripting/behaviourinterface/INodeBehaviourSerializationCallbackReceiver.md")]
	public interface INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// シリアライゼーションコールバック。
		/// </summary>
		/// <remarks>NodeBehaviourのISerializationCallbackReceiver.OnBeforeSerialize()から呼ばれる。</remarks>
#else
		/// <summary>
		/// Serialization Callback.
		/// </summary>
		/// <remarks>It is called from ISerializationCallbackReceiver.OnBeforeSerialize() of NodeBehaviour.</remarks>
#endif
		void OnBeforeSerialize();

#if ARBOR_DOC_JA
		/// <summary>
		/// シリアライゼーションコールバック。
		/// </summary>
		/// <remarks>NodeBehaviourのISerializationCallbackReceiver.OnAfterDeserialize()から呼ばれる。</remarks>
#else
		/// <summary>
		/// Serialization Callback.
		/// </summary>
		/// <remarks>It is called from ISerializationCallbackReceiver.OnAfterDeserialize() of NodeBehaviour.</remarks>
#endif
		void OnAfterDeserialize();
	}
}