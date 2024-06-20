﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BiblicallyAccurateLasers
{
    internal static class GameObjectSpawns
    {
        public static GameObject EyeRing(int eyes)
        {
            GameObject ringHolder = new();
            GameObject eyeRing = new();

            for (int i = 0; i < eyes; i++)
            {
                GameObject eyeCarrier = new GameObject();
                eyeCarrier.transform.parent = eyeRing.transform;
                eyeCarrier.transform.localPosition = Vector3.zero;
                eyeCarrier.transform.localScale = Vector3.one;

                GameObject eye = new GameObject();
                eye.AddComponent<SpriteRenderer>().sprite = BiblicallyAccurateLasers.GetSprite(TextureStrings.EyeKey);
                eye.SetActive(true);
                eye.transform.parent = eyeCarrier.transform;
                eye.transform.localPosition = new Vector3(0, 2f, 0f);
                eye.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);

                eyeCarrier.transform.localRotation = Quaternion.Euler(0, 0, i * (360f/eyes));
                eye.transform.localRotation = Quaternion.Euler(0, 0, -i * (360f/eyes));

                eye.AddComponent<Spin>().SetSpeed(0.5f);
            }

            eyeRing.transform.parent = ringHolder.transform;
            eyeRing.transform.localPosition = new Vector3(0, 0, -1f);
            eyeRing.transform.localScale = Vector3.one;
            eyeRing.transform.localRotation = Quaternion.identity;
            eyeRing.AddComponent<Spin>().SetSpeed(-0.5f);

            return ringHolder;
        }

        public static GameObject Crown(Transform parent, Vector3 localPosition, Vector3 localScale, int eyes = 7)
        {
            GameObject crown = new();

            GameObject eyesHolder = new();
            eyesHolder.transform.parent = crown.transform;
            eyesHolder.transform.localPosition = Vector3.zero;
            eyesHolder.transform.localScale = Vector3.one;
            eyesHolder.transform.localRotation = Quaternion.Euler(0, 0, 0);

            for (int i = 0; i < eyes; i++)
            {
                GameObject eyeCarrier = new();
                eyeCarrier.transform.parent = eyesHolder.transform;
                eyeCarrier.transform.localPosition = Vector3.zero;
                eyeCarrier.transform.localScale = Vector3.one;

                GameObject eye = new();
                eye.AddComponent<SpriteRenderer>();
                eye.GetComponent<SpriteRenderer>().sprite = BiblicallyAccurateLasers.GetSprite(TextureStrings.EyeKey);
                eye.SetActive(true);
                eye.transform.parent = eyeCarrier.transform;
                eye.transform.localPosition = new Vector3(0, 3f, 0);
                eye.transform.localScale = Vector3.one * 0.1f;

                eyeCarrier.transform.rotation = Quaternion.Euler(0, 0, i * 360f / eyes);
                eye.transform.localEulerAngles = new Vector3(-90f, 0, 0);
            }
            eyesHolder.AddComponent<Spin>();

            crown.transform.parent = parent.transform;
            crown.transform.localPosition = localPosition;
            crown.transform.localScale = localScale;
            crown.transform.rotation = Quaternion.Euler(90, 0, 0);

            return crown;
        }
    }
}
