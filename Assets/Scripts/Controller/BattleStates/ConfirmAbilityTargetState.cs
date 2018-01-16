using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmAbilityTargetState : BattleState {

    List<HexCell> cells;
    AbilityArea area;


    public override void Enter() {
        base.Enter();
        area = turn.ability.GetComponent<AbilityArea>();
        cells = area.GetCellsInArea(Grid, currentCell);
    }

    void FindTargets() {
        turn.targets = new List<HexCell>();
        for (int i = 0; i < cells.Count; ++i)
            if (turn.ability.IsTarget(cells[i]))
                turn.targets.Add(cells[i]);
    }
}
