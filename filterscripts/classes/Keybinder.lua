print("Loading Keybinder.lua");

Keybinder = {};
Keybinder.__index = Keybinder;

playersInKeybinder = {};

---Creates an instance of Queue
function Keybinder.new(playerid)
	local newKeybinder = {};
	
	setmetatable(newKeybinder, Keybinder);
	
	newKeybinder.playerid = playerid;
	
	newKeybinder.keyOpenMenu = 0;
	newKeybinder.keySpeakLoud = 0;
	newKeybinder.keySpeakQuiet = 0;

	newKeybinder.drawHeadline = nil;
	newKeybinder.drawKey = nil;
	
	newKeybinder.progress = 0;
	
	return newKeybinder;
end

function Keybinder:start()
	FreezePlayer(self.playerid, 1);

	self.drawHeadline = CreatePlayerDraw(self.playerid, 3800, 3800, "Druecke die Taste fuer folgende Aktion:", "Font_Default.tga", 255, 255, 255);
	self.drawKey = CreatePlayerDraw(self.playerid, 3800, 4000, "INTERAKTIONSMENUE OEFFNEN", "Font_Default.tga", 255, 255, 255);
	
	ShowPlayerDraw(self.playerid, self.drawHeadline);
	ShowPlayerDraw(self.playerid, self.drawKey);
end

function Keybinder:next()
	self.progress = self.progress + 1;
	
	if self.progress == 1 then
		UpdatePlayerDraw(self.playerid, self.drawKey, 3800, 4000, "LAUTER SPRECHEN", "Font_Default.tga", 255, 255, 255);
		
	elseif self.progress == 2 then
		UpdatePlayerDraw(self.playerid, self.drawKey, 3800, 4000, "LEISER SPRECHEN", "Font_Default.tga", 255, 255, 255);
	end
end

function Keybinder:finish()
	DestroyPlayerDraw(self.playerid, self.drawHeadline);
	DestroyPlayerDraw(self.playerid, self.drawKey);
	
	playersInKeybinder[self.playerid] = nil;
	FreezePlayer(self.playerid, 0);
	
	return self.keyOpenMenu, self.keySpeakLoud, self.keySpeakQuiet;
end

function Keybinder:OnPlayerKey(playerid, keyDown, keyUp)
	if self.progress == 0 then
		self.keyOpenMenu = keyDown;
		self:next();
		
	elseif self.progress == 1 then
		self.keySpeakLoud = keyDown;
		self:next();
		
	elseif self.progress == 2 then
		self.keySpeakQuiet = keyDown;
		self:finish();
	end
end