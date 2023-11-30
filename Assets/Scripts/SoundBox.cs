using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    public ScriptableAudioFile boxSound;

private void Start()
{
   boxSound.PlayAudio();
}
}
