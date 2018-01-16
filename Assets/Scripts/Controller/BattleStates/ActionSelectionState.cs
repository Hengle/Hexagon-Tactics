using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionSelectionState : BattleState
{
    List<HexCell> cells;
    Movement mover;
    AbilityList abilityList;

    public override void Enter() {
        base.Enter();
        statPanelController.ShowPrimary(turn.actor.gameObject);
        LoadAbilities();
        mover = turn.actor.GetComponent<Movement>();
        cells = mover.GetCellsInRange(Grid);

        turn.actor.setOutline(true);

        abilityMenuPanelController.Clearselection();
        InputController.AbilitySelectedEvent += OnAbilitySelected;
    }

    public override void Exit() {
        base.Exit();

        turn.actor.setOutline(false);

        statPanelController.HidePrimary();
        abilityMenuPanelController.Hide();
        cells = null;
        Grid.UnHighlightAllCells();
        InputController.AbilitySelectedEvent -= OnAbilitySelected;
    }

    protected void LoadAbilities() {
        abilityList = turn.actor.GetComponentInChildren<AbilityList>();
        List<Ability> abilities = abilityList.getAbilities();
        abilityMenuPanelController.Show(abilities);
    }

    protected override void OnCellSelected(object sender, InfoEventArgs<HexCell> e) {
        Grid.UnHighlightAllCells();
        if (e.info == null) return;
        currentCell = e.info;
        RefreshSecondaryStatPanel(e.info);
        if (cells.Contains(e.info)) {
            List<HexCell> path = mover.PathTo(Grid, e.info, turn.hasUnitMoved);
            if (path != null)
                Grid.HighlightCells(path, Color.green);
        }
    }

    protected override void OnCellClick(object sender, InfoEventArgs<HexCell> e) {
        if (cells.Contains(e.info) && e.info != turn.actor.Cell) {
            currentCell = e.info;
            owner.ChangeState<MoveSequenceState>();
        }
    }

    protected override void OnTurnPass(object sender, InfoEventArgs<int> e) {
        owner.ChangeState<SelectUnitState>();
    }

    protected void OnAbilitySelected(object sender, InfoEventArgs<int> e) {
        abilityMenuPanelController.SetSelection(e.info);
        turn.ability = abilityList.GetAbility(abilityMenuPanelController.selection);
        owner.ChangeState<AbilityTargetState>();
    }

}