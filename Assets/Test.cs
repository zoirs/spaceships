using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.down * Time.deltaTime * 0.5f,0.5f);

	}
	
	void OnTriggerEnter2D(Collider2D collision) {
//		print("коллизии2" + collision);

//        foreach (ContactPoint contact in collision.contacts) {
//            Debug.DrawRay(contact.point, contact.normal, Color.white);
//            Planet planet = contact.otherCollider.gameObject.GetComponent<Planet>();
//            if (planet != null) {
//                setPlanet(planet);
//            }
//        }
        
	}
    
	void OnCollisionEnter2D(Collision2D other){
//		print("коллизии1" + other);

	}
}
