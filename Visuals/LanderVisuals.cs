using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LanderVisuals : MonoBehaviour
{
    private Lander lander;
    
    [SerializeField] private ParticleSystem LeftThrusterParticleSystem;
    [SerializeField] private ParticleSystem MiddleThrusterParticleSystem;
    [SerializeField] private ParticleSystem RightThrusterParticleSystem;

    [SerializeField] private GameObject ExplosionVfx;

    private ParticleSystem.EmissionModule LeftThrusterParticleSystemEmissionModule;
    private ParticleSystem.EmissionModule MiddleThrusterParticleSystemEmissionModule;
    private ParticleSystem.EmissionModule RightThrusterParticleSystemEmissionModule;

    private void Awake()
    {
        lander = GetComponent<Lander>();

        lander.OnNoForce    += Lander_OnNoForce;
        lander.OnLeftForce  += Lander_OnLeftforce;
        lander.OnUpForce    += Lander_OnUpforce;
        lander.OnRightForce += Lander_OnRightforce;
        
        lander.OnLanding += Lander_OnLanding;

        LeftThrusterParticleSystemEmissionModule   = LeftThrusterParticleSystem.emission;
        MiddleThrusterParticleSystemEmissionModule = MiddleThrusterParticleSystem.emission;
        RightThrusterParticleSystemEmissionModule  = RightThrusterParticleSystem.emission;
    }

    private void Lander_OnNoForce(object sender, System.EventArgs e) {
        ChangeParticleSystemEmissionModuleCondition(LeftThrusterParticleSystemEmissionModule, false);
        ChangeParticleSystemEmissionModuleCondition(MiddleThrusterParticleSystemEmissionModule, false);
        ChangeParticleSystemEmissionModuleCondition(RightThrusterParticleSystemEmissionModule, false);
    }

    private bool checkFuelAmount() {
        if (lander.FuelAmount <= 0) { 
            Lander_OnNoForce(null, null);
            return false;
        }

        return true;
    }

    private void Lander_OnLeftforce(object sender, System.EventArgs e) {
        if (!checkFuelAmount()) { return; }

        ChangeParticleSystemEmissionModuleCondition(RightThrusterParticleSystemEmissionModule, true);
    }

    private void Lander_OnUpforce(object sender, System.EventArgs e) {
        if (!checkFuelAmount()) { return; }

        ChangeParticleSystemEmissionModuleCondition(LeftThrusterParticleSystemEmissionModule, true);
        ChangeParticleSystemEmissionModuleCondition(MiddleThrusterParticleSystemEmissionModule, true);
        ChangeParticleSystemEmissionModuleCondition(RightThrusterParticleSystemEmissionModule, true);
    }

    private void Lander_OnRightforce(object sender, System.EventArgs e) {
        if (!checkFuelAmount()) { return; }
        
        ChangeParticleSystemEmissionModuleCondition(LeftThrusterParticleSystemEmissionModule, true);
    }

    private void Lander_OnLanding(object sender, Lander.LandingData landingData) {
        switch (landingData.landingType) {
            case Lander.LandingTypes.Crashed:
                gameObject.SetActive(false);
                Instantiate(ExplosionVfx, transform.position, Quaternion.identity);
                break;
        }
    }

    private void ChangeParticleSystemEmissionModuleCondition(ParticleSystem.EmissionModule particleSystemEmissionModule, bool condition) {
        particleSystemEmissionModule.enabled = condition;
    }
}
