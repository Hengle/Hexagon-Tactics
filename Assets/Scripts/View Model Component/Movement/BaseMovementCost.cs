using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovementCost : MonoBehaviour {

    Movement owner;

    void Awake() {
        owner = GetComponent<Movement>();
    }

    void OnEnable() {
        this.AddObserver(OnCanMoveCheck, Movement.CanMoveCheck, owner);
        this.AddObserver(OnDidMoveNotification, Movement.DidMoveNotification, owner);
    }

    void OnDisable() {
        this.RemoveObserver(OnCanMoveCheck, Movement.CanMoveCheck, owner);
        this.RemoveObserver(OnDidMoveNotification, Movement.DidMoveNotification, owner);
    }

    protected abstract void OnCanMoveCheck(object sender, object args);

    protected abstract void OnDidMoveNotification(object sender, object args);
}
