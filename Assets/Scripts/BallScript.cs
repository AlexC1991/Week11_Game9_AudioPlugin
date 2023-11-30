using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexzanderCowell
{
public class BallScript : MonoBehaviour
{
    private bool hitDestroyedBlock;
    public static int scoreTotal;
    private Vector3 ballStartPosition;
    public ScriptableAudioFile destroySound;
    
    private void Start()
    {
        ballStartPosition = transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockD"))
        {
            destroySound.PlayAudio();
            hitDestroyedBlock = true;
        }
    }

    private void Update()
    {
        Debug.Log(hitDestroyedBlock);

        if (hitDestroyedBlock)
        {
            scoreTotal ++;
            hitDestroyedBlock = false;
        }

        if (BallDeathBarrier.hitLocation)
        {
            transform.position = ballStartPosition;
            BallDeathBarrier.hitLocation = false;
        }
    
    }
}
}
