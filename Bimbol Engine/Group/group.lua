--[[
	
	Module: team.lua
	Autor: Bimbol
	
	Group System
	
]]--

local Player = {};

local TEAM = { DEFAULT = { color = {255, 255, 255}, tag = nil }, USER = {}, ACL = {} };
local ENABLE_TEAM = false;
local ENABLE_COLOR = false;

function group_Init(enable_color)
	if type(enable_color) == "boolean" then
		ENABLE_COLOR = enable_color;
		ENABLE_TEAM = true;
		for i = 0, MAX_PLAYERS - 1 do
			Player[i] = {};
			Player[i].groups = {};
			Player[i].acl = {};
		end
	else
		print("Error: Missing argument on function: group_Init");
	end
end

function restartGroup(playerid)
	if ENABLE_TEAM then
		removeAllPlayerGroup(playerid);
		for i = 1, #Player[playerid].acl do
			table.remove(Player[playerid].acl, 1);
		end
	end
end

function group_Connect(playerid)
	if ENABLE_TEAM then
		if ENABLE_COLOR then
			SetPlayerColor(playerid, TEAM.DEFAULT.color[1], TEAM.DEFAULT.color[2], TEAM.DEFAULT.color[3]);
		end
		if TEAM.DEFAULT.tag then
			SetPlayerName(playerid, TEAM.DEFAULT.tag..GetPlayerName(playerid));
		end
	end
end

function setDefaultGroupTag(tag)
	if type(tag) == "string" then
		TEAM.DEFAULT.tag = tag;
	else
		print("Error: Missing argument on function: setDefaultGroupTag");
	end
end

function setDefaultGroupColor(r, g, b)
	if type(tag) == "string" then
		TEAM.DEFAULT.color = { r, g, b };
	else
		print("Error: Missing argument on function: setDefaultGroupColor");
	end
end

-- ACL

function aclCreateGroup(id)
	if ENABLE_TEAM and id then
		for i, k in ipairs(TEAM.ACL) do
			if k.id == id then
				return false;
			end
		end
			table.insert(TEAM.ACL, { id = id, rights = {} });
			return true;
	else
		print("Error: Missing argument on function: aclCreateGroup");
	end
		return false;
end

function aclRemoveGroup(id)
	if ENABLE_TEAM and id then
		for i, k in ipairs(TEAM.ACL) do
			if k.id == id then
				table.remove(TEAM.ACL, i);
				return true;
			end
		end
	else
		print("Error: Missing argument on function: aclRemoveGroup");
	end
		return false;
end

function aclGetAllGroups()
	local acl = {};
	if #TEAM.ACL > 0 then
		for i, k in ipairs(TEAM.ACL) do
			table.insert(acl, k.id);
		end
			return acl;
	end
		return false;
end

function aclGiveGroupRights(id, rights, access)
	if ENABLE_TEAM and id and rights and type(access) == "boolean" then
		for i, k in ipairs(TEAM.ACL) do
			if k.id == id then
				for j, v in ipairs(k.rights) do
					if v.rights == rights then
						v.access = access;
						return true;
					end
				end
				table.insert(k.rights, { rights = rights, access = access });
				return true;
			end
		end
	else
		print("Error: Missing argument on function: aclGiveGroupRights");
	end
		return false;
end

function aclGetGroupRights(id, rights)
	if ENABLE_TEAM and id and rights then
		for i, k in ipairs(TEAM.ACL) do
			if k.id == id then
				for j, v in ipairs(k.rights) do
					if v.rights == rights then
						return v.access;
					end
				end
			end
		end
			return false;
	else
		print("Error: Missing argument on function: aclGetGroupRights");
	end
		return false;
end

function aclCheckExistGroup(id)
	if ENABLE_TEAM and id then
		for i, k in ipairs(TEAM.ACL) do
			if k.id == id then
				return true;
			end
		end
	else
		print("Error: Missing argument on function: aclCheckExistGroup");
	end
		return false;
end

function aclGivePlayerGroup(playerid, id)
	if ENABLE_TEAM and playerid and id then
		for i, k in pairs(Player[playerid].acl) do
			if k == id then
				return false;
			end
		end
			table.insert(Player[playerid].acl, id);
			return true;
	else
		print("Error: Missing argument on function: aclGivePlayerGroup");
	end
		return false;
end

function aclRemovePlayerGroup(playerid, id)
	if ENABLE_TEAM and playerid and id then
		for i, k in pairs(Player[playerid].acl) do
			if k == id then
				table.remove(Player[playerid].acl, i);
				return true;
			end
		end
	else
		print("Error: Missing argument on function: aclGivePlayerGroup");
	end
		return false;
end

