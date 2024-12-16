using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakCrystal : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(delaygameOver());
    }

    IEnumerator delaygameOver()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("GameOver");
        Destroy(gameObject);

    }

}
