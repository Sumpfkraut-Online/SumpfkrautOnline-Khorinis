print("Loading GarbageCollector.lua");

---Starts the garbage collector timer
function loadGarbageCollector()
	SetTimer("collectGarbage", GARBAGE_COLLECTOR_INTERVAL * 1000, 1);
end

---Executes the garbage collector
function collectGarbage()
	local before = math.floor(collectgarbage("count"));
	
	collectgarbage()
	
	local after = math.floor(collectgarbage("count"));
	
	LogString("GC", string.format("LUA on server is using: %d KByte (%d -> %d = -%d %s)", after, before, after, math.floor(((before - after) / before) * 100), "%"));
end