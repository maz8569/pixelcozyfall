using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueObjectBoxA;
    [SerializeField] private GameObject dialogueObjectBoxB;
    [SerializeField] private TMP_Text textLabelA;
    [SerializeField] private TMP_Text textLabelB;
    [SerializeField] private DialogueObject testDialogue;

    private TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        //GetComponent<TypeWriterEffect>().Run("Hello!\nThis is my second line", textLabelA);
        //GetComponent<TypeWriterEffect>().Run("Hello!\nThis is my second line too", textLabelB);

        CloseDialogueBox(dialogueObjectBoxA, textLabelA);
        CloseDialogueBox(dialogueObjectBoxB, textLabelB);
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        ShowDialogue(testDialogue);

    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueObjectBoxA.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return typeWriterEffect.Run(dialogue, textLabelA);
            //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        CloseDialogueBox(dialogueObjectBoxA, textLabelA);
    }

    private void CloseDialogueBox(GameObject dialogueBox, TMP_Text textLabel)
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
