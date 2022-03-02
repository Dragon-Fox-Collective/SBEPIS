using TMPro;
using UnityEngine;
using System.Collections.Generic;
using SBEPIS.Interaction;
using SBEPIS.Captchalogue;

namespace SBEPIS.Alchemy
{
	public class TotemLathe : MonoBehaviour
	{
		public PlacementHelper card1Placement, card2Placement, dowelPlacement;
		public Transform dowelReciever;
		public Transform dowelSpinner;
		public SkinnedMeshRenderer chisel;
		public Animator animator;
		public float latheProgress;
		public float correctionProgress;

		public TextMeshProUGUI dowelPanel;
		public TextMeshProUGUI progressPanel;
		public TextMeshProUGUI unusedPanel;
		public TextMeshProUGUI cardPanel;
		public TextMeshProUGUI captchaPanel;

		private bool isWorking;
		private bool isLathing;
		private long captchaHash;
		private Totem totem;
		private float[] initialDepths = new float[8];
		private HashSet<(int larger, int smaller)> largerEdges = new HashSet<(int larger, int smaller)>();
		private Vector3 initialIncorrection, correctionTarget;

		private void Start()
		{
			OnRemoveCard1();
		}

		private void Update()
		{
			if (correctionProgress > 0 && totem)
				totem.transform.localPosition = Vector3.Lerp(initialIncorrection, correctionTarget, correctionProgress);

			if (isLathing)
			{
				if (totem)
				{
					for (int i = 0; i < 8; i++)
					{
						float chiselHeight = CaptureCodeUtils.GetCapturePercent(totem.captchaHash, i); // 0: Chisel bit is flat against the top of the chisel; 1: bit goes all the way down
						float chiselDepth = latheProgress - 1f + chiselHeight; // -1: Chisel bit is currently one dowel radius above the dowel; 0: bit is currently touching the outside of a fresh dowel; 1: bit is all the way into the dowel
						float carveDepth = Mathf.Max(chiselDepth, initialDepths[i]);

						totem.SetDepth(i, carveDepth);
					}

					foreach ((int larger, int smaller) in largerEdges)
					{
						float largerChiselHeight = CaptureCodeUtils.GetCapturePercent(totem.captchaHash, larger);
						float largerChiselDepth = latheProgress - 1f + largerChiselHeight;
						float largerCarveDepth = Mathf.Max(largerChiselDepth, initialDepths[larger]);

						float smallerChiselHeight = CaptureCodeUtils.GetCapturePercent(totem.captchaHash, smaller);
						float smallerChiselDepth = latheProgress - 1f + smallerChiselHeight;
						float smallerCarveDepth = Mathf.Max(smallerChiselDepth, initialDepths[smaller]);

						float chiselHeightDifference = (largerChiselHeight - smallerChiselHeight);

						if (largerChiselHeight - initialDepths[larger] > smallerChiselHeight - initialDepths[smaller])
						{
							float edgeProgress = Mathf.Clamp((largerCarveDepth - initialDepths[larger]) / chiselHeightDifference, 0, 1);
							totem.SetEdgeDistance(larger, smaller > larger, edgeProgress);
							totem.SetEdgeDepth(larger, smaller > larger, Mathf.Lerp(largerCarveDepth, smallerChiselDepth, edgeProgress));
						}
						else
						{
							totem.SetEdgeDistance(larger, smaller > larger, 1);
							totem.SetEdgeDepth(larger, smaller > larger, smallerCarveDepth);
						}
					}
				}
				progressPanel.text = (latheProgress * 100).ToString("000.") + "%";
				if (latheProgress == 1)
					isLathing = false;
			}
		}

		/// <summary>
		/// The mathy bits
		/// </summary>
		public void SetCaptchaHash()
		{
			if (!card1Placement.item)
				captchaHash = (1L << (6 * 8)) - 1;
			else if (!card2Placement.item)
				captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
			else
				captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash & card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
		}

		public void OnPlaceTotem()
		{
			totem = dowelPlacement.item.GetComponent<Totem>();
			dowelPlacement.AllowOrphan();
			UpdateDowelPanel();
		}

