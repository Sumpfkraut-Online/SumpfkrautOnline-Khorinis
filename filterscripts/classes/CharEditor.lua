CharEditor = {} -- the table representing the class, which will double as the metatable for the instances
CharEditor.__index = CharEditor -- failed table lookups on the instances should fallback to the class table, to get methods
CharEditorList={}


--------------------------------------------------------------------------------------
-------------------------Hard--coded--Values------------------------------------------
--------------------------------------------------------------------------------------
--Body Meshes
HumBodys ={}
HumBodys[0]="Hum_Body_Naked0"
HumBodys[1]="Hum_Body_Babe0"
--Head Meshes
HumHeads ={}
HumHeads[0]="Hum_Head_Pony";
HumHeads[1]="Hum_Head_Psionic";
HumHeads[2]="Hum_Head_Bald";
HumHeads[3]="Hum_Head_Fatbald";
HumHeads[4]="Hum_Head_Fighter";
HumHeads[5]="Hum_Head_Pony";
HumHeads[6]="Hum_Head_Psionic";
HumHeads[7]="Hum_Head_Thief";
--HumHeads[8]="Hum_Head_Babe";
--HumHeads[9]="Hum_Head_Babe1";
--HumHeads[10]="Hum_Head_Babe2";
--HumHeads[11]="Hum_Head_Babe3";
--HumHeads[12]="Hum_Head_Babe4";
--HumHeads[13]="Hum_Head_Babe5";
--HumHeads[14]="Hum_Head_Babe6";
--HumHeads[15]="Hum_Head_Babe7";
--HumHeads[16]="Hum_Head_Babe8";

--The Player Waypoints
CharEditRooms_Player ={}
CharEditRooms_Player[0]="charedit1_player";
CharEditRooms_Player[1]="charedit2_player";
CharEditRooms_Player[2]="charedit3_player";
CharEditRooms_Player[3]="charedit4_player";
CharEditRooms_Player[4]="charedit5_player";


--The Helper Waypoint
CharEditRooms_Helper = {}
CharEditRooms_Helper[0]="charedit1_helper";
CharEditRooms_Helper[1]="charedit2_helper";
CharEditRooms_Helper[2]="charedit3_helper";
CharEditRooms_Helper[3]="charedit4_helper";
CharEditRooms_Helper[4]="charedit5_helper";


CharEditUsedRooms={};
CharEditUsedRooms[0]=-1;
CharEditUsedRooms[1]=-1;
CharEditUsedRooms[2]=-1;
CharEditUsedRooms[3]=-1;
CharEditUsedRooms[4]=-1;

--List of Body Textures which should not be usable.
BannedBodyTextures={}
BannedBodyTextures[4]=1;
BannedBodyTextures[5]=1;
BannedBodyTextures[6]=1;
BannedBodyTextures[7]=1;
BannedBodyTextures[11]=1;
BannedBodyTextures[12]=1;

--List of Head Textures which should not be usable.
BannedHeadTextures={};
BannedHeadTextures[137]=1;
BannedHeadTextures[138]=1;
BannedHeadTextures[139]=1;
BannedHeadTextures[140]=1;
BannedHeadTextures[141]=1;
BannedHeadTextures[142]=1;
BannedHeadTextures[143]=1;
BannedHeadTextures[144]=1;
BannedHeadTextures[145]=1;
BannedHeadTextures[146]=1;
BannedHeadTextures[147]=1;
BannedHeadTextures[148]=1;
BannedHeadTextures[149]=1;
BannedHeadTextures[150]=1;
BannedHeadTextures[151]=1;
BannedHeadTextures[152]=1;
BannedHeadTextures[153]=1;
BannedHeadTextures[154]=1;
BannedHeadTextures[155]=1;
BannedHeadTextures[156]=1;
BannedHeadTextures[157]=1;
BannedHeadTextures[158]=1;
BannedHeadTextures[159]=1;
BannedHeadTextures[160]=1;

--------------------------------------------------------------------------------------
------------------------Hard--coded--Values--End--------------------------------------
--------------------------------------------------------------------------------------

--Amount of available Classes
ClassAmount=0;
--Character Classes Names
-- Dont use "%" this causes gmp to crash
ClassDescriptions={};
--Character Classes health
ClassHealth={}
--Character Classes max_health
ClassMax_Health={}
--Character Classes Strength
ClassStrength={};
--Character Classes Dexterity
ClassDexterity={};
--Character Classes 1 Handed Skill
Class1Handed={};
--Character Classes 2 Handed Skill
Class2Handed={};
--Character Classes Bow Skill
ClassBow={};
--Character Classes Crossbow Skill
ClassCrossbow={};





