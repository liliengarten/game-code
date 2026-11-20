using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour {
    [SerializeField] private GameVolumeSettings gameVolumeSettings;

    [SerializeField] private Button unpauseButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    public EventHandler onUnpauseButtonPressed;
    public EventHandler onMainMenuButtonPressed;

    private void Start() {
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;

        musicVolumeSlider.onValueChanged.AddListener(gameVolumeSettings.ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(gameVolumeSettings.ChangeSfxVolume);

        unpauseButton.onClick.AddListener(() => {
            onUnpauseButtonPressed?.Invoke(this, System.EventArgs.Empty);
            Hide();
        });

        mainMenuButton.onClick.AddListener(() => {
            onMainMenuButtonPressed?.Invoke(this, System.EventArgs.Empty);
        });

        musicVolumeSlider.value = gameVolumeSettings.MusicVolume;
        sfxVolumeSlider.value   = gameVolumeSettings.SfxVolume;
        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    private void GameManager_OnGameStateChange(object sender, GameManager.GameStates state) {
        switch (state) {
            case GameManager.GameStates.Running:
                Hide();
                break;

            case GameManager.GameStates.Paused:
                Show();
                break;
        }
    }
}