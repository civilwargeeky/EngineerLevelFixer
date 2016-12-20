using System;
using UnityEngine;

namespace EngineerLevelFixer
{
  class ConfigLoader : MonoBehaviour
  {
    protected static string SaveLocation = "GameData/EngineerLevelFixer/Settings.cfg"; //Where the settings file is saved
    protected static bool hasLoaded = false;
    //Default values for variables that will be returned
    protected static int wheelLevel = 1;
    protected static int gearLevel = 1;
    protected static bool doDebug = false;

    public static void saveConfigFile()
    {
      DebugHelper.Info("Writing New Config");

      ConfigNode file = new ConfigNode();
      ConfigNode settings = file.AddNode("SETTINGS");
      settings.AddValue("wheelLevel", wheelLevel, "Stars an engineer must have to repair wheels in career");
      settings.AddValue("doDebug", doDebug, "Debug or not");

      file.Save(SaveLocation);

      hasLoaded = true; //If we save, it means the values are the exact same, so don't load.
    }

    public static void loadConfigFile()
    {
      //NOTE: Be careful to not use debug statements here, as debug checks if config is loaded
      if (hasLoaded) { return; } //If has already loaded, then we don't need to do this

      DebugHelper.Info("Loading Settings File");

      try
      {
        ConfigNode settings = ConfigNode.Load(SaveLocation);

        settings = settings.GetNode("SETTINGS");
        string legacyVal = settings.GetValue("engineerLevel"); //Support the version 0.2 config
        if (legacyVal != null)
        {
          wheelLevel = gearLevel = int.Parse(legacyVal); //Set both to the existing one.
          saveConfigFile();
          return; //Don't bother with rest, they don't exist
        }

        wheelLevel = int.Parse(settings.GetValue("wheelLevel"));
        gearLevel = int.Parse(settings.GetValue("gearLevel"));
        doDebug = bool.Parse(settings.GetValue("doDebug"));

        DebugHelper.Info("Loaded Settings File Successfully");
      }
      catch (NullReferenceException) //If the file doesn't yet exist
      {
        DebugHelper.Info("Settings does not exist, making");
        saveConfigFile();
      }
      catch (ArgumentNullException) //Or if the file is using legacy (int.Parse got null or something)
      {
        DebugHelper.Error("Encountered malformed Settings file");
        saveConfigFile();
      }

      hasLoaded = true; //Finish off so we don't do this again
    }

    public static int getWheelLevel()
    {
      DebugHelper.Debug("Getting Engineer Level");
      ConfigLoader.loadConfigFile(); //Load the config if not already loaded
      DebugHelper.Debug(string.Concat("Engineer Level Required for Wheels: ", wheelLevel.ToString()));
      return wheelLevel;
    }

    public static bool getDebug()
    {
      if (!hasLoaded) {
        ConfigLoader.loadConfigFile(); //This will just return immediately if loaded
      }
      return doDebug;
    }
  }
}
