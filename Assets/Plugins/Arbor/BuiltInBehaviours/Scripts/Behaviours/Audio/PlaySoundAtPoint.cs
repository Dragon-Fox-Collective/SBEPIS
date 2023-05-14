//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
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

		// Use this for enter state
		public override void OnStateBegin()
		{
			PlayClipAtPoint(_Position.value);
		}
	}
}
