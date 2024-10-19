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

    public GameObject chopHolder;

   [HideInInspector] public GameObject selectedTree;
   [HideInInspector] public GameObject selectedCraft;
   [HideInInspector] public GameObject selectedStone;


    [HideInInspector] public GameObject selectedSoil;

    private Text chopText;
    private bool isdamageDelay = false;
    private bool islootDelay = false;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        chopText = chopHolder.GetComponentInChildren<Text>(); ;
    }

    private void Awake()
    {
        if(Instance !=null && Instance != this)
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;


            //TODO : îjâÛÇÕÇ±Ç±Ç…í«â¡ÇµÇƒÇ¢Ç≠
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            Choppablecraft choppableCraft = selectionTransform.GetComponent<Choppablecraft>();
            ChoppableStone choppableStone = selectionTransform.GetComponent<ChoppableStone>();

            //TODO:êÿÇËì|Ç∑èàóù
            if (choppableTree && choppableTree.playerRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopText.text = "ñÿ";
                chopHolder.gameObject.SetActive(true);

            }
            else if (choppableCraft && choppableCraft.playerRange)
            {
                choppableCraft.canBeChopped = true;
                selectedCraft = choppableCraft.gameObject;
                chopText.text = choppableCraft.CraftItemName();
                chopHolder.gameObject.SetActive(true);

            }else if(choppableStone && choppableStone.playerRange)
            {
                choppableStone.canBeChopped = true;
                selectedStone = choppableStone.gameObject;
                chopText.text = "êŒ";
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
            }

            
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            //TODO:èEÇ§èàóù
            if (interactable && interactable.playerRange)
            {
                onTarget = true;
                selectgameObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                centerDotimage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);


                HandIsVisible = true;
            }


            Soil soil = selectionTransform.GetComponent<Soil>();
            if (soil && soil.playerInRange)
            {
                if (soil.isEmpty&&EquipSystem.Instance.IsPlayerHooldingSeed())
                {
                    string seedName = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>().thisName;
                    string onlyPlantName = seedName.Split(new string[] { "ÇÃéÌ" }, StringSplitOptions.None)[0];

                    //TODO:ì˙ñ{åÍÇ…èCê≥Ç∑ÇÈSwitchï∂ÇèëÇ≠

                    interaction_text.text = onlyPlantName + "ÇêAÇ¶ÇÈ";
                    interaction_Info_UI.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        soil.PlantSeed();
                        Destroy(EquipSystem.Instance.selectedItem);
                        Destroy(EquipSystem.Instance.selecteditemModel);
                    }
                }
                else if (soil.isEmpty)
                {
                    interaction_text.text = "ìyèÎ";
                    interaction_Info_UI.SetActive(true);
                }
                else
                {
                    if (EquipSystem.Instance.IsPlayerHooldingWateringCan())
                    {
                        if (soil.currentplant.isWatered)
                        {
                            interaction_text.text = soil.plantName;
                            interaction_Info_UI.SetActive(true);
                        }
                        else
                        {
                            interaction_text.text = "êÖÇÇ†Ç∞ÇƒÇ≠ÇæÇ≥Ç¢";
                            interaction_Info_UI.SetActive(true);

                            if (Input.GetMouseButtonDown(0))
                            {
                                //TODO:ïœçXÇ∑ÇÈÅ@(ÉIÅ[ÉfÉBÉIÉNÉäÉbÉvÇégópÇ∑ÇÈèÍçáÇÕè„)
                                //SoundManager.Instance.wateringCannel.PlayoneShot(SoundManager.Instance.wateringChannel);
                                SoundManager.Instance.PlaySound(SoundManager.Instance.PutSeSound);

                                soil.currentplant.isWatered = true;
                                soil.MakeSoilWatered();
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


            Animal animal = selectionTransform.GetComponent<Animal>();

            if (animal && animal.playerISRange)
            {

                if (animal.isDead)
                {
                    
                    interaction_text.text = "íDÇ§";
                    interaction_Info_UI.SetActive(true);

                    centerDotimage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    HandIsVisible = true;


                  
                    if (Input.GetMouseButtonDown(0)&& !islootDelay)
                    {
                        StartCoroutine(DelayedLoot(animal));
                    }

                }
                else
                {
                    interaction_text.text = animal.GetAnimalName();
                    interaction_Info_UI.SetActive(true);

                    centerDotimage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    HandIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                    {

                        //Ç»Ç∫Ç©Ç§Ç‹Ç≠Ç¢Ç©Ç»Ç¢
                        //Debug.Log("Calling IsThereSwingLock()");
                        //if (EquipSystem.Instance.IsThereSwingLock() == false)
                        //{

                        //    StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaPonDamage()));
                        //}
                        if (isdamageDelay)
                        {
                            return;
                        }
                        else
                        {
                            StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaPonDamage()));
                            StartCoroutine(DelayedAttribute());
                        }
                       
                    }
                }

            }



           


            if (!interactable && !animal)
            {
                onTarget = false;
                HandIsVisible = false;

                centerDotimage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }


            if (!interactable && !animal && !choppableTree && !choppableCraft && !choppableStone && !soil) 
            {
                interaction_text.text = "";
                handIcon.gameObject.SetActive(false);
            }

        }

    }

    IEnumerator DelayedAttribute()
    {
        isdamageDelay = true;
        yield return new WaitForSeconds(1f);
        isdamageDelay = false;
    }

    private IEnumerator DelayedLoot(Animal animal)
    {
        islootDelay = true;
        yield return new WaitForSeconds(1f);
        Lootable lootable = animal.GetComponent<Lootable>();
        Loot(lootable); // íxâÑå„Ç…LootÇåƒÇ—èoÇ∑
        islootDelay = false;
        Debug.Log("Looted!");
    }

    private void Loot(Lootable lootable)
    {
       if(lootable.wasLootCalulated == false)
        {
            List<LootRecieved> lootRecieveds = new List<LootRecieved>();

            foreach(LootPossibility loot in lootable.possibilities)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1);
                if(lootAmount > 0)
                {
                    LootRecieved It = new LootRecieved();
                    It.item = loot.item;
                    It.amount = lootAmount;

                    lootRecieveds.Add(It);
                }
            }


            lootable.finalLoot = lootRecieveds;
            lootable.wasLootCalulated = true;

        }



        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach(LootRecieved lootRecieved in lootable.finalLoot)
        {
            for(int i = 0; i < lootRecieved.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name + "_model"),
                    new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.2f, lootSpawnPosition.z),
                    Quaternion.Euler(0, 0, 0));

                Debug.Log(lootRecieved.item.name);

            }
        }


        if (lootable.GetComponent<Animal>())
        {
            lootable.GetComponent<Animal>().bloodPaddle.transform.SetParent(lootable.transform.parent);
        }


        Destroy(lootable.gameObject);

    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
       

        yield return new WaitForSeconds(delay);

        animal.TakeDamage(damage);
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