local LoginSystem = {};
LoginSystem.__index = LoginSystem;

--Login Location
local spawnRoomX = 29796.;
local spawnRoomY = 4938;
local spawnRoomZ = -15411.;
local spawnAngel = 225;

local charedit = -1;
local debugMode = 1;
instance_LoginSystem=nil;


LogoutMessage = CreateDraw(700, 4400, "Du verlaesst den Server in 10 Sekunden.", "Font_Default.tga", 255, 255, 255);
--Draws Definition

function LoginSystem:new()
    local newLoginSystem = {};
	
	setmetatable(newLoginSystem,LoginSystem);
	newLoginSystem.Login_Message = CreateDraw(700, 4000, "Du musst dich erst einloggen oder registrieren um die Spielwelt betreten zu koennen.", "Font_Default.tga", 255, 255, 255);
	newLoginSystem.Login_Register = CreateDraw(700, 4400, "Registriere dich mit /register username password.", "Font_Default.tga", 255, 255, 255);
	newLoginSystem.Login_Login = CreateDraw(700, 4200, "Logge dich ein mit /login username password.","Font_Default.tga", 255, 255, 255);
	newLoginSystem.revieveMessange = CreateDraw(700, 4400, "Drücke V um den Spieler in deinem Focus wiederzubeleben", "Font_Default.tga", 255, 255, 255);
	newLoginSystem.Login_MessageLauncher = CreateDraw(700, 4000, "Drücke eine beliebige Taste um deinen Charakter zu laden.", "Font_Default.tga", 255, 255, 255);
	
	
	
	--Account Name
	newLoginSystem.userOnline ={};
	--Account ID
	newLoginSystem.accountOnline ={};
	--Is Launcherlogin 
	newLoginSystem.IsLauncherLogin={};
	
	
	--Block the first call of OnPlayerSpawn
	newLoginSystem.OnSpawnFailCall={};
	--Prevent the Onspawn Event from triggering multiple times for one Player.
	newLoginSystem.WasOnSpawnCalled={};
	
	
	newLoginSystem.playerArrowSaveTimer={};
	newLoginSystem.playerBoltSaveTimer={};

	newLoginSystem.RangeAmmoCheckID= 1;
	newLoginSystem.playerArrowCount ={}
	newLoginSystem.playerBoltCount ={}
	newLoginSystem.playerLastWeaponMode={}

	newLoginSystem.deathPlayers ={};

	--Prevents GiveItem calls of this module tracked by the db
	newLoginSystem.debugMode = 1;
	
	return newLoginSystem;
end
----------------------------------------------------------------------
--		Event Handling
----------------------------------------------------------------------
function LoginSystem:OnPlayerConnect(playerid)


		







	playerid = tonumber(playerid);
	if self then
		local token = escapeString(GetPlayerName(playerid))	
		local cmd = "SELECT accountID FROM gothic_multiplayer.launch_logintoken WHERE token_hash = '" .. tostring(token) .. "';"
		local result = runSQL(cmd);
		local row = mysql_fetch_row(result)
		if row then
			SetPlayerEnable_OnPlayerKey(playerid, 1)
			local accountID = row[1];
			local username =  getAccountNameByID(accountID);
			self.accountOnline[playerid]=accountID;
			self.userOnline[playerid]=username;
			cmd = "DELETE FROM `gothic_multiplayer`.`launch_logintoken` WHERE `token_hash`='" .. tostring(token) .. "';";
			runSQL(cmd);
			print("<Account-Sys>Laucher-Login from Player("..playerid..") : "..username.."("..accountID..")");
		else
			if debugMode == 0 then
				ExitGame(playerid);
			end
		end
		SpawnPlayer(playerid)
		if self:isPlayerLoggedIn(playerid) == 0 then
			self.IsLauncherLogin[playerid]=nil;
			self:showLogin(playerid,1,0);
		else
		    self.IsLauncherLogin[playerid]=1;
			self:showLogin(playerid,1,1);
			
		end
	else
		print("LoginSystem:OnPlayerConnect cannot be called without an object (self==nil)");
	end
end

