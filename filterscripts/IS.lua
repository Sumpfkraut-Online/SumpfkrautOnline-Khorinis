
function OnPlayerCommandText(playerid, cmdtext)
	if cmdtext == "/anim1" then
	
		for i = 0, 8
			do
			SendPlayerMessage(playerid,255,255,255,"");
		end
		
		SendPlayerMessage(playerid,0,128,128,"---------------------------");
		SendPlayerMessage(playerid,0,128,128,"- Die Animationen -");
		SendPlayerMessage(playerid,0,128,128,"---------------------------");
		SendPlayerMessage(playerid,255,255,255,"");
		SendPlayerMessage(playerid,255,250,200,"/sit (auf dem Boden sitzen)");
		SendPlayerMessage(playerid,255,250,200,"/pee1 (pullern)");
		SendPlayerMessage(playerid,255,250,200,"/plunder (pluendern)");
		SendPlayerMessage(playerid,255,250,200,"/finish (Todesschlag)");
		SendPlayerMessage(playerid,255,250,200,"/training (Schwerttraining)");
		SendPlayerMessage(playerid,255,250,200,"/inspect (Waffe inspizieren)");
		SendPlayerMessage(playerid,255,250,200,"/search (umschauen)");
		SendPlayerMessage(playerid,255,250,200,"/fegen (fegen)");
		SendPlayerMessage(playerid,255,250,200,"/harken (harken)");

		
		
	elseif cmdtext == "/anim2" then
	
		for i = 0, 8
			do
			SendPlayerMessage(playerid,255,255,255,"");
		end
		
		SendPlayerMessage(playerid,255,250,200,"/dance (tanzen)");
		SendPlayerMessage(playerid,255,250,200,"/guard1 (mit verschraenkten Armen stehen)");
		SendPlayerMessage(playerid,255,250,200,"/guard2 (mit Haenden in den Hueften stehen)");
		SendPlayerMessage(playerid,255,250,200,"/pray1 (anbeten)");
		SendPlayerMessage(playerid,255,250,200,"/pray2 (auf einem Knie beten)");
		SendPlayerMessage(playerid,255,250,200,"/pray3 (auf beiden Knien beten)");
		SendPlayerMessage(playerid,255,250,200,"/dead1 (tot liegen auf dem Bauch)");
		SendPlayerMessage(playerid,255,250,200,"/dead2 (tot liegen auf dem Ruecken)");
		
	elseif cmdtext == "/sit" then
	   PlayAnimation(playerid,"T_STAND_2_SIT");
		
	elseif cmdtext == "/pee1" then
	   PlayAnimation(playerid,"T_STAND_2_PEE");
	   
	elseif cmdtext == "/eat" then
	   PlayAnimation(playerid,"T_EAT");
	   
	elseif cmdtext == "/pee2" then
	   PlayAnimation(playerid,"R_PEE");
		
	elseif cmdtext == "/plunder" then
	   PlayAnimation(playerid,"T_PLUNDER");
		
	elseif cmdtext == "/finish" then
	   PlayAnimation(playerid,"T_1HSFINISH");
		
	elseif cmdtext == "/training" then
	   PlayAnimation(playerid,"T_1HSFREE");
	   
	elseif cmdtext == "/inspect" then
	   PlayAnimation(playerid,"T_1HSINSPECT");
	   
	elseif cmdtext == "/search" then
	   PlayAnimation(playerid,"T_SEARCH");
	   
	elseif cmdtext == "/fegen" then
	   PlayAnimation(playerid,"S_BROOM_S1");
	   
	elseif cmdtext == "/harken" then
	   PlayAnimation(playerid,"S_RAKE_S1");

	elseif cmdtext == "/dance" then
	   PlayAnimation(playerid,"T_DANCE_01");
	   
	elseif cmdtext == "/guard1" then
	   PlayAnimation(playerid,"S_LGUARD");   

	elseif cmdtext == "/guard2" then
	   PlayAnimation(playerid,"S_HGUARD");
	 
	elseif cmdtext == "/pray1" then
	   PlayAnimation(playerid,"S_IDOL_S1");    

	elseif cmdtext == "/pray2" then
	   PlayAnimation(playerid,"S_INNOS_S1");    

	elseif cmdtext == "/pray3" then
	   PlayAnimation(playerid,"S_PRAY");   
	   
	elseif cmdtext == "/wounded" then
	   PlayAnimation(playerid,"S_WOUNDED");
	   
	elseif cmdtext == "/dead1" then
	   PlayAnimation(playerid,"S_DEAD");   
	
	elseif cmdtext == "/dead2" then
	   PlayAnimation(playerid,"S_DEADB");    
	 
	elseif cmdtext == "/sleep1" then
	   PlayAnimation(playerid,"S_SLEEP");    

	elseif cmdtext == "/sleep2" then
	   PlayAnimation(playerid,"S_SLEEPGROUND");
	   
	elseif cmdtext == "/warn" then
	   PlayAnimation(playerid,"T_WARN");

	elseif cmdtext == "/witterung" then
       PlayAnimation(playerid,"T_PERCEPTION");

	elseif cmdtext == "/sitsleep" then
       PlayAnimation(playerid,"S_GUARDSLEEP");
	   
	   	elseif cmdtext == "/test" then
