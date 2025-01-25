using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//�S���ҁ@�z�Y�W��

/// <summary>
/// �Q�[�����̃��j���[���Ǘ�����N���X�B
/// </summary>
public class inGameMenu : MonoBehaviour
{
    /// <summary>
    /// ���C�����j���[�ɖ߂�܂��B
    /// </summary>
    public void BackToMainMenu()
    {
		//��~����Exit����������AMenuManager.cs�ňȉ��̏������s��ꂸ�Ƀ��C�����j���[�ɖ߂邽�߁A�����ŏ��������s����
		Time.timeScale = 1.0f;
		AudioListener.pause = false;
		MenuManager.Instance.isCrystalMove = true;

		MenuManager.Instance.savemenu.SetActive(false);
		MenuManager.Instance.settingmenu.SetActive(false);
		MenuManager.Instance.menu.SetActive(true);

		MenuManager.Instance.UICanvas.SetActive(true);
		MenuManager.Instance.menuCanvas.SetActive(false);

		MenuManager.Instance.isMenuOpen = false;

		SelectionManager.Instance.EnableSelection();
		SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

		SceneManager.LoadScene("MainMenu");
    }
}
