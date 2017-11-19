using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardSelection : MonoBehaviour
{
    private int currentSelected;

    [SerializeField]
    private Image[] cardImages;

    public void LightUpNext()
    {
        if (currentSelected < cardImages.Length)
        {
            cardImages[currentSelected++].color = Color.green;
        }
    }

    public void ClearAll()
    {
        foreach (var image in cardImages)
        {
            image.color = Color.white;
        }
        currentSelected = 0;
    }
}
