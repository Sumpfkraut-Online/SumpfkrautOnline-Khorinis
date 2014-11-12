-- Copyright (c) 2013, ZEditor
-- Made by Midas

-- settings
local CURRENT_DRAW = nil
local CURRENT_TEXTURE = nil
-- local CURRENT_MANAGER = nil

local ZPlayer = { }

for j = 0, GetMaxPlayers( ) do
	ZPlayer[ j ] = { }
end

function HasEditRights( playerid )
	if IsPlayerConnected( playerid ) then
		for line in io.lines( "zeditor/rights.ze" ) do
			if line == GetPlayerName( playerid ) then
				return true
			end
		end
		return false
	else
		return false
	end
end

function GetConnectedPlayers( )
	local t = { }
	for i = 0, GetMaxPlayers( ) do
		if IsPlayerConnected( i ) then
			table.insert( t, i )
		end
	end
	return t
end

function OnPlayerSpawn( playerid, classid )
	if HasEditRights( playerid ) then
		SendPlayerMessage( playerid, 230, 230, 230, "ZEditor 0.4 has been loaded." )
		SendPlayerMessage( playerid, 230, 230, 230, "Use /zeditor for help." )
	end
	--[[
	if CURRENT_MANAGER == nil then
		SendPlayerMessage( playerid, 255, 100, 0, "You are now able to edit draws and textures." )
		CURRENT_MANAGER = playerid
		FreezePlayer( playerid, 1 )
	end
	]]
end

function OnPlayerDisconnect( playerid )
--[[
	if CURRENT_MANAGER == playerid then
		CURRENT_MANAGER = nil
		for i, p in ipairs( GetConnectedPlayers( ) ) do
			SendPlayerMessage( p, 255, 100, 0, GetPlayerName( playerid ) .. " is not zmanager anymore." )
			SendPlayerMessage( p, 255, 100, 0, "Use /zmanager to become zmanager." )
			SendPlayerMessage( p, 255, 100, 0, "ZManager is able to positioning draws and textures." )
		end
	end
	]]
end


local ZEditor = {
	Draw = { 
		Self = { }, 
		Text = { }, 
		Font = { }, 
		X = { }, 
		Y = { }, 
		Red = { }, 
		Green = { }, 
		Blue = { },
	},
	Texture = {
		tSelf = { }, 
		Textur = { },
		Width = { },
		Height = { },
		tX = { },
		tY = { },
	},
}

local commands = { name = { }, handlerfunction = { } }

function AddCommandHandler( commandname, handlerfunction )
	if not commandname or not handlerfunction then
		return false
	else
		if type( commandname ) ~= "table" then
			commandname = { commandname }
		end
		for j, val in ipairs( commandname ) do
			for i, command in ipairs( commands ) do
				if command.name == "/" .. val then
					return false
				end
			end
			local count = #commands + 1
			commands[ count ] = { name = { }, handlerfunction = { } }
			commands[ count ].name = "/" .. tostring( val )
			commands[ count ].handlerfunction = handlerfunction
		end
		return true
	end
end

function RemoveCommandHandler( commandname )
	if not commandname then
		return false
	else
		for i, command in ipairs( commands ) do
			if commandname == "/" .. commandname then
				table.remove( commands, i )
				return true
			end
		end
	end
end

function OnPlayerCommandText( playerid, cmdtext )
	local cmd, params = GetCommand( cmdtext )
	for k, command in ipairs( commands ) do
		if cmd == command.name then
			command.handlerfunction( playerid, params )
		end
	end
end

function OnFilterscriptInit( )
	Enable_OnPlayerKey( 1 )
end

--[[
function ZSetManager( playerid, params )
	local result, newmanager = sscanf( params, "d" )
	if result == 1 then
		if playerid == CURRENT_MANAGER then
			if IsPlayerConnected( newmanager ) then
				FreezePlayer( playerid, 0 )
				CURRENT_MANAGER = newmanager
				FreezePlayer( newmanager, 1 )
				for i, p in ipairs( GetConnectedPlayers( ) ) do
					SendPlayerMessage( p, 255, 100, 0, GetPlayerName( playerid ) .. " is not zmanager anymore." )
					SendPlayerMessage( p, 255, 100, 0, "New zmanager is " .. GetPlayerName( newmanager ) )
				end
			end
		else
			SendPlayerMessage( playerid, 255, 0, 0, "You can't do this, current zmanager is " .. GetPlayerName( CURRENT_MANAGER ) )
		end
	else
		if CURRENT_MANAGER == nil then
			CURRENT_MANAGER = playerid
			SendPlayerMessage( p, 255, 100, 0, "New zmanager is " .. GetPlayerName( playerid ) )
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/zmanager <playerid>" )
		end
	end
end
AddCommandHandler( "zmanager", ZSetManager )
]]

