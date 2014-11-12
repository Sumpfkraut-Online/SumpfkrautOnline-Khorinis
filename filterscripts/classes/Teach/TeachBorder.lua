print("Loading TeachBorder.lua");

TeachBorder = {};
TeachBorder.__index = TeachBorder;

function TeachBorder.new(lwr, upr, cost)
	local newTeachBorder = {};
	
	setmetatable(newTeachBorder, TeachBorder);

	newTeachBorder.lower = lwr;
	newTeachBorder.upper = upr;
	newTeachBorder.cost = cost;
		
	return newTeachBorder;
end


function TeachBorder.toString(self)
	local result = string.format("Border: %d <= n <= %d, COST: %d", self.lower, self.upper, self.cost);
	
	return result;
end

--add toString-function to metatable
TeachBorder.__tostring = TeachBorder.toString;