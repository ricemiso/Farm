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

    public bool isSavingJason;

    #region || -------- General Section -------- ||


    #region || -------- Saving -------- ||
    public void SaveGame()
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();
        SelectSavingType(data);
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

        return new PlayerData(playerStates, playerPosAndRot);
    }

    public void SelectSavingType(AllGameData gameData)
    {
        if (isSavingJason)
        {
            //SaveGameDataToJsonFile();
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }

    #endregion

    #region || -------- Loading -------- ||

    public void LoadGame()
    {
        print("Starting to load game...");  // デバッグメッセージ追加

        SetPlayerData(SelectedLoadingType().playerData);

        //ここに追加していく
    }

    public AllGameData SelectedLoadingType()
    {
        print("Selecting loading type...");  // デバッグメッセージ追加

        if (isSavingJason)
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();

            return gameData;
        }
    }

    private void SetPlayerData(PlayerData playerData)
    {
        if (PlayerState.Instance == null)
        {
            print("PlayerState.Instance is null!");
            return;
        }

        if (PlayerState.Instance.playerBody == null)
        {
            print("Player body is null!");
            return;
        }

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
    }


    public void StartLoadedGame()
    {
        SceneManager.LoadScene("MainScene");

        StartCoroutine(DelayerLoading());

        
    }

    private IEnumerator DelayerLoading()
    {
        yield return new WaitForSeconds(2f);

        print("Now loading game data...");
        LoadGame(); 

        print("Game Loaded");
    }
    #endregion
    #endregion



    #region || -------- To Binary Section -------- ||


    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save_game.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Save to" + Application.persistentDataPath + "/save_game.bin");
    }

    public AllGameData LoadGameDataFromBinaryFile()
    {
        print("Attempting to load game data...");

        string path = Application.persistentDataPath + "/save_game.bin";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Loaded from " + path);

            return data;
        }
        else
        {
            print("Save file not found at " + path); // ファイルが見つからない場合のメッセージ
            return null;
        }
    }


    #endregion

    #region || -------- Setting Section -------- ||


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
        var volumesettings = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        return volumesettings.musics;
    }


    #endregion 



    #endregion 




}
