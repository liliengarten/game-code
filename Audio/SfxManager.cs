using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
    [SerializeField] private Lander lander;
    [SerializeField] private GameVolumeSettings gameVolumeSettings;
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private AudioSource landerThrusterAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip coinPickupAudioClip;
    [SerializeField] private AudioClip fuelPickupAudioClip;
    [SerializeField] private AudioClip landingAudioClip;
    [SerializeField] private AudioClip crashAudioClip;

    private void Start() {
        lander.OnNoForce    += Lander_OnNoForce;
        lander.OnUpForce    += Lander_OnForce;
        lander.OnLeftForce  += Lander_OnForce;
        lander.OnRightForce += Lander_OnForce;
        lander.OnCoinPickup += Lander_OnCoinPickup;
        lander.OnFuelPickup += Lander_OnFuelPickup;
        lander.OnLanding    += Lander_OnLanding;

        gameVolumeSettings.OnSfxVolumeChanged += GameVolumeSettings_OnSfxVolumeChanged;
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;
     }

    private void GameVolumeSettings_OnSfxVolumeChanged(object sender, System.EventArgs e) {
        mixer.SetFloat("sfxVolume", gameVolumeSettings.SfxVolume);
    }

    private void Lander_OnForce(object sender, System.EventArgs e) {
        if (lander.FuelAmount <= 0) {
            Lander_OnNoForce(null, null);
            return;
        }

        if (!landerThrusterAudioSource.isPlaying) { landerThrusterAudioSource.Play(); }
    }

    private void Lander_OnNoForce(object sender, System.EventArgs e) {
        if (landerThrusterAudioSource.isPlaying) { landerThrusterAudioSource.Stop(); }
    }

    private void GameManager_OnGameStateChange(object sender, GameManager.GameStates state) {
        if (state == GameManager.GameStates.Paused) { Lander_OnNoForce(null, null); }
    }

    private void Lander_OnCoinPickup(object sender, float e) {
        sfxAudioSource.PlayOneShot(coinPickupAudioClip);
    }
    
    private void Lander_OnFuelPickup(object sender, System.EventArgs e) {
        sfxAudioSource.PlayOneShot(fuelPickupAudioClip);
    }

    private void Lander_OnLanding(object sender, Lander.LandingData landingData) {
        switch (landingData.landingType) {
            case Lander.LandingTypes.Successful:
                sfxAudioSource.PlayOneShot(landingAudioClip);
                break;

            case Lander.LandingTypes.Crashed:
                sfxAudioSource.PlayOneShot(crashAudioClip);
                break;
        }
    }
}
