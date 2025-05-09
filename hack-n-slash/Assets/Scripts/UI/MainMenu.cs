using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuScreen;

    private void Awake()
    {
        menuScreen.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Volume()
    {

    }

    public void Music()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
