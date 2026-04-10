using System.Collections.Generic;
using UnityEngine;

namespace Level.Controllers
{
    public class PlatformActivatorList : SingletonScene<PlatformActivatorList>
    {
        private Dictionary<int, PlatformActivate> _platformActivates = new Dictionary<int, PlatformActivate>();
        public PlatformActivate GetPlatformActivate(int id)
        {
            if (_platformActivates.TryGetValue(id, out var value))
            {
                return value;
                Debug.Log(_platformActivates.Count);
            }
            else
            {
                Debug.LogWarning("PlatformActivate " + id + " does not exists: " + _platformActivates.Count);
            }
            return null;
        }
        public void AddItem(int id, PlatformActivate platformController)
        {
            if (!_platformActivates.TryAdd(id, platformController))
            {
                Debug.LogWarning("PlatformActivate " + id + " already exists");
            }
        }
        public void RemoveItem(int id)
        {
            if (!_platformActivates.Remove(id))
            {
                Debug.LogWarning("PlatformController " + id + " does not exists");
            }
        }
    }
}