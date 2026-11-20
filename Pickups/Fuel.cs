using UnityEngine;

public class Fuel : MonoBehaviour
{
    private float FuelAmount = 25f;

    public float GetFuelAmount() {
        return FuelAmount;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
