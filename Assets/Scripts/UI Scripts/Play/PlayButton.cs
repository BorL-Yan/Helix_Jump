namespace UI_Scripts
{
    public class PlayButton : UIButton
    {
        protected override void Click()
        {
            //Debug.Log($"Click {transform.name}");
            GameManager.Instance.ActivateLevelScene();
        }
        
    }
}