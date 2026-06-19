using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTerminalCommands : MonoBehaviour
{
    public Terminal terminalScript;
    public static GameObject[] branchObjectsCache;

    [TerminalCommand("instance", "instance-(string InstanceName) Loads target instance\n")]
    public string Instance(string instanceName)
    {
        string returnString = "";
        InstanceChange.LoadInstance(instanceName, 3);
        returnString += "Queued load for instance '" + instanceName+"'";
        return returnString;
    }
    [TerminalCommand("reload", "reload-(string instance/game) Reloads either current instance or the game itself\n")]
    public string Reload(string option)
    {
        string returnString = "";
        switch (option)
        {
            case "instance":
                InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 3);
                returnString += "Queued reload for current instance ('" + SceneManager.GetActiveScene().name+"')";
                break;
            case "game":
                InstanceChange.LoadInstance("LoadAssets", 3);
                returnString += "Queued load for instance 'LoadAssets'\n(Static variables in memory will not be reset, that will require app reboot)";
                break;
        }
        return returnString;
    }
    [TerminalCommand("device-info", "Returns info on the system the game is running on\n")]
    public string DeviceInfo()
    {
        string returnString = "";
        returnString += "Device model: " + SystemInfo.deviceModel;
        returnString += "\nDevice type: " + SystemInfo.deviceType;
        returnString += "\nDevice battery status: " + SystemInfo.batteryStatus;
        returnString += "\nDevice operating system: " + SystemInfo.operatingSystem;
        returnString += "\nDevice operating system family: " + SystemInfo.operatingSystemFamily;
        returnString += "\nDevice vibration/haptic feedback support: " + SystemInfo.supportsVibration;
        returnString += "\nDevice system memory size: " + SystemInfo.systemMemorySize;
        return returnString;
    }
    [TerminalCommand("screen-info", "Returns info on the screen being used to run the application\n")]
    public string ScreenInfo()
    {
        string returnString = "";
        returnString += "\nDisplay systemResolution: x" + Display.displays[Utilities.GetCurrentDisplayNumber()].systemWidth + " y" + Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight;
        returnString += "\nDisplay renderingResolution: x" + Display.displays[Utilities.GetCurrentDisplayNumber()].renderingWidth + " y" + Display.displays[Utilities.GetCurrentDisplayNumber()].renderingHeight;
        returnString += "\nScreen.resolution: x" + Screen.width + " y" + Screen.height;
        returnString += "\nScreen.currentResolution.resolution: x" + Screen.currentResolution.width + " y" + Screen.currentResolution.height;
        return returnString;
    }
    [TerminalCommand("game-info", "Returns info on the current game\n")]
    public string GameInfo()
    {
        string returnString = "";
        try{
            Level levelScript = GameObject.Find("InstanceScripts").GetComponent<Level>();
            returnString += "Current level: " + (levelScript.level/levelScript.levelAntiCheatMultiplier).ToString();
            returnString += "\nLevel speed increase: " + levelScript.levelSpeedIncrease.ToString();
        }
        catch{
            returnString += "Level class not within currently loaded instance";
        }
        try{
            BaseMovement baseMovementScript = GameObject.Find("Player").GetComponent<BaseMovement>();
            returnString += "\nMovement speed: " + baseMovementScript.movementSpeed.ToString();
        }
        catch{
            returnString += "\nBaseMovement class not within currently loaded instance or is disabled";
        }
        try{
            CameraMove cameraMoveScript = GameObject.Find("CM vcam1").GetComponent<CameraMove>();
            returnString += "\nLevel speed: " + cameraMoveScript.moveSpeed.ToString();
        }
        catch{
            returnString += "\nCameraMove class not within currently loaded instance";
        }
        returnString += "\nScore multiplier: " + ((int)(PlatformPass.scoreIncreaseBonus + 1)).ToString();
        returnString += "\nTime played this game: " + StartTime.instanceTime;
        return returnString;
    }
    [TerminalCommand("played", "Returns how long player has been playing the game (In seconds)\n")]
    public string Played()
    {
        string returnString = "Time played: " + ((int)EncryptedPlayerPrefs.GetFloat("TimePlayed")).ToString()+" Seconds";
        return returnString;
    }
    [TerminalCommand("show-ad", "Shows currently loaded ad\n")]
    public string ShowAd()
    {
        string returnString = "Attempting to show ad. . .";
        try
        {
            GameObject.Find("Ads").GetComponent<AdsPlayer>().ShowAd();
            returnString += "\nSuccessfully located ad player";
        }
        catch
        {
            returnString += "\nAdsPlayer is not within currently loaded instance";
        }
        return returnString;
    }
    [TerminalCommand("ads-info", "Returns info on in-game ads\n")]
    public string AdsInfo()
    {
        string returnString = "";
        try
        {
            AdsPlayer adsPlayerScript = GameObject.Find("Ads").GetComponent<AdsPlayer>();
            returnString += "iosInterstitialID: " + adsPlayerScript.iosInterstitialID;
            returnString += "\nandroidInterstitialID: " + adsPlayerScript.androidInterstitialID;
            returnString += "\nCurrent Interstitial ID: " + adsPlayerScript.InterstitialID;
            AdsInitialiser AdsInitialiserScript = GameObject.Find("Ads").GetComponent<AdsInitialiser>();
            //returnString += "\nAndroid Ad service ID: " + AdsInitialiserScript.androidID;
            //returnString += "\niOS Ad service ID: " + AdsInitialiserScript.iosID;
            returnString += "\nTest mode enabled: " + AdsInitialiserScript.testMode;
        }
        catch
        {
            returnString += "\nAdsPlayer is not within currently loaded instance";
        }
        returnString += "\nadCurrentlyPlaying: " + AdsPlayer.adCurrentlyPlaying;
        returnString += "\nclientAttempingToPlayAd: " + AdsPlayer.clientAttempingToPlayAd;
        return returnString;
    }
    [TerminalCommand("db-info", "Returns info on database currently connected to\n")]
    public string DbInfo()
    {
        string returnString = "";
        returnString += "Logged in: " + PlayerManager_LL.currentlyLoggedIn;
        returnString += "\nPlayerID: " + EncryptedPlayerPrefs.GetString("PlayerID");
        return returnString;
    }
    [TerminalCommand("font", "font-(int FontSize) Changes the size of terminal font to given value\n")]
    public string Font(int fontSize)
    {
        TerminalGUI.terminalFontSize = fontSize;
        string returnString = "Set terminal GUI font size to " + fontSize.ToString();
        ReloadTerminalGUI();
        return returnString;
    }
    [TerminalCommand("reload-terminal-gui", "Reloads terminal GUI\n")]
    public string ReloadTerminalGUI()
    {
        string returnString = "Attempting to reload terminal GUI. . .";
        try
        {
            terminalScript.ReloadGUI();
            returnString = "\nReloaded terminal GUI";
        }
        catch
        {
            returnString = "\nFailed to reloaded terminal GUI";
        }
        return returnString;
    }
    [TerminalCommand("statistics", "Returns statistics on players played games\n")]
    public string Statistics()
    {
        string returnString = "";
        returnString += "adsPlayed: "+EncryptedPlayerPrefs.GetInt("adsPlayed", 0).ToString();
        returnString += "\nnoWifiGames: " + EncryptedPlayerPrefs.GetInt("noWifiGames", 0).ToString();
        returnString += "\nwithWifiGames: " + EncryptedPlayerPrefs.GetInt("withWifiGames", 0).ToString();
        return returnString;
    }
    [TerminalCommand("transform", "transform-(string GameObject.name)-(string position/rotation/scale)-(string set/move)-(float x)-(float y)-(float z) Either sets or moves position,rotation or scale of target GameObject with given values\n")]
    public string Transform(string name,string value,string action, float x, float y, float z)
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            name = name.Replace("_", " ");
            Transform target = null;
            try { target = GameObject.Find(name).transform; }
            catch { return "GameObject '" + name + "' does not exist within current instance or is disabled"; }
            if (target == null) { return "GameObject '"+name+"' does not exist within current instance or is disabled"; }
            switch (value)
            {
                case "position":
                    switch (action)
                    {
                        case "set":
                            target.position = new Vector3(x, y, z);
                            break;
                        case "move":
                            target.position += new Vector3(x, y, z);
                            break;
                        default:
                            return "'" + action + "' is not a valid action";
                    }
                    break;
                case "rotation":
                    switch (action)
                    {
                        case "set":
                            target.eulerAngles = new Vector3(x, y, z);
                            break;
                        case "move":
                            target.eulerAngles += new Vector3(x, y, z);
                            break;
                        default:
                            return "'" + action + "' is not a valid action";
                    }
                    break;
                case "scale":
                    switch (action)
                    {
                        case "set":
                            target.localScale = new Vector3(x, y, z);
                            break;
                        case "move":
                            target.localScale += new Vector3(x, y, z);
                            break;
                        default:
                            return "'" + action + "' is not a valid action";
                    }
                    break;
                default:
                    return "'" + value + "' is not a value within Transform class";
            }
            if (action == "move") { return string.Format("Changed {0} of '{1}' by ({2},{3},{4})",value,name,x,y,z); }
            else { return string.Format("Set {0} of '{1}' to ({2},{3},{4})", value, name, x, y, z); }
        }
        else
        {
            return "Manipulating transforms of GameObjects within 'Main' instance is not permitted";
        }
    }
    [TerminalCommand("branch", "branch-(string GameObject.name/instance) Returns names of each child within target GameObject or within currently loaded instance\n")]
    public string Branch(string name)
    {
        string returnString = "";
        if (name != "instance"){
            int idCheck = 0;
            Transform target = null;
            try{
                idCheck = int.Parse(name);
                if (idCheck <= branchObjectsCache.Length){
                    target = branchObjectsCache[idCheck].transform;
                }
                else{
                    return "Invalid branch ID\nIf target name is a single number, start name with '~'";
                }
            }
            catch{
                name = name.Replace("_", " ");
                name = name.Replace("~", "");
                try { target = GameObject.Find(name).transform; }
                catch { return "GameObject '" + name + "' does not exist within current instance or is disabled"; }
                if (target == null) { return "GameObject '" + name + "' does not exist within current instance or is disabled"; }
            }
            if (target.childCount == 0) { return "'" + name + "' does not contain any children"; }
            else { branchObjectsCache = new GameObject[target.childCount]; }
            for (int i = 0; i < target.childCount; i++){
                returnString += i + ") " + target.GetChild(i).gameObject.name;
                if (target.GetChild(i).childCount > 0) { returnString += " {" + target.GetChild(i).childCount + "}"; }
                returnString += "\n";
                branchObjectsCache[i] = target.GetChild(i).gameObject;
            }
        }
        else{
            GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
            if (objects.Length == 0) { return "Current instance does not contain any children. Where are you?"; }
            else { branchObjectsCache = new GameObject[objects.Length]; }
            for (int i = 0; i < objects.Length; i++){
                returnString += i + ") " + objects[i].name;
                if (objects[i].transform.childCount > 0) { returnString += " {" + objects[i].transform.childCount + "}"; }
                returnString += "\n";
                branchObjectsCache[i] = objects[i];
            }
        }
        return returnString;
    }
    [TerminalCommand("gameobject-info", "gameobject-info-(string GameObject.name)-(string position/rotation/scale/components) Returns info on target GameObjects target info\n")]
    public string GameObjectInfo(string name,string value)
    {
        name = name.Replace("_", " ");
        Transform target = null;
        try { target = GameObject.Find(name).transform; }
        catch { return "GameObject '" + name + "' does not exist within current instance or is disabled"; }
        if (target == null) { return "GameObject '" + name + "' does not exist within current instance or is disabled"; }
        switch (value)
        {
            case "position":
                return "The " + value + " of '" + name + "' is " + target.position;
            case "rotation":
                return "The " + value + " of '" + name + "' is " + target.eulerAngles;
            case "scale":
                return "The " + value + " of '" + name + "' is " + target.localScale;
            case "components":
                string text = "";
                int index = -1;
                foreach (var component in target.gameObject.GetComponents(typeof(Component))){
                    index++;
                    text += index+") "+component.GetType().ToString() + "\n";
                }
                return text;
            default:
                return "Value '" + value + "' is not a valid option";
        }
    }
    [TerminalCommand("input-type", "input-type-(int inputID) Sets mobile input type to given ID\n")]
    public string InputType(string id)
    {
        try{
            int setID = 0;
            setID = int.Parse(id);
            PlayerPrefs.SetInt("InputType", setID);
            InputManager.mobileMoveSystem = setID;
            return "Set mobile input type to " + setID;
        }
        catch{
            return "Given value is not a valid ID";
        }
    }
    [TerminalCommand("event", "event-(string event-name) Starts given event\n")]
    public string Event(string eventName)
    {
        if (Application.isEditor)
        {
            switch (eventName)
            {
                case "invert":
                    GameObject.Find("InstanceScripts").GetComponent<InvertColoursEvent>().activate = true;
                    break;
                case "hell":
                    GameObject.Find("InstanceScripts").GetComponent<HellEvent>().active = true;
                    break;
                default:
                    return "'" + eventName + "' is not a known event";
            }
            return "Started '" + eventName + "' event";
        }
        else
        {
            return "This build does not permit the usage of this command";
        }
    }
    [TerminalCommand("capfps", "cap-fps-(int cap)\n")]
    public string CapFPS(string cap)
    {
        int capInt = 0;
        try { capInt = int.Parse(cap); Application.targetFrameRate = capInt; PlayerPrefs.SetInt("FpsCap", capInt); return "Capped FPS to " + capInt.ToString() + ". Cap FPS to 0 to disable"; }
        catch { return "Given value needs to be an integer"; }
    }
