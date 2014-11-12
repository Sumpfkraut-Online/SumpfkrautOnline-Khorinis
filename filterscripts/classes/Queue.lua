print("Loading Queue.lua");

Queue = {};
Queue.__index = Queue;

---Creates an instance of Queue
function Queue.new()
	local newQueue = {};
	
	setmetatable(newQueue, Queue);
	
	newQueue.items = {};
	
	return newQueue;
end

---Adds a string to the queue
function Queue:offer(str)
	table.insert(self.items, str);
end

---Returns all items of the queue as a table
function Queue:poll()
	local result = {};
	
	for k, v in pairs(self.items) do
		table.insert(result, v);
	end
	
	self.items = {};
	
	return result;
end