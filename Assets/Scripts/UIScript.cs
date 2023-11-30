using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlexzanderCowell
{
public class UIScript : MonoBehaviour
{
    public ScriptableAudioFile backgroundSoundl;
    public Text showScore;
    
    private void Start()
    {
        backgroundSoundl.PlayAudio();
    }

    void Update()
    {
        showScore.text = " Total Score " + BallScript.scoreTotal.ToString();
    }


}
}
