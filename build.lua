baseDir = "Q:\\Coding\\Projects\\C#\\EngineerLevelFixer\\EngineerLevelFixer"
release = baseDir.."\\Release\\EngineerLevelFixer"
otherDir = "Q:\\Games\\Kerbal\\ModDev\\GameData\\EngineerLevelFixer"

function copyFile(from, to)
  os.execute("copy "..baseDir..from.." "..to.." /y")
end

copyFile("\\bin\\Debug\\EngineerLevelFixer.dll", release.."\\Plugins\\EngineerLevelFixer.dll")
copyFile("\\bin\\Debug\\EngineerLevelFixer.dll", otherDir.."\\Plugins\\EngineerLevelFixer.dll")
copyFile("\\EngineerLevelFixer.version", release)
copyFile("\\EngineerLevelFixer.version", otherDir)
copyFile("\\MM_AddModuleToWheels.cfg", release)
copyFile("\\MM_AddModuleToWheels.cfg", otherDir)

tArgs = {...}
print("Args[1]: "..(tArgs[1] or ""))
if tArgs[1] == "build" then
  versionFile = io.open(baseDir.."\\EngineerLevelFixer.version","r")
  if not versionFile then
    print("Version File Not Read!")
    io.read()
    error("File not found")
  end
  versionString = versionFile:read("*all")
  versionFile:close()

  versionMajor = versionString:match("MAJOR\":(%d+)")
  versionMinor = versionString:match("MINOR\":(%d+)")
  versionPatch = versionString:match("PATCH\":(%d+)")
  print("Major: "..versionMajor)
  print("Minor: "..versionMinor)
  print("Patch: "..versionPatch)

  os.execute("cd Release")
  os.execute("start /d Release 7za.exe a EngineerLevelFixer_v"..versionMajor.."."..versionMinor.."."..versionPatch..".zip EngineerLevelFixer")
  os.execute("cd ..")
else

  print("Running game with new files")
  os.execute("start /d Q:\\Games\\Kerbal\\ModDev /wait Q:\\Games\\Kerbal\\ModDev\\KSP_x64.exe")

end