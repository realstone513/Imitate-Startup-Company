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
        testPos = new Vector3(-6, 0, -6);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            props.Add(Instantiate(propPrefabs[0], testPos, Quaternion.identity, gameObject.transform));
            props[propIdx].name = propPrefabs[0].name + propIdx;
            propIdx++;
            testPos.x += 2;
            if (testPos.x > 5)
            {
                testPos.x = -6;
                testPos.z += 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            props.Add(Instantiate(propPrefabs[1], testPos, Quaternion.identity, gameObject.transform));
            props[propIdx].name = propPrefabs[1].name + propIdx;
            propIdx++;
            testPos.x += 2;
            if (testPos.x > 5)
            {
                testPos.x = -6;
                testPos.z += 2;
            }
        }
    }
}