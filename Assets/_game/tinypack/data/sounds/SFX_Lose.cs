using Common;
using UnityEngine;

public class SFX_Lose : CMSEntity
{
    public SFX_Lose()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/lose".Load<AudioClip>());
    }
}