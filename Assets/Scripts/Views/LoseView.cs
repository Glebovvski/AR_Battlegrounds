using System;
using UnityEngine;

public class LoseView : MonoBehaviour
{
    public event Action OnTryAgainClick;

    public void TryAgain() => OnTryAgainClick?.Invoke();
}
