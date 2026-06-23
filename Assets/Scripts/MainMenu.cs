using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject MSettings;
    public GameObject MMessages;

	public Image mapImage;
	public Text mapName;

	public Sprite[] images;
	public SelectMapPanel selectMapPanel;

	private void Start()
	{
		FirebaseDataManager.Instance.GetCurrentLevel(
			LoginManager.Instance.UserId,
			level =>
			{
				LevelsManager.Instance.SetCurrentLevel(level);
				UpdateSelectedMap(level);
			});
	}

	public void UpdateSelectedMap(int level)
	{
		LevelTile tile = selectMapPanel.tiles[level - 1];

		mapName.text = tile.Name;
		mapImage.sprite = images[level - 1];
	}
	public void OpenSetting()
    {
        MSettings.SetActive(true);
    }
    public void OpenMessages()
    {
        MMessages.SetActive(true);
    }
    public void ExitSetting()
    {
        MSettings.SetActive(false);
    }
    public void ExitMessages()
    {
        MMessages.SetActive(false);
    }
}
