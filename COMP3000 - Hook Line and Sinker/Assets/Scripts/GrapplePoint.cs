using System;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public SpriteRenderer sprintRenderer;

    void Awake()
    {
        sprintRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            playerMovement.grapplePoint = null;
            playerMovement.canGrapple = false;
            sprintRenderer.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
            sprintRenderer.color = Color.black;
            playerMovement.grapplePoint = transform;
            playerMovement.canGrapple = true;
        }
    }
}
