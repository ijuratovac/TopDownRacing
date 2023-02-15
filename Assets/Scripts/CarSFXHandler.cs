using UnityEngine;

public class CarSFXHandler : MonoBehaviour {

    [Header("Audio sources")]
    public AudioSource tiresScreeching;
    public AudioSource engine;
    public AudioSource carHit;
    public AudioSource grassDrift;
    public AudioSource dirtDrift;
    public AudioSource sandDrift;

    CarController carController;
    SurfaceHandler surfaceHandler;
    string currentSurface;

    private float pitchRange = 0.3f;
    private float maxSpeed = 150.0f;
    private int numGears = 3;
    private float[] gearSpeeds;
    private float[] gearPitchValues;

    private int currentGear = 0;
    private float currentSpeed = 0;
    private float currentPitch = 0;

    void Awake() {
        carController = GetComponent<CarController>();
        surfaceHandler = GetComponentInChildren<SurfaceHandler>();
    }

    private void Start() {
        // Initialize the gear speeds and pitch values
        gearSpeeds = new float[numGears];
        gearPitchValues = new float[numGears];

        // Calculate the gear speeds and pitch values based on the max speed and pitch range
        for (int i = 0; i < numGears; i++) {
            gearSpeeds[i] = maxSpeed / numGears * (i + 1);
            gearPitchValues[i] = 1.2f + (pitchRange / numGears * i * 2);
        }
    }

    void Update() {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX() {
        currentSpeed = carController.GetCarSpeed() * 20;

        float desiredEngineVolume = currentSpeed * 0.05f;
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1f);

        engine.volume = Mathf.Lerp(engine.volume, desiredEngineVolume, Time.deltaTime * 10);

        // Calculate the current gear of the car based on the current speed
        for (int i = 0; i < numGears; i++) {
            if (currentSpeed < gearSpeeds[i]) {
                currentGear = i;
                break;
            }
        }

        // Calculate the current pitch of the car engine sound based on the current gear and speed
        float gearSpeedRange = gearSpeeds[currentGear] / numGears;
        float speedOffset = currentSpeed - (gearSpeedRange * currentGear);
        float pitchOffset = speedOffset / gearSpeedRange * pitchRange;
        currentPitch = gearPitchValues[currentGear] + pitchOffset;

        // Set the pitch of the audio source to the current pitch
        engine.pitch = currentPitch;
    }

    void UpdateTiresScreechingSFX() {
        currentSurface = surfaceHandler.GetSurface();
        HandleDriftSounds("Asphalt", tiresScreeching);
        HandleDriftSounds("Grass", grassDrift);
        HandleDriftSounds("Dirt", dirtDrift);
        HandleDriftSounds("Sand", sandDrift);
    }

    void HandleDriftSounds(string surface, AudioSource audioSource) {
        if (carController.IsTireScreeching() && currentSurface == surface) {
            audioSource.volume = Mathf.Clamp01(Mathf.Pow(currentSpeed, 2) / 3000);
        }
        else {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * 10);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        carHit.pitch = Random.Range(0.8f, 1.2f);
        carHit.volume = collision.relativeVelocity.magnitude * 0.1f;

        if (!carHit.isPlaying) {
            carHit.Play();
        }
    }
}
