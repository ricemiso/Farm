using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

	public static SelectionManager Instance { get; set; }

	public bool onTarget;

	public GameObject selectgameObject;
	public GameObject interaction_Info_UI;
	Text interaction_text;

	public Image centerDotimage;
	public Image handIcon;

	public bool HandIsVisible;
	public bool Watering;
	public bool Chargeing;
	public bool leveling;
	private bool isInteractionOnCooldown = false;

	public GameObject chopHolder;

	[HideInInspector] public GameObject selectedTree;
	[HideInInspector] public GameObject selectedCraft;
	[HideInInspector] public GameObject selectedStone;
	[HideInInspector] public GameObject selectedCrystal;
	[HideInInspector] public GameObject selectedMiniCrystal;
	[HideInInspector] public GameObject selectedAnimal;
	[HideInInspector] public GameObject selectedStorageBox;


	[HideInInspector] public GameObject selectedSoil;

	private Text chopText;
	private bool isdamageDelay = false;
	

	private void Start()
	{
		Watering = false;
		onTarget = false;
		interaction_text = interaction_Info_UI.GetComponent<Text>();
		chopText = chopHolder.GetComponentInChildren<Text>(); ;
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	void Update()
	{
		if (Camera.main == null) return;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			var selectionTransform = hit.transform;

			Debug.Log("Hit Object: " + hit.collider.name);
			//TODO : 破壊はここに追加していく
			ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
			Choppablecraft choppableCraft = selectionTransform.GetComponent<Choppablecraft>();
			ChoppableStone choppableStone = selectionTransform.GetComponent<ChoppableStone>();
			CrystalGrowth crystal = selectionTransform.GetComponent<CrystalGrowth>();
			MiniCrystal minicrystal = selectionTransform.GetComponent<MiniCrystal>();

			//TODO:切り倒す処理
			if (choppableTree && choppableTree.playerRange)
			{
				choppableTree.canBeChopped = true;
				selectedTree = choppableTree.gameObject;
				chopText.text = "木";
				chopHolder.gameObject.SetActive(true);

			}
			else if (choppableCraft && choppableCraft.playerRange)
			{
				choppableCraft.canBeChopped = true;
				selectedCraft = choppableCraft.gameObject;
				chopText.text = choppableCraft.CraftItemName();
				chopHolder.gameObject.SetActive(true);

			}
			else if (choppableStone && choppableStone.playerRange)
			{
				choppableStone.canBeChopped = true;
				selectedStone = choppableStone.gameObject;
				chopText.text = "石";
				chopHolder.gameObject.SetActive(true);

			}
			else if (crystal && crystal.playerRange)
			{
				crystal.canBeWatch = true;
				selectedCrystal = crystal.gameObject;
				chopText.text = "マナクリスタル";
				chopHolder.gameObject.SetActive(true);

				if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsPlayerHooldingMana() && !Chargeing)
				{

					Chargeing = true;

					int stackCount = EquipSystem.Instance.GetEquippedItemStackCountBySlot(EquipSystem.Instance.selectedNumber);
					crystal.GetEnergy(stackCount);

					StartCoroutine(DelayWatering());
				}
			}
			else if (minicrystal && minicrystal.playerRange)
			{
				minicrystal.canBeWatchs = true;
				selectedCrystal = minicrystal.gameObject;
				chopText.text = "ミニクリスタル";
				chopHolder.gameObject.SetActive(true);
			}
			else
			{
				if (selectedTree != null)
				{
					selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
					selectedTree = null;
					chopHolder.gameObject.SetActive(false);
				}
				else if (selectedCraft != null)
				{
					selectedCraft.gameObject.GetComponent<Choppablecraft>().canBeChopped = false;
					selectedCraft = null;
					chopHolder.gameObject.SetActive(false);
				}
				else if (selectedStone != null)
				{
					selectedStone.gameObject.GetComponent<ChoppableStone>().canBeChopped = false;
					selectedStone = null;
					chopHolder.gameObject.SetActive(false);
				}
				else if (selectedCrystal != null)
				{
					//selectedCrystal.gameObject.GetComponent<CrystalGrowth>().canBeCharge = false;
					selectedCrystal = null;
					chopHolder.gameObject.SetActive(false);
				}
				else if (selectedMiniCrystal != null)
				{
					//selectedMiniCrystal.gameObject.GetComponent<MiniCrystal>().canBeCharge = false;
					selectedMiniCrystal = null;
					chopHolder.gameObject.SetActive(false);
				}
			}


			InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
			//TODO:拾う処理
			if (interactable && interactable.playerRange && !isInteractionOnCooldown)
			{
				StartCoroutine(HandleInteraction());
				onTarget = true;
				selectgameObject = interactable.gameObject;

				interaction_text.text = interactable.GetItemName(selectgameObject);
				Debug.Log(selectgameObject.name);
				interaction_Info_UI.SetActive(true);

				centerDotimage.gameObject.SetActive(false);
				handIcon.gameObject.SetActive(true);


				HandIsVisible = true;

			}


			Soil soil = selectionTransform.GetComponent<Soil>();
			if (soil && soil.playerInRange)
			{
				if (soil.isEmpty && EquipSystem.Instance.IsPlayerHooldingSeed())
				{
					string seedName = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>().thisName;
					string onlyPlantName = seedName.Split(new string[] { "の種" }, StringSplitOptions.None)[0];

					//TODO:日本語に修正するSwitch文を書く

					interaction_text.text = onlyPlantName + "を植える";
					interaction_Info_UI.SetActive(true);

					if (Input.GetMouseButtonDown(0))
					{
						soil.PlantSeed();
						GrobalState.Instance.isFarm1 = true;
						ConstructionManager.Instance.HandleItemStack(EquipSystem.Instance.selectedItem);
						//Destroy(EquipSystem.Instance.selecteditemModel);
					}
				}
				else if (soil.isEmpty)
				{
					interaction_text.text = "土壌";
					interaction_Info_UI.SetActive(true);
				}
				else
				{
					if (EquipSystem.Instance.IsPlayerHooldingMana())
					{
						if (soil.currentplant.CheckGrows())
						{
							interaction_text.text = soil.plantName;
							interaction_Info_UI.SetActive(true);
						}
						else
						{
							interaction_text.text = "マナをあげてください";
							interaction_Info_UI.SetActive(true);

							if (Input.GetMouseButtonDown(0) && !Watering)
							{

								Watering = true;
								GrobalState.Instance.isWater = true;

								//TODO:変更する　(オーディオクリップを使用する場合は上)
								//SoundManager.Instance.wateringCannel.PlayoneShot(SoundManager.Instance.wateringChannel);
								SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);

								//soil.currentplant.isWatered = true;
								soil.currentplant.Grow();

								soil.MakeSoilWatered();
								StartCoroutine(DelayWatering());



							}
						}
					}
					else
					{
						interaction_text.text = soil.plantName;
						interaction_Info_UI.SetActive(true);
					}

				}

				selectedSoil = soil.gameObject;

			}
			else
			{
				if (selectedSoil != null)
				{
					selectedSoil = null;
				}
			}


			StrageBox strageBox = selectionTransform.GetComponent<StrageBox>();

			if (strageBox && strageBox.playerInRange && ConstructionManager.Instance.inConstructionMode == false)
			{
				interaction_text.text = "開ける";
				interaction_Info_UI.SetActive(true);

				selectedStorageBox = strageBox.gameObject;

				if (Input.GetMouseButtonDown(0))
				{
					StorageManager.Instance.OpenBox(strageBox);
				}
			}
			else
			{
				if (selectedStorageBox != null)
				{
					selectedStorageBox = null;
				}
			}




			Animal animal = selectionTransform.GetComponent<Animal>();

			if (animal && animal.playerISRange)
			{
				animal.canBeChopped = true;
				animal.canBeChopped = true;
				
				selectedAnimal = animal.gameObject;
				chopText.text = animal.GetAnimalName();
				chopHolder.gameObject.SetActive(true);

				if (animal.CompareTag("Enemy"))
				{
					if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
					{
						if (isdamageDelay)
						{

							return;
						}
						else
						{
							StartCoroutine(DealDamageTo(animal, 1.2f, EquipSystem.Instance.GetWeaPonDamage()));
							StartCoroutine(DelayedAttribute());
						}

					}

				}
				else if (EquipSystem.Instance.IsPlayerHooldingMana())
				{
					if (Input.GetMouseButtonDown(0) && !leveling && animal.level<=3)
					{
						leveling = true;
						int stackCount = EquipSystem.Instance.GetEquippedItemStackCountBySlot(EquipSystem.Instance.selectedNumber);
						animal.LevelUp(stackCount);
						StartCoroutine(DelayWatering());
					}
				}
				else
				{

					//interaction_Info_UI.SetActive(true);

					centerDotimage.gameObject.SetActive(true);
					handIcon.gameObject.SetActive(false);

					HandIsVisible = false;


				}

			}
			else
			{
				if (selectedAnimal != null)
				{
					selectedAnimal.gameObject.GetComponent<Animal>().canBeChopped = false;
					selectedAnimal = null;
					chopHolder.gameObject.SetActive(false);
				}
			}


			if (!interactable && !animal)
			{
				onTarget = false;
				HandIsVisible = false;

				centerDotimage.gameObject.SetActive(true);
				handIcon.gameObject.SetActive(false);
			}


			if (!interactable && !animal && !choppableTree && !choppableCraft && !choppableStone && !soil && !strageBox && !crystal && !minicrystal)
			{
				interaction_text.text = "";
				handIcon.gameObject.SetActive(false);
				chopHolder.gameObject.SetActive(false);
			}

		}

	}


	private IEnumerator HandleInteraction()
	{
		isInteractionOnCooldown = true; // クールダウン開始

		yield return new WaitForSeconds(0.5f); // 0.5秒の遅延

		isInteractionOnCooldown = false; // クールダウン終了
	}

	IEnumerator DelayWatering()
	{
		

		if (Chargeing)
		{
			Destroy(EquipSystem.Instance.selectedItem);
			Destroy(EquipSystem.Instance.selecteditemModel);
		}
		else if (leveling)
        {
			
			ConstructionManager.Instance.HandleItemStack(EquipSystem.Instance.selectedItem,3);
			
		}
		else
		{
			ConstructionManager.Instance.HandleItemStack(EquipSystem.Instance.selectedItem);
		}

		yield return new WaitForSeconds(2.0f);
		// Destroy(EquipSystem.Instance.selecteditemModel);

		Watering = false;
		Chargeing = false;
		leveling = false;
	}

	IEnumerator DelayedAttribute()
	{
		isdamageDelay = true;
		yield return new WaitForSeconds(1f);
		isdamageDelay = false;
	}

	

	
	IEnumerator DealDamageTo(Animal animal, float delay, int damage)
	{
		yield return new WaitForSeconds(delay);

		animal.TakeDamage(damage);
		yield return new WaitForSeconds(0.5f);
		animal.canBeChopped = false;
	}

	public void DisableSelection()
	{
		handIcon.enabled = false;
		centerDotimage.enabled = false;
		interaction_Info_UI.SetActive(false);

		selectgameObject = null;
	}

	public void EnableSelection()
	{
		handIcon.enabled = true;
		centerDotimage.enabled = true;
		interaction_Info_UI.SetActive(true);

	}
}