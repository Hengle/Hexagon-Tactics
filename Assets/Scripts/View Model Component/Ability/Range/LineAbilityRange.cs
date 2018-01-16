/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineAbilityRange : AbilityRange 
{
	public override bool directionOriented { get { return true; }}
	
	public override List<HexCell> GetTilesInRange (HexGrid grid)
	{
		HexCell startCell = unit.Cell;
		HexCell endCell;
		List<Tile> retValue = new List<Tile>();
		
		switch (unit.dir)
		{
		case Directions.North:
			endCell = new Point(startCell.x, grid.max.y);
			break;
		case Directions.East:
			endCell = new Point(grid.max.x, startCell.y);
			break;
		case Directions.South:
			endCell = new Point(startCell.x, grid.min.y);
			break;
		default: // West
			endCell = new Point(grid.min.x, startCell.y);
			break;
		}

		int dist = 0;
		while (startCell != endCell)
		{
			if (startCell.x < endCell.x) startCell.x++;
			else if (startCell.x > endCell.x) startCell.x--;
			
			if (startCell.y < endCell.y) startCell.y++;
			else if (startCell.y > endCell.y) startCell.y--;
			
			Tile t = grid.GetTile(startCell);
			if (t != null && Mathf.Abs(t.height - unit.tile.height) <= vertical)
				retValue.Add(t);

			dist++;
			if (dist >= horizontal)
				break;
		}
		
		return retValue;
	}
} */