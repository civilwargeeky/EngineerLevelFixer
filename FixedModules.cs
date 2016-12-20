using System;

namespace EngineerLevelFixer {
  internal class Properties {
    public static ScreenMessageStyle textStyle = ScreenMessageStyle.UPPER_CENTER;
  }

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
}
  