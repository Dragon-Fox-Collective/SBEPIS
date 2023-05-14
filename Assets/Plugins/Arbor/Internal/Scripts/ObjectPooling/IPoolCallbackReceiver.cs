//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.ObjectPooling
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ObjectPoolのコールバックを受けるインターフェイス
	/// </summary>
	/// <remarks>プール管理下のゲームオブジェクト（子含む）に追加されているMonoBehaviourにこのインターフェイスを定義することでコールバックを受け取れるようになる。</remarks>
#else
	/// <summary>
	/// Interface receiving ObjectPool callback
	/// </summary>
	/// <remarks>You can receive callbacks by defining this interface for MonoBehaviour added to the game object under pool management (including child).</remarks>
#endif
	public interface IPoolCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再開する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when resuming.
		/// </summary>
#endif
		void OnPoolResume();

#if ARBOR_DOC_JA
		/// <summary>
		/// プールに格納された際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when stored in the pool.
		/// </summary>
#endif
		void OnPoolSleep();
	}
}