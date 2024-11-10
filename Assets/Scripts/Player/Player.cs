using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    //private const float moveSpeed = 10f;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    private Rigidbody2D rb;

    private Camera camera;

    bool failed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        camera = FindObjectOfType<Camera>();

    }

    private void Update()
    {
        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //rb.MovePosition(rb.position + input.normalized * (moveSpeed * Time.fixedDeltaTime));

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
        }

        if (!failed)
        {
            if (transform.position.y >= 0)
            {
                camera.gameObject.transform.position = transform.position + new Vector3(0, 0, -10);
            }
            else
            {
                camera.gameObject.transform.position = new Vector3(transform.position.x, camera.gameObject.transform.position.y, -10);
            }
        }

        if (transform.position.y < -8)
        {
            if (!failed)
            {
                DialogueUI.responseHandler.FailureResponse();
                failed = true;
            }
        }
    }
}
