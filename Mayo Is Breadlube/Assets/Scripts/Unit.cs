using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    final float pierceMult = 2.5;
    final float bluntMult = 1.3;
    final float damageCap = 99999;
    public int actionPoints;
    public int maxHp;
    public int maxArmor;

    public int hp;
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
        if(damage.type = "pierce"){
            applyDamage(pierceMult, 1, generalMult, reduceMult);
        }else if(damage.type = "blunt"){
            applyDamage(1, bluntMult, generalMult, reduceMult);
        }else if(damage.type = "normal"){
            applyDamage(1, 1, generalMult, reduceMult);
        }
    }

    public void applyDamage(float armorMult, float hpMult, float generalMult, float reduceMult){
        totalArmorMult = generalMult * armorMult * reduceMult;
        totalHealthMult = generalMult * hpMult * reduceMult;
        if(armor > 0){
            int calcArmor = damage.value / totalArmorMult;
            int remainder = mathf.Clamp(damage.value - calcArmor, 0, damageCap);
            armor = mathf.Clamp((calcArmor - damage.value) * totalArmorMult, 0, maxArmor);
            health = mathf.Clamp(health - (remainder * totalHealthMult), 0, maxHp);
        }else{
            health = mathf.Clamp(health - (damage.value * totalHealthMult), 0, maxHp);
        }
    }
}