function aclGetPlayerGroups(playerid)
	if ENABLE_TEAM and playerid then
		if #Player[playerid].acl > 0 then
			return Player[playerid].acl;
		end
	end
		return false;
end

function aclCheckPlayerGroup(playerid, id)
	if ENABLE_TEAM and playerid then
		for i, k in pairs(Player[playerid].acl) do
			if k == id then
				return true;
			end
		end
	end
		return false;
end

-- Group

function removeDefaultGroupTag(text)
	if text then
		if ENABLE_TEAM then
			if TEAM.DEFAULT.tag then
				local tag_b, tag_e = string.find(text, TEAM.DEFAULT.tag);
				if tag_b then
					local tag_len = string.len(TEAM.DEFAULT.tag);
					if (tag_e - tag_b) == tag_len - 1 then
						local nick_p, nick_e = string.sub(text, 0, tag_b-1), string.sub(text, tag_e+1, string.len(text));
						return nick_p..nick_e;
					else
						local special_char = (tag_len - (tag_e - tag_b + 1))/2;
						local nick_p, nick_e = string.sub(text, 0, tag_b - 1 - special_char), string.sub(text, tag_e + 1 + special_char, string.len(text));
						return nick_p..nick_e;
					end
				end
					return text;
			end
				return text;
		end
	else
		print("Error: Missing argument on function: removeDefaultGroupTag");
	end
		return false;
end

function addGroup(id, r, g, b, tag)
	if id then
		if ENABLE_TEAM then
			table.insert(TEAM.USER, { id = id, color = { r, g, b }, tag = tag, players = {}, ids = {} });
			return true;
		end
	else
		print("Error: Missing argument on function: addGroup");
	end
		return false;
end

function removeGroup(id)
	if id then
		if ENABLE_TEAM then
			for i, k in ipairs(TEAM.USER) do
				if k.id == id then
					OnGroupRemove(id, k.color[1], k.color[2], k.color[3], k.tag, k.players);
					if k.tag or ENABLE_COLOR then
						for j, pid in ipairs(k.ids) do
							if pid ~= -1 then
								if k.tag then
									SetPlayerName(pid, removeTagGroup(GetPlayerName(pid), id));
								end
								if ENABLE_COLOR then
									local group = getPlayerGroup(pid);
									if group then
										for g in pairs(group) do
											if group[g] == id then
												table.remove(group, g);
												break;
											end
										end
										for h, l in ipairs(TEAM.USER) do
											if l.id == group[1] then
												SetPlayerColor(pid, l.color[1], l.color[2], l.color[3]);
												break;
											end
										end
									else
										SetPlayerColor(pid, TEAM.DEFAULT.color[1], TEAM.DEFAULT.color[2], TEAM.DEFAULT.color[3]);
									end
								end
							end
						end
					end
					table.remove(TEAM.USER, i);
						return true;
				end
			end
		end
	else
		print("Error: Missing argument on function: removeGroup");
	end
		return false;
end

function removeTagGroup(text, id)
	if text and id then
		if ENABLE_TEAM then
			for i, k in ipairs(TEAM.USER) do
				if k.id == id then
					if k.tag then
						local tag_b, tag_e = string.find(text, k.tag);
						if tag_b then
							local tag_len = string.len(k.tag);
							if (tag_e - tag_b) == tag_len - 1 then
								local nick_p, nick_e = string.sub(text, 0, tag_b-1), string.sub(text, tag_e+1, string.len(text));
								return nick_p..nick_e;
							else
								local special_char = (tag_len - (tag_e - tag_b + 1))/2;
								local nick_p, nick_e = string.sub(text, 0, tag_b - 1 - special_char), string.sub(text, tag_e + 1 + special_char, string.len(text));
								return nick_p..nick_e;
							end
						end
					end
						return text;
				end
			end
				return false;
		end
	else
		print("Error: Missing argument on function: removeTagGroup");
	end
		return false;
end

function removeAllTagsGroup(playerid)
	if playerid then
		local name = GetPlayerName(playerid);
		for i in pairs(Player[playerid].groups) do
			name = removeTagGroup(name, Player[playerid].groups[i]);
		end
			return name;
	else
		print("Error: Missing argument on function: removeAllTagsGroup");
	end
		return false;
end

function getGroupColor(id)
	if id and ENABLE_TEAM then
		for i,k in ipairs(TEAM.USER) do
			if k.id == id then
				return k.color[1], k.color[2], k.color[3];
			end
		end
	end
		return false;
end

function getGroupTag(id)
	if id and ENABLE_TEAM then
		for i,k in ipairs(TEAM.USER) do
			if k.id == id then
				return k.tag;
			end
		end
	end
		return false;
end

