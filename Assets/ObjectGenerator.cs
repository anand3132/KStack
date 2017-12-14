using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour{
    public Transform parentObject;
    public GameObject baseObject;
    public GameObject currentObject;
    public GameObject PrevObject;
    private Vector3 yValue = Vector3.zero;
    private GameObject initialObject;
    private Vector3 cubeObjectDirection = Vector3.forward;
    private Vector3 cubeObjectPosition = new Vector3(0f, 1.03f, -10f);
    private bool gameOver;
    //private Vector3 cubeObjectDirection = Vector3.left;
    //private Vector3 cubeObjectPosition=new Vector3(-10f, 1.03f, 0f);


    // Use this for initialization
    void Start() {
        initialObject = baseObject;
        generateObject(baseObject);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            PrevObject = currentObject;
            //PrevObject.name = "ff";
            //PrevObject.transform.SetParent(parentObject);
            currentObject.GetComponent<Cube>().ObjectMovement = false;
            sliceCube(currentObject);
            Destroy(currentObject);
            generateObject(PrevObject);
        }
        if (gameOver) {
            ResetGame();
        }
    }

    void ResetGame() {
        foreach (Transform child in parentObject) {
            Destroy(child.gameObject);
        }
        gameOver = false;
        yValue = Vector3.zero;
        generateObject(initialObject);
        baseObject = initialObject;
    }

    void generateObject(GameObject cubeObj) {
        currentObject = Instantiate(cubeObj, parentObject);
        currentObject.transform.position = cubeObjectPosition + yValue;
        currentObject.GetComponent<Cube>().objectDirection = cubeObjectDirection;
        currentObject.GetComponent<Renderer>().material.color = Color.red;
        currentObject.GetComponent<Cube>().ObjectMovement = true;
        yValue.y++;
    }

    void sliceCube(GameObject movingObject) {
        GameObject innerObject = Instantiate(baseObject, parentObject);
        GameObject outerObject = Instantiate(baseObject, parentObject);
        innerObject.name = "Inner";
        outerObject.name = "Outer";

        // if direction is positive then moving from rigth to left
        float direction = movingObject.transform.position.z - baseObject.transform.position.z;

        if (direction < 0) {
            direction = -1.0f;
        } else {
            direction = 1.0f;
        }

        float baseWidth = baseObject.GetComponent<BoxCollider>().bounds.size.z;
        float baseStartX = baseObject.transform.position.z + baseWidth * 0.5f * direction;
        float baseEndX = baseObject.transform.position.z - baseWidth * 0.5f * direction;

        float childWidth = movingObject.GetComponent<BoxCollider>().bounds.size.z;
        float childStartX = movingObject.transform.position.z + childWidth * 0.5f * direction;
        float childEndX = movingObject.transform.position.z - childWidth * 0.5f * direction;

        float outerObjWidth = Mathf.Abs(childStartX - baseStartX);
        float innerObjWidth = Mathf.Abs(childWidth - outerObjWidth);

        float outerObjPivotX = childStartX - outerObjWidth * 0.5f * direction;
        float outerObjEndX = outerObjPivotX - outerObjWidth * 0.5f * direction;
        float innerObjPivotX = outerObjEndX - innerObjWidth * 0.5f * direction;
        Debug.Log(baseStartX + "baseStartX" + baseEndX + "direction" + direction);

        if ((int)direction == 1 && baseStartX < childStartX && baseStartX < childEndX ||
            (int)direction == -1 && baseStartX > childStartX && baseStartX > childEndX  ) {
                movingObject.GetComponent<Rigidbody>().isKinematic = true;
                gameOver = true;
            }

        outerObject.transform.localScale = new Vector3(movingObject.transform.localScale.x,
                                                       movingObject.transform.localScale.y,
                                                       outerObjWidth);
        outerObject.transform.position = new Vector3(movingObject.transform.position.x,
                                                     movingObject.transform.position.y,
                                                     outerObjPivotX
                                                    );
        outerObject.GetComponent<Rigidbody>().isKinematic = false;

        innerObject.transform.localScale = new Vector3(movingObject.transform.localScale.x,
                                                        movingObject.transform.localScale.y,
                                                       innerObjWidth);
        innerObject.transform.position = new Vector3(movingObject.transform.position.x,
                                                     movingObject.transform.position.y,
                                                     innerObjPivotX
                                                    );
        PrevObject = innerObject;
        baseObject = innerObject;
    }
}
