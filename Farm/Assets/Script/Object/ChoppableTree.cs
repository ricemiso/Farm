using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ChoppableTree : MonoBehaviour
{


    public bool playerRange;
    public bool canBeChopped;
    public bool animcooltime;

    public float treeMaxHealth;
    public float treeHealth;

    public Animator Animator;

    public float caloriesSpendChoppingWood;

    [SerializeField] float dis = 10f;

    private void Start()
    {
        if (treeHealth == 0)
        {
            treeHealth = treeMaxHealth;
        }

        caloriesSpendChoppingWood = 20;
        Animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        if (canBeChopped)
        {
            GrobalState.Instance.resourceHelth = treeHealth;
            GrobalState.Instance.resourceMaxHelth = treeMaxHealth;

        }
        if (PlayerState.Instance.playerBody == null) return;
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

        Animator.SetTrigger("shake");

        treeHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

        if (treeHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

            TreeIsDead();

        }
        
    }

    void TreeIsDead()
    {
        Vector3 treePosition = transform.position;
        GrobalState.Instance.isTreeChopped = true;
        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            new Vector3(treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler(0, 0, 0));



        brokenTree.transform.SetParent(transform.parent.transform.parent.transform.parent);
        Destroy(brokenTree, 60f);

    }

   
}
