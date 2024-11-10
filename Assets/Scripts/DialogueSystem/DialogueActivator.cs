using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    //[SerializeField] private DialogueObject dialogueObject;
    public Response response;

    public void Interact(Player player)
    {
        player.DialogueUI.responseHandler.OnPickedResponse(response);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Debug.Log(response.ResponseText);
            player.Interactable = this;
            if (response.isFailure)
            {
                player.Interactable?.Interact(player);
            }
            else
            {
                player.DialogueUI.HoverStart(response.ResponseText);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            
            if(player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                Debug.Log("player exit");
                player.DialogueUI.HoverEnd();
                player.Interactable = null;
            }
        }
    }
}
