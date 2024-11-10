using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingScene : MonoBehaviour
{
    GameObject sticker;
    [SerializeField] GameObject stickerLocation;
    [SerializeField] TMP_Text stickerTitle;
    // Start is called before the first frame update
    void Start()
    {   
        if(!GlobalVariables.responseIsFailure)
        {
            sticker = GlobalVariables.responseSticker;
            Instantiate(sticker, stickerLocation.transform);
            stickerTitle.text = sticker.GetComponent<Sticker>().stickerTitle;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
