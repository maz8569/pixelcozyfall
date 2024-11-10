using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerAlbum : MonoBehaviour
{
    [SerializeField] GameObject[] stickers;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject sticker in stickers)
        {
            if(!GlobalVariables.listLocked[sticker.GetComponent<Sticker>().stickerIndex])
            {
                sticker.GetComponent<Sticker>().unlock();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
