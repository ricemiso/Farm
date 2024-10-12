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


   [HideInInspector] public GameObject selectedSoil;

    private Text chopText;


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

        bool interactionInfoActive = false;
        bool handIconActive = false; // 手アイコンの表示状態を追跡するフラグ

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // TODO: 破壊処理
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            Choppablecraft choppableCraft = selectionTransform.GetComponent<Choppablecraft>();

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
            }

            Animal animal = selectionTransform.GetComponent<Animal>();
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            // 拾う処理
            if (interactable && interactable.playerRange)
            {
                onTarget = true;
                selectgameObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
                interactionInfoActive = true;

                if (interactable.CompareTag("Pickable"))
                {
                    handIconActive = true; // 手アイコンを表示するフラグを立てる
                }
            }

            // 動物処理
            if (animal && animal.playerISRange)
            {
                interaction_text.text = animal.GetAnimalName();
                interaction_Info_UI.SetActive(true);
                interactionInfoActive = true;

                if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                {
                    StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaPonDamage()));
                }
            }

        }

        // onTarget が true でない場合は UI をリセット
        if (!onTarget)
        {
            centerDotimage.gameObject.SetActive(true);
            handIconActive = false;
        }

        // フラグに基づいて手アイコンの状態を設定
        handIcon.gameObject.SetActive(handIconActive);
        centerDotimage.gameObject.SetActive(!handIconActive); // handIcon が表示されていない場合は中央ドットを表示

        // interactionInfoActive フラグが false の場合のみ UI を非アクティブ化
        if (!interactionInfoActive)
        {
            interaction_Info_UI.SetActive(false);
        }
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