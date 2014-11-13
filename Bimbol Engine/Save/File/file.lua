--[[
	
	Module: file.lua
	Autor: Bimbol
	
	File Save System
	
]]--

local files = {};

function checkFileExist(filename)
	if filename then
		for i, k in ipairs(files) do
			if k.fname == filename then
				return true;
			end
		end
	else
		print("Error: Missing argument on function: checkFileExist");
	end
		return false;
end

function openFile(filename, mode)
	if filename and mode then
		local file = io.open(filename, mode);
		if file then
			table.insert(files, { fname = filename, file = file });
			return true;
		end
			return false;
	else
		print("Error: Missing argument on function: openFile");
	end
		return false;
end

function closeFile(filename)
	if filename then
		for i, k in ipairs(files) do
			if k.fname == filename then
				k.file:close();
				table.remove(files, i);
				return true;
			end
		end
	else
		print("Error: Missing argument on function: closeFile");
	end
		return false;
end

function changeFileMode(filename, mode)
	if filename and mode then
		for i, k in ipairs(files) do
			if k.fname == filename then
				k.file:close();
				k.file = io.open(filename, mode);
				if k.file then
					return true;
				else
					return false;
				end
			end
		end
	else
		print("Error: Missing argument on function: changeFileMode");
	end
		return false;
end

function setFileLine(filename, line_number)
	if filename and line_number then
		for i, k in ipairs(files) do
			if k.fname == filename then
				local count = 0;
				k.file:seek("set");
				for line in k.file:lines() do
					count = count + 1;
					if line_number == count then
						return line;
					end
				end
			end
		end
	else
		print("Error: Missing argument on function: setFileLine");
	end
		return false;
end

function getFileNextLine(filename)
	if filename then
		for i, k in ipairs(files) do
			if k.fname == filename then
				local line = k.file:read("*l");
				if line then
					return line;
				end
			end
		end
	else
		print("Error: Missing argument on function: getFileNextLine");
	end
		return false;
end

function readFileLines(filename, params)
	if filename then
		for i, k in ipairs(files) do
			if k.fname == filename then
				local lines = {};
				for line in k.file:lines() do
					if params then
						local result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 = sscanf(line, params);
						if result then
							table.insert(lines, { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
						else
							print("Error: Can't read all arguments");
							return false;
						end
					else
						table.insert(lines, line);
					end
				end
					return lines;
			end
		end
	else
		print("Error: Missing argument on function: readFileLines");
	end
		return false;
end

function writeFileText(filename, text, next_line)
	if filename and text then
		for i, k in ipairs(files) do
			if k.fname == filename then
				if next_line then
					k.file:write(text.."\n");
				else
					k.file:write(text);
				end
				return true;
			end
		end
	else
		print("Error: Missing argument on function: writeFile");
	end
		return false;
end

function writeFileTable(filename, _table, line_argument)
	if filename and _table and type(_table) == "table" then
		local write_value = GetTableValue(_table);
		for i, k in ipairs(files) do
			if k.fname == filename then
				for i in pairs(write_value) do
					k.file:write(write_value[i].." ");
					if line_argument and i % line_argument == 0 then
						k.file:write("\n");
					end
				end
					return true;
			end
		end
	else
		print("Error: Missing argument on function: writeFileTable");
	end
		return false;
end

function findFileText(filename, text)
	if filename and text then
		for i, k in ipairs(files) do
			if k.fname == filename then
				k.file:seek("set");
				local counter = 0;
				for line in k.file:lines() do
					counter = counter + 1;
					if string.find(line, text) then
						return counter, line;
					end
				end
			end
		end
	else
		print("Error: Missing argument on function: findFileText");
	end
		return false;
end

function replaceFileText(filename, old_text, new_text, line_number)
	if filename and old_text and new_text and line_number then
		for i, k in ipairs(files) do
			if k.fname == filename then
				k.file:seek("set");
				local counter, result = 0, false;
				local lines = {};
				for line in k.file:lines() do
					counter = counter + 1;
					if counter == line_number then
						local text, tmp = string.gsub(line, old_text, new_text);
						if tmp ~= 0 then
							result = true;
						end
						table.insert(lines, text);
					else
						table.insert(lines, line);
					end
				end
				k.file:close();
				k.file = io.open(filename, "w+");
				for i in pairs(lines) do
					k.file:write(lines[i].."\n");
				end
					return result;
			end
		end
	else
		print("Error: Missing argument on function: replaceFileText");
	end
		return false;
end

function replaceFileAllText(filename, old_text, new_text)
	if filename and old_text and new_text then
		for i, k in ipairs(files) do
			if k.fname == filename then
				k.file:seek("set");
				local result = false;
				local lines = {};
				for line in k.file:lines() do
					local text, tmp = string.gsub(line, old_text, new_text);
					if tmp ~= 0 then
						result = true;
					end
					table.insert(lines, text);
				end
				k.file:close();
				k.file = io.open(filename, "w+");
				for i in pairs(lines) do
					k.file:write(lines[i].."\n");
				end
					return result;
			end
		end
	else
		print("Error: Missing argument on function: replaceFileAllText");
	end
		return false;
end

-- Loaded
print(debug.getinfo(1).source.." has been loaded.");


