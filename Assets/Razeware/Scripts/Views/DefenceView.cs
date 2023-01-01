using System.Collections;
using System.Collections.Generic;
using Defendable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private DefencesViewModel DefencesViewModel { get; set; }

    public void Init(PoolObjectType type, DefencesViewModel defensesViewModel)
    {
        DefencesViewModel = defensesViewModel;
        text.text = type.ToString();
        button.onClick.AddListener(delegate{SelectDefence(type);});
    }

    private void SelectDefence(PoolObjectType type) => DefencesViewModel.DefenseSelected(type);

}
