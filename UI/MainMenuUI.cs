using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button QuitButton;

    private void Awake() {
        PlayButton.onClick.AddListener(() => {
            SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
        });

        QuitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
