using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI globalStatsTextMesh;
    [SerializeField] private TextMeshProUGUI playerStatsTextMesh;
    [SerializeField] private Lander lander;

    [SerializeField] private GameObject upForceArrowImage;
    [SerializeField] private GameObject downForceArrowImage;
    [SerializeField] private GameObject leftForceArrowImage;
    [SerializeField] private GameObject RightForceArrowImage;

    [SerializeField] private Image fuelAmountImage;

    private void UpdateStatsTextMesh() {
        upForceArrowImage.SetActive(Mathf.Round((lander.GetVelocityY() * 10f)) > 0);
        downForceArrowImage.SetActive(Mathf.Round((lander.GetVelocityY() * 10f)) <= 0);
        leftForceArrowImage.SetActive(Mathf.Round((lander.GetVelocityX() * 10f)) < 0);
        RightForceArrowImage.SetActive(Mathf.Round((lander.GetVelocityX() * 10f)) > 0);

        fuelAmountImage.fillAmount = lander.GetFuelAmount() / lander.GetFuelCapacity();

        globalStatsTextMesh.text = 
            GameManager.Instance.GameScore + "\n" +
            Mathf.Round(GameManager.Instance.GetGameTime()) + "\n";

        playerStatsTextMesh.text =
            System.Math.Round(lander.GetVelocityX(), 2) + "\n" +
            System.Math.Round(lander.GetVelocityY(), 2) + "\n";

    }

    private void FixedUpdate() {
        UpdateStatsTextMesh();
    }
}
