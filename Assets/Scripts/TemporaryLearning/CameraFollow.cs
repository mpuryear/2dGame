using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject m_Camera;
    Transform m_TransformToFollow;

    // Start is called before the first frame update
    void Start()
    {
        m_TransformToFollow = transform;
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera");    
    }

    // Update is called once per frame
    void LateUpdate()
    {
        m_Camera.transform.position = new Vector3(m_TransformToFollow.position.x, m_TransformToFollow.position.y, m_Camera.transform.position.z);
    }
}
