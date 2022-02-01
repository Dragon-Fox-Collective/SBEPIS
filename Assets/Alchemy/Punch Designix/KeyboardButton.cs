using SBEPIS.Interaction;
using UnityEngine;

namespace SBEPIS.Alchemy
{
    [RequireComponent(typeof(Button))]
    public class KeyboardButton : MonoBehaviour
    {
		public PunchDesignix punch;
        public string alphaChar;
        public string numericChar;

        private Button button;

		private void Awake()
		{
            button = GetComponent<Button>();
		}

		private void OnEnable()
		{
			button.onPressed.AddListener(OnPressed);
		}

		private void OnDisable()
		{
			button.onPressed.RemoveListener(OnPressed);
		}

		private void OnPressed(ItemHolder itemHolder)
		{
			punch.PressKey(alphaChar);
		}
	}
}