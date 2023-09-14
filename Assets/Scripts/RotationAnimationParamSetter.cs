using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    internal class RotationAnimationParamSetter: MonoBehaviour
    {
        private Animator animator;

        [SerializeField]
        private Transform rotationController;

        [SerializeField]
        private string xParamName = "x";
        [SerializeField]
        private string yParamName = "y";

        private int xHash;
        private int yHash;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            xHash = Animator.StringToHash(xParamName);
            yHash = Animator.StringToHash(yParamName);
        }

        private void Update()
        {
            animator.SetFloat(xHash, rotationController.up.x);
            animator.SetFloat(yHash, rotationController.up.y);
        }
    }
}