SetPlayerHealth(playerid, 1)
	   
	   	         elseif cmdtext == "/magier" then
           SetPlayerWalk(playerid,"Humans_Mage.mds");
           SendPlayerMessage(playerid,238,180,34," Laufstil: Magier");
 
        elseif cmdtext == "/frau" then
           SetPlayerWalk(playerid,"Humans_Babe.mds");
           SendPlayerMessage(playerid,238,180,34,"Laufstil: Frau");
           
            elseif cmdtext == "/garde" then
           SetPlayerWalk(playerid,"Humans_Militia.mds");
           SendPlayerMessage(playerid,238,180,34,"Laufstil: Garde");
           
            elseif cmdtext == "/relax" then
           SetPlayerWalk(playerid,"Humans_Relaxed.mds");
           SendPlayerMessage(playerid,238,180,34,"Laufstil: Relaxed");
           
            elseif cmdtext == "/arrogant" then
           SetPlayerWalk(playerid,"Humans_Arrogance.mds");
           SendPlayerMessage(playerid,238,180,34,"Laufstil: Arrogant");
             
            elseif cmdtext == "/muede" then
           SetPlayerWalk(playerid,"Humans_Tired.mds");
           SendPlayerMessage(playerid,238,180,34,"Laufstil: Muede");
                   
            elseif cmdtext == "/laufstil" then
        SendPlayerMessage(playerid,220,20,60,"Style: /magier, /frau, /garde");
                    SendPlayerMessage(playerid,220,20,60,"/relax, /arrogant, /muede");
					
			elseif cmdtext == "/koenig" then
        SetPlayerKing(playerid);
elseif cmdtext == "/rebell" then
        SetPlayerRebell(playerid);
elseif cmdtext == "/feuermagier" then
        SetPlayerWizard(playerid);
elseif cmdtext == "/bandit" then
        SetPlayerSchurke(playerid);
elseif cmdtext == "/jaeger" then
        SetPlayerHunter(playerid);
elseif cmdtext == "/schmied" then
        SetPlayerSmith(playerid);
elseif cmdtext == "/bauer" then
        SetPlayerFarmer(playerid);
		
elseif cmdtext == "/aufstehen" then		
		CMD_AUFWACHEN(playerid);
		
