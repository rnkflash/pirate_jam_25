using Common;
using UnityEngine;

public class SFX_Armor : CMSEntity
{
    public SFX_Armor()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/armor".Load<AudioClip>());
    }
}