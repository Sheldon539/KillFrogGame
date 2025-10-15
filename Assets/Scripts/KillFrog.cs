using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KillFrog : MonoBehaviour
{
    [SerializeField] GameObject smokeEffect;
    [SerializeField] AudioClip explodeSound;
    
    private Camera mainCamera;
    private Mouse mouse;

    void Start()
    {
        mainCamera = Camera.main;
        mouse = Mouse.current;
        
        // Ensure collider exists
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void Update()
    {
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(mouse.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                KillThisFrog();
            }
        }
    }

    void OnMouseDown()
    {
        KillThisFrog();
    }

    void KillThisFrog()
    {
        // PLAY SOUND using PlayClipAtPoint - this creates a temporary object so sound plays fully
        if (explodeSound != null)
        {
            AudioSource.PlayClipAtPoint(explodeSound, transform.position, 0.7f);
            Debug.Log("SOUND PLAYED IMMEDIATELY: " + explodeSound.name);
        }

        // Create smoke effect
        if (smokeEffect != null)
            Instantiate(smokeEffect, transform.position, Quaternion.identity);

        // Add score
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(1);

        // Notify spawner
        if (FrogSpawner.Instance != null)
            FrogSpawner.Instance.RemoveFrog(gameObject);

        // Destroy this frog
        Destroy(gameObject);
    }
}