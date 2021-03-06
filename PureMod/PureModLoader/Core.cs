﻿using System;
using System.IO;
using System.Net;
using MelonLoader;
using System.Linq;
using PureMod.API;
using System.Timers;
using System.Reflection;
using PureMod.API.Logger;
using System.Collections.Generic;
using Logger = PureMod.API.Logger.Logger;

namespace PureModLoader
{
    public class Core : MelonMod
    {
        private static List<ModSystem> Mods = new List<ModSystem>();
        private static Logger CoreLogger = new Logger("PureModLoader", LogLevel.Trace);
        private static WebClient client = new WebClient();

        private void LoadMods()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "PureMod")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "PureMod"));

            var filePath = Path.Combine(Environment.CurrentDirectory, "PureMod\\PureMod.dll");

            try
            {
                if (!File.Exists(filePath))
                    client.DownloadFile(new Uri("https://github.com/PureFoxCore/PureMod/releases/latest/download/PureMod.dll"), filePath);

                if (File.Exists(filePath))
                {
                    Assembly.LoadFile(filePath);
                    CoreLogger.Info("PureMod Loaded");

                    var result = new List<Type>();
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    foreach (var assembly in assemblies)
                    {
                        var types = assembly.GetTypes();
                        foreach (var type in types)
                            if (type.IsSubclassOf(typeof(ModSystem)))
                                result.Add(type);
                    }

                    foreach (var item in result)
                        Mods.Add((ModSystem)Activator.CreateInstance(item));

                    Mods = Mods.OrderBy(owo => owo.LoadOrder).ToList();
                }
                else
                    CoreLogger.Error("Can't Load PureMod! [File not found]");
            }
            catch (Exception ex)
            {
                CoreLogger.Error("Can't Load PureMod!");
                CoreLogger.Error(ex.ToString());
            }
        }

        public override void OnApplicationStart()
        {
            LoadMods();

            foreach (ModSystem mod in Mods)
            {
                if (mod.ShowName)
                    CoreLogger.Info($"{mod.ModName} loaded!");

                mod.OnEarlierStart();
            }
            CoreLogger.Info($"Loaded {Mods.Count} mods!");

            new System.Threading.Timer((e) =>
            {
                foreach (ModSystem mod in Mods)
                    mod.OnUpdate10();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        public override void VRChat_OnUiManagerInit()
        {
            foreach (ModSystem mod in Mods)
                mod.OnStart();
        }

        public override void OnUpdate()
        {
            foreach (ModSystem mod in Mods)
                mod.OnUpdate();
        }

        public override void OnLateUpdate()
        {
            foreach (ModSystem mod in Mods)
                mod.OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            foreach (ModSystem mod in Mods)
                mod.OnFixedUpdate();
        }

        public override void OnGUI()
        {
            foreach (ModSystem mod in Mods)
                mod.OnGUI();
        }

        public override void OnApplicationQuit()
        {
            foreach (ModSystem mod in Mods)
                mod.OnQuit();
        }
    }
}
