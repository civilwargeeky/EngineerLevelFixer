using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EngineerLevelFixer
{
    internal class Properties
    {
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
    public class ModuleFixedWheel : ModuleWheel
    {
        public void RepairWheel()
        {
            DebugHelper.Debug("My RepairWheel function has been called");
            int engineerLevel = FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value;
            int requiredEngineerLevel = ConfigLoader.getWheelLevel();

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
                ScreenMessages.PostScreenMessage("You must repair wheels with Engineer Kerbals!", 2.0F, Properties.textStyle);
            else //This means that the kerbal is an engineer, but not high enough level
                ScreenMessages.PostScreenMessage("Engineer Kerbal must be level " + requiredEngineerLevel + " to repair wheels \n Current Level: " + engineerLevel, 2.0F, Properties.textStyle);
        }
    }

    public class ModuleFixedLeg: ModuleLandingLeg
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
}
