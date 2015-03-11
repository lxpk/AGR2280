using UnityEngine;
using System.Collections;

public class ShipAVManager : MonoBehaviour {

    // Audio
    public AudioSource audioEngineSource;
    public AudioSource audioAirbrakeLeftSource;
    public AudioSource audioAirbrakeRightSource;

    // Visual
    private float visualLightItensity;
    private float idleLightFlickerSpeed = 100;
    private float idleLightFlickerAmount = 2f;
    private float idleLightFlickerTimer;

    private float visEngineTrailOpacity;
    private float visEngineTrailStartSize;
    private float visEngineTrailEndSize;
    private float visEngineTrailBrightness;


    void Start()
    {
        // Create engine audio
        GameObject engineAudio = new GameObject("Engine Audio");
        engineAudio.transform.parent = transform;
        engineAudio.transform.localPosition = Vector3.zero;
        audioEngineSource = engineAudio.AddComponent<AudioSource>();
        audioEngineSource.clip = GetComponent<ShipSimulator>().settings.audioEngine;
        audioEngineSource.spatialBlend = 1;
        audioEngineSource.minDistance = 100;
        audioEngineSource.maxDistance = 500;
        audioEngineSource.dopplerLevel = 0;
        audioEngineSource.loop = true;
        audioEngineSource.volume = 0;
        audioEngineSource.pitch = 1;
        audioEngineSource.Play();
    }

    void Update()
    {
        // Get references to needed classes
        ShipController controller = GetComponent<ShipController>();
        ShipSimulator simulator = GetComponent<ShipSimulator>();

        // Engine Pitch and Volume
        float engineMax = 1.1f;
        if (simulator.bShipIsBoosting)
        {
            engineMax = 1.4f;
            
            // Cheeky engine light intensity change :^)
            visualLightItensity = 5.0f;
        }

        float wantedPitch = Mathf.Clamp(transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z * Time.deltaTime, 0.5f, engineMax);
        audioEngineSource.pitch = Mathf.Lerp(audioEngineSource.pitch, wantedPitch, Time.deltaTime * 2);

        // Volume Control
        if (controller.btnThruster)
        {
            audioEngineSource.volume = Mathf.Lerp(audioEngineSource.volume, 1.0f, Time.deltaTime * 2);
        } else
        {
            audioEngineSource.volume = Mathf.Lerp(audioEngineSource.volume, 0.0f, Time.deltaTime);
        }

        // Light Control
        if (controller.btnThruster)
        {
            idleLightFlickerTimer = 0;
            visualLightItensity = Mathf.Lerp(visualLightItensity, 3.0f, Time.deltaTime);
            if (visualLightItensity < 1.5f)
                visualLightItensity = 1.5f;
        } else
        {
            visualLightItensity = Mathf.Lerp(visualLightItensity, 0.0f, Time.deltaTime);
            if (visualLightItensity < 1.5f)
            {
                idleLightFlickerTimer += Time.deltaTime * idleLightFlickerSpeed;
                visualLightItensity = Mathf.Sin(idleLightFlickerTimer) * idleLightFlickerAmount;
            }
        }
        simulator.settings.refEngineLight.GetComponent<Light>().intensity = visualLightItensity;

        // Engine Trail
        if (controller.btnThruster)
        {
            if (simulator.bShipIsBoosting)
            {
                visEngineTrailStartSize = Mathf.Lerp(visEngineTrailStartSize, 0.8f, Time.deltaTime * 12.0f);
                visEngineTrailEndSize = Mathf.Lerp(visEngineTrailEndSize, 6f, Time.deltaTime * 12.0f);
                visEngineTrailOpacity = Mathf.Lerp(visEngineTrailOpacity, 1.0f, Time.deltaTime * 5);
            } else
            {
                if (transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z > 50)
                {
                    visEngineTrailStartSize = Mathf.Lerp(visEngineTrailStartSize, 0.5f, Time.deltaTime * 6.0f);
                    visEngineTrailEndSize = Mathf.Lerp(visEngineTrailEndSize, 5.0f, Time.deltaTime * 6.0f);
                    visEngineTrailOpacity = Mathf.Lerp(visEngineTrailOpacity, 0.5f, Time.deltaTime * 5.0f);
                } else
                {
                    visEngineTrailStartSize = Mathf.Lerp(visEngineTrailStartSize, 0.0f, Time.deltaTime * 6.0f);
                    visEngineTrailEndSize = Mathf.Lerp(visEngineTrailEndSize, 0.0f, Time.deltaTime * 6.0f);
                    visEngineTrailOpacity = Mathf.Lerp(visEngineTrailOpacity, 0.0f, Time.deltaTime * 2.0f);
                }
            }
        } else
        {
            visEngineTrailStartSize = Mathf.Lerp(visEngineTrailStartSize, 0.50f, Time.deltaTime * 6.0f);
            visEngineTrailEndSize = Mathf.Lerp(visEngineTrailEndSize, 0.2f, Time.deltaTime * 6.0f);
            visEngineTrailOpacity = Mathf.Lerp(visEngineTrailOpacity, 0.0f, Time.deltaTime * 2.0f);
        }
        simulator.settings.refEngineTrail.GetComponent<TrailRenderer>().startWidth = visEngineTrailStartSize;
        simulator.settings.refEngineTrail.GetComponent<TrailRenderer>().endWidth = visEngineTrailEndSize;
        simulator.settings.refEngineTrail.GetComponent<TrailRenderer>().material.SetColor("_TintColor", new Color(1, 1, 1, visEngineTrailOpacity));
        Vector2 texOffset = simulator.settings.refEngineTrail.GetComponent<TrailRenderer>().material.GetTextureOffset("_MainTex");
        texOffset.x += Time.deltaTime * 10;
        simulator.settings.refEngineTrail.GetComponent<TrailRenderer>().material.SetTextureOffset("_MainTex", texOffset);
    }

    public void ThrusterUp()
    {
        PlayOneShot.PlayShot("ThrusterRelease", 1.0f, 1.0f, 200, 100, transform, transform.position, GetComponent<ShipSimulator>().settings.audioEngineCooler);
    }

    public void ThrusterDown()
    {
        PlayOneShot.PlayShot("ThrusterDown", 1.0f, 1.0f, 200, 100, transform, transform.position, GetComponent<ShipSimulator>().settings.audioEngineStartup);
        visualLightItensity = 5.0f;
    }
}
