using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeModel : MonoBehaviour
{
    public bool IsPaused = Time.timeScale == 0;
    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Resume()
    {
        Time.timeScale = 1;
    }
}
