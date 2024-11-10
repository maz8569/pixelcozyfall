using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBoxNPC;
    [SerializeField] private GameObject dialogueBoxPlayer;
    [SerializeField] private TMP_Text textLabelA;
    [SerializeField] private TMP_Text textLabelB;
    [SerializeField] private DialogueObject startDialogue;

    public bool isOpen {get; private set;}

    private TypeWriterEffect typeWriterEffect;
    public ResponseHandler responseHandler;

    private void Start()
    {
        //GetComponent<TypeWriterEffect>().Run("Hello!\nThis is my second line", textLabelA);
        //GetComponent<TypeWriterEffect>().Run("Hello!\nThis is my second line too", textLabelB);

        CloseDialogueBox(dialogueBoxNPC, textLabelA);
        CloseDialogueBox(dialogueBoxPlayer, textLabelB);
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        if(GlobalVariables.dialogueIndex != 0)
        {
            startDialogue = GlobalVariables.currentDialogueObject;
        }
        ShowDialogue(startDialogue);

    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        dialogueBoxNPC.SetActive(true);
        dialogueBoxPlayer.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject, textLabelA));
    }

    public void ShowResponse(Response response)
    {
        StartCoroutine(StepThroughResponse(response, textLabelB));
    }

    public void HoverStart(string text)
    {
        //dialogueBoxPlayer.SetActive(true);
        textLabelB.text = text;
        Image image = dialogueBoxPlayer.transform.GetChild(0).GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = 0.5f;
        image.color = tempColor;
        dialogueBoxPlayer.transform.GetChild(1).GetComponent<TMP_Text>().color = new Color(0, 0, 0, 0.5f);
    }

    public void HoverEnd()
    {
        //dialogueBoxPlayer.SetActive(false);
        textLabelB.text = string.Empty;
        Image image = dialogueBoxPlayer.transform.GetChild(0).GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = 1.0f;
        image.color = tempColor;
        dialogueBoxPlayer.transform.GetChild(1).GetComponent<TMP_Text>().color = new Color(0, 0, 0, 1.0f);
    }
    

    public IEnumerator StepThroughDialogue(DialogueObject dialogueObject, TMP_Text textLabel)
    {
        for(int i = 0; i < dialogueObject.Dialogue.Length; ++i)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typeWriterEffect.Run(dialogue, textLabel);
            

            if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
            {
                break;
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);

        }
        else
        {
            CloseDialogueBox(dialogueBoxNPC, textLabel);
        }

        
    }

    public IEnumerator StepThroughResponse(Response response, TMP_Text textLabel)
    {

        yield return typeWriterEffect.Run(response.ResponseText, textLabel);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));


        ChangeLevel(response);
        //CloseDialogueBox(dialogueBoxPlayer, textLabel);
        //ShowDialogue(response.DialogueObject);
        
    }

    private void ChangeLevel(Response response)
    {
        GlobalVariables.dialogueIndex += 1;
        GlobalVariables.currentDialogueObject = response.DialogueObject;
        Debug.Log(response.nextLevel);
        SceneManager.LoadScene(response.nextLevel);
    }

    private void CloseDialogueBox(GameObject dialogueBox, TMP_Text textLabel)
    {
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
