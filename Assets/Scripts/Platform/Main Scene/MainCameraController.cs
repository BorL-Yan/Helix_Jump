using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UI_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Pointer = UnityEngine.InputSystem.Pointer;

namespace Platform.Main_Scene
{
    public class MainCameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetPosOffset;
        [SerializeField] private float _speed;
        [SerializeField] private float _speedSmooth;

        [SerializeField] private float _minZ;
        [SerializeField] private float _maxZ;
        [SerializeField] private float _smoothRerset;

        [SerializeField] private LayerMask platformLayer;
        [SerializeField] private float rayDistance = 100f;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject goToActiveButton;
        [SerializeField] private PlayButtonController _playButton;

        [SerializeField] private float _moveSpeed;
    
        private bool moveToTarget;
        private Sequence moveSequence;
        private SelectingPlatform platform;
        private Vector3 _targetPos;

        private void Awake()
        {
            _targetPos = _targetPosOffset;

            GameManager.Instance.Action.MoveY += Move;
            GameManager.Instance.Action.TouchScreen += SelectPlatform;
            GameManager.Instance.Action.SelectingPlatform += InitialPlatform;
            GameManager.Instance.Action.MoveToActivePlatform += MoveToActivePlatform;
            GameManager.Instance.Action.ActivateGameSelectingPlatform += ActivateGame;
            UpdateGoToButton();
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Action.MoveY -= Move;
                GameManager.Instance.Action.TouchScreen -= SelectPlatform;
                GameManager.Instance.Action.SelectingPlatform -= InitialPlatform;
                GameManager.Instance.Action.MoveToActivePlatform -= MoveToActivePlatform;
                GameManager.Instance.Action.ActivateGameSelectingPlatform -= ActivateGame;
            }
        }

        private void ActivateGame(SelectingPlatform item)
        {
            platform = item;
            _targetPos = _targetPosOffset + new Vector3(0,0, item.gameObject.transform.position.z);
            transform.position = _targetPos;
        }

        private void MoveToActivePlatform()
        {
            MoveToTarget(platform.transform);
        }

        private void InitialPlatform(SelectingPlatform item)
        {
            platform = item;
            MoveToTarget(platform.transform);
        }

        private void SelectPlatform(Vector2 mousePosition)
        {
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (IsPointerOverUI()) return;
            
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, platformLayer))
            {
                SelectingPlatform item = hit.collider.GetComponent<SelectingPlatform>();

                platform?.Activate();
                platform = item;
                platform.Select();

                MoveToTarget(platform.transform);
                GameManager.Instance.Action.MoveToPlatform?.Invoke(platform);
                UpdateGoToButton();
            }
        }
        private bool IsPointerOverUI()
        {
            // 1. Проверяем, существует ли EventSystem
            if (EventSystem.current == null) return false;

            // 2. Получаем позицию указателя (мышь или первый тач)
            // Pointer.current автоматически подхватывает и мышь, и касание
            Vector2 pointerPosition = Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;

            // 3. Создаем данные события для рейкаста
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = pointerPosition
            };
            
            // 4. Список для записи результатов попадания в UI
            List<RaycastResult> results = new List<RaycastResult>();

            // 5. Проводим рейкаст по всем графическим элементам (Canvas)
            EventSystem.current.RaycastAll(eventData, results);

            // 6. Если в списке есть хотя бы один объект — значит, мы над UI
            return results.Count > 0;
        }


        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void Move(float y)
        {
            _targetPos = new Vector3(
                _targetPosOffset.x,
                _targetPosOffset.y,
                -y * _speed + _targetPos.z
            );
        }

        [ProButton]
        private void MoveToTarget(Transform target)
        {
            _targetPos.z = target.position.z + _targetPosOffset.z;
            moveToTarget = true;

            float distance = Mathf.Abs(transform.position.z - _targetPos.z);
            float moveTime = distance / _moveSpeed;
            
            moveSequence?.Kill();
            moveSequence = DOTween.Sequence();
            moveSequence.Append(transform.DOMoveZ(_targetPos.z, moveTime))
                .OnComplete(() =>
                {
                    moveToTarget = false;
                });
        }

        private void Update()
        {
            if (moveToTarget) return;

            float posZ = Mathf.Clamp(_targetPos.z, _minZ, _maxZ);

            _targetPos = Vector3.Lerp(
                _targetPos,
                new Vector3(_targetPos.x, _targetPos.y, posZ),
                Time.deltaTime * _smoothRerset
            );

            transform.position = Vector3.Lerp(
                transform.position,
                _targetPos,
                _speedSmooth * Time.deltaTime
            );
            UpdateGoToButton(); 

        }

       
        
        private bool IsPlatformVisible(Transform platformTransform)
        {
            Vector3 viewportPos = _camera.WorldToViewportPoint(platformTransform.position);

            return viewportPos.z > 0 &&
                   viewportPos.x > 0 && viewportPos.x < 1 &&
                   viewportPos.y > 0 && viewportPos.y < 1;
        }
        
        private void UpdateGoToButton()
        {
            if (platform == null)
            {
                goToActiveButton.SetActive(false);
                return;
            }

            bool isVisible = IsPlatformVisible(platform.transform);

            goToActiveButton.SetActive(!isVisible);
            _playButton.SetActive(isVisible);
        }

    }
}
