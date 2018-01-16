using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AbilityMenuPanelController : MonoBehaviour 
{
	#region Constants
	const string ShowKey = "Show";
	const string HideKey = "Hide";
	const string EntryPoolKey = "AbilityMenuPanel.Entry";
	const int MenuCount = 10;
	#endregion

	#region Fields / Properties
	[SerializeField] GameObject entryPrefab;
	[SerializeField] Panel panel;
	[SerializeField] GameObject canvas;
    [SerializeField] GameObject spellContainer;
    List<AbilityBarSlot> barEntries = new List<AbilityBarSlot>(MenuCount);
    public int selection { get; private set; }
	#endregion

	#region MonoBehaviour
	void Awake ()
	{
		GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
        selection = -1;
	}

	void Start ()
	{
		panel.SetPosition(HideKey, false);
		canvas.SetActive(false);
	}
	#endregion

	#region Public
	public void Show (List<Ability> abilities)
	{
		canvas.SetActive(true);
		Clear ();

        for(int i = 0; i < abilities.Count; i++) {
            AbilityBarSlot entry = Dequeue();
            entry.abilityImage.sprite = abilities[i].Icon;
            entry.abilityNumber = i;
            entry.abilityText.text = string.Format("[{0}]", i+1);
            barEntries.Add(entry);
        }
		TogglePos(ShowKey);
	}

	public void Hide ()
	{
		Tweener t = TogglePos(HideKey);
		t.completedEvent += delegate(object sender, System.EventArgs e)
		{
			if (panel.CurrentPosition == panel[HideKey])
			{
				Clear();
				canvas.SetActive(false);
			}
		};
	}

	public void SetLocked (int index, bool value)
	{
		if (index < 0 || index >= barEntries.Count)
			return;

        barEntries[index].IsLocked = value;
		if (value && selection == index)
			Next();
	}

	public void Next ()
	{
		for (int i = selection + 1; i < selection + barEntries.Count; ++i)
		{
			int index = i % barEntries.Count;
			if (SetSelection(index))
				break;
		}
	}

	public void Previous ()
	{
		for (int i = selection - 1 + barEntries.Count; i > selection; --i)
		{
			int index = i % barEntries.Count;
			if (SetSelection(index))
				break;
		}
	}
	#endregion

	#region Private
	AbilityBarSlot Dequeue ()
	{
		Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        AbilityBarSlot entry = p.GetComponent<AbilityBarSlot>();
		entry.transform.SetParent(spellContainer.transform, false);
		entry.transform.localScale = Vector3.one;
		entry.gameObject.SetActive(true);
		entry.Reset();
		return entry;
	}

	void Enqueue (AbilityBarSlot entry)
	{
		Poolable p = entry.GetComponent<Poolable>();
		GameObjectPoolController.Enqueue(p);
	}

	void Clear ()
	{
		for (int i = barEntries.Count - 1; i >= 0; --i)
			Enqueue(barEntries[i]);
        barEntries.Clear();
	}

	public bool SetSelection (int value) {
		if (barEntries[value].IsLocked)
			return false;
		
		// Deselect the previously selected entry
		if (selection >= 0 && selection < barEntries.Count) {
            barEntries[selection].IsSelected = false;              
        }

        if (selection == value)
            selection = -1;
        else
            selection = value;

        if (selection >= 0 && selection < barEntries.Count)
            barEntries[selection].IsSelected = true;
        return true;
	}

    public void Clearselection() {
        selection = -1;
    }

	Tweener TogglePos (string pos)
	{
		Tweener t = panel.SetPosition(pos, true);
		t.duration = 0.5f;
		t.equation = EasingEquations.EaseOutQuad;
		return t;
	}
	#endregion
}
