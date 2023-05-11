//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public sealed class WaypointProcessor
	{
		private Waypoint _Waypoint;

		private int _DestPoint = 0;
		private bool _Reverse = false;

		public bool isValid
		{
			get
			{
				return (_Waypoint != null && _Waypoint.pointCount != 0);
			}
		}

		public Transform nextPoint
		{
			get;
			private set;
		}

		public bool isDone
		{
			get;
			private set;
		}

		public WaypointProcessor()
		{
		}

		public void Initialize(Waypoint waypoint, bool clearDestPoint)
		{
			if (clearDestPoint || _Waypoint != waypoint)
			{
				_DestPoint = 0;
				_Reverse = false;
			}
			_Waypoint = waypoint;
			isDone = false;

			nextPoint = isValid ? _Waypoint.GetPoint(_DestPoint) : null;
		}

		public void Next(MoveWaypointType moveType)
		{
			if (_Waypoint == null)
			{
				return;
			}

			if (!_Reverse)
			{
				_DestPoint++;
				if (_DestPoint >= _Waypoint.pointCount)
				{
					switch (moveType)
					{
						case MoveWaypointType.Once:
							isDone = true;
							_DestPoint = 0;
							break;
						case MoveWaypointType.Cycle:
							_DestPoint = 0;
							break;
						case MoveWaypointType.PingPong:
							_DestPoint = _Waypoint.pointCount - 2;
							_Reverse = !_Reverse;
							break;
					}
				}
			}
			else
			{
				_DestPoint--;
				if (_DestPoint < 0)
				{
					switch (moveType)
					{
						case MoveWaypointType.Once:
							isDone = true;
							_DestPoint = 0;
							break;
						case MoveWaypointType.Cycle:
							_DestPoint = _Waypoint.pointCount - 1;
							break;
						case MoveWaypointType.PingPong:
							_DestPoint = 1;
							_Reverse = !_Reverse;
							break;
					}
				}
			}

			nextPoint = _Waypoint.GetPoint(_DestPoint);
		}
	}
}