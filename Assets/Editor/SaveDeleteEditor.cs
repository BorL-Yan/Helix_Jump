using UnityEditor;
using UnityEngine;

public class SaveDeleteEditor
{
    [MenuItem("Tools/SaveDelete")]
    public static void DeleteLevelSave()
    {
        GameSave.Init();
        var settings = GameSave.GetSettings();
        settings = new GameSettings();

        settings.Coin = 1000;
        GameSave.SetSettings(settings);
        GameSave.Save();
        
        Debug.Log($"Deleted level save level : {settings.Level}, Coin { settings.Coin}");
    }
}
