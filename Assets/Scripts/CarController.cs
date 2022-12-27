using System;
using UnityEngine;

public class CarController : MonoBehaviour {

    float driftFactor = 0.7f;
    float accelerationFactor = 0.5f;
    float turnFactor = 10f;
    float maxSpeed = 6.0f;

    float accelerationInput = 0;
    float steeringInput = 0;
    float steering = 0;
    float velocityVsUp = 0;

    bool carIsDrifting = false;
    float driftAngle;
    float timer;
    float driftDelay;

    Rigidbody2D carRB;

    void Awake() {
        carRB = GetComponent<Rigidbody2D>();
    }

    void Start() {
        timer = Time.fixedDeltaTime;
    }

    void Update() {
        
    }

    void FixedUpdate() {
        ApplyEngineForce();

        ApplyGrip();

        ApplySteering();

        timer += Time.fixedDeltaTime;
    }

    void ApplyEngineForce() {
        // Calculate velocity with the direction of the car (negative = reverse)
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
            carRB.drag = Mathf.Lerp(carRB.drag, 0.3f, Time.fixedDeltaTime);
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
        // If the car is moving slowly
        if (carRB.velocity.magnitude < 0.5f) {
            // Limit the car's ability to turn
            float minSpeedToTurn = (carRB.velocity.magnitude / 6);
            minSpeedToTurn = Mathf.Clamp01(minSpeedToTurn);

            // Update the rotation angle based on input
            steering = steeringInput * turnFactor * minSpeedToTurn;
        }
        else {
            // Update the rotation angle based on input
            steering = steeringInput * turnFactor / 8;
        }

        // Apply steering by rotating the car object
        carRB.AddTorque(steering);
    }

    void ApplyGrip() {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRB.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRB.velocity, transform.right);

        // How fast the car is moving sideways
        driftAngle = Vector2.Angle(forwardVelocity, carRB.velocity);

        // If the car is moving forwards and braking
        if (accelerationInput < 0 && velocityVsUp > 0) {
            // Start drifting
            carIsDrifting = true;
            driftDelay = timer;
        }

        // If the car has low side movement and 0.3s has passed
        if (Mathf.Abs(driftAngle) < 5 && (timer - driftDelay) > 0.3f) {
            // Stop drifting
            carIsDrifting = false;
        }

        if (carIsDrifting) {
            setDriftGrip();
        }
        else {
            setAsphaltGrip();
        }

        carRB.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    void setAsphaltGrip() {
        driftFactor = 0.7f;
        accelerationFactor = 0.5f;
        turnFactor = 70f;
        maxSpeed = 6.0f;
        carRB.angularDrag = 10;
    }

    void setDriftGrip() {
        driftFactor = 0.95f;
        accelerationFactor = driftAngle / 20;
        turnFactor = 70f;
        maxSpeed = 6.0f;
        carRB.angularDrag = 2;
    }

    public bool IsTireScreeching() {
        if (carIsDrifting) {
            return true;
        }
        return false;
    }

    public void SetInputVector(Vector2 inputVector) {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;

        // If the car is moving backwards
        if (velocityVsUp < 0) {
            // Invert steering
            steeringInput = -steeringInput;

            // Increase acceleration to imitate braking
            if (accelerationInput > 0) { 
                accelerationInput *= 5;
            }
        }
        else {
            // Increase acceleration to imitate braking
            if (accelerationInput < 0) {
                accelerationInput *= 5;
            }
        }
    }
}
