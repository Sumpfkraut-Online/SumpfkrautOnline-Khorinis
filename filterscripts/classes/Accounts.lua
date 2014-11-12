--Another Singleton to for checking Login Data etc;
Accounts = {};
local _instance;
accountIDs={};
accountPWs = {};

function Accounts.getInstance()
    if not _instance then
        _instance = Accounts;
    end
	-----------------------------------------------------------
	_instance.LoadAccountNames= function()
	sqlCMD = "SELECT username,accountID FROM gothic_multiplayer.account";
	result = DBConnection:getInstance():executeSQL(sqlCMD);
		if result then
			while true do
				local row = mysql_fetch_row(result)
				if not row then return end
				accountIDs[tonumber(row[1])]=row[2];
			end
		 end
	end
	-----------------------------------------------------------
	_instance.LoadAccountsPWs = function()
		sqlCMD = "SELECT accountID,username FROM gothic_multiplayer.account";
		result = DBConnection:getInstance():executeSQL(sqlCMD);
		if result then
			while true do
				local row = mysql_fetch_row(result)
				if not row then return end
				accountPWs[tonumber(row[1])]=row[2];
			end
		 end
	end
	-----------------------------------------------------------
	_instance.getAccountIDbyName = function(accountName)
		return accountIDs[accountName];
	end
	-----------------------------------------------------------
	--Returns 1 if there is a comnination of accountname and pw like this. 0 if  not;
	function VertifyLogin(accountName,password)
		escapedName = DBConnection:getInstance():escapeString(accountName) -- Escape the string to avoid security holes
		hashedPW =  SHA256(password);
		result = mysql_query(sqlhandler,"SELECT * FROM gothic_multiplayer.account WHERE username = '"..escapedName.."' AND password = '"..hashedPW.."';")
		if result then
			if mysql_fetch_row(result) then
				return 1
			else
				return 0
			end
		else
			return 0;
		end
	end
	-----------------------------------------------------------
	
end