		public void OnRemoveTotem()
		{
			totem = null;
			UpdateDowelPanel();
		}

		public void OnPullLever1()
		{
			if (isWorking || animator.GetBool("Lever 2 Pulled"))
				return;

			isWorking = true;
			dowelPanel.text = "Please wait";
			animator.SetBool("Lever 1 Pulled", !animator.GetBool("Lever 1 Pulled"));
			if (animator.GetBool("Lever 1 Pulled"))
				if (totem)
					dowelPlacement.DisallowOrphan();
				else
					dowelPlacement.isAdopting = false;
		}

		public void OnFinishPullingLever1()
		{
			if (totem)
				dowelPlacement.AllowOrphan();
			else
				dowelPlacement.isAdopting = true;
		}

		public void OnPullLever2()
		{
			if (isWorking || (totem && !animator.GetBool("Lever 1 Pulled")))
				return;

			isWorking = true;
			dowelPanel.text = "Please wait";
			animator.SetBool("Lever 2 Pulled", !animator.GetBool("Lever 2 Pulled"));
			if (!animator.GetBool("Lever 1 Pulled"))
				dowelPlacement.isAdopting = false;
		}

		public void OnFinishPullingLever2()
		{
			if (!animator.GetBool("Lever 1 Pulled"))
				dowelPlacement.isAdopting = true;

			if (card2Placement.item)
			{
				card2Placement.AllowOrphan();
			}
			else if (card1Placement.item)
			{
				card1Placement.AllowOrphan();
				card2Placement.isAdopting = true;
			}
			else
			{
				card1Placement.isAdopting = true;
			}
			progressPanel.text = "000%";
			UpdateCardPanel();
		}

		public void ResetLevers()
		{
			isWorking = false;
			UpdateDowelPanel();
		}

		/// <summary>
		/// Called during the animation, sets up all the final mathy bits to start lathing
		/// </summary>
		public void Lathe()
		{
			if (card2Placement.item)
			{
				card2Placement.DisallowOrphan();
			}
			else if (card1Placement.item)
			{
				card1Placement.DisallowOrphan();
				card2Placement.isAdopting = false;
			}
			else
			{
				card1Placement.isAdopting = false;
			}

			SetCaptchaHash();
			for (int i = 0; i < 8; i++)
				chisel.SetBlendShapeWeight(i, (1f - CaptureCodeUtils.GetCapturePercent(captchaHash, i)) * 100);
			isLathing = true;

			if (totem)
			{
				long greaterCaptchaHash = 0;
				for (int i = 0; i < 8; i++)
				{
					initialDepths[i] = totem.GetDepth(i);

					greaterCaptchaHash |= (1L << i * 6) * Mathf.Max(CaptureCodeUtils.GetCaptureDigit(totem.captchaHash, i), CaptureCodeUtils.GetCaptureDigit(captchaHash, i));
				}
				totem.captchaHash = greaterCaptchaHash;

				largerEdges.Clear();
				for (int i = 0; i < 8; i++)
				{
					float chiselHeight = CaptureCodeUtils.GetCapturePercent(totem.captchaHash, i);
					if (i > 0)
					{
						float prevChiselHeight = CaptureCodeUtils.GetCapturePercent(totem.captchaHash, i - 1);
						if (chiselHeight > prevChiselHeight || (chiselHeight == prevChiselHeight && initialDepths[i] <= initialDepths[i - 1]))
							largerEdges.Add((i, i - 1));
					}
					if (i < 7)
					{
						float nextChiselHeight = CaptureCodeUtils.GetCapturePercent(totem.captchaHash, i + 1);
						if (chiselHeight > nextChiselHeight || (chiselHeight == nextChiselHeight && initialDepths[i] < initialDepths[i + 1]))
							largerEdges.Add((i, i + 1));
					}
				}
			}
		}

		/// <summary>
		/// Starts the first card animation
		/// </summary>
		public void OnPlaceCard1()
		{
			animator.SetInteger("Cards", 1);
		}

