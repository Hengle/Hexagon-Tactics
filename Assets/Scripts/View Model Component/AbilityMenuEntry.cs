using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityMenuEntry : MonoBehaviour 
{
	#region Enums
	[System.Flags]
	enum States
	{
		None = 0,
		Selected = 1 << 0,
		Locked = 1 << 1
	}
    #endregion

    #region Properties
    public string title;
    public Text label;
    public int barIndex;
    public Sprite Icon {
        get { return image.sprite; }
        set { image.sprite = value; }
    }

	public bool IsLocked
	{
		get { return (State & States.Locked) != States.None; }
		set
		{
			if (value)
				State |= States.Locked;
			else
				State &= ~States.Locked;
		}
	}

	public bool IsSelected
	{
		get { return (State & States.Selected) != States.None; }
		set
		{
			if (value)
				State |= States.Selected;
			else
				State &= ~States.Selected;
		}
	}

	States State
	{ 
		get { return state; }
		set
		{
			if (state == value)
				return;
			state = value;
			
			if (IsLocked)
			{
				//image.color = disabledColor;
				outline.effectColor = new Color32(20, 36, 44, 255);
			}
			else if (IsSelected)
			{
				//image.color = selectedColor;
				outline.effectColor = new Color32(255, 160, 72, 255);
			}
			else
			{
				//image.color = normalColor;
				outline.effectColor = new Color32(20, 36, 44, 255);
			}
		}
	}
	States state;
	
	[SerializeField] Image image;
    [SerializeField] Color disabledColor;
    [SerializeField] Color selectedColor;
    Outline outline;
	#endregion
	
	#region MonoBehaviour
	void Awake ()
	{
		outline = image.GetComponent<Outline>();
	}
	#endregion

	#region Public
	public void Reset ()
	{
		State = States.None;
	}
	#endregion
}
