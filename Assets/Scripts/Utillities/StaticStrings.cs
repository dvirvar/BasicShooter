using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public sealed class StaticStrings
{
    public sealed class Server
    {
        public static string localHostServer = @"http://10.0.0.2:3000/";
        public static string guyServer = @"http://77.126.53.181:3000/";
        public static string herokuServer = @"http://basicshooter.herokuapp.com/";
        public static string socket = @"ws://basicshooter.herokuapp.com/socket.io/?EIO=4&transport=websocket";
    }
    
    public sealed class Input
    {
        public static string horizontalMovement = "Horizontal";
        public static string verticalMovement = "Vertical";
        public static string horizontalLook = "Horizontal Look";
        public static string verticalLook = "Vertical Look";
        public static string cancel = "Cancel";
        public static string tab = "Tab";
        public static string submit = "Submit";
    }
    
    public sealed class Weapon
    {
        public static string fire = "LMB";
        public static string changeMode = "Change Mode";
        public static string reload = "Reload";
        public static string switchWeapon = "Switch Weapon";
    }
    
}

