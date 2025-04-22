using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmerData", menuName = "ScriptableObjects/ArmerData", order = 1)]
public class ArmerData : ScriptableObject
{
	public List<Armer> armerList = new List<Armer>();


}

[System.Serializable]
public class Armer
{
	public string name;
	public float reductionRate;
	public float weight; //‰¼
}