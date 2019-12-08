using System;
using UnityEngine;

public class Satelite : MonoBehaviour {
//    private Rigidbody2D _tr;
    private float _deltaTime;
    public Planet Planet;
    private RectTransform _rectTransform;
    private RectTransform _planetTransform;
    private float _rectWidth;
    private float _rectHeight;
    private Vector3 _target;

    private bool _onPlanet = true;

    // Use this for initialization
    void Start() {
//        _tr = GetComponent<Rigidbody2D>();
        _rectTransform = (RectTransform) transform;
        _planetTransform = (RectTransform) Planet.transform;
        _rectWidth = _rectTransform.rect.width + _planetTransform.rect.width;
        _rectHeight = _rectTransform.rect.height + _planetTransform.rect.height;
    }

    // Update is called once per frame
    void Update() {
        if (_onPlanet) {
            _deltaTime += Time.deltaTime * 0.5f;
            Vector3 vector3 = Planet.transform.position + new Vector3((float) Math.Cos(_deltaTime) * _rectWidth / 2, (float) Math.Sin(_deltaTime) * _rectWidth / 2);
            transform.position = Vector3.MoveTowards(transform.position, vector3, 0.5f);
//            var movePosition = _tr.MovePosition(Planet.transform.position + new Vector3((float) Math.Cos(_deltaTime) * _rectWidth / 2,
//                                                    (float) Math.Sin(_deltaTime) * _rectWidth / 2));
        }
        else {
            float step = 0.5f * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _target * Time.deltaTime * 0.5f,0.5f);
//            _tr.MovePosition(transform.position + _target * Time.deltaTime * 0.5f);
//            transform.position = Vector3.MoveTowards(transform.position, transform.position + _target, step);
//            print(transform.position + " " + _target);

        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            _onPlanet = false;
            _target = _rectTransform.position - _planetTransform.position;
        }
    }

    public void setPlanet(Planet planet) {
        Planet = planet;
        _planetTransform = (RectTransform) planet.transform;
        _rectWidth = _rectTransform.rect.width + _planetTransform.rect.width;
        _rectHeight = _rectTransform.rect.height + _planetTransform.rect.height;
        _onPlanet = true;
//        Planet.GetComponent<BoxCollider2D>().enabled = false;
    }
    
    void OnTriggerEnter2D(Collider2D collision) {
        print("коллизии1" + collision);

            Planet planet = collision.gameObject.GetComponent<Planet>();
        print("коллизии1 pl" + planet);

        if (planet != null) {
            setPlanet(planet);
        }

    }
    
//    void OnCollisionEnter2D(Collision2D other){
//        print("коллизии2" + other);
//
//    }
}