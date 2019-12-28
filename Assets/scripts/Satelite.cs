using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Satelite : MonoBehaviour {

    private float _angleRadian;
    public Planet Planet;
    private RectTransform _sateliteTransform;
    private RectTransform _planetTransform;
    
    //Высота полета
    private float _flightAltitude;
    //Направление движение от платеты
    private Vector3 _flightDirection = Vector3.right;

    private bool _onPlanet = false;
    public float Speed = 2.5f;
    private CircleCollider2D _planetCollider;
    private CameraController _cameraController;
    
    
    //todo вынести в сервис
    public GameObject PlanetPrefab;
    private float _heightScreen;
    private float _widthScreen;

    void Start() {
        _sateliteTransform = (RectTransform) transform;
        _cameraController = Camera.main.gameObject.GetComponent<CameraController>();
        
        _heightScreen = 2.0f * Constants.cameraPozitionZ * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        _widthScreen = _heightScreen * Screen.width / Screen.height;
    }

    void Update() {
        if (_onPlanet) {
            _angleRadian += Time.deltaTime * Speed;
            Vector3 rotation = new Vector3((float) Math.Cos(_angleRadian) * _flightAltitude, (float) Math.Sin(_angleRadian) * _flightAltitude);
            Vector3 nextPosition = Planet.transform.position + rotation;

            transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, nextPosition, 20f),
                Quaternion.AngleAxis((float) (_angleRadian * 180 / Math.PI - 90), Vector3.forward));
        } else {
            float step = Speed * 1.5f * Time.deltaTime;
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

        _angleRadian = (float) (radians + offset);
        _cameraController.go(planet.gameObject);

        Vector3 position = planet.transform.position + 
                           new Vector3(Random.Range(-_widthScreen/2 +_planetCollider.radius*1.3f, _widthScreen/2-_planetCollider.radius*1.3f), 
                               Random.Range( _heightScreen / 2 , _heightScreen - _planetCollider.radius*2.3f), 0);
        
        GameObject nextPlanet = Instantiate(PlanetPrefab);
        RectTransform rectTransform = nextPlanet.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Planet planet = collision.gameObject.GetComponent<Planet>();

        if (planet != null) {
            setPlanet(planet);
        }
    }
}