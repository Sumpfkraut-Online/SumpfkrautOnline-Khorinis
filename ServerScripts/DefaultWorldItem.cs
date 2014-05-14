using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.Types;

namespace GUC.Server.Scripts
{
	public class DefaultWorldItem
	{
		public static void Init()
		{
            Item mi = null;
            String mapName = @"NEWWORLD\NEWWORLD.ZEN";
            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(3240.494f, 304.1594f, 1766.756f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PITCH"), 1);
            mi.Spawn(mapName, new Vec3f(3190.828f, 247.0403f, -300.0144f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERRING"), 1);
            mi.Spawn(mapName, new Vec3f(3202.857f, 433.4729f, -2208.662f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITKE_LOCKPICK"), 1);
            mi.Spawn(mapName, new Vec3f(4172.163f, 289.4225f, -3825.492f), new Vec3f(0f, 0.190809f, 0.9816273f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(8073.321f, 565.9537f, -1187.651f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(2341.275f, 386.2149f, -1458.567f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(1101.169f, 629.5848f, -1343.778f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(2207.36f, 202.6664f, 40.5239f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(2333.364f, 253.225f, 712.4056f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_ICEBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(1191.166f, 576.2249f, 1398.039f), new Vec3f(0f, -0.6946584f, 0.7193398f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(1765.233f, 652.5426f, 2721.971f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET100"), 1);
            mi.Spawn(mapName, new Vec3f(840.4073f, -63.2526f, 23274.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(15933.12f, 1113.029f, 265.7805f), new Vec3f(-0.05233596f, 0f, 0.9986296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(15885.59f, 1113.003f, 319.2883f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SCHWERT3"), 1);
            mi.Spawn(mapName, new Vec3f(786.5247f, 0.310257f, 23301.94f), new Vec3f(-0.9332727f, -0.138968f, 0.331193f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_PIRATENSAEBEL"), 1);
            mi.Spawn(mapName, new Vec3f(-4090.825f, -383.6251f, -17302.96f), new Vec3f(0.5503689f, -0.468381f, 0.6911677f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(2729.411f, 322.9723f, 3931.106f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-599.2374f, -227.8971f, 13107.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-2900.479f, -414.6711f, 12576.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-22408.51f, -502.9018f, 1529.743f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-22947.26f, -173.3748f, 5738.435f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-9088.867f, -209.0544f, -6536.367f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-2492.571f, -481.5284f, -21672.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-5212.564f, -221.5862f, -9589.351f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-1661.334f, -211.6672f, 12833.1f), new Vec3f(0.08030089f, 0.6557225f, 0.7507231f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-2015.087f, -224.6672f, 12946.78f), new Vec3f(-0.02380138f, 0.6815832f, 0.7313539f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-2091.134f, -226.06f, 12872.04f), new Vec3f(-0.02593544f, 0.7426934f, 0.6691313f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERRING"), 1);
            mi.Spawn(mapName, new Vec3f(-14121.16f, -219.3743f, -8967.174f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(-14141.27f, -220.4084f, -8628.552f), new Vec3f(-0.6687233f, -0.02335234f, 0.7431451f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(-13967.49f, -219.2036f, -8651.255f), new Vec3f(-0.8571675f, 1.16711E-10f, 0.5150378f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(-13639.24f, -217.3767f, -9191.938f), new Vec3f(0.5299193f, 0f, 0.8480481f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(-13573.48f, -217.0342f, -9216.984f), new Vec3f(-0.3420202f, 2.571875E-12f, 0.9396926f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(-13632.56f, -217.2484f, -9177.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(-13638.27f, -217.1852f, -9234.246f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(-14447.37f, -218.0319f, -8545.304f), new Vec3f(0.8480481f, 3.132457E-16f, 0.5299193f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_OLDCOIN"), 1);
            mi.Spawn(mapName, new Vec3f(-13940.38f, -219.5382f, -8939.339f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_OLDCOIN"), 1);
            mi.Spawn(mapName, new Vec3f(-13980.22f, -213.3842f, -8880.778f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SAW"), 1);
            mi.Spawn(mapName, new Vec3f(-13876.14f, -217.4076f, -9241.297f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_NUGGET"), 1);
            mi.Spawn(mapName, new Vec3f(-14343.93f, -217.3279f, -8611.876f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-3082.786f, -431.119f, 12253.76f), new Vec3f(0.9975641f, 0f, -0.06975637f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-3107.817f, -431.0333f, 12158.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-3330.197f, -431.314f, 12022.92f), new Vec3f(0.9876884f, 0f, -0.1564343f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-3347.381f, -430.9911f, 12081.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(-2990.972f, -408.1314f, 11544.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_APPLE"), 1);
            mi.Spawn(mapName, new Vec3f(-3050.66f, -407.0129f, 11565.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_MILK"), 1);
            mi.Spawn(mapName, new Vec3f(-3024.178f, -401.3774f, 11570.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(-3029.854f, -395.6256f, 11522.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(-2782.018f, -404.9893f, 11126.07f), new Vec3f(0.9455185f, 0f, 0.3255683f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JOINT"), 1);
            mi.Spawn(mapName, new Vec3f(-2831.603f, -403.7351f, 11175.73f), new Vec3f(0.9659257f, 0f, -0.2588193f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_HAMMER"), 1);
            mi.Spawn(mapName, new Vec3f(-2788.196f, -403.8587f, 11148.73f), new Vec3f(0.7193391f, 0f, -0.694659f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_APFELTABAK"), 1);
            mi.Spawn(mapName, new Vec3f(-2931.807f, -382.8617f, 11237.92f), new Vec3f(0.994522f, 0f, 0.1045283f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(-2836.635f, -402.706f, 11169.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(-3165.915f, -406.7204f, 11035.08f), new Vec3f(0.9986296f, 0f, 0.05233595f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(-3169.019f, -411.1769f, 11073.85f), new Vec3f(0.2063448f, 0.08492196f, -0.9747875f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-10051.52f, -693.6952f, -5612.026f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-9910.387f, -624.4357f, -5894.858f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(-8591.113f, -192.2819f, -6123.322f), new Vec3f(0.9876884f, 0f, -0.1564346f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(-8388.078f, -130.4699f, -6394.837f), new Vec3f(0.777146f, 0f, 0.6293204f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-8417.419f, -126.3995f, -6423.984f), new Vec3f(0.9923119f, 0.1226498f, 0.01652532f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(-8381.538f, -119.9501f, -6444.774f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(-5141.494f, -189.6926f, -10442.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(-6027.861f, -213.0389f, -11294.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(-5200.493f, -200.9251f, -10437.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(-3694.482f, -444.4894f, -18996.99f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(-4104.421f, -433.2831f, -17391.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-4962.68f, -455.628f, -16095.12f), new Vec3f(0f, 0.1218694f, 0.9925464f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-6055.975f, -602.7225f, -14733.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-6430.28f, -495.2158f, -13219.67f), new Vec3f(0f, 0.3583681f, 0.9335813f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-5682.31f, -199.8647f, -12549.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-5874.168f, 19.74625f, -11674.49f), new Vec3f(0.6653954f, 0.7430801f, 0.07128827f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-5801.021f, 19.74624f, -11741.42f), new Vec3f(0.3812581f, 0.7103241f, -0.5916779f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-4535.873f, -360.2975f, -18416.64f), new Vec3f(0.2617805f, 0.8867576f, 0.3809656f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-4525.792f, -360.2975f, -18422.42f), new Vec3f(-0.03232413f, 0.6105238f, -0.7913384f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-4487.789f, -362.1246f, -18519.64f), new Vec3f(-0.243506f, 0.6730089f, -0.6984012f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-4454.771f, -362.1246f, -18603.87f), new Vec3f(-0.3778255f, 0.7454098f, 0.5491942f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_FLASK"), 1);
            mi.Spawn(mapName, new Vec3f(12935.46f, 1032.748f, 3242.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_FLASK"), 1);
            mi.Spawn(mapName, new Vec3f(12981.76f, 1031.902f, 3421.716f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_FLASK"), 1);
            mi.Spawn(mapName, new Vec3f(12911.64f, 1029.756f, 3222.325f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SKELETONBONE"), 1);
            mi.Spawn(mapName, new Vec3f(11988.81f, 1063.492f, 3452.839f), new Vec3f(-0.5446403f, 0f, -0.8386705f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WING"), 1);
            mi.Spawn(mapName, new Vec3f(11974.21f, 1053.321f, 3463.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(10698.29f, 1006.918f, 3291.792f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_QUARTZ"), 1);
            mi.Spawn(mapName, new Vec3f(12018.98f, 1064.396f, 3542.135f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(12944.6f, 1027.046f, 3292.432f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(12061.68f, 1074.836f, 3694.585f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(10289.87f, 921.3533f, -6092.567f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(6088.552f, 822.4583f, 5995.948f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(5671.086f, 837.3821f, 6458.622f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(2966.951f, 943.1929f, 7182.474f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(2977.6f, 879.5915f, 7206.882f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(3094.056f, 937.5914f, 7254.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(3683.477f, 843.5652f, 8573.585f), new Vec3f(-0.5446392f, 0f, 0.8386707f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(2302.523f, -543.9363f, 5568.085f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(3110.283f, -526.1783f, 5750.938f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(8680.687f, 338.2043f, 4044.206f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_FLASK"), 1);
            mi.Spawn(mapName, new Vec3f(8697.672f, 395.0528f, 4048.392f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(8629.549f, 388.0877f, 4003.842f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(8738.392f, 461.9998f, 4055.946f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(8752.977f, 401.2858f, 4070.765f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(2932.374f, 62.34008f, -2373.649f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(2918.966f, 70.83339f, -2590.349f), new Vec3f(0.4573846f, 0.1335105f, 0.8791929f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_HAMMER"), 1);
            mi.Spawn(mapName, new Vec3f(7199.14f, 489.6893f, -1960.079f), new Vec3f(-0.9057418f, -0.05077888f, -0.4207835f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SAW"), 1);
            mi.Spawn(mapName, new Vec3f(7242.675f, 548.6893f, -1874.202f), new Vec3f(0.6957965f, 0.4942399f, 0.5211471f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_HAMMER"), 1);
            mi.Spawn(mapName, new Vec3f(3897.615f, -24.82128f, -6595.866f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SAW"), 1);
            mi.Spawn(mapName, new Vec3f(3916.902f, -25.55709f, -6637.061f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_SAUSAGE"), 1);
            mi.Spawn(mapName, new Vec3f(4205.221f, -92.56037f, 842.192f), new Vec3f(-0.9816283f, 0f, 0.1908071f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_SAUSAGE"), 1);
            mi.Spawn(mapName, new Vec3f(4297.732f, -93.56036f, 861.9346f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(4230.582f, -97.06777f, 860.2296f), new Vec3f(-0.6018153f, 0f, 0.7986354f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(4181.077f, -98.06782f, 872.6348f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER"), 1);
            mi.Spawn(mapName, new Vec3f(349.4725f, -62.75117f, -5857.131f), new Vec3f(-0.9026154f, 0.2288124f, 0.3645998f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(381.131f, -94.44437f, -5892.975f), new Vec3f(0.6820011f, 0f, -0.7313518f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(11128.74f, 992.1144f, 1013.615f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(11012.11f, 989.131f, 982.2335f), new Vec3f(-0.9659269f, 0f, 0.2588185f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(11064.26f, 987.6575f, 982.651f), new Vec3f(-0.1391736f, 0f, -0.9902686f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(11090.92f, 992.655f, 1008.461f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(11042.53f, 986.1776f, 1030.493f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(10940.3f, 1038.176f, 914.7116f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PAN"), 1);
            mi.Spawn(mapName, new Vec3f(10930.21f, 1010.249f, 973.5448f), new Vec3f(-0.8829484f, 0f, 0.469472f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_BROOM"), 1);
            mi.Spawn(mapName, new Vec3f(10498.84f, 1035.829f, 824.7298f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_BROOM"), 1);
            mi.Spawn(mapName, new Vec3f(10547.58f, 1027.607f, 804.6178f), new Vec3f(-0.09137975f, 0.1941896f, 0.9767013f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(10789.99f, 1031.261f, 899.6743f), new Vec3f(-0.3420206f, 0f, -0.9396936f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(10760.06f, 1031.261f, 921.6073f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(14169.61f, 1637.894f, -352.5989f), new Vec3f(-0.4067369f, 0f, 0.9135456f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(14096.99f, 1638.632f, -383.199f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(15831.03f, 1671.598f, 192.3375f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(15814.97f, 1672.606f, 223.8687f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(15238.04f, 1641.659f, -792.6778f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(15178.22f, 1636.723f, -803.4851f), new Vec3f(0.4848095f, 0f, 0.8746195f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(5955.678f, 382.2648f, 2594.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(5857.568f, 382.2648f, 2808.735f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(5558.053f, 382.2646f, 2811.755f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(5629.262f, 382.2646f, 2843.035f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(5921.417f, 382.2646f, 2716.749f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(-357f, -581f, 5503f), new Vec3f(-0.6894199f, 0.5307466f, 0.4929854f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(-313.9694f, -636.2626f, 5518.246f), new Vec3f(0f, -0.9975659f, -0.06975643f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(292.3601f, -611.0684f, 5266.195f), new Vec3f(0.7659988f, 0.6051462f, 0.2169022f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(1412.856f, -544.4612f, 5825.221f), new Vec3f(0.5178365f, 0.1172188f, 0.8474147f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(1414.047f, -567.9214f, 5839.218f), new Vec3f(0.410741f, 0.2970585f, 0.862003f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(1449.056f, -571.7216f, 5965.153f), new Vec3f(0.9510574f, 0f, 0.3090174f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(1469.758f, -565.1108f, 5883.858f), new Vec3f(0.6669614f, 0.2694698f, 0.694659f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(1388.086f, -556.8089f, 6004.759f), new Vec3f(0.7006299f, 0.5090381f, 0.4999998f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(1419.117f, -593.6306f, 5868.755f), new Vec3f(0.8867927f, -0.2525603f, 0.3870575f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(1473.425f, -575.2698f, 6224.715f), new Vec3f(-0.5087933f, 0.8112854f, -0.2880119f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(1487.446f, -600.9629f, 6310.472f), new Vec3f(0.9067557f, -0.0483595f, 0.418882f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(1481.855f, -613.7503f, 6236.543f), new Vec3f(0.03949847f, 0.944957f, -0.3248065f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(1479.018f, -636.2606f, 6405.358f), new Vec3f(0f, -0.997568f, -0.06980023f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(1157.821f, -647.2827f, 4356.067f), new Vec3f(-0.1457617f, 0.02467966f, 0.9890128f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(1146.885f, -636.6415f, 4439.316f), new Vec3f(0.4736644f, -0.8713304f, 0.1281821f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(1135.743f, -506.3318f, 4372.811f), new Vec3f(0.3384456f, 0.1897639f, -0.9216536f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(1110.69f, -450.4614f, 4377.555f), new Vec3f(0.1703828f, 0.8019291f, -0.5726234f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PAN"), 1);
            mi.Spawn(mapName, new Vec3f(10136.23f, 1024.749f, -2994.452f), new Vec3f(-0.9975653f, 0f, -0.06976283f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_BROOM"), 1);
            mi.Spawn(mapName, new Vec3f(10127.28f, 1027.495f, -3075.573f), new Vec3f(-0.4986849f, -0.02410343f, 0.8664532f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(10084.44f, 1022.453f, -3024.825f), new Vec3f(-0.992548f, 0f, 0.1218672f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(10079.2f, 1019.67f, -2975.306f), new Vec3f(0.9439725f, -0.1218697f, -0.306731f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(10594.78f, 1393.206f, -2996.896f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(10480.29f, 989.014f, -2737.324f), new Vec3f(0.7547098f, 0f, -0.6560591f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(10495.34f, 997.4019f, -2679.977f), new Vec3f(0.9271849f, 0f, 0.3746051f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_MILK"), 1);
            mi.Spawn(mapName, new Vec3f(10544.35f, 997.3646f, -2696.969f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_MILK"), 1);
            mi.Spawn(mapName, new Vec3f(12045.27f, 1034.416f, -4938.844f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(12078.38f, 1146.454f, -4981.157f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(11988.48f, 1029.553f, -4929.165f), new Vec3f(-0.3583687f, 0f, -0.9335811f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(11918.99f, 1042.092f, -4816.189f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(11693.87f, 984.9526f, -5122.021f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_APPLE"), 1);
            mi.Spawn(mapName, new Vec3f(11704.71f, 988.8116f, -5162.328f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(11718.61f, 987.1375f, -5168.822f), new Vec3f(-0.4848102f, 0f, -0.8746204f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(11739.12f, 992.4724f, -5111.582f), new Vec3f(-0.05233613f, 0f, 0.9986316f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PAN"), 1);
            mi.Spawn(mapName, new Vec3f(11876.26f, 1008.487f, -4865.46f), new Vec3f(0.9975646f, -3.892458E-09f, 0.06975531f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PLIERS"), 1);
            mi.Spawn(mapName, new Vec3f(11672.95f, 1349.062f, -4537.322f), new Vec3f(-0.6560592f, 0f, -0.7547098f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(13766.17f, 1038.585f, -4249.637f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_STEW"), 1);
            mi.Spawn(mapName, new Vec3f(13870.58f, 989.9709f, -3861.026f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(13835.06f, 995.3654f, -3832.156f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(13907.18f, 994.4393f, -3863.694f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(13818.56f, 989.3916f, -3881.806f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(14258.92f, 1029.165f, -3923.55f), new Vec3f(0.5735767f, 0f, 0.8191525f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(14328.49f, 1036.068f, -3844.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(13823.67f, 996.8691f, -3810.597f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(14041.1f, 1036.956f, -3046.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(14070.73f, 1033.351f, -3079.783f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(14100.06f, 1033.732f, -3075.082f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(14115.88f, 1039.281f, -3084.291f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(2595.682f, 71.21632f, -6396.718f), new Vec3f(-0.6663988f, 0.7404906f, 0.08711455f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_VLK_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(4574.66f, -60.93961f, 768.3463f), new Vec3f(-0.5247632f, 0.851252f, -0.0004423326f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTON"), 1);
            mi.Spawn(mapName, new Vec3f(4314.87f, 68.59433f, 806.1456f), new Vec3f(-0.05022755f, -0.9986649f, 0.01205671f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTON"), 1);
            mi.Spawn(mapName, new Vec3f(4200.962f, 76.44348f, 808.8677f), new Vec3f(2.640292E-06f, -0.9993919f, 0.03490221f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(4372.521f, 69.44348f, 812.0269f), new Vec3f(0.06679273f, -0.9864644f, 0.1497967f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_RAKE"), 1);
            mi.Spawn(mapName, new Vec3f(3755.201f, -2.281075f, 1285.557f), new Vec3f(0.9961945f, 0f, 0.0871558f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SENSE"), 1);
            mi.Spawn(mapName, new Vec3f(3741.26f, -37.47619f, 1347.866f), new Vec3f(-0.03640632f, 0.846359f, 0.5313748f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_BAU_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(3750.368f, -28.47619f, 1276.493f), new Vec3f(2.380987E-06f, -0.9998574f, -0.01744576f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(4464.195f, 11.29667f, 954.3373f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(4391.305f, 3.296673f, 952.3439f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(4294.922f, 8.296673f, 972.7302f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(4059.967f, 11.22237f, -7017.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(4152.52f, -36.69206f, -6937.938f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(4098.959f, 77.37178f, -6975.367f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(4110.399f, 22.59706f, -6984.487f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(11561.77f, 1514.344f, -4519.657f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(12790.67f, 1393.4f, -4071.105f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(12473.16f, 1437.832f, -4465.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_HAMMER"), 1);
            mi.Spawn(mapName, new Vec3f(5702.444f, 836.8388f, 6394.347f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(6335.656f, 842.5054f, 8765.103f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(6319.393f, 840.5054f, 8782.364f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(6311.028f, 842.5054f, 8797.967f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(6134.352f, 867.4889f, 9769.328f), new Vec3f(0.9612628f, 0f, 0.2756377f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(6154.233f, 851.7011f, 9777.288f), new Vec3f(0.9271849f, 0f, 0.3746071f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(6291.259f, 845.5893f, 8863.297f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(4045.927f, 805.4918f, 8128.975f), new Vec3f(-0.9993914f, 0f, -0.03489947f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_MILK"), 1);
            mi.Spawn(mapName, new Vec3f(4052.484f, 812.3404f, 8105.843f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(12097.88f, 1080.89f, -4819.093f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(11905.78f, 1080.89f, -4638.432f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(10548.36f, 1038.342f, -2249.474f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(10840.59f, 1523.766f, -2971.159f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(10256.77f, 1426.2f, -3130.942f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(10406.92f, 1460.372f, -2296.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(11093.2f, 1521.358f, 1896.046f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(11106.52f, 1398.508f, 1077.695f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(14395.77f, 1002.49f, -4026.691f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(14411.55f, 989.7372f, -4064.532f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(14402.11f, 988.7375f, -3999.182f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(14410.72f, 989.7374f, -4089.042f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER"), 1);
            mi.Spawn(mapName, new Vec3f(14415.96f, 1019.285f, -3962.736f), new Vec3f(0.2901923f, -0.2366364f, 0.9272486f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_INNOSSTATUE"), 1);
            mi.Spawn(mapName, new Vec3f(15088.14f, 1443.417f, -3652.2f), new Vec3f(0.5735766f, 0f, 0.8191522f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_CHARM"), 1);
            mi.Spawn(mapName, new Vec3f(13462.92f, 1512.417f, -3621.029f), new Vec3f(-0.04711537f, 0.9983503f, 0.03291418f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHALICE"), 1);
            mi.Spawn(mapName, new Vec3f(14900.01f, 1715.39f, -1033.912f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(14395.45f, 1707.637f, -991.6245f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(14173.18f, 1632.411f, -355.6878f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(14598.44f, 1675.08f, -470.5503f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(14340.62f, 1631.58f, -853.6931f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(-19005.29f, -229.0993f, 4709.044f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(-18846.32f, -205.0051f, 4843.907f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(-19045.03f, -255.0788f, 3942.161f), new Vec3f(-0.05233596f, 0.9984787f, 0.01742858f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(-19275.83f, -332.2457f, 2219.633f), new Vec3f(0f, 0.9205055f, 0.39073f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(2545.027f, 870.1812f, 6257.541f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(5378.088f, 755.7636f, 9680.209f), new Vec3f(0f, 0.9563063f, -0.292372f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-20488f, -689.1588f, 12182.89f), new Vec3f(0.7103556f, -0.4194465f, 0.565209f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(-20559.47f, -690.8674f, 12211.82f), new Vec3f(-0.23248f, -0.2210068f, 0.9471587f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(-20754.14f, -437.2532f, 11887.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(-20450.2f, -530.414f, 11876.69f), new Vec3f(0.1243614f, -0.4967991f, 0.8589096f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(-20450.06f, -487.0086f, 11810.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_BLOODCUP_MIS"), 1);
            mi.Spawn(mapName, new Vec3f(14801.35f, 1676.125f, -654.8752f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_INNOSSTATUE"), 1);
            mi.Spawn(mapName, new Vec3f(15432.08f, 1145.366f, -62.87959f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(6585.055f, 815.9172f, 7762.511f), new Vec3f(0f, 0.9945232f, -0.1045285f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(15774.58f, 1812.216f, -16089.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(58.22722f, 305.7227f, -21896.48f), new Vec3f(0.8191532f, 0f, 0.573576f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_INSTANTFIREBALL"), 1);
            mi.Spawn(mapName, new Vec3f(-79.82645f, 290.4469f, -21822.3f), new Vec3f(-0.02655549f, -0.08964713f, 0.99562f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(-69.23918f, 291.1372f, -21888.43f), new Vec3f(-0.6946585f, 0f, 0.7193398f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(-46.33257f, 296.525f, -21885.42f), new Vec3f(-0.9945219f, 0f, 0.1045284f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(7.193104f, 308.0502f, -22000.58f), new Vec3f(0.9063079f, 0f, 0.4226183f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(-31.22302f, 293.2438f, -21867.63f), new Vec3f(-0.7547097f, 0f, 0.656059f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(12.41947f, 306.778f, -21944.51f), new Vec3f(0.009110256f, 0.1041307f, 0.9945219f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(-2797.596f, 443.2443f, -14250.07f), new Vec3f(0.01354553f, 0.2266656f, 0.9738786f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(-2760.104f, 432.0842f, -14496.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(-2843.752f, 420.0499f, -14283.35f), new Vec3f(-0.01062161f, -0.4853487f, 0.8742572f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(16265.25f, 1794.767f, -16650.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(16011.03f, 1795.132f, -16394.8f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(15468.84f, 1795.111f, -17035.7f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(15796.2f, 1795.126f, -16580.06f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(15636.12f, 1794.324f, -16873.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(-42.54524f, 31.50928f, -8171.268f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(123.538f, -26.23615f, -7914.571f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(1496.549f, 48.20777f, -8338.843f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(1019.588f, 75.30624f, -8631.694f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(1063.02f, 55.33476f, -8401.727f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(12526.26f, 889.2617f, -5373.628f), new Vec3f(-0.888836f, 0.4539906f, -0.06215351f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(12389.57f, 929.2617f, -5348.168f), new Vec3f(0.9622499f, -0.08715574f, -0.2578342f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(12489.87f, 935.2617f, -5411.46f), new Vec3f(-0.4383712f, 0f, 0.8987941f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(12477.34f, 907.8271f, -5106.389f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(12342.51f, 877.8058f, -5435.037f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(13531.89f, 498.2458f, -5772.791f), new Vec3f(0.6004937f, 0.7698784f, -0.2161032f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(12235.91f, 494.6322f, -6082.198f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(11010.86f, 407.8632f, -6871.386f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(8645.142f, 276.5229f, -7728.322f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(9792.565f, 209.2546f, -7137.341f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(11634.33f, 473.7664f, -6931.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(12554.27f, 480.9094f, -5993.734f), new Vec3f(0.8191519f, 1.025048E-15f, -0.5735766f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(10465.25f, 339.8438f, -6884.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(9570.199f, 202.6054f, -7247.093f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(7818.033f, 42.00284f, -6893.242f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMISWORDRAW"), 1);
            mi.Spawn(mapName, new Vec3f(-1578.916f, 567.2692f, -9332.402f), new Vec3f(-0.07267307f, -0.008245528f, 0.997322f));

            mi = new Item(ItemInstance.getItemInstance("ITMISWORDRAW"), 1);
            mi.Spawn(mapName, new Vec3f(-1503.195f, 654.7344f, -9303.895f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(-1581.489f, 558.0581f, -9141.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(-1520.801f, 559.0463f, -9090.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(-1629.218f, 559.0414f, -9061.536f), new Vec3f(0.358368f, 0f, 0.9335809f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(-1581.475f, 559.0876f, -8986.118f), new Vec3f(0.5220602f, 0.02373355f, 0.8525791f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(3376.381f, 570.744f, -18264.24f), new Vec3f(0.6215727f, -0.1564345f, -0.7675785f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(3341.486f, 598.5672f, -18464.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(4214.487f, 351.3712f, -14375.46f), new Vec3f(7.450581E-09f, 0.4694718f, 0.8829478f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(4153.444f, 380.2558f, -14338.89f), new Vec3f(0f, -0.3746067f, 0.9271839f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3272.996f, 256.7106f, -13420.28f), new Vec3f(0.7431449f, 0f, 0.6691309f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3379.152f, 268.1487f, -13583.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(5088.203f, 359.0768f, -9565.962f), new Vec3f(0.2273582f, 0.2629186f, 0.9376475f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(5007.492f, 343.7848f, -9515.896f), new Vec3f(-0.04389262f, -0.5446393f, 0.8375214f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(4888.729f, 318.6511f, -9590.968f), new Vec3f(0f, 0.48481f, 0.8746206f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(9417.311f, 685.2314f, -11206.99f), new Vec3f(0.8987941f, -0.4341049f, 0.06100952f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(9385.255f, 660.4512f, -10973.71f), new Vec3f(0f, -0.8746201f, 0.48481f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(9524.328f, 637.5381f, -10956.11f), new Vec3f(0.06960573f, 0.6018151f, 0.7955967f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(8076.284f, 1155.918f, -17708.65f), new Vec3f(0.4497072f, -0.201953f, 0.8700458f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(12624.62f, 1562.638f, -22115.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(12825.76f, 1556.162f, -22072.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(12644.26f, 1602.458f, -22053.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(12761.74f, 1614.851f, -22061.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_APPLE"), 1);
            mi.Spawn(mapName, new Vec3f(12651.38f, 1604.775f, -22082.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(12625.09f, 1601.611f, -22066.76f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(12763.52f, 1559.917f, -22105.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14500.28f, 1790.129f, -16949.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13893.02f, 1701.305f, -16559.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14168.29f, 1767.711f, -17055.25f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14065.94f, 1682.921f, -17914.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14107.14f, 1692.921f, -17785.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13971.34f, 1712.921f, -17787.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13634.57f, 1712.921f, -17409.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13465.33f, 1687.921f, -17220.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13355.92f, 1684.921f, -17314.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13470.25f, 1697.921f, -17463.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14059.05f, 1752.776f, -16670.43f), new Vec3f(-0.7660438f, 0f, 0.6427869f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14407.35f, 1779.776f, -17116.35f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13773.78f, 1756.753f, -16945.86f), new Vec3f(0.8660248f, 0f, 0.4999998f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14135.3f, 1766.753f, -16479.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14009.41f, 1762.753f, -16830.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14281.2f, 1758.753f, -16924.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14233.46f, 1785.753f, -17157.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13812.41f, 1753.04f, -17462.47f), new Vec3f(-0.9999998f, 0f, 2.980232E-08f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14001.19f, 1785.04f, -17350.09f), new Vec3f(0.9848076f, 0f, -0.1736481f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13756.65f, 1741.04f, -17219.3f), new Vec3f(0.9396925f, 0f, 0.3420202f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13826.21f, 1760.485f, -17089.29f), new Vec3f(0.7660443f, 0f, 0.6427876f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13963.08f, 1772.722f, -17133.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(14000.64f, 1785.871f, -17248.05f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13737.96f, 1749.199f, -17337.72f), new Vec3f(-0.8717589f, -0.07075997f, -0.4847976f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BEET"), 1);
            mi.Spawn(mapName, new Vec3f(13803.79f, 1768.813f, -17323.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(5732.002f, 483.8275f, -23065.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(7976.385f, 1291.982f, -22230.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(8182.234f, 1313.324f, -22341.21f), new Vec3f(0f, -0.08715574f, 0.9961948f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(8033.79f, 1300.459f, -22402.61f), new Vec3f(0f, -0.05233596f, 0.9986296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(10925.18f, 1613.294f, -19315.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(11033.54f, 1621.174f, -19213.87f), new Vec3f(0.05342405f, -0.4351039f, 0.8987947f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(10957.18f, 1654.174f, -19252.16f), new Vec3f(0.01723754f, -0.1564345f, 0.987538f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(14303.12f, 1678.596f, -9455.509f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(14241.46f, 1686.988f, -9722.25f), new Vec3f(-0.4445543f, 0.4021147f, 0.8004243f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(14266.03f, 1671.477f, -9821.864f), new Vec3f(0.3974178f, 0.7526355f, 0.5249772f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(14355.97f, 1708.978f, -9596.741f), new Vec3f(-0.4851489f, 0.1209613f, 0.8660261f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(4958.758f, 486.4901f, -23319.93f), new Vec3f(0.9044728f, 0.003547696f, -0.4265243f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(16865.66f, 2613.586f, -9678.104f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(16306.47f, 2681.853f, -8489.716f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(5204.753f, 447.6282f, -23792.82f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_LIGHTHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(7820.22f, 1135.588f, -17027.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(7815.83f, 1316.643f, -22875.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_AXE_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(18516.43f, 1986.041f, -14509.56f), new Vec3f(0f, -0.05233596f, 0.9986295f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(18512.89f, 1934.851f, -14575.52f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(-1530.372f, 671.85f, -9341.362f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(15267.34f, 1813.232f, -16647.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_RAKE"), 1);
            mi.Spawn(mapName, new Vec3f(8723.829f, 1228.972f, -16716.69f), new Vec3f(0.6634136f, 0.6350371f, 0.395739f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(10518.71f, 1391.948f, -15403.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_PYROKINESIS"), 1);
            mi.Spawn(mapName, new Vec3f(-1708.359f, 4305.266f, 15870.12f), new Vec3f(-0.8660253f, 0f, 0.5000001f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET100"), 1);
            mi.Spawn(mapName, new Vec3f(-1719.408f, 4311.818f, 15806.58f), new Vec3f(-0.01060409f, 0.9999436f, -0.0006396773f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(-1762.324f, 4316.752f, 15817.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(6365.534f, 45.35423f, 11219.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(7358.168f, 293.223f, 10189.52f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(8737.794f, 574.3113f, 9754.527f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(8944.955f, 447.8464f, 8999.838f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(9174.819f, 279.0514f, 8054.629f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(13241.16f, 135.8365f, 6518.004f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(12247.7f, 48.9125f, 8506.066f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(11644.75f, 195.4176f, 7778.234f), new Vec3f(-0.1105503f, 0.4144884f, 0.9033158f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(11642.55f, 194.9652f, 7799.949f), new Vec3f(0.6018151f, 0f, 0.7986356f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(10611.14f, 223.2009f, 9610.491f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(7028.245f, 283.7953f, 10156.52f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(7389.127f, 185.0947f, 11215.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(6838.683f, 4.18209f, 11635.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(6619.772f, 1.337942f, 11572.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(6137.999f, -92.545f, 11968.8f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(4945.199f, 450.9414f, 12861.36f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(3433.65f, 178.4017f, 12116.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(3072.336f, 123.1782f, 11587.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(4080.716f, 117.0134f, 10318.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3997.212f, 243.7497f, 9320.435f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3224.697f, 294.5163f, 9153.597f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(2046.426f, 252.5252f, 9666.519f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(1301.995f, 317.5259f, 9710.528f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(1159.394f, 243.6186f, 8992.067f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(2211.87f, 557.4109f, 8532.078f), new Vec3f(0.2756374f, 0f, -0.9612617f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(2249.9f, 564.7195f, 8466.038f), new Vec3f(-0.3417412f, 0.4848098f, -0.8050917f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(2258.715f, 573.6072f, 8460.039f), new Vec3f(-0.06975651f, 0f, -0.9975642f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(2186.094f, 569.2874f, 8473.454f), new Vec3f(0.0837794f, 0.2756374f, -0.9576037f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(2218.912f, 581.0922f, 8466.907f), new Vec3f(-0.4019346f, 0.309017f, -0.8619502f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(1792.841f, 235.5969f, 8850.937f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(1431.633f, 603.2943f, 7583.212f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(1699.508f, 676.7337f, 7217.645f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SHORTSWORD4"), 1);
            mi.Spawn(mapName, new Vec3f(32482.69f, 3545.061f, 472.7037f), new Vec3f(0.2325069f, 0.273468f, -0.9333567f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(28061.96f, -1646.832f, 2316.586f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(23307.55f, -1088.277f, -4851.899f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(-2338.732f, 2431.872f, 16162.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(-1279.147f, 2412.579f, 16827.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(2036.251f, 3386.713f, 18958.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(1972.328f, 3397.64f, 18799.84f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(5560.794f, 2595.298f, 15443.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(11004.09f, 541.4977f, 10931.66f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(35807.77f, 3805.66f, -6276.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(27831.69f, 3012.908f, 5554.825f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(27790.17f, 2622.468f, 9666.332f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(31657.56f, 3231.774f, 13515.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(7750.276f, 2979.419f, 13866.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(7692.69f, 2963.22f, 13892.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(7741.162f, 2959.064f, 13818.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(15586.53f, 5036.289f, 10040.62f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRI_PROT_MAGE_01"), 1);
            mi.Spawn(mapName, new Vec3f(29387.93f, 4426.729f, 25203.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29415.61f, 4425.918f, 25787.9f), new Vec3f(-0.8480487f, 0f, 0.5299197f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29253.99f, 4374.138f, 25108.33f), new Vec3f(-0.3420202f, 0f, 0.9396931f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29462.03f, 4352.104f, 24685.15f), new Vec3f(0.9335805f, 0f, 0.3583679f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29498.96f, 4355.148f, 24698.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29093.03f, 4396.247f, 25423.3f), new Vec3f(0.2923717f, 0f, 0.9563049f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29039.43f, 4391.383f, 25364.62f), new Vec3f(0.1218694f, 0f, 0.9925463f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29535.01f, 4381.895f, 25217.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29471.94f, 4374.333f, 25076.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(29771.32f, 4368.453f, 25038.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(29776.88f, 4372.394f, 25122.04f), new Vec3f(0.2419219f, 0f, 0.9702956f));

            mi = new Item(ItemInstance.getItemInstance("ITRI_MANA_02"), 1);
            mi.Spawn(mapName, new Vec3f(16747.51f, 5106.001f, 8729.516f), new Vec3f(0f, 0.2079117f, 0.9781479f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_HARMUNDEAD"), 1);
            mi.Spawn(mapName, new Vec3f(16626.41f, 5123.744f, 8682.133f), new Vec3f(-0.9749961f, 0.2221499f, 0.005737758f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_HARMUNDEAD"), 1);
            mi.Spawn(mapName, new Vec3f(16630.85f, 5128.32f, 8729.272f), new Vec3f(0.02560832f, 0.1004134f, 0.994617f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIRESTORM"), 1);
            mi.Spawn(mapName, new Vec3f(16736.2f, 5109.531f, 8618.063f), new Vec3f(-0.9796002f, 0.1708735f, 0.1057657f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_MEDIUMHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(16710f, 5110.332f, 8633.739f), new Vec3f(0.006378151f, 0.05194588f, 0.9986297f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET100"), 1);
            mi.Spawn(mapName, new Vec3f(16669.19f, 5113.498f, 8618.207f), new Vec3f(0.111397f, -0.9937202f, 0.01063685f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(9280.067f, 3664.979f, 14234.24f), new Vec3f(-0.9706621f, -0.2145323f, 0.1086626f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(9256.315f, 3651.685f, 14252.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(9235.366f, 3627.442f, 14367.81f), new Vec3f(0.3617015f, -0.8557624f, 0.3699248f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(9224.821f, 3621.062f, 14389.82f), new Vec3f(-0.81803f, -0.5735768f, -0.0428712f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(9236.349f, 3626.357f, 14376.62f), new Vec3f(-0.3521707f, 0.9354835f, 0.02914979f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(9235.326f, 3627.489f, 14360.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(9253.627f, 3634.316f, 14381.75f), new Vec3f(-0.8910067f, 0f, 0.4539904f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_CROSSBOW_L_02"), 1);
            mi.Spawn(mapName, new Vec3f(9303.05f, 3662.451f, 14350.93f), new Vec3f(-0.04198052f, 0.1251609f, 0.9912483f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18833.01f, 1823.245f, 4352.105f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18827.14f, 1822.076f, 4337.085f), new Vec3f(0f, 0.1045285f, 0.9945225f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18649.98f, 1840.825f, 3656.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18677.34f, 1842.298f, 3642.221f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(19277.83f, 1874.636f, 3552.928f), new Vec3f(-0.2574013f, 0.1045285f, 0.9606346f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(19262.84f, 1868.777f, 3586.77f), new Vec3f(0f, 0.0348995f, 0.9993908f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(19020.71f, 1833.305f, 4640.733f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(19125.61f, 1850.662f, 4548.218f), new Vec3f(0.04663169f, 0.02376003f, 0.9986298f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(19115.73f, 1849.735f, 4574.06f), new Vec3f(0f, 0.961262f, -0.2756374f));

            mi = new Item(ItemInstance.getItemInstance("ITRI_STR_01"), 1);
            mi.Spawn(mapName, new Vec3f(19050.26f, 1840.429f, 4551.808f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRI_PROT_EDGE_01"), 1);
            mi.Spawn(mapName, new Vec3f(19033.58f, 1838.532f, 4560.503f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SHORTSWORD1"), 1);
            mi.Spawn(mapName, new Vec3f(19045.57f, 1840.882f, 4527.539f), new Vec3f(-0.9743703f, 0f, 0.2249507f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(23434.84f, 1583.498f, 4676.901f), new Vec3f(0f, 0.1736482f, 0.984808f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(23431.96f, 1577.574f, 4575.879f), new Vec3f(0f, 0.1045285f, 0.994522f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(23472.3f, 1597.574f, 4674.679f), new Vec3f(-0.07062912f, 0.1414242f, 0.9874269f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(19515.82f, 1172.321f, 1674.203f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(19642.25f, 1156.787f, 1560.021f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(19464.84f, 1115.187f, 1301.812f), new Vec3f(0f, 0.1391731f, 0.9902683f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(19065.34f, 793.7786f, 4688.636f), new Vec3f(0f, 0.4694718f, 0.8829482f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(19012.51f, 797.3104f, 4716.181f), new Vec3f(0f, 0.974371f, -0.2249512f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(19077.82f, 796.4274f, 4730.382f), new Vec3f(-0.9871101f, 0.1218694f, 0.1037496f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_ICEBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(18953.1f, 795.5445f, 4677.268f), new Vec3f(0f, -0.01745242f, 0.9998482f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_MEDIUMHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(19043.89f, 794.3044f, 4729.024f), new Vec3f(0f, 0.01745241f, 0.9998477f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(18988.06f, 797.8501f, 4650.027f), new Vec3f(0f, -0.9925483f, -0.1218698f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_ORCAXE_01"), 1);
            mi.Spawn(mapName, new Vec3f(2883.933f, 144.0348f, 13711.95f), new Vec3f(0.1231208f, 0.9613433f, 0.2463012f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(33232.48f, -1949.704f, 2161.02f), new Vec3f(-0.8191527f, 0f, 0.5735757f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(33200f, -1956.603f, 2166.605f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(30637.8f, -1685.759f, 2984.558f), new Vec3f(0.164519f, 0.9014638f, 0.4003751f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_02"), 1);
            mi.Spawn(mapName, new Vec3f(30560.4f, -1676.929f, 2985.197f), new Vec3f(0f, 0.9659269f, 0.2588194f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(31417.19f, -1680.508f, 3053.807f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(31439.16f, -1683.508f, 3060.84f), new Vec3f(0f, 0.9271851f, 0.3746072f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(31413.89f, -1681.664f, 2995.573f), new Vec3f(-0.6072019f, 0.7617888f, 0.2258156f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(31505.12f, -1681.735f, 2931.852f), new Vec3f(0f, 0.9335818f, -0.3583684f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(33233.32f, 4134.485f, -12300.95f), new Vec3f(0.866026f, 0f, -0.5000002f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(33433.59f, 4118.971f, -12089.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(33414.2f, 4121.974f, -12025.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(33149.44f, 4126.126f, -12375.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(33288.28f, 4133.146f, -12311.61f), new Vec3f(-0.6946588f, 0f, 0.7193404f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(33239.91f, 4131.319f, -12337.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32880.36f, 4139.415f, -12306.66f), new Vec3f(-0.9743704f, 0f, 0.224951f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32777.04f, 4139.217f, -11420.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JEWELERYCHEST"), 1);
            mi.Spawn(mapName, new Vec3f(31214.4f, 4205.167f, -12513.17f), new Vec3f(0f, 0.06975648f, 0.9975641f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_PAL_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(31154.21f, 4237.778f, -12935.3f), new Vec3f(-0.2821507f, 0.3564056f, -0.8907123f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_PAL_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(31691.21f, 4232.278f, -12569f), new Vec3f(0.9993917f, 0f, 0.03489787f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_PAL_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(31163.23f, 4220.173f, -12423.99f), new Vec3f(0.9951363f, -0.06975645f, -0.06958658f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(24681.93f, 3027.433f, 28384.23f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(24402.68f, 3407.827f, 27079.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(23347.31f, 3173.619f, 27396.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(22907.34f, 2844.91f, 27968.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(21825.24f, 2553.168f, 28703.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(22859.38f, 2731.763f, 28830.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_02"), 1);
            mi.Spawn(mapName, new Vec3f(23794.6f, 2793.215f, 29151.35f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(24871.23f, 2911.269f, 29300.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(25455.93f, 3051.287f, 28640.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(27094.54f, 3230.665f, 28157.82f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(26162.65f, 3254.476f, 27965.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(24580.72f, 3349.507f, 27201.24f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(24317.6f, 3191.272f, 27528.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29360.74f, -1934.91f, -705.1841f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29255.36f, -2093.205f, 437.6694f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(25583.54f, -765.1143f, 28.94984f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(23629.52f, -545.143f, -1427.114f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20014.12f, -944.8087f, -3215.703f), new Vec3f(7.615241E-05f, -0.304151f, 0.9526243f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(21838.08f, -389.2147f, -834.3221f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(22644.12f, -348.1548f, 722.6165f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(22768.14f, -343.0896f, 828.3019f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26148.75f, -829.2261f, -835.8932f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26994.52f, -1103.688f, -1605.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(19968.36f, -867.3052f, -4189.101f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(21086f, -690.914f, -3633.722f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22775.42f, -750.7729f, -3042.802f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(4483.477f, 3892.102f, 24430.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(3150.909f, 3991.971f, 23302.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3129.288f, 3405.909f, 20174.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3647.458f, 3291.166f, 19889.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3666.02f, 3381.66f, 20606.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3196.308f, 3462.239f, 21470.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3881.042f, 4017.535f, 23373.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(4591.774f, 3927.768f, 23689.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(4793.879f, 3904.897f, 24105.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(4656.457f, 3907.094f, 24018.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(4344.09f, 3859.947f, 24491.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(3596.272f, 4043.458f, 23694.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(16833.37f, 4976.578f, 8036.143f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17872.67f, 4789.756f, 6416.771f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(19054.94f, 4797.955f, 6686.223f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18465.79f, 4929.861f, 7696.964f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18388.98f, 4954.869f, 7214.369f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18396.6f, 4885.496f, 6609.894f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(16803.5f, 5072.479f, 9613.363f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17061.77f, 5084.111f, 9211.634f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(16847.76f, 5101.987f, 8840.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(15905.58f, 5024.737f, 8562.627f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(15336.75f, 4987.735f, 9390.854f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(15372.07f, 5014.785f, 9828.126f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(26925.03f, 4403.985f, -3834.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(26898.68f, 4389.476f, -3820.992f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(25272.15f, 4424.229f, -5045.544f), new Vec3f(0.9603786f, 0.1137403f, -0.2544341f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(25344.65f, 4411.504f, -4537.365f), new Vec3f(-0.7547097f, 1.862645E-09f, -0.6560589f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_COAL"), 1);
            mi.Spawn(mapName, new Vec3f(24847.23f, 4426.4f, -5488.644f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_COAL"), 1);
            mi.Spawn(mapName, new Vec3f(26095.27f, 4496.093f, -5780.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_COAL"), 1);
            mi.Spawn(mapName, new Vec3f(26922.68f, 4401.785f, -3805.056f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PITCH"), 1);
            mi.Spawn(mapName, new Vec3f(26916.17f, 4410.499f, -3770.235f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PLIERS"), 1);
            mi.Spawn(mapName, new Vec3f(26931.02f, 4401.825f, -3814.942f), new Vec3f(0.579293f, 0.2458952f, 0.7771464f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_APFELTABAK"), 1);
            mi.Spawn(mapName, new Vec3f(26959.26f, 4423.598f, -3816.546f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(26923.95f, 4401.143f, -3767.898f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_FLASK"), 1);
            mi.Spawn(mapName, new Vec3f(26961.8f, 4414.084f, -3764.767f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(20775.76f, -27.96896f, -5603.514f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(20467.33f, -35.20006f, -5603.553f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(20707.11f, -71.26916f, -5772.939f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17984.5f, 485.0541f, -6011.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18210.96f, 396.8936f, -5775.383f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17717.03f, 484.3069f, -5970.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17603.1f, 480.3559f, -5867.128f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_HAMMER"), 1);
            mi.Spawn(mapName, new Vec3f(14939.23f, 463.8775f, -6197.474f), new Vec3f(-0.9907665f, 0.02489057f, 0.1332832f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MACE_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(15293f, 506.8727f, -5964.722f), new Vec3f(0.5783888f, 0.8125277f, 0.07257625f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERRING"), 1);
            mi.Spawn(mapName, new Vec3f(15302.54f, 456.8385f, -6066.331f), new Vec3f(0f, -0.9986308f, -0.0523359f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SAW"), 1);
            mi.Spawn(mapName, new Vec3f(15310.29f, 476.4045f, -6020.708f), new Vec3f(-0.6230041f, 0.6863036f, -0.3753125f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(996.4493f, 2880.683f, 18508.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(1726.615f, 2864.656f, 18878.5f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET100"), 1);
            mi.Spawn(mapName, new Vec3f(1229.358f, 2864.366f, 18696.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(31826.4f, 3119.007f, -718.4247f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_COMMON_01"), 1);
            mi.Spawn(mapName, new Vec3f(1028.479f, 2868.912f, 18459.5f), new Vec3f(0.5590835f, 0.01296967f, -0.8290114f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_02"), 1);
            mi.Spawn(mapName, new Vec3f(1440.14f, 2856.709f, 18698.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(1132.554f, 2865.204f, 18676.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(1737.796f, 2863.341f, 18925.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(-424.4222f, 2178.323f, 19222.28f), new Vec3f(0f, 0.9816291f, -0.1908093f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(-332.6882f, 2178.593f, 19144.54f), new Vec3f(-0.1635367f, 0.9625497f, -0.2162352f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(-63.85561f, 2744.175f, 21758.9f), new Vec3f(0.0348995f, 0.01744178f, 0.999239f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_POCKET"), 1);
            mi.Spawn(mapName, new Vec3f(67.63966f, 2819.546f, 21772.82f), new Vec3f(-0.9695703f, -0.09312043f, -0.226421f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(30701.25f, 3260.679f, 29527.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(29855.43f, 3154.578f, 29173.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(29802.26f, 3166.551f, 29180.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(29818.19f, 3161.917f, 29152.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(29734.84f, 3250.897f, 29485.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(29772.66f, 3153.779f, 29376.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(29746.37f, 3153.894f, 29348.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(29715.38f, 3153.875f, 29395.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(30283.33f, 3153.466f, 29735.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(30169.5f, 3153.612f, 29785.42f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(30159.97f, 3153.328f, 29826.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(30194.4f, 3153.47f, 29772.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(30117.75f, 3153.736f, 29764.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(36772.14f, 4117.107f, -2854.757f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(36744.67f, 4117.105f, -2840.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(36740.66f, 4117.124f, -2857.845f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(37063.04f, 4254.332f, -2803.177f), new Vec3f(0.0009134628f, 0.9998471f, 0.01747355f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(36532.38f, 4108.476f, -3173.766f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(31460.98f, 2698.966f, 20443.8f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_SUMGOBSKEL"), 1);
            mi.Spawn(mapName, new Vec3f(4849.792f, 3038.369f, 24720.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(4944.986f, 3043.5f, 24628.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(5001.246f, 2962.201f, 24721.1f), new Vec3f(0f, 2.070047E-08f, 0.9999998f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(30740.53f, 4283.53f, -6486.717f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITKE_LOCKPICK"), 1);
            mi.Spawn(mapName, new Vec3f(30629.3f, 4316.432f, -6131.988f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(30641.56f, 4300.4f, -6407.165f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(32263.06f, 3558.517f, 8527.743f), new Vec3f(-0.4226197f, 0f, 0.9063073f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(32216.83f, 3538.075f, 8991.88f), new Vec3f(-0.9205058f, 0f, 0.3907297f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(31889.64f, 4242.467f, -414.084f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(9733.093f, 207.5452f, 11012.95f), new Vec3f(0f, -0.03489948f, 0.999391f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(11596.08f, 205.8711f, 7824.017f), new Vec3f(0.9612616f, 0f, 0.2756379f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(13173.66f, 199.3458f, 5715.66f), new Vec3f(-3.629748E-08f, 0f, -0.9999998f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(12022.13f, 193.6441f, 6907.082f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(14221.36f, 4058.124f, 11022.36f), new Vec3f(-0.1096737f, 0.9541094f, 0.2786578f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(3438.352f, 263.4648f, 16159.6f), new Vec3f(-0.02801072f, -0.9851963f, -0.1691321f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(3062.877f, 186.0043f, 16250.73f), new Vec3f(0.6259502f, 0.7310898f, 0.2714749f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(3033.362f, 244.0799f, 16304.09f), new Vec3f(1.000063f, 2.949346E-05f, -0.0003396221f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(3135.32f, 194.6095f, 16488.51f), new Vec3f(0.5120955f, 0.8432811f, 0.163211f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(3216.479f, 246.9692f, 16533.84f), new Vec3f(0.8913404f, 0.4203042f, -0.1698833f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(3085.613f, 166.3423f, 16125.9f), new Vec3f(0.984809f, 0f, 0.1736461f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(2988.41f, 182.0185f, 16123.73f), new Vec3f(0.9369303f, -0.1499319f, 0.3157287f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(4026.484f, 157.7507f, 16201.32f), new Vec3f(0.06698738f, 0.2500002f, 0.965926f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(3299.725f, 257.4064f, 15301.1f), new Vec3f(0.07735923f, 0.3408578f, -0.9369292f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(2998.695f, 484.4909f, 13179.17f), new Vec3f(0.9510577f, -5.014064E-07f, 0.3090149f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(2897.352f, 473.955f, 13120.16f), new Vec3f(0.7313542f, 0f, 0.6819982f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(2531.987f, 97.44851f, 13570.99f), new Vec3f(0.9607961f, 0.202168f, -0.1897421f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(2346.511f, 142.3399f, 14056.89f), new Vec3f(0.8300865f, -0.4596122f, 0.315788f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(6799.106f, 213.135f, 10863.04f), new Vec3f(0.3247727f, 0.4743329f, 0.818254f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(6679.541f, 141.4514f, 11048.42f), new Vec3f(0.6080819f, 0.1564345f, -0.7783092f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(2259.146f, 139.9853f, 12818.93f), new Vec3f(0f, 0.05233595f, 0.9986296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(2097.191f, 139.521f, 12772.79f), new Vec3f(0.9335812f, 0f, 0.3583684f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(2166.872f, 137.0337f, 12745.82f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(7605.481f, 2246.134f, 19161.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(7587.653f, 2243.986f, 19227.84f), new Vec3f(0.8191518f, 0f, 0.5735765f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(7526.389f, 2245.168f, 19192.92f), new Vec3f(-0.9998479f, 0f, -0.01745242f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(1402.828f, 115.531f, 14669.5f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(874.5237f, 113.5341f, 15040.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(1009.942f, 114.5062f, 15098.11f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(629.7308f, 114.5341f, 14966.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SKELETONBONE"), 1);
            mi.Spawn(mapName, new Vec3f(4383.938f, 132.938f, 15117.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(16018.95f, 2879.318f, 11447.46f), new Vec3f(-0.4539903f, 0f, 0.8910066f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(15895.4f, 2912.128f, 11402.17f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(15387.67f, 2980.097f, 10962.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(15936.79f, 3065.954f, 10435.98f), new Vec3f(0.09118149f, -0.9957201f, 0.01509422f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(36723.02f, 3904.724f, -2164.521f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(38208.57f, 3925.081f, -2507.932f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(38047.54f, 3927.773f, -2543.243f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(37994.08f, 3924.115f, -2465.442f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(38172.34f, 3923.983f, -2540.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(37829.51f, 3920.126f, -1855.131f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(37829.6f, 3918.66f, -1896.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_SAUSAGE"), 1);
            mi.Spawn(mapName, new Vec3f(37861.04f, 3923.192f, -1857.484f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(36711.25f, 3871.628f, -2780.428f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(36660.25f, 3867.929f, -2783.266f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISHSOUP"), 1);
            mi.Spawn(mapName, new Vec3f(37155.3f, 4231.456f, -2450.753f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_FLASK"), 1);
            mi.Spawn(mapName, new Vec3f(37157.84f, 4239.839f, -2409.991f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_AQUAMARINE"), 1);
            mi.Spawn(mapName, new Vec3f(36446.91f, 4233.252f, -2230.752f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JOINT"), 1);
            mi.Spawn(mapName, new Vec3f(36459.49f, 4229.487f, -2184.563f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(36926.66f, 4226.773f, -1902.024f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(29005.63f, 4146.896f, 23964.53f), new Vec3f(0f, 0.3420202f, 0.9396929f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26637.38f, 3886.846f, 24619.97f), new Vec3f(-0.007557101f, 0.05184435f, 0.9986271f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29531.1f, 4432.42f, 25850.85f), new Vec3f(0.190809f, 0f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(14064.58f, 4256.916f, 20444.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(13336.08f, 4028.754f, 19315.08f), new Vec3f(-0.889977f, -0.07348365f, 0.4500504f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(8332f, 2252f, 19626f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(8615f, 2271f, 19061f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(8574f, 2289f, 19581f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(8174f, 2258f, 19021f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(8294.175f, 2283.726f, 18777.48f), new Vec3f(-0.9807852f, 0f, -0.1950885f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7278.751f, 2330.144f, 19730.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7586.346f, 2353.247f, 19863.36f), new Vec3f(0.9848077f, 0f, 0.1736484f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7375.19f, 2343.49f, 18641.2f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7074.442f, 2337.29f, 18819.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7792.869f, 2280.319f, 18930.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(8318.828f, 2262.952f, 19244.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7718.056f, 2275.065f, 19188.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7572.088f, 2267.081f, 19117.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(7706.841f, 2264.365f, 19623.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(18858.89f, 1848.474f, 14710.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(18746.62f, 1849.193f, 14780.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(18775.89f, 1861.424f, 14566.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(12704.57f, 2443.424f, 27766.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(12569.05f, 2496.461f, 26850.91f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(13463.96f, 2444.128f, 27562.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(14856.93f, 2315.621f, 29031.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(15658.31f, 2435.715f, 28132.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(17159.65f, 2230.789f, 28556.11f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(18858.5f, 2165.01f, 29281.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(24439.12f, 3137.155f, 27754.42f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(27109.6f, 3248.998f, 28152.62f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(28828.95f, 4197.386f, 27039.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(33614.5f, 4135.594f, 25864.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(28096.78f, 4210.459f, 24915.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(27967.07f, 4179.15f, 26414.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29653.16f, 4071.443f, 23559.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30307.99f, 4060.217f, 23610.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30173.71f, 4025.268f, 24114.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30456.57f, 4228.6f, 24785.02f), new Vec3f(0.100307f, 0.2798216f, 0.9547975f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31167.34f, 4119.054f, 26116.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22977.81f, 2737.857f, 28864.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(23102.88f, 2855.246f, 28062.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22547.66f, 2620.84f, 29698.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22624.69f, 2671.993f, 29052.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(23452.04f, 2738.756f, 29259.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22072.79f, 2671.722f, 28421.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22922.74f, 2763.274f, 28619.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22700.31f, 2708.927f, 28834.76f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26077.08f, 3978.414f, 25725.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26292.83f, 3983.599f, 25423.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31416.36f, 2596.126f, 17309.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31527.1f, 2583.938f, 17876.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31243.15f, 2618.708f, 16908.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30991.48f, 2573.115f, 17000.99f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29229.04f, 2462.007f, 18085.47f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29379.21f, 2511.8f, 18923.26f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30040.63f, 2568.839f, 18312.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(26871.52f, 2274.498f, 18025.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(27410.15f, 2294.764f, 18053.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(28181.42f, 2343.313f, 17935.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(26429.69f, 2286.94f, 17698.05f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(25148.34f, 2284.493f, 17178.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(24987.6f, 2287.093f, 17396.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(28160.38f, 2313.51f, 18081.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(28742.98f, 2411.808f, 18406.17f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(29534.22f, 2500.695f, 18730.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(24521.52f, 1941.621f, 9561.102f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(24659.48f, 2079.489f, 8829.045f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(25448.79f, 2035.84f, 10098.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(25826.66f, 2003.19f, 11397.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(26107.39f, 1910.69f, 12161.76f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(24125.92f, 1940.399f, 11283.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(24392.4f, 1917.725f, 11626.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(22511.32f, 2449.709f, 12883.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(22297.86f, 2465.526f, 13099.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20632.54f, 2573.433f, 14435.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20146.08f, 2574.085f, 15019.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17560.66f, 2474.463f, 15558.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(16709.38f, 2629.726f, 14901.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(13639.27f, 2651.391f, 14298.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(13316.49f, 2733.006f, 14503.25f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MACE_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(24546.08f, 3113.215f, 27723.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(25819.55f, 3394.438f, 27152.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(26156.7f, 3255.077f, 28083.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(24510.94f, 2856.67f, 29327.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(23851.78f, 2892.399f, 28503.15f), new Vec3f(0.8090171f, 1.490116E-08f, 0.5877852f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(22775.58f, 2727.205f, 28792.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(22843.1f, 2831.371f, 28038.16f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(23547.96f, 3188.628f, 27422.95f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(24770.96f, 2999.488f, 28585.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(25860.14f, 3201.876f, 27828.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(26423.05f, 3717.371f, 26798.35f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(26128.09f, 3591.642f, 26982.46f), new Vec3f(-0.9710869f, -0.2146244f, -0.1045286f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(24164f, 2821.108f, 29303.58f), new Vec3f(0.9975641f, -3.72529E-09f, -0.06975671f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_CROSSBOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(23000.39f, 2774.054f, 28612.48f), new Vec3f(0f, -0.8480483f, 0.5299189f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(22924.99f, 2798.427f, 28363.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(23123.74f, 2811.683f, 28429.66f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(22966.71f, 2764.794f, 28666.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(22927.9f, 2772.787f, 28567.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(33959.98f, 4188.261f, -9448.381f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(33944.89f, 4180.06f, -9282.677f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(32825.91f, 4176.819f, -8953.931f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(32370.1f, 4202.876f, -8453.469f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(33678.63f, 4095.88f, -8471.834f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(33461.16f, 4099.586f, -8432.998f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(33147.14f, 4202.279f, -9294.223f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(32668.01f, 4178.752f, -8952.886f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32385.43f, 3506.541f, 737.5062f), new Vec3f(0.2314251f, -0.5265653f, 0.8180304f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32369.2f, 3502.509f, 741.8848f), new Vec3f(0.2366032f, -0.5530008f, 0.7988821f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32382.47f, 3514.383f, 733.3618f), new Vec3f(0.06424828f, -0.4933951f, 0.86743f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32393.12f, 3505.054f, 727.7907f), new Vec3f(0.9902685f, 0.02655549f, -0.1366161f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32306.33f, 3422.976f, 555.4015f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32011.3f, 3355.07f, 565.8295f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32157.03f, 3391.337f, 557.1033f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32118.17f, 3394.825f, 652.6243f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(31927.39f, 3340.013f, 553.2049f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(31730.94f, 3308.467f, 551.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(31930.81f, 3340.698f, 761.2358f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32260.72f, 3416.743f, 583.6735f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32395.81f, 3431.963f, 471.6286f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(31529.19f, 3249.913f, 347.3486f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(31603.83f, 3290.791f, 670.9183f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(31955.8f, 3352.653f, 887.4411f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32038.25f, 3372.785f, 748.5381f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(32311.83f, 3441.044f, 694.564f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PAN"), 1);
            mi.Spawn(mapName, new Vec3f(32231.7f, 3418.841f, 585.1125f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JOINT"), 1);
            mi.Spawn(mapName, new Vec3f(32006.9f, 3372.268f, 946.9459f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_HAMMER"), 1);
            mi.Spawn(mapName, new Vec3f(32223.79f, 3433.627f, 796.3668f), new Vec3f(0.6752971f, 0.001723539f, -0.7375437f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PLIERS"), 1);
            mi.Spawn(mapName, new Vec3f(31683.6f, 3301.041f, 872.4578f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_STOMPER"), 1);
            mi.Spawn(mapName, new Vec3f(31917.05f, 3333.269f, 414.0737f), new Vec3f(0.3746063f, 0f, -0.927184f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(31446.5f, 3253.973f, 709.6029f), new Vec3f(0.7431446f, 0f, -0.6691307f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PITCH"), 1);
            mi.Spawn(mapName, new Vec3f(31778.67f, 3328.344f, 556.2446f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_QUARTZ"), 1);
            mi.Spawn(mapName, new Vec3f(31820.17f, 3330.231f, 707.343f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(31876.17f, 3390.398f, 786.4929f), new Vec3f(-0.9063078f, 0f, -0.4226184f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32046.22f, 3410.582f, 764.1424f), new Vec3f(-0.6782622f, -0.1045285f, -0.727347f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(31958.2f, 3370.211f, 785.9701f), new Vec3f(-0.9902681f, 0f, -0.1391732f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(31861.85f, 3403.853f, 980.1727f), new Vec3f(-0.9911861f, -0.05233596f, -0.1217024f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(31488.24f, 3289.014f, 1163.769f), new Vec3f(-0.8629544f, -0.08521675f, -0.4980438f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(31751.46f, 3393.445f, 710.2896f), new Vec3f(-0.9232409f, -0.1597118f, -0.3494527f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32256.63f, 3461.847f, 727.1454f), new Vec3f(-0.6551918f, -0.6937528f, -0.2990572f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32094.21f, 3435.641f, 923.7436f), new Vec3f(0.1339291f, -0.7237748f, 0.6769152f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(32195.8f, 3442.371f, 771.2104f), new Vec3f(0.1899901f, -0.8904535f, 0.4135191f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(37248.23f, 3885.014f, -2645.295f), new Vec3f(0.3907313f, 0f, 0.9205052f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(37284.94f, 3888.014f, -2728.228f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_STEW"), 1);
            mi.Spawn(mapName, new Vec3f(37229.95f, 3881.401f, -2721.188f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(37891.66f, 3927.087f, -1942.027f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(33819.72f, 4165.133f, -8992.688f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(34267.77f, 4129.599f, -9052.087f), new Vec3f(0.9335802f, 0f, -0.3583678f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(34609f, 4103.993f, -9601.022f), new Vec3f(-0.8910066f, 0f, 0.4539905f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(34156.45f, 4194.062f, -9692.49f), new Vec3f(0.9396924f, 0f, -0.34202f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(33536.1f, 4177.278f, -9230.667f), new Vec3f(0.9205047f, 0f, -0.3907312f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(33481.96f, 4151.669f, -8916.789f), new Vec3f(0.8480479f, 0f, -0.5299192f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(34050.39f, 4059.791f, -8168.434f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(33355.4f, 4136.605f, -8575.029f), new Vec3f(0.996197f, -0.06975648f, -0.05220857f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32685.46f, 4159.562f, -8304.666f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32352.29f, 4193.769f, -8842.684f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(18828.81f, 2513.478f, 16389.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(18698.56f, 2506.351f, 16498.42f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(19033.55f, 2477.897f, 15968.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18909.25f, 2520.557f, 16994.66f), new Vec3f(0f, 0.2249511f, 0.9743704f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20933.36f, 2156.501f, 12034.67f), new Vec3f(0.02741465f, 0.255726f, 0.9663607f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20901.92f, 2140.634f, 12081.91f), new Vec3f(-0.06701513f, -0.2698524f, 0.9605677f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(20767.57f, 2151.894f, 11954.77f), new Vec3f(-7.614121E-05f, -0.2538777f, 0.9672363f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20828.32f, 2195.434f, 11841.54f), new Vec3f(0f, -0.2079117f, 0.978148f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(20019.65f, 2140.444f, 12029.5f), new Vec3f(0f, -0.2588192f, 0.9659264f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(19863.11f, 2097.133f, 12291.91f), new Vec3f(0f, -0.241922f, 0.9702963f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(20818.36f, 1930.098f, 13365.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20785.03f, 1945.916f, 13339.05f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20874.65f, 1926.86f, 13346.37f), new Vec3f(0f, 0.190809f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(17227.62f, 2510.186f, 16069.9f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17200.97f, 2512.369f, 16090.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17227.07f, 2507.417f, 16068.26f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17198.86f, 2513.004f, 16083.36f), new Vec3f(0f, 0.06975648f, 0.9975641f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(17204.42f, 2513.855f, 16065.01f), new Vec3f(0f, 0.1045285f, 0.994522f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18129.68f, 2577.098f, 16576.41f), new Vec3f(0f, 0.2079117f, 0.9781476f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(18175.38f, 2577.165f, 16638.63f), new Vec3f(0.001217418f, 0.1736376f, 0.984809f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18270.95f, 2559.267f, 16643.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(29788.51f, 4407.059f, 25491.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29771.4f, 4421.782f, 25615.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29658.85f, 4349.908f, 24594.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29041.71f, 4143.859f, 23963.76f), new Vec3f(0f, 0.3090171f, 0.951057f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26708.69f, 3899.206f, 24763.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26139.54f, 2889.191f, 20878.85f), new Vec3f(0.06167677f, -0.9857991f, 0.1561975f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(26094.41f, 2903.436f, 20918.14f), new Vec3f(0f, -0.1736483f, 0.9848082f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(26204.6f, 2884.993f, 20849.23f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(25970.82f, 2830.25f, 20767.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(25383.26f, 2769.98f, 21275.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(25393.13f, 2776.519f, 21338.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(25411.51f, 2783.958f, 21369.72f), new Vec3f(-0.8987942f, 0f, 0.4383711f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(25411f, 2998.407f, 22141.42f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(25477.71f, 2971.239f, 22077.73f), new Vec3f(0.8480482f, 0f, 0.5299191f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(23361.71f, 2570.719f, 18264.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(23390.97f, 2577.593f, 18217.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(23338.33f, 2579.42f, 18082.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(22772.16f, 2408.61f, 16025.3f), new Vec3f(0.9819394f, -0.07696381f, -0.1728299f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(22812.93f, 2426.612f, 16084.19f), new Vec3f(0.8191522f, 0f, -0.5735761f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(22869.36f, 2438.936f, 16135.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(22747.83f, 2428.883f, 16079.08f), new Vec3f(0.6946583f, 0f, 0.7193398f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(22749.73f, 2385.657f, 15968.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(25620.12f, 3077.353f, 15791.17f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(25689.59f, 3066.701f, 15864.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(25289.29f, 2929.246f, 16033.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(21433.71f, 2536.368f, 14805.97f), new Vec3f(-0.9466581f, 0.2756374f, 0.1669213f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(21441.6f, 2536.368f, 14833.88f), new Vec3f(-0.9848076f, 0f, 0.1736482f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(21460.52f, 2523.097f, 14822.01f), new Vec3f(0f, 0.0348995f, 0.999391f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(21054.36f, 3194.619f, 9290.808f), new Vec3f(0.7971098f, -0.3968756f, -0.4550891f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(20960.4f, 3191.86f, 9408.575f), new Vec3f(0.3171155f, -0.3746066f, -0.871268f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(21025.97f, 3184.765f, 9316.286f), new Vec3f(0.7771457f, 0f, 0.6293205f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(20963.12f, 3185.165f, 9331.772f), new Vec3f(0f, 0.2756374f, 0.961262f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(20977.32f, 3183.672f, 9377.019f), new Vec3f(0.5f, 0f, 0.8660255f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(22945.05f, 2249.32f, 7484.727f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(22935.78f, 2237.349f, 7516.089f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(23004.59f, 2277.208f, 7383.468f), new Vec3f(-7.615241E-05f, -0.2707196f, 0.9626589f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24389.69f, 1822.146f, 10702.22f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24407.92f, 1823.86f, 10740.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24392.38f, 1825.575f, 10735.58f), new Vec3f(0.2923717f, 0f, 0.9563051f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24361.99f, 1828.182f, 10698.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24383.54f, 1828.16f, 10762.39f), new Vec3f(-0.6427872f, 0f, 0.7660448f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(25870.38f, 2430.343f, 14487.97f), new Vec3f(-0.7547097f, 0f, 0.656059f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(25850.28f, 2426.979f, 14454.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(25885.49f, 2436.963f, 14413.79f), new Vec3f(-0.9876881f, 0f, 0.1564345f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(25871.7f, 2421.519f, 14453.71f), new Vec3f(-0.8746195f, 0f, 0.4848096f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(25824.28f, 2436.147f, 14357.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(19458.7f, 2554.835f, 10726.61f), new Vec3f(0f, -0.1736482f, 0.984808f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(19431.48f, 2589.209f, 10567.67f), new Vec3f(0f, -0.241922f, 0.9702964f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(19440.73f, 2561.19f, 10684.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(21014.19f, 2026.03f, 13776.13f), new Vec3f(0f, 0.4694717f, 0.8829477f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20970.07f, 2050.221f, 13788.15f), new Vec3f(-2.980232E-08f, 0.5735768f, 0.8191519f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(21038.52f, 2040.789f, 13817.52f), new Vec3f(-0.4067369f, 0.3273857f, 0.8528685f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26271.4f, 2687.292f, 6251.557f), new Vec3f(0f, -0.1045285f, 0.994522f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26274.59f, 2678.611f, 6296.319f), new Vec3f(0.7660443f, 0f, 0.6427876f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26306.08f, 2668.291f, 6397.199f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(26193.16f, 2687.641f, 6235.768f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(27849.26f, 2987.717f, 6629.854f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(27741.53f, 2963.775f, 6561.917f), new Vec3f(0.7243093f, 0.3255682f, 0.6077678f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(27772.35f, 2961.179f, 6566.408f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(27826.55f, 2976.072f, 6611.958f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(27858.17f, 2989.055f, 6552.885f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(27757.17f, 2949.971f, 6525.691f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(27810.33f, 2973.328f, 6574.145f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(27640.16f, 2888.747f, 7409.636f), new Vec3f(0.6363811f, 0.3090171f, 0.7067729f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(27671f, 2898.97f, 7370.955f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29323.79f, 3318.35f, 5779.792f), new Vec3f(0.7547095f, -0.2457642f, 0.6082876f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29310.22f, 3311.185f, 5793.85f), new Vec3f(-0.1043602f, -0.3837874f, 0.9175062f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29269.84f, 3296.186f, 5833.181f), new Vec3f(0f, -0.2923717f, 0.9563051f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(32420.62f, 4149.408f, 2614.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(32466.49f, 4159.622f, 2516.735f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(32374.35f, 4154.579f, 2477.11f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(32451.06f, 4149.318f, 2562.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(32382.8f, 4150.28f, 2561.019f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31364.61f, 3924.82f, -2864.363f), new Vec3f(0f, 0.2588192f, 0.9659262f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31453.65f, 3890.328f, -2953.454f), new Vec3f(0f, 0.241922f, 0.9702958f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31474.85f, 3936.673f, -2784.737f), new Vec3f(0f, 0.3907315f, 0.9205058f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31672.04f, 3892.039f, -2849.548f), new Vec3f(0f, 0.3090171f, 0.9510568f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31661.65f, 3938.085f, -2714.983f), new Vec3f(0f, 0.3583683f, 0.9335818f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(32338.7f, 3801.936f, -4244.225f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(32148.11f, 3816.404f, -4310.596f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31850.01f, 3820.087f, -4175.376f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31586.48f, 3812.214f, -3688.463f), new Vec3f(0f, -0.1736483f, 0.9848082f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(35652.78f, 3750.774f, -4474.481f), new Vec3f(0.1553641f, 0.1560534f, 0.9754536f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(35723.86f, 3762.099f, -4402.341f), new Vec3f(0f, 0.2249511f, 0.9743704f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(35714.95f, 3740.654f, -4426.925f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(35683.48f, 3734.705f, -4471.087f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(35604.54f, 3745.972f, -4480.019f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(34899.77f, 3762.828f, -4653.82f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(34909.05f, 3761.257f, -4680.265f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(37936.63f, 3798.833f, -4125.561f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(37975.77f, 3804.251f, -4227.884f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38068.12f, 3801.975f, -4124.919f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(37862f, 3795.34f, -4300.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(37835.08f, 3801.771f, -4044.065f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(37751.16f, 3678.306f, -5459.749f), new Vec3f(0f, 0.4383714f, 0.8987947f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(37799.2f, 3690.231f, -5347.411f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38006.48f, 3668.749f, -5365.098f), new Vec3f(0f, 0.190809f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38127.18f, 3678.79f, -5382.146f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29033.98f, 2628.717f, 5026.348f), new Vec3f(-0.01091232f, 0.1383164f, 0.9903281f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(28695.52f, 2576.044f, 5100.803f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(28878.27f, 2602.752f, 5123.754f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(27731.39f, 2530.348f, 4464.055f), new Vec3f(-7.615611E-05f, -0.4494297f, 0.893316f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(27654.17f, 2530.138f, 4367.019f), new Vec3f(-0.1158029f, -0.8660272f, 0.4864015f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(23591.79f, 1648.107f, 4510.193f), new Vec3f(0.01733425f, 0.2836211f, 0.9587804f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(23570.12f, 1626.728f, 4491.394f), new Vec3f(0f, 0.1045285f, 0.994522f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(17935.63f, 1152.732f, 5859.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17960.23f, 1141.351f, 5776.768f), new Vec3f(-0.072914f, -0.03284501f, 0.9967978f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(17571.49f, 928.3239f, 5488.653f), new Vec3f(0.2801666f, 0.6812486f, 0.6763201f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(17272.18f, 954.796f, 5695.326f), new Vec3f(-0.1981709f, 0.3693749f, 0.907905f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(14055.92f, 248.4691f, 5126.257f), new Vec3f(2.380427E-07f, 0.3420202f, -0.9396929f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(14121.96f, 254.9524f, 5146.234f), new Vec3f(0.1715102f, 0.1564345f, -0.9726832f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(12693.01f, -7.731413f, 4461.501f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(13175.31f, -10.41189f, 4436.396f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(20047.15f, -949.2997f, -3111.522f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(20116.71f, -940.0941f, -3023.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(20156f, -911.0641f, -3212.358f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(20260.86f, -949.0577f, -2638.019f), new Vec3f(0.1759538f, 0.2418539f, 0.954226f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(14443.52f, 4436.659f, 20872.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29902.7f, 3273.833f, 10287.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(39226.21f, 3773.435f, -1263.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39709.18f, 3587.302f, -5026.603f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(18753.75f, 2062.139f, -7024.105f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(17059.42f, 2557.643f, -7942.089f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(18994.23f, 1149.573f, 1362.485f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(17918.79f, 1158.573f, 1822.566f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(17027.39f, 660.3488f, 1971.149f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_CROSSBOW_L_02"), 1);
            mi.Spawn(mapName, new Vec3f(29289.52f, -2050.457f, 10.78947f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_STREITKOLBEN"), 1);
            mi.Spawn(mapName, new Vec3f(31474.95f, -1621.668f, 3037.698f), new Vec3f(-0.7232282f, -0.3181024f, -0.6129853f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(29267.29f, -2030.6f, -55.07883f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SAW"), 1);
            mi.Spawn(mapName, new Vec3f(31975.09f, 4344.212f, -423.9116f), new Vec3f(0.07866678f, 0.05147321f, -0.9955706f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31809.38f, 4236.121f, -477.1315f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ADDON_FIREARROW"), 1);
            mi.Spawn(mapName, new Vec3f(67980.13f, 5523.68f, -21301.84f), new Vec3f(0.8090171f, 0f, 0.5877853f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ADDON_FIREARROW"), 1);
            mi.Spawn(mapName, new Vec3f(67914.75f, 5524.063f, -21215.44f), new Vec3f(-0.559193f, 0f, 0.8290376f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ADDON_FIREARROW"), 1);
            mi.Spawn(mapName, new Vec3f(67904.88f, 5523.468f, -21318.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ADDON_FIREBOW"), 1);
            mi.Spawn(mapName, new Vec3f(67838.03f, 5606.333f, -21264.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(34923.68f, 4114.469f, -12884.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(35001.9f, 4100.192f, -12883.94f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_STING"), 1);
            mi.Spawn(mapName, new Vec3f(35003.46f, 4092.916f, -12855.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_PERM_DEX"), 1);
            mi.Spawn(mapName, new Vec3f(52621.18f, 1472.629f, 8180.404f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIRERAIN"), 1);
            mi.Spawn(mapName, new Vec3f(52621.43f, 1460.674f, 8180.404f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_SLD_BOW"), 1);
            mi.Spawn(mapName, new Vec3f(52621.44f, 1460.628f, 8180.404f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_GOBLINBONE"), 1);
            mi.Spawn(mapName, new Vec3f(52632.13f, 1462.528f, 8146.684f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_CLAW"), 1);
            mi.Spawn(mapName, new Vec3f(52604.95f, 1461.762f, 8211.208f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_CHARM"), 1);
            mi.Spawn(mapName, new Vec3f(62854.23f, 3981.268f, -23339.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(64241.87f, 3988.223f, -21264.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FULLHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(57191.07f, 1746.949f, -16444.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_INSTANTFIREBALL"), 1);
            mi.Spawn(mapName, new Vec3f(47429.15f, 2377.141f, -1174.035f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(84072.04f, 3955.477f, -6341.612f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(82164.1f, 4056.911f, -21359.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(74432.18f, 2245.699f, -24683.3f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SHORTSWORD4"), 1);
            mi.Spawn(mapName, new Vec3f(46822.88f, 2472.456f, -32057.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(50161.42f, 2344.031f, -3499.909f), new Vec3f(0.4694717f, 0f, 0.8829479f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTON"), 1);
            mi.Spawn(mapName, new Vec3f(49977.34f, 2342.479f, -3390.331f), new Vec3f(0.669131f, 0f, 0.7431448f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTON"), 1);
            mi.Spawn(mapName, new Vec3f(49969.76f, 2344.448f, -3423.002f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(50075.15f, 2340.539f, -3531.284f), new Vec3f(0.6293209f, 0f, 0.7771468f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(50049.82f, 2346.407f, -3551.735f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(50086.71f, 2343.473f, -3545.471f), new Vec3f(0.9335813f, 0f, 0.3583677f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64943.56f, 2427.818f, -16818.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64925.99f, 2425.448f, -16896.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64950.2f, 2426.969f, -16894.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDRING"), 1);
            mi.Spawn(mapName, new Vec3f(64858.44f, 2418.002f, -17207.06f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(63804.09f, 2434.763f, -15022.04f), new Vec3f(-0.9455192f, 0f, 0.3255682f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(63885.75f, 2438.918f, -15052.31f), new Vec3f(0.7042467f, 0.0635387f, 0.7071066f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(63836.16f, 2436.228f, -15029.07f), new Vec3f(0.8321946f, 0.2542957f, 0.4927339f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(63866.3f, 2433.857f, -15083.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(63893.43f, 2435.529f, -15092.33f), new Vec3f(0.6792399f, 0.06128251f, -0.7313535f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(63891.79f, 2438.837f, -15082.24f), new Vec3f(0.7880121f, -1.677348E-10f, -0.6156605f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(64422.07f, 2456.131f, -15505.76f), new Vec3f(0.8660256f, 0f, 0.5f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(64896.93f, 2423.588f, -16835.21f), new Vec3f(-0.985777f, -0.04983678f, 0.1604979f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(65038.43f, 2435.639f, -16668.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(64855.76f, 2417.581f, -17250.79f), new Vec3f(0.8910068f, 0f, 0.4539899f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_PERM_DEX"), 1);
            mi.Spawn(mapName, new Vec3f(64847.48f, 2428.468f, -17373.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(41475.36f, 2868.437f, -26701.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(41338.85f, 2838.841f, -26413.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(41420.94f, 2867.121f, -26753.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(41725.11f, 2867.853f, -26569.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(45577.93f, 2684.988f, -29862.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(46797.46f, 2414.758f, -31838.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(46564.23f, 2418.818f, -32026.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(46959.07f, 2405.65f, -32124.82f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(46722.73f, 2437.203f, -31613.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(45185.51f, 2544.026f, -32085.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48695.55f, 2269.156f, -30553.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48833.1f, 2255.516f, -30359.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48845.84f, 2249f, -30066.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(52571.37f, 3088.6f, -26801.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(52707.02f, 3088.51f, -26819.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(52516.04f, 3088.905f, -26850.53f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(59224.89f, 1831.227f, -27709.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(59414.84f, 1837.97f, -27632.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(58930.17f, 1821.274f, -27798.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(58150.88f, 1893.188f, -28663.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(58918.29f, 1810.547f, -28292.25f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(58473.05f, 1751.419f, -25136f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(58407.13f, 1768.772f, -25558.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(58351.78f, 1782.752f, -26156.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(58394.54f, 1780.593f, -25950.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(58478.11f, 1780.595f, -25818.24f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54481.34f, 1741.313f, -25776.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54775.43f, 1867.57f, -26320.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58219.89f, 1899.147f, -28890.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58856.36f, 1902.893f, -29227.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58644.38f, 1876.56f, -28965.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63628.23f, 2296.772f, -17899.91f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63203.48f, 2235.778f, -17457.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63398.04f, 2303.341f, -17142.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(64987.1f, 2430.063f, -17261.78f), new Vec3f(0.4999999f, 0f, 0.8660254f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(64491.52f, 2418.609f, -16845.07f), new Vec3f(0.8987941f, 0f, -0.4383712f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(64796.4f, 2471.252f, -16106.73f), new Vec3f(0.7547097f, 0f, 0.656059f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(64181.6f, 2425.767f, -16146.89f), new Vec3f(-0.9702955f, 0f, -0.2419214f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(64529.19f, 2524.046f, -14815f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(64560.6f, 2533.53f, -14698.59f), new Vec3f(0.994522f, 0f, -0.1045286f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(64562.04f, 2539.512f, -14641.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(48980.21f, 3635.42f, 11401.84f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(48740.39f, 3624.603f, 11334.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(49049.11f, 3619.751f, 11440.47f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(62613f, 1802.762f, 7806.022f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(62566.64f, 1841.986f, 7834.604f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_ROCKCRYSTAL"), 1);
            mi.Spawn(mapName, new Vec3f(62454.91f, 1835.759f, 7924.486f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITWR_KRYPTA_GARON"), 1);
            mi.Spawn(mapName, new Vec3f(74421.02f, 2669.849f, -1114.641f), new Vec3f(0.5000002f, 0f, 0.8660253f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(74491.63f, 2666.506f, -1082.521f), new Vec3f(-0.8987951f, 3.352761E-08f, 0.4383698f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_COMMON_01"), 1);
            mi.Spawn(mapName, new Vec3f(74483.22f, 2696.727f, -1083.67f), new Vec3f(0.9471897f, 0.2213608f, 0.2320245f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(73994.89f, 3310.675f, -1902.224f), new Vec3f(-0.6969894f, -0.2935565f, 0.65425f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(73547.12f, 3320.135f, -1266.898f), new Vec3f(0f, 0.4067368f, 0.913546f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(73598.08f, 3318.403f, -1281.893f), new Vec3f(-0.618058f, 0.2307796f, 0.751499f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(73563.57f, 3324.271f, -1306.585f), new Vec3f(-3.591858E-08f, 0.6427884f, 0.766045f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(73551.25f, 3309.6f, -1299.767f), new Vec3f(-0.8054745f, 0.1205727f, 0.580241f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(73556.16f, 3326.788f, -1276.219f), new Vec3f(-0.2036204f, 0.1456493f, 0.9681571f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(73520.4f, 3306.987f, -1282.62f), new Vec3f(-0.1649905f, 0.1769307f, 0.970296f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(73540.77f, 3308.701f, -1233.324f), new Vec3f(-0.1477196f, 0.3794818f, 0.9133314f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64890.96f, 2425.127f, -16815.76f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64941.51f, 2425.986f, -16922.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64826.72f, 2418.496f, -17064.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64896.27f, 2420.687f, -17122.06f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64787.05f, 2416.583f, -17079.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64922.23f, 2418.184f, -17387.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64906.07f, 2417.963f, -17351.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64869.82f, 2416.203f, -17367.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64959.79f, 2419.979f, -17373.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64940.33f, 2418.46f, -17424.7f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64897.09f, 2417.881f, -17328.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64931.63f, 2419.668f, -17313.22f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64946.93f, 2419.761f, -17348.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64927.71f, 2418.246f, -17399.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64935.61f, 2418.614f, -17396.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(64919.86f, 2418.437f, -17361.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65411.86f, 2436.144f, -18331.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65332.86f, 2437.808f, -18380.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65402.05f, 2439.206f, -18210.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65607.21f, 2434.216f, -18100.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65627.21f, 2441.448f, -17724.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(56977.97f, 1771.14f, -16245.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57008.09f, 1777.649f, -16080.75f), new Vec3f(-0.7366986f, 0.1638955f, 0.656059f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57075.86f, 1772.505f, -15979.3f), new Vec3f(0.6781381f, -0.150596f, 0.7193403f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57897.96f, 1779.836f, -17228.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58028.54f, 1789.344f, -17076.57f), new Vec3f(0.6391634f, 0.06816307f, 0.7660444f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52584.84f, 3177.261f, -25278.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52633.7f, 3173.102f, -24965.36f), new Vec3f(0.9901927f, 0.01222929f, 0.1391732f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52547.14f, 3174.878f, -25156.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52691.75f, 3175.717f, -25085.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52443.87f, 3094.046f, -21851.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52296.57f, 3069.728f, -21604.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52294.57f, 3082.73f, -21852.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(47710.57f, 1638.846f, -10151.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(47860.27f, 1665.682f, -10286.37f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(47565.29f, 1620.297f, -10629.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(47907.37f, 1594.194f, -10718.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(47270.47f, 1634.853f, -10330.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(45083.61f, 2587.425f, -15545.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(44976.73f, 2545.022f, -15541.17f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(45124.93f, 2519.106f, -15663.82f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(44691.87f, 2592.974f, -15566.25f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(45074.69f, 2529.723f, -15302.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(45001.3f, 2543.408f, -15594.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(44993.07f, 2549.541f, -15572.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(45323.61f, 3046.748f, 3515.597f), new Vec3f(-0.9925461f, 1.629814E-09f, -0.1218699f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(45066.71f, 2992.614f, 2591.231f), new Vec3f(0.710548f, 0.1933655f, 0.6765588f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(45313.2f, 3046.46f, 3441.145f), new Vec3f(0.9335806f, -4.656613E-09f, 0.3583676f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(45286.2f, 3046.975f, 3522.002f), new Vec3f(-0.7880052f, 0.002979971f, 0.6156613f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(45236.29f, 3046.358f, 3445.766f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(45214.25f, 3047.335f, 3547.934f), new Vec3f(-0.777146f, 7.450581E-09f, -0.6293204f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_NAGELKEULE"), 1);
            mi.Spawn(mapName, new Vec3f(45486.79f, 3043.184f, 3511.75f), new Vec3f(0.9998475f, 0f, -0.01745379f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(44584.02f, 2994.102f, 3049.089f), new Vec3f(0.3255681f, 1.723566E-09f, -0.9455186f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44471.08f, 2984.257f, 3090.594f), new Vec3f(0.9961947f, 0f, -0.08715564f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44474.21f, 2984.693f, 3093.295f), new Vec3f(0.848048f, 0f, -0.5299194f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44474.1f, 2985.23f, 3101.195f), new Vec3f(0.9945219f, 0f, 0.1045283f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44443.23f, 2980.468f, 3076.865f), new Vec3f(0.898794f, 0f, -0.4383712f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44488.5f, 2989.086f, 3135.75f), new Vec3f(0.9781476f, 0f, -0.2079119f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44464.85f, 2985.845f, 3127.26f), new Vec3f(0.9925461f, 0f, -0.1218695f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44514.42f, 2990.575f, 3116.949f), new Vec3f(0.6946583f, 0f, -0.7193399f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44502.68f, 2987.649f, 3089.155f), new Vec3f(0.9743701f, 0f, -0.2249509f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(44487.5f, 2986.381f, 3094.313f), new Vec3f(0.9563047f, 0f, -0.2923718f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(44112.87f, 3439.305f, 7263.909f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(44237.95f, 3459.218f, 7403.866f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(43883.47f, 3544.417f, 7764.253f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(47331.53f, 3399.476f, 5224.661f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(47463.84f, 3417.364f, 5145.721f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(47522.67f, 3417.13f, 5002.445f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(47311.89f, 3116.77f, 9672.765f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(47266.86f, 3112.712f, 9664.288f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(47704.23f, 3217.504f, 9951.526f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48352.47f, 3348.689f, 10257.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48031.08f, 3504.754f, 7759.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48364.09f, 3497.15f, 7956.106f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48706.23f, 3480.246f, 8106.618f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(48997.08f, 3421.941f, 8341.448f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(49248.69f, 3372.081f, 8507.588f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(49550.49f, 3368.829f, 8719.739f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(52250.13f, 3405.253f, 10786.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(52659.9f, 3456.539f, 10972.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(53191.32f, 3406.936f, 10934.24f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(54657.34f, 2998.211f, 11039.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(54962.11f, 2882.075f, 10746.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(50663.7f, 1276.624f, 5166.595f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(50995.15f, 1313.364f, 5146.192f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(49769.14f, 1215.031f, 5343.505f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(50004.88f, 1111.009f, 6432.421f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(50792.06f, 1111.978f, 6049.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(52053.61f, 1213.607f, 6304.683f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(53423.23f, 1514.835f, 4159.481f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(53453.74f, 1572.949f, 2757.501f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(53681.15f, 1563.671f, 288.9269f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(53885.38f, 1581.121f, 509.7592f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(54012.52f, 1616.625f, 108.8512f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(53294.9f, 1560.294f, 161.4036f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(61527.14f, 2166.745f, 5355.104f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(60870.53f, 2123.751f, 5553.644f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(60683.66f, 2167.453f, 4758.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(63677.5f, 2356.031f, 3639.461f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65093.44f, 2349.553f, 4535.771f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65726.88f, 2339.675f, 3445.757f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68873.45f, 2124.611f, 2011.573f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70283.52f, 2412.394f, 2151.211f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(70484.52f, 2453.398f, 2100.472f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70726.34f, 2477.433f, 2223.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70641.45f, 2495.633f, 1810.937f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(74762.95f, 3274.668f, -929.972f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(73794.27f, 3300.43f, -791.2029f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73939.53f, 3299.185f, -1574.835f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73890.19f, 3257.325f, -1901.926f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(75184.82f, 3252.676f, -806.1224f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(74761.37f, 3245.05f, -371.7441f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(81064.8f, 3779.565f, -5095.168f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(82504.19f, 4127.759f, -7177.623f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(82056.23f, 4167.671f, -8807.749f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(85096.2f, 4876.661f, -9536.339f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(84642.51f, 4923.242f, -11650.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(83649.09f, 4495.763f, -13724.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(81590.7f, 4237.883f, -17557.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(79596.25f, 4097.804f, -19106.99f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(82340.42f, 4143.879f, -19610.22f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHADOWFUR"), 1);
            mi.Spawn(mapName, new Vec3f(82139.36f, 4137.437f, -19764.15f), new Vec3f(-0.743145f, 0f, -0.6691304f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(81853.55f, 4130.884f, -19819.04f), new Vec3f(-0.8660255f, 0f, 0.4999998f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(81906.74f, 4130.705f, -19832.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(81823.52f, 4134.971f, -19735.63f), new Vec3f(-0.8660256f, 0f, -0.4999996f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(81801.09f, 4128.136f, -19854.83f), new Vec3f(0.9998477f, 0f, 0.01745226f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(81920.42f, 4136.511f, -19729.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 1);
            mi.Spawn(mapName, new Vec3f(81971.16f, 4131.094f, -19841.25f), new Vec3f(-0.9271835f, 0f, -0.3746074f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_02"), 1);
            mi.Spawn(mapName, new Vec3f(82391.63f, 4171.742f, -18988.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(82389.84f, 4171.728f, -19071.9f), new Vec3f(0.7660444f, 9.566996E-10f, 0.6427876f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(82404.22f, 4172.651f, -19034.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(82404.16f, 4172.617f, -19034.64f), new Vec3f(0.2419219f, -2.749508E-10f, 0.9702957f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(82404.2f, 4172.671f, -19034.65f), new Vec3f(-0.5299193f, -1.320678E-10f, 0.8480481f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(81729.74f, 4243.386f, -18789.67f), new Vec3f(0.1477006f, 0.2371628f, 0.9601766f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(81501.3f, 4219.67f, -18449.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BEER"), 1);
            mi.Spawn(mapName, new Vec3f(81495.9f, 4215.309f, -18526.3f), new Vec3f(-0.9876885f, 6.352713E-10f, -0.1564336f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_APPLE"), 1);
            mi.Spawn(mapName, new Vec3f(82120.08f, 4189.204f, -18518.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_MILK"), 1);
            mi.Spawn(mapName, new Vec3f(82216.94f, 4191.257f, -18536.49f), new Vec3f(0.1341802f, -6.454827E-10f, 0.990957f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(52538.39f, 1628.438f, -288.9219f), new Vec3f(0.5195995f, -0.5224006f, -0.6761041f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(52670.73f, 1655.323f, 1367.133f), new Vec3f(0.9618567f, -0.08868582f, 0.2587795f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(52457.37f, 1588.39f, 2326.184f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_02"), 1);
            mi.Spawn(mapName, new Vec3f(53000.51f, 1561.05f, 2269.492f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SHORTSWORD1"), 1);
            mi.Spawn(mapName, new Vec3f(52459.39f, 1611.866f, 2353.398f), new Vec3f(0.7850174f, -0.3947669f, 0.4773987f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_NAGELKNUEPPEL"), 1);
            mi.Spawn(mapName, new Vec3f(53902.43f, 1613.172f, 2346.65f), new Vec3f(-0.630368f, -0.04815368f, 0.7748022f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(53900.38f, 1612.865f, 2371.362f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(53959.82f, 1615.577f, 2328.588f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(70079.55f, 1746.658f, -23929.26f), new Vec3f(0.6421546f, -0.03150185f, 0.765928f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(84862.7f, 4272.845f, -10822.23f), new Vec3f(0.9702968f, 0f, 0.2419222f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(84812.02f, 4273.112f, -10814.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(85422.23f, 4299.487f, -11132.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_NUGGET"), 1);
            mi.Spawn(mapName, new Vec3f(63007.91f, 4225.696f, -27348.9f), new Vec3f(0.5877855f, 0f, 0.8090171f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_NUGGET"), 1);
            mi.Spawn(mapName, new Vec3f(63157.82f, 4175.415f, -27282.33f), new Vec3f(-0.9993921f, 0f, -0.03490123f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_NUGGET"), 1);
            mi.Spawn(mapName, new Vec3f(63347.89f, 4070.383f, -27419.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_NUGGET"), 1);
            mi.Spawn(mapName, new Vec3f(63104.46f, 4069.383f, -28708.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_NUGGET"), 1);
            mi.Spawn(mapName, new Vec3f(62901.08f, 4072.218f, -28909.37f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(64333.29f, 3999.02f, -22083.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(65046.86f, 3982.035f, -24803.26f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JOINT"), 1);
            mi.Spawn(mapName, new Vec3f(65542.82f, 3930.102f, -25467.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(65619.71f, 5733.596f, -24501.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(65633.1f, 5734.59f, -24476.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(40994.64f, 3782.813f, -2996.134f), new Vec3f(0.1283916f, 0.1468643f, 0.9807892f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(40974.91f, 3775.396f, -2971.373f), new Vec3f(0.6937855f, 0.358368f, -0.6246873f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(40985.96f, 3768.864f, -2868.585f), new Vec3f(0.9396923f, 0f, 0.3420205f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(44159.28f, 3170.47f, -1438.228f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(44192.39f, 3179.402f, -1422.981f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(41863.58f, 3448.619f, -1478.201f), new Vec3f(0f, -0.1564346f, 0.987689f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(43712.63f, 3300.392f, -2354.697f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(43959.89f, 3263.842f, -2291.623f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45984.78f, 2523.913f, -3897.586f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45788.57f, 2544.089f, -3525.232f), new Vec3f(0.5149598f, -0.01745241f, 0.8570368f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(45873.46f, 2522.383f, -3696.055f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(45240.43f, 2491.689f, -4733.622f), new Vec3f(0f, 0.2419219f, 0.970296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45446.62f, 2469.059f, -4818.776f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45378.3f, 2492.907f, -4712.274f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45072.28f, 2081.177f, -6079.593f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45060.03f, 2081.477f, -6072.863f), new Vec3f(0.9961945f, 0f, 0.08715621f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(45018.94f, 2088.62f, -6053.115f), new Vec3f(0f, 0.3255682f, 0.9455187f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(48213.2f, 2067.438f, -5569.939f), new Vec3f(0f, 0.1736482f, 0.984808f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(48274.33f, 2057.89f, -5618.457f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(47176.86f, 2372.533f, -4109.135f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(47193.26f, 2369.521f, -4148.158f), new Vec3f(0.8090169f, 0f, 0.5877851f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(47090.25f, 2372.557f, -4094.835f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(50462.86f, 1410.436f, -8032.226f), new Vec3f(0.3746066f, 0f, 0.9271838f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(50209.03f, 1394.897f, -7921.544f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(50243.95f, 1405.921f, -7895.154f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(48557.94f, 1403.705f, -6904.035f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(48678.8f, 1414.86f, -6922.171f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(48326.45f, 1508.718f, -10668.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(48378.46f, 1504.489f, -10608.91f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(48089.05f, 1531.206f, -10846.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(48082.75f, 1576.694f, -10549.29f), new Vec3f(-0.0156918f, 0.175383f, 0.9843761f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(48205.07f, 1583.164f, -10236.33f), new Vec3f(0.003328359f, 0.1095907f, 0.9939719f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(48223.77f, 1568.795f, -10385.55f), new Vec3f(0f, 0.2249511f, 0.9743704f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(48062.37f, 1600.096f, -10409.27f), new Vec3f(0f, 0.1391731f, 0.9902682f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(47690.73f, 1601.62f, -10473.96f), new Vec3f(0f, 0.190809f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(47749.88f, 1565.734f, -10701.99f), new Vec3f(0f, 0.1045285f, 0.994522f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(47826.95f, 1586.406f, -10554.03f), new Vec3f(0f, 0.1391731f, 0.9902682f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(47691.16f, 1622.277f, -10288.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(47440.64f, 1553.748f, -11099.75f), new Vec3f(0f, 0.190809f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(47394.31f, 1563.793f, -10865.49f), new Vec3f(0f, 0.1045285f, 0.994522f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(51279.84f, 1508.598f, -12798.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(51506.47f, 1510.261f, -12858.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(50956.11f, 1452.059f, -12706.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(51131.87f, 1501.489f, -12704.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SWAMPHERB"), 1);
            mi.Spawn(mapName, new Vec3f(51169.37f, 1472.341f, -12363.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(52263.59f, 1553.47f, -11917.85f), new Vec3f(0.9281234f, 0.1564344f, -0.3378088f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52246.96f, 1543.553f, -11888.97f), new Vec3f(0.4999999f, 0f, 0.8660254f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52297.4f, 1558.465f, -11827.62f), new Vec3f(0.9999998f, 0f, 1.490116E-07f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52240.75f, 1545.544f, -11795f), new Vec3f(0.9848076f, 0f, -0.1736477f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(52259.98f, 1548.819f, -11825.71f), new Vec3f(0.4226182f, 0f, -0.9063081f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(52342.48f, 1591.131f, -11838.79f), new Vec3f(-0.2902949f, -0.9437829f, 0.15813f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(51489.74f, 1757.95f, -6361.08f), new Vec3f(0.6427875f, 0f, 0.7660444f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(51563.08f, 1740.58f, -6370.887f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54541.07f, 1676.366f, -4783.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54505.7f, 1691.207f, -4889.924f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(52834.42f, 1605.281f, -2836.005f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(52984.82f, 1606.069f, -2770.05f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(53406.76f, 1657.633f, -2443.757f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(53437.97f, 1673.711f, -2393.032f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(54794.67f, 1742.224f, -1318.56f), new Vec3f(0.6326416f, 0.234495f, -0.7380908f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(54893.62f, 1752.552f, -1315.979f), new Vec3f(-0.6018167f, 0f, -0.7986336f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(54847.1f, 1746.865f, -1296.938f), new Vec3f(0.4100617f, 0.2419216f, -0.8793866f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54884.32f, 1738.485f, -1285.928f), new Vec3f(0.8480483f, 0f, -0.5299171f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54747.02f, 1729.813f, -1320.44f), new Vec3f(0.984808f, 0f, -0.173648f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54817.39f, 1743.478f, -1268.47f), new Vec3f(0.9667139f, 0.1908089f, -0.1704568f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54865.72f, 1745.289f, -1232.71f), new Vec3f(0.9998478f, 0f, -0.01745236f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54722.14f, 1736.833f, -1213.301f), new Vec3f(0.6427875f, 0f, 0.7660444f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61537.37f, 1975.685f, -2131.101f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61669.38f, 1991.815f, -2166.227f), new Vec3f(0f, 0.03489951f, 0.9993911f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61277.67f, 1943.441f, -2032.562f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61310.16f, 2225.395f, -7258.344f), new Vec3f(-0.07926824f, -0.04929277f, 0.9956343f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59150.54f, 2093.613f, -7329.552f), new Vec3f(0.9063078f, 0f, 0.422618f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59083.12f, 2094.661f, -7489.343f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59749.27f, 2075.549f, -6651.425f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59886f, 2074.658f, -7011.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59935.31f, 2075.284f, -6790.894f), new Vec3f(-0.8829474f, 0f, 0.4694716f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59940.61f, 2081.309f, -7679.491f), new Vec3f(-0.4999999f, 0f, 0.8660254f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59969.94f, 2086.21f, -7849.104f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59944.34f, 2079.377f, -7791.21f), new Vec3f(-0.981627f, 0f, -0.1908081f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59250.77f, 2083.626f, -7080.463f), new Vec3f(0.1736482f, 0f, 0.984808f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59574.12f, 2078.099f, -6703.631f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59505.49f, 2080.681f, -6944.429f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59880.47f, 2070.873f, -6894.941f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59445.2f, 2089.061f, -7693.025f), new Vec3f(0.7431448f, 0f, 0.6691306f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59542.86f, 2084.996f, -7694.613f), new Vec3f(-0.7313536f, 0f, 0.6819984f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59500.73f, 2085.967f, -7645.14f), new Vec3f(0.9898966f, -0.07186016f, 0.122234f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(60149.39f, 2088.051f, -7804.202f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59730.32f, 2080.968f, -7497.598f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(60920.5f, 2021.942f, -11410.28f), new Vec3f(0.007680404f, 0.5105636f, 0.8598059f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(60855.74f, 2005.87f, -11415.85f), new Vec3f(0.7313538f, 0.3306393f, 0.5964892f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(60956.48f, 2034.639f, -11408.28f), new Vec3f(0f, 0.5735766f, 0.8191524f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(60890.62f, 2022.498f, -11412.08f), new Vec3f(0.6077676f, 0.3255682f, 0.7243093f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(59430.09f, 1990.826f, -12549.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(59729f, 2026.934f, -12670.51f), new Vec3f(0f, -0.1564345f, 0.9876888f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(59535.59f, 1996.114f, -12937.22f), new Vec3f(0f, 0.4226186f, 0.9063088f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54016.49f, 1628.644f, -10360.06f), new Vec3f(-0.3420203f, 0f, 0.9396923f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54017.46f, 1630.942f, -10338.35f), new Vec3f(-0.6560588f, 0f, 0.7547096f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64074.78f, 2367.994f, -11265.38f), new Vec3f(-0.6018149f, 0f, 0.7986355f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64301.05f, 2373.926f, -11232.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64146.55f, 2360.637f, -11321.06f), new Vec3f(-0.6379965f, 0.1218694f, 0.7603346f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64269.48f, 2368.805f, -11307.26f), new Vec3f(0.1558391f, 0.08715573f, 0.9839299f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61762.12f, 2176.421f, -16837.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61672.32f, 2189.629f, -16879.76f), new Vec3f(-0.6492546f, 0.2419219f, 0.7210703f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(61719.69f, 2182.397f, -16872.66f), new Vec3f(-0.02402339f, 0.105501f, 0.9941295f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(56081.01f, 1981.386f, -27182.03f), new Vec3f(0.8151964f, 0.2756374f, -0.5093912f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(56098.75f, 1968.65f, -27163.38f), new Vec3f(0.961262f, 0f, -0.2756374f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(56241.02f, 1962.052f, -27091.92f), new Vec3f(0.1045286f, 0f, -0.9945219f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(56235.35f, 1992.504f, -27117.71f), new Vec3f(-0.9700248f, -0.1744075f, 0.1692273f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(56199f, 1726.55f, -25049.37f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(56102.79f, 1722.453f, -25064.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(57879.39f, 1686.937f, -24341.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(57795.75f, 1675.048f, -24330.32f), new Vec3f(0.9961948f, 0f, -0.08715562f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(57911.84f, 1682.071f, -24310.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(56714.54f, 1650.791f, -22423.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(56805.57f, 1652.079f, -22345.51f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57977.86f, 1747.134f, -20893.99f), new Vec3f(0f, 0.241922f, 0.9702961f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57899.44f, 1735.26f, -20840.44f), new Vec3f(0f, 0.1391731f, 0.9902682f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58159.92f, 1754.115f, -20974.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57810.94f, 1730.052f, -20719.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57749.35f, 1699.323f, -21032.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(55245.7f, 1790.003f, -17924.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(55313.91f, 1775.677f, -18082.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(55339.58f, 1780.793f, -17994.98f), new Vec3f(-0.8138067f, 0.190809f, 0.5489197f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(56742.88f, 1679.178f, -20094.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(56725.84f, 1672.988f, -20161.34f), new Vec3f(0f, 0.1045285f, 0.9945219f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(66417.34f, 2375.84f, -21009.73f), new Vec3f(0.1564344f, 0f, -0.9876881f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(66360.2f, 2381.379f, -20964.04f), new Vec3f(-0.3171155f, 0.3746066f, -0.8712682f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68773.17f, 1741.249f, -23029.6f), new Vec3f(0.1496198f, -0.5796461f, 0.8010166f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(68795.46f, 1702.272f, -23040.56f), new Vec3f(0.001845381f, 0.08724981f, 0.9961854f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(68802.98f, 1710.077f, -22968.73f), new Vec3f(0f, 0.05233596f, 0.9986296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(71141.62f, 1987.505f, -21769.93f), new Vec3f(0.7986355f, 0f, 0.601815f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(71342.48f, 1995.431f, -21911.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(71226.12f, 2001.37f, -21830.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(76776.85f, 3632.618f, -19548.58f), new Vec3f(0f, 0.1391731f, 0.9902683f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(76843.68f, 3650.978f, -19311.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(79415.84f, 4108.462f, -18541.37f), new Vec3f(0.8283452f, 0.1564345f, 0.5379337f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(79173.81f, 3968.307f, -20523.83f), new Vec3f(0.7431445f, 0f, -0.6691306f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(79218f, 3962.428f, -20502.83f), new Vec3f(0.5446393f, 0f, -0.8386707f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80614.13f, 4218.965f, -17965.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80807.56f, 4217.185f, -17859.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80441.58f, 4210.022f, -17783.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(81096.2f, 4221.337f, -17941.23f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(78303.83f, 3902.867f, -14971.54f), new Vec3f(-0.01745241f, 0f, -0.9998475f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(78287.2f, 3889.443f, -14965.9f), new Vec3f(0.5906886f, 0.1834174f, -0.785777f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(80212.11f, 4097.277f, -13709.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80185.56f, 4102.608f, -13774.47f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80211.27f, 4107.198f, -13705.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80194.78f, 4098.968f, -13750.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MIL_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(80383.44f, 4182.672f, -13758.79f), new Vec3f(-0.1152084f, -0.3583344f, -0.9264584f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_SLD_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(80412.08f, 4126.534f, -13658.6f), new Vec3f(-0.5252298f, -0.01680699f, -0.8507972f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(80444.68f, 4130.708f, -13710.35f), new Vec3f(0.1759537f, 0.5257068f, -0.8322703f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(80348.95f, 4114.397f, -13678.62f), new Vec3f(-0.2423875f, 0.9699317f, -0.02198287f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(80400.23f, 4133.207f, -13780.64f), new Vec3f(0f, 0.9961956f, -0.0871557f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(80452.61f, 4153.731f, -13896.2f), new Vec3f(0f, 0.9986295f, -0.05233605f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_MEDIUMHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(80425.42f, 4142.988f, -13851.73f), new Vec3f(0.008187151f, -0.03425528f, 0.9993798f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_MEDIUMHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(80471.36f, 4157.364f, -13941.73f), new Vec3f(0.8596651f, 0.06923656f, 0.5061445f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(82482.19f, 4457.934f, -13970.03f), new Vec3f(-0.3333874f, 0.1642703f, 0.9283704f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(82505.02f, 4444.96f, -13972.45f), new Vec3f(0f, 0.2079117f, 0.9781479f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(82328.38f, 4412.856f, -13836.9f), new Vec3f(0f, -2.04891E-08f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(82252.58f, 4144.335f, -15492.19f), new Vec3f(0f, 0.1391732f, 0.9902683f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(82256.02f, 4161.209f, -15344.11f), new Vec3f(0.006659148f, 0.03553924f, 0.999347f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(85016.26f, 4368.983f, -15447.1f), new Vec3f(0f, 0.190809f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(85033.88f, 4372.491f, -15454.99f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(84614.1f, 4959.731f, -11243.45f), new Vec3f(0.9945215f, 0f, -0.1045275f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(84639.56f, 4953.33f, -11266.07f), new Vec3f(-0.05233584f, 0f, 0.998629f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(84592.52f, 4947.677f, -11306.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(82548.2f, 4379.33f, -10524.72f), new Vec3f(0.8290375f, 0f, -0.5591929f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(82566.21f, 4385.504f, -10552.02f), new Vec3f(-0.8290375f, 0f, -0.5591929f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(82430.49f, 4158.24f, -8129.621f), new Vec3f(0.007596131f, 0.05266444f, 0.9985839f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(82382.23f, 4154.888f, -8204.934f), new Vec3f(1.024455E-08f, -0.1218695f, 0.9925463f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(81922.02f, 3876.787f, -5945.584f), new Vec3f(-0.9975641f, 0f, -0.06975639f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(81906.42f, 3864.374f, -5912.431f), new Vec3f(-0.0523361f, 0f, -0.9986296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(81907.13f, 3856.304f, -5848.092f), new Vec3f(-0.6691306f, 0f, 0.7431448f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78985.2f, 3415.965f, -2741.541f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(79042.79f, 3417.241f, -2745.011f), new Vec3f(0.241922f, 0f, 0.9702958f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(79142.56f, 3417.161f, -2763.243f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(78429.41f, 3480.613f, -3274.671f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(78750.52f, 3488.689f, -3216.04f), new Vec3f(0.02194105f, -0.3382241f, 0.94081f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(78590.78f, 3573.576f, -3990.201f), new Vec3f(0f, -0.1391732f, 0.9902686f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(76821.13f, 3503.435f, -4311.708f), new Vec3f(0f, -0.0348995f, 0.999391f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(76669.05f, 3509.837f, -4500.245f), new Vec3f(0.002434467f, -0.1563923f, 0.9876928f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(76968.04f, 3546.847f, -4584.193f), new Vec3f(-0.01815118f, 0.06816947f, 0.9975088f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(76988.42f, 3525.54f, -4339.86f), new Vec3f(0f, -0.01745241f, 0.9998477f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(77131.77f, 3562.74f, -4611.146f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(76478.69f, 3511.193f, -4822.276f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(76438.41f, 3497.832f, -4717.983f), new Vec3f(0.9848077f, 0f, -0.1736475f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(78833.09f, 3381.278f, -2567.593f), new Vec3f(0f, -0.241922f, 0.9702964f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(78986.09f, 3360.548f, -2462.898f), new Vec3f(0f, -0.1564345f, 0.9876889f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(80720.86f, 3593.241f, -3636.245f), new Vec3f(-0.9135453f, 0f, -0.4067366f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(80907.26f, 3615.128f, -3664.664f), new Vec3f(0.1582126f, -0.02317768f, 0.9871337f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(80825.22f, 3604.029f, -3598.922f), new Vec3f(0f, -0.3420202f, 0.9396929f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(82942.7f, 3935.389f, -5348.753f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(83001.84f, 3952.023f, -5291.508f), new Vec3f(-0.9335805f, 0f, 0.3583679f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(83796.01f, 4397.318f, -8222.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(83769.58f, 4409.92f, -8289.649f), new Vec3f(9.313226E-10f, -0.01745253f, 0.9998478f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(82597.84f, 4197.759f, -8135.765f), new Vec3f(-0.1736482f, 0f, -0.9848077f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(56056.8f, 1731.44f, 1575.892f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(55511.96f, 1610.844f, 2928.891f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(55507.2f, 1613.468f, 2831.264f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(54718.3f, 1651.979f, 2386.49f), new Vec3f(-0.9563048f, 0f, 0.2923715f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54647.19f, 1646.696f, 2468.801f), new Vec3f(-0.8660251f, 0f, -0.4999423f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54729.89f, 1652.405f, 2422.341f), new Vec3f(-0.9271842f, 0.3746066f, 1.381612E-07f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(54685.21f, 1658.912f, 2282.76f), new Vec3f(-0.8746194f, 0f, 0.48481f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(53182.46f, 1545.488f, 2938.023f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(53506.99f, 1575.059f, 2848.042f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(53842.85f, 1599.483f, 2938.554f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(53102.09f, 1699.591f, -5372.605f), new Vec3f(-0.05233609f, 0f, -0.9986295f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(53039.09f, 1695.961f, -5376.099f), new Vec3f(0.9396923f, 0f, -0.3420176f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(53082.7f, 1691.092f, -5373.853f), new Vec3f(0.8829476f, 0f, -0.4694717f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(52429.33f, 1590.451f, 479.2955f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(49529.92f, 1213.475f, 5770.122f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(50464.02f, 1976.857f, 4145.913f), new Vec3f(-0.08545757f, -0.3308495f, 0.9398066f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(51022.75f, 1207.449f, 5687.275f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(49749.59f, 1125.287f, 6396.214f), new Vec3f(0f, -0.2079118f, 0.9781483f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(53086.3f, 999.827f, 7609.291f), new Vec3f(-0.08928135f, -0.345954f, 0.933994f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(53089.88f, 957.2977f, 7822.252f), new Vec3f(-7.615241E-05f, -0.06465577f, 0.9979078f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54772.84f, 1284.909f, 7110.077f), new Vec3f(0.07424913f, -0.3735693f, 0.924626f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54765.54f, 1284.906f, 7112.297f), new Vec3f(0f, -0.4067366f, 0.9135457f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54763.14f, 1325.533f, 6947.542f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54866.18f, 1267.445f, 7201.289f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(56086.03f, 2311.546f, 9931.445f), new Vec3f(-0.8938705f, 0.4383712f, 0.09394991f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(56031.35f, 2296.42f, 9801.661f), new Vec3f(-0.06814849f, 0.3279001f, 0.9422519f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(56035.99f, 2288.844f, 9829.199f), new Vec3f(3.72529E-09f, 0.2249511f, 0.9743702f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(56094.55f, 2315.635f, 9975.779f), new Vec3f(-0.6794032f, 0.7313539f, 0.05944008f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(56071.36f, 2300.635f, 9891.803f), new Vec3f(0f, 0.7313542f, 0.6819984f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(56038.17f, 2299.149f, 9857.027f), new Vec3f(-0.6801497f, 0.6946588f, 0.234194f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59397.48f, 2228.589f, 8736.712f), new Vec3f(0.009088045f, 0.2411415f, 0.9704479f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59454.63f, 2271.973f, 8874.404f), new Vec3f(0f, 0.4539907f, 0.8910071f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59552.4f, 2293.287f, 8928.285f), new Vec3f(0f, 0.1908093f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59567.01f, 2240.732f, 8708.118f), new Vec3f(0f, 0.3090171f, 0.951057f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59989.77f, 2220.396f, 8731.778f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59801.62f, 2303.272f, 9038.501f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(60012.45f, 2281.306f, 9007.797f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(60172.16f, 1611.381f, 9236.269f), new Vec3f(0.8660251f, 0f, -0.499997f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(60233.13f, 1632.181f, 9220.183f), new Vec3f(0.9335802f, 0.3583678f, 1.196383E-06f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(59062.42f, 1612.574f, 9339.931f), new Vec3f(-0.9396924f, 0f, 0.3420205f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(59014.01f, 1620.848f, 9353.449f), new Vec3f(-0.756613f, 0.1564345f, -0.634874f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(58982.66f, 1620.72f, 9308.577f), new Vec3f(-0.8528951f, 0.2155396f, -0.4755098f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(60094.42f, 3898.612f, -31250.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(61414.39f, 4279.395f, -34985.5f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(84950.16f, 4335.175f, -9531.033f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(73428.72f, 2740.449f, -347.1926f), new Vec3f(-0.3798864f, -0.3034897f, -0.8738312f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(52763.03f, 1610.72f, 1453.412f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(66585.71f, 1430.858f, -31612.17f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(74372.43f, 3277.713f, -6936.419f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(74636.8f, 3277.707f, -5950.779f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(74044.84f, 3260.555f, -2254.117f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(75278.52f, 3316.448f, -1880.973f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(81389.72f, 4192.127f, -19334.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(66341.26f, 1450.182f, -29204.22f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(67900.83f, 1493.396f, -30288.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(63666.45f, 4007.859f, -30654.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(61808.99f, 4028.802f, -31850.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(62722.82f, 4115.442f, -23011.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(59847.68f, 2088.913f, -7375.496f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(61321.04f, 1730.05f, 8766.345f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(48690.68f, 1326.825f, 6580.518f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(45520.47f, 3051.911f, 3096.286f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(47861.11f, 1621.69f, -10268.37f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(41441.49f, 2960.566f, -27035.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(49212.57f, 2202.188f, -30905.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(49367.32f, 2177.816f, -31194.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(59092.07f, 2114.019f, -7738.334f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(52313.44f, 1981.815f, -3373.785f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(41763.78f, 1962.688f, -7888.595f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(44624.34f, 2970.083f, 177.0714f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_BAU_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(50415.7f, 1297.638f, 5808.695f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_STABKEULE"), 1);
            mi.Spawn(mapName, new Vec3f(85396.85f, 4294.223f, -9470.523f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(75175.52f, 3354.494f, -1693.962f), new Vec3f(-0.1710099f, 0.1285222f, 0.9768504f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(75153.03f, 3333.001f, -1655.089f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(74336.27f, 3282.495f, -6516.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_BAU_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(64223.12f, 2437.732f, -3813.316f), new Vec3f(-0.7401726f, 0.6613985f, 0.1212195f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(67240.08f, 2502.975f, -22094.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_CHARM"), 1);
            mi.Spawn(mapName, new Vec3f(66795.88f, 2481.989f, -22269.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_SLD_BOW"), 1);
            mi.Spawn(mapName, new Vec3f(66615.7f, 1469.436f, -31716.27f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_RAPIER"), 1);
            mi.Spawn(mapName, new Vec3f(66508.7f, 1429.345f, -31567.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(66557.29f, 1443.899f, -31680.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(63695.64f, 3965.995f, -23749.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_BAU_MACE"), 1);
            mi.Spawn(mapName, new Vec3f(63752.01f, 3952.775f, -23776.39f), new Vec3f(-0.5f, 0f, 0.8660253f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(67587.06f, 4064.682f, -21662.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SPICKER"), 1);
            mi.Spawn(mapName, new Vec3f(67666.98f, 4082.712f, -21681.55f), new Vec3f(-0.3726867f, 0.9153779f, 0.1522904f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_AXE_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(81375.71f, 4258.239f, -18568.74f), new Vec3f(-0.07389074f, -0.2340346f, 0.9694169f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(76650.49f, 3542.46f, -9761.967f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(76499.88f, 3579.181f, -9449.914f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_BAU_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(70027.36f, 3008.75f, -5862.255f), new Vec3f(0f, -0.1736481f, -0.9848076f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(65445.95f, 2428.236f, -6420.143f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(65496.91f, 2409.679f, -6381.371f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(42501.1f, 2733.058f, -19449.47f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(40299.46f, 2829.841f, -20796.22f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(41385.28f, 2823.228f, -22220.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(42236.3f, 2829.63f, -22280.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42690.85f, 2867.422f, -23866.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(43234.68f, 2879.973f, -24743.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(44585.59f, 2881.292f, -24712.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45030.86f, 2954.951f, -26289.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(45651.48f, 2730.547f, -28861.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(43715.41f, 2904.492f, -27265.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39787.18f, 2884.934f, -26259.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39221.07f, 2894.971f, -25929.9f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(38878.61f, 2922.777f, -25416.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(38522.7f, 2740.719f, -23105.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39246.4f, 2732.635f, -21173.9f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38070.95f, 2757.482f, -19603.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38354.45f, 2726.478f, -18149.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(38361.71f, 3088.057f, -15354.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(38553.36f, 3091.455f, -14594.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(39882.49f, 3212.416f, -12015.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(39936.49f, 3387.8f, -10464.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(40760.02f, 3404.695f, -9302.609f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42160.12f, 2884.26f, -13190.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(44000.97f, 2593.765f, -15225.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42615.94f, 2611.51f, -16083.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(44269.79f, 2644.796f, -17298.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(46129.2f, 2518.138f, -15776.91f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(47070.33f, 2508.64f, -14734.42f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(45009.12f, 2720.48f, -19422.52f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(46520.97f, 2899.759f, -19601.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(48600.3f, 3030.879f, -21720.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(49933.02f, 2997.906f, -24748.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(50020.88f, 2503.35f, -28297.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(49473.88f, 2202.975f, -30184.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(50631.43f, 2194.581f, -30167.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(48387.92f, 2315.627f, -30943.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(46652f, 2449.745f, -31143.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(46362.24f, 2480.102f, -32834.84f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(45132.07f, 2505.194f, -32345.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(74287.41f, 3454.078f, -5838.539f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(74137f, 3454f, -5798f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHALICE"), 1);
            mi.Spawn(mapName, new Vec3f(74221.66f, 3446.406f, -5791.981f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(48931.2f, 1328.749f, 6263.578f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_QUARTZ"), 1);
            mi.Spawn(mapName, new Vec3f(48927.25f, 1310.186f, 6252.401f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_PITCH"), 1);
            mi.Spawn(mapName, new Vec3f(48925.59f, 1312.138f, 6233.135f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_LURKERSKIN"), 1);
            mi.Spawn(mapName, new Vec3f(49898.85f, 948.0032f, 7083.063f), new Vec3f(-0.01985055f, -0.2240735f, 0.9743702f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_SHADOWFUR"), 1);
            mi.Spawn(mapName, new Vec3f(50093.45f, 1000.228f, 6873.844f), new Vec3f(0.3428714f, -0.2529643f, 0.9046814f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WARGFUR"), 1);
            mi.Spawn(mapName, new Vec3f(50001.34f, 985.8022f, 6909.229f), new Vec3f(0.9892225f, -0.01530364f, -0.1456171f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WARGFUR"), 1);
            mi.Spawn(mapName, new Vec3f(57458.36f, 1979.291f, -28471.99f), new Vec3f(0.311501f, -0.2973701f, 0.9025179f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(57517.13f, 1986.321f, -28567.17f), new Vec3f(0.05939118f, -0.1781481f, 0.9822097f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(57412.6f, 1960.099f, -28350.17f), new Vec3f(0f, -0.1736481f, 0.9848076f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(49981.4f, 956.3533f, 6999.854f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(50009.76f, 971.6387f, 6985.905f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(49975.64f, 974.6508f, 6955.394f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(49952.8f, 968.412f, 6983.493f), new Vec3f(-0.6427876f, 0f, 0.7660446f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_BROOM"), 1);
            mi.Spawn(mapName, new Vec3f(48384.99f, 5123.2f, 17481.76f), new Vec3f(-0.05169596f, 0.0348943f, 0.9980549f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_BROOM"), 1);
            mi.Spawn(mapName, new Vec3f(49494.41f, 5016.2f, 19183.64f), new Vec3f(-0.5215288f, 0.1805149f, -0.8339195f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(48454.94f, 5115.175f, 16943.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(48225.99f, 5116.217f, 17280.91f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(48293.79f, 5118.268f, 17186.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(47732.19f, 5118.957f, 16407.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(47684.52f, 5118.957f, 16501.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(47494.47f, 5120.957f, 16798.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(47250.78f, 5183.957f, 17205.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(47235.21f, 5123.957f, 17272f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(47228.12f, 5183.957f, 17276.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(47298.85f, 5123.957f, 17167.36f), new Vec3f(0f, 0f, 1f));

            //mi = new Item(ItemInstance.getItemInstance("HOLY_HAMMER_MIS"), 1);
            //mi.Spawn(mapName, new Vec3f(45998.67f, 4337.971f, 19613.93f), new Vec3f(-0.06974585f, -0.01745241f, 0.9974123f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_WEED"), 1);
            mi.Spawn(mapName, new Vec3f(48159.45f, 4897.333f, 18767.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_WEED"), 1);
            mi.Spawn(mapName, new Vec3f(47896.98f, 4899.075f, 18463.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_WEED"), 1);
            mi.Spawn(mapName, new Vec3f(48133.62f, 4897.237f, 18534.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_WEED"), 1);
            mi.Spawn(mapName, new Vec3f(48321.5f, 4906.333f, 18566.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_WEED"), 1);
            mi.Spawn(mapName, new Vec3f(48021.91f, 4903.333f, 18367.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_WEED"), 1);
            mi.Spawn(mapName, new Vec3f(48261.8f, 4898.09f, 18177.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_WEED"), 1);
            mi.Spawn(mapName, new Vec3f(48464.26f, 4903.06f, 18304.31f), new Vec3f(-0.0348995f, 0f, 0.9993908f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(50335.76f, 5102.075f, 20514.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(50258.02f, 5102.122f, 20648.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42354.86f, 4154.469f, 15208.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42507.32f, 4114.794f, 15299.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42509.07f, 4079.952f, 15049.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42204.59f, 4111.14f, 14863.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(42114.41f, 4157.832f, 15696.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(41839.77f, 4094.38f, 15852.66f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(41264.21f, 4137.364f, 15601.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHALICE"), 1);
            mi.Spawn(mapName, new Vec3f(50820.46f, 4345.399f, 18373.2f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(50861.61f, 4354.073f, 18302.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_APPLE"), 1);
            mi.Spawn(mapName, new Vec3f(50614.68f, 4370.885f, 19923.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_APPLE"), 1);
            mi.Spawn(mapName, new Vec3f(50521.02f, 4370.884f, 19870.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(50557.45f, 4308.662f, 19904.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(50559.68f, 4369.706f, 19900.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(50472.7f, 4373.916f, 19849.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(50535.75f, 4371.916f, 19201.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(50554.49f, 4371.916f, 19142.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(50579.09f, 4371.916f, 19065.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(50535.16f, 4309.916f, 19160.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(50590.43f, 4310.916f, 19061.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_MILK"), 1);
            mi.Spawn(mapName, new Vec3f(50929.98f, 4373.905f, 19993.23f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_MILK"), 1);
            mi.Spawn(mapName, new Vec3f(50627.36f, 4315.478f, 19936.51f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_FISH"), 1);
            mi.Spawn(mapName, new Vec3f(51052.11f, 4366.287f, 19995.76f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(50974.45f, 4368.058f, 19984.84f), new Vec3f(0.1736483f, 0f, -0.984808f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(50962.67f, 4308.061f, 19995.43f), new Vec3f(-0.7071074f, 0f, 0.7071066f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(50875.26f, 4345.397f, 18354.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(50830.08f, 4334.952f, 18335.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_INNOSSTATUE"), 1);
            mi.Spawn(mapName, new Vec3f(50838.51f, 4358.031f, 18406.74f), new Vec3f(0.6018154f, 0f, 0.7986356f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JEWELERYCHEST"), 1);
            mi.Spawn(mapName, new Vec3f(50898.47f, 4341.78f, 18314.31f), new Vec3f(0.9945229f, 0f, -0.1045296f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_ROCKCRYSTAL"), 1);
            mi.Spawn(mapName, new Vec3f(30063.61f, 5412.571f, -35781.51f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(34933f, 4690f, -36987f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(37208.79f, 4557.823f, -36951.42f), new Vec3f(0f, -0.999849f, -0.01745236f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(37138.73f, 4554.607f, -36843.01f), new Vec3f(0.6018153f, -0.7115904f, 0.3625735f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MACE_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(37176.6f, 4585.713f, -37068.06f), new Vec3f(0.1498078f, -0.5502191f, -0.8214747f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDRING"), 1);
            mi.Spawn(mapName, new Vec3f(37272.38f, 4555.545f, -36943.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(36436.24f, 4727.467f, -38333.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRU_LIGHTHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(31569.68f, 4609.338f, -41314.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_TRFGIANTBUG"), 1);
            mi.Spawn(mapName, new Vec3f(30790.54f, 4834.418f, -44069.64f), new Vec3f(0.8276765f, 0.04748918f, 0.5591927f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_02"), 1);
            mi.Spawn(mapName, new Vec3f(30501.32f, 4682.034f, -41386.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30456.23f, 4627.667f, -40830.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(31080.55f, 4620.079f, -41291.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(31931.16f, 4722.856f, -41729.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_AQUAMARINE"), 1);
            mi.Spawn(mapName, new Vec3f(31837.8f, 4294.659f, -38561.37f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRI_PROT_EDGE_01"), 1);
            mi.Spawn(mapName, new Vec3f(31681.33f, 4840.329f, -41929.5f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_02"), 1);
            mi.Spawn(mapName, new Vec3f(78961.48f, 4775.167f, 26381.55f), new Vec3f(0.8090169f, 7.295919E-10f, -0.5877854f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(78868.63f, 4789.509f, 26492.69f), new Vec3f(-0.9455186f, 6.198382E-09f, 0.325568f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(78952.13f, 4798.742f, 26517.69f), new Vec3f(-0.9902681f, 5.781266E-09f, 0.139173f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(78911.67f, 4792.766f, 26495.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(78907.89f, 4790.472f, 26486.24f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(80590.13f, 5010.083f, 26163.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JOINT"), 1);
            mi.Spawn(mapName, new Vec3f(80779.48f, 5090.898f, 26187.68f), new Vec3f(0.997564f, 0f, 0.06975655f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JOINT"), 1);
            mi.Spawn(mapName, new Vec3f(80751.87f, 5091.009f, 26205.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(80146.59f, 5045.867f, 26673.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(80081.52f, 5022.318f, 27500.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(80100.66f, 5060.117f, 27595.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(80850.69f, 5013.853f, 26988.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80949.82f, 5011.373f, 27242.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(80777.18f, 5032.55f, 27486.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(80673.19f, 5017.533f, 26658.07f), new Vec3f(-3.662623E-11f, -4.565263E-09f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 1);
            mi.Spawn(mapName, new Vec3f(80799.64f, 5014.49f, 26560.54f), new Vec3f(0.9925475f, -2.447928E-09f, -0.1218695f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(61452.49f, 6538.632f, 42737.39f), new Vec3f(0.9864997f, 0.08715572f, 0.1386435f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(61479.4f, 6543.468f, 42741.31f), new Vec3f(0.9281471f, 0.2134565f, -0.3049264f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(61460.77f, 6544.746f, 42710.59f), new Vec3f(0f, -0.3255683f, 0.9455194f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(61541.25f, 6541.727f, 42798.21f), new Vec3f(0.9659258f, 0f, 0.258819f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(61568.59f, 6552.44f, 42788.81f), new Vec3f(-0.5713934f, 0.08715574f, -0.8160352f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(45133.67f, 7667.625f, 30197.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(46202.08f, 7725.15f, 30313.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(45476.15f, 7766.772f, 31255.95f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(43166.5f, 7204.506f, 30836.17f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(42217.27f, 6880.531f, 30449.37f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(41177.63f, 6814.211f, 30960.26f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(40243.96f, 6648.505f, 29939.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(41159.86f, 6642.597f, 29607.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(40896.43f, 6618.073f, 29633.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(36233.88f, 5199.575f, 27424.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(36171.7f, 5216.68f, 27250.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(36594.27f, 5203.694f, 27794.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(36239.46f, 5204.578f, 27897.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(36149.98f, 5204.14f, 28204.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39142.12f, 6592.838f, 27461.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35576.13f, 6394.768f, 30090.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35566.29f, 6352.914f, 29775.66f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(37047.62f, 6982.646f, 32362.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(37416.64f, 6982.694f, 32419.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(37232.66f, 6991.094f, 32151.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(37885.09f, 7031.794f, 31957.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(37654.09f, 7006.024f, 32283.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(37529.12f, 7016.452f, 32774.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(36950.5f, 7031.854f, 32820.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(36539.36f, 7024.138f, 31959.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(37059.1f, 6984.136f, 31941.05f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(37532.45f, 6981.951f, 31793.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39819.67f, 6827.529f, 32100.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39871.24f, 6817.225f, 32695.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(40331.03f, 6906.208f, 33312.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39566.64f, 6861.832f, 33740.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38423.96f, 6845.198f, 33265.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(37287.98f, 6872.8f, 34065.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(36443.02f, 6977.983f, 34858.25f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(36268.88f, 6865.761f, 34325.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35380.88f, 6815.806f, 34409.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35200.16f, 6830.906f, 33874.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35770.48f, 6867f, 33624.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(36283.18f, 6899.768f, 31935.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35418.53f, 6647.192f, 31860.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35186.77f, 6534.106f, 31453.5f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(35975.52f, 6599.68f, 30938.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(36116.27f, 6604.857f, 30223.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(36417.22f, 6657.257f, 30366.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(37116.5f, 6745.104f, 30150.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(37914.52f, 6757.129f, 29653.51f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38296.05f, 6806.73f, 29787.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(38909.96f, 6790.876f, 29609.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(39363.48f, 6774.654f, 29722f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39210.76f, 6868.559f, 30325.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39241.78f, 6866.71f, 30586.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(39702.47f, 6826.182f, 31056.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(40397.82f, 6798.59f, 31134.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(43109.13f, 7196.832f, 30545.42f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(43285.98f, 7249.365f, 30795.91f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(43938.46f, 7710.424f, 29741.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(45322.66f, 7765.294f, 31406.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(46369.48f, 7790.543f, 32630.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(47322.09f, 7790.021f, 31907.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(46561.49f, 7767.553f, 31419.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(46193.82f, 7686.556f, 30094.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(47421.6f, 7751.821f, 28309.17f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(49499.94f, 7946.75f, 29266.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59674.71f, 6798.591f, 41379.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(60293.93f, 6723.504f, 41920.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(60766.88f, 6577.049f, 42616.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61436.84f, 6541.456f, 42784.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(60697.41f, 6585.673f, 42909.9f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(60007.59f, 6621.039f, 43463.52f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59408.66f, 6565.407f, 43694.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58957.54f, 6587.299f, 43694.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59401.43f, 6765.637f, 42344.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(59418.75f, 6835.836f, 40451.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59406.49f, 6833.078f, 40735.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58271.27f, 6877.035f, 40828.16f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(58016.1f, 7053.535f, 39725.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(58343.64f, 7278.127f, 37666.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(58145.85f, 7298.312f, 37910.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57430.18f, 7325.764f, 37692.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57145.76f, 7382.272f, 38078.06f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(53831.54f, 6939.163f, 29697.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(54676.87f, 6867.885f, 30273.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(56376.77f, 6669.392f, 30195.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(56882.34f, 6709.287f, 30262.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58448.1f, 6872.128f, 29326.91f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57720.5f, 6900.033f, 29119.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(57675.76f, 6788.989f, 30014.25f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59699.87f, 6836.932f, 29346.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(58348.95f, 6824.925f, 29736.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59232.73f, 6672.817f, 30405.47f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63395.5f, 6411.265f, 30823.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64305.95f, 6703.008f, 32962.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64504.34f, 6605.457f, 32602.52f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63981.53f, 6753.66f, 33585.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63822.53f, 6602.772f, 32913.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63390.13f, 6522.787f, 32071.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(63584.22f, 6502.044f, 32007.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(65383.74f, 6392.486f, 32105.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(65756.63f, 6690.307f, 33310.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(68689.63f, 6615.555f, 34535.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70718.37f, 5095.531f, 28479.66f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(71250.77f, 5142.184f, 28648.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72333.95f, 5159.153f, 26853.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72511.85f, 5160.166f, 25297.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72217.79f, 5174.465f, 24968.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(71514.55f, 5122.197f, 25312.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(71360.12f, 5075.665f, 25656.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72119.16f, 5179.42f, 25912.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(72124.33f, 5180.955f, 26146.06f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(72164.37f, 5186.698f, 25472.25f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(72953.78f, 5128.339f, 25485.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75397.64f, 4981.868f, 24037.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75405.23f, 4995.302f, 23637.98f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75114.98f, 5048.208f, 23753.99f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75334.6f, 5030.518f, 22796.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75801.95f, 4928.864f, 21403.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75007.46f, 5056.276f, 21403.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(74704.73f, 5058.204f, 22117.16f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(74947.98f, 5052.821f, 23026.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(74750.68f, 5057.929f, 22710.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(79662.16f, 4885.448f, 24245.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(80477.95f, 4978.305f, 24949.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(80077.95f, 4964.006f, 25233.54f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(79850.98f, 5038.171f, 26539.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(77957.63f, 5063.495f, 28185.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(77573.24f, 5072.419f, 29703.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(77934.26f, 5144.981f, 30545.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(77646.86f, 5142.519f, 30869.95f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(76369.91f, 5070.033f, 31524.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75808.98f, 5099.721f, 31982.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75420.52f, 5242.849f, 33462.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(73741.77f, 5201.522f, 33814.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(73901.34f, 5207.255f, 32883.12f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(75417.18f, 5158.522f, 32224.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73814.25f, 5191.375f, 32493.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72684.88f, 5104.772f, 32084.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73205.57f, 5196.339f, 32939.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(74061.2f, 5153.607f, 34373.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(74900.05f, 5188.048f, 34357.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(75657.64f, 5288.057f, 33365.77f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(75801.83f, 5217.045f, 32623.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(76258.74f, 5084.373f, 31745.11f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(77001.23f, 5338.763f, 32251.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(77398.07f, 5249.971f, 31568.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78701.64f, 5313.345f, 31446.85f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78512.97f, 5215.409f, 30839.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78981.14f, 5227.433f, 29806.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78219.24f, 5142.331f, 29837.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78284.83f, 5086.344f, 28598.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78539.71f, 5042.799f, 27739.62f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(79402.61f, 5257.035f, 28567.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(79343.14f, 5270.494f, 30523.24f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78583.95f, 5189.604f, 29958.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(77594.3f, 5104.11f, 30406.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(74370.91f, 5210.235f, 32383.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72869.62f, 5073.819f, 31700.16f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73243.3f, 5095.847f, 31988.31f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73555.66f, 5201.638f, 33040.23f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(74762.58f, 5172.075f, 33867.7f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(75395.13f, 5233.683f, 33039.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(76524.98f, 5069.371f, 31488.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78194.06f, 5238.688f, 31103.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78476.13f, 5199.382f, 30392.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(78338f, 5132.749f, 29478.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(77926.74f, 5086.1f, 29323.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(74271.44f, 5001.568f, 21583.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74903.76f, 6695.782f, 28354.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74804.66f, 6680.95f, 28505.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74668.97f, 6670.597f, 28492.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74740.93f, 6685.288f, 28404.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74778.8f, 6686.25f, 28424.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74552.48f, 6676.124f, 28614.15f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74435.87f, 6676.218f, 28895.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74495.09f, 6676.183f, 28977.2f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74470.5f, 6676.164f, 29009.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74611.13f, 6676.081f, 29048.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74662.52f, 6676.179f, 28614.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74481.59f, 6676.139f, 28669.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74581.03f, 6676.183f, 28783.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74506.8f, 6676.169f, 28806.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74496.03f, 6676.162f, 28782.82f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74588.03f, 6676.183f, 28905.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74420.59f, 6676.103f, 28986.05f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74293.3f, 6676.244f, 28932.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74296.52f, 6676.3f, 28802.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74458.91f, 6676.084f, 28615.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74812.23f, 6676.223f, 28812.5f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74681.52f, 6676.093f, 28913.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74760.3f, 6676.089f, 29022.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74625.82f, 6676.192f, 29047.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74667.4f, 6673.271f, 28541.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74447.46f, 6676.296f, 28657.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74399.35f, 6676.077f, 28682.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(74699.24f, 6676.135f, 28768.5f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70984.46f, 5120.973f, 28284.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(69195.19f, 5036.125f, 27218.98f), new Vec3f(-0.009708228f, 0.1557583f, 0.987748f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(69061.48f, 5091.669f, 27792.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68817.64f, 5059.096f, 27497.62f), new Vec3f(0f, 0.1391731f, 0.9902682f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(69764.08f, 5106.169f, 28272.99f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(69258.98f, 5077.146f, 27546.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68852.07f, 5123.632f, 28277.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(68377.07f, 5127.825f, 27645.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(69791.56f, 5981.791f, 29161.51f), new Vec3f(0.01211309f, 0.2598211f, 0.9655814f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(68924.08f, 6004.77f, 30356.33f), new Vec3f(-0.009986175f, 0.2578787f, 0.9661266f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(68130.47f, 5558.527f, 28965.47f), new Vec3f(0.03563118f, 0.1097171f, 0.9933246f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(67779.91f, 5117.528f, 27359.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(67675.06f, 5166.291f, 27525.03f), new Vec3f(7.615241E-05f, 0.1513832f, 0.9884756f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(66929.56f, 4991.621f, 26622.38f), new Vec3f(-0.8571678f, 0f, -0.5150378f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(67017.82f, 4982.804f, 26559.03f), new Vec3f(-0.8910067f, 0f, 0.4539906f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(66992.05f, 4964.286f, 26490.82f), new Vec3f(-0.8324777f, 0.388488f, 0.3950437f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(66957.63f, 4976.929f, 26553.97f), new Vec3f(-0.6483589f, 0.3052127f, -0.6974789f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(66967.54f, 4970.336f, 26517.81f), new Vec3f(-0.9702955f, 0f, 0.2419224f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68465.25f, 4643.853f, 26734.82f), new Vec3f(0f, 0.1736483f, 0.9848083f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68968.84f, 4590.442f, 26195.19f), new Vec3f(0f, 0.3255683f, 0.9455189f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68888.89f, 4644.33f, 26549.77f), new Vec3f(0.01212974f, 0.05317593f, 0.9985119f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68818.41f, 4632.218f, 26677.36f), new Vec3f(-0.00545949f, -0.104954f, 0.9944625f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68660.29f, 4602.874f, 26354.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68796.89f, 4597.286f, 26230.71f), new Vec3f(0.0003045865f, 0.1635683f, 0.9865324f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(69760.35f, 4422.538f, 25164.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(69788.98f, 4401.572f, 25109.45f), new Vec3f(0.9366246f, 0.2756389f, -0.2162888f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(69823.59f, 4414.064f, 25128.16f), new Vec3f(0.7547095f, 0f, -0.656059f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(71688.73f, 4518.077f, 22728.99f), new Vec3f(-0.2078535f, 0.5224816f, 0.8269289f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(71626.34f, 4510.769f, 22760.31f), new Vec3f(0.4514063f, 0.8853266f, 0.1114934f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(71533.82f, 4478.796f, 22842.5f), new Vec3f(-0.2943627f, 0.5103999f, 0.8079892f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(71435.88f, 4466.005f, 22930.56f), new Vec3f(-0.1405498f, 0.6890875f, 0.7109189f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(71828.48f, 4494.324f, 22647.09f), new Vec3f(-0.08354574f, -0.126658f, 0.9884233f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(71724.93f, 4505.287f, 22668.82f), new Vec3f(0.2983952f, 0.5792229f, 0.7585928f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(71928.65f, 4514.338f, 22222.16f), new Vec3f(0f, -0.08715577f, 0.9961951f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70649.01f, 4328.064f, 22697.55f), new Vec3f(0f, -0.1564345f, 0.9876888f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70884.41f, 4362.978f, 22910.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(70544.05f, 4334.995f, 22935.89f), new Vec3f(-0.008187151f, -0.07039602f, 0.9974859f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(71119.9f, 4530.903f, 20779.25f), new Vec3f(0f, -0.06975649f, 0.9975641f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(71800.41f, 4574.076f, 20914.47f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(72066.74f, 4580.008f, 21355.91f), new Vec3f(-0.007283765f, 0.0518267f, 0.9986302f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(71704.08f, 4526.313f, 21109.37f), new Vec3f(0.02984912f, -0.1712995f, 0.9847673f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(72011.05f, 4605.23f, 20778.06f), new Vec3f(-0.01485215f, -0.05323052f, 0.9984726f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(72421.91f, 4797.336f, 18620.52f), new Vec3f(-9.313226E-10f, 0.03489952f, 0.9993913f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73663.41f, 4761.765f, 19526.14f), new Vec3f(0f, 0.2079118f, 0.9781475f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73339.7f, 4895.752f, 18471.85f), new Vec3f(0f, -0.0348995f, 0.999391f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72641.88f, 4757.874f, 19034.5f), new Vec3f(0f, -1.862645E-09f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73221.34f, 4714.183f, 17915.07f), new Vec3f(0f, 0.1736483f, 0.9848084f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73110.27f, 4698.301f, 17807.24f), new Vec3f(0.02255758f, 0.07068768f, 0.9972447f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(73232.2f, 4686.21f, 17669.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(72840.09f, 4662.527f, 17427.92f), new Vec3f(0f, 0.1391732f, 0.9902686f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(71865.24f, 4437.826f, 16434.61f), new Vec3f(-0.005681951f, 0.06880673f, 0.997614f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(72131.58f, 4470.002f, 16591.04f), new Vec3f(0f, 0.08715578f, 0.9961951f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(72719.84f, 4353.203f, 15928.33f), new Vec3f(0.04494349f, 0.1620289f, 0.9857628f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(71190.58f, 4299.498f, 15048.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(71706.52f, 4286.798f, 15285.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(71270.46f, 4463.089f, 16416.08f), new Vec3f(0f, 0.1218694f, 0.9925467f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(70915.34f, 4456.637f, 16437.42f), new Vec3f(0f, 0.1391732f, 0.9902684f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(68580.14f, 4335.089f, 13704.94f), new Vec3f(0f, -0.3255682f, 0.945519f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(68298.28f, 4348.702f, 13834.65f), new Vec3f(0f, -0.190809f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(68332.99f, 4263.013f, 14315.13f), new Vec3f(0f, 0.01745242f, 0.9998481f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(68524.52f, 4253.2f, 14519.95f), new Vec3f(0f, 0.1218694f, 0.9925468f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(68496.27f, 4268.51f, 15023.26f), new Vec3f(-0.8290375f, 0f, 0.5591929f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(67684.84f, 4352.937f, 15541.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(67046.28f, 4541.972f, 15093.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66591.29f, 4575.458f, 15324.55f), new Vec3f(0f, -0.1564345f, 0.9876885f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66979.03f, 4554.558f, 16551.48f), new Vec3f(0f, 0.05233597f, 0.9986298f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66474.22f, 4369.063f, 16434.51f), new Vec3f(0.1826615f, -0.2861602f, 0.9406111f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66982.59f, 4533.949f, 16080.69f), new Vec3f(0f, 0.1218694f, 0.9925467f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67251.55f, 4590.422f, 16535.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(66893.95f, 4522.817f, 16824.67f), new Vec3f(-0.002739047f, -0.1086717f, 0.9940741f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(66602.02f, 4472.112f, 16084.87f), new Vec3f(-0.03274438f, 0.01598223f, 0.9993362f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(67012.56f, 4494.695f, 15843.61f), new Vec3f(0f, 0.06975651f, 0.9975645f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(67321.74f, 4604.73f, 16736.31f), new Vec3f(0f, -0.9961951f, 0.08715502f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(67335.07f, 4608.614f, 16703.74f), new Vec3f(0f, 0.2079118f, 0.9781483f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67287.42f, 4593.391f, 16631.23f), new Vec3f(-1.728535E-06f, -0.998632f, -0.05233672f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67309.81f, 4594.806f, 16615.7f), new Vec3f(0.3409994f, -0.9376489f, 0.06734674f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67324.71f, 4594.484f, 16625.98f), new Vec3f(0f, -0.999391f, 0.03489935f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67319.41f, 4593.906f, 16646.35f), new Vec3f(0f, 0.99863f, -0.05233687f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67282.87f, 4593.02f, 16623.41f), new Vec3f(0.05366012f, -0.9982083f, -0.02648336f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67278.28f, 4593.91f, 16647.65f), new Vec3f(3.278255E-07f, -0.8290379f, 0.5591926f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67292.43f, 4594.208f, 16624.65f), new Vec3f(0f, 0.99863f, 0.05233612f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67310.98f, 4594.439f, 16625.85f), new Vec3f(0f, 0.8987941f, -0.4383704f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67310.27f, 4594.251f, 16635.68f), new Vec3f(0f, 0.9902681f, 0.1391735f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(67265.23f, 4592.358f, 16625.5f), new Vec3f(0.183333f, 0.9706628f, 0.1555753f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_CROSSBOW_M_01"), 1);
            mi.Spawn(mapName, new Vec3f(67347.49f, 4646.105f, 16636.35f), new Vec3f(-0.9705079f, 0.001239538f, 0.241076f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(67217.85f, 4534.424f, 14628.39f), new Vec3f(-0.02984027f, -0.107453f, 0.9937629f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(67300.27f, 4500.067f, 13257.24f), new Vec3f(0f, -0.241922f, 0.9702966f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(66501.03f, 4650.174f, 13299.32f), new Vec3f(-0.0249985f, 0.05693464f, 0.9980653f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(66013.94f, 4780.313f, 10892.98f), new Vec3f(0.019945f, -0.188896f, 0.9817954f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(67009.65f, 4809.402f, 11740.47f), new Vec3f(0.002126914f, -0.1907906f, 0.9816292f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(66594.22f, 4768.056f, 12258.74f), new Vec3f(0.1984073f, -0.3090202f, 0.9301301f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(65871.66f, 4667.124f, 11541.64f), new Vec3f(0.07101982f, -0.1737692f, 0.9822227f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(66831.84f, 4841.116f, 11084.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67594.61f, 4890.56f, 11523.69f), new Vec3f(0f, -0.06975649f, 0.9975645f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67966.32f, 4929.011f, 11752.23f), new Vec3f(0.1675697f, -0.08886471f, 0.9818493f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67165.91f, 4780.703f, 12099.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66752.79f, 4894.28f, 10866.32f), new Vec3f(0f, -0.1218694f, 0.9925466f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66850.41f, 4829.01f, 11552.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66398.57f, 4793.087f, 11718.37f), new Vec3f(0.01812072f, 0.01934651f, 0.9996493f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66269.2f, 4864.818f, 10671.08f), new Vec3f(-0.01906459f, -0.1059905f, 0.9941853f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(65911.46f, 4713.59f, 11186.74f), new Vec3f(-7.615241E-05f, -0.219967f, 0.9755079f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66375.66f, 4827.743f, 11149.41f), new Vec3f(0f, -0.190809f, 0.9816275f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66927.34f, 4801.337f, 12051.02f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67109.44f, 4847.148f, 11478.03f), new Vec3f(0f, -0.08715576f, 0.9961946f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67350.1f, 4841.325f, 11729.38f), new Vec3f(2.793968E-09f, -0.1045284f, 0.9945222f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67705.85f, 4879.645f, 11749.25f), new Vec3f(0f, -0.08715573f, 0.9961952f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67474.76f, 4785.319f, 12178.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(63093.63f, 4892.899f, 11322.67f), new Vec3f(0.9101487f, -0.1908091f, -0.3677239f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(62899.53f, 4903.138f, 11261.12f), new Vec3f(7.615241E-05f, 0.04723004f, 0.9988843f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(63536.08f, 4858.247f, 10673.96f), new Vec3f(0f, -0.9993914f, -0.03489937f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(63477.05f, 4871.664f, 10731.56f), new Vec3f(0.07579914f, 0.9498035f, -0.3035283f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(63513.59f, 4869.068f, 10726.34f), new Vec3f(0f, -0.9975647f, -0.06975637f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(63913.39f, 4917.795f, 10289.05f), new Vec3f(0f, 0.978148f, 0.2079114f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(64058.24f, 4908.456f, 10281.22f), new Vec3f(0f, -0.9563056f, -0.2923748f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_02"), 1);
            mi.Spawn(mapName, new Vec3f(63962.25f, 4920.114f, 10266.46f), new Vec3f(0.3332549f, -0.8522688f, 0.4032168f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(64028.13f, 4903.88f, 10297.81f), new Vec3f(-0.3569336f, -0.9034216f, -0.2375573f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(63893.18f, 4858.648f, 10802.97f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64191.29f, 4910.648f, 10393.14f), new Vec3f(-0.008039603f, -0.2866599f, 0.9579995f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(64058.2f, 4767.55f, 11080.03f), new Vec3f(-0.3907312f, 0f, 0.9205054f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(62601.7f, 4499.253f, 13736.56f), new Vec3f(-0.03602067f, 0.03956961f, 0.9985679f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(63609.35f, 4700.2f, 12541.79f), new Vec3f(0f, 0.2419219f, 0.970296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(63425.43f, 4660.444f, 12578.57f), new Vec3f(0f, -0.1908091f, 0.9816278f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(62786.98f, 4368.947f, 15182.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(62684.09f, 4560.513f, 14205.79f), new Vec3f(-0.2364431f, 0.5117288f, 0.8259717f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(62709.94f, 4520.104f, 14036.85f), new Vec3f(-0.8188256f, -0.1245418f, 0.5603699f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(62550.81f, 4578.584f, 14023.58f), new Vec3f(0f, 0.8910066f, 0.4539905f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(62633.43f, 4587.584f, 14041.15f), new Vec3f(0f, 0.8910065f, 0.4539904f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(62529.45f, 4545.584f, 13939.99f), new Vec3f(0.02004355f, 0.8581038f, 0.5130848f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66116.69f, 3432.434f, 16671.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66233.96f, 3442.258f, 16499.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66223.94f, 3423.543f, 16809.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(68931.3f, 3289.107f, 14889.68f), new Vec3f(-0.9063078f, 0f, 0.4226182f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(68907.02f, 3282.272f, 14964.78f), new Vec3f(-0.9396927f, 0f, -0.3420197f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(68918.24f, 3290.781f, 14869.05f), new Vec3f(-0.8627301f, 0.08715573f, 0.4980972f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(69265.66f, 3278.294f, 17459.76f), new Vec3f(0f, 0.1736482f, 0.984808f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(65841.8f, 3144.813f, 18839.04f), new Vec3f(0f, -0.2249512f, 0.9743708f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(63360.11f, 3260.525f, 16236.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64337.57f, 3250.191f, 16582.66f), new Vec3f(0f, -0.1391732f, 0.9902686f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64537.94f, 3276.92f, 16501.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(64240.1f, 3223.969f, 16701.9f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(64139.87f, 3237.786f, 16629.23f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(64952.14f, 3291.884f, 17379.51f), new Vec3f(0f, 0.1564345f, 0.9876885f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(64984.91f, 3300.813f, 17355.53f), new Vec3f(0.1543678f, -0.1115379f, 0.981698f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(65285.64f, 3342.507f, 17533.67f), new Vec3f(0.2810456f, 0.2756374f, -0.9192595f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(65248.43f, 3329.951f, 17591.33f), new Vec3f(0.8305088f, 0.1391731f, -0.5393389f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(65230.87f, 3347.312f, 17525.99f), new Vec3f(0.6090178f, 0.1524251f, -0.7783725f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(65161.62f, 3342.416f, 17528.87f), new Vec3f(-0.6023055f, 0.3244226f, -0.7293688f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(65060.03f, 3308.685f, 17520.84f), new Vec3f(0.01677626f, 0.2756374f, -0.9611154f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(65050.1f, 3308.031f, 17481.47f), new Vec3f(0.7739733f, 0.358368f, -0.5220519f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(65185.09f, 3349.86f, 17511.81f), new Vec3f(-0.5010485f, 0.3255682f, -0.8018451f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(65327.69f, 3331.855f, 17575.57f), new Vec3f(0.7294909f, 0.190809f, -0.6568369f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(64427.55f, 3702.043f, 20143.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(64284.56f, 3677.738f, 20006.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(64425.82f, 3681.271f, 19931.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64531.75f, 3709.627f, 20122.48f), new Vec3f(-0.009619609f, 0.2065803f, 0.9783826f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(63473.85f, 3410.945f, 18852.17f), new Vec3f(0f, 0.1045285f, 0.994522f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(63418.18f, 3422.717f, 18879.63f), new Vec3f(0.981627f, -3.72529E-08f, -0.1908079f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(63446.7f, 3422.005f, 18882.24f), new Vec3f(-0.788011f, 0f, -0.6156613f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(61712.95f, 3261.433f, 18664.54f), new Vec3f(-0.06796709f, -0.9953773f, 0.06787756f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(61856.2f, 3254.928f, 18689.43f), new Vec3f(0.01736829f, -0.9974145f, 0.06974506f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(61817.98f, 3251.5f, 18589.08f), new Vec3f(0f, 0.9925464f, 0.1218694f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(61787.34f, 3260.896f, 18680.25f), new Vec3f(5.960464E-08f, -0.9902681f, 0.1391733f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(61839.46f, 3254.794f, 18589.31f), new Vec3f(0f, -0.9925464f, -0.1218695f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(61747.32f, 3258.531f, 18611.55f), new Vec3f(0f, 0.08715574f, 0.9961948f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(61775.56f, 3259.646f, 18657.2f), new Vec3f(-0.8090171f, 0f, 0.5877851f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(60651.9f, 3376.879f, 18098.22f), new Vec3f(0.173648f, 0f, -0.9848077f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(60207.03f, 3346.044f, 17957.95f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59369.47f, 3308.54f, 18168.75f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59456.96f, 3292.82f, 17868.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59625.6f, 3334.303f, 18236.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(59459.14f, 3317.246f, 18463.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(57608.97f, 3239.731f, 17915.63f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(57985.26f, 3243.494f, 18072.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(57619.73f, 3338.85f, 16899.85f), new Vec3f(0.7689258f, 0.3195865f, -0.5537305f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(57644.2f, 3338.525f, 16925.85f), new Vec3f(-5.902459E-08f, 0.1391731f, -0.9902681f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(56132.26f, 3259.28f, 15822.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(56187.54f, 3257.13f, 16080.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(55985.19f, 3257.135f, 16274.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(60807.61f, 3701.993f, 13040.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61219.74f, 3770.309f, 13161.42f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61434.3f, 3789.921f, 12898.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61301.37f, 3759.468f, 12480.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(60814.64f, 3959.214f, 10649.47f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(60658.73f, 3886.42f, 10568.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61039.21f, 4138.332f, 10220.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(61070.23f, 4094.89f, 10479.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(60414.63f, 3606.54f, 11125.19f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(59113.34f, 3134.759f, 12075.05f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(59123.81f, 3108.492f, 11850.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(56353.45f, 3473.759f, 12751.7f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(55928.74f, 3561.226f, 12996.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(57231.04f, 3621.719f, 13216.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(56593.47f, 3605.682f, 13123.42f), new Vec3f(0.01340626f, 0.2920642f, 0.956305f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(55373.39f, 3687.111f, 13427.09f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(53751f, 3666.108f, 13519.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(53435.02f, 3625.597f, 13060.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(61642.05f, 3499.626f, 15887.8f), new Vec3f(-0.1564345f, 0f, -0.9876881f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(61623.79f, 3419.626f, 15904.04f), new Vec3f(-0.4553227f, 0.04434772f, -0.8892218f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(61614.83f, 3427.626f, 15870.29f), new Vec3f(0.0348994f, 0f, -0.999391f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(61614.49f, 3411.626f, 15876.09f), new Vec3f(0.03489939f, 0f, -0.9993905f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(61613.78f, 3424.626f, 15865.36f), new Vec3f(0.1218694f, 0f, -0.9925459f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(61543.7f, 3395.587f, 15758.41f), new Vec3f(-0.5727902f, -0.05233596f, -0.8180293f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SAW"), 1);
            mi.Spawn(mapName, new Vec3f(61652.21f, 3267.64f, 15864.08f), new Vec3f(-0.9706149f, 0.1770735f, -0.1629523f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(61776.5f, 3306.658f, 16089.44f), new Vec3f(0.1580055f, -0.4367271f, 0.8856099f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(61642.25f, 3418.127f, 15880.78f), new Vec3f(-0.0154485f, 0.1566213f, -0.9875378f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(62228.76f, 3224.689f, 15864.03f), new Vec3f(0.9311543f, -0.1736483f, 0.3206217f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(62163.93f, 3249.84f, 15796f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(62045.91f, 3278.515f, 15755.06f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_SLEEP"), 1);
            mi.Spawn(mapName, new Vec3f(62230.82f, 3281.443f, 15536.32f), new Vec3f(0.7431449f, 0f, 0.6691306f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_MEDIUMHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(62213.21f, 3291.272f, 15507.97f), new Vec3f(-0.7986356f, 0f, 0.6018149f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_LIGHT"), 1);
            mi.Spawn(mapName, new Vec3f(62231.02f, 3290.59f, 15525.8f), new Vec3f(0.4673635f, -0.04444097f, 0.8829477f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FULLHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(62214.54f, 3293.55f, 15499.41f), new Vec3f(0.9743701f, 0f, 0.2249509f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_ICEBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(62213.46f, 3284.699f, 15530.39f), new Vec3f(-0.6946585f, 0f, 0.7193397f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(62146.88f, 3285.316f, 15536.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(62385.75f, 3230.599f, 15800f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(62043.75f, 3213.868f, 15935.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(62176.18f, 3245.064f, 15927.51f), new Vec3f(-0.9986302f, 0f, 0.05233602f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(62149.85f, 3283.231f, 15905.78f), new Vec3f(-0.9945227f, 1.040244E-09f, 0.1045285f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(66747.81f, 6900.937f, 43919.29f), new Vec3f(0.2366354f, 0.9428729f, -0.2345157f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_ZWEIHAENDER1"), 1);
            mi.Spawn(mapName, new Vec3f(66745.91f, 6963.522f, 43798.03f), new Vec3f(-0.9294146f, -0.1222785f, -0.3481931f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(64020.44f, 6885.357f, 43383.87f), new Vec3f(-0.2249512f, -0.1187459f, -0.9671076f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(63995.22f, 6906.426f, 43349.16f), new Vec3f(0f, 0.190809f, 0.9816274f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(64480.82f, 6876.271f, 43815.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(64359.26f, 6880.261f, 43782.32f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(64264.43f, 6886.503f, 43845.92f), new Vec3f(-0.5877855f, 0.2365338f, 0.7736684f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_03"), 1);
            mi.Spawn(mapName, new Vec3f(64086.21f, 6890.269f, 43226.22f), new Vec3f(1.490116E-08f, -0.3907315f, 0.9205046f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(64071.82f, 6884.841f, 43276.92f), new Vec3f(0f, -0.9925472f, 0.1218696f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(64054.95f, 6882.118f, 43340.35f), new Vec3f(-0.04338853f, -0.1961272f, 0.9796185f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(64068.34f, 6873.495f, 43333.57f), new Vec3f(0f, -0.939693f, -0.3420238f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_SLD_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(64557.54f, 6869.635f, 43760.15f), new Vec3f(-0.9455189f, 0f, 0.325568f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_SLD_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(64413.61f, 6868.849f, 43764.53f), new Vec3f(0.9396934f, 0f, -0.3420235f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(62679.04f, 6563.022f, 45338.71f), new Vec3f(0.9612623f, 0f, 0.275637f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(62564.77f, 6557.29f, 44973.28f), new Vec3f(0.4226186f, 0f, -0.9063081f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(62624.18f, 6577.349f, 45006f), new Vec3f(4.190952E-09f, 0.1045285f, 0.9945222f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(62589.62f, 6548.13f, 45015.46f), new Vec3f(0f, 0.05233596f, 0.9986296f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(62589.61f, 6561.563f, 44985.69f), new Vec3f(0.7431448f, 0f, 0.6691306f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_ORCAXE_02"), 1);
            mi.Spawn(mapName, new Vec3f(66564.87f, 3223.602f, 13595.67f), new Vec3f(-0.9063089f, 0f, -0.4226191f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(66858.89f, 3213.645f, 12975.57f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66884.77f, 3224.189f, 12978.17f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(66840.44f, 3218.144f, 12930.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(66585.03f, 3235.177f, 13727.62f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(66195.31f, 3238.449f, 13602.8f), new Vec3f(0.8571678f, 0f, 0.5150378f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(66220.41f, 3231.373f, 13682.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(66459.02f, 3241.292f, 13015.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(66528.94f, 3226.141f, 12886.52f), new Vec3f(0f, -0.9659268f, -0.2588193f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_03"), 1);
            mi.Spawn(mapName, new Vec3f(66880.68f, 3226.13f, 12941.16f), new Vec3f(0.2090359f, -0.3770223f, 0.9023105f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(66587.87f, 3225.48f, 13396f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET100"), 1);
            mi.Spawn(mapName, new Vec3f(66384.48f, 3236.05f, 13628.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(61316.96f, 7075.878f, 23304.45f), new Vec3f(0.9781489f, 0f, -0.2079118f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTON"), 1);
            mi.Spawn(mapName, new Vec3f(61334.82f, 7077.434f, 23247.05f), new Vec3f(-0.5446402f, 0f, 0.8386705f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(61312.12f, 7080.929f, 23284.52f), new Vec3f(-0.9455194f, 0f, 0.3255674f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(61345.16f, 7096.504f, 23502.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(46998.73f, 7858.146f, 35812.7f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET100"), 1);
            mi.Spawn(mapName, new Vec3f(46579.25f, 7881.427f, 35549.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(46534.55f, 7878.907f, 35500.64f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(46623.05f, 7879.724f, 35609.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHALICE"), 1);
            mi.Spawn(mapName, new Vec3f(46549.42f, 7861.489f, 35130.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHEST"), 1);
            mi.Spawn(mapName, new Vec3f(46369.71f, 7860.035f, 35137.19f), new Vec3f(-0.4860906f, -0.1029405f, -0.867821f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCANDLEHOLDER"), 1);
            mi.Spawn(mapName, new Vec3f(46512f, 7873f, 35051f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHALICE"), 1);
            mi.Spawn(mapName, new Vec3f(46056.41f, 7862.54f, 35538.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(46331f, 7834f, 35967f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_2H_SLD_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(46527f, 7870f, 35098f), new Vec3f(-0.00698126f, 0f, 0.9999756f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MIL_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(46115f, 7850f, 35503f), new Vec3f(-0.9890161f, 6.727092E-09f, -0.1478048f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MIL_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(46393f, 7842f, 35952f), new Vec3f(0.3040316f, 0f, -0.9526568f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MIL_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(46943f, 7863f, 35832f), new Vec3f(0.7703963f, 0.01344733f, 0.6374242f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERPLATE"), 1);
            mi.Spawn(mapName, new Vec3f(46205.29f, 7775.89f, 33605.13f), new Vec3f(3.100119E-10f, -0.1391733f, -0.9902695f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(46217.72f, 7805.866f, 33630.15f), new Vec3f(0.9240499f, -0.1034166f, 0.3680311f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(46359.55f, 7842.185f, 33078.68f), new Vec3f(0.6046861f, 0.7951986f, 0.04496526f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(46203.76f, 7833.384f, 33941.87f), new Vec3f(-1.000002f, -3.225644E-08f, -1.749676E-08f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(45919.48f, 7831.409f, 33544.35f), new Vec3f(0.148631f, 0.6944425f, -0.7040405f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCUP"), 1);
            mi.Spawn(mapName, new Vec3f(46183.77f, 7775.676f, 33910.94f), new Vec3f(0f, 0.9902695f, 0.1391734f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHEST"), 1);
            mi.Spawn(mapName, new Vec3f(70217.09f, 4423.616f, 22259.03f), new Vec3f(0.8660251f, 0f, 0.5000001f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_HONEY"), 1);
            mi.Spawn(mapName, new Vec3f(70325.94f, 4417.617f, 22170.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(36396.83f, 5273.866f, 29427.14f), new Vec3f(-0.9938688f, -0.1073913f, -0.02639564f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_CHEESE"), 1);
            mi.Spawn(mapName, new Vec3f(36331.27f, 5217.319f, 29463.06f), new Vec3f(0.8571678f, 0f, -0.515038f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BACON"), 1);
            mi.Spawn(mapName, new Vec3f(36348.96f, 5212.804f, 29413.44f), new Vec3f(0.4397493f, -0.1288322f, 0.8888329f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(36344.03f, 5213.614f, 29402.53f), new Vec3f(-0.3420205f, 0f, -0.9396946f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(36332.83f, 5216.361f, 29377.04f), new Vec3f(0.3746067f, 0f, 0.9271843f));

            mi = new Item(ItemInstance.getItemInstance("ITFOMUTTONRAW"), 1);
            mi.Spawn(mapName, new Vec3f(36343.44f, 5218.008f, 29405.14f), new Vec3f(0.5556156f, 0.1333185f, 0.8206823f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(35928.2f, 5292.6f, 30885.43f), new Vec3f(0.738508f, 0.5318117f, 0.41447f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(35879.38f, 5262.915f, 30909.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOLT"), 1);
            mi.Spawn(mapName, new Vec3f(35901.21f, 5282.209f, 30936.92f), new Vec3f(0.301116f, 0.3850063f, -0.8724124f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(35953.07f, 5257.321f, 30837.35f), new Vec3f(-0.8664514f, -0.1715103f, 0.4688781f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WINE"), 1);
            mi.Spawn(mapName, new Vec3f(35922.93f, 5273.313f, 30894.9f), new Vec3f(0.9293679f, -0.3690926f, -0.007271924f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_DRACHENSCHNEIDE"), 1);
            mi.Spawn(mapName, new Vec3f(46698f, 7916f, 35463.89f), new Vec3f(0.6549259f, 0.003590923f, -0.7556722f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(79157.8f, 5520.963f, 33900.09f), new Vec3f(-0.7313542f, 0f, 0.6819983f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(79184.45f, 5514.04f, 33925.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(79168.76f, 5530.12f, 33921.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(79168.02f, 5530.054f, 33876.33f), new Vec3f(0.8571644f, 0.3065615f, 0.4138732f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BREAD"), 1);
            mi.Spawn(mapName, new Vec3f(79175.06f, 5537.566f, 33894.3f), new Vec3f(0.1851413f, -0.241922f, 0.9524688f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(80522.91f, 5476.206f, 34116.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(80484.8f, 5479.211f, 33995.61f), new Vec3f(-0.9563047f, 0f, -0.292372f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(80579.62f, 5474.188f, 34065.92f), new Vec3f(0.6946588f, 0f, 0.7193397f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(80476.48f, 5451.559f, 34485.11f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(80412.88f, 5464.154f, 34510.54f), new Vec3f(0.6946586f, 0f, -0.7193398f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(80444.28f, 5447.123f, 34534.59f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(80362.54f, 5480.359f, 34530.42f), new Vec3f(0.4226184f, 0f, 0.9063081f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(81337.73f, 5454.354f, 34699.53f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(81344.07f, 5455.357f, 34726.4f), new Vec3f(0.9743702f, 0f, 0.2249512f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(80725.95f, 5292.155f, 33135.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_COMMON_01"), 1);
            mi.Spawn(mapName, new Vec3f(80839.02f, 5278.729f, 33128.16f), new Vec3f(-0.9702969f, 0f, 0.2419206f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_SLD_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(80864.13f, 5277.711f, 33264.59f), new Vec3f(0.6755958f, 0.09667254f, 0.7309083f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(80891.53f, 5282.288f, 33322.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(80796.45f, 5280.681f, 33140.44f), new Vec3f(0.1218694f, 0.9774683f, -0.172354f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(80839.9f, 5294.987f, 32761.91f), new Vec3f(0.7986355f, 0f, -0.6018152f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(80859.92f, 5294.973f, 32827.95f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(80803.8f, 5292.158f, 32813.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(80970.31f, 5279.392f, 33265.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(81666.18f, 5291.198f, 32818.51f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(81275.16f, 5291.135f, 32528.31f), new Vec3f(-0.9135461f, 0f, 0.4067355f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCUP"), 1);
            mi.Spawn(mapName, new Vec3f(81829.66f, 5310.119f, 33173.04f), new Vec3f(0.5365315f, -0.8405796f, 0.07554266f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_SILVERCHALICE"), 1);
            mi.Spawn(mapName, new Vec3f(84726.07f, 5563.55f, 33644.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_JEWELERYCHEST"), 1);
            mi.Spawn(mapName, new Vec3f(84652.59f, 5462.64f, 33770.89f), new Vec3f(0.9396935f, 0f, 0.3420189f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLDCHEST"), 1);
            mi.Spawn(mapName, new Vec3f(84662.77f, 5467.8f, 33811.43f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_ICECUBE"), 1);
            mi.Spawn(mapName, new Vec3f(64001.59f, 4933.431f, 10231.65f), new Vec3f(0f, -0.3420202f, 0.9396928f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_02"), 1);
            mi.Spawn(mapName, new Vec3f(64183.11f, 5081.642f, 9510.904f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_QUARTZ"), 1);
            mi.Spawn(mapName, new Vec3f(63701.59f, 4988.49f, 10239.87f), new Vec3f(0.913546f, 0f, -0.4067391f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_QUARTZ"), 1);
            mi.Spawn(mapName, new Vec3f(63685.31f, 4981.83f, 10245.28f), new Vec3f(5.587935E-09f, -0.1564344f, 0.9876888f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_QUARTZ"), 1);
            mi.Spawn(mapName, new Vec3f(63708.78f, 4979.392f, 10248.57f), new Vec3f(0.01144982f, -0.6559595f, 0.7547102f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(63438.01f, 4991.525f, 10248.58f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(61964.07f, 3253.423f, 15815.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(62426.09f, 3231.879f, 15938.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(61365.23f, 3288.781f, 16015.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63128.7f, 3190.459f, 16724.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(63515.63f, 3188.245f, 16858.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(62947.02f, 3168.131f, 16730.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64137.9f, 3165.688f, 17335.23f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64138.88f, 3154.99f, 17793.28f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64329.42f, 3142.235f, 18335.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64239.99f, 3157.377f, 17974.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(64577.46f, 3160.911f, 18466.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(66813.95f, 3162.998f, 18056.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(67674.91f, 3162.436f, 17412.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(69725.17f, 3212.203f, 16385.11f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(48011.14f, 7951.358f, 39802.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_FOLTERAXT"), 1);
            mi.Spawn(mapName, new Vec3f(47989.66f, 8000.306f, 39781.09f), new Vec3f(-0.6427875f, 0.7544063f, 0.1330222f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_03"), 1);
            mi.Spawn(mapName, new Vec3f(48018.28f, 7954.055f, 39746.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(50265.69f, 7782.949f, 36372.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(51632.03f, 7827.736f, 37466.51f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(50863.89f, 7780.212f, 37183.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(50317.37f, 7781.877f, 34019.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PERM_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(66864.51f, 6914.51f, 43724.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(66018.47f, 6939.245f, 43597.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET100"), 1);
            mi.Spawn(mapName, new Vec3f(64442.07f, 6885.775f, 43010.68f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(59662.73f, 6851.523f, 40418.56f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_LIGHTHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(59742.32f, 6884.429f, 40607.46f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_KRIEGSKEULE"), 1);
            mi.Spawn(mapName, new Vec3f(55469.63f, 7734.997f, 37200.91f), new Vec3f(-8.940697E-08f, 0.6427872f, 0.7660443f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(57690.34f, 3341.168f, 16996.45f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(78859.69f, 5105.882f, 22361.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(79727.56f, 5335.374f, 31052.87f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_SCHWERT2"), 1);
            mi.Spawn(mapName, new Vec3f(64003.21f, 4997.58f, 10241.66f), new Vec3f(0.9708713f, -0.1561588f, -0.1817216f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(70419.85f, 3255.172f, 14822.28f), new Vec3f(-0.3420201f, -8.64904E-08f, 0.9396924f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(38694.05f, 6983.88f, 34394.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_TEMP_HERB"), 1);
            mi.Spawn(mapName, new Vec3f(34358.12f, 6763.561f, 33483.94f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_ZWEIHAENDER2"), 1);
            mi.Spawn(mapName, new Vec3f(35889.96f, 5490.744f, 32870.28f), new Vec3f(-0.807766f, 0.05028389f, -0.5873535f));

            mi = new Item(ItemInstance.getItemInstance("ITRI_PROT_POINT_02"), 1);
            mi.Spawn(mapName, new Vec3f(48070.2f, 7958.797f, 39493.4f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(30483.42f, 4331.384f, -16012.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_DEX_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30693.56f, 4332.002f, -15510.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(30544.75f, 4334.033f, -16003.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(30418.63f, 4324.156f, -16082.12f), new Vec3f(-0.6156618f, 0f, 0.7880106f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_STRENGTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30612.21f, 4326.447f, -15931.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_SPEED_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30685.48f, 4324.94f, -15759.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_PLANEBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(30464.56f, 4330.899f, -15351.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(30514.26f, 4340.831f, -15389.84f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(30660.28f, 4326.706f, -15442f), new Vec3f(-0.4067367f, 0f, 0.9135458f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30430.67f, 4323.918f, -15379.72f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(30575.25f, 4324.362f, -15341.51f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(30372.23f, 4326.005f, -15374.79f), new Vec3f(0.309017f, 0f, 0.9510565f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(30350.47f, 4332.189f, -15355.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(30308.64f, 4320.139f, -15369.72f), new Vec3f(-0.6560612f, 0f, 0.7547098f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(30234.81f, 4327.238f, -14442.09f), new Vec3f(0f, -0.06975648f, 0.9975641f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(29278.92f, 4254.154f, -15465.61f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(29246.8f, 4254.13f, -15630.11f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(29842.97f, 4254.063f, -16570.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(29752.78f, 4254.106f, -16316.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(29645.46f, 4382.873f, -14513.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_FLASK"), 1);
            mi.Spawn(mapName, new Vec3f(29637.64f, 4333.958f, -14565.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(29657.19f, 4370.35f, -14567.44f), new Vec3f(0.9396935f, 0f, 0.3420193f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(29781.65f, 4328.32f, -14228.07f), new Vec3f(-0.6560593f, 0f, -0.75471f));

            mi = new Item(ItemInstance.getItemInstance("ITLSTORCH"), 1);
            mi.Spawn(mapName, new Vec3f(29792.4f, 4330.182f, -14240.38f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_WATER"), 1);
            mi.Spawn(mapName, new Vec3f(29955.95f, 4340.206f, -14326.8f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(23421.32f, 2814.482f, -13094.39f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(23308.96f, 2823.794f, -12747.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(23229.67f, 2823.35f, -12877.24f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(22501.29f, 3177.774f, -10762.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(22554.67f, 3156.988f, -11043.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_FORESTBERRY"), 1);
            mi.Spawn(mapName, new Vec3f(22447.93f, 3172.634f, -10921.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(22621.19f, 3136.008f, -11212.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(22319.64f, 3067.26f, -11423.2f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(21565.36f, 3557.984f, -12502.27f), new Vec3f(0.01816552f, -0.2418481f, 0.970144f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIRESTORM"), 1);
            mi.Spawn(mapName, new Vec3f(21304.13f, 3487.423f, -12285.64f), new Vec3f(0.03366903f, -0.2715436f, 0.9618372f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(21453.15f, 3519.787f, -12321.77f), new Vec3f(0.3290285f, 0.1905476f, -0.9248953f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(21509.8f, 3528.01f, -12369.01f), new Vec3f(0.7547094f, 0f, 0.656059f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(21543.66f, 3530.75f, -12376.45f), new Vec3f(0.9396925f, 0f, 0.3420202f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(21480.33f, 3522.772f, -12323.37f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32194.3f, 4391.501f, -17967.82f), new Vec3f(0.9392571f, -0.108633f, 0.325568f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32142.98f, 4434.019f, -18121.57f), new Vec3f(0.9927726f, -0.1148225f, -0.03489948f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32144.97f, 4422.579f, -18072.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29448.54f, 4405.359f, -16770.2f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29468.9f, 4415.82f, -16978.58f), new Vec3f(0.7188811f, 0.4319474f, 0.5446391f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29445.69f, 4400.163f, -16653.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29414.29f, 4383.264f, -16839.86f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_LIGHT"), 1);
            mi.Spawn(mapName, new Vec3f(30901.9f, 4565.801f, -13676.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(30921.97f, 4557.447f, -13608.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(30921.53f, 4554.312f, -13593.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(29874.09f, 5263.525f, -16630.78f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_SWORD"), 1);
            mi.Spawn(mapName, new Vec3f(26030.94f, 1016.752f, -8911.995f), new Vec3f(-0.9986295f, 0.05154083f, 0.009088032f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET50"), 1);
            mi.Spawn(mapName, new Vec3f(23887.21f, 988.2366f, -9400.006f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(30409.2f, 2156.483f, -18137.4f), new Vec3f(-0.08309749f, 0.9138828f, -0.3973836f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_LIGHTHEAL"), 1);
            mi.Spawn(mapName, new Vec3f(30462.95f, 2157.274f, -18112.84f), new Vec3f(-0.4329742f, -0.1564345f, 0.8877283f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(33137.54f, 3369.543f, -18277.65f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(33140.59f, 3371.141f, -18309.98f), new Vec3f(0f, 0.9702972f, -0.2419221f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(32682.85f, 3379.044f, -17973.22f), new Vec3f(0.2249511f, 0f, 0.9743704f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_02"), 1);
            mi.Spawn(mapName, new Vec3f(32574.89f, 3381.096f, -17953.3f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(13756.6f, 1535.968f, -23707.71f), new Vec3f(0f, -0.0348995f, 0.999391f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(13644.15f, 1532.383f, -23725.48f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(17488.41f, 2040.6f, -23000.36f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(17544.36f, 2045.176f, -23048.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(25762.84f, 3192.682f, -22102.08f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(27894.16f, 3716.977f, -19991.79f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(27671.54f, 3655.883f, -19987.49f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(27654.32f, 2324.366f, -18893.7f), new Vec3f(0f, -0.1391732f, 0.9902684f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29473.18f, 2038.429f, -18425.88f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29560.21f, 2031.601f, -18343.62f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29854.55f, 1305.098f, -16788.93f), new Vec3f(5.096497E-09f, -0.34202f, 0.9396923f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(29854.52f, 1313.551f, -16842.13f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(26773.49f, 420.6486f, -18295.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26533.16f, 365.3037f, -18226.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(26549.02f, 356.4632f, -18303.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(26646.68f, 375.4681f, -18278.03f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(23185.87f, 991.9573f, -10933.6f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(23782f, 968.3682f, -9147.419f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(25994.29f, 983.0508f, -9077.035f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(26078.35f, 978.2716f, -8968.583f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_03"), 1);
            mi.Spawn(mapName, new Vec3f(32667.53f, 4232.058f, -21580.44f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31710.32f, 4074.376f, -21496.93f), new Vec3f(0f, 0.2249511f, 0.9743704f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(31609.02f, 4059.129f, -21554.74f), new Vec3f(0f, 0.1391732f, 0.9902682f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(30683.51f, 6033.837f, -14404.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(30408.4f, 5275.045f, -15381.92f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_BAU_MACE"), 1);
            mi.Spawn(mapName, new Vec3f(29905.21f, 5193.678f, -14387.41f), new Vec3f(-0.9932433f, 0.03404196f, -0.1109678f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26052.3f, 969.386f, -9037.736f), new Vec3f(-0.3746066f, 0f, 0.9271839f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(26002.47f, 969.3627f, -8963.468f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(19333.26f, 3179.669f, -20425.14f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(19752.34f, 3178.744f, -20374.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(17490.96f, 3176.986f, -21338.67f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(17497.38f, 3178.986f, -21220.07f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMI_GOLD"), 1);
            mi.Spawn(mapName, new Vec3f(21470.45f, 2784.757f, -23612.69f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER"), 1);
            mi.Spawn(mapName, new Vec3f(30535.63f, 4479.267f, -19561.66f), new Vec3f(0.3239449f, -0.1921196f, -0.9263632f));

            mi = new Item(ItemInstance.getItemInstance("ITFO_BOOZE"), 1);
            mi.Spawn(mapName, new Vec3f(30635.83f, 4470.931f, -19490.49f), new Vec3f(0.1736482f, 0f, 0.9848078f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(24637.41f, 3241.906f, -19493.96f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(24363.28f, 3234.12f, -19741.38f), new Vec3f(0f, -1.040244E-09f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(25055.13f, 3210.65f, -19451.73f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(25300.76f, 3203.663f, -19583.35f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(15021.78f, 2128.732f, -24036.99f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(15178.63f, 2145.284f, -23937.83f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITMW_1H_MISC_AXE"), 1);
            mi.Spawn(mapName, new Vec3f(24592.71f, 3222.969f, -21999.88f), new Vec3f(0.1842287f, 0.9658845f, -0.1820184f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(31548.43f, 3889.953f, -20253.59f), new Vec3f(0.2249511f, 0f, 0.9743698f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(32018.81f, 3889.005f, -20133.01f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(31991.25f, 3887.083f, -20294.64f), new Vec3f(0.4383713f, 0f, 0.8987941f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(31335.08f, 3888.962f, -20390.6f), new Vec3f(0.358368f, 0f, 0.9335808f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(34287.98f, 3666.813f, -19623.21f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(32235.66f, 3348.454f, -19936.71f), new Vec3f(0.8290382f, 0f, -0.5591931f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(32018.67f, 3350.33f, -19972.98f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29792.7f, 2874.94f, -18564.51f), new Vec3f(0f, -0.1045284f, 0.9945217f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(27592.92f, 2314.813f, -18870.7f), new Vec3f(3.72529E-09f, -0.2588191f, 0.9659257f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(29673.69f, 2002.802f, -18163.66f), new Vec3f(1.862645E-09f, -0.3255683f, 0.945519f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(28100.29f, 485.0993f, -15011.9f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(28415.56f, 669.7646f, -14365.1f), new Vec3f(-0.008501178f, 0.1215726f, 0.9925464f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(27624.08f, 705.2675f, -13699.9f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(25206.15f, 807.7474f, -12514.41f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24239.89f, 855.2211f, -11704.29f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(23470.53f, 998.9746f, -10776.1f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(23496.67f, 1010.619f, -10664.71f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(24168.1f, 983.4329f, -9084.914f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(26377.28f, 1008.865f, -8860.797f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(27553.1f, 685.0067f, -13940.61f), new Vec3f(0f, -0.0348995f, 0.9993907f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(31248.26f, 4477.278f, -19139.36f), new Vec3f(-0.9205049f, 0f, 0.3907309f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(26045.39f, 3264.809f, -22049.33f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(20742.19f, 2901.508f, -22393.24f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(26060.79f, 3288.858f, -21373.18f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MUSHROOM_01"), 1);
            mi.Spawn(mapName, new Vec3f(19646.46f, 2470.358f, -23573.64f), new Vec3f(-0.1736482f, 0f, -0.9848073f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(25944.53f, 969.3705f, -9031.324f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(28104.51f, 574.183f, -18235.81f), new Vec3f(0f, 0.9848091f, -0.1736581f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(24476.34f, 3246.05f, -19982.81f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(30372.3f, 2159.573f, -18108.54f), new Vec3f(0f, 0.933581f, -0.3583679f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(33138.66f, 3366.792f, -18338.75f), new Vec3f(0.06975659f, -0.05220858f, 0.9961981f));

            mi = new Item(ItemInstance.getItemInstance("ITSE_GOLDPOCKET25"), 1);
            mi.Spawn(mapName, new Vec3f(24486.28f, 3260.472f, -19996.27f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_02"), 1);
            mi.Spawn(mapName, new Vec3f(24204.27f, 1000.751f, -10690.34f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_HEALTH_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24047.71f, 978.7504f, -10767.93f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_MANA_HERB_01"), 1);
            mi.Spawn(mapName, new Vec3f(24301.58f, 979.6844f, -10832.35f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_MANA_01"), 1);
            mi.Spawn(mapName, new Vec3f(33137.15f, 4139.094f, -23738.04f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPO_HEALTH_01"), 1);
            mi.Spawn(mapName, new Vec3f(33178.32f, 4139.38f, -23699.56f), new Vec3f(0f, -0.9925467f, -0.1218714f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(33145.59f, 4142.859f, -23800.76f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_BOW_L_01"), 1);
            mi.Spawn(mapName, new Vec3f(33245.05f, 4140.883f, -23700.74f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(33251.86f, 4139.21f, -23670.8f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(33185.05f, 4141.352f, -23669.08f), new Vec3f(0.9743701f, 0f, 0.224951f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(33246.41f, 4139.291f, -23659.68f), new Vec3f(0.05233596f, 0f, 0.9986296f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(33241.71f, 4139.083f, -23674.07f), new Vec3f(0.5877853f, 0f, 0.8090171f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(33238.53f, 4142.241f, -23639f), new Vec3f(-0.7313537f, 0f, 0.6819983f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(33293.6f, 4141.238f, -23636.01f), new Vec3f(-0.9455186f, 0f, 0.3255682f));

            mi = new Item(ItemInstance.getItemInstance("ITRW_ARROW"), 1);
            mi.Spawn(mapName, new Vec3f(33272.83f, 4140.34f, -23641.87f), new Vec3f(0.9998477f, 0f, -0.01744864f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(33170.39f, 4142.972f, -23779.37f), new Vec3f(-0.6946583f, 0f, 0.7193397f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_INSTANTFIREBALL"), 1);
            mi.Spawn(mapName, new Vec3f(30508.97f, 2161.635f, -18114.89f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITSC_FIREBOLT"), 1);
            mi.Spawn(mapName, new Vec3f(20873.48f, 3439.278f, -15779.22f), new Vec3f(-0.9063081f, 0f, 0.4226184f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(21012.26f, 3433.256f, -16079.87f), new Vec3f(0.2756374f, 0f, 0.9612617f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(21115.83f, 3402.965f, -16525.55f), new Vec3f(0f, 0f, 1f));

            mi = new Item(ItemInstance.getItemInstance("ITPL_BLUEPLANT"), 1);
            mi.Spawn(mapName, new Vec3f(21442.03f, 3329.569f, -16569.41f), new Vec3f(-0.01354556f, -0.05055268f, 0.9986298f));






		}
    }
}