elseif cmdtext == "/schlafen" then		
		CMD_SCHLAFEN(playerid);
		
			elseif cmdtext == "/teachschleichen" then
		CMD_schleichen(playerid, params);
		
			elseif cmdtext == "/teachausnehmen" then
		CMD_ausnehmen(playerid, params);
		
			elseif cmdtext == "/teachdiebstahl" then
		CMD_diebstahl(playerid, params);
		
	   elseif cmdtext == "/spenden" then 
		CMD_SPENDEN(playerid);

		elseif cmdtext == "/diebstahl" then
		CMD_KLAUEN(playerid);
		
		elseif cmdtext == "/wirt" then
		CMD_WIRT(playerid);
		


		
	end
			   
	end
	

	function SetPlayerKing(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerHealth(focusid, 120);
                        SetPlayerMaxHealth(focusid, 120);
                        SetPlayerStrength(focusid, 60);
                        SetPlayerDexterity(focusid, 30);
                        SetPlayerMana(focusid, 10);
                        SetPlayerMaxMana(focusid, 10);
                        SetPlayerSkillWeapon(focusid, 0, 60);
                        SetPlayerSkillWeapon(focusid, 1, 60);
                        SetPlayerSkillWeapon(focusid, 2, 30);
                        SetPlayerSkillWeapon(focusid, 3, 60);
                        EquipArmor(focusid, "ITAR_PAL_H");
                        EquipMeleeWeapon(focusid, "ItMw_Schlachtaxt");
                        GiveItem(focusid, "itmw_elbastardo", 1); 
                        GiveItem(focusid, "ITAR_SLD_M", 5);      
                        GiveItem(focusid, "ITAR_SLD_H", 5);
                        GiveItem(focusid, "ITAR_MIL_L", 10);						
                        GiveItem(focusid, "ItMi_Gold", 10000);  
                        GiveItem(focusid, "ItRw_Crossbow_M_02", 1);      
                        GiveItem(focusid, "ItRw_Bolt", 500);
                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end

	function CMD_WIRT(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerHealth(focusid, 90);
                        SetPlayerMaxHealth(focusid, 90);
                        SetPlayerStrength(focusid, 15);
                        SetPlayerDexterity(focusid, 25);
                        SetPlayerMana(focusid, 5);
                        SetPlayerMaxMana(focusid, 5);
                        SetPlayerSkillWeapon(focusid, 0, 10);
                        SetPlayerSkillWeapon(focusid, 1, 5);
                        SetPlayerSkillWeapon(focusid, 2, 5);
                        SetPlayerSkillWeapon(focusid, 3, 5);
                        EquipArmor(focusid, "ITAR_VLK_L");
                        EquipMeleeWeapon(focusid, "ItMw_1h_Vlk_Axe"); 						
                        GiveItem(focusid, "ItFo_Beer", 500);  
                        GiveItem(focusid, "ItFo_Water", 500); 
                        GiveItem(focusid, "ItFo_Wine", 500); 
                        GiveItem(focusid, "ItFo_Milk", 500); 
                        GiveItem(focusid, "ItFo_Booze", 500); 
						GiveItem(focusid, "ITAR_BARKEEPER", 10);
                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end

function SetPlayerRebell(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerHealth(focusid, 120);
                        SetPlayerMaxHealth(focusid, 120);
                        SetPlayerStrength(focusid, 60);
                        SetPlayerDexterity(focusid, 30);
                        SetPlayerMana(focusid, 10);
                        SetPlayerMaxMana(focusid, 10);
                        SetPlayerSkillWeapon(focusid, 0, 60);
                        SetPlayerSkillWeapon(focusid, 1, 60);
                        SetPlayerSkillWeapon(focusid, 2, 30);
                        SetPlayerSkillWeapon(focusid, 3, 60);
                        SetPlayerScience(focusid, 3, 1);
                        EquipArmor(focusid, "ITAR_Thorus_Addon");
                        EquipMeleeWeapon(focusid, "ItMw_Sturmbringer");
                        GiveItem(focusid, "ItRw_Crossbow_M_02", 1);
                        GiveItem(focusid, "ItMi_Gold", 10000);
                        GiveItem(focusid, "ITAR_Thorus_Addon", 5);
                        GiveItem(focusid, "itmw_elbastardo", 1);      
                        GiveItem(focusid, "ITAR_Bloodwyn_Addon", 5);
                        GiveItem(focusid, "ITAR_Diego", 5);
                        GiveItem(focusid, "ItRw_Bolt", 500);
                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end    
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end
 
function SetPlayerWizard(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerHealth(focusid, 90);
                        SetPlayerMaxHealth(focusid, 90);
                        SetPlayerStrength(focusid, 30);
                        SetPlayerDexterity(focusid, 15);
                        SetPlayerMana(focusid, 100);
                        SetPlayerMaxMana(focusid, 100);
                        SetPlayerSkillWeapon(focusid, 1, 30);
                        SetPlayerMagicLevel(focusid, 6);
                        EquipArmor(focusid, "ITAR_KDF_H");
                        GiveItem(focusid, "ITAR_KDF_H", 2);						
                        GiveItem(focusid, "ITAR_NOV_L", 5);
                        GiveItem(focusid, "ITAR_KDF_L", 5);
                        GiveItem(focusid, "ItRu_FireBolt", 5);
                        GiveItem(focusid, "ItRu_Icebolt", 5);
                        GiveItem(focusid, "ItRu_Icelance", 1);
                        GiveItem(focusid, "ItMi_Gold", 10000);
                        GiveItem(focusid, "ItMi_Flask", 150);
						GiveItem(focusid,"ItKe_KlosterBibliothek",15);
						GiveItem(focusid,"ItKe_KlosterStore",15);
						GiveItem(focusid,"ItKe_KlosterSchatz",15);
						GiveItem(focusid,"ItKe_KDFPlayer",15);
						GiveItem(focusid,"ItKe_Innos_MIS",15);
						GiveItem(focusid,"ItPo_Mana_Addon_04",20);
						GiveItem(focusid,"ItMi_RuneBlank",10);
                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end
 
function SetPlayerSchurke(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerHealth(focusid, 120);
                        SetPlayerMaxHealth(focusid, 120);
                        SetPlayerStrength(focusid, 60);
                        SetPlayerDexterity(focusid, 30);
                        SetPlayerSkillWeapon(focusid, 0, 60);
                        SetPlayerSkillWeapon(focusid, 1, 30);
                        SetPlayerSkillWeapon(focusid, 2, 60);
                        SetPlayerSkillWeapon(focusid, 3, 30);
                        SetPlayerScience(focusid, SCIENCE_THIEF, 1);
                        SetPlayerScience(focusid, SCIENCE_SNEAKING, 1);
                        EquipArmor(focusid, "ITAR_OreBaron_Addon");
                        EquipMeleeWeapon(focusid, "ItMw_Sturmbringer");
                        GiveItem(focusid, "ITAR_BDT_M", 5);
                        GiveItem(focusid, "ITAR_BDT_H", 5);
                        GiveItem(focusid, "ITAR_SLD_L", 5);
						GiveItem(focusid,"ItKe_ThiefGuildKey_MIS",15);
						GiveItem(focusid,"itke_thiefguildkey_hotel_mis",10);
                        GiveItem(focusid, "ItMi_Gold", 10000);
                        GiveItem(focusid, "ItRw_Arrow", 500);
                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end
               
function SetPlayerHunter(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerHealth(focusid, 100);
                        SetPlayerMaxHealth(focusid, 100);				
                        SetPlayerStrength(focusid, 45);
                        SetPlayerDexterity(focusid, 55);
                        SetPlayerSkillWeapon(focusid, 0, 15);
                        SetPlayerSkillWeapon(focusid, 2, 30);
                        SetPlayerScience(focusid, 6, 1);
						GiveItem(focusid, "ItMi_Gold", 1500);
                        EquipArmor(focusid, "ITAR_DJG_Crawler");
                        EquipMeleeWeapon(focusid, "ItMw_2h_Sld_Sword");
                        GiveItem(focusid, "ItRw_Bow_L_03", 1);
                        GiveItem(focusid, "ItRw_Arrow", 500);
                        GiveItem(focusid, "ItAt_WolfFur", 50);
                        GiveItem(focusid, "ItAt_Addon_KeilerFur", 50);	
						GiveItem(focusid, "TtAt_LurkerSkin", 25);			
                        GiveItem(focusid, "ItAt_CrawlerPlate", 100);
                        GiveItem(focusid, "ItAt_ShadowFur", 20);
                        GiveItem(focusid, "ItAt_WargFur", 50);
						GiveItem(focusid, "ItWr_Map_NewWorld_City", 25);
						GiveItem(focusid, "ItWr_Map_NewWorld",25);

						
                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end
 
function SetPlayerSmith(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerHealth(focusid, 110);
                        SetPlayerMaxHealth(focusid, 110);				
                        SetPlayerStrength(focusid, 55);
                        SetPlayerDexterity(focusid, 45);
                        SetPlayerMana(focusid, 30);
                        SetPlayerMaxMana(focusid, 30);
                        SetPlayerSkillWeapon(focusid, 0, 50);
                        SetPlayerSkillWeapon(focusid, 1, 50);
                        SetPlayerSkillWeapon(focusid, 3, 50);
                        SetPlayerSkillWeapon(focusid, 2, 10);
                        SetPlayerScience(focusid, 3, 1);
                        EquipArmor(focusid, "ITAR_SMITH");
                        EquipMeleeWeapon(focusid, "ItMw_1H_Mace_L_06");
						GiveItem(focusid, "ItMi_Gold", 1500);
                        GiveItem(focusid, "ItPo_Health_Addon_04", 5);
                        GiveItem(focusid, "ItMi_Pliers", 1);
                        GiveItem(focusid, "ItMiSwordraw", 1500);
                        GiveItem(focusid, "ItMi_Saw", 1);
						GiveItem(focusid, "ItMw_2H_OrcAxe_03", 1);
						GiveItem(focusid, "ItMw_2H_OrcAxe_04", 1);
						GiveItem(focusid, "ITAR_VLK_L", 5);
						GiveItem(focusid, "ITAR_VLK_M", 5);
						GiveItem(focusid, "ITAR_VLK_H", 5);
						GiveItem(focusid, "ITAR_Leather_L", 5);
						GiveItem(focusid, "ITAR_SMITH", 2)
						GiveItem(focusid, "ITAR_Prisoner", 5);
						GiveItem(focusid, "ITAR_RANGER_Addon", 5);
						GiveItem(focusid, "ItMi_Scoop", 5);
						GiveItem(focusid, "ItMi_Pan", 5);

                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end
 
function SetPlayerFarmer(playerid)
        if IsPlayerAdmin(playerid) == 1 then
                local focusid = GetFocus(playerid);
                if focusid >= 0 then
                        SetPlayerStrength(focusid, 30);
                        SetPlayerDexterity(focusid, 30);
                        SetPlayerSkillWeapon(focusid, 0, 35);
                        SetPlayerScience(focusid, 6, 1);
                        EquipArmor(focusid, "ITAR_BAU_M");
                        EquipMeleeWeapon(focusid, "ItMw_1h_Bau_Axe");
                        GiveItem(focusid, "ItFo_Sausage", 500);
						GiveItem(focusid, "ItMi_Gold", 1500);
						GiveItem(focusid, "ItFoMutton", 500);
						GiveItem(focusid, "ItFo_Bread", 500);
						GiveItem(focusid, "ItFo_Bacon", 500);
						GiveItem(focusid, "ItFo_Sausage", 500);
						GiveItem(focusid, "ItFo_Water", 5000);
						GiveItem(focusid, "ITAR_BAU_L", 5);
						GiveItem(focusid, "ITAR_BAU_M", 5);
                else
                        SendPlayerMessage(playerid, 255, 0, 0, "Kein Spieler im Focus");
                end
        else
                SendPlayerMessage(playerid, 255, 0, 0, "Du bist kein Admin");
        end
end

function CMD_schleichen(playerid)
local focusid = GetFocus(playerid);
 
if focusid >= 0 and IsNPC(focusid) == 0
then
        if GetPlayerLearnPoints(focusid) >= 10
        then
                if GetPlayerScience(playerid,SCIENCE_SNEAKING) == 1 and GetPlayerScience(focusid,SCIENCE_SNEAKING) == 0
                then
                        if GetPlayerDexterity(focusid) >= 30
                        then
                        SetPlayerLearnPoints(focusid, GetPlayerLearnPoints(focusid) - 10);
                        SetPlayerScience(playerid,SCIENCE_SNEAKING,1);
						SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 15);
                        SendPlayerMessage(playerid,255,250,200,"Dein Schueler hat Schleichen gelernt");
                        SendPlayerMessage(focusid,255,250,200,"Du hast Schleichen gelernt");
                else
                        SendPlayerMessage(playerid,255,250,200,"Du kannst diesen Spieler das Talent nicht beibringen");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Dieser Spieler hat nicht genug Geschicklichkeit");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Dieser Spieler hat nicht genug Erfahrung");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Keinen Spieler im Fokus");
        end
		end


function CMD_diebstahl(playerid)
local focusid = GetFocus(playerid);
 
if focusid >= 0 and IsNPC(focusid) == 0
then
        if GetPlayerLearnPoints(focusid) >= 10
        then
                if GetPlayerScience(playerid,1) == 1 and GetPlayerScience(focusid,1) == 0
                then
                        if GetPlayerDexterity(focusid) >= 30
                        then
                        SetPlayerLearnPoints(focusid, GetPlayerLearnPoints(focusid) - 10);
                        SetPlayerScience(playerid,1,1);
						SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 15);
                        SendPlayerMessage(playerid,255,250,200,"Dein Schueler hat Taschendiebstahl gelernt");
                        SendPlayerMessage(focusid,255,250,200,"Du hast Taschendiebstahl gelernt");
                else
                        SendPlayerMessage(playerid,255,250,200,"Du kannst diesen Spieler das Talent nicht beibringen");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Dieser Spieler hat nicht genug Geschicklichkeit");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Dieser Spieler hat nicht genug Erfahrung");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Keinen Spieler im Fokus");
        end
end

function CMD_ausnehmen(playerid)
local focusid = GetFocus(playerid);
 
if focusid >= 0 and IsNPC(focusid) == 0
then
        if GetPlayerLearnPoints(focusid) >= 10
        then
                if GetPlayerScience(playerid,6) == 1 and GetPlayerScience(focusid,6) == 0
                then
                        if GetPlayerDexterity(focusid) >= 30
                        then
                        SetPlayerLearnPoints(focusid, GetPlayerLearnPoints(focusid) - 15);
                        SetPlayerScience(playerid,6,1);
						SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 15);
                        SendPlayerMessage(playerid,255,250,200,"Dein Schueler hat Tiere ausnehmen gelernt");
                        SendPlayerMessage(focusid,255,250,200,"Du hast Tiere ausnehmen gelernt");
                else
                        SendPlayerMessage(playerid,255,250,200,"Du kannst diesen Spieler das Talent nicht beibringen");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Dieser Spieler hat nicht genug Geschicklichkeit");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Dieser Spieler hat nicht genug Erfahrung");
                        end
                else
                        SendPlayerMessage(playerid,255,250,200,"Keinen Spieler im Fokus");
        end
end

function CMD_KLAUEN(playerid)
        local anim = GetPlayerAnimationID(playerid);
        local science = GetPlayerScience(playerid,SCIENCE_THIEF);
        if anim == 277 then
                if science == 1 then
                        local focusid = GetFocus(playerid);
                        local klaumsg = string.format("Man hat dich bestohlen!");
                        local klaumsg2 = string.format("%s %s",GetPlayerName(focusid),"hat dich erwischt");
                        local klaumsg3 = string.format("Jemand hat versucht dich zu bestehlen!");
                        local goldrand = random(6);
                        local klaurand = random(2);
                        if focusid >= 0 and IsNPC(focusid) == 0 then
                                if GetPlayerGold(focusid) >= 60 then
                                        if klaurand == 1 then
                                                if goldrand == 0 then
                                                        GiveItem(playerid,"ItMi_Gold",8);
                                                        RemoveItem(focusid,"ItMi_Gold",8);
                                                        SendPlayerMessage(playerid,255,250,200,"8 Goldstuecke erhalten");
                                                        SendPlayerMessage(focusid,255,250,200,klaumsg);
                                                        SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 1);
                                                elseif goldrand == 1 then
                                                        GiveItem(playerid,"ItMi_Gold",15);
                                                        RemoveItem(focusid,"ItMi_Gold",15);
                                                        SendPlayerMessage(playerid,255,250,200,"15 Goldstuecke erhalten");
                                                        SendPlayerMessage(focusid,255,250,200,klaumsg);
                                                        SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 2);
                                                elseif goldrand == 2 then
                                                        GiveItem(playerid,"ItMi_Gold",20);
														RemoveItem(focusid,"ItMi_Gold",20);
                                                        SendPlayerMessage(playerid,255,250,200,"20 Goldstuecke erhalten");
                                                        SendPlayerMessage(focusid,255,250,200,klaumsg);
                                                        SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 3);
                                                elseif goldrand == 3 then
                                                        GiveItem(playerid,"ItMi_Gold",30);
                                                        RemoveItem(focusid,"ItMi_Gold",30);
                                                        SendPlayerMessage(playerid,255,250,200,"30 Goldstuecke erhalten");
                                                        SendPlayerMessage(focusid,255,250,200,klaumsg);
                                                        SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 4);
                                                elseif goldrand == 4 then
                                                        GiveItem(playerid,"ItMi_Gold",35);
                                                        RemoveItem(focusid,"ItMi_Gold",35);
                                                        SendPlayerMessage(playerid,255,250,200,"35 Goldstuecke erhalten");
                                                        SendPlayerMessage(focusid,255,250,200,klaumsg);
                                                        SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 5);
                                                elseif goldrand == 5 then
                                                        GiveItem(playerid,"ItMi_Gold",40);
                                                        RemoveItem(focusid,"ItMi_Gold",40);
                                                        SendPlayerMessage(playerid,255,250,200,"40 Goldstuecke erhalten");
                                                        SendPlayerMessage(focusid,255,250,200,klaumsg);
                                                        SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 6);
                                                end
                                        else
                                                SendPlayerMessage(playerid,255,250,200,klaumsg2);
                                                SendPlayerMessage(focusid,255,250,200,klaumsg3);
                                        end
                                else
                                        SendPlayerMessage(playerid,255,250,200,"Hier gibts nichts zu holen");
                                end
                        else
                                SendPlayerMessage(playerid,255,250,200,"Keinen Spieler im Fokus");
                        end
                end
        end
