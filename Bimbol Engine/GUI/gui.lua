--[[
	
	Module: gui.lua
	Autor: Bimbol
	
	GUI Types
	
]]--

local Player = {};
local GUI = false;

function gui_Init(value)
	if type(value) == "boolean" then
		GUI = true;
		for i = 0, MAX_PLAYERS - 1 do
			Player[i] = {};
			Player[i].Option_ID = {};
			Player[i].Open_Box = {};
			Player[i].Open_Input = {};
			Player[i].Open_Menu = {};
			Player[i].InputValue = {};
			Player[i].OpenedMenu = false;
		end
	end
end

function restartGUI(playerid)
	if GUI then
		for i = 1, #Player[playerid].Option_ID do
			table.remove(Player[playerid].Option_ID, 1);
		end
		for i = 1, #Player[playerid].Open_Box do
			table.remove(Player[playerid].Open_Box, 1);
		end
		for i = 1, #Player[playerid].Open_Input do
			table.remove(Player[playerid].Open_Input, 1);
		end
		for i = 1, #Player[playerid].Open_Menu do
			table.remove(Player[playerid].Open_Menu, 1);
		end
		local player_draw = GetDraw(playerid);
		for i = 1, #player_draw do
			if player_draw[1].type == "input" then
				Player[playerid].InputValue[player_draw[1].id] = nil;
			end
			table.remove(player_draw, 1);
		end
		Player[playerid].OpenedMenu = false;
	end
end

local HEIGHT_LINE = 200;
local COLOR = { 255, 255, 255 };
local GUIType = {};
local InputInfo = {};
local MenuInfo = {};

-- Box

function msgGUIBox(gui_id, gui_text, r, g, b, x, y, font, box_name, b_x, b_y, b_r, b_g, b_b, texture, time)
	if GUI then
		if gui_id and gui_text and x and y and r and g and b and font then
			for i, k in ipairs(GUIType) do
				if k.id == gui_id then
					print("Error: GUI with id: "..gui_id.." already exist.");
					return false;
				end
			end
			table.insert(GUIType, { id = gui_id, type = "msgBox", text = gui_text, x = x, y = y, texture = texture, font = font, color = { r, g, b }, color_box = { b_r, b_g, b_b }, box_name = box_name, bx = b_x, by = b_y, time = time });
			return true;
		else
			print("Error: Missing argument on function: msgGUIBox");
		end
	end
		return false;
end

function getOpenedGUIBox(playerid)
	if GUI then
		if #Player[playerid].Open_Box > 0 then
			return Player[playerid].Open_Box;
		end
	end
		return false;
end

function destroyGUIBox(gui_id)
	if GUI then
		if gui_id then
			for i, k in ipairs(GUIType) do
				if k.id == gui_id then
					table.remove(GUIType, i);
						return true;
				end
			end
		else
			print("Error: Missing argument on function: destroyGUIBox");
		end
	end
		return false;
end

function showGUIBox(playerid, gui_id, height_line)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and gui_id then
			local player_draw = GetDraw(playerid)
			for i, k in ipairs(player_draw) do
				if k.id == gui_id and k.type == "msgBox" then
					return false;
				end
			end
			for i, k in ipairs(GUIType) do
				if k.id == gui_id then
					if k.box_name then
						local msgBoxName = CreatePlayerDraw(playerid, k.bx, k.by, k.box_name, "Font_Old_20_White_Hi.TGA", k.color_box[1], k.color_box[2], k.color_box[3]);
						table.insert(player_draw, { id = gui_id, draw = msgBoxName, type = "msgBox_title" });
						ShowPlayerDraw(playerid, msgBoxName);
					end
					local y = 0;
					for j in pairs(k.text) do
						local draw_id = CreatePlayerDraw(playerid, k.x, y + k.y, k.text[j], k.font, k.color[1], k.color[2], k.color[3]);
						table.insert(player_draw, { id = gui_id, draw = draw_id, type = "msgBox" });
						ShowPlayerDraw(playerid, draw_id);
						y = y + (height_line or HEIGHT_LINE);
					end
					if k.texture then
						ShowTexture(playerid, k.texture);
					end
					if k.time then
						timerAdd("BEBOX" .. playerid, function()
							hideGUIBox(playerid, gui_id);
						end, k.time);
					end
				end
			end
			table.insert(Player[playerid].Open_Box, gui_id);
				return true;
		else
			print("Error: Missing argument on function: showGUIBox");
		end
	end
		return false;
