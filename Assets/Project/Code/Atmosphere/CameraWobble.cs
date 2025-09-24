using UnityEngine;

public class CameraWobble : MonoBehaviour
{
    public CameraTurnController cameraTurnController;
    public float noiseSpeed = 1f;
    public float positionAmount = 0.02f; // How far the camera moves
    public float rotationAmount = 0.2f;  // How much the camera rotates

    [Range(0f, 2f)]
    public float intensity = 1f;

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
        if (cameraTurnController.isBlending) return;
        float time = Time.time * noiseSpeed;

        // Generate Perlin noise-based offsets
        float xOffset = (Mathf.PerlinNoise(time, 0.0f) - 0.5f) * 2.0f * positionAmount * intensity;
        float yOffset = (Mathf.PerlinNoise(0.0f, time) - 0.5f) * 2.0f * positionAmount * intensity;
        float zOffset = (Mathf.PerlinNoise(time, time) - 0.5f) * 2.0f * positionAmount * intensity;

        float xRot = (Mathf.PerlinNoise(time + 100.0f, 0.0f) - 0.5f) * 2.0f * rotationAmount * intensity;
        float yRot = (Mathf.PerlinNoise(0.0f, time + 100.0f) - 0.5f) * 2.0f * rotationAmount * intensity;
        float zRot = (Mathf.PerlinNoise(time + 100.0f, time + 100.0f) - 0.5f) * 2.0f * rotationAmount * intensity;

        // Apply offsets to camera's local position and rotation
        transform.localPosition = initialPosition + new Vector3(xOffset, yOffset, zOffset);
        transform.localRotation = initialRotation * Quaternion.Euler(xRot, yRot, zRot);
    }

    public void ResetBaseTransform()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }
}
