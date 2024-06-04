using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    public enum ParitcleName
    {
        bornSplit,
        run,
    }

    public ParticleSystem bornSplit;
    public ParticleSystem run;

    private void LateUpdate()
    {
        bornSplit.transform.rotation = Quaternion.identity;
        //run.transform.rotation = Quaternion.identity;
    }

    public void Play(ParitcleName name)
    {
        ParticleSystem particleSystem = name switch
        {
            ParitcleName.bornSplit => bornSplit,
            ParitcleName.run => run,
            _ => throw new System.NotImplementedException(),
        };
        particleSystem.Play();
    }

    private void OnEnable()
    {
        PlayerControl.OnBornSplit += Play;
    }

    private void OnDisable()
    {
        PlayerControl.OnBornSplit -= Play;
    }
}
