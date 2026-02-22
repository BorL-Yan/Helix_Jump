using UnityEngine;

namespace Level.Manager
{
    public class LevelManager : SingletonScene<LevelManager>
    {
        [field: SerializeField] public LevelProgress LevelProgress {get; private set;}
    }
}