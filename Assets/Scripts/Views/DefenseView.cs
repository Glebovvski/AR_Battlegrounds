using System.Collections;
using System.Collections.Generic;
using Defendable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenseView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI price;

    private DefensesModel DefensesModel { get; set; }

    public void Init(ScriptableDefense info, DefensesModel defensesModel)
    {
        DefensesModel = defensesModel;
        text.text = info.Type.ToString();
        price.text = info.Price.ToString();
        button.onClick.AddListener(delegate{SelectDefence(info);});
    }

    public void UpdateButton(bool active) => button.interactable = active;

    private void SelectDefence(ScriptableDefense info) => DefensesModel.DefenseSelected(info);

}
