using SBEPIS.Capturllection;
using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Tests.Scenes
{
	public class CapturellectScene : MonoBehaviour
	{
		public Grabber grabber;
		[FormerlySerializedAs("dequeOwner")]
		public DequeBoxOwner dequeBoxOwner;
		public GameObject obj;
		public Capturellectainer emptyCard;
		public Capturellectainer fullCard;
	}
}