using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentTile : MonoBehaviour, IPointerClickHandler
{
	public Image Icon;
	public TextMeshProUGUI LevelText;
	public GameObject[] LogoTypeIcons;
	public GameObject[] RarityBG;
	public string InstanceId;
	public EquipmentData EquipmentData;
	public int Level;
	public EquipmentType EquipmentType;
	public EquipmentRarity Rarity;
	public bool isEquip;
	public void Init(string instanceId, EquipmentData data, int level)
	{
		ResetAll();
		InstanceId = instanceId;

		Level = level;
		LevelText.text = $"Lv.{level}";
		EquipmentData = data;

		SetUpLogoTypeIcons(data.EquipmentType);
		SetUpRarityIcons(data.Rarity);
		Icon.sprite = data.Icon;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (isEquip)
		{
			EquipmentManager.Instance.Unequip(InstanceId);
		}
		else
		{
			EquipmentManager.Instance.Equip(InstanceId);
		}
	}

	public void SetUpLogoTypeIcons(EquipmentType type)
	{
		switch (type) 
		{
			case EquipmentType.Weapon:
				LogoTypeIcons[0].SetActive(true);
				break;
			case EquipmentType.Armor:
				LogoTypeIcons[1].SetActive(true);
				break;
			case EquipmentType.Necklace:
				LogoTypeIcons[2].SetActive(true);
				break;
			case EquipmentType.Belt:
				LogoTypeIcons[3].SetActive(true);
				break;
			case EquipmentType.Glove:
				LogoTypeIcons[4].SetActive(true);
				break;
			case EquipmentType.Shoe:
				LogoTypeIcons[5].SetActive(true);
				break;
		}
		EquipmentType = type;
	}

	public void SetUpRarityIcons(EquipmentRarity rarity) 
	{
		switch (rarity)
		{
			case EquipmentRarity.Uncommon:
				RarityBG[0].SetActive(true);
				break;
			case EquipmentRarity.Common:
				RarityBG[1].SetActive(true);
				break;
			case EquipmentRarity.Rare:
				RarityBG[2].SetActive(true);
				break;
			case EquipmentRarity.Epic:
				RarityBG[3].SetActive(true);
				break;
			case EquipmentRarity.Legendary:
				RarityBG[4].SetActive(true);
				break;

		}
		Rarity = rarity; 
	}

	private void ResetAll()
	{
		EquipmentType = EquipmentType.None;
		Rarity = EquipmentRarity.None;

		foreach (var icon in LogoTypeIcons)
			icon.SetActive(false);

		foreach (var bg in RarityBG)
			bg.SetActive(false);
	}
}
