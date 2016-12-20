﻿using System;

namespace EngineerLevelFixer {
  internal class Properties {
    public static ScreenMessageStyle textStyle = ScreenMessageStyle.UPPER_CENTER;
  }

  /* //Trying to make a button so they can change behavior on the fly
  [KSPAddon(KSPAddon.Startup.SpaceCentre, true)] //?
  public class ButtonClass : MonoBehaviour //?
  {
      private ApplicationLauncherButton myButton;
      private Texture2D buttonTexture;
      private bool showGUI = false;

      private Rect test = new Rect(580f, 40f, 1f, 1f);


      public void Start()
      {
          DebugHelper.Debug("Start in button class is being called");
          if (myButton == null)
          {
              myButton = ApplicationLauncher.Instance.AddModApplication(toggleButton, toggleButton, null, null, null, null, ApplicationLauncher.AppScenes.SPACECENTER, (Texture)buttonTexture);
          }
      }

      public void onDestroy()
      {
          if (myButton != null)
          {
              ApplicationLauncher.Instance.RemoveModApplication(myButton);
              myButton = null;
          }
      }

      public void onGUI()
      {
          if (showGUI)
          {
              //test = GUILayout.Window(1, test, WindowGUI, "Test",null);
          }
      }

      private void WindowGUI() //This is where all the actual GUI things go
      {

      }

      public void toggleButton()
      {
          showGUI = !showGUI;
      }
  }
  */

  //This should theoretically work for landing legs and wheels
  public class ModuleFixedWheel : ModuleWheels.ModuleWheelDamage {

    [KSPEvent(guiActive = false, guiActiveUnfocused = true, unfocusedRange = 4, externalToEVAOnly = true, guiName = "Repair Wheel")]
    public void EventRepairExternal() {
      DebugHelper.Debug("My EventRepairExternal function has been called");
      int engineerLevel = FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value;
      int requiredEngineerLevel = ConfigLoader.getWheelLevel();

      DebugHelper.Debug(String.Concat("Engineer Level: ", engineerLevel.ToString()));
      DebugHelper.Debug(String.Concat("Science Skill: ", FlightGlobals.ActiveVessel.VesselValues.ScienceSkill.value.ToString()));
      DebugHelper.Debug(String.Concat("Pilot Skill: ", FlightGlobals.ActiveVessel.VesselValues.AutopilotSkill.value.ToString()));
      if (HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX || engineerLevel >= requiredEngineerLevel) //We want it always to succeed in Sandbox, otherwise succeed if level is high enough
      {
        DebugHelper.Debug("Repair Succeeded");
        this.SetDamaged(false);
      } else if (engineerLevel < 0) { //For non-engineers. If not an engineer, engineer skill is -1
        ScreenMessages.PostScreenMessage("You must repair wheels with Engineer Kerbals!", 2.0F, Properties.textStyle);
      } else //This means that the kerbal is an engineer, but not high enough level
        ScreenMessages.PostScreenMessage("Engineer Kerbal must be level " + requiredEngineerLevel + " to repair wheels \n Current Level: " + engineerLevel, 2.0F, Properties.textStyle);
    }

  }

