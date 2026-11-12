using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float thrustForce = 50f;
    [SerializeField] private float boostMultiplier = 2.4f;
    [SerializeField] private float maxSpeed = 95f;

    [Header("Rotation")]
    [SerializeField] private float yawTorque = 9f;
    [SerializeField] private float pitchTorque = 8f;
    [SerializeField] private float rollTorque = 6.5f;
    [SerializeField] private float mouseSensitivity = 2.6f;
    [SerializeField] private float rollStabilization = 3.5f;

    [Header("Feel")]
    [SerializeField] private float linearDamping = 1.0f;
    [SerializeField] private float angularDamping = 3.0f;
    [SerializeField] private bool invertY = false;

    private Rigidbody rb;
    private bool isBoosting;

    private float thrustInput;   // W / S
    private float rollInput;     // A / D
    private float mouseX, mouseY;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ReadInput();
        HandleCursor();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
        ClampSpeed();
    }

    private void ReadInput()
    {
        // W = forward, S = backward
        thrustInput = 0f;
        if (Input.GetKey(KeyCode.W)) thrustInput = 1f;
        if (Input.GetKey(KeyCode.S)) thrustInput = -1f;

        // A / D = Roll
        rollInput = 0f;
        if (Input.GetKey(KeyCode.A)) rollInput = 1f;
        if (Input.GetKey(KeyCode.D)) rollInput = -1f;

        // Mouse look
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        if (invertY) mouseY = -mouseY;

        // Boost
        isBoosting = Input.GetKey(KeyCode.LeftShift);
    }

    private void ApplyMovement()
    {
        if (thrustInput != 0f)
        {
            float currentThrust = isBoosting ? thrustForce * boostMultiplier : thrustForce;

            // Using the corrected direction from before
            rb.AddForce(-transform.forward * thrustInput * currentThrust);
        }
    }

    private void ApplyRotation()
    {
        Vector3 torque = Vector3.zero;

        // Mouse: Yaw + Pitch
        torque += transform.up * (mouseX * yawTorque);
        torque += transform.right * (-mouseY * pitchTorque);

        // A / D: Roll
        torque += transform.forward * (rollInput * rollTorque);

        // Auto roll stabilization
        float currentRoll = transform.localEulerAngles.z;
        if (currentRoll > 180f) currentRoll -= 360f;
        torque += -transform.forward * (currentRoll * rollStabilization * 0.012f);

        rb.AddTorque(torque);
    }

    private void ClampSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void HandleCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}