result=runSQL("SELECT * FROM `gothic_multiplayer`.`ce_class`;")
ClassAmount=mysql_num_rows (result);
while true do
	row = mysql_fetch_row(result);
	if (not row) then break end
	 table.insert(ClassDescriptions, row[2])
	 table.insert(ClassHealth, tonumber(row[3]))
	 table.insert(ClassMax_Health, tonumber(row[4]))
	 table.insert(ClassStrength, tonumber(row[5]))
	 table.insert(ClassDexterity, tonumber(row[6]))
	 table.insert(Class1Handed, tonumber(row[7]))
	 table.insert(Class2Handed, tonumber(row[8]))
	 table.insert(ClassBow, tonumber(row[9]))
	 table.insert(ClassCrossbow, tonumber(row[10]))
end
print("<CharEditor> "..ClassAmount.." start Character Classes loaded");

-- syntax equivalent to "MyClass.new = function..."
--@description: Chreates a new instance  of the Character Editor
--@param: playerid
--@return: the new instance of the CharEditor
function CharEditor:new(playerid)
	
	if CharEditorList[playerid] then
		CharEditorList[playerid]:Close();
	end
	local tempSelf = {};
	setmetatable(tempSelf,CharEditor);
	
	if CharEditUsedRooms[0]==-1 then
		CharEditUsedRooms[0]=playerid;
		self.spawnroomnumber=0;
		print("new CharEditor for playerid: "..playerid.." Room 0")
	elseif CharEditUsedRooms[1]==-1 then
		CharEditUsedRooms[1]=playerid;
		self.spawnroomnumber=1;
		print("new CharEditor for playerid: "..playerid.." Room 1")
	elseif CharEditUsedRooms[2]==-1 then
		CharEditUsedRooms[2]=playerid;
		self.spawnroomnumber=2;
		print("new CharEditor for playerid: "..playerid.." Room 2")
	elseif CharEditUsedRooms[3]==-1 then
		CharEditUsedRooms[3]=playerid;
		self.spawnroomnumber=3;
		print("new CharEditor for playerid: "..playerid.." Room 3")
	elseif CharEditUsedRooms[4]==-1 then
		CharEditUsedRooms[4]=playerid;
		self.spawnroomnumber=4;
		print("new CharEditor for playerid: "..playerid.." Room 4")
	else
		SendPlayerMessage(playerid,0,0,255,"Alle Plaetze sind derzeit Belegt. Bitte Warten...");
		SendPlayerMessage(playerid,0,0,255,"Not Implemented Yet.");
		return;
	end
	
	


	
	
	BodyModel,BodyTextureId,HeadModel,HeadTextureID=GetPlayerAdditionalVisual(playerid);
	tempSelf.playID=playerid;
	tempSelf.BodyModelId=0
	tempSelf.Fatnes =GetPlayerFatness(tempSelf.playID) ;
	tempSelf.Class =1;

	--Getting the index of the old Body Mesh in the HumBodys-Array
	for i, model in pairs (HumBodys) do
		if model == BodyModel then
			tempSelf.BodyModelId=i
			break
		end
	end 
	tempSelf.HeadModelId = 0;
		--Getting the index of the old Head Mesh in the HumHeads-Array
	for i, model in pairs (HumHeads) do
		if model == HeadModel then
			tempSelf.HeadModelId=i
			break
		end
	end 

	--Saving the found values.
	tempSelf.BodyTextureId=BodyTextureId;	
	tempSelf.HeadTextureId =HeadTextureID;
	tempSelf.MenueState=0;
	

	
	
	--Create Helper NPC
	tempSelf.MyNpc = CreateNPC("CH")
	--Spawn Helper Npc
	SpawnPlayer(tempSelf.MyNpc);
	--Telerport Helper NPC into the Chareditroom
	TeleportPlayerToWayPoint(tempSelf.MyNpc,CharEditRooms_Helper[self.spawnroomnumber])
	--Set Helper NPC Visuals and Health
    SetPlayerAdditionalVisual(tempSelf.MyNpc,HumBodys[tempSelf.BodyModelId],tempSelf.BodyTextureId,HumHeads[tempSelf.HeadModelId],tempSelf.HeadTextureId);
	SetPlayerMaxHealth(tempSelf.MyNpc, 99999999)
	SetPlayerHealth(tempSelf.MyNpc, 99999999)
    PlayAnimation(tempSelf.MyNpc,"S_LGUARD");
	SetPlayerFatness(tempSelf.MyNpc,tempSelf.Fatnes)
	tempSelf.HelperAngle=GetPlayerAngle(tempSelf.MyNpc);
	--Teleport Player into the Chareditroom and freeze him/her
	FreezePlayer(playerid,1)
	TeleportPlayerToWayPoint(playerid,CharEditRooms_Player[self.spawnroomnumber]);
	FreezePlayer(playerid,1)
	
	--Create PlayerDraws
	tempSelf.Message  = 	 CreatePlayerDraw(playerid,400,4000,"Erstelle deinen Charakter, bestaedige mit der Leertaste / Bitte druecke F!","Font_Old_10_White_Hi.TGA",255,255,0);
	tempSelf.DrawHeadTex =   CreatePlayerDraw(playerid,400,4150,"Gesichtszuege","Font_Old_10_White_Hi.TGA",255,255,255);
	tempSelf.DrawHeadMesh =  CreatePlayerDraw(playerid,400,4300,"Kopfform","Font_Old_10_White_Hi.TGA",255,255,0);
	tempSelf.DrawBodyTex = 	 CreatePlayerDraw(playerid,400,4450,"Hautfarbe","Font_Old_10_White_Hi.TGA",255,255,0);
	tempSelf.DrawBodyMesh =  CreatePlayerDraw(playerid,400,4600,"Koerpermasse","Font_Old_10_White_Hi.TGA",255,255,0);
	tempSelf.DrawClass =  CreatePlayerDraw(playerid,400,4750,"Startklasse: "..ClassDescriptions[tempSelf.Class],"Font_Old_10_White_Hi.TGA",255,255,0);
	tempSelf.DrawRotadeHelper =  CreatePlayerDraw(playerid,400,4900,"Du kannst deinen Charakterhelper mit Q und E drehen","Font_Old_10_White_Hi.TGA",255,255,0);
	--Show Draws
	ShowPlayerDraw(playerid, tempSelf.Message)
	ShowPlayerDraw(playerid, tempSelf.DrawHeadTex)
	ShowPlayerDraw(playerid, tempSelf.DrawHeadMesh)
	ShowPlayerDraw(playerid, tempSelf.DrawBodyTex)
	ShowPlayerDraw(playerid, tempSelf.DrawBodyMesh)
	ShowPlayerDraw(playerid, tempSelf.DrawClass)
	ShowPlayerDraw(playerid, tempSelf.DrawRotadeHelper)
	--Add instance to List
	CharEditorList[playerid]=tempSelf;
