using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

	private static float delta = 29.8f;
//	private float speed;
	public Transform cam;

	public Transform back1;
	public Transform back2;

	private bool whichone = true;
	private float currentHight=delta;

	void Update () {
		if (currentHight < cam.position.y) {
			if (whichone) {
				back1.localPosition = new Vector3(0,back1.localPosition.y + delta*2,0);
			}
			else {
				back2.localPosition = new Vector3(0,back2.localPosition.y + delta*2,0);				
			}

			currentHight += delta;
			whichone = !whichone;
		}
		if (currentHight > cam.position.y + delta) {
			if (whichone) {
				back2.localPosition = new Vector3(0,back2.localPosition.y - delta*2,0);
			}
			else {
				back1.localPosition = new Vector3(0,back1.localPosition.y - delta*2,0);				
			}

			currentHight -= delta;
			whichone = !whichone;
		}
	}
}