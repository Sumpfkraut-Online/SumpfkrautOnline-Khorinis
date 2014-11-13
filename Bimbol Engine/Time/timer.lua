local Timer = {};

-- Timer Manager
function timerAdd(id, callback, time, all, rep, ...)
	table.insert(Timer, { id = id, callback = callback, count = 1, time = time * 10, arg = arg, all = all, rep = rep });
end

function timerRemove(id)
	for i, timer in ipairs(Timer) do
		if timer.id == id then
			table.remove(Timer, i);
			return;
		end
	end
end

function Timer.Call()
	for id, timer in ipairs(Timer) do
		if timer.count == timer.time then
			timer.count = 1;
			if timer.all then
				for _, pid in pairs(GetOnlinePlayers()) do
					timer.callback(pid, unpack(timer.arg));
				end
			else
				timer.callback(unpack(timer.arg));
			end
			if not timer.rep then table.remove(Timer, id); end
		else
			timer.count = timer.count + 1;
		end
	end
end

-- Timer
function TimerFunc()
	Timer.Call();
end

SetTimer("TimerFunc", 100, 1);

-- Loaded
print(debug.getinfo(1).source .. " has been loaded.");