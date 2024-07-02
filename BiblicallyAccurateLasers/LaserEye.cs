using System.Collections;
using UnityEngine;

namespace BiblicallyAccurateLasers
{
    internal class LaserEye : MonoBehaviour
    {
        private bool isActive = true;

        private GameObject eyeBeamGlow;
        private GameObject laser;
        private PlayMakerFSM laserFsm;
        private SpriteRenderer eyeSprite;

        private bool isFiring = false;
        private float time = 0;

        private static readonly Settings settings = BiblicallyAccurateLasers.Instance.settings;

        private Vector3 targetPosition;
        public GameObject targetMarker = new("Target Marker");
        
        public void DelayLaserBy(float delay)
        {
            time = -delay;
        }

        public void SetActive(bool active)
        {
            isActive = active;
            eyeSprite.enabled = active;
            targetMarker.GetComponent<SpriteRenderer>().enabled = active;
        }

        void Awake()
        {
            eyeBeamGlow = Instantiate(BiblicallyAccurateLasers._gameObjects["Eye Beam Glow"], transform);
            laser = eyeBeamGlow.transform.Find("Ascend Beam").gameObject;
            laserFsm = laser.LocateMyFSM("Control");
            eyeSprite = gameObject.GetComponent<SpriteRenderer>();
            
            targetMarker.AddComponent<SpriteRenderer>().sprite = BiblicallyAccurateLasers.GetSprite(TextureStrings.TargetKey);
            targetMarker.transform.localScale = Vector3.one * 0.5f;
            targetMarker.AddComponent<Spin>().SetSpeed(0.25f);
        }

        void Start()
        {
            eyeBeamGlow.transform.localPosition = new Vector3(-1.2f, 0f, 0f);
            eyeBeamGlow.transform.rotation = Quaternion.identity;
            //eyeBeamGlow.transform.localScale = Vector3.one / (0.24f);

            var ls = laser.transform.localScale;
            ls.x *= 1 / 0.24f;
            laser.transform.localScale = ls;

            laser.SetActive(true);
            laserFsm.SetState("Inert");
        }

        void Update()
        {
            time += Time.deltaTime;

            if (time > settings.cooldown)
            {
                if (!isFiring)
                {
                    isFiring = true;
                    if (isActive)
                        StartCoroutine(LaserCycle());
                    else
                        StartCoroutine(EmptyCycle());
                }
                else AimLaser(targetPosition);
            }

            /*if (time > settings.cooldown && !isFiring)
            {
                isFiring = true;
                StartCoroutine(LaserCycle());
            }
            else if (isFiring)
            {
                AimLaser(targetPosition);
            }*/

        }

        private void AimLaser(Vector3 targetPos)
        {
            float rotation = Mathf.Atan2(targetPos.y - laser.transform.position.y, targetPos.x - laser.transform.position.x) * (180 / Mathf.PI);
            Vector3 euler_rot = laser.transform.rotation.eulerAngles;
            euler_rot.z = rotation;
            laser.transform.rotation = Quaternion.Euler(euler_rot);
        }

        IEnumerator LaserCycle()
        {
            eyeBeamGlow.SetActive(true);

            targetPosition = HeroController.instance.transform.position;
            targetMarker.transform.position = targetPosition;
            
            laserFsm.SendEvent("ANTIC");
            targetMarker.SetActive(true);
            yield return new WaitForSeconds(settings.anticTime);
            if (isActive)
                laserFsm.SendEvent("FIRE");
            else
                laserFsm.SetState("Inert");
            yield return new WaitForSeconds(settings.fireTime);
            targetMarker.SetActive(false);
            laserFsm.SendEvent("END");

            eyeBeamGlow.SetActive(false);
            time = 0;
            isFiring = false;
        }

        IEnumerator EmptyCycle()
        {
            yield return new WaitForSeconds(settings.anticTime);
            yield return new WaitForSeconds(settings.fireTime);
            time = 0;
            isFiring = false;
        }
    }
}
