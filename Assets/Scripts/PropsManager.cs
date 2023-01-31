using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour
{
    public List<GameObject> propPrefabs;
    public List<GameObject> props;
    private int propIdx = 0;
    Vector3 testPos;

    private void Start()
    {
        testPos = new Vector3(-8, 0, -8);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            props.Add(Instantiate(propPrefabs[0], testPos, Quaternion.identity, gameObject.transform));
            props[propIdx].name = propPrefabs[0].name + propIdx;
            propIdx++;
            testPos.x += 5;
            if (testPos.x > 8)
            {
                testPos.x = -8;
                testPos.z += 5;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            props.Add(Instantiate(propPrefabs[1], testPos, Quaternion.identity, gameObject.transform));
            props[propIdx].name = propPrefabs[1].name + propIdx;
            propIdx++;
            testPos.x += 5;
            if (testPos.x > 8)
            {
                testPos.x = -8;
                testPos.z += 5;
            }
        }
    }
}