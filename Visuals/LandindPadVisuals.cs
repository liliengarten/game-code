using UnityEngine;
using TMPro;

public class LandindPadVisuals : MonoBehaviour
{
    [SerializeField] private TextMeshPro MultiplierText;

    private void Awake() {
        LandingPad landingPad = GetComponent<LandingPad>();
        MultiplierText.text = "x" + landingPad.Multiplier;
    }
}
