require "scripts/libs/AIFunctions"

require "scripts/default_scripts/SpawnNewWorld"

--Dies ist mein Test!
function OnFilterscriptInit()
    
    print("--------------------");
    print("NPC Loaded");
    print("--------------------");
    SetServerDescription("NPC-Test-Server");
    SetGamemodeName("NPCS");
    SetRespawnTime(2147483647);
    
    Enable_OnPlayerKey(1);
    
    --NPC and AI
    InitAiSystem();
    InitNewWorldNPC();
    
    --AI_DEBUG_DURATION = true;--The AI_Timer will print the duration! It shouldnt go over 200, if it do, Init with InitAiSystem(false) or set down AI_NPCIT_SIZE (default: 70)
    --AI_NPCIT_SIZE = 55;--removed!
    --SpawnNPC(VLK_432_Moe(), "HAFEN");
    
    
end


function OnGamemodeExit()

    print("-------------------");
    print("Gamemode was exited");
    print("-------------------");
end

function OnPlayerChangeClass(playerid, classid)
 
end

function OnPlayerSelectClass(playerid, classid)
 
end

function OnPlayerConnect(playerid)
    AI_OnPlayerConnect(playerid);
    
    
    
    if(IsNPC(playerid) == 0)then
        SendPlayerMessage(playerid,193,70,230,"/spawn [waran, wolf, lurker, goblin, scavenger]");
        SendPlayerMessage(playerid,193,70,230,"/invisible to toogle if NPC can see you");
        SendPlayerMessage(playerid,193,70,230,"/orcattack to let the orcs attack the old village");
        SendPlayerMessage(playerid,193,70,230,"/help for mor informations");
        SetPlayerWorld(playerid, "COLONY.ZEN", "OCR_ARENABATTLE_INSIDE");
        SpawnPlayer(playerid);
    end
end

function OnPlayerDisconnect(playerid, reason)
        AI_OnPlayerDisconnect(playerid);
end


function OnPlayerKey(playerid, keydown)
    AI_OnPlayerKey(playerid, keydown);
    
    if(keydown == KEY_P)then
        StopTradingPlayers(0);
    end
end

function OnPlayerHit(playerid, killerid)
    AI_OnPlayerHit(playerid, killerid);
end

function OnPlayerHasItem(playerid, item_instance, amount, equipped, checkid)
    AI_OnPlayerHasItem(playerid, item_instance, amount, equipped, checkid);
end


function OnPlayerResponseItem(playerid, slot, item_instance, amount, equipped)
    AI_OnPlayerResponseItem(playerid, slot, item_instance, amount, equipped);
end

function ORC_TA_TEST(_playerid)
    
    if(TA_FUNC(_playerid, 06, 0, 19, 0))then
        AI_ClearStates(_playerid);
        AI_SETWALKTYPE(_playerid, 1);
        AI_GOTO(_playerid, "OCR_ARENABATTLE_INSIDE");
    end
    
    if(TA_FUNC(_playerid, 19, 0, 06, 0))then
        AI_ClearStates(_playerid);
        AI_SETWALKTYPE(_playerid, 1);
        if(AI_NPCList[_playerid].StartWP ~= nil)then
            AI_GOTO(_playerid, AI_NPCList[_playerid].StartWP);
        end
    end
end

function ORC_TA_TEST_END(_playerid)
    if(TA_FUNC(_playerid, 0, 0, 24, 0))then
        AI_ClearStates(_playerid);
        AI_SETWALKTYPE(_playerid, 1);
        if(AI_NPCList[_playerid].StartWP ~= nil)then
            AI_GOTO(_playerid, AI_NPCList[_playerid].StartWP);
        end
    end
end

