using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction attackAction;
    private float acc = 500f;
    private float max_speed = 10f;
    Vector2 moveVal;
    private Rigidbody2D rb;

    [SerializeField] GameObject manager;
    Projectile prman;


    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        prman = manager.GetComponent<Projectile>();
    }

    void Update()
    {
        //Move
        moveVal = moveAction.ReadValue<Vector2>();
        if (attackAction.WasPerformedThisFrame())
        {
            Shoot(transform.up); //direction player is facing
        }
    }
    void FixedUpdate()
    {
        
        rb.AddForce(moveVal * acc, ForceMode2D.Force);
        CapSpeed(rb);
        
        if (moveVal.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(moveVal.y, moveVal.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(targetAngle);
        }
        
    }
    void CapSpeed(Rigidbody2D rb)
    {
        Vector2 v_current = rb.linearVelocity;
        Vector2 velocity = new Vector2(v_current.x, v_current.y);
        if (velocity.magnitude > max_speed)
        {
            velocity = Vector2.ClampMagnitude(velocity, max_speed);
            rb.linearVelocity = new Vector2(velocity.x, velocity.y);
        }
    }    

    void Shoot(Vector2 direction)
    {
        GameObject bul = prman.GetFirstActive();

        if (bul == null)
        {
            return;
        }

        bul.transform.position = transform.position;
        prman.Launch(bul, direction);
        //bring bullet to our location
        //start coroutine to have bullet travel in direction

    }
    
    
}
