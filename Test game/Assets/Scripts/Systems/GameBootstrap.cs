using UnityEngine;
using Unity.NetCode;

public class GameBootstrap : ClientServerBootstrap
{
    public override bool Initialize(string defaultWorldName)
    {
        AutoConnectPort = 7979;

        #if !UNITY_EDITOR && UNITY_SERVER
        var defaultWorld = CreateDefaultWorld(defaultWorldName);
        CreateServerWorld(defaultWorld, "ServerWorld");
        return true;
        #endif

        #if !UNITY_EDITOR && !UNITY_SERVER
        var defaultWorld = CreateDefaultWorld(defaultWorldName);
        CreateClientWorld(defaultWorld, "ClientWorld");
        return true;
        #endif

        #if UNITY_EDITOR
        return base.Initialize(defaultWorldName); // Use the regular bootstrap
        #endif
    }
}

