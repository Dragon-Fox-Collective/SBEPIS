using SBEPIS.Capturellection.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class TreeBalanceSettingsPageModule : DequeSettingsPageModule<TreeBalanceSettings>
	{
		[SerializeField] private SwitchCardAttacher balance;
		
		public void ResetBalanceSwitch() => balance.SwitchValue = Settings.Balance;
		public void ChangeBalance(bool balance) => Settings.Balance = balance;
	}
}