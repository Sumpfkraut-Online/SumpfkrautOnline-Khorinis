print("Loading Timerecorder.lua");

local lastDay;
local newDay = false;

---Starts the time recorder timer
function loadTimerecorder()
	SetTimer("checkPlayersOnline", TIME_RECORDER_INTERVAL * 1000, 1);
end

---Checks which players are online and updates their day-online-time
function checkPlayersOnline()
	
	--initialize lastDay variable
	if not lastDay then lastDay = tonumber(os.date("%d")); end;
	
	
	--check if the day (of month) changed, if yes: new day
	if lastDay ~= tonumber(os.date("%d")) then
		lastDay = tonumber(os.date("%d"));
		newDay = true;
	else
		newDay = false;
	end
	
	for k, v in pairs(PlayerCharacters) do
		--only update a player if:
			--a) new day
			--b) player has not reached online time for a teach (this day)
		if v.dayOnlineTime < DAY_ONLINE_TIME_FOR_TEACH or newDay == true then
			v:updateDayOnlineTime(newDay, TIME_RECORDER_INTERVAL);
		end
	end
end