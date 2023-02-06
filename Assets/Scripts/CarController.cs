using System;
using UnityEditor;
using UnityEngine;

public class CarController : MonoBehaviour {

    float driftFactor;
    float accelerationFactor;
    float turnFactor;
    float maxSpeed;

    float accelerationInput = 0;
    float steeringInput = 0;
    float steering = 0;
    float velocityVsUp = 0;

    bool carIsDrifting = false;
    float driftAngle;
    float timer;
    float driftDelay;

    public string surface;
    SurfaceHandler surfaceHandler;

    Rigidbody2D carRB;

    bool controlsEnabled = false;
    float runTime = 0;

    void Awake() {
        surfaceHandler = GetComponentInChildren<SurfaceHandler>();
        carRB = GetComponent<Rigidbody2D>();
        surface = "Grass";
        setGrassGrip();
    }

    void Start() {
        timer = Time.fixedDeltaTime;
    }

    void Update() {
        if (controlsEnabled) {
            runTime += Time.deltaTime;
        }
    }

    void FixedUpdate() {
        surface = surfaceHandler.GetSurface();

        if (controlsEnabled) {
            ApplyEngineForce();
            ApplyGrip();
            ApplySteering();
        }
        else {
            StopTheCar();
        }

        timer += Time.fixedDeltaTime;
    }

    void ApplyEngineForce() {
        // Calculate velocity with the direction of the car (negative = reverse)
        velocityVsUp = Vector2.Dot(transform.up, carRB.velocity);

        // Limit forwards speed to the max speed
        if (velocityVsUp > maxSpeed && accelerationInput > 0) {
            carRB.AddForce(-carRB.velocity / 2);
            return;
        }

        // Limit reversing speed to 25% of max speed
        if (velocityVsUp < -maxSpeed * 0.25f && accelerationInput < 0) {
            carRB.AddForce(-carRB.velocity / 2);
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
            if (carIsDrifting || surface != "Asphalt") {
                // Update the rotation angle based on input, but limit steering more
                steering = steeringInput * turnFactor / Mathf.Lerp(16, 8, velocityVsUp / 5);
            }
            else {
                // Update the rotation angle based on input
                steering = steeringInput * turnFactor / 8;
            }
        }

        // Apply steering by rotating the car object
        carRB.AddTorque(steering);
    }

    void ApplyGrip() {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRB.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRB.velocity, transform.right);

        // How fast the car is moving sideways
        driftAngle = Vector2.Angle(forwardVelocity, carRB.velocity);

        // If the car has low side movement and 0.3s has passed
        if (Mathf.Abs(driftAngle) < 5 && (timer - driftDelay) > 0.3f) {
            // Stop drifting
            carIsDrifting = false;
        }

        if (surface == "Asphalt") {
            if (carIsDrifting) {
                setDriftGrip();
            }
            else {
                setAsphaltGrip();
            }
        }
        else if (surface == "Dirt") {
            carIsDrifting = true;
            setDirtGrip();
        }
        else if (surface == "Sand") {
            carIsDrifting = true;
            setSandGrip();
        }
        else if (surface == "Grass") {
            carIsDrifting = true;
            setGrassGrip();
        }

        carRB.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void setAsphaltGrip() {
        driftFactor = 0.7f;
        accelerationFactor = 0.9f;
        turnFactor = 0.8f;
        maxSpeed = 6f;
        carRB.angularDrag = 10;
        surface = "Asphalt";
    }

    public void setDriftGrip() {
        driftFactor = 0.95f;
        accelerationFactor = Mathf.Clamp(driftAngle / 20, 0.7f, 10f);
        turnFactor = 1f;
        maxSpeed = 6f;
        carRB.angularDrag = 3;
        surface = "Asphalt";
    }

    public void setDirtGrip() {
        driftFactor = 0.96f;
        accelerationFactor = Mathf.Clamp(driftAngle / 15, 0.7f, 10f);
        turnFactor = 1f;
        maxSpeed = 6f;
        carRB.angularDrag = 3;
    }

    public void setSandGrip() {
        driftFactor = 0.98f;
        accelerationFactor = Mathf.Clamp(driftAngle / 30, 0.7f, 7f);
        turnFactor = 0.5f;
        maxSpeed = 2f;
        carRB.angularDrag = 2;
        surface = "Sand";
    }

    public void setGrassGrip() {
        driftFactor = 0.97f;
        accelerationFactor = Mathf.Clamp(driftAngle / 15, 0.7f, 8f);
        turnFactor = 1f;
        maxSpeed = 3f;
        carRB.angularDrag = 2;
        surface = "Grass";
    }

    public bool IsTireScreeching() {
        if (surface == "Asphalt" && carIsDrifting == false) {
            return false;
        }
        return true;
    }

    public void SetInputVector(Vector2 inputVector) {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;

        InvertBackwardsSteering();

        HandleBraking(inputVector);
    }

    private void InvertBackwardsSteering() {
        if (velocityVsUp < 0) {
            steeringInput = -steeringInput;
        }
    }

    private void HandleBraking(Vector2 inputVector) {
        Vector2 brakeDirection = -carRB.velocity.normalized;
        // Going backwards
        if (velocityVsUp < -0.1f) {
            if (accelerationInput > 0) {
                accelerationInput = 0;
                carRB.AddForce(brakeDirection / 2);
                carIsDrifting = true;
                driftDelay = timer;
            }
            else {
                accelerationInput = inputVector.y;
            }
        }
        // Going forwards
        else if (velocityVsUp > 0.1f) {
            if (accelerationInput < 0) {
                accelerationInput = 0;
                carRB.AddForce(brakeDirection / 2);
                carIsDrifting = true;
                driftDelay = timer;
            }
            else {
                accelerationInput = inputVector.y;
            }
        }
        else {
            accelerationInput = inputVector.y;
            // Stop the car if there's no acceleration and it's moving very slowly
            if (accelerationInput == 0 && Mathf.Abs(carRB.velocity.magnitude) < 0.05f) {
                carRB.velocity = Vector2.zero;
            }
        }
    }

    public float GetCarSpeed() {
        return carRB.velocity.magnitude;
    }

    public float GetRunTime() {
        return runTime;
    }

    public bool ControlsAreEnabled() {
        return controlsEnabled;
    }

    public void EnableControls() {
        controlsEnabled = true;
    }

    public void DisableControls() {
        controlsEnabled = false;
    }

    private void StopTheCar() {
        float magnitude = Mathf.Abs(carRB.velocity.magnitude);
        if (magnitude > 0 && magnitude < 0.05f) {
            carRB.velocity = Vector2.zero;
        }
        else {
            carIsDrifting = true;
            carRB.AddForce(-carRB.velocity * 2);
        }
    }
}
