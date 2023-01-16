using System;

public class LoseModel
{
    public event Action OnRestart;
    public void Restart() => OnRestart?.Invoke();
}
