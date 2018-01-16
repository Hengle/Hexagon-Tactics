using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState {

    List<HexCell> rangeCells, areaCells;
    AbilityRange range;
    AbilityArea area;

    public override void Enter() {
        base.Enter();
        range = turn.ability.GetComponent<AbilityRange>();
        area = turn.ability.GetComponent<AbilityArea>();
        SelectCells();
        statPanelController.ShowPrimary(turn.actor.gameObject);

        if (range.directionOriented)
            RefreshSecondaryStatPanel(currentCell);
    }

    public override void Exit() {
        base.Exit();
        Grid.UnHighlightCells(rangeCells);
        statPanelController.HidePrimary();
        statPanelController.HideSecondary();
    }

    void SelectCells() {
        rangeCells = range.GetCellsInRange(Grid);
        areaCells = area.GetCellsInArea(Grid, currentCell);
        Grid.HighlightCells(rangeCells, Color.blue);
        if (currentCell != null && rangeCells.Contains(currentCell)) {
            Grid.HighlightCells(areaCells, Color.red);
        }
    }

    void FindTargets() {
        turn.targets = ListPool<HexCell>.Get();
        foreach( HexCell c in areaCells) {
            if (turn.ability.IsTarget(c))
                turn.targets.Add(c);
        }
    }


    protected override void OnCellSelected(object sender, InfoEventArgs<HexCell> e) {
        if (range.directionOriented) {
            //ChangeDirection(e.info);
        }
        else {
            Grid.UnHighlightAllCells();
            Grid.HighlightCells(rangeCells, Color.blue);
            currentCell = e.info;
            if (currentCell != null && rangeCells.Contains(currentCell)) {
                ListPool<HexCell>.Add(areaCells);
                areaCells = area.GetCellsInArea(Grid, currentCell);
                Grid.HighlightCells(areaCells, Color.red);
            }
            RefreshSecondaryStatPanel(currentCell);
        }
    }

    protected override void OnCellClick(object sender, InfoEventArgs<HexCell> e) {
        if (e.info != null && rangeCells.Contains(e.info)) {
            FindTargets();
            //if (turn.targets.Count > 0)
                owner.ChangeState<AbilitySequenceState>();
        }
    }

    protected override void OnCancel(object sender, InfoEventArgs<int> e) {
        owner.ChangeState<ActionSelectionState>();
    }

}
