using UnityEngine;

public class RotatingPlatform : MonoBehaviour {
    // Script to constantly rotate the platform
    [SerializeField] private float rotationSpeed = 5f;

    private float rotationAngle = 0f;

    private void Update() {
        rotationAngle += rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0.0f, rotationAngle, 0.0f);
    }
}
