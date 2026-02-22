using UnityEngine;

public class ActiveLevelUI: UIButton
{
    protected override void Click()
    {
        GameManager.Instance.Action.MoveToActivePlatform?.Invoke();
    }
}