using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugView : MonoBehaviour
{
    public static DebugView Instance;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void SetText(string value) => text.text = value;
}
