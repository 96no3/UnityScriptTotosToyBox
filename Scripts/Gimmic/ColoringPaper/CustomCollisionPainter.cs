using UnityEngine;

namespace Es.InkPainter.Sample
{
    [RequireComponent(typeof(Collider), typeof(MeshRenderer))]
    public class CustomCollisionPainter : MonoBehaviour
    {
        [SerializeField]
        private Brush brush = null;

        [SerializeField]
        private int wait = 1;

        private int waitCount;

        public void Awake()
        {
            GetComponent<MeshRenderer>().material.color = brush.Color;
        }

        public void FixedUpdate()
        {
            ++waitCount;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ink")
            {
                brush.Color = other.GetComponent<Renderer>().material.color;
            }
        }

        public void OnCollisionStay(Collision collision)
        {
            if (waitCount < wait)
                return;
            waitCount = 0;

            foreach (var p in collision.contacts)
            {
                var canvas = p.otherCollider.GetComponent<InkCanvas>();
                if (canvas != null)
                {
                    canvas.Paint(brush, p.point);
                }
            }
        }
    }
}