using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private const string SAVE_KEY = "CARD_MATCH_SAVE";

    private void Awake()
    {
        Instance = this;
    }

    public void SaveGame(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public GameSaveData LoadGame()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return null;

        string json = PlayerPrefs.GetString(SAVE_KEY);

        return JsonUtility.FromJson<GameSaveData>(json);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
    }
}