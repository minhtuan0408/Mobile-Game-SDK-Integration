using System;
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

}
