using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Don't used anymore.")]
public class SpinachPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMight *= 1 + passiveItemData.Multiplier / 100f;
    }

}