end

function CMD_SCHLAFEN(playerid)

local anim = GetPlayerAnimationID(playerid);
if anim == 92 then
SendPlayerMessage(playerid,46, 222, 204,"Du bist nicht im Bett!");
else
FreezePlayer(playerid,1)
SendPlayerMessage(playerid,0,255,0,"Du schlaefst nun! (Du bist nun AFK) ");
end
end

function CMD_AUFWACHEN(playerid)

local anim = GetPlayerAnimationID(playerid);
if anim == 92 then
SendPlayerMessage(playerid,46, 222, 204,"Du bist nicht am schlafen!");
else
FreezePlayer(playerid,0)
SendPlayerMessage(playerid,0,255,0,"Du bist wieder wach! (Nicht mehr AFK) ");
end
end

function CMD_SPENDEN(playerid)

local anim = GetPlayerAnimationID(playerid);
if anim == 210 then
HasItem(playerid,"ItMi_Gold",99);
else
SendPlayerMessage(playerid,46, 222, 204,"Du bist nicht am Schrein!");
end
end

function OnPlayerHasItem(playerid, item_instance, amount, equipped, checkid)
	    item_instance = string.upper(item_instance);
    if checkid == 99 then
    if item_instance ~= "NULL" then
    if item_instance == string.upper("ItMi_Gold") then
    if amount >= 50 then
    GiveItem(playerid,"ItFo_Bread",1);
    RemoveItem(playerid,"ItMi_Gold",50);
	SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 2);
	SetPlayerHealth(playerid, GetPlayerHealth(playerid)+20);
    SendPlayerMessage(playerid,0,255,0,"INNOS SEGNE DICH !! // + 2 EXP");
	PlayAnimation(playerid,"T_PRACTICEMAGIC3");
    else
    SendPlayerMessage(playerid,0,255,0,"Du hast nicht genug Gold (50 Gold ) !!");
    end
    end
    else
    SendPlayerMessage(playerid,0,255,0,"Du besitzt kein Gold!!");
    end
    end
	
		    item_instance = string.upper(item_instance);
    if checkid == 1 then
    if item_instance ~= "NULL" then
    if item_instance == string.upper("itat_LurkerSkin") then
    if amount >= 50 then
    GiveItem(playerid,"ItWr_Map_NewWorld",1);
    RemoveItem(playerid,"itat_LurkerSkin",1);
	SetPlayerExperience(playerid, GetPlayerExperience(playerid) + 1);
    SendPlayerMessage(playerid,0,255,0,"Khorinis Karte gezeichnet !! // + 1 EXP");
	PlayAnimation(playerid,"S_MAP_S0");
    else
    SendPlayerMessage(playerid,0,255,0,"Du hast keine Haut oder Kohle !!");
    end
    end
    else
    SendPlayerMessage(playerid,0,255,0,"Du besitzt keine Haut/Kohle!!");
    end
    end
	
