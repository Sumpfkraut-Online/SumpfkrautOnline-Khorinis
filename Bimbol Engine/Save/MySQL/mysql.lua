--[[
	
	Module: mysql.lua
	Autor: Bimbol
	
	MySQL Save System
	
]]--

local MySQL_Connection = false
local MySQL_Info = {};
local MySQL_Handler;

function init_MySQL(hostname, username, password, database)
	if not password then password = "" end
	if hostname and username and password and database then
		MySQL_Handler = mysql_connect(hostname, username, password, database);
		if MySQL_Handler then
			MySQL_Info = { hostname, username, password, database };
			MySQL_Connection = true;
			print("Connected with MySQL server");
		else
			print("Error: Can't connect to the MySQL server");
		end
	else
		print("Error: Missing argument on function: init_MySQL");
	end
end

function checkMySQLConnection()
	if MySQL_Connection then
		if MySQL_Handler == nil or (mysql_ping(MySQL_Handler) == false) then
			OnMySQLLostConnection(MySQL_Info[1], MySQL_Info[2], MySQL_Info[3], MySQL_Info[4]);
			if MySQL_Handler then
				mysql_close(MySQL_Handler);
			end
			MySQL_Handler = mysql_connect(MySQL_Info[1], MySQL_Info[2], MySQL_Info[3], MySQL_Info[4]);
			checkMySQLConnection();
		end
	end
end

function createMySQLTable(tablename, table_arg, primary_key)
	if tablename and type(table_arg) == "table" then
		if MySQL_Connection then
			local arg = "CREATE TABLE IF NOT EXISTS "..tablename.." (";
			for i in pairs(table_arg) do
				if table_arg[i][1] and table_arg[i][1] then
					if i < #table_arg then
						arg = arg..table_arg[i][1].." "..table_arg[i][2]..", ";
					else
						arg = arg..table_arg[i][1].." "..table_arg[i][2];
					end
				else
					print("Error: Missing argument in SQL, on function createMySQLTable");
					return;
				end
			end
			if primary_key then
				arg = arg..", PRIMARY KEY("..primary_key.."))";
			else
				arg = arg..")";
			end
			
			checkMySQLConnection(); -- Check
			local result = mysql_query(MySQL_Handler, arg);
			if not(result) then
				OnMySQLError(MySQL_Handler, arg);
			else
				mysql_free_result(result);
			end
		end
	else
		print("Error: Missing argument on function: createMySQLTable");
	end
end

function removeMySQLTable(tablename)
	if tablename then
		if MySQL_Connection then
			checkMySQLConnection(); -- Check
			local arg = "DROP TABLE "..tablename;
			local result = mysql_query(MySQL_Handler, arg);
			if not(result) then
				OnMySQLError(MySQL_Handler, arg);
			else
				mysql_free_result(result);
			end
		end
	else
		print("Error: Missing argument on function: removeMySQLTable");
	end
end

function clearMySQLTable(tablename)
	if tablename then
		if MySQL_Connection then
			checkMySQLConnection(); -- Check
			local arg = "TRUNCATE TABLE "..tablename;
			local result = mysql_query(MySQL_Handler, "TRUNCATE TABLE "..tablename);
			if not(result) then
				OnMySQLError(MySQL_Handler, arg);
			else
				mysql_free_result(result);
			end
		end
	else
		print("Error: Missing argument on function: removeMySQLTable");
	end
end

function insertMySQLTable(tablename, table_arg)
	if tablename and type(table_arg) == "table" then
		if MySQL_Connection then
			local arg = "INSERT INTO `"..tablename.."` (";
			for i in pairs(table_arg) do
				if i < #table_arg then
					arg = arg.."`"..table_arg[i][1].."`, ";
				else
					arg = arg.."`"..table_arg[i][1].."`) VALUES (";
				end
			end
			for i in pairs(table_arg) do
				if i < #table_arg then
					arg = arg.."'"..table_arg[i][2].."', ";
				else
					arg = arg.."'"..table_arg[i][2].."')";
				end
			end
			checkMySQLConnection(); -- Check
			local result = mysql_query(MySQL_Handler, arg);
			if not(result) then
				OnMySQLError(MySQL_Handler, arg);
			else
				mysql_free_result(result);
			end
		end
	else
		print("Error: Missing argument on function: insertMySQLTable");
	end
end

function updateMySQLTable(tablename, table_arg, condition)
	if tablename and type(table_arg) == "table" and condition then
		if MySQL_Connection then
			local arg = "UPDATE `"..tablename.."` SET ";
			for i in pairs(table_arg) do
				if i < #table_arg then
					arg = arg.."`"..table_arg[i][1].."` = '"..table_arg[i][2].."', ";
				else
					arg = arg.."`"..table_arg[i][1].."` = '"..table_arg[i][2].."' WHERE "..condition;
				end
			end
			checkMySQLConnection(); -- Check
			local result = mysql_query(MySQL_Handler, arg);
			if not(result) then
				OnMySQLError(MySQL_Handler, arg);
			else
				mysql_free_result(result);
			end
		end
	else
		print("Error: Missing argument on function: updateMySQLTable");
	end
end

function getMySQLTableRow(tablename, condition, select)
	if tablename then
		if MySQL_Connection then
			local arg = "";
			if condition then
				arg = arg.."SELECT ".. (select or "*") .." FROM "..tablename.." WHERE "..condition;
			else
				arg = arg.."SELECT ".. (select or "*") .." FROM "..tablename;
			end
			
			checkMySQLConnection(); -- Check
			local result = mysql_query(MySQL_Handler, arg);
			if not(result) then
				OnMySQLError(MySQL_Handler, arg);
				return false;
			else
				local row = mysql_fetch_assoc(result);
				mysql_free_result(result);
				return row;
			end
		end
	else
		print("Error: Missing argument on function: getMySQLTableRow");
	end
		return false;
end

function getMySQLTableAllRow(tablename, select)
	if tablename then		
		if MySQL_Connection then
			local arg = "";
			local table_amount = "SELECT COUNT(*) FROM `"..tablename.."`";
			checkMySQLConnection(); -- Check
			local result = mysql_query(MySQL_Handler, table_amount);
			if not(result) then
				OnMySQLError(MySQL_Handler, table_amount);
				return false;
			else
				local rows = {};
				local amount = mysql_fetch_row(result);
				mysql_free_result(result);
				result = mysql_query(MySQL_Handler, "SELECT " .. (select or "*") .. " FROM "..tablename);
				for i = 1, amount[1] do
					local row = mysql_fetch_assoc(result);
					table.insert(rows, row);
				end
				mysql_free_result(result);
				return rows;
			end
		end
	else
		print("Error: Missing argument on function: insertMySQLTable");
	end
		return false;
end

-- Callbacks
function OnMySQLLostConnection(hostname, username, password, database)
end

function OnMySQLError(handler, query)
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");


