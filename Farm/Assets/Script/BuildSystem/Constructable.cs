using System.Collections.Generic;
using UnityEngine;

public class Constructable : MonoBehaviour
{
    // Validation
    public bool isGrounded;
    public bool isOverlappingItems;
    public bool isValidToBeBuilt;
    public bool detectedGhostMemeber;

    // Material related
    private List<Renderer> renderers = new List<Renderer>();
    private Material fullTransparentnMat;
    public Material redMaterial;
    public Material greenMaterial;
    public Material defaultMaterial;

    public List<GameObject> ghostList = new List<GameObject>();

    public BoxCollider solidCollider;

    private void Start()
    {
        // Find all child objects and add only those with a Renderer component to the list
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            if (renderer != null) // Check if the child has a Renderer component
            {
                renderers.Add(renderer);
                if (gameObject.GetComponent<Animal>())
                {
                    return;
                }
                renderer.material = defaultMaterial; // Set default material initially
            }
        }

        fullTransparentnMat = ConstructionManager.Instance.ghostFullTransparentMat;

        // Add all children to the ghost list
        foreach (Transform child in transform)
        {
            ghostList.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (isGrounded && isOverlappingItems == false)
        {
            isValidToBeBuilt = true;
        }
        else
        {
            isValidToBeBuilt = false;
        }

        // Raycast from the box's position towards its center
        var boxHeight = transform.lossyScale.y;
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, boxHeight * 1f, LayerMask.GetMask("Ground", "placedFoundation")))
        {
            isGrounded = true;

            // Align the box's rotation with the ground normal
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
            transform.rotation = newRotation;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Ground") || other.CompareTag("placedFoundation")) && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = true;

            // Align the box's rotation with the ground normal
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = newRotation;
            }
        }

        if (other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("activeConstructable"))
        {
            isOverlappingItems = true;
        }

        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("placedFoundation") && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = false;
        }

        if (other.CompareTag("Stone") || other.CompareTag("Tree") || other.CompareTag("Pickable") && gameObject.CompareTag("activeConstructable"))
        {
            isOverlappingItems = false;
        }

        if (other.gameObject.CompareTag("ghost") && gameObject.CompareTag("activeConstructable"))
        {
            detectedGhostMemeber = false;
        }
    }

    public void SetInvalidColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = redMaterial;
        }
    }

    public void SetValidColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = greenMaterial;
        }
    }

    public void SetfullTransparentnColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = fullTransparentnMat;
        }
    }

    public void SetDefaultColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = defaultMaterial;
        }
    }

    public void ExtractGhostMembers()
    {
        foreach (GameObject item in ghostList)
        {
            item.transform.SetParent(transform.parent, true);
            item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
            item.gameObject.GetComponent<GhostItem>().isPlaced = true;
        }
    }
}
