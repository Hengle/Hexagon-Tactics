using UnityEngine;
using System.IO;
using System.Collections;

public static class UnitFactory
{
	#region Public
	public static GameObject Create (string name, int level)
	{
		UnitRecipe recipe = Resources.Load<UnitRecipe>("Unit Recipes/" + name);
		if (recipe == null)
		{
			Debug.LogError("No Unit Recipe for name: " + name);
			return null;
		}
		return Create(recipe, level);
	}

	public static GameObject Create (UnitRecipe recipe, int level)
	{
		GameObject obj = InstantiatePrefab("Units/" + recipe.model);
		obj.name = recipe.name;
		obj.AddComponent<Unit>();
		AddStats(obj);
		AddLocomotion(obj, recipe.locomotion);
        obj.AddComponent<Animation>();
		//obj.AddComponent<Status>();
		//obj.AddComponent<Equipment>();
		AddJob(obj, recipe.job);
		//AddRank(obj, level);
		obj.AddComponent<Health>();
		obj.AddComponent<Mana>();
		//AddAttack(obj, recipe.attack);
		AddAbilityList(obj, recipe.abilityList);
		//AddAlliance(obj, recipe.alliance);
		//AddAttackPattern(obj, recipe.strategy);
		return obj;
	}
	#endregion

	#region Private
	static GameObject InstantiatePrefab (string name)
	{
		GameObject prefab = Resources.Load<GameObject>(name);
		if (prefab == null)
		{
			Debug.LogError("No Prefab for name: " + name);
			return new GameObject(name);
		}
		GameObject instance = GameObject.Instantiate(prefab);
		instance.name = instance.name.Replace("(Clone)", "");
		return instance;
	}

	static void AddStats (GameObject obj)
	{
		Stats s = obj.AddComponent<Stats>();
		s.SetValue(StatTypes.Lvl, 1, false);
	}

	static void AddJob (GameObject obj, string name)
	{
		GameObject instance = InstantiatePrefab("Jobs/" + name);
		instance.transform.SetParent(obj.transform);
		Job job = instance.GetComponent<Job>();
		job.Employ();
		job.LoadDefaultStats();
	}

	static void AddLocomotion (GameObject obj, Locomotions type)
	{
		switch (type)
		{
		case Locomotions.Walk:
			obj.AddComponent<WalkMovement>();
			break;
		case Locomotions.Fly:
			//obj.AddComponent<FlyMovement>();
			break;
		case Locomotions.Teleport:
			//obj.AddComponent<TeleportMovement>();
			break;
		}
	}

	//static void AddAlliance (GameObject obj, Alliances type)
	//{
	//	Alliance alliance = obj.AddComponent<Alliance>();
	//	alliance.type = type;
	//}

	//static void AddRank (GameObject obj, int level)
	//{
	//	Rank rank = obj.AddComponent<Rank>();
	//	rank.Init(level);
	//}

	static void AddAttack (GameObject obj, string name)
	{
		GameObject instance = InstantiatePrefab("Abilities/" + name);
		instance.transform.SetParent(obj.transform);
	}

	static void AddAbilityList (GameObject obj, string name)
	{
		GameObject main = new GameObject("Ability List");
		main.transform.SetParent(obj.transform);
		main.AddComponent<AbilityList>();

		AbilityListRecipe recipe = Resources.Load<AbilityListRecipe>("Ability List Recipes/" + name);
		if (recipe == null)
		{
			Debug.LogError("No Ability List Recipe Found: " + name);
			return;
		}

		for (int i = 0; i < recipe.entries.Length; ++i)
		{
            string abilityName = string.Format("Abilities/{0}", recipe.entries[i]);
            GameObject ability = InstantiatePrefab(abilityName);
            ability.transform.SetParent(main.transform);
        }
	}

	static void AddAttackPattern (GameObject obj, string name)
	{
		//Driver driver = obj.AddComponent<Driver>();
		if (string.IsNullOrEmpty(name))
		{
			//driver.normal = Drivers.Human;
		}
		else
		{
			//driver.normal = Drivers.Computer;
			GameObject instance = InstantiatePrefab("Attack Pattern/" + name);
			instance.transform.SetParent(obj.transform);
		}
	}
	#endregion
}