print("Loading MenuFunctions.lua");

--Add all systems that need a menu here:

---Navigates throught the menus
function navigateMenu(playerid, keyDown, managingTable)
	if keyDown == KEY_UP then
		managingTable[playerid]:navigateUp();
		
	elseif keyDown == KEY_DOWN then
		managingTable[playerid]:navigateDown();
	
	elseif keyDown == KEY_LEFT then
		
		--find the 'bottom'-menu
		local menu = managingTable[playerid];
			
		while menu.submenu do
			menu = menu.submenu;
		end
			
		--go back one level
		menu.exitItem:exec();
		
	elseif keyDown == KEY_RETURN or keyDown == KEY_RIGHT then
		managingTable[playerid]:accept();
  
elseif keyDown == 90 then -- 90 = POS1
  	--find the 'bottom'-menu
		local menu = managingTable[playerid];
			
		while menu.submenu do
			menu = menu.submenu;
		end
    
    menu:updateTop();
    
  elseif keyDown == KEY_END then
  	--find the 'bottom'-menu
		local menu = managingTable[playerid];
			
		while menu.submenu do
			menu = menu.submenu;
		end
    
    menu:updateBottom();
	end
end

---Checks if the player is in any of the known menus
function isInAnyMenu(playerid)
	if instance_CraftingSystem:isPlayerInValidMenu(playerid) == true then return true; end;
	if instance_TeachSystem:isPlayerInValidMenu(playerid) == true then return true; end;
	if instance_HuntSystem:isPlayerInValidMenu(playerid) == true then return true; end;
	if instance_InventorySystem:isPlayerInValidMenu(playerid) == true then return true; end;
	if instance_InteractionSystem:isPlayerInValidMenu(playerid) == true then return true; end;
	if instance_FarmingSystem:isPlayerInValidMenu(playerid) == true then return true; end;
	
	return false;
end

---This function is called before executing a menu item with execution delay to freeze the current menu
function freezePlayerMenu(playerid)
	if instance_CraftingSystem:isPlayerInValidMenu(playerid) then
		--freeeze menu
		instance_CraftingSystem.isInCraftingMenu[playerid]:freeze();
		
	elseif instance_TeachSystem:isPlayerInValidMenu(playerid) then
		--freeeze menu
		instance_TeachSystem.isInTeachMenu[playerid]:freeze();
		
	elseif instance_HuntSystem:isPlayerInValidMenu(playerid) then
		--freeeze menu
		instance_HuntSystem.isInHuntMenu[playerid]:freeze();
	end
end

---This function is called by SetTimeEx(..) after an execution delay to unfreeze the current menu
function unfreezePlayerMenu(playerid)
	if instance_CraftingSystem:isPlayerInValidMenu(playerid) then
	
		--defrost menu
		instance_CraftingSystem.isInCraftingMenu[playerid]:unfreeze();
		
		--execute the 'exec'-function of the selected MenuItem of the Menu
		instance_CraftingSystem.isInCraftingMenu[playerid]:execSelectedItem(playerid, nil, true);
		
	elseif instance_TeachSystem:isPlayerInValidMenu(playerid) then
		--defrost menu
		instance_TeachSystem.isInTeachMenu[playerid]:unfreeze();
		
		--execute the 'exec'-function of the selected MenuItem of the Menu
		instance_TeachSystem.isInTeachMenu[playerid]:execSelectedItem(playerid, nil, true);
		
	elseif instance_HuntSystem:isPlayerInValidMenu(playerid) then
		--defrost menu
		instance_HuntSystem.isInHuntMenu[playerid]:unfreeze();
		
		--execute the 'exec'-function of the selected MenuItem of the Menu
		instance_HuntSystem.isInHuntMenu[playerid]:execSelectedItem(playerid, nil, true);
	end
end