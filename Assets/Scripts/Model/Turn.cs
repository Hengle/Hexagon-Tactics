using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn {

    public Unit actor;
    public int moveCounter;
    public bool hasUnitMoved;
    public bool hasUnitActed;
    public bool lockMove;
    public Ability ability;
    public List<HexCell> targets;

    public void Change(Unit current) {
        actor = current;
        moveCounter = 0;
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
    }
}
