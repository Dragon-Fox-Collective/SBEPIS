using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptchalogueCard : MonoBehaviour
{
	public Transform cap0_0;
	public Transform cap0_1;
	public Transform cap0_2;
	public Transform cap0_3;
	public Transform cap0_4;
	public Transform cap0_5;

	public Transform cap1_0;
	public Transform cap1_1;
	public Transform cap1_2;
	public Transform cap1_3;
	public Transform cap1_4;
	public Transform cap1_5;

	public Transform cap2_0;
	public Transform cap2_1;
	public Transform cap2_2;
	public Transform cap2_3;
	public Transform cap2_4;
	public Transform cap2_5;

	public Transform cap3_0;
	public Transform cap3_1;
	public Transform cap3_2;
	public Transform cap3_3;
	public Transform cap3_4;
	public Transform cap3_5;

	public Transform cap4_0;
	public Transform cap4_1;
	public Transform cap4_2;
	public Transform cap4_3;
	public Transform cap4_4;
	public Transform cap4_5;

	public Transform cap5_0;
	public Transform cap5_1;
	public Transform cap5_2;
	public Transform cap5_3;
	public Transform cap5_4;
	public Transform cap5_5;

	public Transform cap6_0;
	public Transform cap6_1;
	public Transform cap6_2;
	public Transform cap6_3;
	public Transform cap6_4;
	public Transform cap6_5;

	public Transform cap7_0;
	public Transform cap7_1;
	public Transform cap7_2;
	public Transform cap7_3;
	public Transform cap7_4;
	public Transform cap7_5;

	private Transform[] holeCaps = new Transform[48];

	private Item _item;
	public Item item
	{
		get => _item;
		set
		{
			if (item && value)
				item = null;

			if (value)
			{
				_item = value;
				item.transform.SetParent(transform);
				item.gameObject.SetActive(false);
				itemHash = item.itemType.captchaHash;
			}
			else if (item)
			{
				item.transform.SetParent(null);
				item.transform.position = transform.position + Vector3.up;
				item.GetComponent<Rigidbody>().velocity = Vector3.up * 6 + GetComponent<Rigidbody>().velocity;
				item.gameObject.SetActive(true);
				_item = null;
				itemHash = 0;
			}
		}
	}

	private long _itemHash;
	public long itemHash
	{
		get => _itemHash;
		set
		{
			_itemHash = value;
			for (int i = 0; i < 48; i++)
				holeCaps[i].gameObject.SetActive((itemHash & (1L << i)) == 0);
		}
	}

	private void Awake()
	{
		holeCaps[0] = cap0_0;
		holeCaps[1] = cap0_1;
		holeCaps[2] = cap0_2;
		holeCaps[3] = cap0_3;
		holeCaps[4] = cap0_4;
		holeCaps[5] = cap0_5;

		holeCaps[6] = cap1_0;
		holeCaps[7] = cap1_1;
		holeCaps[8] = cap1_2;
		holeCaps[9] = cap1_3;
		holeCaps[10] = cap1_4;
		holeCaps[11] = cap1_5;

		holeCaps[12] = cap2_0;
		holeCaps[13] = cap2_1;
		holeCaps[14] = cap2_2;
		holeCaps[15] = cap2_3;
		holeCaps[16] = cap2_4;
		holeCaps[17] = cap2_5;

		holeCaps[18] = cap3_0;
		holeCaps[19] = cap3_1;
		holeCaps[20] = cap3_2;
		holeCaps[21] = cap3_3;
		holeCaps[22] = cap3_4;
		holeCaps[23] = cap3_5;

		holeCaps[24] = cap4_0;
		holeCaps[25] = cap4_1;
		holeCaps[26] = cap4_2;
		holeCaps[27] = cap4_3;
		holeCaps[28] = cap4_4;
		holeCaps[29] = cap4_5;

		holeCaps[30] = cap5_0;
		holeCaps[31] = cap5_1;
		holeCaps[32] = cap5_2;
		holeCaps[33] = cap5_3;
		holeCaps[34] = cap5_4;
		holeCaps[35] = cap5_5;

		holeCaps[36] = cap6_0;
		holeCaps[37] = cap6_1;
		holeCaps[38] = cap6_2;
		holeCaps[39] = cap6_3;
		holeCaps[40] = cap6_4;
		holeCaps[41] = cap6_5;

		holeCaps[42] = cap7_0;
		holeCaps[43] = cap7_1;
		holeCaps[44] = cap7_2;
		holeCaps[45] = cap7_3;
		holeCaps[46] = cap7_4;
		holeCaps[47] = cap7_5;
	}

	private void Start()
	{

	}

	private void Update()
	{

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!gameObject.activeInHierarchy)
			return;

		Item collisionItem = collision.gameObject.GetComponent<Item>();
		if (itemHash == 0 && !item && collisionItem)
			item = collisionItem;
	}

	private void OnMouseDrag()
	{
		if (Input.GetMouseButtonDown(1) && item)
			item = null;
	}
}
