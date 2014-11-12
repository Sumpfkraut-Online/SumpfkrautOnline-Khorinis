print("Loading ChatSystem.lua");

instance_ChatSystem = nil;

ChatSystem = {};
ChatSystem.__index = ChatSystem;

---Creates an instance of ChatSystem
function ChatSystem.new()
	local newChatSystem = {};
	
	--disable global chat
	EnableChat(0);
	
	setmetatable(newChatSystem, ChatSystem);
	
	return newChatSystem;
end


---Catches OnPlayerText-Event
function ChatSystem:OnPlayerText(playerid, text)
	local character = getCharacterById(playerid);
	
	if not character then return; end;
	
	local radius = CHAT_RADIUS_NORMAL;
	local sayString = "sagt:";
	local sayString2 = "sagst:";
	
	if character.voiceVolume == 2 then
		radius = CHAT_RADIUS_SHOUT;
		sayString = "ruft:";
		sayString2 = "rufst:";
		
	elseif character.voiceVolume == 0 then
		radius = CHAT_RADIUS_WHISPER;
		sayString = "fluestert:";
		sayString2 = "fluesterst:";
	end
	
	local distance = nil;
	local colorR = 0;
	local colorG = 0;
	local prefix = "";

	for i = 0, GetMaxPlayers() - 1 do
		if IsPlayerConnected(i) == 1 and i ~= playerid then
			distance = GetDistancePlayers(playerid, i);
			
			if distance <= radius then
				colorR = math.floor(255 * (distance / radius));
				colorG = 255 - colorR;
				prefix = string.format("%s %s", instance_NameSystem:getPlayerNameFromViewOf(i, playerid), sayString);
				
				SendPlayerMessage(i, colorR, colorG, 50, text, prefix);
			end
		end
	end
	
	SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, text, string.format("Du %s ", sayString2));
end

function ChatSystem:OnPlayerCommandText(playerid, cmdtext)
	local cmd, params = GetCommand(cmdtext);

	if cmd == "/me" then
		local result, value = sscanf(params,"s");
		
		self:emote(playerid, value);
		
	elseif cmd == "/report" then
		local result, value = sscanf(params,"s");
		
		self:report(playerid, value);
	
	elseif cmd == "/a" then
		local result, value = sscanf(params,"s");

		if instance_Animation:playAnimation(playerid, value) == 0 then
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, string.format("Die Animation %s ist nicht bekannt.", value));
		end
		
	else
		return 0;
	end
	
	return 1;
end

---Catches OnPlayerKey-Event
function ChatSystem:OnPlayerKey(playerid, keyDown)
	if keyDown == KEY_ADD then
		local character = getCharacterById(playerid);
		
		if character.voiceVolume == 0 then
			character.voiceVolume = character.voiceVolume + 1;
			
			SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, "Du sprichst jetzt in normaler Lautstaerke.");
			
		elseif character.voiceVolume == 1 then
			character.voiceVolume = character.voiceVolume + 1;
			
			SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, "Du sprichst jetzt laut.");
		end
		
	elseif keyDown == KEY_SUBTRACT then
		local character = getCharacterById(playerid);
		
		if character.voiceVolume == 1 then
			character.voiceVolume = character.voiceVolume - 1;
			
			SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, "Du sprichst jetzt leise.");
			
		elseif character.voiceVolume == 2 then
			character.voiceVolume = character.voiceVolume - 1;
			
			SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, "Du sprichst jetzt in normaler Lautstaerke.");
		end
	end
end

function SendMessageToAll_(r, g, b, message)
  if not message then return; end;
	
	local length = message:len();
  
  if length <= MAX_CHARS_PER_LINE then
			SendMessageToAll__(r, g, b, message);
			return;
		end
    
   local splitMsg = instance_ChatSystem:splitString(message);

	--send remaining lines
	for i = 1, #splitMsg do
		SendMessageToAll__(r, g, b, splitMsg[i]);
	end
end


