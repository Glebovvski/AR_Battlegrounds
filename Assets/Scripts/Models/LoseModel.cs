using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;
using Zenject;

public class LoseModel : MonoBehaviour
{
    public event Action OnRestart;

    public void Restart() => OnRestart?.Invoke();
}
