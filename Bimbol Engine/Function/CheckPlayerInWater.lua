--[[
	
	Module: CheckPlayerInWater.lua
	Autor: Bimbol
	
	Check if Player is in Water.
	
]]--

function CheckPlayerInWater(playerid)
	if playerid then
		local anim = GetPlayerAnimationID(playerid);
		if anim == 286 or
		   anim == 288 or
		   anim == 145 or
		   anim == 146 or
		   anim == 304 or
		   anim == 308
		then
			return true;
		end
	end
		return false;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");