using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    public class OpenCloss : UIButton
    {
        //[SerializeField] private ActionType actionType;
        [SerializeField] private GameObject settingsPanel;
        private bool openClose = true;
        protected override void Click()
        {
            switch (openClose)
            {
                case true:
                {
                    settingsPanel.SetActive(true);
                    openClose = false;
                    break;
                }

                case false:
                {
                    settingsPanel.SetActive(false);
                    openClose = true;
                    break;
                }
            }
        }
    }
}