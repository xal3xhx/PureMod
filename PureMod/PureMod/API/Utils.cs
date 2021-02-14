﻿using VRC;
using VRC.Core;
using UnityEngine;
using VRC.SDKBase;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

namespace PureMod.API
{
    public class Utils
    {
        public static Logger.Logger CoreLogger = new Logger.Logger("PureMod", Logger.LogLevel.Trace);

        public static GameObject[] GetAllGameObjects()=>
            SceneManager.GetActiveScene().GetRootGameObjects();

        public static List<GameObject> GetAllObjectsInSceneTree() =>
            Resources.FindObjectsOfTypeAll<GameObject>().ToList();

        public static int GetPlayerCount() =>
            VRCPlayerApi.GetPlayerCount();

        public static VRCPlayerApi GetLocalPlayer() =>
            Networking.LocalPlayer;

        public static VRCPlayerApi GetVRCPlayerApiByID(int ID) =>
            VRCPlayerApi.GetPlayerById(ID);

        public static PlayerManager GetPlayerManager() =>
            GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        public static Il2CppSystem.Collections.Generic.List<Player> GetPlayers() =>
            GetPlayerManager().field_Private_List_1_Player_0;

        public static Il2CppSystem.Collections.Generic.List<APIUser> GetAPIUsers()
        {
            var list = new Il2CppSystem.Collections.Generic.List<APIUser>();

            foreach (var player in GetPlayers())
                list.Add(player.prop_APIUser_0);

            return list;
        }

        public static GameObject GetLocalPlayerCamera() =>
            GameObject.Find("Camera (eye)");

        public static Il2CppSystem.Collections.Generic.List<VRCPlayerApi> GetPlayerAPIs() =>
            VRCPlayerApi.AllPlayers;
    }
}
