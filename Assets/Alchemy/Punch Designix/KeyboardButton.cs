using SBEPIS.Interaction;
using UnityEngine;
using UnityEditor;

namespace SBEPIS.Alchemy
{
	[RequireComponent(typeof(Animation))]
	[RequireComponent(typeof(Button))]
    public class KeyboardButton : MonoBehaviour
    {
		public PunchDesignix punch;
        public char alphaChar;
        public char numericChar;
		public float pressAmount;
		public float boxPositionFactor;

		private new Animation animation;
        private Button button;

		private void Awake()
		{
			animation = GetComponent<Animation>();
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
			if (key == 0 || key == '-')
				return;

			if (punch.alphaButton.isToggled && char.IsLetter(key))
				key = char.ToUpper(key);

			punch.SendKey(key);

			animation.Play();
		}

		private void Update()
		{
			Vector3 position = transform.localPosition;
			position.y = pressAmount * boxPositionFactor;
			transform.localPosition = position;

			punch.keyboardRenderer.SetBlendShapeWeight(punch.keyboardRenderer.sharedMesh.GetBlendShapeIndex(gameObject.name), pressAmount * 100);
		}
	}
}