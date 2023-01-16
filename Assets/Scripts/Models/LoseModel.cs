using System;
using Zenject;

public class LoseModel
{
    private GameGrid Grid { get; set; }

    [Inject]
    private void Construct(GameGrid grid)
    {
        Grid = grid;
    }
    
    public event Action OnRestart;
    public void Restart()
    {
        OnRestart?.Invoke();
        Grid.CreateGrid();
    }
}
