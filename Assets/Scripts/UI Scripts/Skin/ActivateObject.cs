using UnityEngine;

namespace UI_Scripts.Skin
{
    public class ActivateObject : MonoBehaviour
    {
        [SerializeField] private GameObject _obj;

        protected virtual void OnEnable()
        {
            _obj.SetActive(true);
            GameManager.Instance.Action.ActivateSkinPanel?.Invoke(true);
            GameManager.Instance.GameState = GameState.skin;
        }
        
        protected virtual void OnDisable()
        {
            _obj.SetActive(false);
            GameManager.Instance.Action.ActivateSkinPanel?.Invoke(false);
            GameManager.Instance.GameState = GameState.level;
        }
    }
}