using System;
using UnityEngine;

public class BallAction
{
    public Action Jump;
    public Action DeactivateGravity;
    public Action<Vector3> ActivateBreakPlatform;

    public Action<BallSkinType> OnActivateSkin;

    public Action<bool> ActivateCombo;
}
