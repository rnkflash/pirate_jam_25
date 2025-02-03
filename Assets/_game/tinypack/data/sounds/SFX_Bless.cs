using Common;
using UnityEngine;

public class SFX_Bless : CMSEntity
{
    public SFX_Bless()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/wololo".Load<AudioClip>());
    }
}