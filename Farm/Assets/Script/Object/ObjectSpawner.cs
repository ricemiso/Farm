using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain; // Terrainオブジェクト
    public int numberOfTrees = 1000; // 生成するTreeの数
    public int numberOfStones = 1000; // 生成するStone_modelの数

    private bool hasSpawned = false; // 一度だけ生成するためのフラグ
    private GameObject itemToAdd;
    public GameObject treeParent;
    public GameObject StoneParent;

    private GameObject spawnedTreesParent;  // 生成されたTreeの親オブジェクト
    private GameObject spawnedStonesParent; // 生成されたStone_modelの親オブジェクト

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // ゲーム開始時に現在のシーンがMainSceneかをチェック
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーンがMainSceneで、まだ生成されていない場合のみ生成
        if (scene.name == "MainScene" && !hasSpawned)
        {
            SpawnObjectsOnTerrain();
            hasSpawned = true; // フラグを立てて以降の生成を防止
        }
        // シーンがMainSceneでない場合、オブジェクトを削除
        else if (scene.name != "MainScene")
        {
            DestroyObjects();
        }
    }

    private GameObject SetName(string gameObject)
    {
        return itemToAdd = Resources.Load<GameObject>(gameObject);
    }

    void SpawnObjectsOnTerrain()
    {
        // Terrainのサイズを取得
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        // Treeのリソースをロード
        SetName("Tree");
        if (itemToAdd == null)
        {
            Debug.LogError("Treeのリソースが見つかりません！");
            return;
        }

        // Treeの親オブジェクトを作成
        spawnedTreesParent = new GameObject("SpawnedTrees");

        // Treeを生成
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain(terrainPosition, terrainSize);
            if (CheckLayerForObjectSpawn(spawnPosition))
            {
                float yPosition = terrain.SampleHeight(spawnPosition) + terrainPosition.y;
                GameObject newTree = Instantiate(itemToAdd, new Vector3(spawnPosition.x, yPosition, spawnPosition.z), Quaternion.identity);
                newTree.name = "Tree_" + (i + 1);  // 名前に連番を追加
                newTree.transform.SetParent(spawnedTreesParent.transform);  // 生成した木の親を設定
            }
        }

        // Stone_modelのリソースをロード
        SetName("Rock");
        if (itemToAdd == null)
        {
            Debug.LogError("Rockのリソースが見つかりません！");
            return;
        }

        // Stone_modelの親オブジェクトを作成
        spawnedStonesParent = new GameObject("SpawnedStones");

        // Stone_modelを生成
        for (int i = 0; i < numberOfStones; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain(terrainPosition, terrainSize);
            if (CheckLayerForObjectSpawn(spawnPosition))
            {
                float yPosition = terrain.SampleHeight(spawnPosition) + terrainPosition.y;
                GameObject newStone = Instantiate(itemToAdd, new Vector3(spawnPosition.x, yPosition, spawnPosition.z), Quaternion.identity);
                newStone.name = "Rock_" + (i + 1);  // 名前に連番を追加
                newStone.transform.SetParent(spawnedStonesParent.transform);  // 生成した石の親を設定
            }
        }
    }

    // Terrainの範囲内でランダムなX,Z座標を取得するメソッド
    Vector3 GetRandomPositionOnTerrain(Vector3 terrainPosition, Vector3 terrainSize)
    {
        float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
        float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);
        return new Vector3(randomX, 0, randomZ);
    }

    // 地面のテクスチャによってオブジェクト生成を決定するメソッド
    private bool CheckLayerForObjectSpawn(Vector3 position)
    {
        int layerIndex = GetCurrentTerrainLayer(position);

        // もしLayer 0（草の層）ならばオブジェクト生成
        if (layerIndex == 0) // 草の層（Layer 0）
        {
            return true;
        }
        else
        {
            return false; // 他の層では生成しない
        }
    }

    // 現在の位置に対応する地面のテクスチャ層を確認
    private int GetCurrentTerrainLayer(Vector3 position)
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatmapData = terrainData.GetAlphamaps(
            (int)((position.x / terrainData.size.x) * terrainData.alphamapWidth),
            (int)((position.z / terrainData.size.z) * terrainData.alphamapHeight),
            1, 1);

        int maxTextureIndex = 0;
        float maxAlpha = 0f;

        // 最も強いアルファ値を持つテクスチャ（層）を特定
        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            float alpha = splatmapData[0, 0, i];

            if (alpha > maxAlpha)
            {
                maxAlpha = alpha;
                maxTextureIndex = i; // 最も強いテクスチャインデックスを選択
            }
        }

        return maxTextureIndex; // 最大のアルファ値を持つテクスチャインデックスを返す
    }

    // シーンが変更された際に生成されたオブジェクトを削除
    void DestroyObjects()
    {
        if (spawnedTreesParent != null)
        {
            Destroy(spawnedTreesParent);
        }
        if (spawnedStonesParent != null)
        {
            Destroy(spawnedStonesParent);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
