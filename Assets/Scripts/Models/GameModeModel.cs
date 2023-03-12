using System;
using System.Collections;
using System.Collections.Generic;
using ARSupportCheck;
using UnityEngine;

public class GameModeModel
{
    public bool IsARSupported => ARSupportChecker.IsSupported();

    private bool _isARModeSelected = false;
    public bool IsARModeSelected => IsARSupported && _isARModeSelected;

    public event Action OnChangeMode;
    public bool ChangeMode()
    {
        _isARModeSelected = !_isARModeSelected;
        OnChangeMode?.Invoke();
        return IsARModeSelected;
    }
}
