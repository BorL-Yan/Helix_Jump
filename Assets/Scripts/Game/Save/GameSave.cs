using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Serialization;


public static class GameSave
{
    private static GameSettings currentSettings;
    private static string settingsFilePath;
    public static event Action OnChangeSettings;
    
    public static void Init()
    {
        settingsFilePath = Path.Combine(Application.persistentDataPath, "Settings.json");
        LoadSettings();
    }
    
    public static void Save()
    {
        OnChangeSettings?.Invoke();
        string json = JsonUtility.ToJson(currentSettings);
        File.WriteAllText(settingsFilePath, json);
    }
    
    private static void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            currentSettings = JsonUtility.FromJson<GameSettings>(json);
        }
        else
        {
            currentSettings = new GameSettings();
        }
        Save();
    }

    public static GameSettings GetSettings() => currentSettings;

    public static void SetSettings(GameSettings settings)
    {
        currentSettings = settings;
        Save();
    }
}

[System.Serializable]
public class GameSettings : ISerializationCallbackReceiver
{
    public int Level = 1;
    public int ActiveLevelSkin = 1;

    public int LeaderID = 3000;
    public int RankedID = 0;
    public int Coin = 100;
    public int Score = 0;
    public int Key = 0;

    public bool newSkin = false;
    
    public bool Vibration = true;
    public bool SFX = true;
    
    public BallSkinType ActiveBallSkin;

    public List<LevelData> LevelData = new();
    public Dictionary<BallSkinType, bool> ActiveSkins = new();

    [SerializeField] private List<BallSkinType> _skinKeys = new();
    [SerializeField] private List<bool> _skinValues = new();

    public void OnBeforeSerialize()
    {
        _skinKeys.Clear();
        _skinValues.Clear();
        foreach (var pair in ActiveSkins)
        {
            _skinKeys.Add(pair.Key);
            _skinValues.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        ActiveSkins = new Dictionary<BallSkinType, bool>();
        for (int i = 0; i != Math.Min(_skinKeys.Count, _skinValues.Count); i++)
        {
            ActiveSkins.Add(_skinKeys[i], _skinValues[i]);
        }
    }

    public GameSettings()
    {
        if (ActiveSkins.Count == 0)
        {
            ActiveSkins[BallSkinType.Sphere] = true;
            ActiveBallSkin = BallSkinType.Sphere;
            _skinKeys.Add(BallSkinType.Sphere);
            _skinValues.Add(true);
        }
    }
    
}


[Serializable]
public class LevelData
{
    public bool TakeKey;
}
