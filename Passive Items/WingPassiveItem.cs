using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
   
}
