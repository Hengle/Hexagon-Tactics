using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AbilityBarSlot : MonoBehaviour {

    public static event EventHandler<InfoEventArgs<int>> barClick;

    Button button;

    [System.Flags]
    enum States {
        None = 0,
        Selected = 1 << 0,
        Locked = 1 << 1
    }

    public int abilityNumber;
    public Text abilityText;
    public Image abilityImage;

    public Color disabledColor;
    public Color selectedColor;
    Color normalColor;

    public bool IsLocked {
        get { return (State & States.Locked) != States.None; }
        set {
            if (value)
                State |= States.Locked;
            else
                State &= ~States.Locked;
        }
    }

    public bool IsSelected {
        get { return (State & States.Selected) != States.None; }
        set {
            if (value)
                State |= States.Selected;
            else
                State &= ~States.Selected;
        }
    }

    States State {
        get { return state; }
        set {
            if (state == value)
                return;
            state = value;

            if (IsLocked) {
                abilityImage.color = disabledColor;
            }
            else if (IsSelected) {
                abilityImage.color = selectedColor;
            }
            else {
                abilityImage.color = normalColor;
            }
        }
    }
    States state;

    public void Start() {
        normalColor = abilityImage.color;
        button = GetComponent<Button>();
        button.onClick.AddListener(onButtonClick);
    }

    void onButtonClick() {
        if (barClick != null)
            barClick(this, new InfoEventArgs<int>(abilityNumber));
    }

    public void Reset() {
        State = States.None;
    }
}
