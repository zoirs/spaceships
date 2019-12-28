using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Satelite : MonoBehaviour {

    private float _angleRadian;
    public Planet Planet;
    private RectTransform _sateliteTransform;
    private RectTransform _planetTransform;
    
    //Высота полета
    private float _flightAltitude;
    //Направление движение от платеты
    private Vector3 _flightDirection;

    private bool _onPlanet = true;
    private float _speed = 2f;
    private CircleCollider2D _planetCollider;
    private CameraController _cameraController;

    void Start() {
        _sateliteTransform = (RectTransform) transform;
        _cameraController = Camera.main.gameObject.GetComponent<CameraController>();
        setPlanet(Planet);
        print(Camera.main.name);
    }

    void Update() {
        if (_onPlanet) {
            _angleRadian += Time.deltaTime * _speed;
            Vector3 rotation = new Vector3((float) Math.Cos(_angleRadian) * _flightAltitude, (float) Math.Sin(_angleRadian) * _flightAltitude);
            Vector3 nextPosition = Planet.transform.position + rotation;

            transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, nextPosition, 20f),
                Quaternion.AngleAxis((float) (_angleRadian * 180 / Math.PI - 90), Vector3.forward));
        } else {
            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _flightDirection * 10f, step);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            _onPlanet = false;
            _flightDirection = _sateliteTransform.position - _planetTransform.position;
        }
    }

    public void setPlanet(Planet planet) {
        Planet = planet;
        _planetTransform = (RectTransform) planet.transform;
        _planetCollider = planet.GetComponent<CircleCollider2D>();
        _flightAltitude = _sateliteTransform.rect.width / 2 + _planetCollider.radius;
        _onPlanet = true;
        double radiunCollision = Math.Sqrt(Math.Pow(_sateliteTransform.position.x - _planetTransform.position.x, 2) +
                                           Math.Pow(_sateliteTransform.position.y - _planetTransform.position.y, 2));
        double horda = Math.Sqrt(Math.Pow(_sateliteTransform.position.x - (_planetTransform.position.x + radiunCollision),2)  + Math.Pow(_sateliteTransform.position.y - _planetTransform.position.y ,2));
        double radians = 2 * Math.Asin(horda / (2 * radiunCollision));
        double offset = _sateliteTransform.position.y < _planetTransform.position.y
            ? (_sateliteTransform.position.x < _planetTransform.position.x ? Math.PI/2 : -Math.PI/2)
            : 0; // в 3 четверти вычесть 90 градусов, в 4 четверти прибавить 90 градусов
        print("угол " +radians * 180 /Math.PI + " offset " + offset);
        _angleRadian = (float) (radians + offset);
        _cameraController.go(planet.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Planet planet = collision.gameObject.GetComponent<Planet>();

        if (planet != null) {
            setPlanet(planet);
        }
    }
}