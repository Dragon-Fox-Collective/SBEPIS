//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
	public abstract class InputBehaviourBase : StateBehaviour
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新メソッドの種類
		/// </summary>
		/// <remarks>
		/// Updateを選択: 遷移とState更新のタイミングがずれる点に注意。<br/>
		/// StateUpdateを選択: UpdateSettingsの設定によっては毎フレーム呼び出されない点に注意。
		/// </remarks>
#else
		/// <summary>
		/// Type of update method
		/// </summary>
		/// <remarks>
		/// Select Update: Note that the timing of transition and State update is off. <br/>
		/// Select State Update: Note that it may not be called every frame depending on the Update Settings settings.
		/// </remarks>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(UpdateMethodType))]
		private FlexibleUpdateMethodType _UpdateMethod = new FlexibleUpdateMethodType(UpdateMethodType.Update);

		#endregion

		protected virtual void Reset()
		{
			_UpdateMethod = new FlexibleUpdateMethodType(UpdateMethodType.StateUpdate);
		}

		//protected override void OnCreated()
		//{
		//	base.OnCreated();

		//	_UpdateMethodType = new FlexibleUpdateMethodType(UpdateMethodType.StateUpdate);

		//	RebuildDataSlotFields();
		//}

		protected abstract void OnUpdate();

		void DoUpdate(UpdateMethodType updateMethodType)
		{
			if (_UpdateMethod.value != updateMethodType)
			{
				return;
			}

			OnUpdate();
		}

		[Internal.ExcludeTest]
		protected virtual void Update()
		{
			using (CalculateScope.OpenScope())
			{
				DoUpdate(UpdateMethodType.Update);
			}
		}

		public override void OnStateUpdate()
		{
			DoUpdate(UpdateMethodType.StateUpdate);
		}
	}
}