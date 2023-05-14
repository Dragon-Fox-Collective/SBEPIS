//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 指定した地点でサウンドを再生します。
	/// </summary>
#else
	/// <summary>
	/// Play the sound at the specified point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Audio/PlaySoundAtPoint")]
	[BuiltInBehaviour]
	public sealed class PlaySoundAtPoint : PlaySoundAtPointBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生する位置。
		/// </summary>
#else
		/// <summary>
		/// Position to play.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Position = new FlexibleVector3();

		#endregion // Serialize fields

		protected override void OnExecute()
		{
			PlayClipAtPoint(_Position.value);
			FinishExecute(true);
		}
	}
}