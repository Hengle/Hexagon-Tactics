using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine {

    public HexGrid hexGrid;
    public Turn turn = new Turn();
    public HexCell currentCell { get; set; }
    public List<Unit> units = new List<Unit>();
    public StatPanelController statPanelController;
    public AbilityMenuPanelController abilityMenuPanelController;
    public IEnumerator round;

    private void Start() {
        ChangeState<InitBattleState>();
    }
}
