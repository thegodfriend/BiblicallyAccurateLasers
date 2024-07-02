using Modding;
using Modding.Utils;
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
        public override string GetVersion() => "1.0.0";

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

                enemy.GetOrAddComponent<RadianceLaserControl>();
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
                },
                new()
                {
                    Name = "Laser Antic Time",
                    Values = new string[]
                    {
                        "0s",
                        "0.1s",
                        "0.2s",
                        "0.3s",
                        "0.4s",
                        "0.5s",
                        "0.6s",
                        "0.7s",
                        "0.8s",
                        "0.9s",
                        "1.0s",
                        "1.1s",
                        "1.2s",
                        "1.3s",
                    },
                    Saver = i => settings.anticTime = i / 10f,
                    Loader = () => (int)(settings.anticTime * 10f)
                },
                /*new()
                {
                    Name = "Laser Damage Time",
                    //Description = "This will take effect starting next fight",
                    Values = new string[]
                    {
                        "0.15s",
                        "0.30s",
                        "0.45s",
                        "0.60s",
                        "0.75s",
                        "0.90s",
                        "1.05s",
                        "1.20s",
                        "1.35s",
                        "1.50s",
                        "1.65s",
                        "1.80s",
                        "1.95s",
                        "2.10s",
                    },
                    Saver = i => settings.fireTime = (i + 1) * 0.15f,
                    Loader = () => (int)(settings.fireTime / 0.15f) - 1
                },*/
                new()
                {
                    Name = "Laser Cooldown Time",
                    Values = new string[]
                    {
                        "0s",
                        "0.25s",
                        "0.50s",
                        "0.75s",
                        "1.00s",
                        "1.25s",
                        "1.50s",
                        "1.75s",
                        "2.00s",
                        "2.25s",
                        "2.50s",
                        "2.75s",
                        "3.00s",
                        "3.25s",
                        "3.50s",
                        "3.75s",
                        "4.00s",
                        "4.25s",
                        "4.50s",
                        "4.75s",
                        "5.00s",
                        "5.25s",
                        "5.50s",
                        "5.75s",
                        "6.00s",
                        "6.25s",
                        "6.50s",
                        "6.75s",
                    },
                    Saver = i => settings.cooldown = i * 0.25f,
                    Loader = () => (int)(settings.cooldown / 0.25f)
                },
            };
            
            return menus;
        }
    }
}