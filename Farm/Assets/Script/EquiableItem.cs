using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Animator))]

public class EquiableItem : MonoBehaviour
{

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack1");
        }
    }
}
