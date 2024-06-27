using System.Collections;
using UnityEngine;

namespace BiblicallyAccurateLasers
{
    internal class LaserEye : MonoBehaviour
    {
        private GameObject eyeBeamGlow;
        private GameObject laser;
        private PlayMakerFSM laserFsm;

        private bool isFiring = false;
        private float time = 0;

        private static Settings settings = BiblicallyAccurateLasers.Instance.settings;

        /*private float _anticTime = 0.5f;
        private float _fireTime = 0.15f;
        private float _cooldown = 3f;*/

        private Vector3 targetPosition;
        
        /*public void SetTiming(float totalTime, int totalEyes, int index)
        {
            if (totalTime < anticTime + fireTime)
            {
                BiblicallyAccurateLasers.Instance.LogError("Total time passed into SetTiming less than time it takes to fire laser.");
                return;
            }

            cooldown = totalTime - (anticTime + fireTime);

            float delay = (totalTime / totalEyes) * index;
            time = cooldown - delay;
        }*/

        /*public void SetTiming(float anticTime, float fireTime, float sleepTime, float delay)
        {
            _anticTime = anticTime;
            _fireTime = fireTime;
            _cooldown = sleepTime;
            time = -delay;
        }*/

        public void DelayLaserBy(float delay)
        {
            time = -delay;
        }

        void Awake()
        {
            eyeBeamGlow = Instantiate(BiblicallyAccurateLasers._gameObjects["Eye Beam Glow"], transform);
            laser = eyeBeamGlow.transform.Find("Ascend Beam").gameObject;
            laserFsm = laser.LocateMyFSM("Control");
        }

        void Start()
        {
            eyeBeamGlow.transform.localPosition = new Vector3(-1.2f, 0f, 0f);
            eyeBeamGlow.transform.rotation = Quaternion.identity;
            //eyeBeamGlow.transform.localScale = Vector3.one / (0.24f);

            laser.SetActive(true);
            laserFsm.SetState("Inert");
        }

        void Update()
        {
            time += Time.deltaTime;

            if (time > settings.cooldown && !isFiring)
            {
                isFiring = true;
                StartCoroutine(LaserCycle());
            }
            else if (isFiring)
            {
                AimLaser(targetPosition);
            }

        }

        private void AimLaser(Vector3 targetPos)
        {
            float rotation = Mathf.Atan2(targetPos.y - transform.position.y, targetPos.x - transform.position.x) * (180 / Mathf.PI);
            Vector3 euler_rot = laser.transform.rotation.eulerAngles;
            euler_rot.z = rotation;
            laser.transform.rotation = Quaternion.Euler(euler_rot);
        }

        IEnumerator LaserCycle()
        {
            eyeBeamGlow.SetActive(true);

            targetPosition = HeroController.instance.transform.position;
            
            laserFsm.SendEvent("ANTIC");
            yield return new WaitForSeconds(settings.anticTime);
            laserFsm.SendEvent("FIRE");
            yield return new WaitForSeconds(settings.fireTime);
            laserFsm.SendEvent("END");

            eyeBeamGlow.SetActive(false);
            time = 0;
            isFiring = false;
        }
    }
}
