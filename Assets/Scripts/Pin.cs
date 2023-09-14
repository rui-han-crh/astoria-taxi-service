using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class Pin : MonoBehaviour
    {
        [SerializeField]
        private Transform pinnedParent;

        [Tooltip("Rotates the offset at each degree interval, 0 for no rotation")]
        public float rotatePerDegrees = 0;

        private Vector3 offset;

        // Start is called before the first frame update
        void Start()
        {
            offset = transform.position - pinnedParent.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 currentOffset = offset;

            if (rotatePerDegrees != 0)
            {
                float closestMultipleOf90 = Mathf.Round(pinnedParent.eulerAngles.z / 90f) * 90f;
                float angleDifference = Mathf.Abs(pinnedParent.eulerAngles.z - closestMultipleOf90);

                if (angleDifference < rotatePerDegrees / 2)
                {
                    // Rotate the offset around the parent
                    currentOffset = Quaternion.Euler(0, 0, closestMultipleOf90) * offset;
                }
                print(closestMultipleOf90);
                print(currentOffset);
            }

            transform.position = pinnedParent.position + currentOffset;
        }
    }
}
