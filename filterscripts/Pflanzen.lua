SetTimer("pflanzenspawn", 60000*60, 1); --60000 == eine Minute
local maxpflanzen =389; -- Maximale Anzahl der Pflanzen die gespawnt werden
function pflanzenspawn()
for i=0, maxpflanzen do
if pflanze[i].spawned == 0 then -- prüft ob pflanze schon gespawnt
local itemID = CreateItem(pflanze[i].instance, pflanze[i].amount, pflanze[i].x, pflanze[i].y, pflanze[i].z, pflanze[i].world);
pflanze[i].itemID = itemID; -- itemID speichern
pflanze[i].spawned = 1; -- 1 = gespawnt // 0 = nicht gespawnt
else
--es wird nichts gespawnt
end
end
end
-- wenn der Spieler ein item aufhebt
function OnPlayerTakeItem(playerid, itemID, itemInstance, amount, x, y, z, worldName)
for i=0, maxpflanzen do
if itemID == pflanze[i].itemID then
pflanze[i].spawned = 0; -- Pflanze wird anhand der itemID identifiziert und auf nicht gespawnt gesetzt
end
end
end


function OnFilterscriptInit()
pflanze = {};
pflanze[0] = {
instance = "ItPl_Mushroom_01",
amount = 3,
x = 11840.392578125,
y = 348.90982055664,
z = 5942.9340820313,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[1] = {
instance = "ItPl_Mushroom_02",
amount = 3,
x = 13691.801757813,
y = 327.13677978516,
z = 5215.4897460938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[2] = {
instance = "ItPl_Health_Herb_03",
amount = 3,
x = 15054.151367188,
y = 298.28533935547,
z = 6548.7221679688,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[3] = {
instance = "ItPl_Health_Herb_03",
amount = 3,
x = 1,
y = 1,
z = 1,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[4] = {
instance = "ItPl_Mushroom_02",
amount = 3,
x = 18596.712890625,
y = 890.59808349609,
z = 5139.9262695313,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[5] = {
instance = "ItPl_Mushroom_01",
amount = 3,
x = 18743.671875,
y = 891.40789794922,
z = 5564.9204101563,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[6] = {
instance = "ItPl_Health_Herb_01",
amount = 3,
x = 18962.640625,
y = 1263.2595214844,
z = 2771.9736328125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[7] = {
instance = "ItPl_Health_Herb_01",
amount = 3,
x = 20233,
y = 1182,
z = 3268,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[8] = {
instance = "ItPl_Health_Herb_01",
amount = 3,
x = 22933,
y = 1418,
z = 3730,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[9] = {
instance = "ItPl_Health_Herb_03",
amount = 2,
x = 26276,
y = 2342,
z = 4635,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[10] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 27834,
y = 2577,
z = 4517,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[11] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 30380,
y = 2898,
z = 3998,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[12] = {
instance = "ItPl_Mana_Herb_02",
amount = 2,
x = 31750,
y = 3344,
z = 1675,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[13] = {
instance = "ItPl_Mushroom_01",
amount = 3,
x = 32276,
y = 3514,
z = 563,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[14] = {
instance = "ItPl_Forestberry",
amount = 2,
x = 34444,
y = 3754,
z = 774,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[15] = {
instance = "ItPl_Forestberry",
amount = 4,
x = 36928,
y = 3915,
z = -913,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[16] = {
instance = "ItPl_Planeberry",
amount = 4,
x = 39154,
y = 3867,
z = -1121,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[17] = {
instance = "ItPl_Mushroom_01",
amount = 2,
x = 39425,
y = 3910,
z = -3179,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[18] = {
instance = "ItPl_Health_Herb_02",
amount = 1,
x = 38689, y = 3897,
z = -4252,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[19] = {
instance = "ItPl_Mana_Herb_01",
amount = 1,
x = 38156,
y = 3786,
z = -5327,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[20] = {
instance = "ItPl_Mana_Herb_02",
amount = 2,
x = 39760,
y = 3733,
z = -8700,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[21] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 41247,
y = 3263,
z = -10422,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[22] = {
instance = "ItPl_Mushroom_01",
amount = 1,
x = 41163,
y = 3167,
z = -11796,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[23] = {
instance = "ItPl_Health_Herb_03",
amount = 2,
x = 40590,
y = 3161,
z = -13292,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[24] = {
instance = "ItPl_Mushroom_02",
amount = 2,
x = 40774,
y = 2829,
z = -16188,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[25] = {
instance = "ItPl_Forestberry",
amount = 1,
x = 39849,
y = 2822,
z = -18640,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[26] = {
instance = "ItFo_Apple",
amount = 3,
x = 40419,
y = 2920,
z = -21528,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[27] = {
instance = "ItPl_Forestberry",
amount = 2,
x = 39871,
y = 2973,
z = -23856,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[28] = {
instance = "ItPl_Forestberry",
amount = 4,
x = 39405,
y = 2975,
z = -25737,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[29] = {
instance = "ItPl_Mana_Herb_02",
amount = 2,
x = 42847,
y = 2967,
z = -22571,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[30] = {
instance = "ItPl_Health_Herb_03",
amount = 2,
x = 46168,
y = 2930,
z = -22052,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[31] = {
instance = "ItPl_Mana_Herb_01",
amount = 2,
x = 48087,
y = 3154,
z = -21498,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[32] = {
instance = "ItPl_Temp_Herb",
amount = 3,
x = 50406,
y = 3120,
z = -21757,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[33] = {
instance = "ItPl_Weed",
amount = 3,
x = 50462,
y = 3142,
z = -22112,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[34] = {
instance = "ItPl_Weed",
amount = 3,
x = 50618,
y = 3112,
z = -22731,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[35] = {
instance = "ItPl_Temp_Herb",
amount = 4,
x = 50854,
y = 3143,
z = -23014,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[36] = {
instance = "ItPl_Beet",
amount = 2,
x = 51225,
y = 3160,
z = -23004,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[37] = {
instance = "ItPl_Beet",
amount = 2,
x = 51363,
y = 3171,
z = -22408,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[38] = {
instance = "ItFo_Honey",
amount = 2,
x = 51531,
y = 3120,
z = -20324,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[39] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 51177,
y = 3075,
z = -14371,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[40] = {
instance = "ItPl_Mana_Herb_01",
amount = 3,
x = 7075,
y = 400,
z = -7904,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[41] = {
instance = "ItPl_Mushroom_02",
amount = 5,
x = 5170.3666992188,
y = 465.34799194336,
z = -9444.9462890625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[42] = {
instance = "ItPl_Forestberry",
amount = 3,
x = 4326.9375,
y = 80.719482421875,
z = -11090.409179688,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[43] = {
instance = "ItPl_Mushroom_02",
amount = 3,
x = 4846.3510742188,
y = 64.09252166748,
z = -11800.489257813,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[44] = {
instance = "ItFo_Honey",
amount = 3,
x = 3952.4084472656,
y = 243.326171875,
z = -12680.322265625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[45] = {
instance = "ItPl_Mushroom_01",
amount = 2,
x = 2237.4125976563,
y = 252.09370422363,
z = -13340.375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[46] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 2316.337890625,
y = 310.48468017578,
z = -16886.927734375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[47] = {
instance = "ItPl_Mana_Herb_01",
amount = 3,
x = 4336.6694335938,
y = 577.97912597656,
z = -17394.615234375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[48] = {
instance = "ItPl_Mushroom_01",
amount = 2,
x = 3712.2626953125,
y = 647.17242431641,
z = -18382.671875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[49] = {
instance = "ItPl_Mushroom_02",
amount = 2,
x = 4766.6059570313,
y = 653.87353515625,
z = -20894.140625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[50] = {
instance = "ItPl_Mana_Herb_02",
amount = 3,
x = 4759.041015625,
y = 409.20581054688,
z = -22164.25390625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[51] = {
instance = "ItPl_Health_Herb_02",
amount = 3,
x = 5815.8408203125,
y = 488.33389282227,
z = -23795.6875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[52] = {
instance = "ItPl_Health_Herb_02",
amount = 1,
x = 7021.6186523438,
y = 600.89526367188,
z = -10311.462890625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[53] = {
instance = "ItPl_Mana_Herb_01",
amount = 2,
x = 8448.9580078125,
y = 612.72790527344,
z = -11778.616210938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[54] = {
instance = "ItPl_Beet",
amount = 2,
x = 10572.45703125,
y = 1315.6457519531,
z = -13234.999023438,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[55] = {
instance = "ItPl_Beet",
amount = 2,
x = 9817.1005859375,
y = 1388.5705566406,
z = -13347.024414063,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[56] = {
instance = "ItPl_Beet",
amount = 2,
x = 9892.3955078125,
y = 1417.0988769531,
z = -13957.49609375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[57] = {
instance = "ItPl_Beet",
amount = 2,
x = 11010.2578125,
y = 1503.8616943359,
z = -14458.685546875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[58] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 42690.8477,
y = 2867.42236,
z = -23866.457,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[59] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 43234.6758,
y = 2879.97314,
z = -24743.0879,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[60] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 44585.5938,
y = 2881.2915,
z = -24712.5605,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[61] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 45030.8594,
y = 2954.95117,
z = -26289.7285,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[62] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 45651.4805,
y = 2730.54736,
z = -28861.6074,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[63] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 43715.4063,
y = 2904.49194,
z = -27265.7246,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[64] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 39787.1836,
y = 2884.93408,
z = -26259.0859,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[65] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 39221.0664,
y = 2894.97119,
z = -25929.9004,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[66] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 38878.6133,
y = 2922.7771,
z = -25416.5977,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[67] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 38522.6992,
y = 2740.71924,
z = -23105.3848,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[68] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 39246.4023,
y = 2732.63525,
z = -21173.8984,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[69] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 38070.9531,
y = 2757.48242,
z = -19603.9746,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[70] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 38354.4531,
y = 2726.47778,
z = -18149.8301,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[71] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 50631.4336,
y = 2194.5813,
z = -30167.6543,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[72] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 48387.918,
y = 2315.6272,
z = -30943.2734,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[73] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 46652,
y = 2449.74536,
z = -31143.332,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[74] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 46362.2383,
y = 2480.10156,
z = -32834.8398,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[75] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 45132.0742,
y = 2505.19409,
z = -32345.5957,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[76] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 70641.4453,
y = 2495.63257,
z = 1810.93652,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[77] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 74762.9453,
y = 3274.66772,
z = -929.971985,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[78] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 73794.2734,
y = 3300.43018,
z = -791.202942,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[79] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 73939.5313,
y = 3299.18481,
z = -1574.83545,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[80] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 73890.1875,
y = 3257.32544,
z = -1901.92566,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[81] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 75184.8203,
y = 3252.67578,
z = -806.122375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[82] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 74761.3672,
y = 3245.0498,
z = -371.74408,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[83] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 81064.8047,
y = 3779.5647,
z = -5095.16797,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[84] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 82504.1875,
y = 4127.75879,
z = -7177.62256,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[85] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 82056.2266,
y = 4167.6709,
z = -8807.74902,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[86] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 85096.2031,
y = 4876.66113,
z = -9536.33887,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[87] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 84642.5078,
y = 4923.2417,
z = -11650.1787,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[88] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 83649.0938,
y = 4495.7627,
z = -13724.7461,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[89] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 81590.7031,
y = 4237.8833,
z = -17557.709,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[90] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 79596.25,
y = 4097.8042,
z = -19106.9902,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[91] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 82340.4219,
y = 4143.87891,
z = -19610.2246,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[92] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65411.8555,
y = 2436.1438,
z = -18331.1855,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[93] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65332.8594,
y = 2437.80786,
z = -18380.625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[94] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65402.0508,
y = 2439.2063,
z = -18210.7285,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[95] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65607.2109,
y = 2434.21582,
z = -18100.4785,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[96] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65627.2109,
y = 2441.44775,
z = -17724.3105,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[97] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 56977.9688,
y = 1771.14026,
z = -16245.0684,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[98] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57008.0898,
y = 1777.6488,
z = -16080.7539,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[99] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57075.8633,
y = 1772.50549,
z = -15979.2969,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[100] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57897.9648,
y = 1779.83582,
z = -17228.709,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[101] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 58028.543,
y = 1789.34399,
z = -17076.5703,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[102] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52584.8398,
y = 3177.26099,
z = -25278.1895,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[103] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52633.6953,
y = 3173.10156,
z = -24965.3594,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[104] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52547.1445,
y = 3174.87793,
z = -25156.0352,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[105] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52691.7539,
y = 3175.71729,
z = -25085.0938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[106] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52443.8672,
y = 3094.0459,
z = -21851.1895,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[107] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52296.5664,
y = 3069.72778,
z = -21604.0938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[108] = {
instance = "ItPl_Beet",
amount = 5,
x = 13792.256835938,
y = 1805.9820556641,
z = -17681.1796875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[109] = {
instance = "ItPl_Beet",
amount = 5,
x = 14288.887695313,
y = 1860.7866210938,
z = -17366.791015625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[110] = {
instance = "ItPl_Temp_Herb",
amount = 5,
x = 12536.279296875,
y = 1526.3616943359,
z = -11586.03515625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[111] = {
instance = "farm",
amount = 10,
x = 11955.759765625,
y = 1535.3176269531,
z = -11192.149414063,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[112] = {
instance = "farm",
amount = 10,
x = 11021.176757813,
y = 1449.4575195313,
z = -11363.5078125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[113] = {
instance = "farm",
amount = 10,
x = 11375.837890625,
y = 1467.6179199219,
z = -11863.006835938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[114] = {
instance = "ItPl_HEALTH_HERB_01",
amount = 1,
x = 7713.435546875,
y = 1243.3634033203,
z = -16704.837890625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[115] = {
instance = "ItFo_Apple",
amount = 10,
x = 7741.0581054688,
y = 1399.0826416016,
z = -18883.63671875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[116] = {
instance = "ItPl_Forestberry",
amount = 3,
x = 10684.8515625,
y = 356.37948608398,
z = 7927.734375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[117] = {
instance = "ItPl_Forestberry",
amount = 3,
x = 8629.3623046875,
y = 614.63177490234,
z = 9365.5185546875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[118] = {
instance = "ItPl_Mana_Herb_01",
amount = 2,
x = 7505.0649414063,
y = 340.54095458984,
z = 10961.302734375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[119] = {
instance = "ItPl_Mushroom_01",
amount = 3,
x = 6146.9047851563,
y = 212.31948852539,
z = 10901.266601563,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[120] = {
instance = "ItPl_Mushroom_01",
amount = 3,
x = 9615.39453125,
y = 397.37557983398,
z = 11466.310546875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[121] = {
instance = "ItPl_Forestberry",
amount = 3,
x = 4823.1245117188,
y = 2235.2473144531,
z = 13602.202148438,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[122] = {
instance = "ItPl_Health_Herb_02",
amount = 2,
x = 5548.2934570313,
y = 2677.7578125,
z = 15242.78125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[123] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 8060.1533203125,
y = 3623.1916503906,
z = 16412.751953125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[124] = {
instance = "ItFo_Apple",
amount = 2,
x = 9014.7763671875,
y = 4034.4973144531,
z = 17019.85546875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[125] = {
instance = "ItPl_Health_Herb_02",
amount = 2,
x = 10813.43359375,
y = 3826.4877929688,
z = 16577.51171875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[126] = {
instance = "ItPl_Forestberry",
amount = 2,
x = 12138.278320313,
y = 3232.1020507813,
z = 15835.783203125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[127] = {
instance = "ItPl_Forestberry",
amount = 3,
x = 10230.639648438,
y = 4195.0512695313,
z = 14761.626953125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[128] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 13211.997070313,
y = 4268.9731445313,
z = 12137.021484375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[129] = {
instance = "ItPl_Planeberry",
amount = 2,
x = 14697.764648438,
y = 4133.48828125,
z = 8386.6689453125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[130] = {
instance = "ItPl_Forestberry",
amount = 2,
x = 16967.8515625,
y = 5186.9057617188,
z = 8775.046875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[131] = {
instance = "ItPl_Temp_Herb",
amount = 20,
x = 28974.5625,
y = 3277.0825195313,
z = 7490,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[132] = {
instance = "ItPl_MUSHROOM_02",
amount = 1,
x = 66967.5391,
y = 4970.33594,
z = 26517.8125,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[133] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 68465.25,
y = 4643.85254,
z = 26734.8223,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[134] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 68968.8438,
y = 4590.44238,
z = 26195.1875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[135] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 68888.8906,
y = 4644.32959,
z = 26549.7715,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[136] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 68818.4141,
y = 4632.21777,
z = 26677.3633,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[137] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 68660.2891,
y = 4602.87402,
z = 26354.4414,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[138] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 68796.8906,
y = 4597.28564,
z = 26230.707,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[139] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 69760.3516,
y = 4422.53809,
z = 25164.0957,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[140] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 69788.9844,
y = 4401.57178,
z = 25109.4453,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[141] = {
instance = "ItPl_MUSHROOM_02",
amount = 1,
x = 69823.5938,
y = 4414.06445,
z = 25128.1621,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[142] = {
instance = "ItPl_PLANEBERRY",
amount = 1,
x = 71688.7266,
y = 4518.07715,
z = 22728.9883,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[143] = {
instance = "ItPl_PLANEBERRY",
amount = 1,
x = 71626.3438,
y = 4510.76855,
z = 22760.3145,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[144] = {
instance = "ItPl_PLANEBERRY",
amount = 1,
x = 71533.8203,
y = 4478.7959,
z = 22842.4961,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[145] = {
instance = "ItPl_PLANEBERRY",
amount = 1,
x = 71435.875,
y = 4466.00488,
z = 22930.5645,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[146] = {
instance = "ItPl_PLANEBERRY",
amount = 1,
x = 71828.4766,
y = 4494.32422,
z = 22647.0879,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[147] = {
instance = "ItPl_PLANEBERRY",
amount = 1,
x = 71724.9297,
y = 4505.28711,
z = 22668.8242,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[148] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 71928.6484,
y = 4514.33789,
z = 22222.1602,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[149] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 70649.0078,
y = 4328.06445,
z = 22697.5488,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[150] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 70884.4063,
y = 4362.97803,
z = 22910.5469,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[151] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 70544.0469,
y = 4334.99512,
z = 22935.8906,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[152] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 71119.8984,
y = 4530.90283,
z = 20779.2461,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[153] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 71800.4063,
y = 4574.07617,
z = 20914.4707,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[154] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 72066.7422,
y = 4580.0083,
z = 21355.9102,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[155] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 71704.0781,
y = 4526.3125,
z = 21109.3672,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[156] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 72011.0469,
y = 4605.23047,
z = 20778.0586,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[157] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 72421.9141,
y = 4797.33643,
z = 18620.5176,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[158] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 73663.4063,
y = 4761.76465,
z = 19526.1445,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[159] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 73339.6953,
y = 4895.75195,
z = 18471.8496,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[160] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72641.875,
y = 4757.87402,
z = 19034.498,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[161] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 73221.3438,
y = 4714.18311,
z = 17915.0742,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[162] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 73110.2734,
y = 4698.30078,
z = 17807.2441,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[163] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 73232.2031,
y = 4686.21045,
z = 17669.5996,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[164] = {
instance = "ItPl_MANA_HERB_03",
amount = 1,
x = 72840.0859,
y = 4662.52734,
z = 17427.916,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[165] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 71865.2422,
y = 4437.82617,
z = 16434.6055,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[166] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72131.5781,
y = 4470.00195,
z = 16591.0371,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[167] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72719.8438,
y = 4353.20313,
z = 15928.3252,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[168] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 71190.5781,
y = 4299.49756,
z = 15048.9375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[169] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 71706.5156,
y = 4286.79785,
z = 15285.0977,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[170] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 71270.4609,
y = 4463.08936,
z = 16416.0762,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[171] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 70915.3359,
y = 4456.63721,
z = 16437.4199,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[172] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 68580.1406,
y = 4335.08936,
z = 13704.9414,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[173] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 68298.2813,
y = 4348.70215,
z = 13834.6465,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[174] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 68332.9922,
y = 4263.01318,
z = 14315.1299,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[175] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 68524.5156,
y = 4253.19971,
z = 14519.9502,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[176] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 68496.2734,
y = 4268.50977,
z = 15023.2607,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[177] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 58145.8516,
y = 7298.31201,
z = 37910.2773,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[178] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57430.1836,
y = 7325.76367,
z = 37692.0195,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[179] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57145.7578,
y = 7382.27246,
z = 38078.0586,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[180] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 53831.5391,
y = 6939.1626,
z = 29697.0332,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[181] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 54676.8711,
y = 6867.88525,
z = 30273.1426,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[182] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 56376.7656,
y = 6669.39209,
z = 30195.3145,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[183] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 56882.3359,
y = 6709.28662,
z = 30262.1016,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[184] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 58448.0977,
y = 6872.12793,
z = 29326.9121,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[185] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 57720.5,
y = 6900.03271,
z = 29119.918,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[186] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57675.7578,
y = 6788.98926,
z = 30014.2539,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[187] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 59699.8711,
y = 6836.93164,
z = 29346.627,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[188] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 58348.9531,
y = 6824.9248,
z = 29736.5996,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[189] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 59232.7266,
y = 6672.81738,
z = 30405.4727,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[190] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 63395.5,
y = 6411.26465,
z = 30823.0938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[191] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 64305.9531,
y = 6703.0083,
z = 32962.582,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[192] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 64504.3438,
y = 6605.45654,
z = 32602.5156,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[193] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 63981.5273,
y = 6753.65967,
z = 33585.8711,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[194] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 63822.5273,
y = 6602.77197,
z = 32913.6875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[195] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 63390.125,
y = 6522.78662,
z = 32071.6895,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[196] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 63584.2188,
y = 6502.04395,
z = 32007.9375,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[197] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 65383.7422,
y = 6392.48584,
z = 32105.6777,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[198] = {
instance = "ItPl_MANA_HERB_03",
amount = 1,
x = 65756.625,
y = 6690.30713,
z = 33310.9766,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[199] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 68689.6328,
y = 6615.55518,
z = 34535.75,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[200] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 70718.3672,
y = 5095.53125,
z = 28479.6563,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[201] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 71250.7656,
y = 5142.18359,
z = 28648.1445,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[202] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72333.9453,
y = 5159.15332,
z = 26853.1797,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[203] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72511.8516,
y = 5160.16553,
z = 25297.8281,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[204] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72217.7891,
y = 5174.46533,
z = 24968.2852,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[205] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 71514.5469,
y = 5122.19727,
z = 25312.9648,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[206] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 71360.1172,
y = 5075.66455,
z = 25656.7402,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[207] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72119.1563,
y = 5179.42041,
z = 25912.6875,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[208] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 72124.3281,
y = 5180.95508,
z = 26146.0625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[209] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 72164.3672,
y = 5186.69824,
z = 25472.25,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[210] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 72953.7813,
y = 5128.33936,
z = 25485.3926,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[211] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 75397.6406,
y = 4981.86816,
z = 24037.3926,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[212] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 75405.2266,
y = 4995.30225,
z = 23637.9824,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[213] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 75114.9766,
y = 5048.20752,
z = 23753.9863,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[214] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 75334.6016,
y = 5030.51807,
z = 22796.9824,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[215] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 75801.9531,
y = 4928.86377,
z = 21403.2871,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[216] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 75007.4609,
y = 5056.27588,
z = 21403.8555,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[217] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 74704.7266,
y = 5058.2041,
z = 22117.1563,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[218] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 74947.9766,
y = 5052.82129,
z = 23026.0898,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[219] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 74750.6797,
y = 5057.9292,
z = 22710.8652,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[220] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 79662.1563,
y = 4885.44775,
z = 24245.3145,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[221] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 80477.9531,
y = 4978.30469,
z = 24949.9824,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[222] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 80077.9531,
y = 4964.00586,
z = 25233.5352,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[223] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 79850.9766,
y = 5038.1709,
z = 26539.0117,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[224] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 77957.625,
y = 5063.49463,
z = 28185.8496,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[225] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 77573.2422,
y = 5072.41943,
z = 29703.6895,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[226] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 77934.2578,
y = 5144.98145,
z = 30545.918,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[227] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 77646.8594,
y = 5142.51904,
z = 30869.9492,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[228] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 73814.25,
y = 5191.37549,
z = 32493.3145,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[229] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 72684.8828,
y = 5104.77197,
z = 32084.6523,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[230] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 73205.5703,
y = 5196.33887,
z = 32939.4922,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[231] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 74061.1953,
y = 5153.60742,
z = 34373.5703,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[232] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 74900.0469,
y = 5188.04834,
z = 34357.7656,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[233] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 75657.6406,
y = 5288.05713,
z = 33365.7656,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[234] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 78981.1406,
y = 5227.43262,
z = 29806.7109,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[235] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 78219.2422,
y = 5142.33105,
z = 29837.7754,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[236] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 78284.8281,
y = 5086.34424,
z = 28598.752,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[237] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 78539.7109,
y = 5042.79883,
z = 27739.6191,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[238] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 79402.6094,
y = 5257.03467,
z = 28567.5293,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[239] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 79343.1406,
y = 5270.49365,
z = 30523.2422,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[240] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 78583.9531,
y = 5189.60352,
z = 29958.6016,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[241] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 77594.3047,
y = 5104.11035,
z = 30406.1816,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[242] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 74370.9063,
y = 5210.23535,
z = 32383.2129,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[243] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 72869.6172,
y = 5073.81934,
z = 31700.1641,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[244] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 78338,
y = 5132.74902,
z = 29478.4063,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[245] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 77926.7422,
y = 5086.1001,
z = 29323.7461,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[246] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 74271.4375,
y = 5001.56787,
z = 21583.3027,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[247] = {
instance = "ITMI_AQUAMARINE",
amount = 1,
x = 31837.7988,
y = 4294.65918,
z = -38561.3672,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[248] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65496.9063,
y = 2409.6792,
z = -6381.37061,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[249] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 42501.1016,
y = 2733.05811,
z = -19449.4727,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[250] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 40299.4609,
y = 2829.84131,
z = -20796.2207,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[251] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 41385.2773,
y = 2823.22778,
z = -22220.7188,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[252] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 42236.2969,
y = 2829.63013,
z = -22280.3965,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[253] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 42690.8477,
y = 2867.42236,
z = -23866.457,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[254] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 43234.6758,
y = 2879.97314,
z = -24743.0879,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[255] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 44585.5938,
y = 2881.2915,
z = -24712.5605,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[256] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 45030.8594,
y = 2954.95117,
z = -26289.7285,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[257] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 45651.4805,
y = 2730.54736,
z = -28861.6074,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[258] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 43715.4063,
y = 2904.49194,
z = -27265.7246,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[259] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 39787.1836,
y = 2884.93408,
z = -26259.0859,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[260] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 39221.0664,
y = 2894.97119,
z = -25929.9004,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[261] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 38878.6133,
y = 2922.7771,
z = -25416.5977,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[262] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 38522.6992,
y = 2740.71924,
z = -23105.3848,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[263] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 39246.4023,
y = 2732.63525,
z = -21173.8984,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[264] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 38070.9531,
y = 2757.48242,
z = -19603.9746,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[265] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 38354.4531,
y = 2726.47778,
z = -18149.8301,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[266] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 50631.4336,
y = 2194.5813,
z = -30167.6543,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[267] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 48387.918,
y = 2315.6272,
z = -30943.2734,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[268] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 46652,
y = 2449.74536,
z = -31143.332,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[269] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 46362.2383,
y = 2480.10156,
z = -32834.8398,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[270] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 45132.0742,
y = 2505.19409,
z = -32345.5957,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[271] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 61414.3945,
y = 4279.39453,
z = -34985.4961,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[272] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 84950.1641,
y = 4335.1748,
z = -9531.0332,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[273] = {
instance = "ItPl_PERM_HERB",
amount = 1,
x = 73428.7188,
y = 2740.44946,
z = -347.192596,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[274] = {
instance = "ItPl_PERM_HERB",
amount = 1,
x = 52763.0313,
y = 1610.72046,
z = 1453.41162,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[275] = {
instance = "ItPl_PERM_HERB",
amount = 1,
x = 66585.7109,
y = 1430.85828,
z = -31612.1719,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[276] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 74372.4297,
y = 3277.71265,
z = -6936.41895,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[277] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 74636.7969,
y = 3277.70654,
z = -5950.77881,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[278] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 74044.8438,
y = 3260.55542,
z = -2254.11694,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[279] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 75278.5234,
y = 3316.44775,
z = -1880.97266,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[280] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 81389.7188,
y = 4192.12695,
z = -19334.3086,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[281] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 66341.2578,
y = 1450.18164,
z = -29204.2188,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[282] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 67900.8281,
y = 1493.39648,
z = -30288.6328,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[283] = {
instance = "ItPl_TEMP_HERB",
amount = 1,
x = 82482.1875,
y = 4457.93408,
z = -13970.0303,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[284] = {
instance = "ItPl_MANA_HERB_03",
amount = 1,
x = 82505.0234,
y = 4444.96045,
z = -13972.4531,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[285] = {
instance = "ItPl_MANA_HERB_03",
amount = 1,
x = 82328.3828,
y = 4412.85596,
z = -13836.9014,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[286] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 82252.5781,
y = 4144.33496,
z = -15492.1904,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[287] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 82256.0156,
y = 4161.20898,
z = -15344.1084,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[288] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 85016.2578,
y = 4368.9834,
z = -15447.0996,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[289] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 85033.875,
y = 4372.49121,
z = -15454.9932,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[290] = {
instance = "ItPl_MUSHROOM_02",
amount = 1,
x = 84614.1016,
y = 4959.73145,
z = -11243.4502,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[291] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 84639.5625,
y = 4953.32959,
z = -11266.0654,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[292] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 84592.5234,
y = 4947.67725,
z = -11306.7129,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[293] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 82548.1953,
y = 4379.33008,
z = -10524.7197,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[294] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 82566.2109,
y = 4385.50439,
z = -10552.0244,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[295] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 82430.4922,
y = 4158.24023,
z = -8129.62061,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[296] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 82382.2344,
y = 4154.88818,
z = -8204.93359,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[297] = {
instance = "ItPl_MUSHROOM_02",
amount = 1,
x = 81922.0234,
y = 3876.78735,
z = -5945.58398,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[298] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 81906.4219,
y = 3864.37354,
z = -5912.43066,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[299] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 81907.1328,
y = 3856.30396,
z = -5848.09229,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[300] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 78985.2031,
y = 3415.96533,
z = -2741.54102,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[301] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 79042.7891,
y = 3417.24097,
z = -2745.01099,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[302] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 79142.5625,
y = 3417.16113,
z = -2763.24268,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[303] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 78429.4063,
y = 3480.61304,
z = -3274.67114,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[304] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 78750.5234,
y = 3488.68945,
z = -3216.04004,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[305] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 78590.7813,
y = 3573.57593,
z = -3990.20117,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[306] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 76821.1328,
y = 3503.4353,
z = -4311.70752,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[307] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 76669.0469,
y = 3509.83691,
z = -4500.24512,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[308] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 76968.0391,
y = 3546.84668,
z = -4584.19336,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[309] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 76988.4219,
y = 3525.54004,
z = -4339.85986,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[310] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 77131.7734,
y = 3562.73975,
z = -4611.14648,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[311] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 76478.6875,
y = 3511.19312,
z = -4822.27588,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[312] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 76438.4063,
y = 3497.83203,
z = -4717.9834,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[313] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 78833.0859,
y = 3381.27759,
z = -2567.59253,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[314] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 78986.0859,
y = 3360.5481,
z = -2462.89844,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[315] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 80720.8594,
y = 3593.24097,
z = -3636.24463,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[316] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 80907.2578,
y = 3615.12769,
z = -3664.66406,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[317] = {
instance = "ItPl_MANA_HERB_03",
amount = 1,
x = 80825.2188,
y = 3604.02905,
z = -3598.92188,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[318] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 51022.75,
y = 1207.44885,
z = 5687.27539,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[319] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 49749.5859,
y = 1125.28674,
z = 6396.21387,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[320] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 53086.3008,
y = 999.826965,
z = 7609.29053,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[321] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 53089.8789,
y = 957.297729,
z = 7822.25195,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[322] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 54765.5352,
y = 1284.90552,
z = 7112.29688,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[323] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 54763.1406,
y = 1325.53271,
z = 6947.54199,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[324] = {
instance = "ItPl_MANA_HERB_01",
amount = 1,
x = 54866.1797,
y = 1267.44519,
z = 7201.28857,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[325] = {
instance = "ItPl_MUSHROOM_02",
amount = 1,
x = 56086.0273,
y = 2311.5459,
z = 9931.44531,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[326] = {
instance = "ItPl_MUSHROOM_02",
amount = 1,
x = 56031.3516,
y = 2296.41968,
z = 9801.66113,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[327] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 56035.9922,
y = 2288.84375,
z = 9829.19922,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[328] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 56094.5508,
y = 2315.63452,
z = 9975.7793,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[329] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 56071.3555,
y = 2300.63452,
z = 9891.80273,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[330] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 56038.168,
y = 2299.14917,
z = 9857.02734,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[331] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 59454.6328,
y = 2271.97339,
z = 8874.4043,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[332] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 59552.3984,
y = 2293.28735,
z = 8928.28516,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[333] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 59567.0117,
y = 2240.73242,
z = 8708.11816,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[334] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 59989.7695,
y = 2220.39575,
z = 8731.77832,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[335] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 59801.6211,
y = 2303.27222,
z = 9038.50098,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[336] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 60012.4492,
y = 2281.30566,
z = 9007.79688,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[337] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 60172.1563,
y = 1611.38074,
z = 9236.26855,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[338] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 60233.125,
y = 1632.18054,
z = 9220.18262,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[339] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 59062.4219,
y = 1612.5741,
z = 9339.93066,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[340] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 59014.0078,
y = 1620.84814,
z = 9353.44922,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[341] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 50462.8594,
y = 1410.43567,
z = -8032.22607,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[342] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 50209.0273,
y = 1394.89661,
z = -7921.54395,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[343] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 50243.9492,
y = 1405.92126,
z = -7895.1543,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[344] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 48557.9414,
y = 1403.70508,
z = -6904.03516,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[345] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 48678.8008,
y = 1414.86035,
z = -6922.1709,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[346] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 48326.4492,
y = 1508.71802,
z = -10668.4531,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[347] = {
instance = "ItPl_MANA_HERB_03",
amount = 1,
x = 48378.4609,
y = 1504.48865,
z = -10608.9131,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[348] = {
instance = "ItPl_MANA_HERB_03",
amount = 1,
x = 48089.0508,
y = 1531.2063,
z = -10846.3086,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[349] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 48082.7461,
y = 1576.69373,
z = -10549.2939,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[350] = {
instance = "ItPl_MANA_HERB_02",
amount = 1,
x = 48205.0703,
y = 1583.16382,
z = -10236.3281,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[351] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 48223.7656,
y = 1568.79529,
z = -10385.5488,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[352] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 48062.3711,
y = 1600.09607,
z = -10409.2725,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[353] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 47690.7305,
y = 1601.62012,
z = -10473.957,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[354] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 47749.8828,
y = 1565.73389,
z = -10701.9873,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[355] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 47826.9531,
y = 1586.40613,
z = -10554.0273,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[356] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 47691.1602,
y = 1622.2771,
z = -10288.1348,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[357] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 47440.6445,
y = 1553.74768,
z = -11099.749,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[358] = {
instance = "ItPl_BLUEPLANT",
amount = 1,
x = 47394.3125,
y = 1563.79346,
z = -10865.4873,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[359] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 51279.8398,
y = 1508.59827,
z = -12798.7549,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[360] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 51506.4727,
y = 1510.26135,
z = -12858.0127,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[361] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 51489.7422,
y = 1757.94995,
z = -6361.07959,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[362] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 85096.2031,
y = 4876.66113,
z = -9536.33887,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[363] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 84642.5078,
y = 4923.2417,
z = -11650.1787,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[364] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 83649.0938,
y = 4495.7627,
z = -13724.7461,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[365] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 81590.7031,
y = 4237.8833,
z = -17557.709,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[366] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 79596.25,
y = 4097.8042,
z = -19106.9902,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[367] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 82340.4219,
y = 4143.87891,
z = -19610.2246,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[368] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65411.8555,
y = 2436.1438,
z = -18331.185,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[369] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65332.8594,
y = 2437.80786,
z = -18380.625,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[370] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65402.0508,
y = 2439.2063,
z = -18210.7285,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[371] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65607.2109,
y = 2434.21582,
z = -18100.4785,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[372] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 65627.2109,
y = 2441.44775,
z = -17724.3105,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[373] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 56977.9688,
y = 1771.14026,
z = -16245.0684,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[374] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57008.0898,
y = 1777.6488,
z = -16080.7539,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[375] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57075.8633,
y = 1772.50549,
z = -15979.2969,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[376] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 57897.9648,
y = 1779.83582,
z = -17228.709,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[377] = {
instance = "ItPl_HEALTH_HERB_02",
amount = 1,
x = 58028.543,
y = 1789.34399,
z = -17076.5703,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[378] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52584.8398,
y = 3177.26099,
z = -25278.1895,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[379] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52633.6953,
y = 3173.10156,
z = -24965.3594,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[380] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52547.1445,
y = 3174.87793,
z = -25156.0352,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[381] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52691.7539,
y = 3175.71729,
z = -25085.0938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[382] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52443.8672,
y = 3094.0459,
z = -21851.1895,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[383] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52296.5664,
y = 3069.72778,
z = -21604.0938,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[384] = {
instance = "ItPl_MUSHROOM_01",
amount = 1,
x = 52294.5703,
y = 3082.73022,
z = -21852.043,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[385] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 47710.5664,
y = 1638.84644,
z = -10151.0918,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[386] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 47860.2656,
y = 1665.68213,
z = -10286.3662,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[387] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 47565.293,
y = 1620.29724,
z = -10629.5713,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[388] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 47907.3711,
y = 1594.19397,
z = -10718.5791,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
}
pflanze[389] = {
instance = "ItPl_SWAMPHERB",
amount = 1,
x = 47270.4727,
y = 1634.85315,
z = -10330.292,
world = "NEWWORLD\\NEWWORLD.ZEN",
itemID = 0,
spawned = 0,
} 

end