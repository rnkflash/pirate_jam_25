using Common;
using UnityEngine;

public class SFX_PoisonAttack : CMSEntity
{
    public SFX_PoisonAttack()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/poison_attack".Load<AudioClip>());
    }
}