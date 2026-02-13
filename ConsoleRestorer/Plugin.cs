using System.Collections.Generic;
using System.Net.Http;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;

namespace ConsoleRestorer
{
    [BepInPlugin("ImudTrust.ConsoleRestorer", "ImudTrust", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = new Harmony("ImudTrust.ConsoleRestorer");
            harmony.PatchAll();
            Debug.Log("[ConsoleRestorer] Initialized – previously blocked URLs are now allowed");
        }
    }

    public class Constants
    {
        public static List<string> BlockedUrls = new List<string>()
        {
            "https://iidk.online/",
            "https://raw.githubusercontent.com/iiDk-the-actual/Console",
            "https://data.hamburbur.org",
            "https://files.hamburbur.org"
        };
    }

    [HarmonyPatch(typeof(UnityWebRequest), nameof(UnityWebRequest.SendWebRequest))]
    public class UnityWebRequestPatch
    {
        [HarmonyPrefix]
        static bool Prefix(UnityWebRequest __instance)
        {
            if (Constants.BlockedUrls.Contains(__instance.url))
            {
                Debug.Log($"[ConsoleRestorer] Unblocking {__instance.url}");
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(HttpClient), nameof(HttpClient.GetByteArrayAsync), new[] { typeof(string) })]
    public class HttpClientPatch
    {
        [HarmonyPrefix]
        static bool Prefix(string requestUri)
        {
            if (Constants.BlockedUrls.Contains(requestUri))
            {
                Debug.Log($"[ConsoleRestorer] Unblocking {requestUri}");
            }
            return true;
        }
    }
}