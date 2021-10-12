using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    // Script to constatnly move the platform between waypoints
    [SerializeField] private Transform[] wPoints;
    [SerializeField] private float closeEnoughDistance = 1f;
    [SerializeField] private float movementSpeed = 5f;

    private int currentWPointIndex = 0;

    private void Update() {
        // Use move towards function to step towards the target
        transform.position = Vector3.MoveTowards(
            transform.position,
            wPoints[currentWPointIndex].position,
            movementSpeed * Time.deltaTime);

        // Check if at a waypoint
        if(Vector3.Distance(transform.position, wPoints[currentWPointIndex].position) < closeEnoughDistance) {
            currentWPointIndex++;
            if(currentWPointIndex >= wPoints.Length) {
                currentWPointIndex = 0;
            }
        }
    }
}
