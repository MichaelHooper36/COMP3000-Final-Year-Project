using System;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public SpriteRenderer sprintRenderer;
    public PlayerMovement playerMovement;

    void Awake()
    {
        sprintRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            sprintRenderer.color = Color.black;
            playerMovement.grapplePoint = transform;
            playerMovement.canGrapple = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            playerMovement.grapplePoint = null;
            playerMovement.canGrapple = false;
            sprintRenderer.color = Color.white;
        }
    }
}
