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
        comboSplit,
    }

    public ParticleSystem bornSplit;
    public ParticleSystem comboSplit;

    private void LateUpdate()
    {
        bornSplit.transform.rotation = Quaternion.identity;
        comboSplit.transform.rotation = Quaternion.identity;
    }

    public void Play(ParitcleName name)
    {
        ParticleSystem particleSystem = name switch
        {
            ParitcleName.bornSplit => bornSplit,
            ParitcleName.comboSplit => comboSplit,
            _ => throw new System.NotImplementedException(),
        };
        particleSystem.Play();
    }

    private void OnEnable()
    {
        PlayerControl.OnBornSplit += Play;
        PlayerSplit.OnComboSplit += Play;
    }

    private void OnDisable()
    {
        PlayerControl.OnBornSplit -= Play;
        PlayerSplit.OnComboSplit -= Play;
    }
}
