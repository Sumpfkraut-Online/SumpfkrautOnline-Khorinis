local MAX_SLOTS = GetMaxPlayers();

function OnFilterscriptInit()
	print("------------------------");
	print("Admin Panel 0.3 loaded");
	print("------------------------");
end

function LoopTwo()

	print("admin.lua: zalogowany =",gPlayer.zalogowany);
end

function OnFilterscriptExit()

    print("-----------------------");
	print("Admin Panel unloaded...");
	print("-----------------------");
end

function OnPlayerConnect(playerid)

end

function OnPlayerCommandText(playerid, cmdtext)

    local cmd,params = GetCommand(cmdtext);

	if cmdtext == "/ahelp" then
		CMD_AdminHelp(playerid);

	elseif cmd == "/kick" then
		CMD_Kick(playerid,params);

	elseif cmd == "/ban" then
		CMD_Ban(playerid,params);

	elseif cmd == "/kill" then
		CMD_Kill(playerid,params);

	elseif cmd == "/tp" then
		CMD_Teleport(playerid,params);

	elseif cmd == "/giveitem" then
		CMD_GiveItem(playerid,params);
	
	elseif cmd == "/time" then
		CMD_Time(playerid,params);
		
	elseif cmd == "/name" then
		CMD_Name(playerid,params);
	
	elseif cmd == "/color" then
		CMD_Color(playerid,params);
	end
end

function CMD_AdminHelp(playerid)

	if IsPlayerAdmin(playerid) == 1
	then
		for i = 0, 8
		do
			SendPlayerMessage(playerid,255,255,255,"");
		end

		SendPlayerMessage(playerid,255,255,255,"Admin help:");
		SendPlayerMessage(playerid,255,250,200,"/kick /ban /kill /tp /giveitem /name /color");
	else
		SendPlayerMessage(playerid,255,250,200,"(Server): You are not admin!");
	end
end

function CMD_Kick(playerid, params)

	if IsPlayerAdmin(playerid) == 1
	then
		local result,id,reason = sscanf(params,"ds");

		if result == 1
		then
			if IsPlayerConnected(id) == 1
			then
				for i = 0, MAX_SLOTS - 1
				do
					SendPlayerMessage(i,255,0,0,string.format("%s %s %s %s%s %s %s","(Server):",GetPlayerName(id),"has been kicked by",GetPlayerName(playerid),".","Reason:",reason));
				end

				Kick(id);
			else
				SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s","(Server): Player ID",id,"is not connected with server."));
			end
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /kick (playerid) (reason)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server): You are not admin!");
	end
end

function CMD_Ban(playerid, params)

	if IsPlayerAdmin(playerid) == 1
	then
		local result,id,reason = sscanf(params,"ds");

		if result == 1
		then
			if IsPlayerConnected(id) == 1
			then
				for i = 0, MAX_SLOTS - 1
				do
					SendPlayerMessage(i,255,0,0,string.format("%s %s %s %s%s %s %s","(Server):",GetPlayerName(id),"has been banned by",GetPlayerName(playerid),".","Reason:",reason));
				end

				Ban(id);
			else
				SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s","(Server): Player ID",id,"is not connected with server."));
			end
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /ban (playerid) (reason)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server): You are not admin!");
	end
end

function CMD_Kill(playerid, params)

	if IsPlayerAdmin(playerid) == 1
	then
		local result,id = sscanf(params,"d");

		if result == 1
		then
			if IsPlayerConnected(id) == 1
			then
				SendPlayerMessage(id,255,250,200,string.format("%s %s","You have been killed by",GetPlayerName(playerid)));
				SendPlayerMessage(playerid,255,250,200,string.format("%s %s","You killed",GetPlayerName(id)));
				SetPlayerHealth(id,0);
			else
				SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s","(Server): Player ID",id,"is not connected with server."));
			end
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /kill (playerid)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server): You are not admin!");
	end
end

function CMD_Teleport(playerid, params)

	if IsPlayerAdmin(playerid) == 1
	then
		local result,from_id,to_id = sscanf(params,"dd");

		if result == 1
		then
			if IsPlayerConnected(from_id) == 1
			then
				if IsPlayerConnected(to_id) == 1
				then
					SendPlayerMessage(from_id,255,250,200,string.format("%s %s","You have been teleported to",GetPlayerName(to_id)));
					SendPlayerMessage(to_id,255,250,200,string.format("%s %s","To you has teleported",GetPlayerName(from_id)));

					local x,y,z = GetPlayerPos(to_id);
					SetPlayerPos(from_id,x + 50,y,z);
				else
					SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s","(Server): Player ID",to_id,"is not connected with server."));
				end
			else
				SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s","(Server): Player ID",from_id,"is not connected with server."));
			end
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /tp (fromplayerid) (toplayerid)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server): You are not admin!");
	end
end

function CMD_GiveItem(playerid, params)

	if IsPlayerAdmin(playerid) == 1
	then
		local result,id,item,amount = sscanf(params,"dsd");

		if result == 1
		then
			if IsPlayerConnected(id) == 1
			then
				SendPlayerMessage(id,255,250,200,string.format("%s %d %s %s %s","You have received",amount,item,"from",GetPlayerName(playerid)));
				SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s %s %s","You gave",amount,item,"for",GetPlayerName(id)));
				GiveItem(id,item,amount);
			else
				SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s","(Server): Player ID",id,"is not connected with server."));
			end
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /giveitem (playerid) (iteminstance) (amount)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server): You are not admin!");
	end
end

function CMD_Time(playerid, params)

	if IsPlayerAdmin(playerid) == 1
	then
		local result,hour,minute = sscanf(params,"dd");
		
		if result == 1
		then		
			SendMessageToAll(255,250,200,string.format("%s %s %s %d:%02d","(Server):",GetPlayerName(playerid),"set time at",hour,minute));
			SetTime(hour,minute);
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /time (hour) (minute)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server): You are not admin!");
	end
end

function CMD_Name(playerid, params)

	if IsPlayerAdmin(playerid) == 1 then
		local result,id,name = sscanf(params,"ds");

		if result == 1 then
		
			if IsPlayerConnected(id) == 1 then
			
				local adminname = GetPlayerName(playerid);
				local beforename = GetPlayerName(id);
				if SetPlayerName(id,name) == 1 then
				
					SendMessageToAll(0,255,0,string.format("%s %s %s %s %s %s","Admin",adminname,"set name player",beforename,"to",name));
				else
					SendPlayerMessage(playerid,255,250,200,"(Server) Can't set name.");
				end
			else
				SendPlayerMessage(playerid,255,250,200,string.format("%s %d %s","(Server): Player ID",id,"is not connected with server."));
			end
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /name (playerid) (name)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server) You are not admin!");
	end
end

function CMD_Color(playerid, params)

	if IsPlayerAdmin(playerid) == 1 then
	
		local result,id,r,g,b = sscanf(params,"dddd");
	
		if result == 1 then
			if IsPlayerConnected(id) == 1 then
				
				local adminName = GetPlayerName(playerid);
				SendPlayerMessage(id,255,250,200,string.format("%s %d %d %d %s %s","Your color was changed to",r,g,b,"by",adminName));
				SetPlayerColor(id,r,g,b);
			end
		else
			SendPlayerMessage(playerid,255,250,200,"Use: /color (playerid) (r) (g) (b)");
		end
	else
		SendPlayerMessage(playerid,255,250,200,"(Server) You are not admin!");
	end
end