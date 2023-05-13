//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Profiling;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Profiler.BeginSample / EndSampleを管理するDisposable ヘルパークラス。
	/// <para>usingを使用して簡略してProfiler.BeginSample / EndSampleを記述できます。</para>
	/// </summary>
#else
	/// <summary>
	/// Disposable helper class that manages the Profiler.BeginSample / EndSample.
	/// <para>Simple to use the using You can write Profiler.BeginSample / EndSample.</para>
	/// </summary>
#endif
	public struct ProfilerScope : System.IDisposable
	{
		private bool _Disposed;

#if ARBOR_DOC_JA
		/// <summary>
		/// 新しいProfilerScopeを作成し、プロファイラのサンプリングを開始します。
		/// </summary>
		/// <param name="name">サンプリングの名前</param>
#else
		/// <summary>
		/// Create a new ProfilerScope, to start the sampling of the profiler.
		/// </summary>
		/// <param name="name">The name of the sampling</param>
#endif
		public ProfilerScope(string name)
		{
			_Disposed = false;
			Profiler.BeginSample(name);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 廃棄する。
		/// </summary>
#else
		/// <summary>
		/// Dispose.
		/// </summary>
#endif
		public void Dispose()
		{
			if (_Disposed)
			{
				return;
			}

			_Disposed = true;
			Profiler.EndSample();
		}
	}
}
