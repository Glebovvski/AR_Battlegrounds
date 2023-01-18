using System;
using System.Collections;
using System.Collections.Generic;
using Defendable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DefenseView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI price;

    public event Action OnDefenseSelected;

    public void Init(string Price, Sprite Image)
    {
        price.text = Price;
        image.sprite = Image;
        button.onClick.AddListener(SelectDefence);
    }

    public void UpdateButton(bool active) => button.interactable = active;

    private void SelectDefence()
    {
        OnDefenseSelected?.Invoke();
    }
}
