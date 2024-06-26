using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BiblicallyAccurateLasers
{
    public class BiblicallyAccurateLasers : Mod, IGlobalSettings<Settings>, IMenuMod
    {
        internal static BiblicallyAccurateLasers Instance;
        public Settings settings = new();
        public bool ToggleButtonInsideMenu => true;

        internal TextureStrings SpriteDict { get; private set; }
        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

        public static readonly Dictionary<string, GameObject> _gameObjects = new();
        private readonly Dictionary<string, ValueTuple<string, string>> _preloads = new()
        {
            ["Eye Beam Glow"] = ("GG_Radiance", "Boss Control/Absolute Radiance/Eye Beam Glow"),
        };
        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return _preloads.Values.ToList();
        }

        public BiblicallyAccurateLasers() : base("Biblically Accurate Lasers") { }
        public override string GetVersion() => "0.1.0";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;
            SpriteDict = new TextureStrings();

            foreach (var (name, (scene, path)) in _preloads)
            {
                _gameObjects[name] = preloadedObjects[scene][path];
            }

            ModHooks.OnEnableEnemyHook += EnemyEnabled;
            ModHooks.LanguageGetHook += LanguageGet;

            Log("Initialized");
        }

        private string LanguageGet(string key, string sheetTitle, string orig)
        {
            if (settings.originalText) switch (key)
            {
                case "FINAL_BOSS_SUPER":
                    return "";
                case "FINAL_BOSS_MAIN":
                    return "Beauty";
                case "ABSOLUTE_RADIANCE_SUPER":
                    return "";
                case "ABSOLUTE_RADIANCE_MAIN":
                    return "Beauty";
                case "NAME_FINAL_BOSS":
                    return "Beauty";
                case "GG_S_RADIANCE":
                    return "is in the eye of the beholder.";
            }
            return orig;
        }

        private bool EnemyEnabled(GameObject enemy, bool isAlreadyDead)
        {
            if (settings.modOn && enemy.name.Contains("Radiance"))
            {
                /*GameObject crown = GameObjectSpawns.SpawnEyeCrown();
                crown.transform.parent = enemy.transform;
                crown.transform.localPosition = new Vector3(-0.1f, 2f, 0f);
                crown.transform.localScale = Vector3.one;*/
                //Crown is really distracting

                GameObject eyeRing = GameObjectSpawns.SpawnEyeRing(settings.eyeCount);
                eyeRing.transform.parent = enemy.transform;
                eyeRing.transform.localPosition = new Vector3(-0.1f, 1.5f, -0.001f);
                eyeRing.transform.localScale = Vector3.one * 3;
            }
            return isAlreadyDead;
        }

        public void OnLoadGlobal(Settings _settings) => settings = _settings;
        public Settings OnSaveGlobal() => settings;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? menu)
        {
            List<IMenuMod.MenuEntry> menus = new()
            {
                new()
                {
                    Name = "Mod On",
                    Description = "This will take effect starting next fight",
                    Values = new string[]
                    {
                        Language.Language.Get("MOH_ON", "MainMenu"),
                        Language.Language.Get("MOH_OFF", "MainMenu"),
                    },
                    Saver = i => settings.modOn = i == 0,
                    Loader = () => settings.modOn ? 0 : 1
                },
                new()
                {
                    Name = "EotB text",
                    Description = "Use the text changes from the original 'Eye Of The Beholder' mod",
                    Values = new string[]
                    {
                        Language.Language.Get("MOH_ON", "MainMenu"),
                        Language.Language.Get("MOH_OFF", "MainMenu"),
                    },
                    Saver = i => settings.originalText = i == 0,
                    Loader = () => settings.originalText ? 0 : 1
                },
                new()
                {
                    Name = "Eye Count",
                    Description = "This will take effect starting next fight",
                    Values = new string[]
                    {
                        "1",
                        "2",
                        "3",
                        "4",
                        "5",
                        "6",
                        "7",
                        "8",
                        "9",
                        "10",
                        "11",
                        "12",
                        "13",
                    },
                    Saver = i => settings.eyeCount = i + 1,
                    Loader = () => settings.eyeCount - 1
                }
            };

            return menus;
        }
    }
}