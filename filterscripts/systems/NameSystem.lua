print("Loading NameSystem.lua");

instance_NameSystem = nil;

NameSystem = {};
NameSystem.__index = NameSystem;

---Creates an instance of NameSystem
function NameSystem.new()
	local newNameSystem = {};
	
	setmetatable(newNameSystem, NameSystem);
	
	return newNameSystem;
end

---Callback: OnPlayerKey
function NameSystem:OnPlayerFocus(playerid, focusid)
	local character = getCharacterById(playerid);
	if not character then return; end;
	
	if focusid ~= -1 then		
		if not character.otherPlayerNameDraw then
			character.otherPlayerNameDraw = CreatePlayerDraw(playerid, NAME_X_STANDARD, NAME_Y_STANDARD, "", "Font_Default.tga", 255, 255, 255);
		end
		
		if IsNPC(focusid) == 0 then
			UpdatePlayerDraw(playerid, character.otherPlayerNameDraw, NAME_X_STANDARD, NAME_Y_STANDARD, instance_NameSystem:getPlayerNameFromViewOf(playerid, focusid), "Font_Default.tga", 255, 255, 255);
			ShowPlayerDraw(playerid, character.otherPlayerNameDraw);
		else
			--hide the draw if an NPC is focused
			HidePlayerDraw(playerid, character.otherPlayerNameDraw);
		end
	else
		HidePlayerDraw(playerid, character.otherPlayerNameDraw);
	end
end

---Returns the name of a player [focusid] from the view of the player [playerid]
function NameSystem:getPlayerNameFromViewOf(playerid, focusid)
	if focusid ~= -1 then
		local character = getCharacterById(playerid);
		
    if character then
      if character:isFriend(focusid) == true then
        return GetPlayerName(focusid);
      end
    end
    
    local focusedCharacter = getCharacterById(focusid);
  
    if focusedCharacter then
      return string.format("Unbekannt #%d", focusedCharacter.accountID);
    end
	end
	
	return "Unbekannt";
end

--instantiate name system
if not instance_NameSystem then
	instance_NameSystem = NameSystem.new();
end
