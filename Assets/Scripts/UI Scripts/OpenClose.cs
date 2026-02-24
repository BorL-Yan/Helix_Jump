using UnityEngine;
namespace UI_Scripts
{ 
    public class OpenClose : UIButton
    {
        [SerializeField] private State currentState;
        [SerializeField] private GameObject settingsPanel;

        protected override void Click()
        {
            switch (currentState)
            {
                case State.Open:
                    settingsPanel.SetActive(true);
                    break;

                case State.Close:
                    settingsPanel.SetActive(false);
                    break;
            }
        }

        public enum State
        {
            Open,
            Close
        }
    }
}
