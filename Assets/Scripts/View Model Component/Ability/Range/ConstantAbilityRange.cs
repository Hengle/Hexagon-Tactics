using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConstantAbilityRange : AbilityRange {

    public bool canTargetSelf = true;

    public override List<HexCell> GetCellsInRange (HexGrid grid)
	{
        return grid.SearchInRange(unit.Cell, horizontal, canTargetSelf);
	}
	
}