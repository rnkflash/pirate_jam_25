using Common;
using UnityEngine;

public class SFX_AddBuff : CMSEntity
{
    public SFX_AddBuff()
    {
        Define<SFXTag>();
        Define<SFXArray>().files.Add("audio/add_buff".Load<AudioClip>());
    }
}