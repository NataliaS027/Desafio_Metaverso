using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationObject : MonoBehaviour
{
    public List<Sprite> ListAnimation;
    [SerializeField]private float duration;
    private Image image;
    private int index = 0;
    private string lenght;
    private float timer = 0;

    private void Awake() => image = GetComponent<Image>();

    private void Update()
    {

        if ((timer += Time.deltaTime) >= (duration / ListAnimation.Capacity))
        {
            timer = 0;
            image.sprite = ListAnimation[index];
            index = (index + 1) % ListAnimation.Capacity;

            if (index == ListAnimation.Count)
            {
                index = 0;
            }
        }
    }
}
