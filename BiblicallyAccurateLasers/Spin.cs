using UnityEngine;

namespace BiblicallyAccurateLasers
{
    internal class Spin : MonoBehaviour
    {

        private float rot;
        private float speed = 1f;

        public void Start()
        {
            rot = transform.localRotation.eulerAngles.z;
        }

        public void Update()
        {
            rot += speed * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(0, 0, rot);
        }

        public void SetSpeed(float s)
        {
            speed = s;
        }
    }
}
