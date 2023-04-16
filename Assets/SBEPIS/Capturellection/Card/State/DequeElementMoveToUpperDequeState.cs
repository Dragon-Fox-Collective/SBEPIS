using SBEPIS.Utils;

namespace SBEPIS.Capturellection.CardState
{
	public class DequeElementMoveToUpperDequeState : DequeElementTargettingState
	{
		protected override LerpTarget TargetToTargetTo => State.DequeBox.UpperTarget;
	}
}
