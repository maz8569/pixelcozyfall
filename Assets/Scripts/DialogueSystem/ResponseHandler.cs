using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    [SerializeField] private GameObject responseObjectTemplate;

    [SerializeField] private GameObject[] responseLocations;

    private DialogueUI dialogueUI;

    List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }

    public void ShowResponses(Response[] responses)
    {
        float ResponseBoxHeight = 0;
        int index = 0;
        foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseObjectTemplate, responseLocations[index].transform);
            responseButton.GetComponent<DialogueActivator>().response = response;
            responseButton.SetActive(true);
            //responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            //responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));

            tempResponseButtons.Add(responseButton);

            //ResponseBoxHeight += responseButtonTemplate.sizeDelta.y;
            index++;
        }

        //responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, ResponseBoxHeight);
        //responseBox.gameObject.SetActive(true);
    }

    public void HoverOnResponse(Response response)
    {
        //dialogueUI.
    }

    public void OnPickedResponse(Response response)
    {
        //responseBox.gameObject.SetActive(false);

        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();
        
        dialogueUI.ShowResponse(response);
        //dialogueUI.ShowDialogue(response.DialogueObject);
    }
}
