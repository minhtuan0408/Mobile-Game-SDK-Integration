using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
	public Transform WeaponSlot;
	public Transform AmmorSlot;
	public Transform NecklaceSlot;
	public Transform BeltSlot;
	public Transform GloveSlot;
	public Transform ShoeSlot;

	public Transform MyEquipment;

	[SerializeField]
	private EquipmentTile tilePrefab;
	private void OnEnable()
	{
		EquipmentManager.Instance.OnInventoryChanged += Refresh;

		EquipmentManager.Instance.LoadInventory();
		EquipmentManager.Instance.LoadEquipped();
	}
	private void OnDisable()
	{
		if (EquipmentManager.Instance != null)
		{
			EquipmentManager.Instance.OnInventoryChanged -= Refresh;
		}
	}

	private void Refresh()
	{
		RefreshInventory();
	}

	private void RefreshInventory()
	{
		foreach (Transform child in MyEquipment)
		{
			Destroy(child.gameObject);
		}

		foreach (var item in EquipmentManager.Instance.Inventory)
		{
			Debug.Log(item);
			EquipmentTile tile =
				Instantiate(tilePrefab, MyEquipment);
			Debug.Log(item.Data);
			tile.Init(
				item.InstanceId,
				item.Data,
				item.Level);
		}
	}

	private void RefreshEquipped()
	{
		ClearSlot(WeaponSlot);
		ClearSlot(AmmorSlot);
		ClearSlot(NecklaceSlot);
		ClearSlot(BeltSlot);
		ClearSlot(GloveSlot);
		ClearSlot(ShoeSlot);

		EquippedData equipped =
			EquipmentManager.Instance.EquippedData;

		if (equipped == null)
			return;

		CreateEquippedTile(equipped.weapon, WeaponSlot);
		CreateEquippedTile(equipped.armor, AmmorSlot);
		CreateEquippedTile(equipped.necklace, NecklaceSlot);
		CreateEquippedTile(equipped.belt, BeltSlot);
		CreateEquippedTile(equipped.glove, GloveSlot);
		CreateEquippedTile(equipped.shoe, ShoeSlot);
	}

	private void CreateEquippedTile(
	string instanceId,
	Transform slot)
	{
		if (string.IsNullOrEmpty(instanceId))
			return;

		EquipmentInstance item =
			EquipmentManager.Instance.GetEquipmentInstance(instanceId);

		if (item == null)
			return;

		EquipmentTile tile =
			Instantiate(tilePrefab, slot);

		tile.Init(
			item.InstanceId,
			item.Data,
			item.Level);
	}

	private void ClearSlot(Transform slot)
	{
		foreach (Transform child in slot)
		{
			Destroy(child.gameObject);
		}
	}
}
