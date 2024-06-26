using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            laser.SetActive(true);
            laserFsm.SetState("Inert");
        }

        void Update()
        {
            time += Time.deltaTime;

            if (time > 3f && !isFiring)
            {
                isFiring = true;
                StartCoroutine(LaserCycle());
            }
        }

        
        IEnumerator LaserCycle()
        {
            eyeBeamGlow.SetActive(true);
            laserFsm.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.5f);
            laserFsm.SendEvent("FIRE");
            yield return new WaitForSeconds(0.15f);
            laserFsm.SendEvent("END");

            eyeBeamGlow.SetActive(false);
            time = 0;
            isFiring = false;
        }

    }
}
