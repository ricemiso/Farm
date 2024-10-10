using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSaveManager : MonoBehaviour
{
    public static MainMenuSaveManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
          
        }

        DontDestroyOnLoad(gameObject);

    }

    string JsonPathProject;
    string JsonPathPersistant;

    string binaryPath;

    string fileName = "SaveGame";

    public bool isSavingJason;

    public Canvas loadScreen;

    private void Start()
    {
        JsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        JsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }


    #region || -------- セーブ＆ロード -------- ||


    #region || -------- Saving -------- ||
    public void SaveGame(int sloaNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        data.enviromentData = GetEnviromentData();

        SelectSavingType(data, sloaNumber);
    }

    private EnviromentData GetEnviromentData()
    {
        List<string> itemsPickedup = InventorySystem.Instance.itemsPickedup;
        print(InventorySystem.Instance.itemsPickedup);
        print(itemsPickedup);
        return new EnviromentData(itemsPickedup);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStates = new float[3];
        playerStates[0] = PlayerState.Instance.currentHealth;
        playerStates[1] = PlayerState.Instance.currentCalories;
        playerStates[2] = PlayerState.Instance.currentHydrationPercent;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        string[] inventry = InventorySystem.Instance.itemList.ToArray();

        string[] quickSlot = GetQuickSlotcontent();


        return new PlayerData(playerStates, playerPosAndRot, inventry, quickSlot);
    }

    private string[] GetQuickSlotcontent()
    {
        List<string> temp = new List<string>();

        foreach(GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if(slot.transform.childCount != 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }

        return temp.ToArray();
    }

    public void SelectSavingType(AllGameData gameData,int slotNumber)
    {
        if (isSavingJason)
        {
            SaveGameDataToJsonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }

    #endregion

    #region || -------- Loading -------- ||

    public void LoadGame(int slotNumber)
    {
      
        SetPlayerData(SelectedLoadingType(slotNumber).playerData);
        SetEnviromentData(SelectedLoadingType(slotNumber).enviromentData);


        DisableLoadingScene();
    }


    private void SetEnviromentData(EnviromentData enviromentData)
    {

        foreach (Transform itemType in EnviromentManager.Instance.allItems.transform)
        {
            foreach (Transform item in itemType.transform)
            {
                if (enviromentData.pickedupItems.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.itemsPickedup = enviromentData.pickedupItems;
    }



    public AllGameData SelectedLoadingType(int slotNumber)
    {
        Debug.Log("Selecting loading type for slot: " + slotNumber);

        AllGameData gameData = null;

        if (isSavingJason)
        {
            gameData = LoadGameDataFromJsonFile(slotNumber);
        }
        else
        {
            gameData = LoadGameDataFromBinaryFile(slotNumber);
        }

        if (gameData != null)
        {
            Debug.Log("Game data successfully loaded from " + (isSavingJason ? "JSON" : "Binary") + " for slot: " + slotNumber);
        }
        else
        {
            Debug.LogError("Failed to load game data for slot: " + slotNumber);
        }

        return gameData;
    }

    private void SetPlayerData(PlayerData playerData)
    {
        //プレイヤーの状態
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];


        //プレイヤーの位置
        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;


        //プレイヤーの回転
        Vector3 loadRotation;
        loadRotation.x = playerData.playerPositionAndRotation[3];
        loadRotation.y = playerData.playerPositionAndRotation[4];
        loadRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadRotation);
    
    
        foreach(string item in playerData.inventortContent)
        {
            InventorySystem.Instance.LoadToinventry(item);
        }

        foreach (string item in playerData.quickSlotContent)
        {
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();

            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));
            itemToAdd.transform.SetParent(availableSlot.transform, false);
        }
    }


    public void StartLoadedGame(int slotNumber)
    {
        ActivateLoadingScene();

        SceneManager.LoadScene("MainScene");

        StartCoroutine(DelayerLoading(slotNumber));

        
    }

    private IEnumerator DelayerLoading(int slotNumber)
    {
        yield return new WaitForSeconds(5f);


        LoadGame(slotNumber);


    }
    #endregion
    #endregion



    #region || -------- バイナリに保存 -------- ||


    public void SaveGameDataToBinaryFile(AllGameData gameData ,int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Save to" + binaryPath + fileName + slotNumber + ".bin");
    }

    public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
    {

        if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Loaded from " + binaryPath + fileName + slotNumber + ".bin");

            return data;
        }
        else
        {
            print("Save file not found at " + binaryPath + fileName + slotNumber + ".bin"); // ファイルが見つからない場合のメッセージ
            return null;
        }
    }


    #endregion



    #region || -------- Jsonに保存 -------- ||


    public void SaveGameDataToJsonFile(AllGameData gameData,int slotNumber)
    {
        string Json = JsonUtility.ToJson(gameData);

        string encrypted = EncryptionDecryption(Json);


        using (StreamWriter writer = new StreamWriter(JsonPathProject + fileName + slotNumber + ".json")) 
        {
            writer.Write(encrypted);
            print("Saved Game To Json" + JsonPathProject + fileName + slotNumber + ".json");
        }
    }

    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        using(StreamReader reader = new StreamReader(JsonPathProject + fileName + slotNumber + ".json"))
        {
            string Json = reader.ReadToEnd();

            string decrypted = EncryptionDecryption(Json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
            return data;
        }
    }


    #endregion



    #region || -------- 設定 -------- ||


    #region || -------- Volume Setting -------- ||
    [System.Serializable]
    public class VolumeSettings
    {
        public float musics;
        public float effects;
        public float masters;
    }

    public void SaveVolumeSettings(float music, float effect, float master)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            musics = music,
            effects = effect,
            masters = master
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();


        print("Saved Player Prefs");

    }


    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
    }

    public float LoadMusicSettings()
    {
        var volumesettings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Music"));
        return volumesettings.musics;
    }


    #endregion 



    #endregion 


    #region || -------- 暗号化 -------- ||

    public string EncryptionDecryption(string data)
    {
        string keyword = "1234567";

        string result = "";


        //キーワードとJsonの文字列をびっとえんざん(XOR)を行うことで暗号化
        for(int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }

    #endregion


    #region || -------- ボタンからロード -------- ||

    public bool DoesFileExist(int sloatNumber)
    {
        if(isSavingJason)
        {
            if (System.IO.File.Exists(JsonPathProject + fileName + sloatNumber + ".json")) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.IO.File.Exists(binaryPath + fileName + sloatNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExist(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    #endregion


    #region || -------- ロードセクション -------- ||

    public void ActivateLoadingScene()
    {
        loadScreen.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Todo::ロードアニメーション、Tipsなどはここで
    }

    public void DisableLoadingScene()
    {
        loadScreen.gameObject.SetActive(false);
    }

    #endregion
}