function LoginSystem:OnPlayerDisconnect(playerid, reason)
	if self.userOnline[playerid] then
		print(self.userOnline[playerid].." was logged out on disconnect")
		self.userOnline[playerid]=nil;
		self.IsLauncherLogin[playerid]=nil;
	end
end

function LoginSystem:OnPlayerSpawn(playerid)
	if self:isPlayerLoggedIn(playerid)==0 then
		FreezePlayer(playerid, 1);
	end
end

--Reaktion to PlayerKey on Launcherlogin
function LoginSystem:OnPlayerKey(playerid)
	if self.IsLauncherLogin[playerid] then
		self.IsLauncherLogin[playerid]=nil;
		local username = self.userOnline[playerid];
		self.userOnline[playerid]=nil;
		self:loginUser(playerid,username);
	end
end



--Löscht den Spieler aus der Liste der eingeloggten User
function LoginSystem:logoutUser(playerid)
	if(self:isPlayerLoggedIn(playerid)==1) then
		--CharToDB(playerid,self.accountOnline[playerid],sqlhandler);
		LogString("account-system","<Account-Sys> Account "..self.userOnline[playerid].." with playerid :"..playerid.. " was logged out")
		self.userOnline[playerid] = nil;
		self.accountOnline[playerid]=nil;
		self.playerArrowCount[playerid]=nil;
		self.playerBoltCount[playerid]=nil;

	else
		LogString("account-system","<Account-Sys> Tried to log out Account  with playerid :"..playerid.. " which wasnt even logged in")	
	end
	
end

function LoginSystem:OnPlayerCommandText(playerid, cmdtext)
	local params = {}
	local counter = 0
	for id in cmdtext:gmatch("%S+") do 
		params[counter] = id
		counter  = counter + 1
	end
	cmd = params[0]

	if cmd == "/login" then
		self:loginCMD(playerid,params)
		return 1;
	elseif cmd == "/register" then
		self:registerCMD(playerid,params);
		return 1;	
	elseif cmd == "/quit"  or cmd == "/exit" then
		SetPlayerWeaponMode(playerid,WEAPON_NONE)
		FreezePlayer(playerid, 1)
		ShowDraw(playerid,LogoutMessage);
		SetTimerEx("quit",10000,0,playerid)
		return 1;
	elseif cmd == "/pos"  and debugMode == 1 then
		local x,y,z = GetPlayerPos(playerid);
        local message = string.format("%s %f %f %f","Meine Position ist:",x,y,z);
		SendPlayerMessage(playerid,0,255,0,message);
	    local angle = GetPlayerAngle(playerid);
        local message = string.format("%s %d","Mein Winkel ist:",angle);
	    SendPlayerMessage(playerid,0,255,0,message);
		return 1;
	else
		return 0;
	end	
	
end

---------------------------------------------------------------------------------------

function LoginSystem:isPlayerLoggedIn(playerid)
	if self.userOnline[playerid] then
		return 1;
	else
		return 0;
	end
end

function LoginSystem:loginUser(playerid,username)
	if  self:isUserOnline(username)==0 then
		print("login player("..playerid..") : "..username);
		--Mark account as online and save the useraccount id ind userOnline
		self.userOnline[playerid] = username;
		self.accountOnline[playerid] = getAccountIdByName(username);
		SetPlayerName(playerid, username)

		--CharFromDB(playerid,accountOnline[playerid],sqlhandler,1,1)
		LogString("account-system","<Account-Sys>"..username.." succesfully logged in  Playerid:"..playerid);
		
		SetPlayerEnable_OnPlayerKey(playerid, 1);
		Inventory.new(playerid,self.accountOnline[playerid]);
		Character:new(playerid,self.accountOnline[playerid]);
		
		if GetPlayerHealth(playerid) == 0 then
			print("player " .. playerid .. " logged in dead")
			deathPlayers[playerid] = 1;
		end
		
		self:showLogin(playerid, 0, 1);
		return 1;
	else
		return 0;
	end
end

