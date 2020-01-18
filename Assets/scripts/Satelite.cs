using System;
using System.Collections;
using System.Collections.Generic;
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
    private float Speed = 3.1f;
    private CircleCollider2D _planetCollider;
    private CameraController _cameraController;

    //todo вынести в сервис
    public List<GameObject> PlanetPrefabs;
    private float _heightScreen;
    private float _widthScreen;

    void Start() {
        _sateliteTransform = (RectTransform) transform;
        _cameraController = Camera.main.gameObject.GetComponent<CameraController>();

        _heightScreen = 2.0f * Constants.cameraPozitionZ * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        _widthScreen = _heightScreen * Screen.width / Screen.height;
        _tail = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    void Update() {
        if (_onPlanet) {
            Vector3 rotation = new Vector3((float) Math.Cos(_angleRadian) * _flightAltitude,
                (float) Math.Sin(_angleRadian) * _flightAltitude);
            Vector3 nextPosition = Planet.transform.position + rotation;

            _sateliteTransform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, nextPosition, Time.deltaTime * Speed),
                Quaternion.AngleAxis((float) (_angleRadian * 180 / Math.PI - 90), Vector3.forward));
            _angleRadian += Time.deltaTime * Speed;
        }
        else {
            float step = Speed * 1.5f * Time.deltaTime;
            transform.position =
                Vector3.MoveTowards(transform.position, transform.position + _flightDirection * 10f, step);
        }

        if (Input.touchCount > 0) {
            Touch theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began) {
                _onPlanet = false;
                _flightDirection = _sateliteTransform.position - _planetTransform.position;
                _tail.gameObject.SetActive(true); 
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            _onPlanet = false;
            _flightDirection = _sateliteTransform.position - _planetTransform.position;
            _tail.gameObject.SetActive(true); 
        }
    }

    public void setPlanet(Planet planet, Vector3 contactPoint) {
        _planetCollider = planet.GetComponent<CircleCollider2D>();
        _planetCollider.isTrigger = false;
        Planet = planet;
        _planetTransform = (RectTransform) planet.transform;
        _flightAltitude = _planetCollider.radius;
        
        double horda = Math.Sqrt(Math.Pow(contactPoint.x - (_planetTransform.position.x + _planetCollider.radius), 2) +
                                 Math.Pow(contactPoint.y - _planetTransform.position.y, 2));
        double radians = 2 * Math.Asin(horda / (2 * _planetCollider.radius));
        bool needOffset = _sateliteTransform.position.y < _planetTransform.position.y;
        
        _angleRadian = (float) (needOffset ? Math.PI * 2 - radians : radians);
        _onPlanet = true;
        _cameraController.go(planet.gameObject);

        Vector3 position = planet.transform.position +
                           new Vector3(
                               Random.Range(-_widthScreen / 2 + _planetCollider.radius * 1.3f,
                                   _widthScreen / 3 - _planetCollider.radius * 1.3f),
                               Random.Range(_heightScreen / 3, _heightScreen - _planetCollider.radius * 7f), 0);

        GameObject nextPlanet = Instantiate(PlanetPrefabs[Random.Range(0, PlanetPrefabs.Count - 1)]);

        RectTransform rectTransform = nextPlanet.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        
        Vector2 closestPoint = collision.ClosestPoint(_sateliteTransform.position);
        Planet planet = collision.gameObject.GetComponent<Planet>();

        if (planet != null) {
            setPlanet(planet, closestPoint);
            _tail.gameObject.SetActive(false); 
        }
    }

    private void OnBecameInvisible() {
        ReloadLevel();
//        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    public bool gameOver = false;
    private ParticleSystem _tail;

    public void ReloadLevel() {
        gameOver = true;
        StartCoroutine(RestarCurrentLevel());
    }

    IEnumerator RestarCurrentLevel() {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}