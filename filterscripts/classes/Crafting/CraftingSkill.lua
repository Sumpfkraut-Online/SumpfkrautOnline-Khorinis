print("Loading CraftingSkill.lua");

CraftingSkill = {};
CraftingSkill.__index = CraftingSkill;

function CraftingSkill.new(skillID, amount)
	local newCraftingSkill = {};
	
	setmetatable(newCraftingSkill, CraftingSkill);
	
	newCraftingSkill.skillID = skillID;
	newCraftingSkill.amount = amount;
	
	return newCraftingSkill;
end