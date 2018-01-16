using UnityEngine;
using UnityEditor;

public class YourClassAsset
{

	[MenuItem("Assets/Create/Unit Recipe")]
	public static void CreateUnitRecipe ()
	{
		ScriptableObjectUtility.CreateAsset<UnitRecipe> ();
	}
	
	[MenuItem("Assets/Create/Ability Catalog Recipe")]
	public static void CreateAbilityCatalogRecipe ()
	{
		ScriptableObjectUtility.CreateAsset<AbilityListRecipe> ();
	}
}