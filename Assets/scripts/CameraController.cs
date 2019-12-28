using UnityEngine;

public class CameraController : MonoBehaviour {
    private float _moveSpeed = 0.2f;

    private Vector3 _targetPosition;

    void Start() {
        _targetPosition = transform.position;
        _targetPosition.z = -10;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed);
    }

    public void go(GameObject o) {
        _targetPosition = o.transform.position;
        _targetPosition.z = -10;
    }
}