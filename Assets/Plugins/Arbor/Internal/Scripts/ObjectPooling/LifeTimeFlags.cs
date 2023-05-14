//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor.ObjectPooling
{
#if ARBOR_DOC_JA
	/// <summary>
	/// プールされたオブジェクトのライフタイム
	/// </summary>
#else
	/// <summary>
	/// Lifetime of pooled objects
	/// </summary>
#endif
	[System.Flags]
	[Internal.Documentable]
	public enum LifeTimeFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// プールされる直前までオブジェクトが配置されていたシーンがアンロードするまでプールに残る。
		/// </summary>
#else
		/// <summary>
		/// The scene where the object was placed until just before being pooled remains in the pool until it is unloaded.
		/// </summary>
#endif
		SceneUnloaded = 1 << 0,

#if ARBOR_DOC_JA
		/// <summary>
		/// プールされてからの指定時間経過するまでプールに残る。
		/// </summary>
#else
		/// <summary>
		/// It remains in the pool until the specified time has passed since it was pooled.
		/// </summary>
#endif
		TimeElapsed = 1 << 1,
	}
}