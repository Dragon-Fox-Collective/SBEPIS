using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }

	public Captcharoid captcharoid;

	private void Awake()
	{
		instance = this;
	}
}
