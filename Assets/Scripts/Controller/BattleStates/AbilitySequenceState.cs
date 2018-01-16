using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySequenceState : BattleState {
    protected Animation anim;
    private bool shouldEnd;

    public override void Enter() {
        base.Enter();
        shouldEnd = false;
        this.AddObserver(TurnConsumed, Ability.TurnConsumedNotification);
        anim = turn.actor.GetComponent<Animation>();
        StartCoroutine(Animate());
    }

    public override void Exit() {
        this.RemoveObserver(TurnConsumed, Ability.TurnConsumedNotification);
    }

    IEnumerator Animate() {
        Movement m = turn.actor.GetComponent<Movement>();
        HexDirection dir = turn.actor.Cell.GetDirection(currentCell);
        yield return StartCoroutine(m.Turn(dir));

        anim.Attack();

        yield return null;

        ApplyAbility();
        if (shouldEnd) {
            owner.ChangeState<SelectUnitState>();
        } else {
            owner.ChangeState<ActionSelectionState>();
        }
    }

    void ApplyAbility() {
        turn.ability.Perform(turn.targets);
    }

    void TurnConsumed(object sender, object args) {
        shouldEnd = true;
    }
}
