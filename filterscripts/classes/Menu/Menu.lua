require "filterscripts/classes/Menu/Color"

print("Loading Menu.lua");

Menu = {};
Menu.__index = Menu;

---Creates an instance of menu
function Menu.new(playerID, secondPlayerID, animID, x, y, managingTable, parentMenu, interactWithSecondPlayer)
	local newMenu = {};
	
	setmetatable(newMenu, Menu);

	newMenu.playerID = playerID;
	
	--the player id of the second player (only needed for trainer-student-teach menu)
	newMenu.secondPlayerID = secondPlayerID;
	
	--the draw id of the second player draw
	newMenu.secondPlayerDraw = nil;
	
	--id of the current animation (only needed if interacting with a vob; else: -1)
	newMenu.animID = animID;
	
	--maximum content lines (excluding headline)
	newMenu.maxLines = MAX_LINES_IN_MENU;
	
	--space for items
	newMenu.headlineItem = nil;
	newMenu.items = {};
	newMenu.exitItem = nil;
	
	newMenu.x = x;
	newMenu.y = y;
	
	--indizes of the selected line and item
	newMenu.currentLine = -1;
	newMenu.currentItem = -1;
	
	--reference to the currently selected item
	newMenu.selectedItem = nil;
	
	--determines whether the menu is frozen or not
	newMenu.frozen = false;
	
	--contains a reference to the table which manages the players menu
	newMenu.managingTable = managingTable;
	
	--contains a reference to the parent menu (if nil <-> this is the menu on first level)
	newMenu.parentMenu = parentMenu;
	
	--contains a reference to the menu's sub menu (if nil <-> no sub menu)
	newMenu.submenu = nil;
	
	--contains the index of the item that was selected before opening a sub menu
	--newMenu.lastSelectedItem = 1;
	
	--determines whether this menu should interact with the second player or not
	--EXAMPLE: Teach menu should interact with the second player
	--EXAMPLE: Trade menu must not interact with the second player (-> he has his own menu)
	if not interactWithSecondPlayer then
		newMenu.interactWithSecondPlayer = 1;
	else
		newMenu.interactWithSecondPlayer = interactWithSecondPlayer;
	end
	
	--freeze the player
	FreezePlayer(playerID, 1);
	
	--freeze the second player
	if newMenu.interactWithSecondPlayer == 1 and secondPlayerID then
		FreezePlayer(secondPlayerID, 1);
	end
	
	
	return newMenu;
end

---Add an item to the menu
function Menu:addItem(menuItem)
	menuItem.colorSelected = COLOR_MENU_SELECTED_ITEM;
	menuItem.colorDeselected = COLOR_MENU_DESELECTED_ITEM;

	menuItem.x = self.x + SELECTION_INDENT_IN_MENU;
	
	table.insert(self.items, menuItem);
end

---Set the headline item
function Menu:setHeadline(headlineItem)
	headlineItem.colorSelected = COLOR_MENU_HEADLINE;
	headlineItem.colorDeselected = COLOR_MENU_HEADLINE;

	headlineItem.x = self.x;
	headlineItem.y = self.y;
	
	self.headlineItem = headlineItem;
end

---Set the exit item
function Menu:setExit(exitItem)
	exitItem.colorSelected = COLOR_MENU_SELECTED_ITEM;
	exitItem.colorDeselected = COLOR_MENU_DESELECTED_ITEM;

	exitItem.x = self.x;
	
	--set the function of this StaticMenuItem
	exitItem.exec = function (playerid)
						self:exitMenu();
					end
	
	self.exitItem = exitItem;
end

---Selects an item and deselects the 'old' one
function Menu:selectItem(item)

	--deselect "old" selected item
	if self.selectedItem then
		self.selectedItem:deselectItem(self.playerID);
	end

	--select "new" item
	self.selectedItem = item;
	item:selectItem(self.playerID);
end

---Create the draw instances
function Menu:createDraws()
	--create headline draw
	self.headlineItem:draw(self.playerID)
	
	--create item draws
	for i = 1, #self.items do
		self.items[i]:draw(self.playerID)
	end
	
	--create exit draw
	self.exitItem:draw(self.playerID)
	
	--create a draw for the second player
	if self.interactWithSecondPlayer == 1 and self.secondPlayerID then
		self.secondPlayerDraw = CreatePlayerDraw(self.secondPlayerID, self.x, self.y, instance_NameSystem:getPlayerNameFromViewOf(self.secondPlayerID, self.playerID) .. " bringt dir gerade etwas bei.", "Font_Default.tga", 255, 255, 255);

		ShowPlayerDraw(self.secondPlayerID, self.secondPlayerDraw);
	end
