using TMPro;
using UnityEngine;
using WrightWay.SBEPIS.Util;
using System.Collections.Generic;

namespace WrightWay.SBEPIS.Devices
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
		private Dowel dowel;
		private float[] initialDepths = new float[8];
		private float[,] initialEdges = new float[8, 2];
		private Vector3 initialIncorrection, correctionTarget;

		private void Start()
		{
			CardOutFirst();
		}

		private void Update()
		{
			if (correctionProgress > 0 && dowel)
				dowel.transform.localPosition = Vector3.Lerp(initialIncorrection, correctionTarget, correctionProgress);

			if (isLathing)
			{
				if (dowel)
				{
					List<(int larger, int smaller)> edges = new List<(int larger, int smaller)>;

					for (int i = 0; i < 8; i++)
					{
						float chiselHeight = CaptchaUtil.GetCaptchaPercent(dowel.captchaHash, i); // 0: Chisel bit is flat against the top of the chisel; 1: bit goes all the way down
						float chiselAbsDepth = latheProgress - 1f + chiselHeight; // -1: Chisel bit is currently one dowel radius above the dowel; 0: bit is currently touching the outside of a fresh dowel; 1: bit is all the way into the dowel
						float carveDepth = Mathf.Max(chiselAbsDepth, initialDepths[i]);

						dowel.SetDepth(i, carveDepth);

						if (i > 0 && chiselHeight > CaptchaUtil.GetCaptchaPercent(dowel.captchaHash, i - 1))
							edges.Add((i, i - 1));
						if (i < 7 && chiselHeight > CaptchaUtil.GetCaptchaPercent(dowel.captchaHash, i + 1))
							edges.Add((i, i + 1));
					}

					foreach ((int larger, int smaller) in edges)
					{
						float largerChiselHeight = CaptchaUtil.GetCaptchaPercent(dowel.captchaHash, larger);
						float largerChiselAbsDepth = latheProgress - 1f + largerChiselHeight;
						float largerCarveDepth = Mathf.Max(largerChiselAbsDepth, initialDepths[larger]);

						float smallerChiselHeight = CaptchaUtil.GetCaptchaPercent(dowel.captchaHash, smaller);
						float smallerChiselAbsDepth = latheProgress - 1f + smallerChiselHeight;
						float smallerCarveDepth = Mathf.Max(smallerChiselAbsDepth, initialDepths[smaller]);

						float chiselHeightDifference = (largerChiselHeight - smallerChiselHeight);

						float edgeProgress = (largerCarveDepth - initialDepths[larger]) / chiselHeightDifference;
						edgeProgress = Mathf.Clamp(edgeProgress, 0, 1);
						dowel.SetEdgeDistance(larger, smaller > larger, edgeProgress);
						dowel.SetEdgeDepth(larger, smaller > larger, Mathf.Lerp(edgeProgress, largerCarveDepth, smallerCarveDepth));
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

		public void PlaceDowel()
		{
			dowel = dowelPlacement.item.GetComponent<Dowel>();
			dowelPlacement.AllowOrphan();
			UpdateDowelPanel();
		}

		public void RemoveDowel()
		{
			dowel = null;
			UpdateDowelPanel();
		}

		public void HitLever1()
		{
			if (isWorking || animator.GetBool("Lever 2 Pulled"))
				return;

			Debug.Log("Lever hit");
			isWorking = true;
			dowelPanel.text = "Please wait";
			animator.SetBool("Lever 1 Pulled", !animator.GetBool("Lever 1 Pulled"));
			if (animator.GetBool("Lever 1 Pulled"))
				if (dowel)
					dowelPlacement.DisallowOrphan();
				else
					dowelPlacement.isAdopting = false;
		}

		public void PostLever1()
		{
			if (dowel)
				dowelPlacement.AllowOrphan();
			else
				dowelPlacement.isAdopting = true;
		}

		public void HitLever2()
		{
			if (isWorking || (dowel && !animator.GetBool("Lever 1 Pulled")))
				return;

			isWorking = true;
			dowelPanel.text = "Please wait";
			animator.SetBool("Lever 2 Pulled", !animator.GetBool("Lever 2 Pulled"));
			if (!animator.GetBool("Lever 1 Pulled"))
				dowelPlacement.isAdopting = false;
		}

		public void PostLever2()
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
				chisel.SetBlendShapeWeight(i, (1f - CaptchaUtil.GetCaptchaPercent(captchaHash, i)) * 100);
			isLathing = true;

			if (dowel)
			{
				long greaterCaptchaHash = 0;
				for (int i = 0; i < 8; i++)
				{
					initialDepths[i] = dowel.GetDepth(i);

					if (i > 0)
						initialEdges[i, 0] = dowel.GetEdgeDistance(i, false);
					else
						initialEdges[i, 0] = 0;
					if (i < 7)
						initialEdges[i, 1] = dowel.GetEdgeDistance(i, true);
					else
						initialEdges[i, 1] = 0;

					greaterCaptchaHash |= (1L << i * 6) * Mathf.Max(CaptchaUtil.GetCaptchaDigit(dowel.captchaHash, i), CaptchaUtil.GetCaptchaDigit(captchaHash, i));
				}
				dowel.captchaHash = greaterCaptchaHash;
			}
		}

		/// <summary>
		/// Starts the first card animation
		/// </summary>
		public void CardGoingInFirst()
		{
			animator.SetInteger("Cards", 1);
		}

		/// <summary>
		/// Enables the second card
		/// </summary>
		public void CardInFirst()
		{
			card1Placement.AllowOrphan();
			card2Placement.collider.enabled = true;
			UpdatePanels();
		}

		/// <summary>
		/// Resets the first card animation
		/// </summary>
		public void CardOutFirst()
		{
			card2Placement.collider.enabled = false;
			animator.SetInteger("Cards", 0);
			UpdatePanels();
		}

		/// <summary>
		/// Starts the second card animation
		/// </summary>
		public void CardGoingInSecond()
		{
			animator.SetInteger("Cards", 2);
		}

		/// <summary>
		/// Does mathy bits ig
		/// </summary>
		public void CardInSecond()
		{
			card1Placement.DisallowOrphan();
			card2Placement.AllowOrphan();
			animator.SetInteger("Cards", 2);
			UpdatePanels();
		}

		/// <summary>
		/// Resets the second card animation
		/// </summary>
		public void CardOutSecond()
		{
			card1Placement.AllowOrphan();
			animator.SetInteger("Cards", 1);
			UpdatePanels();
		}

		public void HandOffTotem()
		{
			if (dowel)
				dowel.transform.SetParent(dowelReciever);
		}

		public void TakeBackTotem()
		{
			if (dowel)
				dowel.transform.SetParent(dowelPlacement.itemParent);
		}

		public void GiveTotem()
		{
			if (dowel)
			{
				dowel.transform.SetParent(dowelSpinner);
				initialIncorrection = dowel.transform.localPosition;
				correctionTarget = Vector3.up * dowel.transform.localPosition.y;
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
			string code1 = card1Placement.item ? CaptchaUtil.UnhashCaptcha(card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash) : "!!!!!!!!";
			string code2 = card2Placement.item ? CaptchaUtil.UnhashCaptcha(card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash) : "!!!!!!!!";
			string codeRes = CaptchaUtil.UnhashCaptcha(captchaHash);
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
			if (!dowel && !lowerPulled && !upperPulled)
				dowelPanel.text = "Insert dowel";
			else if (( dowel && dowel.captchaHash == 0 && !lowerPulled && !upperPulled) ||
					 ( dowel && dowel.captchaHash != 0 &&  lowerPulled && !upperPulled) ||
					 (!dowel &&							   lowerPulled && !upperPulled))
				dowelPanel.text = "Pull lower lever";
			else if (( dowel && dowel.captchaHash == 0 &&  lowerPulled && !upperPulled && captchaHash != 0) ||
					 ( dowel &&							   lowerPulled &&  upperPulled) ||
					 (!dowel &&											   upperPulled))
				dowelPanel.text = "Pull upper lever";
			else if (  dowel && dowel.captchaHash == 0 &&  lowerPulled && !upperPulled && captchaHash == 0)
				dowelPanel.text = "Invalid punch code";
			else if (( dowel && dowel.captchaHash != 0 && !lowerPulled && !upperPulled) ||
					 (!dowel &&							  !lowerPulled && !upperPulled))
				dowelPanel.text = "Remove dowel";
			else
				dowelPanel.text = "Error";
		}
	}
}