//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	[System.Serializable]
	internal sealed class QuaternionListParameter : ListParameterBaseInternal<Quaternion>
	{
		[EulerAngles]
		public List<Quaternion> list = new List<Quaternion>();

		protected override sealed List<Quaternion> listInstance
		{
			get
			{
				return list;
			}
		}
	}
}