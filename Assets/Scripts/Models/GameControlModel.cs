using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlModel
{
    public event Action OnRestart;
    public void Restart()
    {
        OnRestart?.Invoke();
    }
}
