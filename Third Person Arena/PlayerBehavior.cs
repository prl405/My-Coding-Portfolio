using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 10f ;
    public float rotateSpeed = 75f;
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.1f;
    public float bulletSpeed = 100f;

    public LayerMask groundLayer;

    public GameObject bullet;

    private float vInput;
    private float hInput;

    private CapsuleCollider _col;

    private Rigidbody _rb;

    private GameBehavior _gameManager;

    public delegate void JumpingEvent();
    public event JumpingEvent playerJump;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
    }

    private void Update()
    {
        vInput = Input.GetAxis("Vertical") * moveSpeed;

        hInput = Input.GetAxis("Horizontal") * rotateSpeed;

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);

            playerJump();
        }
    }

    private void FixedUpdate() //Use fixed update when applying rigidbody related code
    {
        Vector3 rotation = Vector3.up * hInput; //Rotation about y-axis

        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + 
            this.transform.forward * vInput * Time.fixedDeltaTime);//Current postion + position being inputted

        _rb.MoveRotation(_rb.rotation * angleRot);//Current rotation * rotation being inputted

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, this.transform.position, 
                this.transform.rotation) as GameObject;

            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();

            bulletRB.velocity = this.transform.forward * bulletSpeed;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Enemy")
        {
            _gameManager.Lives -= 1;
        }
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new 
            Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);

        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, 
            distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);

        return grounded;
    }

}