		/// <summary>
		/// Enables the second card
		/// </summary>
		public void OnFinishPlacingCard1()
		{
			card1Placement.AllowOrphan();
			card2Placement.collider.enabled = true;
			UpdatePanels();
		}

		/// <summary>
		/// Resets the first card animation
		/// </summary>
		public void OnRemoveCard1()
		{
			card2Placement.collider.enabled = false;
			animator.SetInteger("Cards", 0);
			UpdatePanels();
		}

		/// <summary>
		/// Starts the second card animation
		/// </summary>
		public void OnPlaceCard2()
		{
			animator.SetInteger("Cards", 2);
		}

		/// <summary>
		/// Does mathy bits ig
		/// </summary>
		public void OnFinishPlacingCard2()
		{
			card1Placement.DisallowOrphan();
			card2Placement.AllowOrphan();
			animator.SetInteger("Cards", 2);
			UpdatePanels();
		}

		/// <summary>
		/// Resets the second card animation
		/// </summary>
		public void OnRemoveCard2()
		{
			card1Placement.AllowOrphan();
			animator.SetInteger("Cards", 1);
			UpdatePanels();
		}

		public void GiveTotemToHolder()
		{
			if (totem)
				totem.transform.SetParent(dowelReciever);
		}

		public void GiveTotemToPlacement()
		{
			if (totem)
				totem.transform.SetParent(dowelPlacement.itemParent);
		}

		public void GiveTotemToPistons()
		{
			if (totem)
			{
				totem.transform.SetParent(dowelSpinner);
				initialIncorrection = totem.transform.localPosition;
				correctionTarget = Vector3.up * totem.transform.localPosition.y;
			}
		}

		private void UpdatePanels()
		{
			SetCaptchaHash();
			UpdateCaptchaPanel();
			UpdateCardPanel();
			UpdateDowelPanel();
		}

		private void UpdateCaptchaPanel()
		{
			string code1 = card1Placement.item ? CaptureCodeUtils.UnhashCaptureHash(card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash) : "!!!!!!!!";
			string code2 = card2Placement.item ? CaptureCodeUtils.UnhashCaptureHash(card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash) : "!!!!!!!!";
			string codeRes = CaptureCodeUtils.UnhashCaptureHash(captchaHash);
			captchaPanel.text = $"{code1}\n&\n{code2}\n=\n{codeRes}";
		}

		private void UpdateCardPanel()
		{
			switch (animator.GetInteger("Cards"))
			{
				case 0:
					cardPanel.text = "Insert punched card";
					break;
				case 1:
					cardPanel.text = "Lathe dowel\n||\nInsert another card";
					break;
				case 2:
					cardPanel.text = "Lathe dowel";
					break;
			}
		}

		private void UpdateDowelPanel()
		{
			bool lowerPulled = animator.GetBool("Lever 1 Pulled"), upperPulled = animator.GetBool("Lever 2 Pulled");
			if (!totem && !lowerPulled && !upperPulled)
				dowelPanel.text = "Insert dowel";
			else if (( totem && totem.captchaHash == 0 && !lowerPulled && !upperPulled) ||
					 ( totem && totem.captchaHash != 0 &&  lowerPulled && !upperPulled) ||
					 (!totem &&							   lowerPulled && !upperPulled))
				dowelPanel.text = "Pull lower lever";
			else if (( totem && totem.captchaHash == 0 &&  lowerPulled && !upperPulled && captchaHash != 0) ||
					 ( totem &&							   lowerPulled &&  upperPulled) ||
					 (!totem &&											   upperPulled))
				dowelPanel.text = "Pull upper lever";
			else if (  totem && totem.captchaHash == 0 &&  lowerPulled && !upperPulled && captchaHash == 0)
				dowelPanel.text = "Invalid punch code";
			else if (( totem && totem.captchaHash != 0 && !lowerPulled && !upperPulled) ||
					 (!totem &&							  !lowerPulled && !upperPulled))
				dowelPanel.text = "Remove dowel";
			else
				dowelPanel.text = "Error";
		}
	}
}