using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMovementcost : BaseMovementCost {

    public int amount;

    protected override void OnCanMoveCheck(object sender, object args) {
        Stats s = GetComponentInParent<Stats>();
        if (s[StatTypes.MP] < amount) {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }

    protected override void OnDidMoveNotification(object sender, object args) {
        Stats s = GetComponentInParent<Stats>();
        s[StatTypes.MP] -= amount;
    }

}
