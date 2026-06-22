using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager Instance;

	public EquipmentData[] equipmentDatas;
	public readonly List<EquipmentInstance> Inventory = new();
	public EquippedData EquippedData;
	public event Action OnInventoryChanged;	
	private void Awake()
	{
		Instance = this;
	}

	public EquipmentData GetEquipmentById(string id)
	{
		foreach (var equipment in equipmentDatas)
		{
			if (equipment.Id == id)
				return equipment;
		}

		return null;
	}

	//private void TestLoad()
	//{
	//	FirebaseDataManager.Instance.LoadAllEquipments(
	//LoginManager.Instance.UserId,
	//equipments =>
	//{
	//	foreach (var item in equipments)
	//	{
	//		string instanceId = item.Key;

	//		EquipmentSaveData saveData = item.Value;

	//		EquipmentData data =
	//			EquipmentManager.Instance.GetEquipmentById(
	//				saveData.equipmentId);

	//		Debug.Log($"Firebase Id: {instanceId}");
	//		Debug.Log($"Equipment Id: {saveData.equipmentId}");
	//		Debug.Log($"Level: {saveData.level}");
	//		Debug.Log($"Name: {data.EquipmentName}");
	//		Debug.Log($"Type: {data.EquipmentType}");
	//		Debug.Log($"Rarity: {data.Rarity}");
	//	}
	//});
	//}

	public EquipmentData GetRandomEquipment()
	{
		if (equipmentDatas == null || equipmentDatas.Length == 0)
			return null;

		int randomIndex = UnityEngine.Random.Range(0, equipmentDatas.Length);	
		return equipmentDatas[randomIndex];
	}

	public void SaveEquipment(EquipmentData equipment)
	{
		FirebaseDataManager.Instance.AddEquipment(
			LoginManager.Instance.UserId,
			equipment.Id,
			1);
	}

	public void LoadInventory()
	{
		string userId = LoginManager.Instance.UserId;

		if (string.IsNullOrEmpty(userId))
			return;

		FirebaseDataManager.Instance.LoadAllEquipments(
			userId,
			equipments =>
			{
				Inventory.Clear();

				foreach (var item in equipments)
				{
					string instanceId = item.Key;

					EquipmentSaveData saveData = item.Value;

					EquipmentData data =
						GetEquipmentById(saveData.equipmentId);

					if (data == null)
						continue;

					Inventory.Add(new EquipmentInstance
					{
						InstanceId = instanceId,
						Data = data,
						Level = saveData.level
					});
				}

				OnInventoryChanged?.Invoke();
			});
	}

	public void LoadEquipped()
	{
		FirebaseDataManager.Instance.LoadEquipped(
			LoginManager.Instance.UserId,
			data =>
			{
				EquippedData = data;
				OnInventoryChanged?.Invoke();
			});
	}

	public void Equip(string instanceId)
	{
		EquipmentInstance item =
			Inventory.Find(x => x.InstanceId == instanceId);

		if (item == null)
			return;

		FirebaseDataManager.Instance.EquipItem(
			LoginManager.Instance.UserId,
			item.Data.EquipmentType,
			instanceId);

		LoadEquipped();
	}
	public void Unequip(string instanceId)
	{
		EquipmentInstance item =
			GetEquipmentInstance(instanceId);

		if (item == null)
			return;

		FirebaseDataManager.Instance.UnequipItem(
			LoginManager.Instance.UserId,
			item.Data.EquipmentType);

		LoadEquipped();
	}
	public EquipmentInstance GetEquipmentInstance(string instanceId)
	{
		return Inventory.Find(x => x.InstanceId == instanceId);
	}
}

public enum EquipmentType
{
    None,
    Weapon,
    Armor,
    Necklace,
    Belt,
    Glove,
    Shoe
}

public enum EquipmentRarity
{
    None,
	Uncommon,
	Common,
	Rare,
	Epic,
	Legendary
}

public class EquipmentInstance
{
	public string InstanceId;
	public EquipmentData Data;
	public int Level;
	public bool IsEquipped;
}