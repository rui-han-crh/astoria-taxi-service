using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Pin : MonoBehaviour
{
    [SerializeField]
    private Transform pinnedParent;

    [SerializeField]
    private bool useOffset = false;

    private Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        offset = transform.position - pinnedParent.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (useOffset)
        {
            transform.position = pinnedParent.position + offset;
        }
        else
        {
            transform.position = pinnedParent.position;
        }
    }
}
