using System.Collections.Generic;
using UnityEngine;

namespace Platform.Configuration
{
    [CreateAssetMenu(fileName = "Level Color Collection", menuName = "Helix/ColorCollection")]
    public class LevelColorCollection : ScriptableObject
    {
        [SerializeField] private List<LevelColor> _levels;

        public LevelColor GetColor()
        {
            int id = GameManager.Instance.CurrentActiveLevel;
            if (_levels == null || id < 0 || id >= _levels.Count)
            {
                return _levels != null && _levels.Count > 0 ? _levels[0] : null;
            }
            return _levels[id];
        }
    }
}