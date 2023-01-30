using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour
{
    public List<GameObject> props;
    Vector3 testPos;

    private void Start()
    {
        testPos = new Vector3(-8, 0, -8);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Instantiate(props[0], testPos, Quaternion.identity, gameObject.transform);
            testPos.x += 3;
            if (testPos.x > 8)
            {
                testPos.x = -8;
                testPos.z += 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(props[1], testPos, Quaternion.identity, gameObject.transform);
            testPos.x += 3;
            if (testPos.x > 8)
            {
                testPos.x = -8;
                testPos.z += 3;
            }
        }
    }
}