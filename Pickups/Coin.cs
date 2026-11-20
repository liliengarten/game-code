using UnityEngine;

public class Coin : MonoBehaviour
{
    private float CoinScore = 100f;

    public float GetCoinScore() {
        return CoinScore;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}