end

function hideGUIBox(playerid, gui_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and gui_id then
			local player_draw = GetDraw(playerid);
			local tab = {};
			for i, k in ipairs(player_draw) do
				if k.id == gui_id and (k.type == "msgBox" or k.type == "msgBox_title") then
					table.insert(tab, i);
					DestroyPlayerDraw(playerid, k.draw);
				end
			end
			for i = #tab, 1, -1 do
				table.remove(player_draw, tab[i]);
			end
			for i, k in ipairs(GUIType) do
				if k.id == gui_id and k.texture then
					HideTexture(playerid, k.texture);
				end
			end
			for i, k in pairs(Player[playerid].Open_Box) do
				if k == gui_id then
					table.remove(Player[playerid].Open_Box, i);
					break;
				end
			end
				return true;
		else
			print("Error: Missing argument on function: hideGUIBox");
		end
	end
		return false;
end

function changeGUIBoxTitle(playerid, gui_id, title, r, g, b)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and gui_id and title then
			local player_draw = GetDraw(playerid);
			for i, k in ipairs(GUIType) do
				if k.id == gui_id then
					if k.box_name then
						for j, v in ipairs(player_draw) do
							if v.id == gui_id and v.type == "msgBox_title" then
								if r and g and b then
									UpdatePlayerDraw(playerid, v.draw, k.bx, k.by, title, "Font_Old_20_White_Hi.TGA", r, g, b);
								else
									UpdatePlayerDraw(playerid, v.draw, k.bx, k.by, title, "Font_Old_20_White_Hi.TGA", k.color_box[1], k.color_box[2], k.color_box[3]);
								end
									return true;
							end
						end
					end
				end
			end
		else
			print("Error: Missing argument on function: changeGUIBoxTitle");
		end
	end
		return false;
end

function changeGUIBoxText(playerid, gui_id, text, height_line)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and gui_id and type(text) == "table" then
			local player_draw = GetDraw(playerid);
			local draws = {};
			for i, k in ipairs(player_draw) do
				if k.id == gui_id and k.type == "msgBox" then
					table.insert(draws, i);
				end
			end
			for i, k in ipairs(GUIType) do
				if k.id == gui_id then
					local y = 0;
					if #draws > #text then
						local diff = #draws - #text;
						local count = #draws;
						local rem = count + 1 - diff;
						for j = rem, count do
							DestroyPlayerDraw(playerid, player_draw[draws[rem]].draw);
							table.remove(player_draw, draws[rem]);
						end
					end
					for j = 1, #text do
						if draws[j] and text[j] then
							UpdatePlayerDraw(playerid, player_draw[draws[j]].draw, k.x, k.y + y, text[j], k.font, k.color[1], k.color[2], k.color[3]);
						else
							local draw_id = CreatePlayerDraw(playerid, k.x, y + k.y, text[j], k.font, k.color[1], k.color[2], k.color[3]);
							table.insert(player_draw, { id = gui_id, draw = draw_id, type = "msgBox" });
							ShowPlayerDraw(playerid, draw_id);
						end
						y = y + (height_line or HEIGHT_LINE);
					end
						return true;
				end
			end
		else
			print("Error: Missing argument on function: changeGUIBoxText");
		end
	end
		return false;
end

function isOpenBox(playerid, gui_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and gui_id then
			for i, k in pairs(Player[playerid].Open_Box) do
				if k == gui_id then
					return true;
				end
			end
		else
			print("Error: Missing argument on function: isOpenBox");
		end
	end
		return false;
end

-- Input
function createGUIInput(input_id, x, y, r, g, b, font, callback)
	if GUI then
		if input_id and x and y and r and g and b and font then
			for i, k in ipairs(InputInfo) do
				if k.id == input_id then
					return false;
				end
			end
			table.insert(InputInfo, { id = input_id, type = "input", x = x, y = y, font = font, color = { r, g, b }, func = callback });
			return true;
		else
			print("Error: Missing argument on function: createGUIInput");
		end
	end
		return false;
end

function getOpenedGUIInput(playerid)
	if GUI then
		if #Player[playerid].Open_Input > 0 then
			return Player[playerid].Open_Input;
		end
	end
		return false;
end

function destroyGUIInput(input_id)
	if GUI then
		if input_id then
			for i, k in ipairs(InputInfo) do
				if k.id == input_id then
					if isOpenInput(playerid, input_id) then
						hideGUIInput(playerid, input_id);
					end
					table.remove(InputInfo, i);
						return true;
				end
			end
		else
			print("Error: Missing argument on function: destroyGUIInput");
		end
	end
		return false;
end

function enableGUIInput(playerid, input_id, value)
	if GUI then
		if playerid and input_id and type(value) == "boolean" then
			local player_draw = GetDraw(playerid);
			for i, k in ipairs(player_draw) do
				if k.id == input_id and k.type == "input" then
					k.enable = value;
					return true;
				end
			end
		else
			print("Error: Missing argument on function: enableGUIInput");
		end
	end
		return false;
end

function showGUIInput(playerid, input_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and input_id then
			local player_draw = GetDraw(playerid)
			for i, k in ipairs(player_draw) do
				if k.id == input_id and k.type == "input" then
					return false;
				end
			end
			for i, k in ipairs(InputInfo) do
				if k.id == input_id then
					local draw_id = CreatePlayerDraw(playerid, k.x, k.y, "", k.font, k.color[1], k.color[2], k.color[3]);
					table.insert(player_draw, { id = input_id, draw = draw_id, type = "input", enable = false });
					ShowPlayerDraw(playerid, draw_id);
					table.insert(Player[playerid].Open_Input, input_id);
					return true;
				end
			end
		else
			print("Error: Missing argument on function: showGUIIntup");
		end
	end
		return false;
end

function hideGUIInput(playerid, input_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and input_id then
			local player_draw = GetDraw(playerid);
			for i, k in ipairs(player_draw) do
				if k.id == input_id and k.type == "input" then
					DestroyPlayerDraw(playerid, k.draw);
					table.remove(player_draw, i);
					Player[playerid].InputValue[k.id] = nil;
				end
			end
			for i, k in pairs(Player[playerid].Open_Input) do
				if k == input_id then
					table.remove(Player[playerid].Open_Input, i);
					break;
				end
			end
				return true;
		else
			print("Error: Missing argument on function: hideGUIIntup");
		end
	end
		return false;
end

function updateGUIPlayerInput(playerid, input_id, text, update)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and input_id and text then
			local player_draw = GetDraw(playerid)
			for i, k in ipairs(player_draw) do
				if k.id == input_id and k.type == "input" then
					if update then
						Player[playerid].InputValue[input_id] = text;
					end
					for j, v in ipairs(InputInfo) do
						if v.id == k.id then
							UpdatePlayerDraw(playerid, k.draw, v.x, v.y, text, v.font, v.color[1], v.color[2], v.color[3])
							return true;
						end
					end
				end
			end
		else
			print("Error: Missing argument on function: updateGUIPlayerInput");
		end
	end
		return false;
end

function updateGUIInput(playerid, text)
	local player_draw = GetDraw(playerid)
	for i, k in ipairs(player_draw) do
		if k.type == "input" and k.enable then
			for j,v in ipairs(InputInfo) do
				if v.id == k.id then
					--UpdatePlayerDraw(playerid, k.draw, v.x, v.y, text, v.font, v.color[1], v.color[2], v.color[3]); -- Updating draw input disabled :)
					OnPlayerInputText(playerid, k.id, text, v.color[1], v.color[2], v.color[3]);
					Player[playerid].InputValue[k.id] = text;
					if v.func then
						v.func(playerid, k.id, text, v.color[1], v.color[2], v.color[3]);
					end
				end
			end
		end
	end
end

function getGUIInputText(playerid, input_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and input_id then
			return Player[playerid].InputValue[input_id];
		else
			print("Error: Missing argument on function: getGUIInputText");
		end
	end
		return false;
end

function isOpenInput(playerid, input_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and input_id then
			for i, k in pairs(Player[playerid].Open_Input) do
				if k == input_id then
					return true;
				end
			end
		else
			print("Error: Missing argument on function: isOpenInput");
		end
	end
		return false;
end

-- Menu
function createGUIMenu(menu_id, menu_text, r, g, b, r_on, g_on, b_on, x, y, font, texture, scroll_amount, callback)
	if GUI then
		if menu_id and menu_text and r and g and b and r_on and g_on and b_on and x and y and font and (type(callback) == "function" or callback == nil) then
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id then
					return false;
				end
			end
			local menu = {};
			local menu_c = {};
			for i in pairs(menu_text) do
				local temp_x, temp_y = x, y;
				local param = false;
				if menu_text[i][2] then
					temp_x = menu_text[i][2];
					param = true;
				end
				if menu_text[i][3] then
					temp_y = menu_text[i][3];
					param = true;
				end
				table.insert(menu, { menu_text[i][1], x = temp_x, y = temp_y, param = param });
				table.insert(menu_c, { r = r, g = g, b = b, r_on = r_on, g_on = g_on, b_on = b_on });
			end
			table.insert(MenuInfo, { id = menu_id, menu_text = menu, menu_color = menu_c, font = font, texture = texture, scroll_amount = scroll_amount, func = callback });
			return true;
		else
			print("Error: Missing argument on function: createGUIMenu");
		end
	end
		return false;
end

function getOpenedGUIMenu(playerid)
	if GUI then
		if #Player[playerid].Open_Menu > 0 then
			return Player[playerid].Open_Menu;
		end
	end
		return false;
end

function destroyGUIMenu(menu_id)
	if GUI then
		if menu_id then
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id then
					--if isOpenMenu(playerid, menu_id) then
					--	hideGUIMenu(playerid, menu_id);
					--end
					table.remove(MenuInfo, i);
						return true;
				end
			end
		else
			print("Error: Missing argument on function: destroyGUIMenu");
		end
	end
		return false;
end

function changeGUIMenuOptionColor(menu_id, option_id, r, g, b, r_on, g_on, b_on)
	if GUI then
		if menu_id and option_id then
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id then
					k.menu_color[option_id].r = r or k.menu_color[option_id].r;
					k.menu_color[option_id].g = g or k.menu_color[option_id].g;
					k.menu_color[option_id].b = b or k.menu_color[option_id].b;
					k.menu_color[option_id].r_on = r_on or k.menu_color[option_id].r_on;
					k.menu_color[option_id].g_on = g_on or k.menu_color[option_id].g_on;
					k.menu_color[option_id].b_on = b_on or k.menu_color[option_id].b_on;
				end
			end
		else
			print("Error: Missing argument on function: changeGUIMenuOptionColor");
		end
	end
		return false;
end

function showGUIMenu(playerid, menu_id, height_line)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and menu_id then
			-- Blockade showing more menu than 1
			--if Player[playerid].OpenedMenu ~= true then
				local player_draw = GetDraw(playerid);
				for i, k in ipairs(player_draw) do
					if k.id == menu_id and k.type == "menu" then
						return false;
					end
				end
				for i, k in ipairs(MenuInfo) do
					if k.id == menu_id then
						local y = 0;
						local menu_amount;
						if k.scroll_amount then
							menu_amount = k.scroll_amount;
						else
							menu_amount = #k.menu_text;
						end
						for j = 1, menu_amount do
							local draw_id;
							if j == 1 then
								if k.menu_text[j].param then
									draw_id = CreatePlayerDraw(playerid, k.menu_text[j].x, k.menu_text[j].y, k.menu_text[j][1], k.font, k.menu_color[j].r_on, k.menu_color[j].g_on, k.menu_color[j].b_on);
								else
									draw_id = CreatePlayerDraw(playerid, k.menu_text[j].x, y + k.menu_text[j].y, k.menu_text[j][1], k.font, k.menu_color[j].r_on, k.menu_color[j].g_on, k.menu_color[j].b_on);
								end
							else
								if k.menu_text[j].param then
									draw_id = CreatePlayerDraw(playerid, k.menu_text[j].x, k.menu_text[j].y, k.menu_text[j][1], k.font, k.menu_color[j].r, k.menu_color[j].g, k.menu_color[j].b);
								else
									draw_id = CreatePlayerDraw(playerid, k.menu_text[j].x, y + k.menu_text[j].y, k.menu_text[j][1], k.font, k.menu_color[j].r, k.menu_color[j].g, k.menu_color[j].b);
								end
							end
							table.insert(player_draw, { id = menu_id, draw = draw_id, type = "menu" });
							ShowPlayerDraw(playerid, draw_id);
							y = y + (height_line or HEIGHT_LINE);
						end
						if k.texture then
							ShowTexture(playerid, k.texture);
						end
						Player[playerid].Option_ID[menu_id] = 1;
						Player[playerid].OpenedMenu = true;
						OnPlayerOpenMenu(playerid, menu_id);
						table.insert(Player[playerid].Open_Menu, menu_id);
							return true;
					end
				end
			--end
		else
			print("Error: Missing argument on function: showGUIMenu");
		end
	end
		return false;
end

function hideGUIMenu(playerid, menu_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and menu_id then
			local player_draw = GetDraw(playerid);
			local tab = {};
			for i, k in ipairs(player_draw) do
				if k.id == menu_id and k.type == "menu" then
					table.insert(tab, i);
					DestroyPlayerDraw(playerid, k.draw);
				end
			end
			for i = #tab, 1, -1 do
				table.remove(player_draw, tab[i]);
			end
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id and k.texture then
					HideTexture(playerid, k.texture);
				end
			end
			Player[playerid].Option_ID[menu_id] = nil;
			Player[playerid].OpenedMenu = false;
			OnPlayerCloseMenu(playerid, menu_id);
			for i, k in pairs(Player[playerid].Open_Menu) do
				if k == menu_id then
					table.remove(Player[playerid].Open_Menu, i);
					break;
				end
			end
				return true;
		else
			print("Error: Missing argument on function: hideGUIMenu");
		end
	end
		return false;
end

function addGUIMenuOption(menu_id, text, place)
	if GUI then
		if menu_id and text then
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id then
					if place then
						table.insert(k.menu_text, place, text);
					else
						table.insert(k.menu_text, text);
					end
						return true;
				end
			end
		else
			print("Error: Missing argument on function: addGUIMenuOption");
		end
	end
		return false
end

function removeGUIMenuOption(menu_id, place)
	if GUI then
		if menu_id and place then
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id then
					if #k.menu_text > place and place < 1 then
						return false;
					end
						table.remove(k.menu_text, place);
						return true;
				end
			end
		else
			print("Error: Missing argument on function: removeGUIMenuOption");
		end
	end
		return false
end

function isOpenMenu(playerid, menu_id)
	if IsNPC(playerid) == 0 and GUI then
		if playerid and menu_id then
			for i, k in pairs(Player[playerid].Open_Menu) do
				if k == menu_id then
					return true;
				end
			end
		else
			print("Error: Missing argument on function: isOpenMenu");
		end
	end
		return false;
end

function switchGUIMenuUp(playerid, menu_id, height_line)
	if IsNPC(playerid) == 0 and GUI and Player[playerid].Option_ID[menu_id] then
		if playerid and menu_id then
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id then
					local player_draw = GetDraw(playerid);
					local y, menu_count = 0, 1;
					if Player[playerid].Option_ID[menu_id] - 1 == 0 then
						Player[playerid].Option_ID[menu_id] = #k.menu_text;
					else
						Player[playerid].Option_ID[menu_id] = Player[playerid].Option_ID[menu_id] - 1;
					end
					if k.scroll_amount and k.scroll_amount < Player[playerid].Option_ID[menu_id] then
						menu_count = menu_count + (Player[playerid].Option_ID[menu_id] - k.scroll_amount);
					end
					for j, v in ipairs(player_draw) do
						if v.id == menu_id and v.type == "menu" then
							if menu_count == Player[playerid].Option_ID[menu_id] then
								if k.menu_text[menu_count].param then
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r_on, k.menu_color[menu_count].g_on, k.menu_color[menu_count].b_on);
								else
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, y + k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r_on, k.menu_color[menu_count].g_on, k.menu_color[menu_count].b_on);
								end
							else
								if k.menu_text[menu_count].param then
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r, k.menu_color[menu_count].g, k.menu_color[menu_count].b);
								else
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, y + k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r, k.menu_color[menu_count].g, k.menu_color[menu_count].b);
								end
							end
							y = y + (height_line or HEIGHT_LINE);
							menu_count = menu_count + 1;
						end
					end
					OnPlayerSwitchMenu(playerid, menu_id, Player[playerid].Option_ID[menu_id]);
					if k.func then
						k.func(playerid, menu_id, Player[playerid].Option_ID[menu_id]);
					end
					return true;
				end
			end
		else
			print("Error: Missing argument on function: switchGUIMenuUp");
		end
	end
		return false;
end

function switchGUIMenuDown(playerid, menu_id, height_line)
	if IsNPC(playerid) == 0 and GUI and Player[playerid].Option_ID[menu_id] then 
		if playerid and menu_id then
			for i, k in ipairs(MenuInfo) do
				if k.id == menu_id then
					local player_draw = GetDraw(playerid);
					local y, menu_count = 0, 1;
					if #k.menu_text < Player[playerid].Option_ID[menu_id] + 1 then
						Player[playerid].Option_ID[menu_id] = 1;
					else
						Player[playerid].Option_ID[menu_id] = Player[playerid].Option_ID[menu_id] + 1;
					end
					if k.scroll_amount and k.scroll_amount < Player[playerid].Option_ID[menu_id] then
						menu_count = menu_count + (Player[playerid].Option_ID[menu_id] - k.scroll_amount);
					end
					for j, v in ipairs(player_draw) do
						if v.id == menu_id and v.type == "menu" then
							if menu_count == Player[playerid].Option_ID[menu_id] then
								if k.menu_text[menu_count].param then
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r_on, k.menu_color[menu_count].g_on, k.menu_color[menu_count].b_on);
								else
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, y + k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r_on, k.menu_color[menu_count].g_on, k.menu_color[menu_count].b_on);
								end
							else
								if k.menu_text[menu_count].param then
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r, k.menu_color[menu_count].g, k.menu_color[menu_count].b);
								else
									UpdatePlayerDraw(playerid, v.draw, k.menu_text[menu_count].x, y + k.menu_text[menu_count].y, k.menu_text[menu_count][1], k.font, k.menu_color[menu_count].r, k.menu_color[menu_count].g, k.menu_color[menu_count].b);
								end
							end
							y = y + (height_line or HEIGHT_LINE);
							menu_count = menu_count + 1;
						end
					end
					OnPlayerSwitchMenu(playerid, menu_id, Player[playerid].Option_ID[menu_id]);
					if k.func then
						k.func(playerid, menu_id, Player[playerid].Option_ID[menu_id]);
					end
					return true;
				end
			end
		else
			print("Error: Missing argument on function: switchGUIMenuDown");
		end
	end
		return false;
end

function getPlayerOptionID(playerid, menu_id)
	if Player[playerid].Option_ID[menu_id] and IsNPC(playerid) == 0 and GUI then
		return Player[playerid].Option_ID[menu_id];
	else
		return false;
	end
end

-- Callbacks

function OnPlayerInputText(playerid, intput_id, text, r, g, b)
end

function OnPlayerOpenMenu(playerid, menu_id)
end

function OnPlayerCloseMenu(playerid, menu_id)
end

function OnPlayerSwitchMenu(playerid, menu_id, option_id)
end

-- Loaded
print(debug.getinfo(1).source .. " has been loaded.")