using UnityEngine;
using System.Collections;

public class MoveSequenceState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		StartCoroutine("Sequence");
	}
	
	IEnumerator Sequence ()
	{
		Movement m = turn.actor.GetComponent<Movement>();
		yield return StartCoroutine(m.Traverse(owner.currentCell));
		turn.hasUnitMoved = true;
        m.PostNotification(Movement.DidMoveNotification);
        owner.ChangeState<ActionSelectionState>();
    }
}
