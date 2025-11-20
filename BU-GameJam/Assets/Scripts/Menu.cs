using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnCreditsButton()
    {
        //to be implemented
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
