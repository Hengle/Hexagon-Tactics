using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkMovement : Movement {

    public override IEnumerator Traverse(HexCell cell) {
        unit.Place(cell);
        List<HexCell> targets = new List<HexCell>();
        while (cell != null) {
            targets.Insert(0, cell);
            cell = cell.PathFrom;
        }

        for (int i = 1; i < targets.Count; ++i) {
            HexCell from = targets[i - 1];
            HexCell to = targets[i];

            HexDirection dir = from.GetDirection(to);
            
            if (unit.direction != dir)
                yield return StartCoroutine(Turn(dir));

           // if (from.height == to.height)
                yield return StartCoroutine(Walk(to));
           // else
             //   yield return StartCoroutine(Jump(to));
        }

        moveCounter += targets.Count - 1;

        anim.Walking = false;
        yield return null;
    }

    IEnumerator Walk(HexCell target) {
        anim.Walking = true;
        Tweener tweener = transform.MoveTo(target.Position, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }
}