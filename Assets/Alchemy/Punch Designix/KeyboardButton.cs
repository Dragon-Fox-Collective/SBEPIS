using SBEPIS.Interaction;
using UnityEngine;

namespace SBEPIS.Alchemy
{
    [RequireComponent(typeof(Button))]
    public class KeyboardButton : MonoBehaviour
    {
		public PunchDesignix punch;
        public char alphaChar;
        public char numericChar;

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
			char key = punch.numericButton.isToggled ? numericChar : alphaChar;
			if (key == 0)
				return;

			if (punch.alphaButton.isToggled && char.IsLetter(key))
				key = char.ToUpper(key);

			punch.SendKey(key);
		}
	}
}