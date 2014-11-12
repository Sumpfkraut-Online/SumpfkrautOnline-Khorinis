print("Loading MenuItem.lua");

MenuItem = {};
MenuItem.__index = MenuItem;

---Creates an instance of MenuItem
function MenuItem.new(element)
	local newMenuItem = {};
	
	setmetatable(newMenuItem, MenuItem);
	
	newMenuItem.caption = element.caption;
	newMenuItem.element = element;
	
	--time that is needed to execute this item (in ms)
	newMenuItem.duration = element.duration;
	
	newMenuItem.colorSelected = nil;
	newMenuItem.colorDeselected = nil;
	newMenuItem.x = 0;
	newMenuItem.y = 0;
	
	newMenuItem.drawID = -1;
	
	newMenuItem.isVisible = false;
	
	return newMenuItem;
end

---Create the draw instance for the item
function MenuItem:draw(playerid)
	--set the draw ID
	self.drawID = CreatePlayerDraw(playerid, self.x, self.y, self.caption, "Font_Default.tga", self.colorDeselected:getR(), self.colorDeselected:getG(), self.colorDeselected:getB());
	
	return self.drawID;
end

---Destroy the draw instance
function MenuItem:destroy(playerid)
	DestroyPlayerDraw(playerid, self.drawID);
end

---Update the position of the item
--ATTENTION: x-value is not changed here.
function MenuItem:update(playerid, y)
	self.y = y;
	
	UpdatePlayerDraw(playerid, self.drawID, self.x, self.y, self.caption, "Font_Default.tga", self.colorDeselected:getR(), self.colorDeselected:getG(), self.colorDeselected:getB());
	
	self:show(playerid);
end

---Show the draw instance
function MenuItem:show(playerid)
	if self.isVisible == true then return; end;
	
	ShowPlayerDraw(playerid, self.drawID);
	
	self.isVisible = true;
end

---Hide the draw instance
function MenuItem:hide(playerid)
	if self.isVisible == false then return; end;
	
	HidePlayerDraw(playerid, self.drawID);
	
	self.isVisible = false;
end

---Select the item
function MenuItem:selectItem(playerid)
	--x is only changed here
	self.x = self.x - SELECTION_INDENT_IN_MENU;
	
	UpdatePlayerDraw(playerid, self.drawID, self.x, self.y, SELECTION_STRING_IN_MENU .. self.caption, "Font_Default.tga", self.colorSelected:getR(), self.colorSelected:getG(), self.colorSelected:getB());
end

--Deselect the item
function MenuItem:deselectItem(playerid)
	--... and here
	self.x = self.x + SELECTION_INDENT_IN_MENU;
	
	UpdatePlayerDraw(playerid, self.drawID, self.x, self.y, self.caption, "Font_Default.tga", self.colorDeselected:getR(), self.colorDeselected:getG(), self.colorDeselected:getB());
end

---Execute the item
function MenuItem:exec(playerid, secondPlayerID, execElementDirectly)
	
	--check whether the calling menu is single- or two-player-menu
	if secondPlayerID then
		--!ignore duration in a two-player-menu!
	
		return self.element:exec(playerid, secondPlayerID);
	else
		if execElementDirectly then
			--call exec()-function of the menu items ELEMENT
			self.element:exec(playerid);
		else
			--call the menu items exec()-function
			
			--check the conditions
			local result = self.element:checkExecConditions(playerid);
	
			--freeze the player for the time amount of duration
			--ATTENTION: only if the execution will be successful (e.g. successfully can craft something)
			if result == true then
				if self.duration then
					if self.duration > 0 then
						freezePlayerMenu(playerid);	
						SetTimerEx("unfreezePlayerMenu", self.duration, 0, playerid);
					else
						self.element:exec(playerid);
					end
				else
					return self.element:exec(playerid);
				end
			end
		end
	end
end