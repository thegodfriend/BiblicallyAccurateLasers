using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BiblicallyAccurateLasers
{
    public class BiblicallyAccurateLasers : Mod
    {
        internal static BiblicallyAccurateLasers Instance;

        internal TextureStrings SpriteDict { get; private set; }
        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        public BiblicallyAccurateLasers() : base("Biblically Accurate Lasers") { }
        public override string GetVersion() => "0.0.0";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;
            SpriteDict = new TextureStrings();

            ModHooks.OnEnableEnemyHook += EnemyEnabled;

            Log("Initialized");
        }

        private bool EnemyEnabled(GameObject enemy, bool isAlreadyDead)
        {
            if (enemy.name.Contains("Radiance"))
            {
                GameObject crown = Crown(enemy.transform, new Vector3(-0.1f, 2f, 0f), Vector3.one);
            }

            return isAlreadyDead;
        }

        private GameObject Crown(Transform parent, Vector3 localPosition, Vector3 localScale, int eyes = 7)
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