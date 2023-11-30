using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexzanderCowell
{
public class BallDeathBarrier : MonoBehaviour
{
    public static bool hitLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            hitLocation = true;
        }
    }
}
}
