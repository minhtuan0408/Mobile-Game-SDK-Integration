using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
	public static LevelsManager Instance;

	public GameObject[] Levels;
	public GameObject Level1;
	public GameObject CurrentLevelPrefab { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public void SetCurrentLevel(int level)
	{
		CurrentLevelPrefab = Levels[level - 1];
	}
}