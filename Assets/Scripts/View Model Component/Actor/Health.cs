using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	#region Fields & Properties
	public int HP
	{
		get { return stats[StatTypes.HP]; }
		set { stats[StatTypes.HP] = value; }
	}
	
	public int HPMax
	{
		get { return stats[StatTypes.HPMax]; }
		set { stats[StatTypes.HPMax] = value; }
	}
	
	public int HPMin = 0;
	Stats stats;
	#endregion
	
	#region MonoBehaviour
	void Awake ()
	{
		stats = GetComponent<Stats>();
	}
	
	void OnEnable ()
	{
		this.AddObserver(OnHPWillChange, Stats.WillChangeNotification(StatTypes.HP), stats);
		this.AddObserver(OnHPMaxDidChange, Stats.DidChangeNotification(StatTypes.HPMax), stats);
	}
	
	void OnDisable ()
	{
		this.RemoveObserver(OnHPWillChange, Stats.WillChangeNotification(StatTypes.HP), stats);
		this.RemoveObserver(OnHPMaxDidChange, Stats.DidChangeNotification(StatTypes.HPMax), stats);
	}
	#endregion
	
	#region Event Handlers
	void OnHPWillChange (object sender, object args)
	{
		ValueChangeException vce = args as ValueChangeException;
		vce.AddModifier(new ClampValueModifier(int.MaxValue, HPMin, stats[StatTypes.HPMax]));
	}
	
	void OnHPMaxDidChange(object sender, object args)
	{
		int oldHPMax = (int)args;
		if (HPMax > oldHPMax)
			HP += HPMax - oldHPMax;
		else
			HP = Mathf.Clamp(HP, HPMin, HPMax);
	}
	#endregion
}
