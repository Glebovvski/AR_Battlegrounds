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
    [SerializeField] private TextMeshProUGUI price;

    private DefencesViewModel DefencesViewModel { get; set; }

    public void Init(ScriptableDefence info, DefencesViewModel defensesViewModel)
    {
        DefencesViewModel = defensesViewModel;
        text.text = info.Type.ToString();
        price.text = info.Price.ToString();
        button.onClick.AddListener(delegate{SelectDefence(info.PoolType);});
    }

    private void SelectDefence(PoolObjectType type) => DefencesViewModel.DefenseSelected(type);

}
