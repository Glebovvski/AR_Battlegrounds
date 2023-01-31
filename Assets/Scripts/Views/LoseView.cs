using System;
using UnityEngine;

public class LoseView : MonoBehaviour
{
    public event Action OnTryAgainClick;
    public event Action OnMenuClick;

    public void TryAgain() => OnTryAgainClick?.Invoke();
    public void Menu() => OnMenuClick?.Invoke();

    public void Open() => this.gameObject.SetActive(true);
    public void Close() => this.gameObject.SetActive(false);
}
