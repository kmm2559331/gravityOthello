using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void TitleLoad()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void GameLoad()
    {
        SceneManager.LoadScene("GameScene");
    }
}
