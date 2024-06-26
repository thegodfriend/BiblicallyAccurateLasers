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

        private GameObject laser;
        private PlayMakerFSM laserFsm;

        private bool isFiring = false;
        private float time = 0;

        void Awake()
        {
            laser = Instantiate(BiblicallyAccurateLasers._gameObjects["Ascend Beam"], transform);
            laserFsm = laser.LocateMyFSM("Control");
        }

        void Start()
        {
            laser.transform.localPosition = Vector3.zero;
            laser.transform.rotation = Quaternion.identity;

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
            laserFsm.SendEvent("ANTIC");
            yield return new WaitForSeconds(0.5f);
            laserFsm.SendEvent("FIRE");
            yield return new WaitForSeconds(0.15f);
            laserFsm.SendEvent("END");

            time = 0;
            isFiring = false;
        }

    }
}
