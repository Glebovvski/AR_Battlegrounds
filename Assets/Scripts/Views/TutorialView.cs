using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(string value) => text.text = value;
}
