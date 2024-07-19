using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text mapName;
    [SerializeField] private Image mapImage;
    [SerializeField] private Button playButton;

    public void DisplayMap(Map map)
    {
        mapName.text = map.mapName;
        mapImage.sprite = map.mapImage;

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => SceneManager.LoadScene(map.sceneLoad.name));
    }
}

