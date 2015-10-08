using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EngineerLevelFixer
{
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

    public class ModuleFixedWheel : ModuleWheel
    {
        static ScreenMessageStyle textStyle = ScreenMessageStyle.UPPER_CENTER;

        public override void OnStart(StartState state)
        {
            this.Events["RepairWheel"].guiName = "Repair Wheel"; //Must do this here because KSPEvent is not overridden in declaration
            this.Events["RepairWheel"].unfocusedRange = 5f;
            base.OnStart(state);
        }

        public void RepairWheel()
        {
            DebugHelper.Debug("My RepairWheel function has been called");
            int engineerLevel = FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value;
            int requiredEngineerLevel = ConfigLoader.getEngineerLevel();

            DebugHelper.Debug(String.Concat("Engineer Level: ", engineerLevel.ToString()));
            DebugHelper.Debug(String.Concat("Science Skill: ", FlightGlobals.ActiveVessel.VesselValues.ScienceSkill.value.ToString()));
            DebugHelper.Debug(String.Concat("Pilot Skill: ", FlightGlobals.ActiveVessel.VesselValues.AutopilotSkill.value.ToString()));
            if (HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX || engineerLevel >= requiredEngineerLevel) //We want it always to succeed in Sandbox, otherwise succeed if level is high enough
            {
                DebugHelper.Debug("Repair Succeeded");
                this.wheels[0].repairWheel();
                this.isDamaged = false;
                this.Events["RepairWheel"].active = false;
            }
            else if (engineerLevel < 0) //For non-engineers. If not an engineer, engineer skill is -1
                ScreenMessages.PostScreenMessage("You must repair wheels with Engineer Kerbals!", 2.0F, textStyle);
            else //This means that the kerbal is an engineer, but not high enough level
                ScreenMessages.PostScreenMessage("Engineer Kerbal must be level " + requiredEngineerLevel + " to repair wheels \n Current Level: " + engineerLevel, 2.0F, textStyle);
        }
    }
}
