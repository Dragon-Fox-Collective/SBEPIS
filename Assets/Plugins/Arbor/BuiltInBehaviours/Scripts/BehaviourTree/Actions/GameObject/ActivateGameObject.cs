//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectのアクティブを切り替える。
	/// </summary>
#else
	/// <summary>
	/// It will switch the active GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/ActivateGameObject")]
	[BuiltInBehaviour]
	public sealed class ActivateGameObject : ActionBehaviour
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブを切り替えるGameObject。
		/// </summary>
#else
		/// <summary>
		/// GameObject to switch the active.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _Target = new FlexibleGameObject(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブ切り替え。
		/// </summary>
#else
		/// <summary>
		/// Active switching.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _Active = new FlexibleBool();

		#endregion // Serialize fields

		protected override void OnExecute()
		{
			GameObject target = _Target.value;
			if (target != null)
			{
				target.SetActive(_Active.value);
			}
			FinishExecute(true);
		}
	}
}