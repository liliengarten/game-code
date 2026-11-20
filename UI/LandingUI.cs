using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandingUI : MonoBehaviour
{
    [SerializeField] private Lander lander;

    [SerializeField] private TextMeshProUGUI landingTypeTextMesh;
    [SerializeField] private TextMeshProUGUI landingStatsNameTextMesh;
    [SerializeField] private TextMeshProUGUI landingStatsTextMesh;

    [SerializeField] private Button RestartButton;
    [SerializeField] private Button NextLevelButton;

    private Lander.LandingData landingData;
    
    public EventHandler OnLevelRestartButtonPressed;
    public EventHandler OnNextLevelButtonPressed;

    private void Start() {
        lander.OnLanding += Lander_OnLanding;
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;

        RestartButton.onClick.AddListener(() => {
            OnLevelRestartButtonPressed?.Invoke(this, System.EventArgs.Empty);
        });

        NextLevelButton.onClick.AddListener(() => {
            OnNextLevelButtonPressed?.Invoke(this, System.EventArgs.Empty);
        });

        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void UpdateLandingUI() {
        string landingType = "";

        if (landingData.landingType == Lander.LandingTypes.Successful) {
            landingType = "Successfull landing!";
        }
        else {
            landingType = "Crashed!";
        }

        landingTypeTextMesh.text = landingType;

        landingStatsTextMesh.text =
            System.Math.Round(landingData.landingSpeed, 2) + "\n" +
            System.Math.Round(landingData.landingAngle, 2) + "\n" +
            landingData.landingPadMultiplier + "x" + "\n" +
            "\n" +
            Mathf.Round(GameManager.Instance.GetGameTime()) + "s" + "\n" +
            Mathf.Round(GameManager.Instance.GetGameScore()); //!!!RACE CONDITION
    }

    public void GameManager_OnGameStateChange(object sender, GameManager.GameStates state) {
        if (state == GameManager.GameStates.NotRunning) { 
            UpdateLandingUI();
            Show();
        }
    }

    public void Lander_OnLanding(object sender, Lander.LandingData landingData) {
        this.landingData = landingData;
    }
}