using UnityEngine;
using System.Collections;

public class PhysicalAbilityPower : BaseAbilityPower 
{
	public int level;
	
	protected override int GetBaseAttack ()
	{
		return GetComponentInParent<Stats>()[StatTypes.Attack];
	}

	protected override int GetBaseDefense (Unit target)
	{
		return target.GetComponent<Stats>()[StatTypes.Armor];
	}
	
	protected override int GetPower ()
	{
		return level;
	}
}
