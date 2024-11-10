using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static DialogueObject currentDialogueObject;
    public static int dialogueIndex = 0;

    public static int responseStickerIndex = 0;
    public static GameObject responseSticker;
    public static bool responseIsFailure = false;

    public static List<bool> listLocked = new List<bool>{true, true, true, true, true, true, true, true, true, true, true, true, true};

    public static void changeLocked(int index, bool result)
    {
        listLocked[index] = result;
    }
}
