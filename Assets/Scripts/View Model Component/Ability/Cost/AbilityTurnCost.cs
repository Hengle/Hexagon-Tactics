using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTurnCost : BaseAbilityCost {

    protected override void OnCanPerformCheck(object sender, object args) {
        
    }

    protected override void OnDidPerformNotification(object sender, object args) {
        this.PostNotification(Ability.TurnConsumedNotification);
    }

}
