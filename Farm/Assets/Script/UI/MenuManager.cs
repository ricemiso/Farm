using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject UICanvas;

    public GameObject savemenu;
    public GameObject settingmenu;
    public GameObject menu;

    public bool isMenuOpen;
    public bool isCrystalMove;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
        {
            UICanvas.SetActive(false);
            menuCanvas.SetActive(true);
            savemenu.SetActive(false);

            isMenuOpen = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isCrystalMove = false;
			Time.timeScale = 0f;
			AudioListener.pause = true;

		}
		else if (Input.GetKeyDown(KeyCode.M) && isMenuOpen)
        {
			Time.timeScale = 1.0f;
			AudioListener.pause = false;
			isCrystalMove = true;

			savemenu.SetActive(false);
            settingmenu.SetActive(false);
            menu.SetActive(true);



            UICanvas.SetActive(true);
            menuCanvas.SetActive(false);

            isMenuOpen = false;


            if(!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }
}
