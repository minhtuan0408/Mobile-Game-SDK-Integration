using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour, IEndDragHandler
{
	public Action<int> OnItemSelected;

	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private float snapSpeed = 10f;

	private float[] positions;
	private float targetPosition;
	private bool isSnapping;

	private int itemCount;
	private int currentIndex = -1;
	private void Start()
	{
		itemCount = scrollRect.content.childCount;

		positions = new float[itemCount];

		if (itemCount == 1)
		{
			positions[0] = 0;
		}
		else
		{
			float distance = 1f / (itemCount - 1);

			for (int i = 0; i < itemCount; i++)
			{
				positions[i] = distance * i;
			}
		}

		SnapTo(0);
	}

	private void Update()
	{
		if (!isSnapping)
			return;

		scrollRect.horizontalNormalizedPosition =
			Mathf.Lerp(
				scrollRect.horizontalNormalizedPosition,
				targetPosition,
				snapSpeed * Time.deltaTime);

		if (Mathf.Abs(
			scrollRect.horizontalNormalizedPosition -
			targetPosition) < 0.001f)
		{
			scrollRect.horizontalNormalizedPosition =
				targetPosition;

			isSnapping = false;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		SnapToNearest();
	}

	private void SnapToNearest()
	{
		float currentPos =
			scrollRect.horizontalNormalizedPosition;

		int nearestIndex = 0;
		float nearestDistance = float.MaxValue;

		for (int i = 0; i < positions.Length; i++)
		{
			float distance =
				Mathf.Abs(currentPos - positions[i]);

			if (distance < nearestDistance)
			{
				nearestDistance = distance;
				nearestIndex = i;
			}
		}

		SnapTo(nearestIndex);
	}

	public void SnapTo(int index)
	{
		if (index < 0 || index >= positions.Length)
			return;

		targetPosition = positions[index];
		isSnapping = true;

		if (currentIndex != index)
		{
			currentIndex = index;
			OnItemSelected?.Invoke(index);
		}
	}

	public int CurrentIndex => currentIndex;
}