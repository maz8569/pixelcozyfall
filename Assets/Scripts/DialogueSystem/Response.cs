using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] public string nextLevel;
    [SerializeField] public bool isFailure;

    public string ResponseText => responseText;

    public DialogueObject DialogueObject => dialogueObject;
}
