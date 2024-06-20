using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace BiblicallyAccurateLasers
{
    public class BiblicallyAccurateLasers : Mod, IGlobalSettings<Settings>, IMenuMod
    {
        internal static BiblicallyAccurateLasers Instance;
        public Settings settings = new();
        public bool ToggleButtonInsideMenu => true;

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
            if (settings.modOn && enemy.name.Contains("Radiance"))
            {
                GameObject crown = GameObjectSpawns.SpawnEyeCrown();
                crown.transform.parent = enemy.transform;
                crown.transform.localPosition = new Vector3(-0.1f, 2f, 0f);
                crown.transform.localScale = Vector3.one;

                GameObject eyeRing = GameObjectSpawns.SpawnEyeRing(settings.eyeCount);
                eyeRing.transform.parent = enemy.transform;
                eyeRing.transform.localPosition = new Vector3(-0.1f, 1.5f, 0f);
                eyeRing.transform.localScale = Vector3.one * 3;
            }
            return isAlreadyDead;
        }

        public void OnLoadGlobal(Settings _settings) => settings = _settings;
        public Settings OnSaveGlobal() => settings;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? menu)
        {
            List<IMenuMod.MenuEntry> menus = new();

            menus.Add(
                new()
                {
                    Name = "Mod on",
                    Description = "This will apply only to the next fight",
                    Values = new string[]
                    {
                        Language.Language.Get("MOH_ON", "MainMenu"),
                        Language.Language.Get("MOH_OFF", "MainMenu"),
                    },
                    Saver = i => settings.modOn = i == 0,
                    Loader = () => settings.modOn ? 0 : 1
                }
            );

            return menus;
        }
    }
}