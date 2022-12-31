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

    private DefencesViewModel DefencesViewModel { get; set; }

    public void Init(Defence defence, DefencesViewModel defensesViewModel)
    {
        DefencesViewModel = defensesViewModel;
        text.text = defence.gameObject.name;
        button.onClick.AddListener(delegate{SelectDefence(defence);});
    }

    private void SelectDefence(Defence defence) => DefencesViewModel.DefenseSelected(defence);

}