end	


local MetalWeapons = {
["ITMW_1H_VLK_DAGGER"] = true,
["ItMw_1H_Mace_L_04"] = true,
["ItMw_ShortSword1"] = true,
["ItMw_1H_Sword_L_03"] = true,
["ItMw_ShortSword2"] = true,
["ItMw_1h_Vlk_Sword"] = true,
["ItMw_1h_MISC_Sword"] = true,
["ItMw_1h_Misc_Axe"] = true,
["ItMw_2H_Sword_M_01"] = true,
["ItMw_1h_Mil_Sword"] = true,
["ItMw_1h_Sld_Axe"] = true,
["ItMw_1h_Sld_Sword"] = true,
["ItMw_2h_Sld_Axe"] = true,
["ItMw_2h_Sld_Sword"] = true,
["ItMw_1h_Pal_Sword"] = true,
["ItMw_2h_Pal_Sword"] = true,
["ItMw_2H_OrcAxe_01"] = true,
["ItMw_2H_OrcAxe_02"] = true,
["ItMw_2H_OrcAxe_03"] = true,
["ItMw_2H_OrcAxe_04"] = true,
["ItMw_2H_OrcSword_01"] = true,
["ItMw_2H_OrcSword_02"] = true,
["ItMw_ShortSword3"] = true,
["ItMw_ShortSword4"] = true,
["ItMw_Kriegskeule"] = true,
["ItMw_ShortSword5"] = true,
["ItMw_Kriegshammer1"] = true,
["ItMw_Hellebarde"] = true,
["ItMw_Piratensaebel"] = true,
["ItMw_1H_Common_01"] = true,
["ItMw_Zweihaender1"] = true,
["ItMw_Morgenstern"] = true,
["ItMw_Schwert3"] = true,
["ItMw_1H_Special_01"] = true,
["ItMw_2H_Special_01"] = true,
["ItMw_Zweihaender2"] = true,
["ItMw_Orkschlaechter"] = true,
["ItMw_Meisterdegen"] = true,
["ItMw_Folteraxt"] = true,
["ItMw_Krummschwert"] = true,
["ItMw_1H_Sword_L_06"] = true,
["ItMw_1H_Mace_L_09"] = true,
["ItMw_1H_Sword_M_02"] = true,
["ItMw_1H_Sword_H_01"] = true

};



