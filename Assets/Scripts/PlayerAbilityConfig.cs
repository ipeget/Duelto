using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbilityConfig", menuName = "PlayerAbilityConfig")]
public class PlayerAbilityConfig : ScriptableObject
{
    [SerializeReference, SubclassSelector] private List<Ability> abilities = new List<Ability>();
    public List<Ability> GetAbilities() { return new List<Ability>(abilities); } 

    public List<IAbility> GetAbilityPredicates()
    {
        List<IAbility> list = new List<IAbility>();
        foreach (var ability in abilities)
            list.Add(ability);

        return list;
    }

}
