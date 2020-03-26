using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public int dam;         //in order to play damage sound.


    [SerializeField] protected float impactForce;
    private bool invulnerable = false;
    public bool IsInvulnerable () { return invulnerable; }



    //Handle whether or not the player is burning here
    private bool burning = false;
    public bool IsBurning() { return burning; }



    //How slowly the player flashes during invulnerability
    public float meshFlashInterval;


    public void TakeHit (Transform enemy, int damage)
    {

        //We'll check here if the player is burning, in that case we skip taking damage
        if (burning) { return; }


        //Debug.Log(damage);
        damage = (int) (Mathf.Max(1f, damage * (1 - DamageReduction())) + 0.5f);
        //Debug.Log("Damage: " + damage);
        PlayerStats.instance.ModifyHealth(-damage);



        //Push the player away from this enemy
        Vector3 strikeDirection = transform.position - enemy.position;
        GetComponent<Rigidbody>().AddForce(strikeDirection.normalized * impactForce, ForceMode.Impulse);

        //Camera shake on damage
        CameraShake.instance.OnDamageShake();

        //Player is invulnerable for a bit after being hit
        StartCoroutine(StartInvulnerability(PlayerStats.instance.GetInvulnerabilityTime()));

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private float DamageReduction()
    {
        int armor = PlayerStats.instance.GetArmor();
        //Debug.Log("DamageReduction: " + PlayerStats.instance.GetArmorScaling() * (Mathf.Sqrt(2 * armor + Mathf.Sqrt(armor) + Mathf.Sqrt(armor + Mathf.Sqrt(2 * armor)))));
        return PlayerStats.instance.GetArmorScaling() * (Mathf.Sqrt(2 * armor + Mathf.Sqrt(armor) + Mathf.Sqrt(armor + Mathf.Sqrt(2 * armor))));
    }
    IEnumerator StartInvulnerability (float invulnTime)
    {

        float timer = 0;
        float flashTimer = 0;
        MeshRenderer mesh = GetComponent<MeshRenderer>();


        while (timer < invulnTime)
        {
            timer += Time.deltaTime;

            flashTimer += Time.deltaTime;
            if (flashTimer >= meshFlashInterval)
            {
                flashTimer = 0;
                mesh.enabled = !mesh.enabled;
            }

            invulnerable = true;
            yield return null;
        }


        mesh.enabled = true;

        invulnerable = false;

    }
    public void shortInvuln(float invulnTime)
    {
        StartCoroutine(StartInvulnerability(invulnTime));
    }



    Coroutine lastBurnBuff;
    //Call this when a flame-pickup is picked up
    public void StartBurn(float duration)
    {
        if (lastBurnBuff != null) { StopCoroutine(lastBurnBuff); }
        GetComponent<MeshRenderer>().enabled = true;
        invulnerable = false;
        lastBurnBuff = StartCoroutine(BurnDuration(duration));
    }
    public IEnumerator BurnDuration(float duration)
    {

        //Start burning
        burning = true;
        //PickUpAudio.instance.PlayStartBurn();

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //Stop burning
        burning = false;
        //PickUpAudio.instance.PlayStopBurn();
    }



}
