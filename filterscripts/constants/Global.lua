require "filterscripts/classes/Menu/Color"

--------
---GENERAL
--------
--the server name
GENERAL_HOSTNAME = "Minental-Online.net";
print("GENERAL_HOSTNAME = " .. tostring(GENERAL_HOSTNAME));

--the server gamemode name
GENERAL_GAMEMODE_NAME = "Roleplay";
print("GENERAL_GAMEMODE_NAME = " .. tostring(GENERAL_GAMEMODE_NAME));

--the server description
GENERAL_DESCRIPTION = "Das Minental, EINZIGARTIGE FEATURES!";
print("GENERAL_DESCRIPTION = " .. tostring(GENERAL_DESCRIPTION));

--debug mode
GENERAL_DEBUG_MODE = 0;
print("GENERAL_DEBUG_MODE = " .. tostring(GENERAL_DEBUG_MODE));

--the time a server restart kick is announced in advance
GENERAL_SERVER_RESTART_ANNOUNCE_TIME = 60; --IN SECONDS
print("GENERAL_SERVER_RESTART_ANNOUNCE_TIME = " .. tostring(GENERAL_SERVER_RESTART_ANNOUNCE_TIME));

--the default admin password
GENERAL_DEFAULT_ADMIN_PASSWORD = "A2o9mG";
print("GENERAL_DEFAULT_ADMIN_PASSWORD = " .. tostring(GENERAL_DEFAULT_ADMIN_PASSWORD));

--------
---DATABASE
--------
--the host of the database
DATABASE_HOST = "127.0.0.1";
print("DATABASE_HOST = " .. tostring(DATABASE_HOST));

--the username for authentication
DATABASE_USER = "root";
print("DATABASE_USER = " .. tostring(DATABASE_USER));

--the password for authentication
DATABASE_PASSWORD = "";
print("DATABASE_PASSWORD = " .. tostring(DATABASE_PASSWORD));

--the name of the database
DATABASE_NAME = "gothic_multiplayer";
print("DATABASE_NAME = " .. tostring(DATABASE_NAME));

--------
---CHAT
--------

--maximum characters without line break in chat
MAX_CHARS_PER_LINE = 60;
print("MAX_CHARS_PER_LINE = " .. tostring(MAX_CHARS_PER_LINE));

--the radius other players can 'hear' what a player says when this player whispers
CHAT_RADIUS_WHISPER = 300; -- in cm
print("CHAT_RADIUS_WHISPER = " .. tostring(CHAT_RADIUS_WHISPER));

--the radius other players can 'hear' what a player says when this player speaks in normal volume
CHAT_RADIUS_NORMAL = 1000; -- in cm
print("CHAT_RADIUS_NORMAL = " .. tostring(CHAT_RADIUS_NORMAL));

--the radius other players can 'hear' what a player says when this player shouts
CHAT_RADIUS_SHOUT = 5000; -- in cm
print("CHAT_RADIUS_SHOUT = " .. tostring(CHAT_RADIUS_SHOUT));

--the radius other players can 'see' what a player emotes
EMOTION_RADIUS = 1000; -- in cm
print("EMOTION_RADIUS = " .. tostring(EMOTION_RADIUS));

--------
---INGAME MENU AND MENUITEMS
--------

--the standard x-value of a menu
MENU_X_STANDARD = 250;
print("MENU_X_STANDARD = " .. tostring(MENU_X_STANDARD));

--the standard y-value of a menu
MENU_Y_STANDARD = 6000;
print("MENU_Y_STANDARD = " .. tostring(MENU_Y_STANDARD));

--maximum displayed options in menu
MAX_LINES_IN_MENU = 6;
print("MAX_LINES_IN_MENU = " .. tostring(MAX_LINES_IN_MENU));

--the height of each menu line (in points)
LINE_HEIGHT_IN_MENU = 200;
print("LINE_HEIGHT_IN_MENU = " .. tostring(LINE_HEIGHT_IN_MENU));

--the width the selected menu item is shifted to right
SELECTION_INDENT_IN_MENU = 150;
print("SELECTION_INDENT_IN_MENU = " .. tostring(SELECTION_INDENT_IN_MENU));

--the string that is displayed in front of the selected menu item
SELECTION_STRING_IN_MENU = "> ";
print("SELECTION_STRING_IN_MENU = " .. tostring(SELECTION_STRING_IN_MENU));

--the color of the headline of the ingame menu
COLOR_MENU_HEADLINE = Color.new(255, 255, 0);
print("COLOR_MENU_HEADLINE = " .. tostring(COLOR_MENU_HEADLINE));

--the color of the selected item of the ingame menu
COLOR_MENU_SELECTED_ITEM = Color.new(50, 205, 50);
print("COLOR_MENU_SELECTED_ITEM = " .. tostring(COLOR_MENU_SELECTED_ITEM));

--the color of all not-selected items of the ingame menu
COLOR_MENU_DESELECTED_ITEM = Color.new(200, 200, 200);
print("COLOR_MENU_DESELECTED_ITEM = " .. tostring(COLOR_MENU_DESELECTED_ITEM));


---------
---COLORS
---------