--In work, needed for launcher login.
function LoginSystem:loginUserById(playerid,AccountID)
	local username =  getAccountNameByID(AccountID);
	print("Launcher Login: playerid is: "..playerid.." AccountId is: "..AccountID.." Accountname is: "..username);
	self:loginUser(tonumber(playerid),username)
end
	
--Returns 1 if the player with the given username is logged in
 function  LoginSystem:isUserOnline(username)
	for variable = 0, GetMaxPlayers(), 1 do
		if  self.userOnline[variable] == username then
			return 1
		end
	end
	return 0;
 end
	
function LoginSystem:loginCMD(playerid,params)

	if self:isUserOnline(params[1]) == 1 then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b,"Ein Spieler mit dem angegebenen Name ist bereits online.");
		return 0;
		
	elseif(self:isPlayerLoggedIn(playerid) == 1) then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du bist bereits eingeloggt.");
		return 0;
		
	else
		if(params[1] == nil)then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Unvollstaendige Login-Daten. Nutzername fehlt.");
			return 0;
			
		elseif(params[2]==nil) then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Unvollstaendige Login-Daten. Passwort fehlt.");
			return 0;
			
		else
			if(checkLoginData(playerid,params[1], params[2]) == 1) then
					self:loginUser(playerid, params[1]);
					SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Login erfolgreich.");
					return 1;
			else
					SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Ungueltige Login-Daten.");
					LogString("account-system","<Account-Sys>" .. params[1] .. "  logindata incorrect");
					return 0;
			end
		end
	end
end 






--Handles the register Chat Command
function LoginSystem:registerCMD(playerid,params)

	
	print("register cmd "..playerid)
		if(self:isUserOnline(params[1])==1) then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Ein Nutzer mit dem angegebenen Name ist bereits online.");
		return 0;
		
	elseif(self:isPlayerLoggedIn(playerid)==1) then
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du bist bereits eingeloggt.");
		return 0;
		
	else
		if(params[1]==nil)then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b,"Unvollstaendige Nutzerdaten. Nutzername fehlt.");
			return 0;
			
		elseif(params[2]==nil) then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Unvollstaendige Nutzerdaten. Passwort fehlt.");
			return 0;
			
		else
			if not getAccountIdByName(params[1]) then 
					SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, "Der Account wurde erfolgreich erstellt.");

					self:showLogin(playerid,0);
					createAccount(params[1],params[2]);
					self:loginUser(playerid,params[1]);
					SendMessageToAll(COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, username .. " ist in Khorinis angekommen ");
					return 1;
			else
					SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Dieser Account existiert bereits.");
					LogString("account-system","<Account-Sys>" .. params[1] .. " Account cannot be registered because it allready exists.");
					return 0;
			end

		end
	end
end

function LoginSystem:showLogin(playerid,enabled,typ)
		if enabled == 1 then
			if typ ==0 then 
				FreezePlayer(playerid, 1);		
				TeleportPlayerToWayPoint(playerid,"login");
				FreezePlayer(playerid, 1);
				PlayAnimation(playerid, "S_IDOL_S1")
				ShowDraw(playerid,  self.Login_Message);
				ShowDraw(playerid,  self.Login_Login);
				ShowDraw(playerid,  self.Login_Register);
			elseif typ==1 then		
				ShowDraw(playerid,  self.Login_MessageLauncher);
				FreezePlayer(playerid, 1)		
				TeleportPlayerToWayPoint(playerid,"login");
				FreezePlayer(playerid, 1)		
				PlayAnimation(playerid, "S_IDOL_S1")
			end
		else
			HideDraw(playerid,  self.Login_Message);
			HideDraw(playerid,  self.Login_Login);
			HideDraw(playerid,  self.Login_Register);
			HideDraw(playerid,  self.Login_MessageLauncher);
			FreezePlayer(playerid, 0)
			PlayAnimation(playerid, "S_RUN")
		end
end



function quit(playerid)
HideDraw(playerid,LogoutMessage);
ExitGame(playerid);
end

if not instance_LoginSystem then 
	instance_LoginSystem = LoginSystem:new()
	if instance_LoginSystem then
		print("<Login-System> loaded and instantiated (instance_LoginSystem variable name)");
	end
end


