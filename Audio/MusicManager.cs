using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {
    public static MusicManager Instance;

    [SerializeField] private GameVolumeSettings gameVolumeSettings;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioMixer mixer;
    private float time;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        musicAudioSource.time = time;
        gameVolumeSettings.OnMusicVolumeChanged   += GameVolumeSettings_OnMusicVolumeChanged;
        gameVolumeSettings.OnVolumeSettingsLoaded += GameVolumeSettings_OnMusicVolumeChanged;
        gameVolumeSettings.LoadFromUserPreferences();
    }

    private void Update() {
        time = musicAudioSource.time;
    }

    private void GameVolumeSettings_OnMusicVolumeChanged(object sender, System.EventArgs e) {
        mixer.SetFloat("musicVolume", gameVolumeSettings.MusicVolume);
    }
}
