using UnityEngine;
using System.Collections;

public class AbilityMagicCost : BaseAbilityCost {

    public int amount;

    protected override void OnCanPerformCheck(object sender, object args) {
        Stats s = GetComponentInParent<Stats>();
        if (s[StatTypes.MP] < amount) {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }

    protected override void OnDidPerformNotification(object sender, object args) {
        Stats s = GetComponentInParent<Stats>();
        s[StatTypes.MP] -= amount;
    }
}