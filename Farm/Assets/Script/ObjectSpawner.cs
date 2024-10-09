using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain; // Terrain�I�u�W�F�N�g
    public int numberOfTrees = 1000; // ��������Tree�̐�
    public int numberOfStones = 1000; // ��������Stone_model�̐�

    private GameObject itemToAdd;

    public bool inventoryUpdated;

    private bool isWaking; // �������邩�ǂ����̃t���O

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
        // Terrain�̃T�C�Y���擾
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        // Tree�̃��\�[�X�����[�h
        SetName("Tree");
        if (itemToAdd == null)
        {
            Debug.LogError("Tree�̃��\�[�X��������܂���I");
            return;
        }

        // Tree�𐶐�
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain(terrainPosition, terrainSize);
            if (CheckLayerForObjectSpawn(spawnPosition))
            {
                float yPosition = terrain.SampleHeight(spawnPosition) + terrainPosition.y;
                Instantiate(itemToAdd, new Vector3(spawnPosition.x, yPosition, spawnPosition.z), Quaternion.identity);
                Debug.Log($"Tree����������܂���: {spawnPosition}");
            }
            else
            {
                Debug.Log("���̑w��������܂���ATree�����X�L�b�v");
            }
        }

        // Stone_model�̃��\�[�X�����[�h
        SetName("Stone_model");
        if (itemToAdd == null)
        {
            Debug.LogError("Stone_model�̃��\�[�X��������܂���I");
            return;
        }

        // Stone_model�𐶐�
        for (int i = 0; i < numberOfStones; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain(terrainPosition, terrainSize);
            if (CheckLayerForObjectSpawn(spawnPosition))
            {
                float yPosition = terrain.SampleHeight(spawnPosition) + terrainPosition.y;
                Instantiate(itemToAdd, new Vector3(spawnPosition.x, yPosition, spawnPosition.z), Quaternion.identity);
                Debug.Log($"Stone_model����������܂���: {spawnPosition}");
            }
            else
            {
                Debug.Log("���̑w��������܂���AStone_model�����X�L�b�v");
            }
        }
    }

    // Terrain�͈͓̔��Ń����_����X,Z���W���擾���郁�\�b�h
    Vector3 GetRandomPositionOnTerrain(Vector3 terrainPosition, Vector3 terrainSize)
    {
        float randomX = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
        float randomZ = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);
        return new Vector3(randomX, 0, randomZ);
    }

    // �n�ʂ̃e�N�X�`���ɂ���ăI�u�W�F�N�g���������肷�郁�\�b�h
    private bool CheckLayerForObjectSpawn(Vector3 position)
    {
        int layerIndex = GetCurrentTerrainLayer(position);
        Debug.Log("���݂̃e�N�X�`���w�̃C���f�b�N�X: " + layerIndex);

        // ����Layer 0�i���̑w�j�Ȃ�΃I�u�W�F�N�g����
        if (layerIndex == 0) // ���̑w�iLayer 0�j
        {
            return true;
        }
        else
        {
            return false; // ���̑w�ł͐������Ȃ�
        }
    }

    // ���݂̈ʒu�ɑΉ�����n�ʂ̃e�N�X�`���w���m�F
    private int GetCurrentTerrainLayer(Vector3 position)
    {
        TerrainData terrainData = terrain.terrainData;
        float[,,] splatmapData = terrainData.GetAlphamaps(
            (int)((position.x / terrainData.size.x) * terrainData.alphamapWidth),
            (int)((position.z / terrainData.size.z) * terrainData.alphamapHeight),
            1, 1);

        int maxTextureIndex = 0;
        float maxAlpha = 0f;

        // �ł������A���t�@�l�����e�N�X�`���i�w�j�����
        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            float alpha = splatmapData[0, 0, i];

            if (alpha > maxAlpha)
            {
                maxAlpha = alpha;
                maxTextureIndex = i; // �ł������e�N�X�`���C���f�b�N�X��I��
            }
        }

        return maxTextureIndex; // �ő�̃A���t�@�l�����e�N�X�`���C���f�b�N�X��Ԃ�
    }
}
