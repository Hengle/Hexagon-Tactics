using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitBattleState : BattleState 
{
	public override void Enter ()
	{
		base.Enter ();
		StartCoroutine(Init());
	}
	
	IEnumerator Init ()
	{
		SpawnTestUnits();
		//AddVictoryCondition();
		owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
		yield return null;
		owner.ChangeState<SelectUnitState>();
	}
	
	void SpawnTestUnits ()
	{
		string[] recipes = new string[]
		{
			"Otario",
            "Jones",
		};
		
		GameObject unitContainer = new GameObject("Units");
		unitContainer.transform.SetParent(owner.transform);

        List<HexCell> locations = Grid.GetCells();
		for (int i = 0; i < recipes.Length; ++i)
		{
			int level = UnityEngine.Random.Range(9, 12);
			GameObject instance = UnitFactory.Create(recipes[i], level);
			instance.transform.SetParent(unitContainer.transform);
			
			int random = UnityEngine.Random.Range(0, locations.Count);
			HexCell randomTile = locations[ random ];
			locations.RemoveAt(random);
			
			Unit unit = instance.GetComponent<Unit>();
			unit.Place( randomTile );
			unit.direction = (HexDirection)UnityEngine.Random.Range(0, 6);
			unit.Match();
			
			units.Add(unit);
		}
	}
	
	/* void AddVictoryCondition ()
	{
		DefeatTargetVictoryCondition vc = owner.gameObject.AddComponent<DefeatTargetVictoryCondition>();
		Unit enemy = units[ units.Count - 1 ];
		vc.target = enemy;
		Health health = enemy.GetComponent<Health>();
		health.MinHP = 10;
	} */
}