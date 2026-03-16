using com.cyborgAssets.inspectorButtonPro;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Platform.Controllers
{
    public class KeyActivate : MonoBehaviour
    {
        private LevelAction _levelAction;

        private GameObject _keyParent;
        [SerializeField] private GameObject _keyPrefab;

        [SerializeField] private float _platformDistance;
        [SerializeField] private int _keyCount;
        [SerializeField] private int _platformContinue;

        [SerializeField] private GameObject _boostPrefab;
        [SerializeField] private int _platformContinueBoost;
        
    
        [Inject]
        public void Construct( LevelAction levelAction)
        {
            _levelAction = levelAction;
        }
    

        private void Start()
        {
            ActivateBoost();
            ActivateKey();
        }
    
    
        [VInspector.Button]
        private void ActivateKey()
        {
            var settings = GameSave.GetSettings();
            int level = GameManager.Instance.CurrentActiveLevel;
            
            if (settings.LevelData == null) return;

            // 1. Убеждаемся, что в списке достаточно элементов, чтобы не было ошибки индекса
            while (settings.LevelData.Count <= level)
            {
                settings.LevelData.Add(new LevelData { TakeKey = false });
            }
            
            if (settings.LevelData[level - 1] == null)
            {
                settings.LevelData.Add(new LevelData()
                {
                    TakeKey = false
                });
            }
            
            if (settings.LevelData[level - 1 ].TakeKey == false)
            {
                _keyParent = new GameObject("Key");
                _keyParent.transform.SetParent(this.transform);
                
                for (int i = 0; i < _keyCount; i++)
                {
                    GameObject key = PrefabUtility.InstantiatePrefab(_keyPrefab, _keyParent.transform) as GameObject;
                    key.SetActive(true);
                    key.transform.position = (i + 1) * Vector3.down * _platformDistance * _platformContinue;
                }
            }
            
        }


        private void ActivateBoost()
        {
            GameObject BoostParent = new GameObject("Boost");
            BoostParent.transform.SetParent(this.transform);
            for (int i = 0; i < 2; i++)
            {
                GameObject boost = PrefabUtility.InstantiatePrefab(_boostPrefab, BoostParent.transform) as GameObject;
                boost.SetActive(true);
                boost.transform.position = (i + 1) * Vector3.down * _platformDistance * _platformContinueBoost;
                boost.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360),0);
            }
        }

        private void DeactivateKey()
        {
            _keyParent.SetActive(false);
        }


        private void OnEnable()
        {
            _levelAction.OnTakeKey += DeactivateKey;
        }

        private void OnDisable()
        {
            _levelAction.OnTakeKey -= DeactivateKey;
        }
    }
}