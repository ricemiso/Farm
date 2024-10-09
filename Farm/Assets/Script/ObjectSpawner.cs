using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain; // Terrain�I�u�W�F�N�g
    public int numberOfObjects = 10; // ��������I�u�W�F�N�g�̐�

    private GameObject itemToAdd;

    public bool inventoryUpdated;

    private bool isWaking; // �������邩�ǂ����̃t���O

    void Start()
    {
        SpawnObjectsOnTerrain();
    }

    void SpawnObjectsOnTerrain()
    {
        // Terrain�̃T�C�Y���擾
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        // Tree�̃��\�[�X�����[�h
        itemToAdd = Resources.Load<GameObject>("Tree");

        if (itemToAdd == null)
        {
            Debug.LogError("Tree�̃��\�[�X��������܂���I");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Terrain�͈͓̔��̃����_���Ȉʒu���v�Z
            Vector3 spawnPosition = GetRandomPositionOnTerrain(terrainPosition, terrainSize);

            // ���݂̈ʒu�̃e�N�X�`�������ł���΃I�u�W�F�N�g�𐶐�
            if (CheckLayerForObjectSpawn(spawnPosition))
            {
                // ���፷�ɍ��킹��Y���W���擾
                float yPosition = terrain.SampleHeight(spawnPosition) + terrainPosition.y;

                // �I�u�W�F�N�g��Terrain��ɐ��� (Y���W�͍��፷�ɍ��킹��)
                Instantiate(itemToAdd, new Vector3(spawnPosition.x, yPosition, spawnPosition.z), Quaternion.identity);
                Debug.Log($"Tree����������܂���: {spawnPosition}");
            }
            else
            {
                Debug.Log("���̑w��������܂���A�I�u�W�F�N�g�����X�L�b�v");
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
