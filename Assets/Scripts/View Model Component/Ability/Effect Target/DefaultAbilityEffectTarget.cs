using UnityEngine;
using System.Collections;

public class DefaultAbilityEffectTarget : AbilityEffectTarget 
{
	public override bool IsTarget (HexCell tile)
	{
		if (tile == null || tile.Content == null)
			return false;

		Stats s = tile.Content.GetComponent<Stats>();
		return s != null && s[StatTypes.HP] > 0;
	}
}