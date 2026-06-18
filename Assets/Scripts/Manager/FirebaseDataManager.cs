using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseDataManager : MonoBehaviour
{
	public static FirebaseDataManager Instance;

	private DatabaseReference reference;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			reference = FirebaseDatabase.DefaultInstance.RootReference;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	#region Level
	public void SaveLevel(string userId, int level)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("level")
				 .SetValueAsync(level);
	}
	public void LoadLevel(string userId, Action<int> callback)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("level")
				 .GetValueAsync()
				 .ContinueWithOnMainThread(task =>
				 {
					 if (task.IsCompleted && task.Result.Exists)
					 {
						 int level = Convert.ToInt32(task.Result.Value);
						 callback?.Invoke(level);
					 }
					 else
					 {
						 callback?.Invoke(1);
					 }
				 });
	}
	public void UnlockNextLevel(string userId, int currentLevel)
	{
		reference.Child("users")
			.Child(userId)
			.Child("level")
			.GetValueAsync()
			.ContinueWithOnMainThread(task =>
			{
				if (task.IsCompleted && task.Result.Exists)
				{
					int savedLevel = Convert.ToInt32(task.Result.Value);

					if (savedLevel <= currentLevel)
					{
						reference.Child("users")
								 .Child(userId)
								 .Child("level")
								 .SetValueAsync(currentLevel + 1);

						Debug.Log($"Unlocked Level {currentLevel + 1}");
					}
				}
			});
	}
	#endregion

	#region Gold
	public void SaveGold(string userId, int gold)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("gold")
				 .SetValueAsync(gold);
	}

	public void LoadGold(string userId, Action<int> callback)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("gold")
				 .GetValueAsync()
				 .ContinueWithOnMainThread(task =>
				 {
					 if (task.IsCompleted && task.Result.Exists)
					 {
						 int gold = Convert.ToInt32(task.Result.Value);
						 callback?.Invoke(gold);
					 }
					 else
					 {
						 callback?.Invoke(100);
					 }
				 });
	}
	#endregion

	#region Equipment

	public void AddEquipment(string userId, string equipmentId, int level)
	{
		string instanceId = reference
			.Child("users")
			.Child(userId)
			.Child("equipments")
			.Push()
			.Key;

		EquipmentSaveData data =
			new EquipmentSaveData(equipmentId, level);

		reference.Child("users")
				 .Child(userId)
				 .Child("equipments")
				 .Child(instanceId)
				 .SetRawJsonValueAsync(JsonUtility.ToJson(data));
	}
	public void RemoveEquipment(string userId, string instanceId)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("equipments")
				 .Child(instanceId)
				 .RemoveValueAsync();
	}
	public void LoadAllEquipments(
		string userId,
		Action<Dictionary<string, EquipmentSaveData>> callback)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("equipments")
				 .GetValueAsync()
				 .ContinueWithOnMainThread(task =>
				 {
					 Dictionary<string, EquipmentSaveData> result = new();

					 if (task.IsCompleted && task.Result.Exists)
					 {
						 foreach (var child in task.Result.Children)
						 {
							 string instanceId = child.Key;

							 EquipmentSaveData data =
								 JsonUtility.FromJson<EquipmentSaveData>(
									 child.GetRawJsonValue());

							 result.Add(instanceId, data);
						 }
					 }

					 callback?.Invoke(result);
				 });
	}
	public void EquipItem(string userId,EquipmentType type,string instanceId)
	{
		string slot = type.ToString().ToLower();

		reference.Child("users")
				 .Child(userId)
				 .Child("equipped")
				 .Child(slot)
				 .SetValueAsync(instanceId);
	}
	public void UnequipItem(string userId, EquipmentType type)
	{
		string slot = type.ToString().ToLower();

		reference.Child("users")
				 .Child(userId)
				 .Child("equipped")
				 .Child(slot)
				 .RemoveValueAsync();
	}
	public void LoadEquipped(
	string userId,
	Action<EquippedData> callback)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("equipped")
				 .GetValueAsync()
				 .ContinueWithOnMainThread(task =>
				 {
					 if (task.IsCompleted &&
						 task.Result.Exists)
					 {
						 EquippedData data =
							 JsonUtility.FromJson<EquippedData>(
								 task.Result.GetRawJsonValue());

						 callback?.Invoke(data);
					 }
				 });
	}
	#endregion
}
[System.Serializable]
public class EquipmentSaveData
{
	public string equipmentId;
	public int level;

	public EquipmentSaveData(string equipmentId, int level)
	{
		this.equipmentId = equipmentId;
		this.level = level;
	}
}
[System.Serializable]
public class EquippedData
{
	public string weapon;
	public string armor;
	public string necklace;
	public string belt;
	public string glove;
	public string shoe;
}