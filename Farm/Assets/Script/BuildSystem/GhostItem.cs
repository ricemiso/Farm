using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostItem : MonoBehaviour
{
    public BoxCollider solidCollider; 

    public Renderer mRenderer;
    private Material semiTransparentMat; 
    private Material fullTransparentnMat;
    private Material selectedMaterial;

    public bool isPlaced;

   
    public bool hasSamePosition = false;



    private void Start()
    {
        mRenderer = GetComponent<Renderer>();
       
        semiTransparentMat = ConstructionManager.Instance.ghostSemiTransparentMat;
        fullTransparentnMat = ConstructionManager.Instance.ghostFullTransparentMat;
        selectedMaterial = ConstructionManager.Instance.ghostSelectedMat;

        mRenderer.material = fullTransparentnMat; 
        
        solidCollider.enabled = false;
    }

    private void Update()
    {
        if (ConstructionManager.Instance.inConstructionMode)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), ConstructionManager.Instance.player.GetComponent<Collider>());
        }


        if (ConstructionManager.Instance.inConstructionMode && isPlaced)
        {
            solidCollider.enabled = true;
        }

        if (!ConstructionManager.Instance.inConstructionMode)
        {
            solidCollider.enabled = false;
        }

       
        if (ConstructionManager.Instance.selectedGhost == this.gameObject)
        {
            mRenderer.material = selectedMaterial;
        }
        else
        {
            mRenderer.material = fullTransparentnMat; 
        }

    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject);


        if (!this.gameObject.CompareTag("wallghost") && other.CompareTag("placedFoundation") && !ConstructionManager.Instance.inConstructionMode)
        {
           
            this.gameObject.SetActive(false);
            Debug.Log("Ghost hit placedFoundation"+gameObject.name);

        }
    }



}