-- Fonts
-- id: 1 - Font_Old_10_White_Hi

function ZCreateDraw( playerid, params )
	local result, text, font, red, green, blue = sscanf( params, "sdddd" )
	if HasEditRights( playerid ) then
		if result == 1 then
			local i = { }
			for line in io.lines( "zeditor/fonts.ze" ) do
				table.insert( i, line )
				if font == #i then
					local draw = CreateDraw( 400, 3000, text:gsub( "_", " " ), line, red, green, blue )
					ShowDraw( playerid, draw )
					table.insert( ZEditor.Draw, { Self = { draw }, X = { 400 }, Y = { 3000 }, Text = { text:gsub( "_", " " ) }, Font = { line }, Red = { red }, Green = { green }, Blue = { blue } } )
					CURRENT_DRAW = #ZEditor.Draw
					SendPlayerMessage( playerid, 0, 255, 0, "You have created draw ( id: " .. #ZEditor.Draw .. " ) successfully." )
				end
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/createdraw <text> <fontid> <red> <green> <blue>" )
			SendPlayerMessage( playerid, 230, 230, 230, "Fonts: (1) Font_Old_10_White_Hi" )
			SendPlayerMessage( playerid, 230, 230, 230, "(2) Font_Old_10_White (6) Font_Old_20_White_Hi" )
			SendPlayerMessage( playerid, 230, 230, 230, "(3) Font_Old_20_White (7) Font_Default" )
			SendPlayerMessage( playerid, 230, 230, 230, "(4) Font_10_Book      (8) Font_10_Book_Hi" )
			SendPlayerMessage( playerid, 230, 230, 230, "(5) Font_20_Book      (9) Font_20_Book_Hi" )
		end
	end
end
AddCommandHandler( "createdraw", ZCreateDraw )

-- Textures
-- id: 1 - Frame_GMPA

function ZCreateTexture( playerid, params )
	local result, textureid = sscanf( params, "d" )
	if HasEditRights( playerid ) then
		if result == 1 then
			local i = { }
			for line in io.lines( "zeditor/textures.ze" ) do
				table.insert( i, line )
				print(line, textureid);
				if textureid == #i then
					local texture = CreateTexture( 1400, 3000, 3100, 4000, line )
					ShowTexture( playerid, texture )
					table.insert( ZEditor.Texture, { tSelf = { texture }, tX = { 1400 }, tY = { 3000 }, Width = { 3100 }, Height = { 4000 }, Textur = { line } } )
					CURRENT_TEXTURE = #ZEditor.Texture
					SendPlayerMessage( playerid, 0, 255, 0, "You have created texture ( id: " .. #ZEditor.Texture .. " ) successfully." )
				end
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/createtexture <texture>" )
			SendPlayerMessage( playerid, 230, 230, 230, "Textures: (1) Frame_GMPA" )
		end
	end
end
AddCommandHandler( "createtexture", ZCreateTexture )

function ZDestroyDraw( playerid, params )
	local result, drawid = sscanf( params, "d" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Draw[ drawid ] then
				local draw = unpack( ZEditor.Draw[ drawid ].Self )
				if CURRENT_DRAW == drawid then
					if #ZEditor.Draw > 0 then
						CURRENT_DRAW = #ZEditor.Draw
					else
						CURRENT_DRAW = nil
					end
				end
				DestroyDraw( draw )
				table.remove( ZEditor.Draw, drawid )
				SendPlayerMessage( playerid, 0, 255, 0, "You have destroyed draw ( id: " .. drawid .. " ) successfully." )
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/destroydraw <drawid>" )
		end
	end
end
AddCommandHandler( "destroydraw", ZDestroyDraw )

function ZDestroyTexture( playerid, params )
	local result, textureid = sscanf( params, "d" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Texture[ textureid ] then
				local texture = unpack( ZEditor.Texture[ textureid ].tSelf )
				if CURRENT_TEXTURE == textureid then
					CURRENT_TEXTURE = nil
				end
				DestroyTexture( texture )
				table.remove( ZEditor.Texture, textureid )
				SendPlayerMessage( playerid, 0, 255, 0, "You have destroyed texture ( id: " .. textureid .. " ) successfully." )
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Texture ( id: " .. textureid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/destroytexture <textureid>" )
		end
	end
end
AddCommandHandler( "destroytexture", ZDestroyTexture )

function ZShowDraw( playerid, params )
	local result, drawid = sscanf( params, "d" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Draw[ drawid ] then
				for k, p in ipairs( GetConnectedPlayers( ) ) do
					ShowDraw( p, unpack( ZEditor.Draw[ drawid ].Self ) )
				end
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/showdraw <drawid>" )
		end
	end
end
AddCommandHandler( "showdraw", ZShowDraw )

function ZShowTexture( playerid, params )
	local result, textureid = sscanf( params, "d" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Texture[ textureid ] then
				for k, p in ipairs( GetConnectedPlayers( ) ) do
					ShowTexture( p, unpack( ZEditor.Texture[ textureid ].tSelf ) )
				end
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Texture ( id: " .. textureid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/showtexture <textureid>" )
		end
	end
end
AddCommandHandler( "showtexture", ZShowTexture )

function ZHideDraw( playerid, params )
	local result, drawid = sscanf( params, "d" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Draw[ drawid ] then
				HideDraw( playerid, unpack( ZEditor.Draw[ drawid ].Self ) )
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/hidedraw <drawid>" )
		end
	end
end
AddCommandHandler( "hidedraw", ZHideDraw )

function ZHideTexture( playerid, params )
	local result, textureid = sscanf( params, "d" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Texture[ textureid ] then
				HideTexture( playerid, unpack( ZEditor.Texture[ textureid ].tSelf ) )
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Texture ( id: " .. textureid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/hidetexture <textureid>" )
		end
	end
end
AddCommandHandler( "hidetexture", ZHideTexture )

function ZSetCurrentDraw( drawid )
	if drawid then
		if drawid ~= CURRENT_DRAW then
			CURRENT_DRAW = drawid
			return true
		else
			return false
		end
	else
		return false
	end
end

AddCommandHandler( { "setcurrentdraw", "currentdraw" },
	function( playerid, params )
		local result, drawid = sscanf( params, "d" )
		if HasEditRights( playerid ) then
			if result == 1 then
				if ZEditor.Draw[ drawid ] then
					local draw = unpack( ZEditor.Draw[ drawid ].Self )
					if draw ~= -1 then
						if drawid ~= CURRENT_DRAW then
							ZSetCurrentDraw( drawid )
						else
							SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) is already current element!" )
						end
					end
				else
					SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
				end
			else
				SendPlayerMessage( playerid, 230, 230, 230, "/currentdraw <drawid>" )
			end
		end
	end
)

function ZSetCurrentTexture( textureid )
	if textureid then
		if textureid ~= CURRENT_TEXTURE then
			CURRENT_TEXTURE = textureid
			return true
		else
			return false
		end
	else
		return false
	end
end

AddCommandHandler( { "setcurrenttexture", "currenttexture" },
	function( playerid, params )
		local result, textureid = sscanf( params, "d" )
		if HasEditRights( playerid ) then
			if result == 1 then
				if ZEditor.Texture[ textureid ] then
					local texture = unpack( ZEditor.Texture[ textureid ].tSelf )
					if texture ~= -1 then
						if textureid ~= CURRENT_TEXTURE then
							ZSetCurrentTexture( textureid )
						else
							SendPlayerMessage( playerid, 255, 0, 0, "Texture ( id: " .. textureid .. " ) is already current texture!" )
						end
					end
				else
					SendPlayerMessage( playerid, 255, 0, 0, "Texture ( id: " .. textureid .. " ) doesn't exist!" )
				end
			else
				SendPlayerMessage( playerid, 230, 230, 230, "/currenttexture <textureid>" )
			end
		end
	end
)

function ZDisplayDraw( playerid, drawid )
	if playerid and drawid then
		if HasEditRights( playerid ) then
			if IsPlayerConnected( playerid ) then
				if ZEditor.Draw[ drawid ] then
					SendPlayerMessage( playerid, 0, 255, 0, "There are " .. table.getn( ZEditor.Draw ) .. " draws." )
					SendPlayerMessage( playerid, 0, 255, 0, "Draw(" .. drawid .. "-" .. table.getn( ZEditor.Draw ) .. ")" )
					SendPlayerMessage( playerid, 0, 255, 0, "Text: " .. unpack( ZEditor.Draw[ drawid ].Text ) )
					SendPlayerMessage( playerid, 0, 255, 0, "X: " .. unpack( ZEditor.Draw[ drawid ].X ) .. " Y: " .. unpack( ZEditor.Draw[ drawid ].Y ) )
				end
			end
		end
	end
end

AddCommandHandler( { "displaydraw", "getdrawinfo" },
	function( playerid, params )
		local result, drawid = sscanf( params, "d" )
		if HasEditRights( playerid ) then
			if result == 1 then
				if ZEditor.Draw[ drawid ] then
					ZDisplayDraw( playerid, drawid )
				else
					SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
					SendPlayerMessage( playerid, 255, 0, 0, "You can choose from 1 to " .. table.getn( ZEditor.Draw ) .. " draw/s." )
				end
			else
				SendPlayerMessage( playerid, 230, 230, 230, "/displaydraw <drawid>" )
			end
		end
	end
)

function ZDisplayTexture( playerid, textureid )
	if playerid and textureid then
		if IsPlayerConnected( playerid ) then
			if HasEditRights( playerid ) then
				if ZEditor.Texture[ textureid ] then
					SendPlayerMessage( playerid, 0, 255, 0, "There are " .. table.getn( ZEditor.Texture ) .. " textures." )
					SendPlayerMessage( playerid, 0, 255, 0, "Texture(" .. textureid .. "-" .. table.getn( ZEditor.Texture ) .. ")" )
					SendPlayerMessage( playerid, 0, 255, 0, "X: " .. unpack( ZEditor.Texture[ textureid ].tX ) .. " Y: " .. unpack( ZEditor.Texture[ textureid ].tY ) )
					SendPlayerMessage( playerid, 0, 255, 0, "Width: " .. unpack( ZEditor.Texture[ textureid ].Width ) .. "Height: " .. unpack( ZEditor.Texture[ textureid ].Height ) )
				end
			end
		end
	end
end

AddCommandHandler( { "displaytexture", "gettextureinfo" },
	function( playerid, params )
		local result, textureid = sscanf( params, "d" )
		if HasEditRights( playerid ) then
			if result == 1 then
				if ZEditor.Texture[ textureid ] then
					ZDisplayTexture( playerid, textureid )
				else
					SendPlayerMessage( playerid, 255, 0, 0, "Texture ( id: " .. textureid .. " ) doesn't exist!" )
					SendPlayerMessage( playerid, 255, 0, 0, "You can choose from 1 to " .. table.getn( ZEditor.Texture ) .. " texture/s." )
				end
			else
				SendPlayerMessage( playerid, 230, 230, 230, "/displaytexture <textureid>" )
			end
		end
	end
)

AddCommandHandler( { "zedit", "zeditor" },
	function( playerid, params )
		if HasEditRights( playerid ) then
			SendPlayerMessage( playerid, 0, 255, 255, "ZEditor 0.2 made by Midas" )
			SendPlayerMessage( playerid, 0, 255, 255, "Use /drawedit and /textureedit to see help for them." )
			SendPlayerMessage( playerid, 0, 255, 255, "To generate LUA code just use /saveproj" )
		end
	end
)

AddCommandHandler( { "drawedit", "helpdraw" },
	function( playerid, params )
		if HasEditRights( playerid ) then
			SendPlayerMessage( playerid, 0, 255, 255, "Draw edit help" )
			SendPlayerMessage( playerid, 0, 255, 255, " /createdraw, /destroydraw" )
			SendPlayerMessage( playerid, 0, 255, 255, " /showdraw, /hidedraw, /drawcolor, /drawname" )
			SendPlayerMessage( playerid, 0, 255, 255, " /currentdraw, /drawfont" )
			SendPlayerMessage( playerid, 0, 255, 255, " Arrows - steering current draw" )
		end 
	end
)

AddCommandHandler( { "textureedit", "helptexture" },
	function( playerid, params )
		if HasEditRights( playerid ) then
			SendPlayerMessage( playerid, 0, 255, 255, "Texture edit help" )
			SendPlayerMessage( playerid, 0, 255, 255, " /createtexture, /destroytexture" )
			SendPlayerMessage( playerid, 0, 255, 255, " /showtexture, /hidetexture" )
			SendPlayerMessage( playerid, 0, 255, 255, " /currenttexture, /changetexture" )
			SendPlayerMessage( playerid, 0, 255, 255, " i,j,l,m - steering current texture" )
			SendPlayerMessage( playerid, 0, 255, 255, " z,x,y,h, numpad - resizing current texture" )
		end
	end
)

-- this one will be used later

function ZUpdateDraw( ... )
	if table.getn( arg ) == 8 then
		UpdateDraw( unpack( arg ) )
		return true
	else
		return false
	end
end

function UpdateTexture( ztexid, x, y, width, height, texture )
	if ZEditor.Texture[ ztexid ] then
		local TEX = unpack( ZEditor.Texture[ ztexid ].tSelf )
		if TEX ~= -1 then
			local new = CreateTexture( x, y, width, height, texture )
			for key, p in ipairs( GetConnectedPlayers( ) ) do
				ShowTexture( p, new )
			end
			DestroyTexture( TEX )
			ZEditor.Texture[ ztexid ].tSelf = { new }
			return true
		end
	else
		return false
	end
end

function ZChangeTexture( playerid, params )
	local result, textureid, newtexture = sscanf( params, "dd" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Texture[ textureid ] then
				local i = { }
				for line in io.lines( "zeditor/textures.ze" ) do
					table.insert( i, line )
					if newtexture == #i then
						ZEditor.Texture[ textureid ].Textur = { line }
						UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
					end
				end
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Texture ( id: " .. textureid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/changetexture <texture> <textureid>" )
			SendPlayerMessage( playerid, 230, 230, 230, "Textures: (1) Frame_GMPA" )
		end
	end
end
AddCommandHandler( "changetexture", ZChangeTexture )

function ZSetDrawText( playerid, params )
	local result, drawid, newname = sscanf( params, "ds" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Draw[ drawid ] then
				local draw = unpack( ZEditor.Draw[ drawid ].Self )
				ZEditor.Draw[ drawid ].Text = { newname }
				ZUpdateDraw( draw, unpack( ZEditor.Draw[ drawid ].X ), unpack( ZEditor.Draw[ drawid ].Y ), newname, unpack( ZEditor.Draw[ drawid ].Font ), unpack( ZEditor.Draw[ drawid ].Red ), unpack( ZEditor.Draw[ drawid ].Green ), unpack( ZEditor.Draw[ drawid ].Blue ) )
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/setdrawtext <drawid> <newname>" )
		end
	end
end
AddCommandHandler( {"setdrawname", "drawname"}, ZSetDrawText )

function ZSetDrawFont( playerid, params )
	local result, drawid, fontid = sscanf( params, "dd" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Draw[ drawid ] then
				local draw = unpack( ZEditor.Draw[ drawid ].Self )
				local i = { }
				for line in io.lines( "zeditor/fonts.ze" ) do
					table.insert( i, line )
					if fontid == #i then
						ZEditor.Draw[ drawid ].Font = { line }
						ZUpdateDraw( draw, unpack( ZEditor.Draw[ drawid ].X ), unpack( ZEditor.Draw[ drawid ].Y ), unpack( ZEditor.Draw[ drawid ].Text ), unpack( ZEditor.Draw[ drawid ].Font ), unpack( ZEditor.Draw[ drawid ].Red ), unpack( ZEditor.Draw[ drawid ].Green ), unpack( ZEditor.Draw[ drawid ].Blue ) )
					end
				end
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/drawfont <drawid> <fontid>" )
			SendPlayerMessage( playerid, 230, 230, 230, "Fonts: (1) Font_Old_10_White_Hi" )
			SendPlayerMessage( playerid, 230, 230, 230, "(2) Font_Old_10_White (6) Font_Old_20_White_Hi" )
			SendPlayerMessage( playerid, 230, 230, 230, "(3) Font_Old_20_White (7) Font_Default" )
			SendPlayerMessage( playerid, 230, 230, 230, "(4) Font_10_Book      (8) Font_10_Book_Hi" )
			SendPlayerMessage( playerid, 230, 230, 230, "(5) Font_20_Book      (9) Font_20_Book_Hi" )
		end
	end
end
AddCommandHandler( { "drawfont", "setdrawfont" }, ZSetDrawFont )

function ZSetDrawColor( playerid, params )
	local result, drawid, r, g, b = sscanf( params, "dddd" )
	if HasEditRights( playerid ) then
		if result == 1 then
			if ZEditor.Draw[ drawid ] then
				local draw = unpack( ZEditor.Draw[ drawid ].Self )
				ZEditor.Draw[ drawid ].Red = { r }
				ZEditor.Draw[ drawid ].Green = { g }
				ZEditor.Draw[ drawid ].Blue = { b }
				ZUpdateDraw( draw, unpack( ZEditor.Draw[ drawid ].X ), unpack( ZEditor.Draw[ drawid ].Y ), unpack( ZEditor.Draw[ drawid ].Text ), unpack( ZEditor.Draw[ drawid ].Font ), r, g, b )
			else
				SendPlayerMessage( playerid, 255, 0, 0, "Draw ( id: " .. drawid .. " ) doesn't exist!" )
			end
		else
			SendPlayerMessage( playerid, 230, 230, 230, "/setdrawcolor <drawid> <r> <g> <b>" )
		end
	end
end
AddCommandHandler( {"setdrawcolor", "drawcolor"}, ZSetDrawColor )

function ZSave( playerid, params )
	if HasEditRights( playerid ) then
		local file = io.open( "zeditor/projects/proj" .. tostring( os.date("%Y-%m-%d-%H-%M-%S") ) .. ".lua", "w+" )
		file:write( "--Generated with ZEditor by Midas\n" )
		if #ZEditor.Texture > 0 then
			for i, v in ipairs( ZEditor.Texture ) do
				file:write("CreateTexture(" .. unpack( ZEditor.Texture[ i ].tX ) .. ", " .. unpack( ZEditor.Texture[ i ].tY ) .. ", " .. unpack( ZEditor.Texture[ i ].Width ) .. ", " .. unpack( ZEditor.Texture[ i ].Height ) .. ', "' .. unpack( ZEditor.Texture[ i ].Textur ) .. '")\n' )
			end
		end
		if #ZEditor.Draw > 0 then
			for i, v in ipairs( ZEditor.Draw ) do
				file:write("CreateDraw(" .. unpack( ZEditor.Draw[ i ].X ) .. ", " .. unpack( ZEditor.Draw[ i ].Y ) .. ', "' .. unpack( ZEditor.Draw[ i ].Text ) .. '", "' .. unpack( ZEditor.Draw[ i ].Font ) .. '", ' .. unpack( ZEditor.Draw[ i ].Red ) .. ", " .. unpack( ZEditor.Draw[ i ].Green ) .. ", " .. unpack( ZEditor.Draw[ i ].Blue ) .. ")\n" )
			end
		end
		file:flush()
		file:close()
		SendPlayerMessage( playerid, 0, 255, 0, "Saved ( proj" .. tostring( os.date("%d-%m-%Y-%H-%M-%S") ) .. ".lua )"  )
	end
end
AddCommandHandler( {"saveproject", "saveproj"}, ZSave )

function GO_LEFT( drawid )
	if drawid and drawid ~= nil then
		if ZEditor.Draw[ drawid ] then
			local draw = unpack( ZEditor.Draw[ drawid ].Self )
			if draw ~= -1 then
				local x = unpack( ZEditor.Draw[ drawid ].X )
				local y = unpack( ZEditor.Draw[ drawid ].Y )
				ZEditor.Draw[ drawid ].X = { x - 50 }
				ZUpdateDraw( draw, unpack( ZEditor.Draw[ drawid ].X ), unpack( ZEditor.Draw[ drawid ].Y ), unpack( ZEditor.Draw[ drawid ].Text ), unpack( ZEditor.Draw[ drawid ].Font ), unpack( ZEditor.Draw[ drawid ].Red ), unpack( ZEditor.Draw[ drawid ].Green ), unpack( ZEditor.Draw[ drawid ].Blue ) )
			end
		end
	end
end

function GO_RIGHT( drawid )
	if drawid and drawid ~= nil then
		if ZEditor.Draw[ drawid ] then
			local draw = unpack( ZEditor.Draw[ drawid ].Self )
			if draw ~= -1 then
				local x = unpack( ZEditor.Draw[ drawid ].X )
				local y = unpack( ZEditor.Draw[ drawid ].Y )
				ZEditor.Draw[ drawid ].X = { x + 50 }
				ZUpdateDraw( draw, unpack( ZEditor.Draw[ drawid ].X ), unpack( ZEditor.Draw[ drawid ].Y ), unpack( ZEditor.Draw[ drawid ].Text ), unpack( ZEditor.Draw[ drawid ].Font ), unpack( ZEditor.Draw[ drawid ].Red ), unpack( ZEditor.Draw[ drawid ].Green ), unpack( ZEditor.Draw[ drawid ].Blue ) )
			end
		end
	end
end

function GO_UP( drawid )
	if drawid and drawid ~= nil then
		if ZEditor.Draw[ drawid ] then
			local draw = unpack( ZEditor.Draw[ drawid ].Self )
			if draw ~= -1 then
				local x = unpack( ZEditor.Draw[ drawid ].X )
				local y = unpack( ZEditor.Draw[ drawid ].Y )
				ZEditor.Draw[ drawid ].Y = { y - 50 }
				ZUpdateDraw( draw, unpack( ZEditor.Draw[ drawid ].X ), unpack( ZEditor.Draw[ drawid ].Y ), unpack( ZEditor.Draw[ drawid ].Text ), unpack( ZEditor.Draw[ drawid ].Font ), unpack( ZEditor.Draw[ drawid ].Red ), unpack( ZEditor.Draw[ drawid ].Green ), unpack( ZEditor.Draw[ drawid ].Blue ) )
			end
		end
	end
end

function GO_DOWN( drawid )
	if drawid and drawid ~= nil then
		if ZEditor.Draw[ drawid ] then
			local draw = unpack( ZEditor.Draw[ drawid ].Self )
			if draw ~= -1 then
				local x = unpack( ZEditor.Draw[ drawid ].X )
				local y = unpack( ZEditor.Draw[ drawid ].Y )
				ZEditor.Draw[ drawid ].Y = { y + 50 }
				ZUpdateDraw( draw, unpack( ZEditor.Draw[ drawid ].X ), unpack( ZEditor.Draw[ drawid ].Y ), unpack( ZEditor.Draw[ drawid ].Text ), unpack( ZEditor.Draw[ drawid ].Font ), unpack( ZEditor.Draw[ drawid ].Red ), unpack( ZEditor.Draw[ drawid ].Green ), unpack( ZEditor.Draw[ drawid ].Blue ) )
			end
		end
	end
end

function TEXTURE_LEFT( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local x = unpack( ZEditor.Texture[ textureid ].tX )
			local y = unpack( ZEditor.Texture[ textureid ].tY )
			local width = unpack( ZEditor.Texture[ textureid ].Width )
			ZEditor.Texture[ textureid ].Width = { width - 50 }
			ZEditor.Texture[ textureid ].tX = { x - 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function TEXTURE_RIGHT( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local x = unpack( ZEditor.Texture[ textureid ].tX )
			local y = unpack( ZEditor.Texture[ textureid ].tY )
			local width = unpack( ZEditor.Texture[ textureid ].Width )
			ZEditor.Texture[ textureid ].Width = { width + 50 }
			ZEditor.Texture[ textureid ].tX = { x + 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function TEXTURE_UP( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local x = unpack( ZEditor.Texture[ textureid ].tX )
			local y = unpack( ZEditor.Texture[ textureid ].tY )
			local height = unpack( ZEditor.Texture[ textureid ].Height )
			ZEditor.Texture[ textureid ].Height = { height - 50 }
			ZEditor.Texture[ textureid ].tY = { y - 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function TEXTURE_DOWN( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local x = unpack( ZEditor.Texture[ textureid ].tX )
			local y = unpack( ZEditor.Texture[ textureid ].tY )
			local height = unpack( ZEditor.Texture[ textureid ].Height )
			ZEditor.Texture[ textureid ].Height = { height + 50 }
			ZEditor.Texture[ textureid ].tY = { y + 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function RESIZE_WIDTH( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local width = unpack( ZEditor.Texture[ textureid ].Width )
			ZEditor.Texture[ textureid ].Width = { width + 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function RESIZE_WIDTH_2( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local width = unpack( ZEditor.Texture[ textureid ].Width )
			ZEditor.Texture[ textureid ].Width = { width - 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function RESIZE_HEIGHT( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local height = unpack( ZEditor.Texture[ textureid ].Height )
			ZEditor.Texture[ textureid ].Height = { height + 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function RESIZE_HEIGHT_2( textureid )
	if textureid and textureid ~= nil then
		if ZEditor.Texture[ textureid ] then
			local height = unpack( ZEditor.Texture[ textureid ].Height )
			ZEditor.Texture[ textureid ].Height = { height - 50 }
			UpdateTexture( textureid, unpack( ZEditor.Texture[ textureid ].tX ), unpack( ZEditor.Texture[ textureid ].tY ), unpack( ZEditor.Texture[ textureid ].Width ), unpack( ZEditor.Texture[ textureid ].Height ), unpack( ZEditor.Texture[ textureid ].Textur ) )
		end
	end
end

function OnPlayerKey( playerid, keydown, keyup )
	if HasEditRights( playerid ) then
		if keydown == KEY_LEFT then
			if CURRENT_DRAW ~= nil then
				ZPlayer[ playerid ].timer1 = SetTimerEx( "GO_LEFT", 50, 1, CURRENT_DRAW )
			end
		elseif keyup == KEY_LEFT then
			if CURRENT_DRAW ~= nil then
				KillTimer( ZPlayer[ playerid ].timer1 )
			end
		elseif keydown == KEY_RIGHT then
			if CURRENT_DRAW ~= nil then
				ZPlayer[ playerid ].timer2 = SetTimerEx( "GO_RIGHT", 50, 1, CURRENT_DRAW )
			end
		elseif keyup == KEY_RIGHT then
			if CURRENT_DRAW ~= nil then
				KillTimer( ZPlayer[ playerid ].timer2 )
			end
		elseif keydown == KEY_UP then
			if CURRENT_DRAW ~= nil then
				ZPlayer[ playerid ].timer3 = SetTimerEx( "GO_UP", 50, 1, CURRENT_DRAW )
			end
		elseif keyup == KEY_UP then
			if CURRENT_DRAW ~= nil then
				KillTimer( ZPlayer[ playerid ].timer3 )
			end
		elseif keydown == KEY_DOWN then
			if CURRENT_DRAW ~= nil then
				ZPlayer[ playerid ].timer4 = SetTimerEx( "GO_DOWN", 50, 1, CURRENT_DRAW )
			end
		elseif keyup == KEY_DOWN then
			if CURRENT_DRAW ~= nil then
				KillTimer( ZPlayer[ playerid ].timer4 )
			end
		elseif keydown == KEY_X or keydown == KEY_NUMPAD6 then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer5 = SetTimerEx( "RESIZE_WIDTH", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_X or keyup == KEY_NUMPAD6 then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer5 )
			end
		elseif keydown == KEY_Z or keydown == KEY_NUMPAD4 then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer6 = SetTimerEx( "RESIZE_WIDTH_2", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_Z or keyup == KEY_NUMPAD4 then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer6 )
			end
		elseif keydown == KEY_Y or keydown == KEY_NUMPAD8 then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer11 = SetTimerEx( "RESIZE_HEIGHT_2", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_Y or keyup == KEY_NUMPAD8 then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer11 )
			end
		elseif keydown == KEY_H or keydown == KEY_NUMPAD2 then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer12 = SetTimerEx( "RESIZE_HEIGHT", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_H or keyup == KEY_NUMPAD2 then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer12 )
			end
		elseif keydown == KEY_J then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer7 = SetTimerEx( "TEXTURE_LEFT", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_J then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer7 )
			end
		elseif keydown == KEY_L then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer8 = SetTimerEx( "TEXTURE_RIGHT", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_L then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer8 )
			end
		elseif keydown == KEY_I then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer9 = SetTimerEx( "TEXTURE_UP", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_I then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer9 )
			end
		elseif keydown == KEY_M then
			if CURRENT_TEXTURE ~= nil then
				ZPlayer[ playerid ].timer10 = SetTimerEx( "TEXTURE_DOWN", 50, 1, CURRENT_TEXTURE )
			end
		elseif keyup == KEY_M then
			if CURRENT_TEXTURE ~= nil then
				KillTimer( ZPlayer[ playerid ].timer10 )
			end
		end
	end
end

print("Zeditor loaded");