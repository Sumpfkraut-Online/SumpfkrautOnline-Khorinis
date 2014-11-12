print("Loading InventorySend.lua");

InventorySend = {};
InventorySend.__index = InventorySend;

---Creates an instance of InventorySend (special element for a MenuItem)
function InventorySend.new(menu)
	local newInventorySend = {};
	
	setmetatable(newInventorySend, InventorySend);
	
	newInventorySend.caption = "Absenden";
	newInventorySend.menu = menu;
	
	return newInventorySend;
end


---Checks whether this instance (as a MenuItem) can be executed
function InventorySend:checkExecConditions(playerid)
	return true;
end

---Creates a sub menu with the inventory of this category as items
function InventorySend:exec(playerid, secondPlayerID)
	if self.menu.amount > 0 then
		if self.menu.maxAmount >= self.menu.amount then
			RemoveItem(playerid, self.menu.instanceName, self.menu.amount);
			GiveItem(secondPlayerID, self.menu.instanceName, self.menu.amount);
			
			SendPlayerMessage(playerid, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("Du hast %s %dx %s gegeben.", instance_NameSystem:getPlayerNameFromViewOf(playerid, secondPlayerID), self.menu.amount, self.menu.instanceCaption));
			SendPlayerMessage(secondPlayerID, COLOR_SUCCESS.r, COLOR_SUCCESS.g, COLOR_SUCCESS.b, string.format("%s hat dir %dx %s gegeben.", instance_NameSystem:getPlayerNameFromViewOf(secondPlayerID, playerid), self.menu.amount, self.menu.instanceCaption));
			
		else
			--player has not enought items
			SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du hast nicht so viele Items.");
		end
		
		--exit the menu
		self.menu:exitMenu()
	else
		SendPlayerMessage(playerid, COLOR_FAILURE.r, COLOR_FAILURE.g, COLOR_FAILURE.b, "Du kannst einem Spieler nicht 0 Items geben.");
	end
end