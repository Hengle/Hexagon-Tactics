using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public static event EventHandler<InfoEventArgs<HexCell>> CellSelectionEvent;
    public static event EventHandler<InfoEventArgs<HexCell>> CellClickEvent;
    public static event EventHandler<InfoEventArgs<int>> CancelEvent;
    public static event EventHandler<InfoEventArgs<int>> TurnPassEvent;
    public static event EventHandler<InfoEventArgs<int>> AbilitySelectedEvent;

    public HexGrid hexGrid;

    HexCell previousCell, currentCell;

    private void OnEnable() {
        AbilityBarSlot.barClick += OnAbilityInBarSelected;
    }

    private void OnDisable() {
        AbilityBarSlot.barClick -= OnAbilityInBarSelected;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetAxis("Pass") > 0.9) {
            if (TurnPassEvent != null) {
                TurnPassEvent(this, new InfoEventArgs<int>());
            }
        }

        for (int i = 0; i < 5; i ++) {
            string name = string.Format("Alpha{0}", i+1);
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), name);
            if (Input.GetKeyUp(key)) {
                if (AbilitySelectedEvent != null) {
                    AbilitySelectedEvent(this, new InfoEventArgs<int>(i));
                }
            }
        }

        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            currentCell = hexGrid.GetCell(hit.point);           
        } else {
            currentCell = null;
        }
        if (currentCell != previousCell) {
            if (CellSelectionEvent != null)             
                CellSelectionEvent(this, new InfoEventArgs<HexCell>(currentCell));
            previousCell = currentCell;
        }
        if (Input.GetMouseButtonUp(0) && currentCell != null) {
            if (CellClickEvent != null)
                CellClickEvent(this, new InfoEventArgs<HexCell>(currentCell));
        }
        if (Input.GetMouseButtonUp(1)) {
            if (CancelEvent != null) {
                CancelEvent(this, new InfoEventArgs<int>());
            }
        }
	}

    public void OnAbilityInBarSelected(object sender, InfoEventArgs<int> e) {
        Debug.Log("Ability Clicked: " + e.info);
        if (AbilitySelectedEvent != null) {
            AbilitySelectedEvent(this, new InfoEventArgs<int>(e.info));
            
        }          
    }
}
