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
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void Music()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
