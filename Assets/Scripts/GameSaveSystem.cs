using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class GameSaveSystem : MonoBehaviour
{
    [Header("Ссылки на игрока")]
    public Transform playerTransform;
    
    [Header("Настройки")]
    public string gameSceneName = "GameScene";
    
    [Header("Опционально: для сохранения прогресса экскурсии")]
    public ExcursionManager excursionManager;
    
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
        
        if (excursionManager != null && excursionManager.campusPoints != null)
        {
            data.completedPoints = new bool[excursionManager.campusPoints.Length];
            for (int i = 0; i < excursionManager.campusPoints.Length; i++)
            {
                data.completedPoints[i] = excursionManager.campusPoints[i].isCompleted;
            }
            Debug.Log($"Сохранено {data.completedPoints.Length} точек экскурсии");
        }
        
        // Сохраняем в JSON
        string json = JsonUtility.ToJson(data, true);
        string path = GetSavePath();
        File.WriteAllText(path, json);
        
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
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            if (data == null)
            {
                Debug.LogError("Ошибка парсинга JSON");
                return;
            }
            
            if (!string.IsNullOrEmpty(data.sceneName))
            {
                SceneManager.LoadScene(data.sceneName);
                StartCoroutine(LoadPositionAndProgressAfterSceneLoad(data));
            }
            
            Debug.Log("Прогресс загружен!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка загрузки: " + e.Message);
        }
    }
    
    private IEnumerator LoadPositionAndProgressAfterSceneLoad(SaveData data)
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
        
        if (excursionManager == null)
        {
            excursionManager = FindObjectOfType<ExcursionManager>();
        }
        
        if (excursionManager != null && data.completedPoints != null && data.completedPoints.Length > 0)
        {
            if (excursionManager.campusPoints != null && 
                excursionManager.campusPoints.Length == data.completedPoints.Length)
            {
                for (int i = 0; i < data.completedPoints.Length; i++)
                {
                    excursionManager.campusPoints[i].isCompleted = data.completedPoints[i];
                }
                Debug.Log($"Загружен прогресс экскурсии");
                
                // Обновляем UI если есть метод
                if (excursionManager.UpdateUI != null)
                {
                    excursionManager.UpdateUI();
                }
            }
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
        return Application.persistentDataPath + "/game_save.json";
    }
}

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    public bool[] completedPoints;
}