using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
	private RectTransform _rectTransform;
	private float angel;

	// Use this for initialization
	void Start () {
		_rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		angel += Time.deltaTime * Random.Range( -0.1f , 0.1f);
//		_rectTransform.Rotate(new Vector3(0, 0,  angel));
	}
}
