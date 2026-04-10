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

        [SerializeField] private GameObject _boostParent;
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
                _keyParent.transform.SetParent(_boostParent.transform);
                
                for (int i = 0; i < _keyCount; i++)
                {
#if UNITY_EDITOR
                    GameObject key = PrefabUtility.InstantiatePrefab(_keyPrefab, _keyParent.transform) as GameObject;
#else
                    GameObject key = Instantiate(_keyPrefab, _keyParent.transform);
#endif
                    
                    key.SetActive(true);
                    key.transform.position = (i + 1) * Vector3.down * _platformDistance * _platformContinue;
                }
            }
        }
        
        private void ActivateBoost()
        {
            GameObject BoostParent = new GameObject("Boost");
            BoostParent.transform.SetParent(_boostParent.transform);
            for (int i = 0; i < 2; i++)
            {
#if UNITY_EDITOR
                GameObject boost = PrefabUtility.InstantiatePrefab(_boostPrefab, BoostParent.transform) as GameObject;
#else
                GameObject boost = Instantiate(_boostPrefab, BoostParent.transform) as GameObject;
#endif
                boost.SetActive(true);

                Vector3 pos = (i + 1) * Vector3.down * _platformDistance * _platformContinueBoost;
                pos -= Vector3.up * 0.1f;
                boost.transform.position = pos;
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