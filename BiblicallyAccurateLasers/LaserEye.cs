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

        void Awake()
        {
            laser = Instantiate(BiblicallyAccurateLasers._gameObjects["Ascend Beam"], transform);
        }

        void Start()
        {
            laser.transform.localPosition = Vector3.zero;
            laser.transform.rotation = Quaternion.identity;

            laser.SetActive(true);
            laser.LocateMyFSM("Control").SetState("Antic");
        }

    }
}
