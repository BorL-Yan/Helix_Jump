using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

namespace Level.Controllers
{
    public class PlatformActivate : MonoBehaviour
    {
        [SerializeField] private PlatformController _controller;
        
        public void ActivateBoom()
        {
            _controller.ActivateBoom();
            gameObject.SetActive(false);
        }

        public void ActivateBoom(Material material)
        {
            _controller.ActivateBoom(material);
            gameObject.SetActive(false);
        }
        
        
        private void OnEnable()
        {
            int id = gameObject.transform.GetInstanceID();
            PlatformActivatorList.Instance.AddItem(id, this);
        }
        
        private void OnDisable()
        {
            PlatformActivatorList.Instance.RemoveItem(gameObject.transform.GetInstanceID());
        }
        
        private void OnValidate()
        {
            // Автозаполнение, если ссылка пустая
            if (_controller == null)
            {
                _controller = GetComponentInParent<PlatformController>();
            }
        }

        [ProButton]
        private void Reset()
        {
            // Выполнится при добавлении компонента на объект
            _controller = GetComponentInParent<PlatformController>();
        }
    }
}