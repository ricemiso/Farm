using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager instance { get; set; }

    public bool onTarget;

    public GameObject selectgameObject;
    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotimage;
    public Image handIcon;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if(instance !=null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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

            if (interactable && interactable.playerRange)
            {
                onTarget = true;
                selectgameObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                if (interactable.CompareTag("Pickable"))
                {
                    centerDotimage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }
                else
                {
                    centerDotimage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                }

            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                centerDotimage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            centerDotimage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
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