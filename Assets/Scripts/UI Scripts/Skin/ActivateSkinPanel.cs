using UnityEngine;

namespace UI_Scripts.Skin
{
    public class ActivateSkinPanel : ActivateObject
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.Instance.GameState = GameState.skin;
            GameManager.Instance.Action.ActivateSkinPanel?.Invoke(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.Instance.Action.ActivateSkinPanel?.Invoke(false);
            GameManager.Instance.GameState = GameState.level;
        }
    }
}