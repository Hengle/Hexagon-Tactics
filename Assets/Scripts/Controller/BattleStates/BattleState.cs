using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BattleState : State 
{
	protected BattleController owner;
	public HexGrid Grid { get { return owner.hexGrid; }}
    public StatPanelController statPanelController { get { return owner.statPanelController; } }
    public AbilityMenuPanelController abilityMenuPanelController { get { return owner.abilityMenuPanelController; } }
    public Turn turn { get { return owner.turn; } }
    public List<Unit> units { get { return owner.units; } }

    public HexCell currentCell {
        get { return owner.currentCell; }
        set {
            if (owner.currentCell != value)
                owner.currentCell = value;
        }
    }

	protected virtual void Awake ()
	{
		owner = GetComponent<BattleController>();
	}

    protected override void AddListeners() {
        InputController.CellSelectionEvent += OnCellSelected;
        InputController.CellClickEvent += OnCellClick;
        InputController.CancelEvent += OnCancel;
        InputController.TurnPassEvent += OnTurnPass;

    }

    protected override void RemoveListeners ()
	{
        InputController.CellSelectionEvent -= OnCellSelected;
        InputController.CellClickEvent -= OnCellClick;
        InputController.CancelEvent -= OnCancel;
        InputController.TurnPassEvent -= OnTurnPass;
    }

	public override void Enter ()
	{
		base.Enter ();
	}

	protected virtual void OnCellSelected (object sender, InfoEventArgs<HexCell> e)
	{
		
	}

    protected virtual void OnCellClick(object sender, InfoEventArgs<HexCell> e) {

    }

    protected virtual void OnCancel(object sender, InfoEventArgs<int> e) {

    }

    protected virtual void OnTurnPass(object sender, InfoEventArgs<int> e) {

    }

    protected virtual Unit GetUnit (HexCell cell)
	{
		GameObject content = cell != null ? cell.Content : null;
		return content != null ? content.GetComponent<Unit>() : null;
	}

	protected virtual void RefreshPrimaryStatPanel (HexCell cell)
	{
        Unit target = GetUnit(cell);
		if (target != null)
			statPanelController.ShowPrimary(target.gameObject);
		else
			statPanelController.HidePrimary();
	}

	protected virtual void RefreshSecondaryStatPanel (HexCell cell)
	{
        Unit target = GetUnit(cell);
        if (target != null)
			statPanelController.ShowSecondary(target.gameObject);
		else
			statPanelController.HideSecondary();
	}
     /*
	protected virtual bool DidPlayerWin ()
	{
		return owner.GetComponent<BaseVictoryCondition>().Victor == Alliances.Hero;
	}
	
	protected virtual bool IsBattleOver ()
	{
		return owner.GetComponent<BaseVictoryCondition>().Victor != Alliances.None;
	}
    */
}