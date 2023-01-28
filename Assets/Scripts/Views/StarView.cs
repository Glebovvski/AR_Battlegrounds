using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite empty;
    [SerializeField] private Sprite filled;

    [SerializeField] private Animator animator;

    public void Activate()
    {
        animator.SetTrigger("Activate");
    }

    public void ChangeSprite()
    {
        image.sprite = filled;
    }

    public void Reset() => image.sprite = empty;
}
