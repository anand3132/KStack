using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
	public float timeDelay=1;
	public float speed=30;
	private int  ground=-5;
	public bool ObjectMovement=true;

    private Vector3 eulerAngle=new Vector3(0,180,0);
    public Vector3 objectDirection;

    void Start () {
		StartCoroutine("SwitchAngle");
	}

    // Update is called once per frame
    void Update()  {
        if (ObjectMovement) {
            transform.Translate(objectDirection * speed * Time.deltaTime);
        }
        if (transform.position.y < ground) {
            Destroy(gameObject);
        }
    }

	IEnumerator SwitchAngle(){
        while (ObjectMovement){
			yield return new WaitForSeconds(timeDelay);
            objectDirection = -objectDirection;
		}
	}
}
