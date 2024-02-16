using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : Weapon
{
    float _randomPitch;
    public void Reload()
    {
        _randomPitch = Random.Range(.8f, 1.2f);
        _mySoundHelper.PlaySound(_reloadAudioClip, .3f, _randomPitch);
    }
}
