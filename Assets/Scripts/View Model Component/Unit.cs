using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public HexCell Cell { get; protected set;  }

    private Material mat;
    public HexDirection direction;

    private void Start() {
        mat = GetComponentInChildren<Renderer>().material;
    }

    public void setOutline(bool b) {
        if (b) {
            mat.SetFloat("_OutlineWidth", 0.2f);
        }
        else {
            mat.SetFloat("_OutlineWidth", 0f);
        }
    }

    public void Place(HexCell target) {
        if (Cell != null && Cell.Content == gameObject) {
            Cell.Content = null;
        }
        Cell = target;
        if (target != null) {
            target.Content = gameObject;
        }
    }

    public void Match() {
        transform.localPosition = Cell.transform.position;
        transform.localEulerAngles = direction.ToEuler();
    }
}
