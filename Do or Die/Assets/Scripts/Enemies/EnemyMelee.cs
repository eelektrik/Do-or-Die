using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] protected float meleeSpeed;        public void SetMeleeSpeed(float speed) { meleeSpeed = speed; }
    protected float meleeDuration;                      public void SetMeleeDuration(float duration) { meleeDuration = duration; }
    private Vector3 direction;                          public void SetDirection(Vector3 v) { direction = v; }
    private int damage;                                 public int GetDamage() { return damage; }
    [SerializeField] protected GameObject meleeAnimation;
    [SerializeField] protected GameObject reachAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MeleeSwing(Vector3 dir, int dam)
    {
        //Set direction
        direction = dir;
        //Set damage
        damage = dam;
        Vector3 force_direction = (direction.normalized * meleeSpeed);
        //Reset the velocity to zero, then set it
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(force_direction, ForceMode.Impulse);
        //Start life timer
        StartCoroutine(MeleeDuration());
    }

    public void SetMelee()
    {
        if (!meleeAnimation.gameObject.activeSelf)
        {
            meleeAnimation.SetActive(true);
            reachAnimation.SetActive(false);
        }
    }

    public void SetReach()
    {
        if (!reachAnimation.gameObject.activeSelf)
        {
            reachAnimation.SetActive(true);
            meleeAnimation.SetActive(false);
        }
    }

    IEnumerator MeleeDuration()
    {
        float timer = 0;

        while (timer < meleeDuration)
        {
            timer+= Time.deltaTime;
            yield return null;
        }
        FinishAttack();
    }

    private void FinishAttack()
    {
        EnemyMeleeManager.instance.AddUnusedProjectile(this);
        gameObject.SetActive(false);
    }
}