function IsWeaponMetal(item)
item = string.upper(item);
return MetalWeapons[item]==true;
end

function OnPlayerHit(playerid, killerid)
if IsNPC(playerid) == 0 then

	local rand = random(3);

	   	for i = 0, GetMaxSlots() - 1
		do
			if IsPlayerConnected(i) == 1
			then
			
				if GetDistancePlayers(playerid,i) < 1000
				then
					if rand == 0 then
					PlayPlayerSound(i,CreateSound("SVM_12_AARGH_1.WAV"));
					elseif rand == 1 then
					PlayPlayerSound(i,CreateSound("SVM_12_AARGH_2.WAV"));
					elseif rand == 2 then
					PlayPlayerSound(i,CreateSound("SVM_12_AARGH_3.WAV"));
			end
		end
	end
end
end
end

function OnPlayerStandUp(playerid)
 
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(3);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_12_OHMYGODHESDOWN.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_12_OHMYGODITSAFIGHT.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_12_OHMYHEAD.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 1000);
end
			end
		end
		
function OnPlayerSpellSetup(playerid, spellInstance)
 
    local msg = string.format("%s %s","Dein aktueller Zauber ist:",spellInstance);
    SendPlayerMessage(playerid,255,255,0,msg);
end

function OnPlayerDeath(playerid, p_classid, killerid, k_classid)
if IsNPC(playerid) == 0 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(3);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_14_DEAD.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_13_DEAD.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_15_DEAD.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 3000);
end
end
end

