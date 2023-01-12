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

    public void Init(ScriptableDefense info)
    {
        text.text = info.Type.ToString();
        price.text = info.Price.ToString();
        button.onClick.AddListener(SelectDefence);
    }

    public void UpdateButton(bool active) => button.interactable = active;

    private void SelectDefence() => DefenseViewModel.SelectDefence();

}