--color for neutral messages to the player
COLOR_NEUTRAL = Color.new(255, 255, 255);
print("COLOR_NEUTRAL = " .. tostring(COLOR_NEUTRAL));

--color for success messages to the player
COLOR_SUCCESS = Color.new(0, 255, 0);
print("COLOR_SUCCESS = " .. tostring(COLOR_SUCCESS));

--color for "you are trying to do"- or "you still need one more teach to ..."-message to the player
COLOR_TRY = Color.new(255, 255, 0);
print("COLOR_TRY = " .. tostring(COLOR_TRY));

--color for failure messages to the player
COLOR_FAILURE = Color.new(255, 0, 0);
print("COLOR_FAILURE = " .. tostring(COLOR_FAILURE));

--------
---TEACH
--------

--the time that a character has to be online per day to be able to teach
DAY_ONLINE_TIME_FOR_TEACH = 600; -- IN SECONDS
print("DAY_ONLINE_TIME_FOR_TEACH = " .. tostring(DAY_ONLINE_TIME_FOR_TEACH));

--the maximum skill value a player can reach by reading a book (without a special teach)
MAXIMUM_TEACH_SKILL_IN_BOOK = 100;
print("MAXIMUM_TEACH_SKILL_IN_BOOK = " .. tostring(MAXIMUM_TEACH_SKILL_IN_BOOK));

--------
---PLUNDER
--------

--the time that has to pass until a player can be plundered again
MIN_PLUNDER_SAFETY_TIME = 1800; -- IN SECONDS
print("MIN_PLUNDER_SAFETY_TIME = " .. tostring(MIN_PLUNDER_SAFETY_TIME));

--the amount of gold (in percent / 100) that is stolen when a player is plundered
PLUNDER_PERCENT = 0.25; -- BETWEEN 0 AND 1
print("PLUNDER_PERCENT = " .. tostring(PLUNDER_PERCENT));

--------
---NAME SYSTEM
--------

--the standard x-value of the displayed player-name
NAME_X_STANDARD = 7200;
print("NAME_X_STANDARD = " .. tostring(NAME_X_STANDARD));

--the standard y-value of the displayed player-name
NAME_Y_STANDARD = 7400;
print("NAME_Y_STANDARD = " .. tostring(NAME_Y_STANDARD));

--------
---HUNGER
--------
--the interval of the hunger system
HUNGER_SYSTEM_INTERVAL = 180; -- IN SECONDS
print("HUNGER_SYSTEM_INTERVAL = " .. tostring(HUNGER_SYSTEM_INTERVAL));

--if the players hunger level is lower than this value, he will lose 1 hp per interval
HUNGER_BORDER_DECREMENT = 1;
print("HUNGER_BORDER_DECREMENT = " .. tostring(HUNGER_BORDER_DECREMENT));

--if the players hunger level is higher than this value, hi will gain 1 hp per interval
HUNGER_BORDER_INCREMENT1 = 50;
print("HUNGER_BORDER_INCREMENT1 = " .. tostring(HUNGER_BORDER_INCREMENT1));

--if the players hunger level is higher than this value, hi will gain 2 hp per interval
HUNGER_BORDER_INCREMENT2 = 75;
print("HUNGER_BORDER_INCREMENT2 = " .. tostring(HUNGER_BORDER_INCREMENT2));


--------
---RESURRECTION
--------
--the maximum distance a player can revive another player
RESURRECTION_RADIUS = 1000; -- IN CENTIMETRES
print("RESURRECTION_RADIUS = " .. tostring(RESURRECTION_RADIUS));

--the amount of mana that is needed to revive one player
RESURRECTION_MANA_COST = 10;
print("RESURRECTION_MANA_COST = " .. tostring(RESURRECTION_MANA_COST));

--the vob-file that replaces a dead player
RESURRECTION_VOB = "ow_orc_standart_01.3DS";
print("RESURRECTION_VOB = " .. tostring(RESURRECTION_VOB));


--------
---MISCELLANEOUS
--------
--the interval to check player anims for crafting and teach menu
ANIM_CHECK_INTERVAL = 500; -- IN MILLISECONDS
print("ANIM_CHECK_INTERVAL = " .. tostring(ANIM_CHECK_INTERVAL));

--the interval of the garbage collector
GARBAGE_COLLECTOR_INTERVAL = 300; -- IN SECONDS
print("GARBAGE_COLLECTOR_INTERVAL = " .. tostring(GARBAGE_COLLECTOR_INTERVAL));

--the interval of the time recorder
TIME_RECORDER_INTERVAL = 60; -- IN SECONDS
print("TIME_RECORDER_INTERVAL = " .. tostring(TIME_RECORDER_INTERVAL));

--the interval of the inventory DB-update
INVENTORY_UPDATE_INTERVAL = 15; -- IN SECONDS
print("INVENTORY_UPDATE_INTERVAL = " .. tostring(INVENTORY_UPDATE_INTERVAL));

--the interval of the plant DB-update
PLANT_UPDATE_INTERVAL = 15; -- IN SECONDS
print("PLANT_UPDATE_INTERVAL = " .. tostring(PLANT_UPDATE_INTERVAL));
