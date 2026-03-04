namespace UI_Scripts
{
    public class PlayButton : UIButton
    {
        protected override void Click()
        {
            GameManager.Instance.ActivateLevelScene(null);
        }
    }
}