using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Vector3 ConstantForce;
    public CollisionDetection TackCollisionDetector;
    public Material SelectedMaterial;
    public Material NotSelectedMaterial;

    // Control fire speed
    public float RateOfFire;
    private float TimeOfLastFireSeconds;

    public float MinTurnStrength;
    public float MaxTurnStrength;

    public float TurretTurnRate;
    public float TurretTurnStrength;

    public float MinDistanceFromTarget;

    public float AttackRange;

    public GameObject Turret;
    public GameObject Cannon;

    public GameObject Projectile;

    private enum ETargetType
    {
        eNone = 0,
        eGoTo,
        eAttack
    }

    private ETargetType TargetType;

    private Vector3 GoToTarget;
    private EnemyController AttackTarget;

    private Rigidbody TankRigidbody;

    private Rigidbody TurretRigidbody;
    private Transform TurretTransform;

    private Transform CannonTransform;

    public Transform MuzzleTransform;

	// Use this for initialization
	void Start ()
    {
        GoToTarget = new Vector3();
        AttackTarget = null;
        TargetType = ETargetType.eNone;
        TankRigidbody = GetComponent<Rigidbody>();

        TurretRigidbody = Turret.GetComponent<Rigidbody>();
        TurretTransform = Turret.GetComponent<Transform>();

        CannonTransform = Cannon.GetComponent<Transform>();

        TimeOfLastFireSeconds = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (TackCollisionDetector.IsColliding())
        {
            if (TargetType == ETargetType.eGoTo)
            {
                MoveToTarget();
            }
            else if (TargetType == ETargetType.eAttack)
            {
                bool ready_to_fire = AimAtTarget(AttackTarget.GetPosition());

                if (ready_to_fire)
                {
                    FireCannon();
                }
            }

        }
    }

    void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, GoToTarget) <= MinDistanceFromTarget)
        {
            TargetType = ETargetType.eNone;
        }
        else
        {
            TankRigidbody.AddRelativeForce(ConstantForce);
            float angle_to_target = Vector3.SignedAngle(transform.up, transform.position - GoToTarget, Vector3.up);
            Debug.Log(angle_to_target);

            if (Mathf.Abs(angle_to_target) > 5.0)
            {
                TankRigidbody.AddRelativeTorque(new Vector3(0, 0, angle_to_target * MaxTurnStrength));
            }
            else
            {
                TankRigidbody.AddRelativeTorque(new Vector3(0, 0, angle_to_target * MinTurnStrength));
            }
        }
    }

    // Point the cannon at a target location. Return true if ready to fire
    bool AimAtTarget(Vector3 target_location)
    {
        // Find the angle that the turret will need to turn by to face target
        float angle_to_target = Vector3.SignedAngle(TurretTransform.up, transform.position - target_location, Vector3.up);

        // Convert this in to a target turn rate
        float target_turn_rate = TurretTurnRate * angle_to_target / 180.0f;

        float turn_force = TurretTurnStrength * (target_turn_rate - TurretRigidbody.angularVelocity.y);

        TurretRigidbody.AddRelativeTorque(new Vector3(0, 0, turn_force));

        bool ready_to_fire = false;

        if (Mathf.Abs(angle_to_target) < 10.0)
        {
            ready_to_fire = true;
        }

        return ready_to_fire;

        //// Within this angle (degrees) scale turn speed to slow down
        //const float angle_threshold = 10.0f;

        //if (Mathf.Abs(angle_to_target) > 10.0)
        //{
        //    TurretRigidbody.AddRelativeTorque(new Vector3(0, 0, Mathf.Sign(angle_to_target) * MaxTurretTurnStrength));
        //}
        //else
        //{
        //    if (angle_to_target < 5.0)
        //    {
        //        ready_to_fire = true;
        //    }

        //    TurretRigidbody.AddRelativeTorque(new Vector3(0, 0, angle_to_target * MaxTurretTurnStrength / angle_threshold));
        //}

        //// Add friction
        //TurretRigidbody.AddRelativeTorque(-TurretRigidbody.angularVelocity * TurretRotateFriction);

        //return ready_to_fire;
    }

    public void SetTargetLocation(Vector3 new_target)
    {
        GoToTarget = new_target;
        TargetType = ETargetType.eGoTo;
    }

    public void SetAttackTarget(EnemyController target_enemy)
    {
        AttackTarget = target_enemy;
        TargetType = ETargetType.eAttack;
    }
    
    public void Select()
    {
        GetComponent<MeshRenderer>().materials[2] = SelectedMaterial;
    }

    public void UnSelect()
    {
        GetComponent<MeshRenderer>().materials[2] = NotSelectedMaterial;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    // Spawn a projectil from the cannon
    // Will only fire if allowed by the rate of fire
    private void FireCannon()
    {
        float current_time_seconds = Time.time;

        // Is it time to fire?
        if (current_time_seconds - TimeOfLastFireSeconds > (1.0f / RateOfFire))
        {
            Debug.Log("Firing!");

            GameObject new_projectile = Instantiate(Projectile, MuzzleTransform.position, CannonTransform.rotation);
            new_projectile.GetComponent<Rigidbody>().AddForce(-CannonTransform.up.normalized * 5000);
            TimeOfLastFireSeconds = Time.time;
        }
    }
}
