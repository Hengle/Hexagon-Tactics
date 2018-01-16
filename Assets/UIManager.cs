using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public List<GameObject> Windows = new List<GameObject>();

    private int CurrentWindow = -1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeWindow(int id) {
        if (CurrentWindow == id)
            return;

        if (id != 2) {
            for (int i = 0; i < Windows.Count; i++) {
                Windows[i].SetActive(false);
            }

        }
        CurrentWindow = id;
        Windows[id].SetActive(true);
    }

    public void QuitApp() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
		    Application.Quit();
        #endif
    }
}
