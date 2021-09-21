using BSPlayerDataBackup.Configuration;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BSPlayerDataBackup
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, Config config)
        {
            Instance = this;
            Log = logger;
            Log.Info("BSPlayerDataBackup initialized.");
            PluginConfig.Instance = config.Generated<PluginConfig>();
        }
        [OnEnable]
        public void OnEnable()
        {
            var descPath = Path.Combine(PluginConfig.Instance.BackupPath, $"{Application.version}_{DateTime.Now:yyyyMMdd}");
            var tmpPath = Path.Combine(Path.GetTempPath(), "BSPlayerDataBackup");
            try {
                if (!Directory.Exists(Path.GetDirectoryName(descPath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(descPath));
                }
                Log.Info($"Start backup : {descPath}");
                this.Copy(Application.persistentDataPath, tmpPath);
                if (File.Exists($"{descPath}.zip")) {
                    File.Delete($"{descPath}.zip");
                }
                ZipFile.CreateFromDirectory(tmpPath, $"{descPath}.zip");
            }
            catch (Exception e) {
                Log.Error(e);
            }
            finally {
                if (Directory.Exists(tmpPath)) {
                    try {
                        Directory.Delete(tmpPath, true);
                    }
                    catch (Exception e) {
                        Log.Error(e);
                    }
                }
            }
        }

        private void Copy(string sourceDirectoryName, string destDirectoryName, bool overwrite = false)
        {
            if (!Directory.Exists(destDirectoryName)) {
                Directory.CreateDirectory(destDirectoryName);
            }
            foreach (var file in Directory.EnumerateFiles(sourceDirectoryName, "*", SearchOption.TopDirectoryOnly)) {
                if (string.Equals(new FileInfo(file).Directory.Name, "BSPlayerDataBackup", StringComparison.InvariantCultureIgnoreCase)) {
                    return;
                }
                File.Copy(file, Path.Combine(destDirectoryName, Path.GetFileName(file)), overwrite);
            }
            foreach (var dir in Directory.EnumerateDirectories(sourceDirectoryName, "*", SearchOption.TopDirectoryOnly)) {
                this.Copy(dir, Path.Combine(destDirectoryName, Path.GetFileName(dir)), overwrite);
            }
        }
    }
}
