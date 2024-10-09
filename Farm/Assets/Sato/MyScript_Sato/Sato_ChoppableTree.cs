using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Sato_ChoppableTree : MonoBehaviour
{
	public static Sato_ChoppableTree instance; //‰¼

	public bool playerRange;
	public bool canBeChopped;
	public bool animcooltime;

	public float treeMaxHealth;
	public float treeHealth;

	//public Animator Animator;

	public float caloriesSpendChoppingWood;

	private void Start()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}


		treeHealth = treeMaxHealth;
		caloriesSpendChoppingWood = 20;
		//Animator = transform.parent.transform.parent.GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerRange = false;
		}
	}

	public void GetHit()
	{

		//Animator.SetTrigger("shake");

		treeHealth -= 1;
		Debug.Log("CryHealth:" + treeHealth);

		//PlayerState.Instance.currentCalories -= caloriesSpendChoppingWood;

		if (treeHealth <= 0)
		{
			//SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallSound);

			TreeIsDead();

		}
		
	}

	void TreeIsDead()
	{
		Destroy(gameObject);
		canBeChopped = false;
	}

	private void Update()
	{
		if (canBeChopped)
		{
			GrobalState.Instance.resourceHelth = treeHealth;
			GrobalState.Instance.resourceMaxHelth = treeMaxHealth;

		}
	}
}
