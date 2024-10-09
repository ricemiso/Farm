using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain; // Terrainオブジェクト
    public int numberOfTrees = 1000; // 生成するTreeの数
    public int numberOfStones = 1000; // 生成するStone_modelの数

    private GameObject itemToAdd;

    public bool inventoryUpdated;

    private bool isWaking; // 生成するかどうかのフラグ

    void Start()
    {
        SpawnObjectsOnTerrain();
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

        // Treeを生成
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain(terrainPosition, terrainSize);
            if (CheckLayerForObjectSpawn(spawnPosition))
            {
                float yPosition = terrain.SampleHeight(spawnPosition) + terrainPosition.y;
                Instantiate(itemToAdd, new Vector3(spawnPosition.x, yPosition, spawnPosition.z), Quaternion.identity);
                Debug.Log($"Treeが生成されました: {spawnPosition}");
            }
            else
            {
                Debug.Log("草の層が見つかりません、Tree生成スキップ");
            }
        }

        // Stone_modelのリソースをロード
        SetName("Stone_model");
        if (itemToAdd == null)
        {
            Debug.LogError("Stone_modelのリソースが見つかりません！");
            return;
        }

        // Stone_modelを生成
        for (int i = 0; i < numberOfStones; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain(terrainPosition, terrainSize);
            if (CheckLayerForObjectSpawn(spawnPosition))
            {
                float yPosition = terrain.SampleHeight(spawnPosition) + terrainPosition.y;
                Instantiate(itemToAdd, new Vector3(spawnPosition.x, yPosition, spawnPosition.z), Quaternion.identity);
                Debug.Log($"Stone_modelが生成されました: {spawnPosition}");
            }
            else
            {
                Debug.Log("草の層が見つかりません、Stone_model生成スキップ");
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
        Debug.Log("現在のテクスチャ層のインデックス: " + layerIndex);

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
}
