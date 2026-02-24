using UnityEngine.SceneManagement;

public class RestartLevel : UIButton
{
    protected override void Click()
    {
        Restart();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
