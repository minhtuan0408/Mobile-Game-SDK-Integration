using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Game/Equipment")]
public class EquipmentData : ScriptableObject
{
	public string Id;
	[Header("Info")]
	public string EquipmentName;
	[TextArea]
	public string Description;

	[Header("Type")]
	public EquipmentType EquipmentType;
	public EquipmentRarity Rarity;

	[Header("Visual")]
	public Sprite Icon;

	[Header("Stats")]
	public int BaseAttack;
	public int BaseHP;
	public int BaseDefense;

	[Header("Upgrade")]
	public int MaxLevel = 100;
}