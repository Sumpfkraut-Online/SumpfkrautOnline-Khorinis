print("Loading StaticMenuItem.lua");

StaticMenuItem = {};
StaticMenuItem.__index = StaticMenuItem;

---Creates an instance of StaticMenuItem
function StaticMenuItem.new(headline)
	local newStaticMenuItem = {};
	
	setmetatable(newStaticMenuItem, StaticMenuItem);
	
	newStaticMenuItem.caption = headline;
	newStaticMenuItem.element = nil;
	
	return newStaticMenuItem;
end

---Dummy of exec()-function. Has to be filled by the calling menu.
function StaticMenuItem:exec(playerid)
	--do nothing
end