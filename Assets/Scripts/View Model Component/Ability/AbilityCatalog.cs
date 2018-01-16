using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Assumes that all direct children are categories
/// and that the direct children of categories
/// are abilities
/// </summary>
public class AbilityList : MonoBehaviour {

	public int AbilityCount () {
		return transform.childCount;
	}

    public List<Ability> getAbilities() {
        List<Ability> abilities = new List<Ability>();
        for( int i = 0; i< AbilityCount(); i++) {
            Ability ability = GetAbility(i);
            if (ability != null)
                abilities.Add(ability);
        }
        return abilities;
    }

	public Ability GetAbility (int abilityIndex) {
		if (abilityIndex < 0 || abilityIndex >= transform.childCount)
			return null;
		return transform.GetChild(abilityIndex).GetComponent<Ability>();
	}
}