function OnPlayerCommandText(playerid, cmdtext)
    local cmd,params = GetCommand(cmdtext);
    
    if(cmd == "/help")then
        if(params == "spawn")then
            SendPlayerMessage(playerid,193,70,230,"/spawn [waran, wolf, lurker, goblin, scavenger]");
        elseif(params == "goto")then
            SendPlayerMessage(playerid,193,70,230,"/goto waypoint [wpname]");
            SendPlayerMessage(playerid,193,70,230,"/goto world [colony, khorines, jhakendar]");
            
        else
            SendPlayerMessage(playerid,193,70,230,"/help [spawn, goto]");
        end
    end
    if(cmd == "/trade")then
        local focus = GetFocus(playerid);
        if(focus ~= -1)then
            LoadTradingPlayers(playerid, focus);
        end
    end
    
    if(cmd == "/invisible")then
        if(AI_PlayerList[playerid].Invisible)then
            AI_PlayerList[playerid].Invisible = false;
            SendPlayerMessage(playerid,193,70,230,"Invisible Off");
        else
            AI_PlayerList[playerid].Invisible = true;
            SendPlayerMessage(playerid,193,70,230,"Invisible On");
        end
        
    end
    if(cmd == "/gettime")then
        local hour,minute = GetTime();
        SendPlayerMessage(playerid,193,70,230,"Time: "..hour..":"..minute);
    end
    if(cmd == "/settime")then
        local spl = params:split(" ");
        spl[1] = trim(spl[1]);
        spl[2] = trim(spl[2]);
        if(#spl >= 2)then
            SetTime(tonumber(spl[1]), tonumber(spl[2]));
            SendPlayerMessage(playerid,193,70,230,"Set Time: "..spl[1]..":"..spl[2]);
        end
    end
    
    if(cmd == "/goto")then
        local spl = params:split(" ");
        spl[1] = trim(spl[1]);
        spl[2] = trim(spl[2]);
        if(spl[1] == "waypoint")then
            local wp = AI_WayNets[GetPlayerOrNPC(playerid).GP_World]:GetWaypoint(spl[2]);
            if(wp ~= nil)then
                SetPlayerPos(playerid, wp.x, wp.y, wp.z);
            end
        else
            local worlds = {};
            worlds["colony"] = "COLONY.ZEN";
            worlds["khorines"] = "NEWWORLD\\NEWWORLD.ZEN";
            worlds["jhakendar"] = "ADDON\\ADDONWORLD.ZEN";
            SetPlayerWorld(playerid, worlds[spl[2]], "");
        end
    end
    
    if(cmd=="/orcattack")then
        local f = true;
        list = GetNPCwithGuild(AI_GUILD_Orc);
            for key,val in ipairs(list) do
                if(val.GP_World == "COLONY.ZEN")then
                    if(val.DailyRoutine == ORC_TA_TEST)then
                        val.DailyRoutine = ORC_TA_TEST_END;
                        f = false;
                    else
                        val.DailyRoutine = ORC_TA_TEST;
                        f = true;
                    end
                end
            end
        if(f)then
            SendPlayerMessage(playerid,193,70,230,"Orcattack On");
        else
            SendPlayerMessage(playerid,193,70,230,"Orcattack Off");
        end
    end
    if(cmd == "/spawn")then
        local wp = AI_WayNets[GetPlayerWorld(playerid)]:GetNearestWP(playerid);
        SendPlayerMessage(playerid,193,70,230,"Spawned: "..params);
        if(params == "wolf")then
            SpawnNPC(Wolf(), wp.name, GetPlayerWorld(playerid));
        elseif(params == "goblin")then
            SpawnNPC(Gobbo_Green(), wp.name, GetPlayerWorld(playerid));
        elseif(params == "waran")then
            SpawnNPC(Waran(), wp.name, GetPlayerWorld(playerid));
        elseif(params == "lurker")then
            SpawnNPC(Lurker(), wp.name, GetPlayerWorld(playerid));
        else
            SpawnNPC(Scavenger(), wp.name, GetPlayerWorld(playerid));
        end
    end
end