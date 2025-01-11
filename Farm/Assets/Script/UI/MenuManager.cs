using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�S���ҁ@�z�Y�W��

/// <summary>
/// ���j���[���Ǘ�����N���X�B
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// ���j���[�̃C���X�^���X�B
    /// </summary>
    public static MenuManager Instance { get; set; }

    /// <summary>
    /// ���j���[�L�����o�X�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject menuCanvas;

    /// <summary>
    /// UI�L�����o�X�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject UICanvas;

    /// <summary>
    /// �Z�[�u���j���[�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject savemenu;

    /// <summary>
    /// �ݒ胁�j���[�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject settingmenu;

    /// <summary>
    /// ���j���[�̃Q�[���I�u�W�F�N�g�B
    /// </summary>
    public GameObject menu;

    /// <summary>
    /// ���j���[���J���Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool isMenuOpen;

    /// <summary>
    /// �N���X�^���������Ă��邩�ǂ����������t���O�B
    /// </summary>
    public bool isCrystalMove;

    /// <summary>
    /// �V���O���g���p�^�[����K�p���A�C���X�^���X�����������܂��B
    /// </summary>
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

    /// <summary>
    /// �����ݒ���s���܂��B
    /// </summary>
    private void Start()
    {
        isCrystalMove = true;
    }

    /// <summary>
    /// ���j���[��ʂ̕\��
    /// </summary>
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

            if (!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }
}