function setPlayerGroup(id, playerid)
	if id and playerid then
		if ENABLE_TEAM then
			for i, k in ipairs(TEAM.USER) do
				if k.id == id then
					local name = removeDefaultGroupTag(GetPlayerName(playerid));
					table.insert(k.players, removeAllTagsGroup(playerid));
					table.insert(k.ids, playerid);
					table.insert(Player[playerid].groups, id);
					if ENABLE_COLOR and k.color[1] then
						SetPlayerColor(playerid, k.color[1], k.color[2], k.color[3]);
					end
					if k.tag then
						SetPlayerName(playerid, k.tag .. name);
					end
					OnPlayerChangeGroup(playerid, id);
					return true
				end
			end
				return false;
		end
	else
		print("Error: Missing argument on function: setPlayerGroup");
	end
		return false;
end

function removePlayerGroup(id, playerid)
	if id and playerid then
		if ENABLE_TEAM then
			for i, k in ipairs(TEAM.USER) do
				if k.id == id then
					if ENABLE_COLOR then
						SetPlayerColor(playerid, TEAM.DEFAULT.color[1], TEAM.DEFAULT.color[2], TEAM.DEFAULT.color[3]);
					end
					if k.tag then
						SetPlayerName(playerid, removeTagGroup(GetPlayerName(playerid), id));
					end
					local name = removeAllTagsGroup(playerid);
					for j, v in ipairs(k.players) do
						if v == name then
							table.remove(k.players, j);
							break;
						end
					end
					for j, v in ipairs(k.ids) do
						if v == playerid then
							table.remove(k.ids, j);
						end
					end
					local group = getPlayerGroup(playerid);
					for g in pairs(group) do
						if group[g] == id then
							table.remove(group, g);
							break;
						end
					end
					if #group > 0 then
						for h, l in ipairs(TEAM.USER) do
							if l.id == group[1] and l.color[1] then
								SetPlayerColor(playerid, l.color[1], l.color[2], l.color[3]);
								break;
							end
						end
					end
					OnPlayerLeavesGroup(playerid, id, k.color[1], k.color[2], k.color[3], k.tag);
					return true;
				end
			end
				return false;
		end
	else
		print("Error: Missing argument on function: removePlayerGroup");
	end
		return false;
end

function removeAllPlayerGroup(playerid)
	if playerid then
		local group = getPlayerGroup(playerid);
		if group then
			for i in pairs(group) do
				removePlayerGroup(group[i], playerid);
			end
		end
	else
		print("Error: Missing argument on function: removeAllPlayerGroup");
	end
end

function getPlayerGroup(playerid)
	if playerid and ENABLE_TEAM then
		if Player[playerid].groups[1] then
			return Player[playerid].groups;
		end
	else
		print("Error: Missing argument on function: getPlayerGroup");
	end
		return false;
end

function checkPlayerGroup(playerid, id)
	if playerid and id and ENABLE_TEAM then
		for i, k in ipairs(Player[playerid].groups) do
			if k.id == id then
				return true;
			end
		end
	else
		print("Error: Missing argument on function: checkPlayerGroup");
	end
		return false;
end

function getGroupAmount(id)
	if id and ENABLE_TEAM then
		for i, k in ipairs(TEAM.USER) do
			if k.id == id then
				return #k.players;
			end
		end
	else
		print("Error: Missing argument on function: getGroupAmount");
	end
		return false;
end

function getGroupMembers(id)
	if id and ENABLE_TEAM then
		for i, k in ipairs(TEAM.USER) do
			if k.id == id then
				return k.players;
			end
		end
	else
		print("Error: Missing argument on function: getGroupMembers");
	end
		return false;
end

function getGroupMembersID(id)
	if id and ENABLE_TEAM then
		for i, k in ipairs(TEAM.USER) do
			if k.id == id then
				return k.ids;
			end
		end
	else
		print("Error: Missing argument on function: getGroupMembers");
	end
		return false;
end

function getGroupList(id)
	if id and ENABLE_TEAM then
		for i, k in ipairs(TEAM.USER) do
			if i == id then
				return k.id;
			end
		end
	else
		print("Error: Missing argument on function: getGroupList");
	end
		return false;
end

function getPlayeridGroups(name)
	if name then
		for i = 0, GetMaxSlots() - 1 do
			if IsPlayerConnected(i) == 1 then
				if removeAllTagsGroup(i) == name then
					return i;
				end
			end
		end
	end
		return -1;
end

-- Callbacks

function OnPlayerChangeGroup(playerid, group_id)
end

function OnGroupRemove(id, r, g, b, tag, players)
end

function OnPlayerLeavesGroup(playerid, id, r, g, b, tag)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");