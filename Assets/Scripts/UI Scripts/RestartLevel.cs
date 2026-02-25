using UnityEngine.SceneManagement;

public class RestartLevel : UIButton
{
    protected override void Click()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