if IsNPC(playerid) == 0 then
SetPlayerPos(playerid,45239.16015625,3137.9907226563,3333.822265625);
SetPlayerAngle(playerid,221);
PlayAnimation(playerid,"S_DEAD");
FreezePlayer(playerid,1);
SendPlayerMessage(playerid,255,250,200,"Du wurdest von einem Unbekannten in ein unbekanntes Lager gebracht!");
SendPlayerMessage(playerid,255,250,200,"Hinweis: Spiele vorsichtiger! Du wirst pro Tod immer schwaecher!");
SendPlayerMessage(playerid,255,250,200,"Hinweis: Du kannst in 5 Minuten weiterspielen!");
SetTimerEx("TOD",300000,0,playerid)
end


function TOD(playerid)

	 		if (GetPlayerHealth(playerid) <= 0) then
			SpawnPlayer(playerid);
		end
	 SetPlayerPos(playerid,44940.18359375,3125.1279296875,3000.5388183594);
	 SetPlayerAngle(playerid,220);
	 PlayAnimation(playerid,"S_RUN");
	 SetPlayerStrength(playerid, GetPlayerStrength(playerid) - 1);
	 FreezePlayer(playerid,0);
	 SetTimerEx("RELOGG",3000,0,playerid)
end
end

function RELOGG(playerid)
SendPlayerMessage(playerid,255,250,200,"Du musst aufgrund des eines GMP Bug reloggen!");
Kick(playerid);
end

