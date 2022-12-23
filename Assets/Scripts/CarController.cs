using UnityEngine;

public class CarController : MonoBehaviour {

    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20.0f;

    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    Rigidbody2D carRB;

    void Awake() {
        carRB = GetComponent<Rigidbody2D>();
    }

    void Start() {

    }

    void Update() {
        
    }

    void FixedUpdate() {
        ApplyEngineForce();

        ApplyGrip();

        ApplySteering();
    }

    void ApplyEngineForce() {
        // Calculate how much forward we are going in terms of the direction of our velocity
        velocityVsUp = Vector2.Dot(transform.up, carRB.velocity);

        // Limit so we cannot drive forwards faster than the max speed
        if (velocityVsUp > maxSpeed && accelerationInput > 0) {
            return;
        }

        // Limit so we cannot drive reverse faster than 20% of max speed
        if (velocityVsUp < -maxSpeed * 0.2f && accelerationInput < 0) {
            return;
        }

        // Limit so we cannot go faster in any direction while accelerating
        if (carRB.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) {
            return;
        }

        // Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (accelerationInput == 0) {
            carRB.drag = Mathf.Lerp(carRB.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else {
            carRB.drag = 0;
        }

        // Create a force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        // Apply force and push the car forward
        carRB.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering() {
        // Limit the car's ability to turn when moving slowly
        float minSpeedToTurn = (carRB.velocity.magnitude / 8);
        minSpeedToTurn = Mathf.Clamp01(minSpeedToTurn);

        // Update the rotation angle based on input
        rotationAngle -= steeringInput * turnFactor * minSpeedToTurn;

        // Apply steering by rotating the car object
        carRB.MoveRotation(rotationAngle);
    }

    void ApplyGrip() {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRB.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRB.velocity, transform.right);

        carRB.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector) {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