end


--@description: Returns the self value.
--@param: self
--@return: self.value
function CharEditor:get_value(self)
  return self.value
end




--@description:	Update the Selction Draws
--@param the selected Menue Point
--@return void
function CharEditor:UpdateDrawSelection(selection)
	if self then
			local playerid = self.playID;
			if(self.MenueState == 0)then
				UpdatePlayerDraw(playerid, self.DrawHeadTex,400,4150,"Gesichtszuege","Font_Old_10_White_Hi.TGA",255,255,255);
				UpdatePlayerDraw(playerid, self.DrawHeadMesh,400,4300,"Kopfform","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyTex,400,4450,"Hautfarbe","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyMesh,400,4600,"Koerpermasse","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawClass,400,4750,"Startklasse: "..ClassDescriptions[self.Class],"Font_Old_10_White_Hi.TGA",255,255,0);
			elseif(self.MenueState == 1)then
				UpdatePlayerDraw(playerid, self.DrawHeadTex,400,4150,"Gesichtszuege","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawHeadMesh,400,4300,"Kopfform","Font_Old_10_White_Hi.TGA",255,255,255);
				UpdatePlayerDraw(playerid, self.DrawBodyTex,400,4450,"Hautfarbe","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyMesh,400,4600,"Koerpermasse","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawClass,400,4750,"Startklasse: "..ClassDescriptions[self.Class],"Font_Old_10_White_Hi.TGA",255,255,0);
			elseif(self.MenueState == 2)then
				UpdatePlayerDraw(playerid, self.DrawHeadTex,400,4150,"Gesichtszuege","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawHeadMesh,400,4300,"Kopfform","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyTex,400,4450,"Hautfarbe","Font_Old_10_White_Hi.TGA",255,255,255);
				UpdatePlayerDraw(playerid, self.DrawBodyMesh,400,4600,"Koerpermasse","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawClass,400,4750,"Startklasse: "..ClassDescriptions[self.Class],"Font_Old_10_White_Hi.TGA",255,255,0);
			elseif(self.MenueState == 3)then
				UpdatePlayerDraw(playerid, self.DrawHeadTex,400,4150,"Gesichtszuege","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawHeadMesh,400,4300,"Kopfform","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyTex,400,4450,"Hautfarbe","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyMesh,400,4600,"Koerpermasse","Font_Old_10_White_Hi.TGA",255,255,255);
				UpdatePlayerDraw(playerid, self.DrawClass,400,4750,"Startklasse: "..ClassDescriptions[self.Class],"Font_Old_10_White_Hi.TGA",255,255,0);
			elseif(self.MenueState == 4)then
				UpdatePlayerDraw(playerid, self.DrawHeadTex,400,4150,"Gesichtszuege","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawHeadMesh,400,4300,"Kopfform","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyTex,400,4450,"Hautfarbe","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawBodyMesh,400,4600,"Koerpermasse","Font_Old_10_White_Hi.TGA",255,255,0);
				UpdatePlayerDraw(playerid, self.DrawClass,400,4750,"Startklasse: "..ClassDescriptions[self.Class],"Font_Old_10_White_Hi.TGA",255,255,255);
			end
	else
			print("CharEditor:UpdateDrawSelection() is a member function and cannot be called without a instance-object");
	end
end


--@description:	switches to the next BodyTexture
--@param void
--@return void
function CharEditor:nextBodyTexture()

	if self then
		if self.BodyTextureId== 12 then
			self.BodyTextureId= 0;
		else
			self.BodyTextureId=self.BodyTextureId+1;
		end
		if BannedBodyTextures[self.BodyTextureId]==1 then
			self:nextBodyTexture();
		else
			print(self.BodyTextureId);
			self:setVisual();
		end
	else
		print("CharEditor:nextBodyTexture() is a member function and cannot be called without a instance-object");
	end
end


--@description:	switches to the last BodyTexture
--@param void
--@return void
function CharEditor:lastBodyTexture()
	if self then
		if self.BodyTextureId== 0 then
			self.BodyTextureId= 12;
		else
			self.BodyTextureId=self.BodyTextureId-1;
		end
		if BannedBodyTextures[self.BodyTextureId]==1 then
			self:lastBodyTexture();
		else
			self:setVisual();
		end
	else
		print("CharEditor:lastBodyTexture() is a member function and cannot be called without a instance-object");
	end
end

--@description:	switches to the next HeadTexture
--@param void
--@return void
function CharEditor:nextHeadTexture()
	if self then
		if self.HeadTextureId== 162 then
			self.HeadTextureId= 0;
		else
			self.HeadTextureId=self.HeadTextureId+1;
		end
		if BannedHeadTextures[self.HeadTextureId] == 1 then
			self:nextHeadTexture();
		else
			self:setVisual();
		end
	else
		print("CharEditor:nextHeadTexture() is a member function and cannot be called without a instance-object");
	end
end


--@description:	switches to the last HeadTexture
--@param void
--@return void
function CharEditor:lastHeadTexture()
	if self then
		if self.HeadTextureId== 0 then
			self.HeadTextureId= 162;
		else
			self.HeadTextureId=self.HeadTextureId-1;
		end
		if BannedHeadTextures[self.HeadTextureId] == 1 then
			self:lastHeadTexture();	
		else
			self:setVisual();
		end
	else
		print("CharEditor:lastHeadTexture() is a member function and cannot be called without a instance-object");
	end
end


--@description:	switches to moreFatnes
--@param void
--@return void
function CharEditor:moreFatnes()
	if self then
		if	self.Fatnes == 2 then
			self.Fatnes=-1;
		else
			self.Fatnes=self.Fatnes+1
		end
			SetPlayerFatness(self.MyNpc, self.Fatnes)
			SendPlayerMessage(self.playID,255,0,0,"Deine aktuelle Koerpermasse ist: "..self.Fatnes)
	else
		print("CharEditor:moreFatnes() is a member function and cannot be called without a instance-object");
	end
end

--@description:	switches to lessFattnes
--@param void
--@return void
function CharEditor:lessFatnes()
	if self then
		if	self.Fatnes == -1 then
			self.Fatnes=2;
		else
			self.Fatnes=self.Fatnes-1
		end
		SetPlayerFatness(self.MyNpc, self.Fatnes)
		SendPlayerMessage(self.playID,255,0,0,"Deine aktuelle Koerpermasse ist: "..self.Fatnes)
	else
		print("CharEditor:lessFatnes() is a member function and cannot be called without a instance-object");
	end
end

--@description:	switches to the next HeadModel
--@param void
--@return void
function CharEditor:nextHeadModel()
	if self then
		if self.HeadModelId== table.maxn(HumHeads) then
			self.HeadModelId= 0;
		else
			self.HeadModelId=self.HeadModelId+1;
		end
		self:setVisual();
	else
		print("CharEditor:nextHeadModel() is a member function and cannot be called without a instance-object");
	end
end

--@description:	switches to the last HeadModel
--@param void
--@return void
function CharEditor:lastHeadModel()
	if self then
		if self.HeadModelId==0 then
			self.HeadModelId=  table.maxn(HumHeads);
		else
			self.HeadModelId=self.HeadModelId-1;
		end
		self:setVisual();
	else
		print("CharEditor:nextHeadModel() is a member function and cannot be called without a instance-object");
	end
end



--@description: Actualises the Player visuals in game
--@param void
--@return void
function CharEditor:setVisual()
	if self then 	
		--print( HumBodys[self.BodyModelId].."  "..self.BodyTextureId.."  ".. HumHeads[self.HeadModelId].."  "..self.HeadTextureId)
		SetPlayerAdditionalVisual(self.MyNpc,HumBodys[self.BodyModelId], self.BodyTextureId, HumHeads[self.HeadModelId], self.HeadTextureId)
		SetPlayerAdditionalVisual(self.playID,HumBodys[self.BodyModelId], self.BodyTextureId, HumHeads[self.HeadModelId], self.HeadTextureId)
	else
		print("CharEditor:setVisual() is a member function and cannot be called without a instance-object");
	end
end



--@description: Closes this instance of the Character Editor
--@param void
--@return void
function CharEditor:Close()
	if self then
		DestroyNPC(self.MyNpc)
		SetPlayerAdditionalVisual(self.playID,HumBodys[self.BodyModelId], self.BodyTextureId, HumHeads[self.HeadModelId], self.HeadTextureId)
		--Destroy Draws
		DestroyPlayerDraw(self.playID, self.Message);
		DestroyPlayerDraw(self.playID, self.DrawHeadTex);
		DestroyPlayerDraw(self.playID, self.DrawHeadMesh);
		DestroyPlayerDraw(self.playID, self.DrawBodyTex);
		DestroyPlayerDraw(self.playID, self.DrawBodyMesh);
		DestroyPlayerDraw(self.playID, self.DrawClass);
		DestroyPlayerDraw(self.playID, self.DrawRotadeHelper);
		
		
		--Set Character Values
		SetPlayerFatness(self.playID, self.Fatnes)
		--EquipArmor(self.playID, "ITAR_Fake_RANGER")
		SetPlayerHealth(self.playID, ClassHealth[self.Class])
		SetPlayerMaxHealth(self.playID, ClassMax_Health[self.Class])
		SetPlayerStrength(self.playID, ClassStrength[self.Class])
		SetPlayerDexterity(self.playID, ClassDexterity[self.Class])
		SetPlayerSkillWeapon(self.playID, 0, Class1Handed[self.Class])
		SetPlayerSkillWeapon(self.playID, 1, Class2Handed[self.Class])
		SetPlayerSkillWeapon(self.playID, 2, ClassBow[self.Class])
		SetPlayerSkillWeapon(self.playID, 3, ClassCrossbow[self.Class])
		--Delete this char editor from the global list
		CharEditorList[self.playID]=nil;
		--TeleportPlayerToWayPoint(self.playID,"spawn_hafen");
		SetPlayerWorld(self.playID, "NEWWORLD\\NEWWORLD.ZEN", "SHIP");
		
		--Unfreeze the Player	
		CharEditUsedRooms[self.spawnroomnumber]=-1;
		FreezePlayer(self.playID, 0)
		self=nil;
	else
		print("CharEditor:Close() is a member function and cannot be called without a instance-object");
	end
end




--@description:	handels OnPlayKey.
--@param void
--@return void
function CharEditor:OnPlayerKey(playerid, press, release)
	if(playerid and press) then
		if CharEditorList[playerid]  then	

			local temEdit = CharEditorList[playerid] 
			FreezePlayer(playerid,1)
			TeleportPlayerToWayPoint(playerid,CharEditRooms_Player[temEdit.spawnroomnumber]);
			--Up
			if(press==91) then
				if(temEdit.MenueState == 0) then
						temEdit.MenueState = 4;
				else
					tempInt = temEdit.MenueState
					temEdit.MenueState =  tempInt-1;
				end
				temEdit:UpdateDrawSelection(temEdit.MenueState)
			--Down
			elseif(press ==96)then
				if(temEdit.MenueState == 4) then
					temEdit.MenueState = 0;
				else
					tempInt = temEdit.MenueState
					temEdit.MenueState =  tempInt+1;
				end
				temEdit:UpdateDrawSelection(temEdit.MenueState)
			--Right
			elseif(press==94)then
				if(temEdit.MenueState==0) then
					--Headtexture
					temEdit:nextHeadTexture()
				elseif (temEdit.MenueState==1) then
					--Headmesh
					temEdit:nextHeadModel()
				elseif (temEdit.MenueState==2) then
					--BodyTexture
					temEdit:nextBodyTexture()
				elseif (temEdit.MenueState==3) then
					--Fat
					temEdit:moreFatnes()
				elseif (temEdit.MenueState==4) then
				--Class
					if(temEdit.Class == ClassAmount) then
						temEdit.Class = 1;
					else
						tempInt = temEdit.Class;
						temEdit.Class =  tempInt+1;
					end
				UpdatePlayerDraw(playerid, temEdit.DrawClass,400,4750,"Startklasse: "..ClassDescriptions[temEdit.Class],"Font_Old_10_White_Hi.TGA",255,255,255);
				end
			--Left
			elseif(press==93)then
				if(temEdit.MenueState==0) then
					--Headtexture
					temEdit:lastHeadTexture()
				elseif (temEdit.MenueState==1) then
					--Headmesh
					temEdit:lastHeadModel()
				elseif (temEdit.MenueState==2) then
					--BodyTexture
					temEdit:lastBodyTexture();
				elseif (temEdit.MenueState==3) then
					--Fat
					temEdit:lessFatnes()
				elseif (temEdit.MenueState==4) then
				--Class
					if(temEdit.Class == 1) then
						temEdit.Class = ClassAmount;
					else
						tempInt = temEdit.Class;
						temEdit.Class =  tempInt-1;
					end
				UpdatePlayerDraw(playerid, temEdit.DrawClass,400,4750,"Startklasse: "..ClassDescriptions[temEdit.Class],"Font_Old_10_White_Hi.TGA",255,255,255);
				end 
			--Space
			elseif(press==56)then
					temEdit:Close()
			
			elseif press==15 then
			--Q
			local angle = temEdit.HelperAngle;
			if angle==0 then
				angle=270;
			else
				angle= angle-90;
			end
			temEdit.HelperAngle=angle;
			SetPlayerAngle(temEdit.MyNpc, angle)
			
			elseif press==17 then
			--E
			local angle = temEdit.HelperAngle;
			if angle==270 then
				angle=0;
			else
				angle= angle+90;
			end
			temEdit.HelperAngle=angle;
			SetPlayerAngle(temEdit.MyNpc, angle)
			else
				return 
			end
		end
	end
end


function CharEditor:OnPlayerUpdate(playerid)
		if CharEditorList[playerid]  then	
			local temEdit = CharEditorList[playerid] 
			FreezePlayer(playerid,1)
			TeleportPlayerToWayPoint(playerid,CharEditRooms_Player[temEdit.spawnroomnumber]);
		end
end

function CharEditor:OnPlayerDisconnect(playerid)
	if CharEditorList[playerid]  then	
		CharEditUsedRooms[CharEditorList[playerid].spawnroomnumber]=-1;
		DestroyNPC(CharEditorList[playerid].MyNpc);
		DestroyPlayerDraw(playerid, CharEditorList[playerid].Message);
		DestroyPlayerDraw(playerid, CharEditorList[playerid].DrawHeadTex);
		DestroyPlayerDraw(playerid, CharEditorList[playerid].DrawHeadMesh);
		DestroyPlayerDraw(playerid, CharEditorList[playerid].DrawBodyTex);
		DestroyPlayerDraw(playerid, CharEditorList[playerid].DrawBodyMesh);
		DestroyPlayerDraw(playerid, CharEditorList[playerid].DrawClass);
		DestroyPlayerDraw(playerid, CharEditorList[playerid].DrawRotadeHelper);
		CharEditorList[playerid]=nil;
	end
end

print("<CharEditor> loaded");