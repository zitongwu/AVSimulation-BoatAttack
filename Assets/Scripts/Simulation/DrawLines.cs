using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLines : MonoBehaviour
{

    bool enterPressed = false;
    public Camera perspectiveCam;
    public Camera orthographicCam;
    Camera currentCam;
    bool drawLineMode = false;
    LineRenderer lr;

    Vector3 mousePos;
    Vector3 pos;
    Vector3 previousPos;
    List<Vector3> linePositions;
    float minimumDistance = 0.1f;
    public Material mat;
    public GameObject canvas;
    public GameObject[] prefabs;
    GameObject selectedPrefab;
    GameObject newObj;
    bool newObjInstantiated = false;
    Color c1 = new Color(1, 0, 0, 0.5f);
    Color c2 = new Color(1, 0, 0, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        perspectiveCam.enabled = true;
        orthographicCam.enabled = false;
        currentCam = perspectiveCam;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            perspectiveCam.enabled = !perspectiveCam.enabled;
            orthographicCam.enabled = !orthographicCam.enabled;
            currentCam = (currentCam == perspectiveCam) ? orthographicCam : perspectiveCam;
            enterPressed = !enterPressed;
            canvas.SetActive(!canvas.activeSelf);

            if (!enterPressed)
            {
                drawLineMode = false;
                newObjInstantiated = false;
            }
        }

        if (drawLineMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                InstantiateObject();
                mousePos = Input.mousePosition;
                mousePos.z = 10f;
                pos = currentCam.ScreenToWorldPoint(mousePos);
                pos.y = selectedPrefab.transform.position.y;

                previousPos = pos;
                linePositions.Add(pos);
            }
            else if (Input.GetMouseButton(0))
            {
                mousePos = Input.mousePosition;
                mousePos.z = 10f;
                pos = currentCam.ScreenToWorldPoint(mousePos);
                pos.y = selectedPrefab.transform.position.y;

                float distance = Vector3.Distance(pos, previousPos);
                if (distance >= minimumDistance)
                {
                    previousPos = pos;
                    linePositions.Add(pos);
                    lr.positionCount = linePositions.Count;
                    lr.SetPositions(linePositions.ToArray());
                }
            }

            if (Input.GetMouseButtonDown(1) && newObjInstantiated)
            {
                newObj.transform.position = lr.GetPosition(0);
                drawLineMode = false;
                newObjInstantiated = false;
                newObj.AddComponent<Navigation>();
                Navigation nav = newObj.GetComponent<Navigation>();
                Vector3[] linePositions = new Vector3[lr.positionCount];
                lr.GetPositions(linePositions);
                nav.wayPoints = linePositions;
            }

        }
    }

    void InstantiateObject()
    {
        newObj = Instantiate(selectedPrefab, selectedPrefab.transform.position, Quaternion.identity);
        newObj.transform.parent = this.transform;
        newObj.AddComponent<LineRenderer>();
        lr = newObj.GetComponent<LineRenderer>();
        lr.material = mat;
        //lr.SetColors(c1, c2);
        lr.SetWidth(0.05f, 0.05f);
        linePositions = new List<Vector3>();
        lr.positionCount = linePositions.Count;
        lr.SetPositions(linePositions.ToArray());
        newObjInstantiated = true;
    }

    public void ButtonClick(int i)
    {
        selectedPrefab = prefabs[i];
        drawLineMode = true;
    }


}
