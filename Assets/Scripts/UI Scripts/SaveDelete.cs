using UnityEngine;

namespace UI_Scripts
{
    public class SaveDelete : UIButton
    {
        protected override void Click()
        {
            GameSave.Init();
            var settings = GameSave.GetSettings();
            settings = new GameSettings();

            settings.Coin = 100;
            settings.Level = 1;
            settings.RankedID = 0;
            settings.LeaderID = 4000;
            settings.Key = 0;
            GameSave.SetSettings(settings);
            GameSave.Save();
  
        }
    }
}