---Sends a message to a player. Splits it into multiple lines (= messages) if necessary
function SendPlayerMessage_(playerid, r, g, b, message, prefix)
	if not message then return; end;
	
	local length = message:len();

	if prefix then
		length = length + prefix:len();
		
		if length <= MAX_CHARS_PER_LINE then
			SendPlayerMessage__(playerid, r, g, b, string.format("%s%s", prefix, message));
			return;
		end
		
		--send prefix-message only if prefix is defined
		if prefix then
			SendPlayerMessage__(playerid, r, g, b, prefix);
		end
	else
		if length <= MAX_CHARS_PER_LINE then
			SendPlayerMessage__(playerid, r, g, b, message);
			return;
		end
	end
	
	local splitMsg = instance_ChatSystem:splitString(message);

	--send remaining lines
	for i = 1, #splitMsg do
		SendPlayerMessage__(playerid, r, g, b, splitMsg[i]);
	end
end

---Executes an emotion by "/me"
function ChatSystem:emote(playerid, text)
	local distance = nil;
	local color = 0;
	local prefix = "";
		
	for i = 0, GetMaxPlayers() - 1 do
		if IsPlayerConnected(i) == 1 and i ~= playerid then
			distance = GetDistancePlayers(playerid, i);
			
			if distance <= EMOTION_RADIUS then
				color = math.floor(255 * (1 - distance / EMOTION_RADIUS));
				prefix = string.format("%s ", instance_NameSystem:getPlayerNameFromViewOf(i, playerid));
				
				SendPlayerMessage(i, color, color, color, text, prefix);
			end
		end
	end
		
	SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, text, string.format("%s ", GetPlayerName(playerid)));
end

---Sends a report message to all admins
function ChatSystem:report(playerid, text)

	--TODO: Send message ONLY to admin players
	
	local x,y,z = GetPlayerPos(playerid);
	x = math.floor(x);
	y = math.floor(y);
	z = math.floor(z);

	local position = string.format("(%s, %s, %s)", x, y, z);
	
	local character = nil;

	for i = 0, GetMaxPlayers() - 1 do
		if IsPlayerConnected(i) == 1 then
			character = getCharacterById(playerid);
			
			if character.rights >= 1 then
				SendPlayerMessage(i, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, text, string.format("%s %s meldet: ", GetPlayerName(playerid), position));
			end
		end
	end
	
	SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, text, string.format("Du %s meldest: ", position));
	
	LogString("Report", string.format("%s %s meldet: %s", GetPlayerName(playerid), position, text));
end

---Executes an emotion by "/me"
function ChatSystem:dice(playerid, text)
	local distance = nil;
	local color = 0;
	local prefix = "";
		
	for i = 0, GetMaxPlayers() - 1 do
		if IsPlayerConnected(i) == 1 and i ~= playerid then
			distance = GetDistancePlayers(playerid, i);
			
			if distance <= EMOTION_RADIUS then
				color = math.floor(255 * (1 - distance / EMOTION_RADIUS));
				prefix = string.format("%s wuerfelt: ", instance_NameSystem:getPlayerNameFromViewOf(i, playerid));
				
				SendPlayerMessage(i, color, color, color, text, prefix);
			end
		end
	end
		
	SendPlayerMessage(playerid, COLOR_NEUTRAL.r, COLOR_NEUTRAL.g, COLOR_NEUTRAL.b, text, "Du wuerfelst: ");
end

---Splits a string and returns a table with the single lines
function ChatSystem:splitString(message)
	local result = {};
	local length = message:len();
	
	local increment = MAX_CHARS_PER_LINE - 1;
	
	local currentIndex = 1;
	
	while currentIndex <= length do
		table.insert(result, string.sub(message, currentIndex, currentIndex + increment));
		currentIndex = currentIndex + MAX_CHARS_PER_LINE;
	end
	
	return result;
end


--Overwrite original SendMessageToAll function
SendMessageToAll__ = SendMessageToAll;
SendMessageToAll = SendMessageToAll_;

--Overwrite original SendPlayerMessage function
SendPlayerMessage__ = SendPlayerMessage;
SendPlayerMessage = SendPlayerMessage_;


--instantiate chat system
if not instance_ChatSystem then
	instance_ChatSystem = ChatSystem.new();
end
