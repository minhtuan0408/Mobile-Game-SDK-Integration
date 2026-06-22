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
	public void LoadLevels(string userId,Action<Dictionary<int, bool>> callback)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("level")
				 .GetValueAsync()
				 .ContinueWithOnMainThread(task =>
				 {
					 Dictionary<int, bool> levels = new();

					 if (task.IsCompleted && task.Result.Exists)
					 {
						 foreach (var child in task.Result.Children)
						 {
							 levels.Add(
								 int.Parse(child.Key),
								 Convert.ToBoolean(child.Value));
						 }

						 callback?.Invoke(levels);
					 }
					 else
					 {
						 Dictionary<string, object> defaultLevels = new();

						 for (int i = 1; i <= 6; i++)
						 {
							 defaultLevels[i.ToString()] = (i == 1);

							 levels.Add(i, i == 1);
						 }

						 reference.Child("users")
								  .Child(userId)
								  .Child("level")
								  .SetValueAsync(defaultLevels);
						 callback?.Invoke(levels);
					 }
				 });
	}
	public void UnlockLevel(string userId, int level)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("level")
				 .Child(level.ToString())
				 .SetValueAsync(true);
	}
	public void GetCurrentLevel(string userId,Action<int> callback)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("currentLevel")
				 .GetValueAsync()
				 .ContinueWithOnMainThread(task =>
				 {
					 if (task.IsCompleted && task.Result.Exists)
					 {
						 callback?.Invoke(
							 Convert.ToInt32(task.Result.Value));
					 }
					 else
					 {
						 callback?.Invoke(1);
					 }
				 });
	}

	public void SetCurrentLevel(string userId, int level)
	{
		reference.Child("users")
				 .Child(userId)
				 .Child("currentLevel")
				 .SetValueAsync(level);
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