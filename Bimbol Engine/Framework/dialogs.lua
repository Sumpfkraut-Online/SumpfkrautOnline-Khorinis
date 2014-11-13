--[[
	
	Module: dialogs.lua
	Autor: Bimbol
	
	Dialogs System
	
]]--

local Player = {};
local Dialogs = {};

local MY_NAME = "MY_NAME";
local KEY_SCROLL = false;

local scrollDialog;

function dialog_Init(key)
	if key then
		KEY_SCROLL = key;
		bindKeyGlobal(KEY_SCROLL, "DOWN", scrollDialog);
		for i = 0, MAX_PLAYERS - 1 do
			Player[i] = {};
			Player[i].ActiveDialogID = -1;
			Player[i].DialogTextID = -1;
			Player[i].DialogHeight = -1;
			Player[i].NPC = -1;
		end
	end
end

function restartDialog(playerid)
	if KEY_SCROLL then
		Player[playerid].ActiveDialogID = -1;
		Player[playerid].DialogTextID = -1;
		Player[playerid].DialogHeight = -1;
		Player[playerid].NPC = -1;
	end
end

function createDialog(dialog_id, text, x, y, r, g, b, issues, issues_x, issues_y, i_r, i_g, i_b, font, texture)
	if dialog_id and type(text) == "table" and x and y and type(issues) == "table" and issues_x and issues_y and r and g and b and font then
		for i, k in ipairs(Dialogs) do
			if k.id == dialog_id then
				print("Error: Dialog with id: "..dialog_id.." already exist.");
				return false;
			end
		end
		msgGUIBox(dialog_id.."BENGINE", text[1], r, g, b, x, y, font, issues[1], issues_x, issues_y, i_r, i_g, i_b, texture);
		table.insert(Dialogs, { id = dialog_id, text = text, issues = issues });
		print("Dialog ID: "..dialog_id.." was created successfully!");
			return true;
	else
		print("Error: Missing argument on function: createDialog");
	end
		return false;
end

function destroyDialog(dialog_id)
	if dialog_id then
		for i, k in ipairs(Dialogs) do
			if k.id == dialog_id then
				table.remove(Dialogs, i);
				return true;
			end
		end
	else
		print("Error: Missing argument on function: destroyDialog");
	end
		return false;
end

function startDialog(playerid, npcid, dialog_id)
	if playerid and npcid and dialog_id then
		for i, k in ipairs(Dialogs) do
			if k.id == dialog_id then
				showGUIBox(playerid, dialog_id.."BENGINE");
				Player[playerid].ActiveDialogID = i;
				Player[playerid].DialogTextID = 1;
				Player[playerid].NPC = npcid;
				OnPlayerBeginDialog(playerid, npcid, k.id, Dialogs[Player[playerid].ActiveDialogID].issues[Player[playerid].DialogTextID]);
					return true;
			end
		end
	else
		print("Error: Missing argument on function: startDialog");
	end
		return false;
end

function stopDialog(playerid)
	if playerid and Player[playerid].ActiveDialogID ~= -1 then
		hideGUIBox(playerid, Dialogs[Player[playerid].ActiveDialogID].id.."BENGINE");
		OnPlayerFinishDialog(playerid, Dialogs[Player[playerid].ActiveDialogID].id);
		Player[playerid].ActiveDialogID = -1;
		Player[playerid].DialogTextID = -1;
		Player[playerid].DialogHeight = -1;
			return true;
	else
		print("Error: Missing argument on function: stopDialog");
	end
		return false;
end

function scrollDialog(playerid)
	if playerid and Player[playerid].ActiveDialogID ~= -1 then
		Player[playerid].DialogTextID = Player[playerid].DialogTextID + 1;
		if Dialogs[Player[playerid].ActiveDialogID].issues[Player[playerid].DialogTextID] and Dialogs[Player[playerid].ActiveDialogID].text[Player[playerid].DialogTextID] then
			changeGUIBoxTitle(playerid, Dialogs[Player[playerid].ActiveDialogID].id.."BENGINE", Dialogs[Player[playerid].ActiveDialogID].issues[Player[playerid].DialogTextID]);
			changeGUIBoxText(playerid, Dialogs[Player[playerid].ActiveDialogID].id.."BENGINE", Dialogs[Player[playerid].ActiveDialogID].text[Player[playerid].DialogTextID]);
			OnPlayerScrollDialog(playerid, Player[playerid].NPC, Dialogs[Player[playerid].ActiveDialogID].id, Player[playerid].DialogTextID, Dialogs[Player[playerid].ActiveDialogID].issues[Player[playerid].DialogTextID]);
				return true;
		else
			hideGUIBox(playerid, Dialogs[Player[playerid].ActiveDialogID].id.."BENGINE");
			OnPlayerFinishDialog(playerid, Player[playerid].NPC, Dialogs[Player[playerid].ActiveDialogID].id, Dialogs[Player[playerid].ActiveDialogID].issues[Player[playerid].DialogTextID]);
			Player[playerid].ActiveDialogID = -1;
			Player[playerid].DialogTextID = -1;
			Player[playerid].DialogHeight = -1;
			Player[playerid].NPC = -1;
				return true;
		end
	end
		return false;
end

function getActiveDialogID(playerid)
	return Player[playerid].ActiveDialogID;
end

-- Callbacks
function OnPlayerBeginDialog(playerid, npcid, dialogid, issue) 
end

function OnPlayerFinishDialog(playerid, npcid, dialogid)
end

function OnPlayerScrollDialog(playerid, npcid, dialogid, dialog_textid, issue)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");