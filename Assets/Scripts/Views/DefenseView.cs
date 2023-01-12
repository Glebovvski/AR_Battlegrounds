using System.Collections;
using System.Collections.Generic;
using Defendable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DefenseView : MonoBehaviour
{
    private DefenseViewModel DefenseViewModel { get; set; }

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI price;

    [Inject]
    private void Construct(DefenseViewModel defenseViewModel)
    {
        DefenseViewModel = defenseViewModel;
    }

    public void Init(ScriptableDefense info)
    {
        text.text = info.Type.ToString();
        price.text = info.Price.ToString();
        button.onClick.AddListener(delegate { SelectDefence(info); });
    }

    public void UpdateButton(bool active) => button.interactable = active;

    private void SelectDefence(ScriptableDefense info) => DefenseViewModel.SelectDefence();

}
