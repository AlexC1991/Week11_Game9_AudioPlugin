using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexzanderCowell
{
public class PlayerBlockMovement : MonoBehaviour
{
   private float horizontalMovement;
   public float horizontalMovementSpeed = 5f; // Adjust this speed as needed
   public ScriptableAudioFile bounceSound;

      private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            bounceSound.PlayAudio();
        }
    }


    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        // Move the object based on horizontal input
        if (horizontalMovement != 0)
        {
            Vector3 movement = new Vector3(horizontalMovement, 0, 0);
            transform.Translate(movement * horizontalMovementSpeed * Time.deltaTime);
        }
    }
}
}
