using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private InputAction moveAction;

    private float acc = 500f;

    private float max_speed = 5f;
    Vector2 moveVal;


    private Rigidbody2D rigidbody;


    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Move
        moveVal = moveAction.ReadValue<Vector2>();

        
    }
    void FixedUpdate()
    {
        
        rigidbody.AddForce(moveVal * acc, ForceMode2D.Force);
        CapSpeed(rigidbody);
        
        if (moveVal.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(moveVal.y, moveVal.x) * Mathf.Rad2Deg - 90f;
            rigidbody.MoveRotation(targetAngle);
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
    
    
}
