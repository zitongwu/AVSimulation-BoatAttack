using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USVController : MonoBehaviour
{
    float m_Speed = 7f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            transform.position += transform.up * m_Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.up * m_Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * m_Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * m_Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * m_Speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * m_Speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.O))
        {
            transform.Rotate(Vector3.up, -40 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.P))
        {
            transform.Rotate(Vector3.up, 40 * Time.deltaTime);
        }
    }
}
