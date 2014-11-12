print("Loading InventoryAmount.lua");

InventoryAmount = {};
InventoryAmount.__index = InventoryAmount;

---Creates an instance of InventoryAmount (special element for a MenuItem)
function InventoryAmount.new(amount, menu)
	local newInventoryAmount = {};
	
	setmetatable(newInventoryAmount, InventoryAmount);
	
	newInventoryAmount.amount = amount;
	
	if amount > 0 then
		newInventoryAmount.caption = string.format("+%d", amount);
	else
		newInventoryAmount.caption = tostring(amount);
	end
	
	newInventoryAmount.menu = menu;
	
	return newInventoryAmount;
end


---Checks whether this instance (as a MenuItem) can be executed
function InventoryAmount:checkExecConditions(playerid)
	return true;
end

---Creates a sub menu with the inventory of this category as items
function InventoryAmount:exec(playerid)
	self.menu.amount = self.menu.amount + self.amount;
	
	--fix the selected amount in borders (0 <= N <= max)
	if self.menu.amount < 0 then self.menu.amount = 0; end;
	if self.menu.maxAmount < self.menu.amount then self.menu.amount = self.menu.maxAmount; end;
	
	--update the menu headline
	self.menu:updateHeadline(playerid, string.format("%s (%d / %d)", self.menu.headlineItem.caption, self.menu.amount,  self.menu.maxAmount));
end