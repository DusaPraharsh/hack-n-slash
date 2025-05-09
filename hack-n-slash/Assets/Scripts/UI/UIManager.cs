using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public AudioClip gameOverSound;

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }
}
