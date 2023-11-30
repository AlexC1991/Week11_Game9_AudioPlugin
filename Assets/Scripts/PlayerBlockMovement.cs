using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexzanderCowell
{
public class PlayerBlockMovement : MonoBehaviour
{
    private float horizonalMovement;
    private float horizonalMovementSpeed;


    private void Update()
    {
       horizonalMovement = Input.GetAxis("Horizontal");


       if (horizonalMovement < 0)
       {
          transform.position = Vector3.left * Time.deltaTime;
       }
       else if (horizonalMovement > 0)
       {
         transform.position = Vector3.right * Time.deltaTime;
       }
    }
}
}
