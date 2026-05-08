using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaveSystem : MonoBehaviour
{
    [Header("Ссылки на игрока")]
    public Transform playerTransform;
    
    [Header("Настройки")]
    public string gameSceneName = "GameScene";
    
    private static GameSaveSystem instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        if (HasSaveFile())
        {
            LoadGame();
        }
    }

    public void SaveAndExit()
    {
        Debug.Log("Сохраняем прогресс и выходим...");
        SaveGame();
        
        System.Threading.Thread.Sleep(100);
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void SaveGame()
    {
        SaveData data = new SaveData();
        
        data.sceneName = SceneManager.GetActiveScene().name;
        
        if (playerTransform != null)
        {
            data.playerPosX = playerTransform.position.x;
            data.playerPosY = playerTransform.position.y;
            data.playerPosZ = playerTransform.position.z;
        }
       
        string path = GetSavePath();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();
        
        Debug.Log($"Прогресс сохранён! Сцена: {data.sceneName}, Позиция: ({data.playerPosX}, {data.playerPosY}, {data.playerPosZ})");
    }
    
    public void LoadGame()
    {
        string path = GetSavePath();
        
        if (!File.Exists(path))
        {
            Debug.Log("Сохранение не найдено, начинаем новую игру");
            return;
        }
        
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            
            if (!string.IsNullOrEmpty(data.sceneName))
            {
                SceneManager.LoadScene(data.sceneName);
                StartCoroutine(LoadPositionAfterSceneLoad(data));
            }
            
            Debug.Log("Прогресс загружен!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка загрузки: " + e.Message);
        }
    }
    
    private System.Collections.IEnumerator LoadPositionAfterSceneLoad(SaveData data)
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerTransform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
            Debug.Log($"Игрок перемещён на позицию: ({data.playerPosX}, {data.playerPosY}, {data.playerPosZ})");
        }
        else
        {
            Debug.LogWarning("Игрок не найден в сцене! Убедитесь, что у игрока есть тег 'Player'");
        }
    }
    
    public bool HasSaveFile()
    {
        return File.Exists(GetSavePath());
    }
    
    public void DeleteSave()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Сохранение удалено");
        }
    }
    
    private string GetSavePath()
    {
        return Application.persistentDataPath + "/game_save.dat";
    }
}

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
}