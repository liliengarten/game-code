using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    public float Time { get; private set; }
    private bool isRunning;
    
    public void StartStopwatch() {
        RefreshStopwatch();
        isRunning = true;
    }

    public void StopStopwatch() {
        isRunning = false;
    }

    public void RefreshStopwatch()
    {
        Time = 0f;
    }

    private void Update() {
        if (isRunning) {
            Time += UnityEngine.Time.deltaTime;
        }
    }
}
