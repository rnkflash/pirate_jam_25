using Common;
using UnityEngine;

public class SFX_Heal : CMSEntity
{
    public SFX_Heal()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/heal".Load<AudioClip>());
    }
}