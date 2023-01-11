using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class GoldView : MonoBehaviour
{
    private GoldViewModel GoldViewModel { get; set; }

    [SerializeField] TextMeshProUGUI goldText;

    [Inject]
    private void Construct(GoldViewModel goldViewModel)
    {
        GoldViewModel = goldViewModel;
    }

    private void Start()
    {
        GoldViewModel.OnGoldChanged += UpdateText;
    }

    private void UpdateText(int value)
    {
        goldText.text = value.ToString();
    }

    private void OnDestroy()
    {
        GoldViewModel.OnGoldChanged -= UpdateText;
    }
}
