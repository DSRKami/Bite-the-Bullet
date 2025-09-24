using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public float noiseSpeed = 1f;
    public float intensity = 1f;

    public Vector3 positionIntensity = new Vector3(0.02f, 0.02f, 0.02f); // How far the object moves
    public Vector3 rotationIntensity = new Vector3(0.2f, 0.2f, 0.2f);  // How much the object rotates

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time * noiseSpeed;

        // Generate Perlin noise-based offsets
        float xOffset = (Mathf.PerlinNoise(time, 0.0f) - 0.5f) * 2.0f * positionIntensity.x * intensity;
        float yOffset = (Mathf.PerlinNoise(0.0f, time) - 0.5f) * 2.0f * positionIntensity.y * intensity;
        float zOffset = (Mathf.PerlinNoise(time, time) - 0.5f) * 2.0f * positionIntensity.z * intensity;

        float xRot = (Mathf.PerlinNoise(time + 100.0f, 0.0f) - 0.5f) * 2.0f * rotationIntensity.x * intensity;
        float yRot = (Mathf.PerlinNoise(0.0f, time + 100.0f) - 0.5f) * 2.0f * rotationIntensity.y * intensity;
        float zRot = (Mathf.PerlinNoise(time + 100.0f, time + 100.0f) - 0.5f) * 2.0f * rotationIntensity.z * intensity;

        // Apply offsets to object's local position and rotation
        transform.localPosition = initialPosition + new Vector3(xOffset, yOffset, zOffset);
        transform.localRotation = initialRotation * Quaternion.Euler(xRot, yRot, zRot);
    }
}