end

---Hide all draw instances
function Menu:hideDraws()
	--hide headline
	self.headlineItem:hide(self.playerID);
	
	--hide content lines
	for i = 1, #self.items do
		self.items[i]:hide(self.playerID);
	end
	
	--hide exit line
	self.exitItem:hide(self.playerID);
end

---Destroy all draw instances
function Menu:destroyDraws()
	--destroy headline
	self.headlineItem:destroy(self.playerID);
	
	--destroy content lines
	for i = 1, #self.items do
		self.items[i]:destroy(self.playerID);
	end
	
	--destroy exit line
	self.exitItem:destroy(self.playerID);
	
	--destroy the other players draw
	if self.interactWithSecondPlayer == 1 and self.secondPlayerDraw then
		DestroyPlayerDraw(self.secondPlayerID, self.secondPlayerDraw);
	end
end

---Update the draw instances from lowerBound to upperBound
function Menu:updateDraws(lowerBound, upperBound)
	--draw headline (if needed)
	self.headlineItem:show(self.playerID);
	
	--draw content lines
	for i = 1, #self.items do
		if lowerBound <= i and i <= upperBound then
			self.items[i]:update(self.playerID, self.y + (i - lowerBound + 1) * LINE_HEIGHT_IN_MENU);
		else
			self.items[i]:hide(self.playerID);
		end
	end

	--check whether to draw exit or not
	if upperBound > #self.items then
		--draw exit line
		self.exitItem:update(self.playerID, self.y + (#self.items - lowerBound + 2)* LINE_HEIGHT_IN_MENU);
	else
		self.exitItem:hide(self.playerID);
	end
end

---Draw the first N items of the menu and select the first one
function Menu:updateTop()
	self:selectItemWithIndex(1);
end

---Draw the last N items of the menu and select the exit item
function Menu:updateBottom()
  self:selectItemWithIndex(#self.items + 1);
end


---Selects the N-th item from the menu and draws all surrounding entries
function Menu:selectItemWithIndex(index)
	if index <= #self.items then
		if index <= self.maxLines then
			self:updateDraws(1, self.maxLines);
			self.currentLine = index;
			
		else
			self:updateDraws(index + 1 - self.maxLines, index);
			self.currentLine = self.maxLines;
		end
		
		self.currentItem = index;
		self:selectItem(self.items[self.currentItem]);
			
	else
		--select exit
		local lowerBound = #self.items - self.maxLines + 2;
		if lowerBound < 1 then lowerBound = 1; end;
	
		self:updateDraws(lowerBound, #self.items + 1);
		
		self.currentLine = self.maxLines;
		self.currentItem = #self.items + 1;
		self:selectItem(self.exitItem);
	end
end

---Navigate down in the menu
function Menu:navigateDown()
	--navigate the submenu
	if self.submenu then
		self.submenu:navigateDown();
		return;
	end
	
	--if the menu is frozen: do not react
	if self.frozen == true then return; end;
	
	local nextItem = self.currentItem + 1;
	local nextLine = self.currentLine + 1;
	
	if self.currentLine < self.maxLines and self.currentItem <= #self.items then
		if nextItem <= #self.items then
			--select the next item
			self:selectItem(self.items[nextItem]);
		else
			--select the exit item
			self:selectItem(self.exitItem);
		end
		
		self.currentItem = nextItem;
		self.currentLine = nextLine;
		
	elseif self.currentLine == self.maxLines and self.currentItem <= #self.items then
		--hide the item which will not be visible after shift
		self.items[nextItem - self.maxLines]:hide(self.playerID);
		
		self:updateDraws(nextItem - self.maxLines + 1, nextItem);
		
		if nextItem <= #self.items then
			--select the next item
			self:selectItem(self.items[nextItem]);
		else
			--select the exit item
			self:selectItem(self.exitItem);
		end
		
		self.currentItem = nextItem;
	
	elseif self.currentItem > #self.items then
		--exit elem is selected -> go to the first item
		self:updateTop();
	end
end

---Navigate up in the menu
function Menu:navigateUp()
	--navigate the submenu
	if self.submenu then
		self.submenu:navigateUp();
		return;
	end
	
	--if the menu is frozen: do not react
	if self.frozen == true then return; end;
	
	local nextItem = self.currentItem - 1;
	local nextLine = self.currentLine - 1;
	
	if self.currentLine > 1 and self.currentItem > 1 then
		self:selectItem(self.items[nextItem]);
		
		self.currentItem = nextItem;
		self.currentLine = nextLine;
		
	elseif self.currentLine == 1 and self.currentItem > 1 then

		--hide the item which will not be visible after shift
		if self.currentItem == #self.items - self.maxLines + 2 then
			--last item is selected		
			self.exitItem:hide(self.playerID);
		else		
			self.items[self.currentItem + self.maxLines - 1]:hide(self.playerID);
		end
		
		self:updateDraws(nextItem, nextItem - 1 + self.maxLines);
		
		self:selectItem(self.items[nextItem]);
		
		self.currentItem = nextItem;
	
	elseif self.currentItem <= 1 then
		--first item is selected --> go to the exit-item
    self:updateBottom();
	end
end


---Accept (->Execute) the selected option
function Menu:accept()
	--navigate the submenu
	if self.submenu then
		self.submenu:accept();
		return;
	end
	
	--if the menu is frozen: do not react
	if self.frozen == true then return; end;

	--execute the selected item
	local result = self:execSelectedItem(self.playerID, self.secondPlayerID);
			
	--the only value that can (or should) be returned as an result is a reference to a sub menu
	if result then
		result.parentMenu = self;
		self.submenu = result;
		self:hideDraws();
	end
end

---Exits the current (sub-) menu
function Menu:exitMenu()
	self:destroyDraws();
	
	if self.parentMenu then
		--reactivate the parent menu
		self.parentMenu.submenu = nil;
		self.parentMenu:selectItemWithIndex(self.parentMenu.currentItem);
	else
		--dont have a parent menu -> close self and all children
		
		--unfreeze the player(s)
		FreezePlayer(self.playerID, 0);
		
		if self.interactWithSecondPlayer == 1 and self.secondPlayerID then
			FreezePlayer(self.secondPlayerID, 0);
		end

		self.managingTable[self.playerID] = self.animID;
	end
end

---Exists this menu and all sub menus (Top-Down)
function Menu:exitMenuAndSubMenus()	
	self:destroyDraws();
	
	if self.submenu then
		self.submenu:exitMenuAndSubMenus();
	end
	
	if not self.parentMenu then
		--unfreeze the player(s)
		FreezePlayer(self.playerID, 0);
		
		if self.interactWithSecondPlayer == 1 and self.secondPlayerID then
			FreezePlayer(self.secondPlayerID, 0);
		end

		self.managingTable[self.playerID] = self.animID;
	end
end

---Exists this menu and all superior menus (Bottom-Up)
function Menu:exitMenuAndSuperiorMenus()
	self:destroyDraws();
	
	--destroy all sub menus first
	if self.submenu then
		self.submenu:exitMenuAndSubMenus();
	end
	
	if self.parentMenu then
		--close all superior menus
		self.parentMenu:exitMenuAndSuperiorMenus();
	else
		--top menu
		
		--unfreeze the player(s)
		FreezePlayer(self.playerID, 0);
		
		if self.interactWithSecondPlayer == 1 and self.secondPlayerID then
			FreezePlayer(self.secondPlayerID, 0);
		end

		self.managingTable[self.playerID] = self.animID;
	end
end

---Freezes the current menu and its sub menus
function Menu:freeze()
	self.frozen = true;
	
	if self.submenu then
		self.submenu:freeze();
	end
end

---Unfreezes the current menu and its sub menus
function Menu:unfreeze()
	self.frozen = false;
	
	if self.submenu then
		self.submenu:unfreeze();
	end
end

---Executes the selected item of the current (sub-) menu
function Menu:execSelectedItem(playerid, secondPlayerID, execElementDirectly)
	if self.submenu then
		return self.submenu:execSelectedItem(playerid, secondPlayerID, execElementDirectly);
	end
	
	return self.selectedItem:exec(playerid, secondPlayerID, execElementDirectly);
end

---Updates the text displayed in the headline of the menu
function Menu:updateHeadline(playerid, headlineText)
	local caption_old = self.headlineItem.caption;
	
	self.headlineItem.caption = headlineText;
	self.headlineItem:update(playerid, self.y);
	
	self.headlineItem.caption = caption_old;
end