function OnPlayerTakeItem(playerid, itemID, itemInstance, amount, x, y, z, worldName)
 
       if itemID == ITEM_UNSYNCHRONIZED then
 
           GiveItem(playerid,itemInstance,amount);
end
end

function OnPlayerKey(playerid, keyDown, keyUp)
if keyDown == KEY_NUMPAD1 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_8_HELP.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_5_HELP.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_6_HELP.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_7_HELP.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 5000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD2 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_6_GUARDS.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_8_GUARDS.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_7_GUARDS.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_5_GUARDS.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 5000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD3 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_7_ITOOKYOURGOLD.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_6_ITOOKYOURGOLD.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_5_ITOOKYOURGOLD.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_8_ITOOKYOURGOLD.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 1000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD4 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_5_CHEERFRIEND03.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_6_CHEERFRIEND03.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_7_CHEERFRIEND03.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_8_CHEERFRIEND03.WAV");
end
PlayAnimation(playerid,"T_WATCHFIGHT_YEAH");
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 3000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD5 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_7_MONSTERKILLED.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_5_MONSTERKILLED.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_6_MONSTERKILLED.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_8_MONSTERKILLED.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 3000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD6 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_6_ISAIDWEAPONDOWN.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_5_ISAIDWEAPONDOWN.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_7_ISAIDWEAPONDOWN.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_8_ISAIDWEAPONDOWN.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 3000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD7 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_1_RUNCOWARD.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_5_RUNCOWARD.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_6_RUNCOWARD.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_8_RUNCOWARD.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 4000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD8 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_6_DIEMONSTER.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_7_DIEMONSTER.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_8_DIEMONSTER.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_5_DIEMONSTER.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 3000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD9 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_1_YOUASKEDFORIT.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_3_YOUASKEDFORIT.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_4_YOUASKEDFORIT.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_5_YOUASKEDFORIT.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 3000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_NUMPAD0 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_7_MILGREETINGS.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_6_MILGREETINGS.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_5_MILGREETINGS.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_8_MILGREETINGS.WAV");
end
PlayAnimation(playerid,"T_GREETGRD");
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 1500);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
elseif keyDown == KEY_DIVIDE then
if IsPlayerAdmin(playerid) == 1 then
local x, y, z = GetPlayerPos(playerid);
local soundid = 0;
local rand = random(4);
if rand == 0 then --Random Sound wird erstellt
soundid = CreateSound("SVM_19_DEAD.WAV");
elseif rand == 1 then
soundid = CreateSound("SVM_19_DIEENEMY.WAV");
elseif rand == 2 then
soundid = CreateSound("SVM_19_AARGH.WAV");
elseif rand == 3 then
soundid = CreateSound("SVM_19_RUNCOWARD.WAV");
end
for i = 0, GetMaxPlayers()-1 do --Sound wird an alle Spieler in der Umgebung abgespielt
if IsPlayerConnected(i) == 1 then
PlayPlayerSound3D(i, soundid, x, y, z, 3000);
end
end
DestroySound(soundid); --Der Sound wird wieder zerstört
end
end
end

function OnPlayerConnect(playerid)
if IsNPC(playerid) == 0 then
GetMD5File(playerid, "Data\\SO-K.vdf");
end
end

function OnPlayerMD5File(playerid, pathFile, hash)
print(hash);
if hash ~= "e8afc4db5a1adea04bfa9c5278200a42" then
SendPlayerMessage(playerid, 255, 0, 0, "Falsche .mod Datei bitte lade die aktuelle im Forum herunter");
Kick(playerid);
else
SendPlayerMessage(playerid, 0, 255, 0, ".mod Datei ist aktuell");
end
end


-- Loaded
print(debug.getinfo(1).source.." has been loaded.");
