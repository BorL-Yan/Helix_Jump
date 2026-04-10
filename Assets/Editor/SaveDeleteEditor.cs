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

        settings.Coin = 750;
		settings.Level = 3;
        settings.RankedID = 0;
        settings.LeaderID = 4000;
        settings.Key = 2;
        GameSave.SetSettings(settings);
        GameSave.Save();
                  
        Debug.Log("Deleted");
    }
}
