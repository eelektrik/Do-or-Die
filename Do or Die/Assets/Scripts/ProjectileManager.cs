using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private bool Assist;
    private float AssistAngleThreshold;
    private float AimAssistMaxDistance;
    private float dice_spread_variance;
    private float dice_spread_angle;
    private float dotProductThreshold;
    private int frameCounter;
    public Projectile projectile;
    public Transform player;
    public List<Transform> gunLocations;
    private List<(Enemy, float)> targetlist = new List<(Enemy, float)>();
    private ScoreComp comp = new ScoreComp();
    
    private List<Projectile> projectilePool = new List<Projectile>();
    private List<Projectile> unusedProjectiles = new List<Projectile>();
    public void AddUnusedProjectile (Projectile p) { unusedProjectiles.Add(p); }
    public class ScoreComp : IComparer<(Enemy, float)>
    {
        public int Compare((Enemy, float) x, (Enemy, float) y)
        {
            if (x.Item2 == y.Item2)
            {
                return 0;
            }
            else if (x.Item2 > y.Item2)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    //Singleton
    public static ProjectileManager instance;
    private void Awake()
    {
        instance = this;
        
    }


    // Start is called before the first frame update
    void Start()
    {
        Assist = PlayerStats.instance.GetAssist();
        AssistAngleThreshold = PlayerStats.instance.GetAssistAngle() * Mathf.PI / 180;
        dotProductThreshold = Mathf.Cos(AssistAngleThreshold);
        AimAssistMaxDistance = PlayerStats.instance.GetAssistDistance();
        dice_spread_variance = PlayerStats.instance.GetDiceSpreadVariance();
        dice_spread_angle = PlayerStats.instance.GetDiceSpreadAngle() * Mathf.PI / 180;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckFire();
    }


    void CheckFire ()
    {
        //Update the shot counter
        frameCounter++;
        Vector2 input = InputManager.instance.GetRightJoystickInputs();

        if (input.x != 0 || input.y != 0) //Check for input first for projectile spread varying from input
        {
            if (frameCounter < PlayerStats.instance.GetShotInterval())
            {
                return;
                //If it's too soon to fire, then don't, otherwise, check input and reset the counter if firing.

            }
            else
            {

                CameraShake.instance.FireShake();
                Vector3 debuginput = new Vector3(input.x, 0, input.y);
                Debug.DrawRay(transform.position, debuginput.normalized * AimAssistMaxDistance, Color.green);
                Debug.DrawRay(transform.position, Vector3.RotateTowards(debuginput.normalized * AimAssistMaxDistance, -debuginput.normalized * AimAssistMaxDistance, AssistAngleThreshold, 1f), Color.red);
                Debug.DrawRay(transform.position, Vector3.RotateTowards(debuginput.normalized * AimAssistMaxDistance, -debuginput.normalized * AimAssistMaxDistance, -AssistAngleThreshold, 1f), Color.red);

                Vector3 input3d = AimAssist(input);
                StartCoroutine(ProjectileFire(input3d));
                frameCounter = 0;
                
            }
            
        }
    }
    Vector3 AimAssist(Vector2 input)
    {
        if (Assist)
        {
            targetlist.Clear();
            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            {
                Vector3 enemy_direction = EnemyDirection(enemy.transform.position);
                float dotprod = Vector2.Dot(input.normalized, new Vector2(enemy_direction.x, enemy_direction.z).normalized);
                //Debug.Log("dotprod: " + dotprod + " dotProductThreshhood: " + dotProductThreshold);
                //Debug.Log("Distance: "+ EnemyDirection(enemy.transform.position).magnitude);
                if (dotprod >= dotProductThreshold && EnemyDirection(enemy.transform.position).magnitude <= AimAssistMaxDistance)
                {
                    targetlist.Add((enemy, CalculateRating(dotprod, EnemyDirection(enemy.transform.position).magnitude)));
                }
            }
            //sort targetlist by scores
            //Debug.Log(targetlist.Count);
            if (targetlist.Count > 0)
            {
                targetlist.Sort(comp);
                return EnemyDirection(targetlist[0].Item1.transform.position);
            }
            else
            {
                return new Vector3(input.x, 0, input.y);
            }

        }
        else
        {
            return new Vector3(input.x, 0, input.y).normalized;
        }


    }
    float CalculateRating(float dotprod, float distance)
    {
        return (distance / ((Mathf.Sin(Mathf.Acos(dotprod)) * distance) * dotprod))/ distance;
    }

    Vector3 EnemyDirection(Vector3 enemyposition)
    {
        return new Vector3(enemyposition.x - transform.position.x, enemyposition.y - transform.position.y, enemyposition.z - transform.position.z);

    }
    IEnumerator ProjectileFire(Vector3 input)
    {

        int counter = 0;
        foreach (int die in PlayerStats.instance.GetDice())
        {
            float variance = Random.Range(-dice_spread_variance, dice_spread_variance) * counter;
            Vector3 direction = Vector3.RotateTowards(input, -input, dice_spread_angle * variance, 1f);
            Projectile newProjectile = RequestProjectile();
            //new_projectile.direction = new Vector3(input.x, 0, input.y);
            newProjectile.StartProjectile(direction, die, PlayerStats.instance.GetBonusDamage());
            counter++;
            yield return null;


            //Play Fire Sound.
            //SpeedRTPCControl.instance.PlayFireSound();
        }

    }

    Vector2 aGetAxisDirection()
    {
        return new Vector2(Input.GetAxis("RightThumbstickX"), Input.GetAxis("RightThumbstickY"));
    }


    public Projectile RequestProjectile ()
    {
        if (unusedProjectiles.Count != 0)
        {
            Projectile p = unusedProjectiles[0];
            p.gameObject.SetActive(true);
            p.transform.position = GetRandomFirePosition();

            unusedProjectiles.RemoveAt(0);
            return p;
        }
        else
        {
            return Instantiate(projectile, GetRandomFirePosition(), Quaternion.identity);
        }
    }

    public Vector3 GetRandomFirePosition ()
    {
        int rng = Random.Range(0, gunLocations.Count);
        return gunLocations[rng].position;
    }


}
