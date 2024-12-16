using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakCrystal : MonoBehaviour
{

    

    void Start()
    {
        StartCoroutine(delaygameOver());
        PlayerState.Instance.playerBody.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        
    }

    IEnumerator delaygameOver()
    {
        yield return new WaitForSeconds(10);
        //SceneManager.LoadScene("GameOver");
    }

}
