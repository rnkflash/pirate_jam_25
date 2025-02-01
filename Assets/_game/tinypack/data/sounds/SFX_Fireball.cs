using Common;
using UnityEngine;

public class SFX_Fireball : CMSEntity
{
    public SFX_Fireball()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/fireball".Load<AudioClip>());
    }
}