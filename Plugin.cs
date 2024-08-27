using BepInEx;
using HarmonyLib;
using LethalLib.Modules;
using System.IO;
using System.Reflection;
using UnityEngine;
using pnot0sThings.Behaviour;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using GameNetcodeStuff;
using Unity.Netcode;
using NetworkPrefabs = LethalLib.Modules.NetworkPrefabs;
using JetBrains.Annotations;
using pnot0sThings.ItemBehaviour;
using pnot0sThings.MonoBehaviours;

namespace pnot0sThings
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        const string GUID = "pnot0.pnot0sThings";
        const string NAME = "pnot0sThings";
        const string VERSION = "1.0";

        public static Plugin instance;

        private void Awake()
        {

            instance = this;

            //Declaring asset path
            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "pnot0sthings");
            AssetBundle bundle = AssetBundle.LoadFromFile(assetDir);

            //Declaring each item and their corresponding asset
            Item catObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/CatObject/CatObjectItem.asset");
            Item faultyRPGObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/FaultyRPG/FaultyRPGObjectItem.asset");
            Item speedCoilObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/SpeedCoil/SpeedCoilObjectItem.asset");
            Item gravityCoilObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/GravityCoil/GravityCoilObjectItem.asset");
            Item albumVinylObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/AlbumVinyl/AlbumVinylObjectItem.asset");
            Item dollySodaObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/DollySoda/DollySodaObjectItem.asset");
            Item dollarBillsObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/DollarBills/DollarBillsObjectItem.asset");
            Item zapSodaObject = bundle.LoadAsset<Item>("Assets/pnot0sThings/ZapSoda/ZapSodaObjectItem.asset");
            Item colt1911Object = bundle.LoadAsset<Item>("Assets/pnot0sThings/Colt1911/Colt1911ObjectItem.asset");


            //Adding the components
            CatItem catBehaviour = catObject.spawnPrefab.AddComponent<CatItem>();
            catBehaviour.itemProperties = catObject;

            FaultyRPG faultyRPGBehaviour = faultyRPGObject.spawnPrefab.AddComponent<FaultyRPG>();
            faultyRPGObject.spawnPrefab.AddComponent<RPGExplosion>();
            faultyRPGBehaviour.itemProperties = faultyRPGObject;
            
            SpeedCoil speedCoilBehaviour = speedCoilObject.spawnPrefab.AddComponent<SpeedCoil>();
            speedCoilBehaviour.itemProperties = speedCoilObject;

            GravityCoil gravityCoilBehaviour = gravityCoilObject.spawnPrefab.AddComponent<GravityCoil>();
            gravityCoilBehaviour.itemProperties = gravityCoilObject;

            AlbumVinyl albumVinylBehaviour = albumVinylObject.spawnPrefab.AddComponent<AlbumVinyl>();
            albumVinylBehaviour.itemProperties = albumVinylObject;

            DollySoda dollySodaBehaviour = dollySodaObject.spawnPrefab.AddComponent<DollySoda>();
            dollySodaBehaviour.itemProperties = dollySodaObject;

            DollarBills dollarBillsBehaviour = dollarBillsObject.spawnPrefab.AddComponent<DollarBills>();
            dollarBillsBehaviour.itemProperties = dollarBillsObject;

            ZapSoda zapSodaBehaviour = zapSodaObject.spawnPrefab.AddComponent<ZapSoda>();
            zapSodaBehaviour.itemProperties = zapSodaObject;

            Colt1911 colt1911Behaviour = colt1911Object.spawnPrefab.AddComponent<Colt1911>();
            colt1911Behaviour.itemProperties = colt1911Object;

            //Networking registering, so that it works on multiplayer
            NetworkPrefabs.RegisterNetworkPrefab(catObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(faultyRPGObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(speedCoilObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(gravityCoilObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(albumVinylObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(dollySodaObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(dollarBillsObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(zapSodaObject.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(colt1911Object.spawnPrefab);

            //This fixes audio issues
            Utilities.FixMixerGroups(catObject.spawnPrefab);
            Utilities.FixMixerGroups(faultyRPGObject.spawnPrefab);
            Utilities.FixMixerGroups(speedCoilObject.spawnPrefab);
            Utilities.FixMixerGroups(gravityCoilObject.spawnPrefab);
            Utilities.FixMixerGroups(albumVinylObject.spawnPrefab);
            Utilities.FixMixerGroups(dollySodaObject.spawnPrefab);
            Utilities.FixMixerGroups(dollarBillsObject.spawnPrefab);
            Utilities.FixMixerGroups(zapSodaObject.spawnPrefab);
            Utilities.FixMixerGroups(colt1911Object.spawnPrefab);

            //Registering item as scrap, followed by rarity and presence in levels
            Items.RegisterScrap(catObject, 3, Levels.LevelTypes.All);
            Items.RegisterScrap(faultyRPGObject, 5, Levels.LevelTypes.All);
            Items.RegisterScrap(albumVinylObject, 10, Levels.LevelTypes.All);
            Items.RegisterScrap(dollySodaObject, 10, Levels.LevelTypes.All);
            Items.RegisterScrap(dollarBillsObject, 15, Levels.LevelTypes.All);
            Items.RegisterScrap(zapSodaObject, 15, Levels.LevelTypes.All);
            Items.RegisterScrap(colt1911Object, 8, Levels.LevelTypes.All);


            //Shop registering
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();

            node.clearPreviousText = true;
            node.displayText = "Speed Coil";
            Items.RegisterShopItem(speedCoilObject, null, null, node, 250);

            node.clearPreviousText = true;
            node.displayText = "Gravity Coil";
            Items.RegisterShopItem(gravityCoilObject, null, null, node, 200);

            //Harmony patch and network patch
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "pnot0sthings");
            Logger.LogInfo($"Plugin {NAME} is lock and loaded!");

            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }
    }
}
