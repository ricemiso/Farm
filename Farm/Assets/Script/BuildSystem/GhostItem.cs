using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当者　越浦晃生

/// <summary>
/// 建設モードで配置可能なゴーストアイテムを表すクラス。
/// </summary>
public class GhostItem : MonoBehaviour
{
    /// <summary>
    /// ゴーストアイテムのコライダー。
    /// </summary>
    public BoxCollider solidCollider;

    /// <summary>
    /// ゴーストアイテムのレンダラー。
    /// </summary>
    public Renderer mRenderer;

    /// <summary>
    /// 半透明マテリアル。
    /// </summary>
    private Material semiTransparentMat;

    /// <summary>
    /// 完全透明マテリアル。
    /// </summary>
    private Material fullTransparentnMat;

    /// <summary>
    /// 選択されたマテリアル。
    /// </summary>
    private Material selectedMaterial;

    /// <summary>
    /// ゴーストアイテムが設置されたかどうか。
    /// </summary>
    public bool isPlaced;

    /// <summary>
    /// 同じ位置を持つかどうか。
    /// </summary>
    public bool hasSamePosition = false;

    /// <summary>
    /// ゴーストアイテムを初期化し、全ゴーストリストに追加します。
    /// </summary>
    private void Awake()
    {
        ConstructionManager.Instance.allGhostsInExistence.Add(this.gameObject);
    }

    /// <summary>
    /// ゴーストアイテムのマテリアルと初期状態を設定します。
    /// </summary>
    private void Start()
    {
        mRenderer = GetComponent<Renderer>();
        semiTransparentMat = ConstructionManager.Instance.ghostSemiTransparentMat;
        fullTransparentnMat = ConstructionManager.Instance.ghostFullTransparentMat;
        selectedMaterial = ConstructionManager.Instance.ghostSelectedMat;

        mRenderer.material = fullTransparentnMat;

        solidCollider.enabled = false;
    }

    /// <summary>
    /// 建設モードの状態に基づいてゴーストアイテムの状態を更新します。
    /// </summary>
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

    /// <summary>
    /// ゴーストアイテムが他のコライダー内にあるときの衝突検出を処理します。
    /// </summary>
    /// <param name="other">このゴーストアイテムが接触しているコライダー。</param>
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject);

        if (!this.gameObject.CompareTag("wallghost") && other.CompareTag("placedFoundation") && !ConstructionManager.Instance.inConstructionMode)
        {
            this.gameObject.SetActive(false);
            Debug.Log("ゴーストがplacedFoundationにヒットしました" + gameObject.name);
        }
    }
}
