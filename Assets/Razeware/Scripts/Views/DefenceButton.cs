using System.Collections;
using System.Collections.Generic;
using Defendable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    public Defence Defence { get; private set; }
    public GameGrid Grid { get; private set; }

    private void Start()
    {
        button.onClick.AddListener(SelectDefence);
    }

    private void SelectDefence()
    {
        var match = Grid.GridList.FindAll(Defence.ConditionToPlace).FindAll(ConditionsData.IsEmptyCell);
        match.ForEach(x => x.SetSelected());
    }
}
