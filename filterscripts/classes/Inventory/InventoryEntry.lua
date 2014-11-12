print("Loading InventoryEntry.lua");

InventoryEntry = {};
InventoryEntry.__index = InventoryEntry;

---Creates an instance of InventoryEntry
function InventoryEntry.new(instance, amount)
	local newInventoryEntry = {};
	
	setmetatable(newInventoryEntry, InventoryEntry);
	
	--check whether the handed value 'instance' is an instance name or a real instance
	if type(instance) == "string" then
		newInventoryEntry.inventoryItem = instance_InventorySystem.inventoryMan:getItemForInstanceName(instance);
	else
		newInventoryEntry.inventoryItem = instance;
	end
	
	if not newInventoryEntry.inventoryItem then return nil; end;
	
	newInventoryEntry.caption = newInventoryEntry.inventoryItem.caption;
	newInventoryEntry.amount = amount;
	
	return newInventoryEntry;
end

---Adds an amount to this instance's amount
function InventoryEntry:addAmount(amount)	
	self.amount = self.amount + amount;
end

---Removes an amount from this instance's amount
function InventoryEntry:removeAmount(amount)
	if amount < 1 then return 0; end;
	
	self.amount = self.amount - amount;
	
	if self.amount < 0 then
		self.amount = 0;
	end
	
	return self.amount;
end

---Sets the amount of this instance
function InventoryEntry:setAmount(amount)
  if amount > 0 then
    self.amount = amount;
  else
    self.amount = 0;
  end
end