  /*
  public class ModuleFixedLeg : ModuleLandingLeg
  {

    private KSPEvent GetCustomEventMethodInfo(string methodName) //Reflection magic from xEvilReeperx. TODO: Understand
    {
      return (KSPEvent)GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic)
          .GetCustomAttributes(typeof(KSPEvent), true).Single();
    }

    private BaseEvent CreateEvent(string originalMethod, string customMethodName) //More reflection magic
    {
      return new BaseEvent(Events, originalMethod, (BaseEventDelegate)Delegate.CreateDelegate(typeof(BaseEventDelegate), this, customMethodName), GetCustomEventMethodInfo(customMethodName));
    }

    private void myInitialize()
    {
      const string oldLock = "LockSuspension", //Variables because concise
                   oldUnlock = "UnLockSuspension",
                   myLock = "FixedLockSuspension",
                   myUnlock = "FixedUnLockSuspension";

      if (this.Events[oldLock] == null) //If we haven't yet initialized.
      {
        this.Events.Remove(Events[myLock]); //Remove duplicate events
        this.Events.Remove(Events[myUnlock]);

        this.Events.Add(CreateEvent(oldLock, myLock)); //Change method of existing events
        this.Events.Add(CreateEvent(oldUnlock, myUnlock));
      }

    }

    public override void OnInitialize() //Because parts are initialized in editor and on load
    {
      myInitialize();
      base.OnInitialize();
    }

    public override void OnStart(PartModule.StartState state) //Required because parts are started on load
    {
      myInitialize();
      base.OnStart(state);
    }

    [KSPEvent(guiName = "Repair Leg", guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4f)]
    public void RepairLeg()
    {
      DebugHelper.Debug("My Repair Leg Function has been called!");

      int engineerLevel = FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value;
      int requiredEngineerLevel = ConfigLoader.getGearLevel();
      if (HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX || engineerLevel >= requiredEngineerLevel) //We want it always to succeed in Sandbox, otherwise succeed if level is high enough
      {
        DebugHelper.Debug("Repair Succeeded");
        this.legState = ModuleLandingLeg.LegStates.DEPLOYED;
        SendMessage("DecompressSuspension", SendMessageOptions.RequireReceiver);
        this.Events["RepairLeg"].active = false;
      }
      else if (engineerLevel < 0) //For non-engineers. If not an engineer, engineer skill is -1
        ScreenMessages.PostScreenMessage("You must repair landing legs with Engineer Kerbals!", 2.0F, Properties.textStyle);
      else //This means that the kerbal is an engineer, but not high enough level
        ScreenMessages.PostScreenMessage("Engineer Kerbal must be level " + requiredEngineerLevel + " to repair landing legs \n Current Level: " + engineerLevel, 2.0F, Properties.textStyle);

    }

    //Function I have to implement to pass the load checks.
    [KSPEvent(guiName = "Lock Suspension", guiActiveEditor = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f)]
    private void FixedLockSuspension()
    {
      SendMessage("LockSuspension", SendMessageOptions.RequireReceiver);
    }

    [KSPEvent(guiName = "Unlock Suspension", guiActiveEditor = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f)]
    private void FixedUnLockSuspension()
    {
      SendMessage("UnLockSuspension", SendMessageOptions.RequireReceiver);
    }
  }
  */
}
//Copied from "Stage Recovery" mod for reference. Hopefully I remember to delete this before publishing
/*
public void ShowWindow()
        {
            Settings.instance.Clicked = true;
            if (HighLogic.LoadedSceneIsFlight)
                flightGUI.showFlightGUI = true;
            else if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
                ShowSettings();
            else if (HighLogic.LoadedSceneIsEditor)
                EditorCalc();
        }

        //Does stuff to draw the window.
        public void SetGUIPositions(GUI.WindowFunction OnWindow)
        {
            if (showWindow) mainWindowRect = GUILayout.Window(8940, mainWindowRect, DrawSettingsGUI, "StageRecovery", HighLogic.Skin.window);
            if (flightGUI.showFlightGUI) flightGUI.flightWindowRect = GUILayout.Window(8940, flightGUI.flightWindowRect, flightGUI.DrawFlightGUI, "StageRecovery", HighLogic.Skin.window);
            if (showBlacklist) blacklistRect = GUILayout.Window(8941, blacklistRect, DrawBlacklistGUI, "Ignore List", HighLogic.Skin.window);
            if (editorGUI.showEditorGUI) editorGUI.EditorGUIRect = GUILayout.Window(8940, editorGUI.EditorGUIRect, editorGUI.DrawEditorGUI, "StageRecovery", HighLogic.Skin.window);
        }

        //More drawing window stuff. I only half understand this. It just works.
        public void DrawGUIs(int windowID)
        {
            if (showWindow) DrawSettingsGUI(windowID);
            if (flightGUI.showFlightGUI) flightGUI.DrawFlightGUI(windowID);
            if (showBlacklist) DrawBlacklistGUI(windowID);
            if (editorGUI.showEditorGUI) editorGUI.DrawEditorGUI(windowID);
        }

        //Hide all the windows. We only have one so this isn't super helpful, but alas.
        public void hideAll()
        {
            showWindow = false;
            flightGUI.showFlightGUI = false;
            editorGUI.showEditorGUI = false;
            showBlacklist = false;
            Settings.instance.Clicked = false;
            editorGUI.UnHighlightAll();
        }

        //Resets the windows. Hides them and resets the Rect object. Not really needed, but it's here
        public void reset()
        {
            hideAll();
            mainWindowRect = new Rect(0, 0, windowWidth, 1);
            flightGUI.flightWindowRect = new Rect((Screen.width - 768) / 2, (Screen.height - 540) / 2, 768, 540);
            editorGUI.EditorGUIRect = new Rect(Screen.width / 3, Screen.height / 3, 200, 1);
            blacklistRect = new Rect(0, 0, 360, 1);
        }

        private string tempListItem = "";
        private void DrawBlacklistGUI(int windowID)
        {
            GUILayout.BeginVertical();
            scrollPos = GUILayout.BeginScrollView(scrollPos, HighLogic.Skin.textArea, GUILayout.Height(Screen.height / 4));
            foreach (string s in Settings.instance.BlackList.ignore)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(s);
                if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                {
                    Settings.instance.BlackList.Remove(s);
                    break;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            tempListItem = GUILayout.TextField(tempListItem);
            if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
            {
                Settings.instance.BlackList.Add(tempListItem);
                tempListItem = "";
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save"))
            {
                Settings.instance.BlackList.Save();
                showBlacklist = false;
            }
            if (GUILayout.Button("Cancel"))
            {
                Settings.instance.BlackList.Load();
                showBlacklist = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (!Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
                GUI.DragWindow();
        }

        //This function will show the settings window and copy the current settings into their holders
        public void ShowSettings()
        {
            enabled = Settings.instance.SREnabled;
            recMod = Settings.instance.RecoveryModifier;
            cutoff = Settings.instance.CutoffVelocity;
            DRMaxVel = Settings.instance.DeadlyReentryMaxVelocity.ToString();
            recoverSci = Settings.instance.RecoverScience;
            recoverKerb = Settings.instance.RecoverKerbals;
            showFail = Settings.instance.ShowFailureMessages;
            showSuccess = Settings.instance.ShowSuccessMessages;
            flatRate = Settings.instance.FlatRateModel;
            lowCut = Settings.instance.LowCut;
            highCut = Settings.instance.HighCut;
            poweredRecovery = Settings.instance.PoweredRecovery;
            recoverClamps = Settings.instance.RecoverClamps;
            minTWR = Settings.instance.MinTWR.ToString();
            useUpgrades = Settings.instance.UseUpgrades;
            useToolbar = Settings.instance.UseToolbarMod;
            showWindow = true;
        }

        //The function that actually draws all the gui elements. I use GUILayout for doing everything because it's easy to use.
        private void DrawSettingsGUI(int windowID)
        {
            //We start by begining a vertical segment. All new elements will be placed below the previous one.
            GUILayout.BeginVertical();

            //Whether the mod is enabled or not
            enabled = GUILayout.Toggle(enabled, " Mod Enabled");

            //We can toggle the Flat Rate Model on and off with a toggle
            flatRate = GUILayout.Toggle(flatRate, flatRate ? "Flat Rate Model" : "Variable Rate Model");
            //If Flat Rate is on we show this info
            if (flatRate)
            {
                //First off is a label saying what the modifier is (in percent)
                GUILayout.Label("Recovery Modifier: " + Math.Round(100 * recMod) + "%");
                //Then we have a slider that goes between 0 and 1 that sets the recMod
                recMod = GUILayout.HorizontalSlider(recMod, 0, 1);
                //We round the recMod for two reasons: it looks better and it makes it easier to select specific values. 
                //In this case it limits it to whole percentages
                recMod = (float)Math.Round(recMod, 2);

                //We do a similar thing for the cutoff velocity, limiting it to between 2 and 12 m/s
                GUILayout.Label("Cutoff Velocity: " + cutoff + "m/s");
                cutoff = GUILayout.HorizontalSlider(cutoff, 2, 12);
                cutoff = (float)Math.Round(cutoff, 1);
            }
            //If we're using the Variable Rate Model we have to show other info
            else
            {
                //Like for the flat rate recovery modifier and cutoff, we present a label and a slider for the low cutoff velocity
                GUILayout.Label("Low Cutoff Velocity: " + lowCut + "m/s");
                lowCut = GUILayout.HorizontalSlider(lowCut, 0, 10);
                lowCut = (float)Math.Round(lowCut, 1);

                //And another slider for the high cutoff velocity (with limits between lowCut and 16)
                GUILayout.Label("High Cutoff Velocity: " + highCut + "m/s");
                highCut = GUILayout.HorizontalSlider(highCut, lowCut+0.1f, 16);
                highCut = (float)Math.Max(Math.Round(highCut, 1), lowCut+0.1);
            }

            //We begin a horizontal, meaning new elements will be placed to the right of previous ones
            GUILayout.BeginHorizontal();
            //First element is a label
            GUILayout.Label("DR Velocity");
            //Followed by a text field where we can set the DRMaxVel value (as a string for the moment)
            DRMaxVel = GUILayout.TextField(DRMaxVel, 6);
            //Ending the horizontal means new elements will now be placed below previous ones (so these two will be side by side with things above and below too)
            //Make sure to End anything you Begin!
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Powered TWR");
            minTWR = GUILayout.TextField(minTWR, 4);
            GUILayout.EndHorizontal();

            //The rest are just toggles and are put one after the other
            recoverSci = GUILayout.Toggle(recoverSci, "Recover Science");
            recoverKerb = GUILayout.Toggle(recoverKerb, "Recover Kerbals");
            showFail = GUILayout.Toggle(showFail, "Failure Messages");
            showSuccess = GUILayout.Toggle(showSuccess, "Success Messages");
            poweredRecovery = GUILayout.Toggle(poweredRecovery, "Try Powered Recovery");
            recoverClamps = GUILayout.Toggle(recoverClamps, "Recover Clamps");
            useUpgrades = GUILayout.Toggle(useUpgrades, "Tie Into Upgrades");
            useToolbar = GUILayout.Toggle(useToolbar, "Use Toolbar Mod");

            if (GUILayout.Button("Edit Ignore List"))
            {
                showBlacklist = true;
            }

            //We then provide a single button to save the settings. The window can be closed by clicking on the toolbar button, which cancels any changes
            if (GUILayout.Button("Save"))
            {
                //When the button is clicked then this all is executed.
                //This all sets the settings to the GUI version's values
                Settings.instance.SREnabled = enabled;
                Settings.instance.FlatRateModel = flatRate;
                Settings.instance.LowCut = lowCut;
                Settings.instance.HighCut = highCut;
                Settings.instance.RecoveryModifier = recMod;
                Settings.instance.CutoffVelocity = cutoff;
                //Strings must be parsed into the correct type. Using TryParse returns a bool stating whether it was sucessful. The value is saved in the out if it works
                //Otherwise we set the value to the default
                if (!float.TryParse(DRMaxVel, out Settings.instance.DeadlyReentryMaxVelocity))
                    Settings.instance.DeadlyReentryMaxVelocity = 2000f;
                Settings.instance.RecoverScience = recoverSci;
                Settings.instance.RecoverKerbals = recoverKerb;
                Settings.instance.ShowFailureMessages = showFail;
                Settings.instance.ShowSuccessMessages = showSuccess;
                Settings.instance.PoweredRecovery = poweredRecovery;
                Settings.instance.RecoverClamps = recoverClamps;
                Settings.instance.UseUpgrades = useUpgrades;
                Settings.instance.UseToolbarMod = useToolbar;
                if (!float.TryParse(minTWR, out Settings.instance.MinTWR))
                    Settings.instance.MinTWR = 1.0f;
                //Finally we save the settings to the file
                Settings.instance.Save();
            }

            //The last GUI element is added, so now we close the Vertical with EndVertical(). If you don't close all the things you open, the GUI will not display any elements
            GUILayout.EndVertical();

            //This last thing checks whether the right mouse button or middle mouse button are clicked on the window. If they are, we ignore it, otherwise we GUI.DragWindow()
            //Calling that allows the window to be moved by clicking it (anywhere empty on the window) with the left mouse button and dragging it to wherever you want.
            if (!Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
                GUI.DragWindow();
        }

        public void EditorCalc()
        {
            editorGUI.BreakShipIntoStages();
            editorGUI.HighlightAll();
            editorGUI.showEditorGUI = true;
            editorGUI.EditorGUIRect.height = 1; //reset the height

        }

    }*/
