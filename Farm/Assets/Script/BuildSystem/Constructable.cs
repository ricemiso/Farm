using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 建設可能なオブジェクトを制御するクラス。設置可能かどうかの判定や、視覚的なフィードバックを管理する。
/// プレイヤーがオブジェクトを設置できるか、他のアイテムと重なっていないかを確認し、色を変更して状態を示す。
/// </summary>
public class Constructable : MonoBehaviour
{
    // Validation
    /// <summary>
    /// オブジェクトが地面に接しているかどうかを示すフラグ。
    /// </summary>
    public bool isGrounded;

    /// <summary>
    /// 他のアイテムと重なっているかどうかを示すフラグ。
    /// </summary>
    public bool isOverlappingItems;

    /// <summary>
    /// オブジェクトが設置可能かどうかを示すフラグ。
    /// </summary>
    public bool isValidToBeBuilt;

    /// <summary>
    /// ゴーストメンバーが検出されたかどうかを示すフラグ。
    /// </summary>
    public bool detectedGhostMemeber;

    // Material related
    /// <summary>
    /// レンダラーコンポーネントのリスト。
    /// </summary>
    private List<Renderer> renderers = new List<Renderer>();

    /// <summary>
    /// 完全に透明なマテリアル。
    /// </summary>
    private Material fullTransparentnMat;

    /// <summary>
    /// オブジェクトが無効な状態を示す赤いマテリアル。
    /// </summary>
    public Material redMaterial;

    /// <summary>
    /// オブジェクトが有効な状態を示す緑のマテリアル。
    /// </summary>
    public Material greenMaterial;

    /// <summary>
    /// オブジェクトのデフォルトのマテリアル。
    /// </summary>
    public Material defaultMaterial;

    /// <summary>
    /// ゴーストオブジェクトのリスト。
    /// </summary>
    public List<GameObject> ghostList = new List<GameObject>();

    /// <summary>
    /// 固体コライダー。オブジェクトの物理的な衝突判定に使用される。
    /// </summary>
    public BoxCollider solidCollider;

    /// <summary>
    /// 初期化時に必要なコンポーネントを設定する。子オブジェクトのレンダラーをリストに追加し、初期のマテリアルを設定する。
    /// ゴーストオブジェクトをリストに追加する。
    /// </summary>
    private void Start()
    {
        // 子オブジェクトのレンダラーコンポーネントをリストに追加
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            if (renderer != null) // 子オブジェクトにレンダラーコンポーネントがあるかを確認
            {
                renderers.Add(renderer);
                if (gameObject.GetComponent<Animal>())
                {
                    return;
                }
                renderer.material = defaultMaterial; // 初期のマテリアルを設定
            }
        }

        fullTransparentnMat = ConstructionManager.Instance.ghostFullTransparentMat; // 透明なマテリアルを取得

        // ゴーストオブジェクトをリストに追加
        foreach (Transform child in transform)
        {
            ghostList.Add(child.gameObject);
        }
    }

    /// <summary>
    /// 毎フレーム更新される処理。オブジェクトが地面に接しているか、重なっているアイテムがあるかをチェックし、
    /// 設置可能かどうかを判定する。
    /// </summary>
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

        // レイキャストを使って地面との接触を確認
        var boxHeight = transform.lossyScale.y;
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, boxHeight * 1f, LayerMask.GetMask("Ground", "placedFoundation")))
        {
            isGrounded = true;

            // オブジェクトの回転を地面の法線に合わせて調整
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
            transform.rotation = newRotation;
        }
        else
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// トリガー範囲に入ったときに呼ばれる処理。地面や他のアイテムとの重なりを検出し、状態を更新する。
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Ground") || other.CompareTag("placedFoundation")) && gameObject.CompareTag("activeConstructable"))
        {
            isGrounded = true;

            // 地面に合わせてオブジェクトの回転を調整
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

    /// <summary>
    /// トリガー範囲から出たときに呼ばれる処理。地面や他のアイテムとの重なりを解除し、状態を更新する。
    /// </summary>
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

    /// <summary>
    /// 無効な設置状態を示すために、オブジェクトの色を赤に変更する。
    /// </summary>
    public void SetInvalidColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = redMaterial;
        }
    }

    /// <summary>
    /// 有効な設置状態を示すために、オブジェクトの色を緑に変更する。
    /// </summary>
    public void SetValidColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = greenMaterial;
        }
    }

    /// <summary>
    /// 完全に透明な状態を示すために、オブジェクトの色を透明に変更する。
    /// </summary>
    public void SetfullTransparentnColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = fullTransparentnMat;
        }
    }

    /// <summary>
    /// デフォルトの設置状態を示すために、オブジェクトの色をデフォルトに戻す。
    /// </summary>
    public void SetDefaultColor()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material = defaultMaterial;
        }
    }

    /// <summary>
    /// ゴーストメンバーを親オブジェクトから切り離し、固体コライダーを無効化して配置状態を更新する。
    /// </summary>
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