#if !UNITY_ANDROID && !UNITY_IPHONE
    [TerminalCommand("exit", "Exits the application\n")]
    public string Exit()
    {
        Application.Quit();
        return "Quitting application. . .";
    }
#endif
    [TerminalCommand("ver", "Returns info on client version\n")]
    public string Ver()
    {
        return "Version: "+ClientVersionManager.version+"\nVersion check ID: "+ClientVersionManager.versionNumber;
    }
#if UNITY_IPHONE || UNITY_EDITOR
    [TerminalCommand("request-store-review", "Requests app store review. Only usable 3 times every year\n")]
    public string RequestReview()
    {
        try{
            UnityEngine.iOS.Device.RequestStoreReview();
            return "Requested store review";
        }
        catch{
            return "An error occured whilst trying to request store review";
        }
    }
#endif
    [TerminalCommand("login", "Attempts a login session to database\n")]
    public string Login()
    {
        try{
            PlayerManager_LL playerManagerScript = null;
            playerManagerScript = GameObject.Find("InstanceScripts").GetComponent<PlayerManager_LL>();
            if (playerManagerScript == null) { playerManagerScript = GameObject.Find("DB(LL) Manager").GetComponent<PlayerManager_LL>(); }
            if (playerManagerScript == null) { return "'PlayerManager_LL' Class could not be found within currently loaded instance"; }
            else {
                playerManagerScript.Login();
                return "Attempted to call login session. Use command 'db-info' to check status";
            }
        }
        catch{
            return "An error occured whilst trying to login to session";
        }
    }
    [TerminalCommand("logout", "Attempts a logout of database session\n")]
    public string Logout()
    {
        try{
            PlayerManager_LL playerManagerScript = null;
            playerManagerScript = GameObject.Find("InstanceScripts").GetComponent<PlayerManager_LL>();
            if (playerManagerScript == null) { playerManagerScript = GameObject.Find("DB(LL) Manager").GetComponent<PlayerManager_LL>(); }
            if (playerManagerScript == null) { return "'PlayerManager_LL' Class could not be found within currently loaded instance"; }
            else{
                playerManagerScript.Logout();
                return "Attempted to call logout session. Use command 'db-info' to check status";
            }
        }
        catch{
            return "An error occured whilst trying to logout of session";
        }
    }
    [TerminalCommand("run", "run-(string function) Runs given function\n")]
    public string Run(string function)
    {
        string returnString = "";
        switch (function)
        {
            case "resetalldata":
                PlayerPrefs.DeleteAll();
                InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 5);
                returnString += "Resetting all saved player data. . .";
                break;
            case "submitscoretoggle":
                PlayerPrefs.SetInt("disableScoreSubmission",  Utilities.BoolToInt(!Utilities.IntToBool(PlayerPrefs.GetInt("disableScoreSubmission", 0))));
                if (PlayerPrefs.GetInt("disableScoreSubmission") == 0) { returnString += "Enabled score submittion"; }
                else { returnString += "Disabled score submittion"; }
                break;
            case "unlockalltrophies":
                if (Application.isEditor)
                {
                    EncryptedPlayerPrefs.SetInt("HlMobile01", 99999);
                    InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 5);
                    returnString += "Unlocked all trophies";
                }
                else
                {
                    return "This build does not permit the usage of this command";
                }
                break;
            case "needcash":
                if (Application.isEditor)
                {
                    EncryptedPlayerPrefs.SetInt("Coins", 99999);
                    InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 5);
                    returnString += "Gave 99999 coins";
                }
                else
                {
                    return "This build does not permit the usage of this command";
                }
                break;
            default:
                returnString += "Unrecognised function '"+function+"'";
                break;
        }
        return returnString;
    }
    [TerminalCommand("next-coin", "Returns the number of platforms until the next coin spawns\n")]
    public string NextCoin()
    {
        return CoinsSpawn.platformsRequiredForNextCoin + " platforms until next coin spawns";
    }
    [TerminalCommand("skin", "Returns the currently saved skin data, as well as the amount of skins you currently have\n")]
    public string Skin()
    {
        return "Currently loaded skin data: "+ EncryptedPlayerPrefs.GetString("skin" + EncryptedPlayerPrefs.GetInt("SelectedSkin"))+"\nSkins: "+ EncryptedPlayerPrefs.GetInt("skinsUnlocked", 0).ToString();
    }
    [TerminalCommand("coins", "Returns how many coins you currently have\n")]
    public string Coins()
    {
        return "Coins: " + EncryptedPlayerPrefs.GetInt("Coins").ToString();
    }
}
//PlayerPrefs.SetInt("InputType", inputTypeDropdown.value);
