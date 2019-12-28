using System;
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
        //переводит координаты экрана в координаты мира
        Vector3 bottom = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        float y = transform.position.y + Math.Abs(o.GetComponent<RectTransform>().position.y - bottom.y) - o.GetComponent<CircleCollider2D>().radius*1.3f;
        _targetPosition =
            new Vector3(o.transform.position.x, y, -10);
        _targetPosition.z = -10;
    }
}