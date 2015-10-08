using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EngineerLevelFixer
{
    class ConfigLoader : MonoBehaviour
    {
        protected static string SaveLocation = "GameData/EngineerLevelFixer/Settings.cfg"; //Where the settings file is saved
        protected static bool hasLoaded = false;
        protected static int engineerLevel = 1;
        protected static bool doDebug = false;

        public static void saveConfigFile()
        {
            DebugHelper.Info("Writing New Config");

            ConfigNode file = new ConfigNode();
            ConfigNode settings = file.AddNode("SETTINGS");
            settings.AddValue("engineerLevel", engineerLevel);
            settings.AddValue("doDebug", doDebug);

            file.Save(SaveLocation);
        }

        public static void loadConfigFile()
        {
            if (hasLoaded) { return; } //If has already loaded, then we don't need to do this

            DebugHelper.Info("Loading Settings File");

            try
            { 
                ConfigNode settings = ConfigNode.Load(SaveLocation); 
                settings = settings.GetNode("SETTINGS");
                engineerLevel = int.Parse(settings.GetValue("engineerLevel"));
                doDebug = bool.Parse(settings.GetValue("doDebug"));

                DebugHelper.Info("Config Exists Already");
            }
            catch (NullReferenceException) //If the file doesn't yet exist
            {
                saveConfigFile();
            }
            hasLoaded = true; //Finish off so we don't do this again
        }

        public static int getEngineerLevel()
        {
            DebugHelper.Debug("Getting Engineer Level");
            ConfigLoader.loadConfigFile(); //Load the config if not already loaded

            DebugHelper.Info(String.Concat("Engineer Level Required: ", engineerLevel.ToString()));
            return engineerLevel;
        }
        public static bool getDebug()
        {
            ConfigLoader.loadConfigFile();

            return doDebug;
        }
    }
}
