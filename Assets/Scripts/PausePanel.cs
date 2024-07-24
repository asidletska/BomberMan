using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public GameObject panel, gameObjects;
    public GameManager manager;
    public GameObject[] items;
    public void PauseButtonPressed()
    {
        panel.SetActive(true);
        gameObjects.SetActive(false);
        items[2].SetActive(false);
        Time.timeScale = 0f;
    }
    public void ContinueButtonPressed()
    {
        panel.SetActive(false);
        gameObjects.SetActive(true);
        items[2].SetActive(true);
        Time.timeScale = 1.0f;
    }
    public void MenuButtonPressed()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

}
