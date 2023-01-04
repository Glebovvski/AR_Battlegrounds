using System.Collections;
using System.Collections.Generic;
using Defendable;
using UnityEngine;

public class DefenseViewModel
{
    public Defense Defence {get;private set;}
    public GameGrid Grid {get; private set;}

    public DefenseViewModel(GameGrid grid, Defense defence)
    {
        Grid = grid;
        Defence = defence;
    }
}
