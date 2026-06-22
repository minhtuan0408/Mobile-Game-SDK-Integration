using UnityEngine;

public class LevelTile : MonoBehaviour
{
	public int Level;
	public string Name;
	public string Description;
	public bool IsLock;
	public GameObject LockImage;
	[SerializeField] private float selectedScale = 1f;
	[SerializeField] private float normalScale = 0.8f;
	[SerializeField] private float scaleSpeed = 10f;

	private Vector3 targetScale;

	private void Awake()
	{
		targetScale = Vector3.one * normalScale;
	}

	private void Update()
	{
		transform.localScale = Vector3.Lerp(
			transform.localScale,
			targetScale,
			scaleSpeed * Time.deltaTime);
	}

	public void SetSelected(bool selected)
	{
		targetScale = Vector3.one *
			(selected ? selectedScale : normalScale);
	}

	public void SetLocked(bool locked)
	{
		IsLock = locked;

		if (LockImage != null)
			LockImage.SetActive(locked);
	}
}