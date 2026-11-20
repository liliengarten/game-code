using System.Collections.Generic;
using System;
using Unity.Cinemachine;
using Unity.Tutorials.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameStates {
        Running,
        NotRunning,
        Paused
    }

    public GameStates GameState = GameStates.NotRunning;
    public EventHandler<GameStates> OnGameStateChange;

    private static int currentLevelNumber = 1;

    [SerializeField] private Lander lander;
    [SerializeField] private Stopwatch stopwatch;
    [SerializeField] private CinemachineCameraManager cinemachineCameraManager;
    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private LandingUI landingUI;
    [SerializeField] private PauseUI pauseUI;

    private GameLevel loadedGameLevel;

    public float GameScore { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    private void Start() {
        //Lander events
        lander.OnUpForce    += Lander_OnForce;
        lander.OnLeftForce  += Lander_OnForce;
        lander.OnRightForce += Lander_OnForce;
        lander.OnCoinPickup += AddGameScore;
        lander.OnLanding    += StopGame;

        //UI events
        landingUI.OnLevelRestartButtonPressed += LandingUI_OnLevelRestartButtonPressed;
        landingUI.OnNextLevelButtonPressed    += LandingUI_OnNextLevelButtonPressed;

        GameInput.Instance.onMenuActionPressed += PauseUI_OnPauseButtonPressed;
        pauseUI.onUnpauseButtonPressed  += PauseUI_OnPauseButtonPressed;
        pauseUI.onMainMenuButtonPressed += PauseUI_OnMainMenuButtonPressed;
        
        LoadCurrentLevel();        
    }

    private void ChangeGameState(GameStates state) {
        OnGameStateChange?.Invoke(this, state);
        GameState = state;
    }

    private void LoadCurrentLevel() {
        loadedGameLevel = Instantiate(gameLevelList[currentLevelNumber - 1], Vector3.zero, Quaternion.identity);
        lander.transform.position = loadedGameLevel.GetLanderStartPositionTransform();

        UpdateCinemachineCamera(loadedGameLevel.GetCameraStartTrackingPosition(), lander.State);
    }

    public float GetGameTime() {
        return stopwatch.Time;
    }

    public float GetGameScore() {
        return GameScore;
    }

    public void AddGameScore(object sender, float score) {
        GameScore += score;
    }

    public void StartGame() {
        ChangeGameState(GameStates.Running);
        UpdateCinemachineCamera(lander.transform, lander.State);

        GameScore = 0;

        stopwatch.StartStopwatch();
    }

    private void PauseUnpauseGame() {
        switch (GameState) {
            case GameStates.Running:
                ChangeGameState(GameStates.Paused);
                Time.timeScale = 0f;
                break;

            case GameStates.Paused:
                ChangeGameState(GameStates.Running);
                Time.timeScale = 1f;
                break;
        }
    }

    public void StopGame(object sender, Lander.LandingData landingData) {
        stopwatch.StopStopwatch();
        GameScore = GameScore * landingData.landingAngle * landingData.landingPadMultiplier;        
        ChangeGameState(GameStates.NotRunning);
    }


    //Handling events
    public void Lander_OnForce(object sender, System.EventArgs e) {        
        if (GameState == GameStates.NotRunning) { 
            StartGame();
        }
    }

    private void LandingUI_OnLevelRestartButtonPressed(object sender, System.EventArgs e) {
        SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
    }

    private void LandingUI_OnNextLevelButtonPressed(object sender, System.EventArgs e) {
        currentLevelNumber++;
        SceneLoader.LoadScene(SceneLoader.Scenes.GameScene);
    }

    private void PauseUI_OnPauseButtonPressed(object sender, System.EventArgs e) {
        PauseUnpauseGame();
    }  

    private void PauseUI_OnMainMenuButtonPressed(object sender, System.EventArgs e) {
        PauseUnpauseGame();
        SceneLoader.LoadScene(SceneLoader.Scenes.MainMenuScene);
    }

    private void UpdateCinemachineCamera(Transform target, Lander.States playerState) {
        cinemachineCameraManager.ChangeCinemachineCameraTargetOrtographicSize(playerState);
        cinemachineCameraManager.ChangeCinemachineCameraTarget(target);
    }
}