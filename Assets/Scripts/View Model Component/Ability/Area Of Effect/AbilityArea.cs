using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbilityArea : MonoBehaviour
{
	public abstract List<HexCell> GetCellsInArea (HexGrid grid, HexCell cell);
}