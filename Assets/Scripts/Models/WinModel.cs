using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinModel
{
    public event Action OnWin;

    //TODO: ADD STARS BASED ON RESULTS

    public void Win() => OnWin?.Invoke();
}
