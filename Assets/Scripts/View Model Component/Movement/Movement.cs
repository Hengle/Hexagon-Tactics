using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour {

    public const string CanMoveCheck = "Movement.CanMoveCheck";
    public const string DidMoveNotification = "Movement.DidMoveNotification";

    protected Animation anim;
    protected Stats stats;
    protected int moveCounter { get { return stats[StatTypes.MoveCounter]; } set { stats[StatTypes.MoveCounter] = value; } }
    public int Range { get { return stats[StatTypes.Move]; } }
    protected Unit unit;

    public bool CanMove(bool hasUnitMoved) {
        BaseException exc = new BaseException(true);
        this.PostNotification(CanMoveCheck, exc);
        return exc.toggle;
    }

    protected virtual void Awake() {
        unit = GetComponent<Unit>();
        
    }

    protected virtual void Start() {
        stats = GetComponent<Stats>();
        anim = GetComponent<Animation>();
    }

    private void OnEnable() {
        this.AddObserver(OnTurnBeganNotification, TurnOrderController.TurnBeganNotification, unit);
    }

    private void OnDisable() {
        this.RemoveObserver(OnTurnBeganNotification, TurnOrderController.TurnBeganNotification, unit);
    }

    protected virtual void OnTurnBeganNotification(object sender, object args) {
        moveCounter = 0;
    }


    public abstract IEnumerator Traverse(HexCell cell);

    public virtual List<HexCell> GetCellsInRange (HexGrid grid) {
        return grid.SearchInRange(unit.Cell, Range - moveCounter, true);
    }

    public virtual List<HexCell> PathTo(HexGrid grid, HexCell cell, bool hasUnitMoved) {
        if (CanMove(hasUnitMoved)) {
            List<HexCell> path = grid.FindPath(unit.Cell, cell);
            return path;
        }
        return null;
    }

    public virtual IEnumerator Turn (HexDirection dir) {
        anim.Walking = false;
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.RotateToLocal(dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);
       
        // When rotating between North and West, we must make an exception so it looks like the unit
        // rotates the most efficient way (since 0 and 360 are treated the same)
        if (Mathf.Approximately(t.startTweenValue.y, 0f) && Mathf.Approximately(t.endTweenValue.y, 270f))
            t.startTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
        else if (Mathf.Approximately(t.startTweenValue.y, 270) && Mathf.Approximately(t.endTweenValue.y, 0))
            t.endTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);

        unit.direction = dir;

        while (t != null)
            yield return null;
    }
}
