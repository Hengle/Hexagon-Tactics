using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitAbilityArea : AbilityArea 
{
	public override List<HexCell> GetCellsInArea(HexGrid grid, HexCell cell) {
		List<HexCell> retValue = new List<HexCell>();
		if (cell != null)
	    	retValue.Add(cell);
		return retValue;
	}
}
