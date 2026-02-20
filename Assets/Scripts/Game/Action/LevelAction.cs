using System;
using UnityEngine;


public class LevelAction
{
    public Action OnStartLevel;
    public Action OnEndLevel;
    public Action OnFinishLevel;
    public Action OnLoos;


    public Action<int> OnSetPoint;
    public Action<int> OnPointAnimation;
    public Action OnActivateFinalPointAnimation;
    public Action AddMultiplyCount;
    
    public Func<Vector3> GetFinshPosition;
}
