using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int actionPoints;
    public int maxHp;
    public int maxArmor;

    public int health;
    public int armor;

    public bool guard;
    public bool parry;
    public float generalMult;
    public float reduceMult;
    public int atkMult;

    //public List<string> statuses;

    //If theres enough probably worth making them into an array
    public int poisonCounter;
    public int stunCounter;
    public int staggerCounter;
    
    //public string weapon;

    public AbilityTree abilityTree;

    public void takeDamage(Damage damage){
        //might need to make this check for all parry abilities active if there is more than one but lets simplify to just one for now
        if(parry){
            Ability parry = abilityTree.getAbility("Parry");
            parry.activate(damage.source);
        }

        if(damage.type == "pierce"){
            applyDamage(damage, Global.pierceMult, 1, generalMult, reduceMult);
        }else if(damage.type == "blunt"){
            applyDamage(damage, 1, Global.bluntMult, generalMult, reduceMult);
        }else if(damage.type == "normal"){
            applyDamage(damage, 1, 1, generalMult, reduceMult);
        }
    }

    public void applyDamage(Damage damage, float armorMult, float hpMult, float generalMult, float reduceMult){
        float totalArmorMult = generalMult * armorMult * reduceMult;
        float totalHealthMult = generalMult * hpMult * reduceMult;
        if(armor > 0){
            float calcArmor = damage.value / totalArmorMult;
            float remainder = Mathf.Clamp(damage.value - calcArmor, 0, Global.damageCap);

            armor = (int) Mathf.Clamp((calcArmor - damage.value) * totalArmorMult, 0, maxArmor);
            health = (int) Mathf.Clamp(health - (remainder * totalHealthMult), 0, maxHp);
        }else{
            health = (int) Mathf.Clamp(health - (damage.value * totalHealthMult), 0, maxHp);
        }
    }
}
