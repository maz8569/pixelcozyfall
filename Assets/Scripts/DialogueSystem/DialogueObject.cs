using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private bool[] isPlayer;
    [SerializeField] private Response[] responses;
    //[SerializeField] public string nextLevel;

    public string[] Dialogue => dialogue;

    public bool HasResponses => responses != null && responses.Length > 0;

    public Response[] Responses => responses;
}
