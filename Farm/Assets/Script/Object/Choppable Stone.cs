using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChoppableStone : MonoBehaviour
{
    public bool playerRange;
    public bool canBeChopped;
    public bool animcooltime;

    public float stoneMaxHealth;
    public float stoneHealth;

   //public Animator Animator;

    public float caloriesSpendChoppingWood;

    [SerializeField] float dis = 10f;

    private void Start()
    {
        if (stoneHealth == 0)
        {
            stoneHealth = stoneMaxHealth;
        }

        caloriesSpendChoppingWood = 20;
       // Animator = transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = stoneHealth;
            GrobalState.Instance.resourceMaxHelth = stoneMaxHealth;

        }

        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < dis)
        {
            playerRange = true;
        }
        else
        {
            playerRange = false;
        }
    }

    public void GetHit()
    {

        //Animator.SetTrigger("shake");

        stoneHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (stoneHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

            StoneIsDead();

        }

    }

    void StoneIsDead()
    {
        Vector3 treePosition = transform.position;
        GrobalState.Instance.isStoneChopped = true;
        Destroy(transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedStone = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);


        // オブジェクト1
        GameObject brokenTree1 = Instantiate(Resources.Load<GameObject>("Stone_model"),
            treePosition, Quaternion.Euler(0, 0, 0));

        // オブジェクト2 (x 座標を少しずらす)
        GameObject brokenTree2 = Instantiate(Resources.Load<GameObject>("Stone_model"),
            new Vector3(treePosition.x + 1.5f, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));

        // オブジェクト3 (z 座標を少しずらす)
        GameObject brokenTree3 = Instantiate(Resources.Load<GameObject>("Stone_model"),
            new Vector3(treePosition.x, treePosition.y, treePosition.z + 1.5f), Quaternion.Euler(0, 0, 0));


        Destroy(brokenTree1, 60f);
        Destroy(brokenTree2, 60f);
        Destroy(brokenTree3, 60f);

    }

    
}
