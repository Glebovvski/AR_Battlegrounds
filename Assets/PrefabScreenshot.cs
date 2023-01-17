using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PrefabScreenshot : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs = new List<GameObject>();
    [SerializeField] List<Image> sprites = new List<Image>();

    public void Shot()
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            var tex = PrefabUtility.GetIconForGameObject(prefabs[i]);
            sprites[i].sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            // File.WriteAllBytes(@"D:\", tex.EncodeToPNG());
        }
    }
}
