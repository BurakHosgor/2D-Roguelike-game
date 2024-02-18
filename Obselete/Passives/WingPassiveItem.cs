using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Don't used anymore")]
public class WingPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
   
}
