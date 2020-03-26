using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : BaseUpgrade
{
    [SerializeField] protected short tier;    //tier 0 for attributes
    [SerializeField] protected bool diceUpgrade;
    [SerializeField] protected bool movement;   //tier 1 for dash, 2 for jump, 3 for bullet time
    [SerializeField] protected short element; //1: cold, 2: lightning, 3: fire, 4: acid     Use tier 1, 2, 3 for d6, d8, d10

    [SerializeField] protected bool damageUp;
    [SerializeField] protected bool shotSpeedUp;
    [SerializeField] protected bool knockbackUp;
    [SerializeField] protected bool jumpUp;

    [SerializeField] protected bool speedUp;
    [SerializeField] protected bool fireRateUp;
    [SerializeField] protected bool responsivenessUp;
    [SerializeField] protected bool dashUp;

    [SerializeField] protected bool critChanceUp;
    [SerializeField] protected bool critBonusUp;
    [SerializeField] protected bool elementalUp;
    [SerializeField] protected bool BTSlowUp;

    [SerializeField] protected bool healthUp;
    [SerializeField] protected bool rechargeUp;
    [SerializeField] protected bool invulnerabilityUp;
    [SerializeField] protected bool knockbackResistUp;

    [SerializeField] protected bool shotSizeUp;
    [SerializeField] protected bool armorUp;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void UpgradePlayer()
    {

        base.UpgradePlayer();

        /*List<int> upgrades = new List<int> {damageUp, shotSpeedUp, knockbackUp, jumpUp, speedUp, fireRateUp, responsivenessUp, dashUp,
                                            critChanceUp, critBonusUp, elementalUp, BTSlowUp, healthUp, rechargeUp, invulnerabilityUp, knockbackResistUp,
                                            shotSizeUp, armorUp};*/

        if (diceUpgrade)
        {
            switch (tier)
            {
                case 1:
                    PlayerStats.instance.AddDie(6);
                    break;
                case 2:
                    PlayerStats.instance.AddDie(8);
                    break;
                case 3:
                    PlayerStats.instance.AddDie(10);
                    break;
            }
        }
        if (movement)
        {
            switch (tier)
            {
                case 1:
                    PlayerController.instance.GetComponent<Dash>().enabled = true;
                    break;
                case 2:
                    PlayerController.instance.GetComponent<Jump>().enabled = true;
                    break;
                case 3:
                    PlayerController.instance.GetComponent<Bullettime>().enabled = true;
                    break;
            }
        }


        if (damageUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeBonusDamage(.55f);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeBonusDamage(1f);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeBonusDamage(1.55f);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeBonusDamage(2f);
                    break;
            }
        }

        if (shotSpeedUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeShotSpeed(1f);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeShotSpeed(2f);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeShotSpeed(3f);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeShotSpeed(4f);
                    break;
            }
        }
        if (knockbackUp)
        {
            PlayerStats.instance.UpgradeShotKnockback(.25f);
        }
        if (jumpUp)
        {
            PlayerStats.instance.UpgradeJump(50f);
        }
        if (speedUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeSpeed(.5f);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeSpeed(1f);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeSpeed(1.5f);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeSpeed(2f);
                    break;
            }
        }
        if (fireRateUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeFireRate(1);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeFireRate(2);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeFireRate(2);
                    PlayerStats.instance.UpgradeShotSpeed(1);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeFireRate(3);
                    break;
            }
        }
        if (responsivenessUp)
        {
            PlayerStats.instance.UpgradeResponsiveness(.25f);
        }
        if (dashUp)
        {
            PlayerStats.instance.UpgradeDash(100f);
        }
        if (critChanceUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeCritChance(.01f);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeCritChance(.02f);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeCritChance(.025f);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeCritChance(.03f);
                    break;
            }
        }
        if (critBonusUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeCritDamage(.1f);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeCritDamage(.2f);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeCritDamage(.25f);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeCritDamage(.3f);
                    break;
            }
        }

        if (BTSlowUp)
        {
            PlayerStats.instance.UpgradeBTSlow(.05f);
        }

        if (healthUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeMaxHealth(2);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeMaxHealth(3);
                    //PlayerStats.instance.UpgradeMaxHealth(Random.Range(1, 7));
                    //TODO Test with random values?
                    break;
                case 2:
                    PlayerStats.instance.UpgradeMaxHealth(5);
                    //PlayerStats.instance.UpgradeMaxHealth(Random.Range(1, 9));
                    break;
                case 3:
                    PlayerStats.instance.UpgradeMaxHealth(7);
                    //PlayerStats.instance.UpgradeMaxHealth(Random.Range(1, 11));
                    break;
            }
        }

        if (rechargeUp)
        {
            switch (tier)
            {
                case 0:
                    PlayerStats.instance.UpgradeRecharge(.05f);
                    break;
                case 1:
                    PlayerStats.instance.UpgradeRecharge(.10f);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeRecharge(.15f);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeRecharge(.20f);
                    break;
            }
        }

        if (knockbackResistUp)
        {
            PlayerStats.instance.UpgradeKnockbackMultiplier(.10f);
        }

        if (invulnerabilityUp)
        {
            PlayerStats.instance.UpgradeInvulnerabilityTime(.25f);
        }

        if (armorUp)
        {
            switch (tier)
            {
                case 1:
                    PlayerStats.instance.UpgradeArmor(1);
                    break;
                case 2:
                    PlayerStats.instance.UpgradeArmor(2);
                    break;
                case 3:
                    PlayerStats.instance.UpgradeArmor(3);
                    break;
            }
        }

        if (shotSizeUp) //Not currently used, figure out where to include this
        {
            switch (tier)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        /*
        for (int i = 0; i < damageUp; i++)          {PlayerStats.instance.UpgradeBonusDamage(1);}
        for (int i = 0; i < shotSpeedUp; i++)       {PlayerStats.instance.UpgradeShotSpeed(.25f);}
        for (int i = 0; i < knockbackUp; i++)       {PlayerStats.instance.UpgradeShotKnockback(.25f);}
        for (int i = 0; i < jumpUp; i++)            {PlayerStats.instance.UpgradeJump(100f);}

        for (int i = 0; i < speedUp; i++)           {PlayerStats.instance.UpgradeSpeed(.5f);}
        for (int i = 0; i < fireRateUp; i++)        {PlayerStats.instance.UpgradeFireRate(1);}
        for (int i = 0; i < responsivenessUp; i++)  {PlayerStats.instance.UpgradeResponsiveness(.25f);}
        for (int i = 0; i < dashUp; i++)            {PlayerStats.instance.UpgradeDash(100f);}
        
        for (int i = 0; i < critChanceUp; i++)      {PlayerStats.instance.UpgradeCritChance(.01f);}
        for (int i = 0; i < critBonusUp; i++)       {PlayerStats.instance.UpgradeCritDamage(.15f);}
        for (int i = 0; i < elementalUp; i++)       {PlayerStats.instance.UpgradeElemental(.25f);}
        for (int i = 0; i < BTSlowUp; i++)          {PlayerStats.instance.UpgradeBTSlow(.05f);}

        for (int i = 0; i < healthUp; i++)          {PlayerStats.instance.UpgradeMaxHealth(4);}
        for (int i = 0; i < rechargeUp; i++)        {PlayerStats.instance.UpgradeRecharge(.25f);}
        for (int i = 0; i < invulnerabilityUp; i++) {PlayerStats.instance.UpgradeInvulnerabilityTime(.25f);}
        for (int i = 0; i < knockbackResistUp; i++) {PlayerStats.instance.UpgradeKnockbackMultiplier(.15f);}

        for (int i = 0; i < shotSizeUp; i++)        {PlayerStats.instance.UpgradeShotSize(.25f); }
        for (int i = 0; i < armorUp; i++)           {PlayerStats.instance.UpgradeArmor(1);}
        */
    }

}
