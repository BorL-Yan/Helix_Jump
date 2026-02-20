using System;
using Platform.Main_Scene;
using UnityEngine;

public class GameAction
{
    public Action<float> MoveX;
    public Action<float> MoveY;
    public Action<Vector2> TouchScreen;
    
    public Action<int> SelectPlatform;
    public Action<SelectingPlatform> SelectingPlatform;
    public Action<SelectingPlatform> ActivateGameSelectingPlatform;
    public Action<int> ActivateGameSelectPlatform;
    public Action MoveToActivePlatform;
    public Action<SelectingPlatform> MoveToPlatform;

}
