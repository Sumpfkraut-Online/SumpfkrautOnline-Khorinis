print("Loading Color.lua");

Color = {};
Color.__index = Color;

---Creates an instance of Color
function Color.new(r, g, b)
	local newColor = {};
	
	setmetatable(newColor, Color);

	newColor.r = r;
	newColor.g = g;
	newColor.b = b;
	
	return newColor;
end

---GETTERS AND SETTERS

function Color:getR()
	return self.r;
end

function Color:setR(r)
	self.r = r;
end

function Color:getG()
	return self.g;
end

function Color:setG(g)
	self.g = g;
end

function Color:getB()
	return self.b;
end

function Color:setB(b)
	self.b = b;
end

---Returns a string representation of this instance
function Color.toString(self)
	return string.format("(Red: %d, Green: %d, Blue: %d)", self.r, self.g, self.b);
end

--add toString-function to metatable
Color.__tostring = Color.toString;