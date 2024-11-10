using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sticker : MonoBehaviour
{
    [SerializeField] private bool isLocked = true;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] public string stickerTitle;
    [SerializeField] public int stickerIndex;

    private void Start()
    {
        if(!isLocked)
        {
            unlock();
        }
        else if(isLocked)
        {
            lockSticker();
        }
    }

    public void unlock()
    {
        isLocked = false;
        GetComponent<Image>().sprite = unlockedSprite; 
    }

    public void lockSticker()
    {
        isLocked = true;
        GetComponent<Image>().sprite = lockedSprite; 
    }

    public void pointerEnter()
    {
        transform.localScale = new Vector2(1.2f, 1.2f);
    }

    public void pointerExit()
    {
        transform.localScale = new Vector2(1.0f, 1.0f);
    }
}
