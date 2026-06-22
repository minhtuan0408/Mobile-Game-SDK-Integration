using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectMapPanel : MonoBehaviour
{
	public Text HeaderTitile;
	public Text HeaderTitle;
	public Text Description;

	public Animator Header;
	public Text Title;

	public Button Select;
	public Button Selected;
	public Button WatchAds;
	public ScrollSnap scrollSnap;
	public MainMenu mainMenu;
	private Dictionary<int, bool> unlockedLevels;

	public LevelTile[] tiles;

	private int currentLevel;

	private void Start()
	{
		scrollSnap.OnItemSelected += SelectMap;

		FirebaseDataManager.Instance.LoadLevels(
			LoginManager.Instance.UserId,
			levels =>
			{
				unlockedLevels = levels;

				UpdateLevelUI();
				SelectMap(0);
			});
		FirebaseDataManager.Instance.GetCurrentLevel(
			LoginManager.Instance.UserId,
			level =>
			{
				currentLevel = level;
			});
	}
	private void UpdateLevelUI()
	{
		for (int i = 0; i < tiles.Length; i++)
		{
			bool unlocked =
				unlockedLevels.ContainsKey(i + 1) &&
				unlockedLevels[i + 1];

			tiles[i].SetLocked(!unlocked);
		}
	}
	private void OnDestroy()
	{
		scrollSnap.OnItemSelected -= SelectMap;
	}
	private void SelectMap(int index)
	{
		Header.Play("Header");

		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i].SetSelected(i == index);
		}

		Title.text = tiles[index].Name;
		Description.text = tiles[index].Description;

		if (tiles[index].Level == currentLevel)
		{
			Selected.gameObject.SetActive(true);
			Select.gameObject.SetActive(false);
			WatchAds.gameObject.SetActive(false);
		}
		else if (tiles[index].IsLock)
		{
			Select.gameObject.SetActive(false);
			Selected.gameObject.SetActive(false);
			WatchAds.gameObject.SetActive(true);
		}
		else
		{
			Select.gameObject.SetActive(true);
			Selected.gameObject.SetActive(false);
			WatchAds.gameObject.SetActive(false);
		}
	}

	public void OnClickSelect()
	{
		int index = scrollSnap.CurrentIndex;
		LevelTile tile = tiles[index];
		currentLevel = tile.Level;
		FirebaseDataManager.Instance.SetCurrentLevel(
			LoginManager.Instance.UserId,
			currentLevel);
		mainMenu.UpdateSelectedMap(currentLevel);
		SelectMap(index);
	}
}


//"1.wild Streets";
//"The City is in chaos with the zombie horde\r\nroaming the streets.";
//Title.text = "2.Jungle War";
//Description.text = "The jungl is in chaos with the monsters thats kill peopples.";
//Title.text = "3.wild Road";
//Description.text = "You have to help soliders from attacking Zombies and monsters in Base.";
//Title.text = "4.desert boombs";
//Description.text = "The desert Base its going on to Attack by Enemys.";
//Title.text = "5.Enemy Streets";
//Description.text = "The Street its fulled With Z and M thats kill peopple Your Mission its to protect them.";
//Title.text = "6.container  Soliders";
//Description.text = "Help the Container soliders to finish killing Zombies and FlowwersMonsters.";