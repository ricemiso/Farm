using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager1 : MonoBehaviour
{

    public static SelectionManager1 Instance { get; set; }

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

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();


            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            Choppablecraft choppableCraft = selectionTransform.GetComponent<Choppablecraft>();

            //�؂�|��
            if (choppableTree && choppableTree.playerRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopText.text = "��";
                chopHolder.gameObject.SetActive(true);

            }else if(choppableCraft && choppableCraft.playerRange)
            {
                choppableCraft.canBeChopped = true;
                selectedCraft = choppableCraft.gameObject;
                chopText.text = choppableCraft.CraftItemName();
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if(selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
                else if(selectedCraft != null)
                {
                    selectedCraft.gameObject.GetComponent<Choppablecraft>().canBeChopped = false;
                    selectedCraft = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }



            //�E��
            if (interactable && interactable.playerRange)
            {
                onTarget = true;
                selectgameObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName(selectgameObject);
                interaction_Info_UI.SetActive(true);

                if (interactable.CompareTag("Pickable"))
                {
                    centerDotimage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    HandIsVisible = true;
                }
                else
                {
                    centerDotimage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    HandIsVisible = false;
                }

            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                centerDotimage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);

                HandIsVisible = false;
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            centerDotimage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);

            HandIsVisible = false;
        }
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