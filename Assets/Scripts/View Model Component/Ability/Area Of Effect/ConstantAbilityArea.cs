using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConstantAbilityArea : AbilityArea 
{
	public int horizontal;
	public int vertical;
	HexCell cell;

	public override List<HexCell> GetCellsInArea(HexGrid grid, HexCell cell)
	{
        return grid.SearchInRange(cell, horizontal, true);
	}

}