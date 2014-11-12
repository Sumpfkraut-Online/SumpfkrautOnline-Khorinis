local DBConnection = {};
DBConnection.__index = DBConnection

--global DBConnection instance --veraendert ceddy
instance_DBConnection = nil; 

----------------------------------------------------------------
--			Class Functions  	  	 				 	      --
---------------------------------------------------------------

--Constructor
function DBConnection:new()
    local newDBConnection = {};
	
	setmetatable(newDBConnection,DBConnection);
	self.sqlhandler = mysql_connect(DATABASE_HOST, DATABASE_USER, DATABASE_PASSWORD, DATABASE_NAME)
	
	return newDBConnection;
end
function DBConnection:checkMySQLConnection()
	if not self.sqlhandler or mysql_ping(self.sqlhandler) == 0 then -- We lost the connection to the MySQL server
		LogString("DBConnection","SQL-ServerPing failed");
		sqlhandler = mysql_connect(DATABASE_HOST, DATABASE_USER, DATABASE_PASSWORD, DATABASE_NAME) -- Connect to server
			if not sqlhandler or mysql_ping(sqlhandler) == 0 then
					LogString("DBConnection","Could not reconnect to SQL-Server");
					return 0
			else
				LogString("DBConnection","SQL-Server Connection was restored");
				return 1
			end
	else 
		return 1
	end
end
function DBConnection:escapeString(text)
	return mysql_escape_string(self.sqlhandler, text);
end
 --Returns the Result of the SQL-CMD returns nil if an Error occurs
 function DBConnection:executeSQL(sqlCMD)
		if sqlCMD then
			if self:checkMySQLConnection()==1 then
				result = mysql_query(self.sqlhandler,sqlCMD);	
				if (not result) then
					message = string.format("%s %d %s","mysql_query failed: ",mysql_errno(self.sqlhandler),mysql_error(self.sqlhandler))-- Some error occurred
					LogString("DBConnection",message);
					LogString("DBConnection",sqlCMD);
					return nil;
				else		
					return result;
				end
			else
				LogString("DBConnection","Check Connection in executeSQL failed ");
				return nil;
			end
		else
			--CMD Command was a nil value
			LogString("DBConnection","tried to execute nil SQL-Script");
			return nil;
		end	
end



----------------------------------------------------------------
--			General Global Functions 				 	      --
----------------------------------------------------------------


function escapeString(text)
	if not text then return nil end;
	return instance_DBConnection:escapeString(text)
end
function runSQL(cmd)
	return instance_DBConnection:executeSQL(cmd)
end

-----------------------------------------------------------------------------------
--									Account related functions					 --
-----------------------------------------------------------------------------------
--Returns the Account id for a username. returns nil if that account does not exist;
 function getAccountIdByName(username)
	local sqlCMD = "SELECT accountID FROM account WHERE username='" .. escapeString(username) .. "'"
	local result = runSQL(sqlCMD)
	if mysql_num_rows(result) == 1 then 
		return mysql_fetch_row (result)[1]	;
	else
		return nil;
	end
	mysql_free_result(result) 
  end 

  --Returns the Name of an account by its ID
  --@params int Id
  --@return string name
function getAccountNameByID(id)
	local sqlCMD = "SELECT username FROM gothic_multiplayer.account WHERE accountID = "..id..";";
	local result = runSQL(sqlCMD)
	if mysql_num_rows(result) == 1 then 
		return mysql_fetch_row (result)[1]	;
	else
		return nil;
	end
	mysql_free_result(result) 
end  

 --Create a Account with this login Data
 function createAccount(username,password)
		--Prevent double registration of the same name
		if(getAccountIdByName(username)) then
			return 0
		end
		escapedName = escapeString(username)	
		hashedPW =  SHA256(password); -- Use SHA256 Algorithm to save the password in encrypted form
		result = runSQL("INSERT INTO `gothic_multiplayer`.`account` (`username`, `password`, `banned`,`lastLogin`, `rights`) VALUES ('"..escapedName.."', '"..hashedPW.."','false', '"..os.time().."', 'std');")
		if result then
			LogString("DBConnection","<DB> Account with name '"..escapedName.."' was registered") 
			return 1;
		end
		return 0;
  end
  --Checks if there is a Login with this Data
 function checkLoginData(playerid,username,password) 
		local hashedPW =  SHA256(password);
		local escapedName = escapeString(username); -- Escape the string to avoid security holes
		result = instance_DBConnection:executeSQL("SELECT * FROM gothic_multiplayer.account WHERE username = '"..escapedName.."' AND password = '"..hashedPW.."';");	
		if mysql_num_rows(result) == 1 then 
				mysql_free_result(result) -- Free the query result
				runSQL("UPDATE `gothic_multiplayer`.`account` SET  `lastLogin`='"..os.time().."' WHERE `username`='"..escapedName.."';");
				return 1;
		else	
				LogString("DBConnection","Failed Login by:" ..username) 
				return 0;
		end
		
  end
 
--OnFileLoad------------------------------------------------------
if not instance_DBConnection then
	instance_DBConnection= DBConnection:new()
	if instance_DBConnection:checkMySQLConnection() == 1 then
		print("<DBConnection> loaded and instantiated (instance_DBConnection variable name)")
	end
end
  







