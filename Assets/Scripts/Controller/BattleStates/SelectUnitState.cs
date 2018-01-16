using UnityEngine;
using System.Collections;

public class SelectUnitState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		StartCoroutine("ChangeCurrentUnit");
	}

	public override void Exit ()
	{
		base.Exit ();
		statPanelController.HidePrimary();
	}

	IEnumerator ChangeCurrentUnit ()
	{
        yield return new WaitForSeconds(0.6f);
		owner.round.MoveNext();
		RefreshPrimaryStatPanel(currentCell);
		yield return null;
		owner.ChangeState<ActionSelectionState>();
	}

}