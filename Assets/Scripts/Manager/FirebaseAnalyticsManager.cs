using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class FirebaseAnalyticsManager : MonoBehaviour
{
	public static FirebaseAnalyticsManager Instance;
	private bool isInit = false;
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private async void Start()
	{
		await UnityServices.InitializeAsync();
		AnalyticsService.Instance.StartDataCollection();
		isInit = true;
	}

	public void FinishLevel(int currentLevel)
	{
		if (!isInit) 
		{
			return;
		}
		CustomEvent myEvent = new CustomEvent("finish_level")
		{
			{"level_index", currentLevel},
		};

		AnalyticsService.Instance.RecordEvent(myEvent);
		AnalyticsService.Instance.Flush();
		Debug.Log("FinishLevel Event");
	}
	public void ExitLevel()
	{
		if (!isInit)
			return;

		AnalyticsService.Instance.RecordEvent("exit_level");
		AnalyticsService.Instance.Flush();

		Debug.Log("exit_level");
	}
}
