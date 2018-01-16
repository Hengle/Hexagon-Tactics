using UnityEngine;
using System.Collections;

public abstract class BaseAbilityCost : MonoBehaviour {
    #region Fields
    Ability owner;
    #endregion

    #region MonoBehaviour
    void Awake() {
        owner = GetComponent<Ability>();
    }

    void OnEnable() {
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }

    void OnDisable() {
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }
    #endregion

    #region Notification Handlers
    protected abstract void OnCanPerformCheck(object sender, object args);

    protected abstract void OnDidPerformNotification(object sender, object args);
    #endregion
}