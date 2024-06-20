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
                GameObject crown = GameObjectSpawns.Crown(enemy.transform, new Vector3(-0.1f, 2f, 0f), Vector3.one);
                GameObject eyeRing = GameObjectSpawns.EyeRing(7);
                eyeRing.transform.parent = enemy.transform;
                eyeRing.transform.localPosition = new Vector3(-0.1f, 1.5f, 0f);
                eyeRing.transform.localScale = Vector3.one * 3;
            }

            return isAlreadyDead;
        }
    }
}