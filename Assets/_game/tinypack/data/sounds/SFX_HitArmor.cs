using Common;
using UnityEngine;

public class SFX_HitArmor : CMSEntity
{
    public SFX_HitArmor()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/armor_hit".Load<AudioClip>());
    }
}