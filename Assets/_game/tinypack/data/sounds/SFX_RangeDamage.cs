using Common;
using UnityEngine;

public class SFX_RangeDamage : CMSEntity
{
    public SFX_RangeDamage()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/arrow_hit".Load<AudioClip>());
    }
}