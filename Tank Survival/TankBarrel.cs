using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBarrel : MonoBehaviour
{
    private HingeJoint2D barrelHinge;
    public GameObject bullet;
    private Rigidbody2D tankRigidBody;
    private Vector3 bulletSpawnOffset = new Vector3(0.5f, 0, 0);
    public int rotationOffset = 0;
    private float bulletSpeed = 5f;
    private float timeTillNextFire = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        InitialisePlayerTank();        
    }
   
    // Update is called once per frame
    void Update()
    {
        PlayerShoot();
    }

    void FixedUpdate()
    {
        RotateBarrel();
    }

    void InitialisePlayerTank()
    {
        tankRigidBody = GameObject.Find("NavyTankBase").GetComponent<Rigidbody2D>();
        barrelHinge = gameObject.AddComponent<HingeJoint2D>();

        JointAngleLimits2D jointLimits = barrelHinge.limits;

        barrelHinge.connectedBody = tankRigidBody;
        barrelHinge.autoConfigureConnectedAnchor = true;
    }

    void RotateBarrel()
    {
        Vector3 differenceToMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
        differenceToMouse.Normalize();
        float turretAngle = Mathf.Atan2(differenceToMouse.y, differenceToMouse.x) * Mathf.Rad2Deg;
        if (turretAngle <= 180 && turretAngle >= 0)
        {
            transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0f, 0f, turretAngle + rotationOffset), 2000 * Time.deltaTime);
        }
    }

    void PlayerShoot()
    {
        Vector3 bulletStartPostion = this.transform.TransformPoint(bulletSpawnOffset);

        float rateOfFire = 0.3f;
        
        if(Input.GetMouseButton(0) && Time.time > timeTillNextFire)
        {
            timeTillNextFire = Time.time + rateOfFire;
            
            GameObject spawnBullet = Instantiate(bullet, bulletStartPostion, this.transform.rotation) as GameObject;
            Rigidbody2D firedBullet = spawnBullet.GetComponent<Rigidbody2D>();
            firedBullet.velocity = this.transform.TransformDirection(1, 0, 0) * bulletSpeed;
        }
    }
}

// TODO: