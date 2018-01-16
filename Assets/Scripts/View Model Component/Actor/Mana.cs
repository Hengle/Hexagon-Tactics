using UnityEngine;
using System.Collections;

public class Mana : MonoBehaviour 
{
	#region Fields
	public int MP
	{
		get { return stats[StatTypes.MP]; }
		set { stats[StatTypes.MP] = value; }
	}
	
	public int MPMax
	{
		get { return stats[StatTypes.MPMax]; }
		set { stats[StatTypes.MPMax] = value; }
	}

	Unit unit;
	Stats stats;
	#endregion
	
	#region MonoBehaviour
	void Awake ()
	{
		stats = GetComponent<Stats>();
		unit = GetComponent<Unit>();
	}
	
	void OnEnable ()
	{
		this.AddObserver(OnMPWillChange, Stats.WillChangeNotification(StatTypes.MP), stats);
		this.AddObserver(OnMPMaxDidChange, Stats.DidChangeNotification(StatTypes.MPMax), stats);
		this.AddObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
	}
	
	void OnDisable ()
	{
		this.RemoveObserver(OnMPWillChange, Stats.WillChangeNotification(StatTypes.MP), stats);
		this.RemoveObserver(OnMPMaxDidChange, Stats.DidChangeNotification(StatTypes.MPMax), stats);
		this.RemoveObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
	}
	#endregion
	
	#region Event Handlers
	void OnMPWillChange (object sender, object args)
	{
		ValueChangeException vce = args as ValueChangeException;
		vce.AddModifier(new ClampValueModifier(int.MaxValue, 0, stats[StatTypes.MPMax]));
	}
	
	void OnMPMaxDidChange(object sender, object args)
	{
		int oldMPMax = (int)args;
		if (MPMax > oldMPMax)
			MP += MPMax - oldMPMax;
		else
			MP = Mathf.Clamp(MP, 0, MPMax);
	}

	void OnTurnBegan (object sender, object args)
	{
		if (MP < MPMax)
			MP += Mathf.Max(Mathf.FloorToInt(MPMax * 0.1f), 1);
	}
	#endregion
}
