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
	public class DefaultVobs
	{
		public static void Init()
		{
            MobInter mi = null;
            String mapName = @"NEWWORLD\NEWWORLD.ZEN";
            mi = new MobInter("INNOS_BELIAR_ADDON_01.ASC", "MOBNAME_ADDON_IDOL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2570.125f, -642.0471f, 5569.523f), new Vec3f(-0.05233548f, 0f, -0.9986295f));

            mi = new MobInter("PAN_OC.MDS", "", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(-3349.417f, -437.4977f, 11783.97f), new Vec3f(-0.8571669f, 0f, 0.5150379f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(-7848.042f, -150.0547f, -5580.918f), new Vec3f(0.7771462f, 0f, -0.6293203f));

            mi = new MobInter("PAN_OC.MDS", "", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(-8553.845f, -205.2607f, -6034.539f), new Vec3f(0.9915028f, 0.1205851f, -0.04881358f));

            mi = new MobInter("SMOKE_WATERPIPE.MDS", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(-4376.727f, -470.8703f, -17361.58f), new Vec3f(-0.9335805f, 0f, 0.3583683f));

            mi = new MobInter("PAN_OC.MDS", "", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(-3757.243f, -452.6047f, -18935.53f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2313.814f, -184.5019f, -2684.225f), new Vec3f(0.6427875f, 0f, 0.7660441f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(12432.91f, 896.1024f, 270.8759f), new Vec3f(0.8480486f, 0f, -0.5299194f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(11007.29f, 893.9945f, 565.311f), new Vec3f(0.6691307f, 0f, -0.7431448f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(11380.88f, 897.0635f, -2440.006f), new Vec3f(0.6691304f, 0f, -0.743145f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(10229.87f, 898.2355f, -5457.497f), new Vec3f(0.9975638f, 0f, -0.06975681f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(10646.63f, 901.2355f, -5659.71f), new Vec3f(-0.2756371f, 0f, 0.9612616f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(9980.133f, 895.3944f, -3818.765f), new Vec3f(0.997564f, 0f, -0.06975676f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2404.551f, 882.0388f, 7990.139f), new Vec3f(-0.5150388f, 0f, 0.8571686f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2795.803f, 756.0388f, 7839.55f), new Vec3f(-0.2756379f, 0f, -0.9612627f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, true);
            mi.Spawn(mapName, new Vec3f(4409.315f, 747.5122f, 5466.615f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4654.018f, 750.7007f, 6115.068f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-1277.301f, -178.3534f, -3657.61f), new Vec3f(0.1218692f, 0f, 0.9925462f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-7669.924f, -218.4615f, -11941.38f), new Vec3f(-0.3907316f, 0f, 0.9205047f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-6868.689f, -238.9585f, -12329.07f), new Vec3f(-0.9961946f, 0f, -0.08715628f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7268.638f, -639.5806f, 4267.052f), new Vec3f(-0.9455185f, 0f, 0.3255676f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(2268.164f, -188.259f, 347.4416f), new Vec3f(0.9999995f, 0f, 2.402812E-07f));

            mi = new MobInter("REPAIR_PLANK.ASC", "", ItemInstance.getItemInstance("ITMI_HAMMER"), null, false, false);
            mi.Spawn(mapName, new Vec3f(3168.608f, -90.54131f, 565.3643f), new Vec3f(-0.3090172f, 0f, 0.951057f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, false);
            mi.Spawn(mapName, new Vec3f(2750.228f, -187.0356f, 201.1884f), new Vec3f(0.9876896f, 0f, -0.1564348f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, false);
            mi.Spawn(mapName, new Vec3f(3188.883f, -142.1273f, -1431.78f), new Vec3f(-0.978148f, 0f, -0.2079119f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, false);
            mi.Spawn(mapName, new Vec3f(2734.413f, -187.249f, 703.1655f), new Vec3f(-0.8660256f, 0f, 0.5000001f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(2860.225f, -183.4453f, -84.61119f), new Vec3f(0.1218694f, 0f, 0.9925458f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3381.771f, -187.9646f, 3303.135f), new Vec3f(-0.9396927f, 0f, -0.3420202f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-238.8055f, -164.6914f, 116.8288f), new Vec3f(-0.9925461f, 0f, 0.1218694f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8197.084f, 269.0771f, 2899.724f), new Vec3f(0.8480489f, 0f, -0.5299197f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(8833.106f, 268.4804f, 3108.48f), new Vec3f(0.3420207f, 0f, -0.9396937f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8638.188f, 273.2932f, 3358.427f), new Vec3f(0.8386713f, 0f, 0.5446395f));

            mi = new MobInter("SMOKE_WATERPIPE.MDS", "MOBNAME_WATERPIPE", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(6786.598f, 282.7729f, 3160.135f), new Vec3f(-0.3583677f, 0f, -0.9335803f));

            mi = new MobInter("SMOKE_WATERPIPE.MDS", "MOBNAME_WATERPIPE", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(6741.27f, 293.0124f, 2972.391f), new Vec3f(-0.6156613f, 0f, 0.7880105f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(3516.742f, -118.4042f, -2337.842f), new Vec3f(0.9975637f, 0f, -0.06975666f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3146.222f, -112.814f, -2577.417f), new Vec3f(0.4383713f, 0f, -0.8987936f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(5484.784f, 299.4131f, 2655.011f), new Vec3f(-0.9816353f, 0f, 0.1907672f));

            mi = new MobInter("SMOKE_WATERPIPE.MDS", "MOBNAME_WATERPIPE", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(6898.096f, 297.7481f, 2711.38f), new Vec3f(-0.6293206f, 0f, 0.7771461f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(4429.654f, -188.5941f, 839.3012f), new Vec3f(0.1391729f, 0f, 0.9902676f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4834.911f, -189.2218f, 753.5451f), new Vec3f(-0.9925463f, 0f, 0.1218694f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, false);
            mi.Spawn(mapName, new Vec3f(5385.114f, -189.6115f, 57.4865f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10881.39f, 1300.781f, 1504.142f), new Vec3f(-0.5299192f, 0f, -0.8480485f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10459.35f, 1431.728f, 1171.228f), new Vec3f(-0.9702964f, 0f, -0.2419221f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(11034.03f, 898.2692f, 1144.988f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10578.21f, 905.5776f, 1539.921f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(10915.56f, 897.7404f, 939.9799f), new Vec3f(0.2588193f, 0f, -0.9659265f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14445.99f, 1539.884f, -749.5632f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14200.72f, 1541.763f, -510.0808f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14213.39f, 1672.47f, -736.2581f), new Vec3f(-0.7313539f, 0f, -0.6819982f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14456.6f, 1671.058f, -1000.878f), new Vec3f(-0.7313549f, 0f, -0.6819993f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15444.4f, 1541.776f, 571.1039f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15110.66f, 1543.776f, -652.5573f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14645.22f, 1328.674f, -4684.458f), new Vec3f(0.08715656f, 0f, -0.9961956f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15275.52f, 1328.674f, -4064.246f), new Vec3f(0.5150377f, 0f, 0.8571681f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(13652.58f, 1333.807f, -3993.171f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(13864.3f, 1334.807f, -3353.744f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14145.57f, 1461.987f, -3131.076f), new Vec3f(0.4694721f, 0f, 0.8829474f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(13350.77f, 1463.773f, -4002.418f), new Vec3f(-0.9563046f, 0f, -0.2923709f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(14018.85f, 1327.35f, -4265.254f), new Vec3f(-0.8829473f, 0f, 0.4694723f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7011.252f, 123.932f, -264.7305f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(5258.394f, 158.6117f, -355.2857f), new Vec3f(0.8191538f, 0f, 0.573577f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6068.325f, 270.3356f, -690.9987f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6075.354f, 270.0541f, -493.4251f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(6204.941f, 267.9279f, -312.51f), new Vec3f(0.8480486f, 0f, 0.5299193f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(1025.75f, -53.40888f, 892.3632f), new Vec3f(-0.7193397f, 0f, -0.6946582f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3684.402f, -185.2747f, 407.1343f), new Vec3f(0.987689f, 0f, -0.1564344f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(16591.48f, 1299.838f, -2663.782f), new Vec3f(0.3907309f, 0f, -0.9205043f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14825.38f, 1671.904f, -4.244082f), new Vec3f(0.2923717f, 0f, -0.9563058f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14782.62f, 1537.602f, -794.3517f), new Vec3f(0.2588194f, 0f, 0.9659269f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14352.83f, 1232.071f, -602.7307f), new Vec3f(0.6819981f, 0f, -0.7313536f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15873f, 1539.628f, -61.6291f), new Vec3f(-0.1564347f, 0f, 0.9876881f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15711.84f, 1537.544f, -317.9729f), new Vec3f(-0.9975648f, 0f, -0.06975668f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(6459.499f, 268.2227f, -3754.95f), new Vec3f(0.9902682f, 0f, 0.1391731f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(1975.874f, -185.9828f, -468.4845f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2231.985f, -188.9828f, -524.4097f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(2981.984f, -120.9643f, -5630.736f), new Vec3f(-0.8191521f, 0f, 0.5735765f));

            mi = new MobInter("REPAIR_PLANK.ASC", "", ItemInstance.getItemInstance("ITMI_HAMMER"), null, false, false);
            mi.Spawn(mapName, new Vec3f(4351.408f, -18.5442f, -4839.239f), new Vec3f(-0.5446393f, 0f, -0.838671f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8124.328f, 268.6747f, 5050.899f), new Vec3f(0.4226185f, 0f, -0.9063074f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7099.528f, 276.6811f, 3109.353f), new Vec3f(0.9135457f, 0f, 0.4067369f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7601.548f, 126.8323f, 981.061f), new Vec3f(-0.5210944f, 0f, -0.8534993f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8222.747f, 123.0478f, -1085.075f), new Vec3f(0.9816274f, 0f, 0.190809f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7026.624f, 270.0357f, -1978.266f), new Vec3f(-0.4694718f, 0f, 0.8829481f));

            mi = new MobInter("BOOK_BLUE.ASC", "MOBNAME_BOOK", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(6715.255f, 123.4328f, -606.2182f), new Vec3f(-0.3255681f, 0f, -0.9455187f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, false);
            mi.Spawn(mapName, new Vec3f(6356.664f, 124.4691f, -223.1976f), new Vec3f(-0.9961963f, 0f, 0.08715586f));

            mi = new MobInter("BSSHARP_OC.MDS", "MOBNAME_GRINDSTONE", ItemInstance.getItemInstance("ITMISWORDBLADE"), null, false, false);
            mi.Spawn(mapName, new Vec3f(5639.21f, 275.0764f, -1663.667f), new Vec3f(-0.5446385f, 0f, -0.8386701f));

            mi = new MobInter("BSFIRE_OC.MDS", "MOBNAME_FORGE", ItemInstance.getItemInstance("ITMISWORDRAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(5437.341f, 270.0098f, -854.2729f), new Vec3f(-0.5591934f, 0f, 0.8290381f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, false);
            mi.Spawn(mapName, new Vec3f(5768.668f, 274.8962f, -1098.321f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BSANVIL_OC.MDS", "MOBNAME_ANVIL", ItemInstance.getItemInstance("ITMISWORDRAWHOT"), null, true, false);
            mi.Spawn(mapName, new Vec3f(5872.567f, 273.6223f, -1481.374f), new Vec3f(-0.9461342f, -0.01902968f, 0.3232172f));

            mi = new MobInter("REPAIR_PLANK.ASC", "", ItemInstance.getItemInstance("ITMI_HAMMER"), null, false, false);
            mi.Spawn(mapName, new Vec3f(3722.791f, -72.43447f, 230.6021f), new Vec3f(0.9961944f, 0f, -0.08715568f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(683.3502f, -185.4316f, 1626.472f), new Vec3f(-0.9205055f, 0f, -0.3907312f));

            mi = new MobInter("REPAIR_PLANK.ASC", "", ItemInstance.getItemInstance("ITMI_HAMMER"), null, false, false);
            mi.Spawn(mapName, new Vec3f(697.7027f, -83.59538f, 1346.488f), new Vec3f(0.1736482f, 0f, -0.9848074f));

            mi = new MobInter("SMOKE_WATERPIPE.ASC", "MOBNAME_WATERPIPE", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(909.0797f, -179.438f, -3258.279f), new Vec3f(0.9781496f, 0f, 0.2079127f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(626.4776f, -183.4124f, -3438.955f), new Vec3f(-0.8191518f, 0f, -0.5735763f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(811.612f, -182.6549f, -3487.333f), new Vec3f(-0.1391732f, 0f, -0.9902689f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(1274.606f, -188.1556f, 87.68612f), new Vec3f(-0.9993912f, 0f, 0.03489954f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4418.63f, 748.7318f, 8145.782f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4274.932f, 748.726f, 8340.924f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4486.589f, 749.7013f, 8452.915f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4666.058f, 749.6682f, 8259.875f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(2908.839f, -119.7223f, -3066.965f), new Vec3f(-0.01745247f, 0f, -0.9998478f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(425.1574f, -186.7359f, -5763.454f), new Vec3f(-0.9612616f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(1487.55f, -187.2987f, -5103.829f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(1250.985f, -187.2986f, -4737.549f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2107.927f, -187.6763f, -4937.684f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(1431.511f, -183.8404f, -4553.942f), new Vec3f(0.6946583f, 0f, 0.7193396f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3452.975f, -115.9968f, -6157.299f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3562.8f, -121.0585f, -5060.138f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4726.837f, -117.8359f, -3808.734f), new Vec3f(-0.9816272f, 0f, 0.1908089f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2765.812f, -184.985f, -739.7236f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4371.194f, -186.2635f, 414.1365f), new Vec3f(-0.6560593f, 0f, 0.7547097f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3502.495f, -117.386f, -3678.316f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3556.842f, -185.9008f, 872.9494f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2919.114f, -186.4726f, 2138.747f), new Vec3f(-0.9612616f, 0f, 0.2756374f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(7602.563f, 265.0147f, -2984.809f), new Vec3f(-0.4694718f, 0f, 0.8829482f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7660.919f, 268.8225f, -2405.483f), new Vec3f(-0.9876891f, 0f, 0.1564347f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(5743.429f, 266.9321f, -5229.509f), new Vec3f(-0.9902683f, 0f, -0.1391732f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8363.992f, 268.615f, -3237.877f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6298.467f, 269.8129f, -4768.615f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6456.298f, 269.5421f, -4936.134f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7606.494f, 401.4297f, -1880.372f), new Vec3f(-0.01745258f, 0f, 0.9998489f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(8845.167f, 126.9953f, 586.831f), new Vec3f(0.8386714f, 0f, -0.5446396f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(7012.851f, 267.8332f, -2362.083f), new Vec3f(-0.8987946f, 0f, -0.4383712f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6655.627f, 266.8963f, -4795.178f), new Vec3f(0.4694716f, 0f, -0.882948f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6540.162f, 401.0057f, -5280.248f), new Vec3f(0.1391733f, 0f, -0.9902683f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8648.88f, 269.4368f, 3156.29f), new Vec3f(0.9396943f, 0f, -0.3420205f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8611.708f, 269.3888f, 3801.581f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(8166.349f, 125.2971f, 1873.18f), new Vec3f(0.5446395f, 0f, 0.8386713f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8304.663f, 127.9018f, 1539.901f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8567.58f, 125.4005f, 1553.066f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8821.097f, 128.1795f, 1393.478f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(9108.338f, 129.6938f, 1204.323f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8428.292f, 125.3087f, 1086.834f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8516.173f, 127.2665f, 887.934f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(10194.04f, 268.463f, 1311.708f), new Vec3f(0.9743704f, 0f, 0.2249511f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(5816.798f, 309.1463f, 2513.415f), new Vec3f(-0.9816346f, -0.001217417f, 0.190767f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(8199.352f, -641.218f, 3898.663f), new Vec3f(-2.04891E-07f, 0f, -1.000001f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(10219.29f, 898.3678f, -3109.316f), new Vec3f(-0.6427883f, 0f, -0.7660451f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10644.41f, 899.8284f, -2556.313f), new Vec3f(0.1218698f, 0f, -0.9925478f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10351.12f, 898.8284f, -2867.307f), new Vec3f(0.5735763f, 0f, 0.8191533f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10446.75f, 1430.48f, -2319.563f), new Vec3f(-0.7660449f, 0f, 0.6427883f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10430.14f, 1299.915f, -2935.634f), new Vec3f(0.5735757f, 0f, 0.8191526f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10705.88f, 1299.192f, -2735.204f), new Vec3f(-0.9902679f, 0f, -0.1391732f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(11882.86f, 900.2026f, -4831.291f), new Vec3f(0.6946588f, 0f, 0.7193402f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(11608.89f, 899.9226f, -5020.089f), new Vec3f(0.1218699f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(11687.96f, 1305.248f, -5081.255f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(11914.4f, 1299.249f, -4843.056f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12127.02f, 1429.227f, -5008.836f), new Vec3f(0.6946587f, 0f, -0.719341f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(12632.31f, 898.5446f, -4865.864f), new Vec3f(0.03489914f, 0f, -0.9993929f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12498.89f, 899.684f, -4420.655f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12187.85f, 902.6874f, -4388.386f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(14109.45f, 896.303f, -4207.537f), new Vec3f(0.8829486f, 0f, -0.4694724f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(13776f, 899.3778f, -4037.01f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(13677.36f, 901.3785f, -3751.094f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14806.23f, 899.5265f, -4444.505f), new Vec3f(0.9205059f, 0f, 0.3907323f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14511.93f, 899.5289f, -3815.455f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14302.24f, 899.5293f, -4259.979f), new Vec3f(0.1218701f, 0f, -0.9925461f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14842.45f, 1030.781f, -3503.309f), new Vec3f(0.4694723f, 0f, 0.8829486f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(11191.96f, 899.6838f, 2921.055f), new Vec3f(0.5299194f, 0f, -0.8480483f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12974.95f, 1030.079f, 3530.669f), new Vec3f(0.9135457f, 0f, 0.4067369f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(16871.98f, 897.6216f, -2292.629f), new Vec3f(0.1045287f, 0f, 0.9945219f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(16955.57f, 900.5292f, -2848.696f), new Vec3f(0.1218711f, 0f, -0.9925458f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(17027.75f, 899.5292f, -2570.603f), new Vec3f(0.1218711f, 0f, -0.9925458f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(16697.49f, 901.5292f, -2815.171f), new Vec3f(0.1218711f, 0f, -0.9925458f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(16847.54f, 1432.449f, -3147.54f), new Vec3f(-0.1218695f, 0f, -0.9925462f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8738.428f, 569.4586f, 3724.919f), new Vec3f(-0.9816271f, 0f, 0.1908089f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2082.983f, -195.4629f, 80.75179f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CAULDRON_OC.ASC", "MOBNAME_CAULDRON", ItemInstance.getItemInstance("ITMI_SCOOP"), null, false, false);
            mi.Spawn(mapName, new Vec3f(2420.905f, -117.9217f, -6323.801f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("FIREPLACE_GROUND.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(2416.998f, -117.035f, -6256.773f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(4179.536f, -110.4691f, -5735.052f), new Vec3f(0.9848087f, 0f, -0.1736485f));

            mi = new MobInter("BSFIRE_OC.MDS", "MOBNAME_FORGE", ItemInstance.getItemInstance("ITMISWORDRAW"), null, false, false);
            mi.Spawn(mapName, new Vec3f(4758.222f, -122.014f, -6487.446f), new Vec3f(0.5735767f, 0f, -0.8191524f));

            mi = new MobInter("BSANVIL_OC.MDS", "MOBNAME_ANVIL", ItemInstance.getItemInstance("ITMISWORDRAWHOT"), null, true, false);
            mi.Spawn(mapName, new Vec3f(4391.226f, -95.70056f, -6193.74f), new Vec3f(0.9708792f, 0f, -0.2395694f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, false);
            mi.Spawn(mapName, new Vec3f(4629.094f, -44.0546f, -5910.221f), new Vec3f(-0.2419219f, 0f, 0.970296f));

            mi = new MobInter("BSSHARP_OC.MDS", "MOBNAME_GRINDSTONE", ItemInstance.getItemInstance("ITMISWORDBLADE"), null, false, false);
            mi.Spawn(mapName, new Vec3f(3979.356f, -119.8179f, -6245.007f), new Vec3f(0.7071066f, 0f, -0.7071062f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, false);
            mi.Spawn(mapName, new Vec3f(4332.995f, -118.3213f, -6873.1f), new Vec3f(-0.2419218f, 0f, 0.9702957f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, false);
            mi.Spawn(mapName, new Vec3f(3317.441f, -120.4515f, -6710.77f), new Vec3f(-0.9975642f, 0f, 0.06975614f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3571.186f, 14.21001f, -6450.178f), new Vec3f(0.03489916f, 0f, 0.9993907f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3777.212f, -114.0604f, -6790.296f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(6440.796f, 266.6235f, -2611.299f), new Vec3f(0.9816277f, 0f, -0.1908092f));

            mi = new MobInter("REPAIR_PLANK.ASC", "", ItemInstance.getItemInstance("ITMI_HAMMER"), null, false, false);
            mi.Spawn(mapName, new Vec3f(6650.353f, 371.5932f, -2706.781f), new Vec3f(0.9781476f, 0f, -0.2079118f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8751.276f, 129.5842f, 897.7361f), new Vec3f(-0.9612617f, 0f, 0.2756374f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2233f, -183.5707f, -781.3022f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(1218.249f, -185.7229f, -472.9813f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(17157.9f, 1295.533f, -2415.297f), new Vec3f(0.9816276f, 0f, -0.1908095f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, false);
            mi.Spawn(mapName, new Vec3f(2971.599f, -120.6816f, -3893.879f), new Vec3f(-0.2588193f, 0f, -0.9659264f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, false);
            mi.Spawn(mapName, new Vec3f(347.2092f, -183.8099f, -4722.415f), new Vec3f(0.9271856f, 0f, 0.3746077f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(2526.074f, -185.0387f, -4735.675f), new Vec3f(-0.5150384f, 0f, -0.8571678f));

            mi = new MobInter("CAULDRON_OC.ASC", "MOBNAME_CAULDRON", ItemInstance.getItemInstance("ITMI_SCOOP"), null, false, false);
            mi.Spawn(mapName, new Vec3f(2681.086f, -184.2783f, 1134.025f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("FIREPLACE_GROUND.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(2678.454f, -184.9759f, 1193.63f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, false);
            mi.Spawn(mapName, new Vec3f(3204.141f, -138.1387f, -4499.391f), new Vec3f(0.515039f, 0f, 0.8571692f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(3695.306f, -116.3191f, -5308.41f), new Vec3f(-0.4226183f, 0f, -0.9063076f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(4212.498f, -113.6207f, -3607.82f), new Vec3f(-0.9063089f, 0f, 0.4226187f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7471.894f, -522.2076f, 3914.973f), new Vec3f(0.08715653f, 0f, -0.9961949f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-7781.58f, -221.8392f, -20786.33f), new Vec3f(-0.9925462f, 0f, 0.1218692f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-6894.172f, -228.9534f, -21424.17f), new Vec3f(-0.9925462f, 0f, 0.1218691f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-7497.232f, -232.9435f, -22366.14f), new Vec3f(0.1218691f, 0f, 0.9925459f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-7072.219f, -219.0505f, -13139.33f), new Vec3f(-0.5446392f, 0f, 0.8386704f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-2403.314f, -197.2224f, -4588.725f), new Vec3f(0.1218692f, 0f, 0.992546f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-3010.155f, -196.2224f, -4570.156f), new Vec3f(-0.08715586f, 0f, 0.9961945f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-3681.595f, -174.2224f, -4588.554f), new Vec3f(-0.05233581f, 0f, 0.9986295f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-4376.992f, -160.7867f, -4671.585f), new Vec3f(-0.2249512f, 0f, 0.9743698f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-5812.408f, -162.2047f, -5039.317f), new Vec3f(-0.3420204f, 0f, 0.9396926f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2478.168f, 750.9778f, 7560.103f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, true);
            mi.Spawn(mapName, new Vec3f(6985.596f, 750.0159f, 6615.155f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BSFIRE_OC.MDS", "MOBNAME_FORGE", ItemInstance.getItemInstance("ITMISWORDRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(6574.802f, 742.0159f, 6188.446f), new Vec3f(0.4539908f, 0f, -0.8910074f));

            mi = new MobInter("BSSHARP_OC.MDS", "MOBNAME_GRINDSTONE", ItemInstance.getItemInstance("ITMISWORDBLADE"), null, true, true);
            mi.Spawn(mapName, new Vec3f(6137.359f, 755.0159f, 6368.503f), new Vec3f(0.2588191f, 0f, 0.965926f));

            mi = new MobInter("BSANVIL_OC.MDS", "MOBNAME_ANVIL", ItemInstance.getItemInstance("ITMISWORDRAWHOT"), null, true, true);
            mi.Spawn(mapName, new Vec3f(6636.793f, 750.9845f, 6721.737f), new Vec3f(0.544535f, 0f, -0.8387381f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6655.917f, 750.4889f, 8998.188f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(4115.661f, 748.6168f, 7994.816f), new Vec3f(0.8829485f, 0f, 0.4694715f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4050.477f, 750.4918f, 8120.405f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3199.413f, 883.8612f, 6336.506f), new Vec3f(0.891008f, 0f, 0.4539912f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3580.229f, 750.1345f, 5715.856f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3454.784f, 882.7377f, 5378.122f), new Vec3f(-0.8746217f, 0f, -0.4848104f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(12837.08f, 890.7583f, 3085.547f), new Vec3f(0.9396939f, 0f, -0.3420202f));

            mi = new MobInter("BENCH_NW_CITY_02.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(11428.31f, 899.4324f, -4338.325f), new Vec3f(0.6691311f, 0f, -0.7431454f));

            mi = new MobInter("LEVER_1_OC.MDS", "MOBNAME_SECRETSWITCH", null, "EVT_CITY_THIEV_ISLE_MSG_01", true, false);
            mi.Spawn(mapName, new Vec3f(-19199.42f, -168.349f, 2844.351f), new Vec3f(0.7093845f, -0.1163542f, 0.6951618f));

            mi = new MobInter("PAN_OC.MDS", "MOBNAME_PAN", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-5598.788f, -218.809f, -10428.07f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("REPAIR_PLANK.ASC", "", ItemInstance.getItemInstance("ITMI_HAMMER"), null, false, false);
            mi.Spawn(mapName, new Vec3f(-8813.276f, -131.1403f, -6204.716f), new Vec3f(0.906307f, 0f, -0.4226181f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(12678.58f, 1555.06f, -22038.21f), new Vec3f(-0.1218694f, 0f, 0.9925468f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14946f, 1591f, -13282f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14700f, 1589f, -13269f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14713.59f, 1593.474f, -13805.96f), new Vec3f(0.9612627f, 0f, -0.2756375f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(14916f, 1592f, -13001f), new Vec3f(0.1158041f, 0f, 0.9932733f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, false, true);
            mi.Spawn(mapName, new Vec3f(8054.793f, 1149.87f, -17606.6f), new Vec3f(-0.8910066f, 0f, -0.4539905f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(38532.95f, 3810.453f, -2480.437f), new Vec3f(0.9702995f, 0f, 0.2419074f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-953.0506f, 2411.72f, 16846.2f), new Vec3f(-0.5446391f, 0f, 0.8386707f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12893.72f, 4076.053f, 11675.23f), new Vec3f(0.7437888f, -2.218344E-05f, 0.6684172f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30327.24f, 3146.829f, 29206.8f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CAULDRON_OC.ASC", "MOBNAME_CAULDRON", ItemInstance.getItemInstance("ITMI_SCOOP"), null, true, true);
            mi.Spawn(mapName, new Vec3f(29823.95f, 3146.305f, 29240.25f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CAULDRON_OC.ASC", "MOBNAME_CAULDRON", ItemInstance.getItemInstance("ITMI_SCOOP"), null, true, true);
            mi.Spawn(mapName, new Vec3f(29949.15f, 3146.305f, 29586.05f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CAULDRON_OC.ASC", "MOBNAME_CAULDRON", ItemInstance.getItemInstance("ITMI_SCOOP"), null, true, true);
            mi.Spawn(mapName, new Vec3f(30376.08f, 3146.305f, 29328.96f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(29995.07f, 3146.305f, 29032.54f), new Vec3f(-0.7660438f, 0f, -0.6427872f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(29706.65f, 3146.305f, 29563.83f), new Vec3f(-0.9999994f, 0f, 8.294521E-08f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(31490.18f, 2628.974f, 20396.82f), new Vec3f(0.9890721f, 0.03463936f, -0.1433f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4880.426f, 2988.932f, 24651.35f), new Vec3f(-0.6427874f, 0f, -0.7660443f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(30632.92f, 4267.396f, -6204.252f), new Vec3f(-0.8829476f, 0f, -0.4694716f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(37828.35f, 3814.132f, -3071.361f), new Vec3f(0.8386711f, 0f, 0.5446395f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(37270.86f, 3797.767f, -2921.365f), new Vec3f(-0.0871425f, -0.001521078f, 0.996195f));

            mi = new MobInter("BARBQ_NW_MISC_SHEEP_01.MDS", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(31803.11f, 3476.792f, 9772.389f), new Vec3f(0.9986311f, 0f, -0.05233597f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-757.8283f, 2408.615f, 16354.28f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-621.1052f, 2408.038f, 16432.71f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, false);
            mi.Spawn(mapName, new Vec3f(31683.48f, 3442.016f, 9166.995f), new Vec3f(-0.997564f, 0f, 0.06975584f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(32126.17f, 3448.151f, 8714.189f), new Vec3f(0.9925461f, 0f, 0.1218704f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(32020.24f, 3450.151f, 8184.621f), new Vec3f(0.9848076f, 0f, 0.1736492f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(32028.83f, 3445.151f, 9039.001f), new Vec3f(0.9848076f, 0f, 0.1736492f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(31080.1f, 3462.751f, 8012.014f), new Vec3f(-0.694469f, 0f, -0.7195218f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(32154.4f, 3452.14f, 9559.216f), new Vec3f(-0.2590731f, 0f, 0.9658575f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(38433.96f, 3813.933f, -1923.438f), new Vec3f(0.9993917f, 0f, 0.03488445f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(37698.25f, 3823.264f, -1903.965f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(38005.75f, 3823.227f, -1869.77f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(38304.03f, 3823.229f, -1866.006f), new Vec3f(0.9975649f, 0f, 0.06975652f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(36915.86f, 3793.24f, -2205.012f), new Vec3f(-0.9612617f, 0f, 0.2756373f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(36630.71f, 3793.767f, -2621.691f), new Vec3f(-0.9612617f, 0f, 0.2756372f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(36897.4f, 3794.116f, -2708.638f), new Vec3f(-0.2756375f, 0f, 0.9612617f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(37145.94f, 3793.505f, -2738.996f), new Vec3f(-0.2756368f, 0f, 0.9612619f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(37395.86f, 3793.504f, -2722.233f), new Vec3f(-0.9612622f, 0f, 0.2756383f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-1572.527f, 2408.464f, 16323.35f), new Vec3f(-0.4999998f, 0f, -0.866025f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(31818.62f, 3319.808f, 956.7693f), new Vec3f(0.4511841f, 0.05041813f, 0.8910058f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(34384.91f, 3717.816f, 83.86105f), new Vec3f(-0.7545801f, -0.01511422f, 0.6560338f));

            mi = new MobInter("TREASURE_ADDON_01.ASC", "", ItemInstance.getItemInstance("ITMW_2H_AXE_L_01"), null, false, false);
            mi.Spawn(mapName, new Vec3f(47827.37f, 1609.534f, -10442.68f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("TREASURE_ADDON_01.ASC", "", ItemInstance.getItemInstance("ITMW_2H_AXE_L_01"), null, false, false);
            mi.Spawn(mapName, new Vec3f(44186.16f, 2931.839f, -27256.17f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("TREASURE_ADDON_01.ASC", "", ItemInstance.getItemInstance("ITMW_2H_AXE_L_01"), null, false, false);
            mi.Spawn(mapName, new Vec3f(48060.79f, 2378.112f, -1102.45f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BARBQ_NW_MISC_SHEEP_01.MDS", "MOBNAME_BBQ_SHEEP", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(64086.86f, 3975.554f, -22245.36f), new Vec3f(0.9848074f, 0f, 0.1736481f));

            mi = new MobInter("REPAIR_PLANK.ASC", "MOBNAME_REPAIR", ItemInstance.getItemInstance("ITMI_HAMMER"), null, true, true);
            mi.Spawn(mapName, new Vec3f(64926.59f, 3941.886f, -24846.72f), new Vec3f(0.8829478f, 0f, -0.4694768f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(64314.93f, 3947.719f, -24142.28f), new Vec3f(0.9986292f, 0f, 0.05233587f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(44091f, 2993.849f, 3098f), new Vec3f(0.2556127f, -0.06931016f, -0.964293f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45641.08f, 3086.984f, 4596.835f), new Vec3f(0.017606f, 0f, 0.999845f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(43869f, 2955.115f, 2339f), new Vec3f(0.6285533f, 0.0571743f, 0.7756625f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(62534.02f, 1781.863f, 7908.835f), new Vec3f(0.6156616f, 0f, 0.788011f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(58257.71f, 1630.432f, 10791.62f), new Vec3f(-0.882947f, 0f, 0.4694723f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(58631.23f, 1630.394f, 10306.3f), new Vec3f(0.5299193f, 0f, -0.8480484f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(48899.15f, 3587.902f, 11390.07f), new Vec3f(-0.2756369f, 0f, 0.9612608f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(45871.14f, 2805.71f, -26553.67f), new Vec3f(-0.9848074f, 0f, 0.1736481f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(64279f, 2294.698f, -19183.66f), new Vec3f(-0.7660442f, 0f, -0.6427875f));

            mi = new MobInter("TOUCHPLATE_STONE.MDS", "MOBNAME_SWITCH", null, "CEMENTARY_CODEMASTER_01", true, false);
            mi.Spawn(mapName, new Vec3f(74011f, 3404f, -1286f), new Vec3f(-0.8466602f, 0f, -0.5321366f));

            mi = new MobInter("TOUCHPLATE_STONE.MDS", "MOBNAME_SWITCH", null, "CEMENTARY_CODEMASTER_01", true, false);
            mi.Spawn(mapName, new Vec3f(74353f, 3404f, -1054f), new Vec3f(0.8191529f, 0f, 0.5735771f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(65355.49f, 3842.732f, -25403.41f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(65613.52f, 3832.675f, -25365.46f), new Vec3f(0.8910081f, 0f, -0.4539914f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(65203.17f, 3839.045f, -24562.47f), new Vec3f(-0.6884822f, 0f, -0.7252553f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(50626.29f, 2964.477f, -19055.84f), new Vec3f(-0.9902694f, 0f, 0.1391734f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51206.23f, 2966.023f, -18255.84f), new Vec3f(-0.8910069f, 0f, -0.4539898f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51211.36f, 2966.025f, -18042.23f), new Vec3f(-0.4383723f, 0f, -0.8987941f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50917.83f, 2966.723f, -18219.26f), new Vec3f(-0.8910076f, 0f, -0.4539912f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50918f, 2966f, -18028f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50979.96f, 2964.293f, -18729.9f), new Vec3f(-0.9015318f, 0f, -0.4327152f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51023.93f, 2961.974f, -18443.16f), new Vec3f(-0.7389269f, 0f, 0.6737876f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(58088.17f, 1973.013f, -2185.736f), new Vec3f(-0.8660253f, 0f, -0.5000001f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75900.31f, 3513.005f, -12693.79f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75358.91f, 3513.368f, -12658.48f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75321.38f, 3511.964f, -12852.96f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(49467.6f, 3056.042f, -16836.43f), new Vec3f(0.900016f, -0.02078106f, -0.4353619f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72186.2f, 3160.28f, -13290.39f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(71670.98f, 3150.292f, -8547.724f), new Vec3f(0.06975647f, 0f, 0.9975641f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(74697.55f, 3513.415f, -9831.044f), new Vec3f(-0.9848076f, 0f, -0.1736482f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(74995.4f, 3512.949f, -10050.91f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75224.95f, 3511.063f, -10044.08f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75391.7f, 3511.085f, -10037.08f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75550.74f, 3512.953f, -9752.427f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75355.42f, 3510.958f, -9497.549f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75142.76f, 3510.975f, -9549.239f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(74889.8f, 3513.213f, -9530.279f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75572.75f, 3825.253f, -12621.89f), new Vec3f(0.9848076f, 0f, 0.173649f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(74739.32f, 3832.682f, -13182.74f), new Vec3f(0.9848076f, 0f, 0.1736481f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(71889.27f, 3159.514f, -9443.931f), new Vec3f(0.6156616f, 0f, 0.7880108f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72168.94f, 3156.121f, -9302.771f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72641.34f, 3151.033f, -8820.872f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72850.83f, 3150.033f, -8992.533f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72416.96f, 3153.774f, -9257.874f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72569.12f, 3158.26f, -9543.456f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72375.72f, 3159.684f, -12717.84f), new Vec3f(0.1045285f, 0f, -0.9945219f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72026.16f, 3155.349f, -13420.16f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(71742.98f, 3155.85f, -13087.84f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(71732.27f, 3157.149f, -12858.35f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(71989.13f, 3154.886f, -12600.34f), new Vec3f(1f, 0f, 0f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75500.24f, 3513.553f, -12972.58f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75920.98f, 3512.553f, -13131.98f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72117.8f, 3198.562f, -11761.11f), new Vec3f(0.8287628f, 0.01745505f, 0.5593256f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(71420.94f, 3169.031f, -10559.89f), new Vec3f(0.5299194f, 0f, 0.8480483f));

            mi = new MobInter("BSANVIL_OC.MDS", "MOBNAME_ANVIL", ItemInstance.getItemInstance("ITMISWORDRAWHOT"), null, true, true);
            mi.Spawn(mapName, new Vec3f(73018.34f, 3155.031f, -10519.2f), new Vec3f(0.7662045f, 0.01789994f, -0.6423467f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, true);
            mi.Spawn(mapName, new Vec3f(72754.59f, 3150.289f, -10652.27f), new Vec3f(0.891006f, 0f, 0.4539902f));

            mi = new MobInter("BSFIRE_OC.MDS", "MOBNAME_FORGE", ItemInstance.getItemInstance("ITMISWORDRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(72829.59f, 3154.591f, -10070.18f), new Vec3f(0.06974587f, 0.01745241f, 0.9974126f));

            mi = new MobInter("BSSHARP_OC.MDS", "MOBNAME_GRINDSTONE", ItemInstance.getItemInstance("ITMISWORDBLADE"), null, true, true);
            mi.Spawn(mapName, new Vec3f(72622.7f, 3151.222f, -10388.21f), new Vec3f(-0.866025f, 0f, -0.4999999f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(75291.15f, 3834.209f, -9393.453f), new Vec3f(-0.1045272f, 0f, 0.9945217f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75335.16f, 3831.127f, -10064.48f), new Vec3f(-0.642787f, 0f, -0.7660449f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(56886.22f, 1903.956f, -1091.707f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(56711.83f, 1903.039f, -1002.855f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(56146.29f, 1906.674f, -1109.692f), new Vec3f(-0.358368f, 0f, 0.9335809f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(56392f, 1898.882f, -1093.627f), new Vec3f(0.5735767f, 0f, 0.819151f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(57461f, 1902f, -1062f), new Vec3f(0.2635367f, 0f, 0.9646513f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(64761.18f, 3973.338f, -21894.31f), new Vec3f(0.99863f, 0f, 0.05233602f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(64569.15f, 3984.227f, -21502.57f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(64390.83f, 3984.545f, -21634.11f), new Vec3f(0.3255682f, 0f, 0.945519f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(64883.2f, 3978.85f, -21431.69f), new Vec3f(-0.8910066f, 0f, 0.4539905f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(67740.83f, 3968.079f, -21818.34f), new Vec3f(0.3255697f, 0f, 0.9455179f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(53094.05f, 1640.509f, -10640.69f), new Vec3f(0.4067368f, 0f, 0.9135461f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(53006.52f, 1640.965f, -10412.52f), new Vec3f(0.927183f, 0f, -0.3746077f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(52755.03f, 1642.116f, -10500.83f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(44643.29f, 2935.514f, 2590.877f), new Vec3f(0.9457009f, -0.05199638f, -0.3208521f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(81406.3f, 4200.362f, -18648.16f), new Vec3f(0.9993969f, 0f, 0.03474252f));

            mi = new MobInter("BENCH_NW_OW_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(82127.95f, 4191.998f, -18388.72f), new Vec3f(0.7772447f, 0f, 0.6291985f));

            mi = new MobInter("SMOKE_WATERPIPE.MDS", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(82097.41f, 4148.368f, -19505.99f), new Vec3f(0.9510574f, 0f, 0.3090173f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(69561.88f, 1678.974f, -24520.14f), new Vec3f(-0.4694693f, 0f, 0.8829474f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(69775.81f, 1692.24f, -23807.04f), new Vec3f(0.5299212f, 0f, 0.8480468f));

            mi = new MobInter("BAUMSAEGE_1.ASC", "MOBNAME_SAW", ItemInstance.getItemInstance("ITMI_SAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(65290.04f, 3750.124f, -23353.95f), new Vec3f(-0.325567f, 0f, -0.945519f));

            mi = new MobInter("ORE_GROUND.ASC", "", null, null, false, false);
            mi.Spawn(mapName, new Vec3f(63070.58f, 4185.826f, -27307.55f), new Vec3f(-0.5299194f, 0f, 0.8480485f));

            mi = new MobInter("TREASURE_ADDON_01.ASC", "", ItemInstance.getItemInstance("ITMW_2H_AXE_L_01"), null, false, false);
            mi.Spawn(mapName, new Vec3f(59557.18f, 1817.755f, -26176.95f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("TREASURE_ADDON_01.ASC", "", ItemInstance.getItemInstance("ITMW_2H_AXE_L_01"), null, false, false);
            mi.Spawn(mapName, new Vec3f(50926.68f, 3029.083f, -15042.92f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50599.27f, 4992.391f, 19433.74f), new Vec3f(-0.993768f, 0f, 0.1114704f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51537.25f, 5014.22f, 19111.08f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51148.63f, 4999.22f, 18596f), new Vec3f(-0.9980273f, 0f, -0.06278951f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51558f, 4995.22f, 18572.58f), new Vec3f(0.9937695f, 0f, -0.1114704f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51355.43f, 4990.678f, 19442.16f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49075.9f, 4992.847f, 22313.83f), new Vec3f(0.2249503f, 0f, 0.9743702f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49227.2f, 4993.121f, 22071.16f), new Vec3f(0.9998481f, 0f, 0.01745243f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49694.91f, 4993.619f, 22039.44f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49889.18f, 4994.568f, 22168.84f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49493.15f, 4993.746f, 22384.26f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_EDEL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49642.94f, 4992.39f, 22519.58f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(50400.65f, 4991.851f, 18117.78f), new Vec3f(-0.8480482f, 0f, -0.5299193f));

            mi = new MobInter("RMAKER_1.MDS", "MOBNAME_RUNEMAKER", ItemInstance.getItemInstance("ITMI_RUNEBLANK"), null, true, false);
            mi.Spawn(mapName, new Vec3f(50767.59f, 4990.851f, 17976.45f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BOOK_BLUE.ASC", "MOBNAME_BOOKSTAND", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45970.43f, 4992.94f, 20520.66f), new Vec3f(-0.8660269f, 0f, -0.4999992f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45475.05f, 5125.94f, 20188.2f), new Vec3f(-0.8571663f, 0f, -0.5150402f));

            mi = new MobInter("BOOK_BLUE.ASC", "MOBNAME_BOOKSTAND", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(46075.75f, 4992.94f, 19446.17f), new Vec3f(-0.3746084f, 0f, -0.9271841f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45748.35f, 5123.94f, 19690.18f), new Vec3f(-0.8829482f, 0f, -0.4694726f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45881.33f, 5124.94f, 20904.74f), new Vec3f(-0.5000015f, 0f, 0.8660269f));

            mi = new MobInter("BOOK_BLUE.ASC", "MOBNAME_BOOKSTAND", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(46216.49f, 4992.94f, 20518.91f), new Vec3f(0.8746194f, 0f, 0.4848097f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(38361.52f, 4361.226f, 9492.803f), new Vec3f(0.3255683f, 0f, 0.9455189f));

            mi = new MobInter("BOOK_BLUE.ASC", "MOBNAME_ALMANACH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50166.45f, 3480.907f, 18951.71f), new Vec3f(-0.4848093f, 0f, 0.8746189f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51714.84f, 3613.743f, 18577.64f), new Vec3f(0.4848108f, 0f, -0.8746185f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(49546.46f, 4241.375f, 21304.46f), new Vec3f(0.5299191f, 0f, -0.8480479f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(48117.41f, 4241.178f, 21779.74f), new Vec3f(-0.4999989f, 0f, 0.8660247f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48482.08f, 4375.305f, 22020.12f), new Vec3f(-0.5150383f, 0f, 0.8571664f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49101.31f, 4375.673f, 20999.18f), new Vec3f(0.5150385f, 0f, -0.857167f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50071.27f, 4375.761f, 21578.56f), new Vec3f(0.5150385f, 0f, -0.857167f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49453.12f, 4374.749f, 22586.73f), new Vec3f(-0.5150383f, 0f, 0.8571664f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(38683.73f, 3195.618f, 2291.142f), new Vec3f(0.438438f, 0f, 0.8987615f));

            mi = new MobInter("THRONE_BIG.ASC", "MOBNAME_THRONE", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51007.98f, 5042.945f, 20793.79f), new Vec3f(0.8571668f, 0f, 0.5150392f));

            mi = new MobInter("THRONE_BIG.ASC", "MOBNAME_THRONE", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50794.16f, 5041.897f, 21151.96f), new Vec3f(0.8571669f, 0f, 0.5150393f));

            mi = new MobInter("THRONE_BIG.ASC", "MOBNAME_THRONE", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50728.15f, 5039.321f, 20863.55f), new Vec3f(0.848048f, 0f, 0.5299192f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(47688.14f, 4242.284f, 17493.52f), new Vec3f(0.4848099f, 0f, -0.8746194f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(47407.55f, 4241.257f, 17971.39f), new Vec3f(-0.5150376f, 0f, 0.8571676f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(46963.54f, 4991.393f, 21617.84f), new Vec3f(-0.5298558f, 0f, 0.8480878f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(45753.79f, 4242.453f, 19982.64f), new Vec3f(-0.9998494f, 0f, -0.01737723f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, true);
            mi.Spawn(mapName, new Vec3f(47741.87f, 4990.188f, 17076.47f), new Vec3f(0.5735767f, 0f, -0.8191524f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, true);
            mi.Spawn(mapName, new Vec3f(47394.18f, 4992.006f, 17490.67f), new Vec3f(0.9612625f, 0f, -0.2756374f));

            mi = new MobInter("HERB_NW_MISC_01.ASC", "MOBNAME_WINEMAKER", ItemInstance.getItemInstance("ITMI_STOMPER"), null, true, true);
            mi.Spawn(mapName, new Vec3f(48032.64f, 4987.298f, 16765.81f), new Vec3f(-0.1908081f, 0f, 0.9816276f));

            mi = new MobInter("RMAKER_1.MDS", "MOBNAME_RUNEMAKER", ItemInstance.getItemInstance("ITMI_RUNEBLANK"), null, true, false);
            mi.Spawn(mapName, new Vec3f(46271.55f, 4984.525f, 19627.22f), new Vec3f(-0.9063084f, 0f, 0.4226173f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(46721.77f, 5125.023f, 19671.22f), new Vec3f(0.9396933f, 0f, -0.3420204f));

            mi = new MobInter("BOOK_BLUE.ASC", "MOBNAME_BOOKSTAND", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(46099.43f, 4991.171f, 20717.17f), new Vec3f(0.8829475f, 0f, 0.4694718f));

            mi = new MobInter("BOOK_BLUE.ASC", "MOBNAME_BOOKSTAND", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45386.93f, 4991.657f, 20528.97f), new Vec3f(-0.5299195f, 0f, 0.848048f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50749.64f, 4243.176f, 18274.69f), new Vec3f(0.0348995f, 0f, 0.9993908f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49751.03f, 4242.491f, 17892.22f), new Vec3f(-0.984808f, 0f, 0.1736483f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49948.16f, 4241.937f, 17583.36f), new Vec3f(-0.2756374f, 0f, -0.9612619f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(50521f, 4242.791f, 18896.33f), new Vec3f(0.8571688f, 0f, 0.5150357f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49568.16f, 4992.969f, 20661.66f), new Vec3f(-0.8571674f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49670.65f, 4992.969f, 20490.68f), new Vec3f(-0.8571672f, 0f, -0.5150386f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49354.77f, 4988.501f, 20536.38f), new Vec3f(-0.8571675f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49457.27f, 4988.501f, 20365.4f), new Vec3f(-0.8571675f, 0f, -0.5150387f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49150.15f, 4992.337f, 20411.31f), new Vec3f(-0.8571674f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49252.64f, 4992.337f, 20240.33f), new Vec3f(-0.8571674f, 0f, -0.5150387f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49904.02f, 4993.522f, 20094.99f), new Vec3f(-0.8571674f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50006.51f, 4993.522f, 19924.01f), new Vec3f(-0.8571674f, 0f, -0.5150387f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49695.09f, 4990.198f, 19971.78f), new Vec3f(-0.8571674f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49797.58f, 4990.198f, 19800.8f), new Vec3f(-0.8571674f, 0f, -0.5150387f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49489.12f, 4992.925f, 19850.8f), new Vec3f(-0.8571674f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49591.61f, 4992.925f, 19679.82f), new Vec3f(-0.8571674f, 0f, -0.5150387f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48957.13f, 4992.737f, 20294.3f), new Vec3f(-0.8571674f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49059.63f, 4992.737f, 20123.32f), new Vec3f(-0.8571674f, 0f, -0.5150387f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49306.2f, 4992.681f, 19739.59f), new Vec3f(-0.8571674f, 0f, -0.5150381f));

            mi = new MobInter("BENCH_NW_CITY_01.ASC", "MOBNAME_BENCH", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49408.69f, 4992.681f, 19568.61f), new Vec3f(-0.8571674f, 0f, -0.5150387f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48161.17f, 4990.876f, 21824.15f), new Vec3f(-0.8779828f, 0f, -0.4786923f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(47904.63f, 4368.997f, 17284.36f), new Vec3f(0.5299207f, 0f, -0.8480499f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(47350.5f, 4366.997f, 18261.04f), new Vec3f(-0.5000004f, 0f, 0.866026f));

            mi = new MobInter("PAN_OC.MDS", "MOBNAME_PAN", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(37217.36f, 2937.862f, -24672.27f), new Vec3f(0.03542408f, 0.1219662f, 0.9919022f));

            mi = new MobInter("RMAKER_1.MDS", "MOBNAME_RUNEMAKER", ItemInstance.getItemInstance("ITMI_RUNEBLANK"), null, true, false);
            mi.Spawn(mapName, new Vec3f(-9266.13f, -114.9878f, -17456.28f), new Vec3f(-0.6427875f, 0f, 0.7660442f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-9473f, -116f, -17488f), new Vec3f(-0.01745244f, 0f, -0.9998462f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-9651.209f, 19.37242f, -17463.69f), new Vec3f(-0.6073759f, 0f, -0.7944143f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-9287.535f, 389.6366f, -16405.55f), new Vec3f(-0.9961947f, 0f, 0.08715581f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-10851.7f, 401.2474f, -16352.06f), new Vec3f(-0.9986297f, 0f, -0.05233543f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-9426.024f, 392.1649f, -16952.05f), new Vec3f(-0.9612616f, 0f, 0.2756379f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-10776.36f, 390.2431f, -16898.72f), new Vec3f(-0.9876884f, 0f, -0.1564339f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-9107.464f, -114.1787f, -15367.47f), new Vec3f(-0.9743698f, 0f, 0.224952f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-9212.384f, -113.7159f, -13133.6f), new Vec3f(-0.9743702f, 0f, -0.2249501f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-10235.24f, 462.0741f, -12336.3f), new Vec3f(-0.1218703f, 0f, -0.9925458f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-9649.277f, 461.9355f, -12337.18f), new Vec3f(0.05233496f, 0f, -0.9986293f));

            mi = new MobInter("BENCH_3_OC.ASC", "MOBNAME_BENCH", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-10180.21f, 1034.155f, -19312.41f), new Vec3f(0.08715571f, 0f, 0.9961945f));

            mi = new MobInter("BSANVIL_OC.MDS", "MOBNAME_ANVIL", ItemInstance.getItemInstance("ITMISWORDRAWHOT"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-9795.077f, -113.766f, -15165.34f), new Vec3f(0.4999995f, 0f, 0.8660245f));

            mi = new MobInter("BSCOOL_OC.MDS", "MOBNAME_BUCKET", ItemInstance.getItemInstance("ITMISWORDBLADEHOT"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-9861.72f, -114.9912f, -15591.23f), new Vec3f(-0.7660444f, 0f, 0.6427875f));

            mi = new MobInter("BSFIRE_OC.MDS", "MOBNAME_FORGE", ItemInstance.getItemInstance("ITMISWORDRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-10173.2f, -113.4707f, -15619.17f), new Vec3f(-8.568168E-08f, 0f, -0.9999997f));

            mi = new MobInter("BSSHARP_OC.MDS", "MOBNAME_GRINDSTONE", ItemInstance.getItemInstance("ITMISWORDBLADE"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-10323.22f, -113.2857f, -15200.83f), new Vec3f(-0.6427867f, 0f, 0.7660435f));

            mi = new MobInter("CAULDRON_OC.ASC", "MOBNAME_CAULDRON", ItemInstance.getItemInstance("ITMI_SCOOP"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-10863.69f, 195.0945f, -18102.18f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(-10761.13f, 194.0945f, -18241.21f), new Vec3f(0.05233599f, 0f, -0.9986306f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10517.36f, 193.8867f, -18180.99f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10206.89f, 193.8867f, -18202.37f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10800.9f, 198.8867f, -17820.07f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10520.87f, 194.8867f, -17799.73f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10335.05f, 193.8867f, -17832.2f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10054.6f, 193.8867f, -17817.87f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-9487.478f, 192.606f, -17918.57f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-9619.59f, 193.606f, -18131.45f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_SEAT", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-9935.792f, 723.5594f, -19640.12f), new Vec3f(0.7901477f, 0f, -0.6129017f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10511.75f, 702.5858f, -18938.46f), new Vec3f(-0.7313536f, 0f, 0.6819983f));

            mi = new MobInter("PAN_OC.MDS", "MOBNAME_PAN", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(80785.52f, 5007.796f, 26692.62f), new Vec3f(0.8090177f, 0f, 0.5877857f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(59721.39f, 6836.476f, 40533.66f), new Vec3f(0.8386817f, 0.003925939f, -0.5446067f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(59333.14f, 3143.092f, 12065.01f), new Vec3f(0.9999998f, 0f, 2.980232E-08f));

            mi = new MobInter("INNOS_NW_MISC_01.ASC", "MOBNAME_INNOS", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(70253.66f, 4359.196f, 22198.17f), new Vec3f(-0.6819981f, 0f, -0.7313535f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(84002.05f, 3495.788f, 28770.57f), new Vec3f(0.9848076f, 0f, 0.173649f));

            mi = new MobInter("RMAKER_1.MDS", "MOBNAME_RUNEMAKER", ItemInstance.getItemInstance("ITMI_RUNEBLANK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(83889.2f, 3493.798f, 29011.97f), new Vec3f(-0.7660442f, 0f, -0.6427875f));

            mi = new MobInter("BARBQ_SCAV.MDS", "MOBNAME_BBQ_SCAV", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(83545.62f, 3488.009f, 28626.47f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("TOUCHPLATE_STONE.MDS", "MOBNAME_DOOR", ItemInstance.getItemInstance("ITKE_PORTALTEMPELWALKTHROUGH_ADDON"), "EVT_NW_TROLLAREA_TEMPELDOOR_MASTER_01", true, false);
            mi.Spawn(mapName, new Vec3f(82508.48f, 3462.952f, 23365.96f), new Vec3f(-0.01745219f, 0f, -0.9998483f));

            mi = new MobInter("TOUCHPLATE_STONE.MDS", "", ItemInstance.getItemInstance("ITMI_PORTALRING_ADDON"), "EVT_ADDON_TROLLPORTAL_GAME_EVENT_START_01", true, false);
            mi.Spawn(mapName, new Vec3f(82110.41f, 3674.937f, 29394.16f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29417.23f, 6031.176f, -15698.12f), new Vec3f(-0.9271851f, 0f, -0.3746068f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30101.43f, 6028.541f, -16010.26f), new Vec3f(0.4383713f, 0f, -0.898795f));

            mi = new MobInter("CHAIR_NW_NORMAL_01.ASC", "MOBNAME_CHAIR", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(17699.52f, 3159.393f, -21339.03f), new Vec3f(0f, 0f, 1f));

            mi = new MobInter("THRONE_NW_CITY_01.ASC", "MOBNAME_ARMCHAIR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30128.33f, 5148.463f, -14811.7f), new Vec3f(-0.1736485f, 0f, -0.9848083f));

            mi = new MobInter("STOVE_NW_CITY_01.ASC", "MOBNAME_STOVE", ItemInstance.getItemInstance("ITFOMUTTONRAW"), null, true, true);
            mi.Spawn(mapName, new Vec3f(29817f, 5148f, -14369f), new Vec3f(0.2850204f, 0f, 0.958526f));

            mi = new MobInter("LAB_PSI.ASC", "MOBNAME_LAB", ItemInstance.getItemInstance("ITMI_FLASK"), null, true, true);
            mi.Spawn(mapName, new Vec3f(30593.36f, 5150.527f, -15496.24f), new Vec3f(0.9945223f, 0f, 0.1045285f));

            mi = new MobInter("RMAKER_1.MDS", "MOBNAME_RUNEMAKER", ItemInstance.getItemInstance("ITMI_RUNEBLANK"), null, true, false);
            mi.Spawn(mapName, new Vec3f(30539.96f, 5148.629f, -15813.44f), new Vec3f(-0.5877857f, 0f, 0.8090175f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29307.2f, 5281.791f, -15363.19f), new Vec3f(-0.9975644f, 0f, 0.06975664f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29474.74f, 5278.664f, -15933.73f), new Vec3f(-0.7834781f, 0f, -0.6214219f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30428.54f, 6031.79f, -15730.1f), new Vec3f(0.9702956f, 0f, -0.2419221f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29708.26f, 6034.476f, -15998.89f), new Vec3f(-0.5735762f, 0f, -0.819152f));

            mi = new MobInter("BOOK_NW_CITY_CUPBOARD_01.ASC", "MOBNAME_BOOKSBOARD", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29733.91f, 6030.213f, -15047.91f), new Vec3f(0.104529f, 0f, 0.9945225f));

            mi = new MobInter("INNOS_BELIAR_ADDON_01.ASC", "MOBNAME_ADDON_IDOL", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(29655.37f, 5924.062f, -14564.92f), new Vec3f(-0.9975645f, 0f, 0.0697563f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 250, 5 }, true, null, "RLLRRLLLRRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(682.9697f, -426.3414f, 24026.74f), new Vec3f(0.4574393f, 0.2249511f, 0.8603178f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_AXE"), ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_BACON") }, new int[] { 26, 1, 1, 1 }, true, null, "RLLRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-2327.38f, -358.8218f, 20525.63f), new Vec3f(-0.6020718f, 0.229194f, -0.7648402f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUM.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 32 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-2841.872f, -442.1248f, 12826.73f), new Vec3f(-0.9781476f, 0f, -0.2079116f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUM.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITWR_STONEPLATECOMMON_ADDON"), ItemInstance.getItemInstance("ITMI_PITCH"), ItemInstance.getItemInstance("ITFO_CHEESE") }, new int[] { 17, 1, 1, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-3224.33f, -422.2288f, 11208.11f), new Vec3f(0.806656f, 0.1549121f, 0.5703575f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMW_1H_VLK_SWORD"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 2, 1, 1, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(1573.009f, -181.6234f, -2755.593f), new Vec3f(0.7660446f, 0f, -0.6427876f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_BLOODCUP_MIS"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 150 }, true, ItemInstance.getItemInstance("ITKE_BUERGER"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(11559.29f, 899.5332f, 3521.911f), new Vec3f(-0.8987944f, 0f, -0.4383712f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_BOLT") }, new int[] { 50 }, true, null, "RLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6007.365f, 750.996f, 9204.42f), new Vec3f(0.8480488f, 0f, 0.5299197f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_FISH"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 2, 75 }, true, null, "RLLRRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(5761.686f, 750.5546f, 6636.632f), new Vec3f(0.9135455f, 0f, -0.4067365f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_CITY_TOWER_06"), ItemInstance.getItemInstance("ITMI_MARIASGOLDPLATE"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 50 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2613.417f, 750.5546f, 7108.611f), new Vec3f(0.358368f, 0f, 0.9335807f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 50 }, true, null, "RLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2833.434f, 750.5546f, 6695.861f), new Vec3f(0.3907323f, 0f, -0.9205073f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_CORAGONSSILBER"), ItemInstance.getItemInstance("ITRW_BOW_L_03_MIS") }, new int[] { 8, 1 }, true, null, "RRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2510.319f, -641.9491f, 6589.773f), new Vec3f(-0.06275089f, 0f, -0.9980291f));

            mi = new MobContainer("CHESTBIG_NW_RICH_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITAR_VLK_H"), ItemInstance.getItemInstance("ITAR_VLK_M"), ItemInstance.getItemInstance("ITAR_VLK_L"), ItemInstance.getItemInstance("ITFO_SAUSAGE"), ItemInstance.getItemInstance("ITMI_FLASK") }, new int[] { 1, 3, 5, 15, 10 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(5660.43f, 261.868f, -5692.02f), new Vec3f(0.8178486f, 0.01736571f, -0.5751721f));

            mi = new MobContainer("CHESTBIG_NW_RICH_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_PLIERS"), ItemInstance.getItemInstance("ITMI_HAMMER"), ItemInstance.getItemInstance("ITLSTORCH"), ItemInstance.getItemInstance("ITAR_LEATHER_L") }, new int[] { 3, 5, 15, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(5573.449f, 268.868f, -6033.792f), new Vec3f(0.5446054f, 0f, 0.838692f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRI_PROT_POINT_01_MIS") }, new int[] { 20, 1 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6612.918f, 122.0018f, 28.72816f), new Vec3f(0.5446727f, 0f, -0.8386492f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 36, 1, 1 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6107.376f, 268.1922f, -157.9734f), new Vec3f(-0.8660059f, 0f, -0.5000346f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_SULFUR"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 2, 1 }, true, null, "LL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4042.534f, -113.741f, -6830.253f), new Vec3f(-0.9817718f, 0f, -0.1900802f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 3 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4621.383f, -115.851f, -3451.401f), new Vec3f(-0.4067004f, 0f, -0.9135613f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01") }, new int[] { 5 }, true, ItemInstance.getItemInstance("ITKE_STORAGE"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2586.069f, 110.2284f, 2492.737f), new Vec3f(-0.06975637f, 0f, 0.9975643f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_HERBPAKET") }, new int[] { 1 }, true, ItemInstance.getItemInstance("ITKE_STORAGE"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3160.189f, 110.8121f, 2693.969f), new Vec3f(0.2249511f, 0f, 0.9743701f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 2, 1 }, true, null, "LRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(817.9002f, -187.134f, -5529.48f), new Vec3f(-0.8299153f, 0.02328978f, -0.5574083f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 1, 1 }, true, null, "RLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(871.4547f, -188.8853f, -5318.052f), new Vec3f(0.7277411f, -0.003188853f, 0.6858459f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_SULFUR"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 2, 1 }, true, null, "LLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2669.192f, -187.8879f, -4953.99f), new Vec3f(-0.4838389f, 0.00268502f, -0.8751523f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 3, 1 }, false, null, "RRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3297.38f, -114.4416f, -5107.778f), new Vec3f(0.92049f, 0f, 0.390768f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_SULFUR"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 2, 1 }, true, null, "LL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2920.709f, -116.6508f, -5975.174f), new Vec3f(0.99982f, -0.005298649f, -0.01819471f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER"), ItemInstance.getItemInstance("ITFO_STEW") }, new int[] { 1, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3166.694f, -184.0383f, -909.3986f), new Vec3f(0.01150694f, -0.001476236f, 0.9999298f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_APPLE"), ItemInstance.getItemInstance("ITFO_STEW") }, new int[] { 10, 3, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3394.079f, -186.267f, 1494.399f), new Vec3f(0.004077423f, 0f, -0.9999917f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 4, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(4604.301f, -187.4268f, 243.1912f), new Vec3f(-0.4845489f, 0.03489953f, 0.8740678f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 3 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3188.372f, -114.5099f, -3590.447f), new Vec3f(0.857146f, 0f, 0.5150716f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 3, 7 }, true, null, "RL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3529.466f, -191.0055f, 1953.687f), new Vec3f(-0.9411236f, 0.1372427f, -0.3089546f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITPO_MANA_02") }, new int[] { 42, 13, 2 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8267.586f, 271.4124f, -2799.094f), new Vec3f(-0.866008f, 0f, -0.5000359f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 32 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7654.307f, 126.2051f, 1238.297f), new Vec3f(0.9135287f, 0f, 0.406773f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 25, 25, 1, 1, 1 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7086.85f, 269.0717f, -2130.404f), new Vec3f(0.4226543f, 0f, -0.906291f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 3, 4, 1 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6412.64f, 267.5302f, -5302.119f), new Vec3f(-0.1045683f, 0f, 0.9945181f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 41, 1, 1 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(5625.557f, 269.909f, -4159.813f), new Vec3f(0.249405f, 0f, -0.9684001f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITSC_ICEBOLT"), ItemInstance.getItemInstance("ITSC_CHARM"), ItemInstance.getItemInstance("ITSC_SLEEP"), ItemInstance.getItemInstance("ITSC_LIGHT") }, new int[] { 50, 1, 1, 2, 2, 1 }, true, null, "LRRLRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(8223.502f, 268.954f, 3600.449f), new Vec3f(0.9902633f, 0f, 0.1392129f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITSC_CHARGEFIREBALL"), ItemInstance.getItemInstance("ITSC_LIGHTHEAL"), ItemInstance.getItemInstance("ITSC_INSTANTFIREBALL"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 2, 100 }, true, null, "LRLLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7432.637f, -642.8083f, 5009.127f), new Vec3f(0.3007444f, 0f, -0.9537067f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 50 }, true, null, "RRRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10017.85f, -642.1121f, 3071.128f), new Vec3f(-0.8048728f, 0f, -0.5934525f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_SILVERCUP"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 100 }, true, null, "LLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10039.48f, -639.5757f, 2876.737f), new Vec3f(-0.9999629f, 0f, 0.008687093f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 25, 25, 75 }, true, null, "LRRRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(9066.751f, -641.8978f, 2152.418f), new Vec3f(0.725347f, 0f, 0.6883829f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 100 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(10548.31f, 899.5892f, -3311.969f), new Vec3f(-0.7193696f, 0f, 0.6946319f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_BLOODCUP_MIS"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 100 }, true, null, "RRLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12360.44f, 1299.009f, -4544.752f), new Vec3f(-0.9986297f, 0f, -0.05237529f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 50 }, true, null, "LLRRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12834.18f, 1299.445f, -4820.873f), new Vec3f(0.06971753f, 0f, 0.9975682f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITPO_MANA_01") }, new int[] { 50, 30, 1 }, true, null, "LRRLRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15290.21f, 900.3315f, -4106.856f), new Vec3f(-0.8571884f, 0f, 0.5150047f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITPO_HEALTH_02"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 3, 2, 50 }, true, ItemInstance.getItemInstance("ITKE_PALADINTRUHE"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14854.95f, 1326.607f, -4797.214f), new Vec3f(-0.3090544f, 0f, 0.9510451f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITWR_MAP_NEWWORLD") }, new int[] { 25, 1 }, true, null, "LLRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(1519.275f, -194.4613f, 92.10081f), new Vec3f(-0.9693002f, 0f, -0.2458755f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFOMUTTON") }, new int[] { 5, 70, 5 }, true, null, "RLLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(11069.57f, 265.0967f, 5327.329f), new Vec3f(-0.8746195f, 0f, -0.4848094f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 2, 3, 70 }, true, null, "RLLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(7413.851f, 266.0967f, -6397.586f), new Vec3f(-0.5299204f, 0f, 0.8480495f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_BLOODCUP_MIS"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 1, 31, 13, 1, 1, 1 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3488.674f, -115.1453f, -2151.905f), new Vec3f(-0.8829294f, 0f, -0.4695068f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_BLOODCUP_MIS"), ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITPO_MANA_02"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 3, 1, 1, 72 }, true, ItemInstance.getItemInstance("ITKE_RICHTER"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(16313.37f, 900.5078f, -3030.249f), new Vec3f(0.9902698f, 0f, -0.1391737f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_02"), ItemInstance.getItemInstance("ITPO_MANA_02"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 1, 50 }, true, ItemInstance.getItemInstance("ITKE_SALANDRIL"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(12695.19f, 897.6389f, 2804.033f), new Vec3f(0.309017f, 0f, 0.9510567f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 50 }, true, null, "RRRRRRRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15705.56f, 1538.99f, 155.8144f), new Vec3f(0.1218694f, 0f, -0.9925458f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_PALADINTRUHE"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 1, 1 }, true, null, "RLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(13950.96f, 1328.767f, -3055.779f), new Vec3f(-0.4539918f, 0f, -0.8910088f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_GOLDCUP") }, new int[] { 150, 1 }, true, null, "RLLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-18826.35f, -339.9137f, 4706.103f), new Vec3f(-0.4999998f, 0f, -0.8660251f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_SILVERCUP"), ItemInstance.getItemInstance("ITMI_INNOSSTATUE"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 100 }, true, null, "RLLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-18628.91f, -338.3015f, 4582.442f), new Vec3f(-0.8660254f, 0f, -0.4999998f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_SILVERCUP"), ItemInstance.getItemInstance("ITMI_GOLDCHALICE"), ItemInstance.getItemInstance("ITMI_GOLDCHEST"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 2, 1, 1, 400 }, true, ItemInstance.getItemInstance("ITKE_THIEFTREASURE"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-18672.35f, -328.3015f, 4144.645f), new Vec3f(-0.1564344f, 0f, 0.9876879f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 100 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-18821.24f, -343.3015f, 4409.59f), new Vec3f(-0.9945219f, 0f, -0.1045283f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_PERM_DEX"), ItemInstance.getItemInstance("ITAM_STRG_01"), ItemInstance.getItemInstance("ITMI_GOLDCUP"), ItemInstance.getItemInstance("ITMI_SILVERCUP"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 1, 5, 300 }, true, null, "RRRLRRRLRLRLRRLLLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(6415.869f, -639.8768f, 4665.741f), new Vec3f(-0.7313544f, 0f, -0.6819987f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_BLOODCUP_MIS"), ItemInstance.getItemInstance("ITMI_SILVERCUP"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 150 }, true, ItemInstance.getItemInstance("ITKE_VALENTINO"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(11844.59f, 899.4477f, -4136.909f), new Vec3f(0.7431719f, 0f, -0.6691024f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_SILVERPLATE"), ItemInstance.getItemInstance("ITMI_SILVERCUP"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 2, 1, 100 }, true, null, "RLRRRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2339.62f, -177.1674f, -3001.217f), new Vec3f(-0.9848076f, 0f, -0.1736481f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "Chest", new ItemInstance[] { ItemInstance.getItemInstance("ITAR_BAU_L"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_CHEESE"), ItemInstance.getItemInstance("ITFO_APPLE") }, new int[] { 1, 2, 1, 2 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14056.03f, 1591.758f, -13911.69f), new Vec3f(-0.04318183f, -0.0008421556f, 0.9990717f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 20 }, false, null, "LRRL", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5283.209f, 420.6029f, -23775.72f), new Vec3f(0.2465226f, 0.06201322f, 0.9671561f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_AXE"), ItemInstance.getItemInstance("ITFO_BACON") }, new int[] { 2, 17, 4, 1, 1 }, true, null, "RLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-1642.609f, 553.5197f, -9346.222f), new Vec3f(-0.642788f, 0f, 0.766045f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUMLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_RUNEBLANK") }, new int[] { 1 }, true, ItemInstance.getItemInstance("ITKE_RUNE_MIS"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(31946.08f, 3088.209f, -779.5698f), new Vec3f(0f, 0f, 1f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_BOW_M_01"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 1, 36, 5 }, true, null, "RLLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(26788.88f, 4404.663f, -4144.01f), new Vec3f(0.3201032f, -0.1391519f, 0.9371079f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUMLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 3, 13, 1 }, true, null, "LRLLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(31954.01f, 3111.244f, -326.0641f), new Vec3f(-0.06975649f, 0f, -0.9975637f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 3, 31, 23, 23, 1 }, true, null, "LRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(20624.13f, 817.6805f, 4820.414f), new Vec3f(0.7071072f, 0f, 0.7071074f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 54, 3 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30382.18f, 3146.305f, 29846.89f), new Vec3f(-0.1736481f, 0f, -0.9848074f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_PITCH") }, new int[] { 350, 1 }, true, null, "RRLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30263.65f, 3146.305f, 29876.08f), new Vec3f(-0.642787f, 0f, -0.7660434f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_FISH"), ItemInstance.getItemInstance("ITFO_SMELLYFISH") }, new int[] { 10, 3, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(3201.645f, 98.92307f, 16448.55f), new Vec3f(0.636488f, 0f, -0.7712862f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMW_1H_VLK_SWORD"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 5, 1, 1, 1 }, false, null, "RLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-1763.489f, 2407.624f, 16387.08f), new Vec3f(0.8571681f, 0f, 0.5150385f));

            mi = new MobContainer("CHESTBIG_NW_RICH_OPEN.MDS", "CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_CHEESE"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 2, 3, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(31523.58f, 3440.328f, 8715.939f), new Vec3f(-0.9945256f, 1.129873E-08f, 0.1044886f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_FISH"), ItemInstance.getItemInstance("ITFO_HALVORFISH_MIS") }, new int[] { 25, 3, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15260.08f, 2874.005f, 11226.21f), new Vec3f(0.936858f, 0f, -0.3497091f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 1, 10, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(37691f, 3826.921f, -2740.775f), new Vec3f(-0.03263185f, 0.004166675f, 0.9994625f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_STEW"), ItemInstance.getItemInstance("ITFO_SAUSAGE"), ItemInstance.getItemInstance("ITFO_MILK"), ItemInstance.getItemInstance("ITFO_BEER"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 3, 2, 3, 5, 32 }, false, null, "RLLRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(38360.72f, 3821.717f, -2687.377f), new Vec3f(-0.9205216f, 0f, 0.390695f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1 }, true, null, "LL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(37020.46f, 4103.128f, -2728.416f), new Vec3f(0f, 0f, 1f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_SLD_AXE"), ItemInstance.getItemInstance("ITMI_PITCH"), ItemInstance.getItemInstance("ITFO_CHEESE") }, new int[] { 17, 1, 1, 1 }, true, null, "RRLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(37141.45f, 4103.128f, -1975.59f), new Vec3f(-0.9848072f, 0f, -0.173648f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMW_1H_VLK_SWORD"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 2, 1, 1, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(36792.15f, 4109.55f, -1882.401f), new Vec3f(1.062691E-07f, 0f, -0.9999993f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 20, 17 }, true, null, "RLLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30217.9f, 4201.882f, -1348.192f), new Vec3f(0.9848071f, 0f, 0.1736482f));

            mi = new MobContainer("CHESTSMALL_OCCHESTSMALLLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ADDON_FIREARROW") }, new int[] { 17 }, true, null, "LRRLLLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(67889.05f, 5520.293f, -21125.88f), new Vec3f(0.8090169f, 0f, -0.5877854f));

            mi = new MobContainer("CHESTSMALL_OCCHESTSMALL.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 12, 22 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(44218.95f, 2905.686f, 2129.868f), new Vec3f(-0.3746057f, 0f, 0.9271812f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 16, 15 }, true, null, "LRLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(66858.81f, 2484.566f, -22364.13f), new Vec3f(0.9396925f, 0f, 0.3420201f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITPO_HEALTH_02"), ItemInstance.getItemInstance("ITPO_MANA_02"), ItemInstance.getItemInstance("ITMI_QUARTZ") }, new int[] { 101, 43, 3, 2, 2 }, true, ItemInstance.getItemInstance("ITKE_EVT_CRYPT_02"), null, null, "EVT_CRYPT_02_TRIGGER", true, true);
            mi.Spawn(mapName, new Vec3f(72816.76f, 2663.238f, 1062.206f), new Vec3f(0.5446733f, 0f, -0.8386506f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_HEALTH_02"), ItemInstance.getItemInstance("ITMI_QUARTZ") }, new int[] { 1, 95, 2, 2 }, true, ItemInstance.getItemInstance("ITKE_EVT_CRYPT_01"), null, null, "EVT_CRYPT_01_TRIGGER", true, true);
            mi.Spawn(mapName, new Vec3f(72361.1f, 2660.929f, -1301.036f), new Vec3f(0.8386507f, 0f, 0.5446735f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_ROCKCRYSTAL"), ItemInstance.getItemInstance("ITFO_WINE") }, new int[] { 28, 1, 1, 3 }, true, null, "LRRLRLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(62905.66f, 4278.731f, -38406.3f), new Vec3f(-0.5878179f, 8.294935E-09f, 0.8089929f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_BOLT") }, new int[] { 10, 11 }, true, null, "RLLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(58694.29f, 4134.473f, -33400.75f), new Vec3f(0.9122887f, 0.02349698f, 0.4088727f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 3, 9 }, true, null, "LLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(59912.11f, 3879.051f, -31208.07f), new Vec3f(0.9961916f, 0f, 0.08719576f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_PLIERS"), ItemInstance.getItemInstance("ITMI_SAW"), ItemInstance.getItemInstance("ITMI_HAMMER"), ItemInstance.getItemInstance("ITMISWORDRAW") }, new int[] { 1, 1, 1, 7 }, true, null, "LRLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(66072.34f, 3834.455f, -25002.83f), new Vec3f(-0.9902689f, 0f, -0.1391712f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_PLIERS"), ItemInstance.getItemInstance("ITMI_SAW"), ItemInstance.getItemInstance("ITMI_HAMMER"), ItemInstance.getItemInstance("ITMISWORDRAW") }, new int[] { 1, 1, 1, 2 }, true, null, "LRLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(64203.06f, 3982.553f, -21519.15f), new Vec3f(-0.03490149f, 0f, -0.9993924f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 11 }, false, null, "LRRLLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75228.87f, 3512.244f, -10401.11f), new Vec3f(0.3419827f, 0f, 0.9397056f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_2H_SLD_AXE"), ItemInstance.getItemInstance("ITMW_2H_SLD_SWORD"), ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 59, 1, 1, 26, 21 }, true, null, "RLLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75304.4f, 3832.253f, -13185.37f), new Vec3f(-0.9455311f, 0f, 0.3255305f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFOMUTTONRAW"), ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITFO_CHEESE") }, new int[] { 1, 2, 3 }, false, null, "LRRLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(70731.96f, 3151.7f, -8902.571f), new Vec3f(0.9925414f, 0f, 0.1219093f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 1, 13, 1 }, true, null, "LRRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(76350.39f, 3512.384f, -10847.26f), new Vec3f(-0.9970326f, 0.01501146f, -0.07549931f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 3, 11, 1 }, false, null, "LLRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75389.32f, 3514.653f, -12363.07f), new Vec3f(0.4848583f, 0.009248429f, -0.8745683f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_HEALTH_01") }, new int[] { 22, 1 }, true, null, "LRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(76131.8f, 3513.398f, -12317.56f), new Vec3f(-0.9986264f, 0f, -0.05237531f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER"), ItemInstance.getItemInstance("ITMI_PITCH"), ItemInstance.getItemInstance("ITFO_STEW") }, new int[] { 5, 1, 1, 1 }, true, null, "RLRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(74959.43f, 3514.899f, -13521.33f), new Vec3f(-0.08671136f, -0.04126925f, 0.9953776f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_PLIERS"), ItemInstance.getItemInstance("ITMI_SAW"), ItemInstance.getItemInstance("ITMI_HAMMER"), ItemInstance.getItemInstance("ITMISWORDRAW") }, new int[] { 1, 1, 1, 5 }, true, null, "LRLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(72568.13f, 3151.581f, -10008.7f), new Vec3f(0.0523358f, 0f, -0.9986306f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFOMUTTONRAW"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 1, 3, 4 }, false, null, "LRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(71227.58f, 3152.075f, -8688.286f), new Vec3f(-0.2923722f, 0f, -0.9563055f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_HEALTH_01") }, new int[] { 210, 1 }, true, null, "LRLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(74967.64f, 3829.954f, -9473.867f), new Vec3f(-0.6156321f, 0f, -0.7880333f));

            mi = new MobContainer("CHESTBIG_NW_RICH_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_WINE"), ItemInstance.getItemInstance("ITFO_CHEESE") }, new int[] { 3, 6, 3, 5 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75945.92f, 3832.954f, -10071.08f), new Vec3f(-0.7313858f, -0.004222146f, 0.6819806f));

            mi = new MobContainer("CHESTBIG_NW_RICH_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_FISH"), ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITFO_WINE"), ItemInstance.getItemInstance("ITFO_CHEESE") }, new int[] { 6, 3, 4, 2 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75630.96f, 3830.954f, -9383.574f), new Vec3f(0.4540195f, 0f, -0.8909912f));

            mi = new MobContainer("CHESTBIG_NW_RICH_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITFO_WINE") }, new int[] { 8, 2 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(75896.85f, 3831.363f, -9395.996f), new Vec3f(-0.6819768f, 0f, -0.7313735f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_SILVERCUP"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 1, 1, 30 }, true, ItemInstance.getItemInstance("ITKE_DEXTER"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(67499.38f, 3968.758f, -22104.74f), new Vec3f(0.52992f, 0f, 0.848049f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 1, 23, 3 }, true, null, "LRRLLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(84027.9f, 4273.219f, -11226.79f), new Vec3f(0.9764391f, -0.106097f, -0.1879089f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_SWORD"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 2, 32, 1, 1, 1 }, true, null, "RLRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(85181.44f, 4278.219f, -10015.64f), new Vec3f(0.2367961f, -0.001901135f, 0.9715572f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 1, 13, 1 }, true, null, "LRRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(86150.63f, 4279.219f, -9583.963f), new Vec3f(-0.7299767f, 0.004210358f, -0.6834594f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 20, 19 }, true, null, "RLLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(56622.63f, 1904.617f, -2091.319f), new Vec3f(0.2733977f, 0.0007509104f, 0.961901f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_FLASK"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITRI_MANA_01"), ItemInstance.getItemInstance("ITRU_SUMGOBSKEL"), ItemInstance.getItemInstance("ITWR_XARDASBOOKFORPYROKAR_MIS") }, new int[] { 25, 1, 5, 1, 1, 1 }, true, ItemInstance.getItemInstance("ITKE_CHEST_SEKOB_XARDASBOOK_MIS"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(57121.37f, 1899.66f, -984.5163f), new Vec3f(-0.2413309f, 0.01191469f, -0.9703695f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITPO_MANA_02"), ItemInstance.getItemInstance("ITMI_QUARTZ") }, new int[] { 81, 23, 2, 2 }, true, ItemInstance.getItemInstance("ITKE_EVT_CRYPT_03"), null, null, "EVT_CRYPT_03_TRIGGER", true, true);
            mi.Spawn(mapName, new Vec3f(75175.91f, 2661.636f, 618.6479f), new Vec3f(-0.8290153f, 0f, -0.5592259f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_SULFUR"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 1, 1, 1 }, true, null, "LLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(58644.45f, 1635.428f, 11132.61f), new Vec3f(0.866025f, 0f, -0.4999997f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_BOLT"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 23, 10, 13 }, true, null, "RLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(58804.2f, 1630.428f, 11224.33f), new Vec3f(-0.1736481f, 0f, -0.9848074f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUMLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_BOLT") }, new int[] { 10, 17 }, true, null, "RLLRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45545.21f, 3039.665f, 3433.785f), new Vec3f(-0.9876882f, 0f, 0.1564346f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUMLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_ROCKCRYSTAL"), ItemInstance.getItemInstance("ITFOMUTTONRAW"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 1, 1, 1 }, true, null, "LRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(45308.33f, 3041f, 3676.858f), new Vec3f(-0.06975654f, 0f, -0.9975638f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUM.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 20, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(81738.89f, 4150.927f, -19413.7f), new Vec3f(0.978148f, 0f, -0.2079118f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUM.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER"), ItemInstance.getItemInstance("ITMI_SULFUR") }, new int[] { 3, 9, 1, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(70172.66f, 1682.492f, -24365f), new Vec3f(0.2419221f, 0f, 0.9702958f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUMLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_ROCKCRYSTAL"), ItemInstance.getItemInstance("ITFO_WINE") }, new int[] { 8, 1, 1, 1 }, true, null, "LRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(70335.11f, 1675.708f, -24400.72f), new Vec3f(0f, 0f, 1f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUMLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_HEALTH_01") }, new int[] { 6, 10, 1 }, true, null, "LRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48925.22f, 1300.37f, 6317.779f), new Vec3f(0.9848076f, 0f, 0.1736481f));

            mi = new MobContainer("CHESTBIG_OCCHESTMEDIUMLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_SLD_AXE"), ItemInstance.getItemInstance("ITMI_PITCH"), ItemInstance.getItemInstance("ITFO_CHEESE") }, new int[] { 17, 1, 1, 1 }, true, null, "RRLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48794.14f, 1285.37f, 6554.041f), new Vec3f(0.9848073f, 0f, 0.1736481f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITMI_PITCH"), ItemInstance.getItemInstance("ITMW_1H_VLK_MACE") }, new int[] { 35, 1, 1, 1 }, true, null, "LRLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(58960.91f, 1883.875f, -29030.11f), new Vec3f(-0.5f, 0f, 0.8660253f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 38 }, true, null, "LLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(58719.45f, 1893.875f, -29087.05f), new Vec3f(0f, 0f, 1f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_MANA_02"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 3, 26, 1 }, true, null, "LRRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51821.21f, 4990.391f, 19097.85f), new Vec3f(-0.6560584f, 0f, -0.7547107f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_MANA_02"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 3, 26, 1 }, true, null, "RLRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51511.95f, 4990.391f, 19639.13f), new Vec3f(-0.8746197f, 0f, -0.4848112f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_MANA_03"), ItemInstance.getItemInstance("ITSC_SUMSKEL"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITSC_CHARGEFIREBALL") }, new int[] { 3, 1, 42, 3 }, true, null, "RLLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49417.79f, 4991.992f, 21210.12f), new Vec3f(0f, 0f, 1f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 45 }, true, null, "LRLLRRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50262.59f, 4992.913f, 21728.86f), new Vec3f(-0.984808f, 0f, 0.1736483f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_MANA_02"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 3, 26, 1 }, false, null, "RRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49458.47f, 4992.686f, 23078.89f), new Vec3f(-0.2923718f, 0f, -0.9563063f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { }, new int[] { }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50790.23f, 4989.484f, 18508.86f), new Vec3f(-0.8480498f, 0f, -0.5299203f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_SCHAFSWURST") }, new int[] { 14 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(50785.78f, 4241.588f, 19111.98f), new Vec3f(-0.4539907f, 0f, 0.8910071f));

            mi = new MobContainer("CHESTSMALL_OCCHESTSMALLLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { }, new int[] { }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49063.9f, 4990.882f, 17317.29f), new Vec3f(-0.8987947f, 0f, -0.4383714f));

            mi = new MobContainer("CHESTSMALL_OCCHESTSMALLLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 20, 1 }, true, null, "LR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48923.74f, 4988.882f, 16759.24f), new Vec3f(-0.4383712f, 0f, 0.8987944f));

            mi = new MobContainer("CHESTSMALL_OCCHESTSMALLLOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { }, new int[] { }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48573.17f, 4991.566f, 17041.96f), new Vec3f(0.8386714f, 0f, 0.5446395f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_03"), ItemInstance.getItemInstance("ITPO_MANA_03"), ItemInstance.getItemInstance("ITMI_QUARTZ"), ItemInstance.getItemInstance("ITFO_BOOZE"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_SULFUR") }, new int[] { 2, 3, 1, 1, 400, 1 }, false, null, "RLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(47616.48f, 3475.854f, 16584.18f), new Vec3f(0.1045288f, 0f, 0.9945229f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_JEWELERYCHEST"), ItemInstance.getItemInstance("ITMI_GOLDRING"), ItemInstance.getItemInstance("ITMI_GOLDCUP"), ItemInstance.getItemInstance("ITMI_GOLDPLATE"), ItemInstance.getItemInstance("ITMI_SULFUR") }, new int[] { 1, 1, 1, 1, 5 }, true, null, "RLRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49616.32f, 4241.308f, 18210.44f), new Vec3f(0.4539909f, 0f, 0.8910069f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_SULFUR") }, new int[] { 350, 1 }, true, null, "RLRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(49510.55f, 4238.308f, 18429.29f), new Vec3f(0.9925459f, 0f, -0.1218703f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITSC_TRFWOLF"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 2, 2, 32 }, false, null, "LRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48152.47f, 4995.622f, 22353.8f), new Vec3f(0.5135404f, 0f, -0.8580636f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_SULFUR"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 21, 1, 1, 1 }, true, null, "LLRR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(47481.58f, 4995.788f, 21581.76f), new Vec3f(0.9593117f, 0f, -0.2823403f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_IGARAZ", new ItemInstance[] { ItemInstance.getItemInstance("ITWR_BABOSDOCS_MIS") }, new int[] { 1 }, true, ItemInstance.getItemInstance("ITKE_IGARAZCHEST_MIS"), null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(48007.7f, 4989.755f, 21736.91f), new Vec3f(-0.7241709f, 0f, -0.6896202f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_HONEY"), ItemInstance.getItemInstance("ITFO_BREAD"), ItemInstance.getItemInstance("ITFO_SAUSAGE") }, new int[] { 3, 5, 10 }, true, null, "LRLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(51327.77f, 4243.015f, 19805.39f), new Vec3f(-0.8480473f, 0f, -0.52992f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 1, 20, 1 }, true, null, "LRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(36504.19f, 4680.061f, -38162.44f), new Vec3f(0f, 0f, 1f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_02"), ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 2, 25 }, true, null, "RLRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29298.7f, 5399.184f, -35018.42f), new Vec3f(0.9807845f, 0f, 0.1950903f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.ASC", "MOBNAME_CHEST", new ItemInstance[] { }, new int[] { }, true, null, "RLRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10478.75f, 723.1086f, -19432.53f), new Vec3f(0.9925473f, 0f, 0.1218695f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_QUARTZ") }, new int[] { 111, 23, 1, 1 }, true, null, "LRRLRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(82987.7f, 3547.792f, 22025.21f), new Vec3f(-0.4999993f, 0f, 0.8660246f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 55 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(60720.14f, 7086.126f, 24873.06f), new Vec3f(-0.5446404f, 0f, -0.83867f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 25, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(60576.73f, 7086.466f, 24902.72f), new Vec3f(-0.0697564f, 0f, -0.9975648f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_BOW_L_03"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMI_SULFUR"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 1, 1, 1, 1 }, true, null, "LLRRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(56935.32f, 6932.794f, 25788.09f), new Vec3f(0.1218695f, 0f, -0.992547f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMW_MORGENSTERN"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITSE_GOLEMCHEST_MIS") }, new int[] { 1, 150, 1 }, true, null, "LLRRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(84871.34f, 5451.599f, 33451.47f), new Vec3f(-0.9876892f, 0f, -0.1564338f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_ROCKCRYSTAL"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 1, 1, 1 }, true, null, "LRRRL", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(34849.65f, 5059.265f, 31437.37f), new Vec3f(0.7986363f, 0f, -0.6018159f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMI_ROCKCRYSTAL"), ItemInstance.getItemInstance("ITFOMUTTON"), ItemInstance.getItemInstance("ITFO_BOOZE") }, new int[] { 150, 1, 1, 1 }, true, null, "LRRRL", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(37788.73f, 5410.367f, 32117.73f), new Vec3f(-0.9563596f, -0.01091232f, -0.2919949f));

            mi = new MobContainer("CHESTBIG_NW_RICH_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_RUNEBLANK") }, new int[] { 1 }, true, ItemInstance.getItemInstance("ITKE_MAGICCHEST"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(66055.4f, 6712.728f, 46370.94f), new Vec3f(-0.7431452f, 0f, -0.6691309f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRI_PROT_FIRE_02"), ItemInstance.getItemInstance("ITFO_FISH") }, new int[] { 1, 1 }, true, null, "LRLRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(73994.27f, 5491.545f, 23710.38f), new Vec3f(-0.8386706f, 0f, 0.544639f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 11 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(79082.98f, 5479.306f, 34039.04f), new Vec3f(0.8660265f, 0f, 0.5000007f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 32, 1 }, false, null, "LRLLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(79042.31f, 5475.306f, 34133f), new Vec3f(0.9961962f, 0f, 0.08715593f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 100, 7 }, true, null, "RLLRLR", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(81407.68f, 5432.512f, 34724.04f), new Vec3f(0.1736487f, 0f, -0.9848092f));

            mi = new MobContainer("CHESTBIG_OCCHESTLARGELOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMW_ORKSCHLAECHTER"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_ARROW") }, new int[] { 1, 120, 17 }, true, null, "RLLRRRL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(74465.47f, 6678.921f, 28941.58f), new Vec3f(0.1736481f, 0f, -0.9848074f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITSC_LIGHTHEAL"), ItemInstance.getItemInstance("ITSC_INSTANTFIREBALL") }, new int[] { 20, 1, 1 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30045.9f, 5147.116f, -16555.56f), new Vec3f(-0.6944947f, 0f, 0.7194987f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITSC_LIGHT"), ItemInstance.getItemInstance("ITKE_LOCKPICK"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 5, 1, 2, 1 }, true, ItemInstance.getItemInstance("ITKE_XARDAS"), "LRLL", null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29465.3f, 5893.728f, -15319.75f), new Vec3f(0.9133196f, 0f, -0.4072479f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITPO_HEALTH_01"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 1, 5, 1 }, false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(24472.27f, 3109.886f, -21075.66f), new Vec3f(0.636488f, 0f, -0.7712862f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_LOCKED.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITRW_ARROW"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITRW_BOW_L_01") }, new int[] { 15, 29, 1 }, true, ItemInstance.getItemInstance("ITKE_BANDIT"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(17897.08f, 3163.391f, -20770.13f), new Vec3f(-0.6664597f, 0.007859284f, -0.7455016f));

            mi = new MobContainer("CHESTBIG_NW_NORMAL_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITLSTORCH"), ItemInstance.getItemInstance("ITMI_GOLD"), ItemInstance.getItemInstance("ITSC_LIGHTHEAL"), ItemInstance.getItemInstance("ITSC_INSTANTFIREBALL") }, new int[] { 1, 10, 1, 2 }, false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(29897.81f, 4248.067f, -14872.52f), new Vec3f(0.7661476f, 0.01090523f, 0.6425769f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITFO_APPLE"), ItemInstance.getItemInstance("ITPO_MANA_01"), ItemInstance.getItemInstance("ITMW_1H_VLK_DAGGER") }, new int[] { 1, 1, 1 }, false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(33911.71f, 3649.859f, -20389.39f), new Vec3f(0.2159216f, 0f, 0.976413f));

            mi = new MobContainer("CHESTSMALL_NW_POOR_OPEN.MDS", "MOBNAME_CHEST", new ItemInstance[] { ItemInstance.getItemInstance("ITMI_GOLD") }, new int[] { 35 }, false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(34442.09f, 3652.016f, -19768.42f), new Vec3f(-0.833594f, 0f, -0.552381f));

            mi = new MobBed("BEDHIGH_PSI.ASC", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-2918.516f, -406.6863f, 11381.18f), new Vec3f(0.9455191f, 0f, 0.3255681f));

            mi = new MobBed("BEDHIGH_PSI.ASC", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-2994.797f, -405.5036f, 11038.1f), new Vec3f(0.4067366f, 0f, -0.9135459f));

            mi = new MobBed("BEDHIGH_PSI.ASC", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-8271.111f, -144.1366f, -6358.561f), new Vec3f(0.9205052f, 0f, -0.3907311f));

            mi = new MobBed("BEDHIGH_PSI.ASC", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-3671.823f, -463.0626f, -19196.04f), new Vec3f(-0.777221f, 0f, 0.6292289f));

            mi = new MobBed("BEDHIGH_PSI.ASC", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-4158.208f, -443.8782f, -17211.82f), new Vec3f(-0.9062578f, 0f, -0.4227257f));

            mi = new MobBed("BEDHIGH_PSI.ASC", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-6126.924f, -212.4408f, -11405.77f), new Vec3f(-0.8745534f, -0.00456138f, -0.4849085f));

            mi = new MobBed("BEDHIGH_PSI.ASC", "", null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-5046.29f, -186.9131f, -10619.16f), new Vec3f(-0.9659259f, 0f, 0.258819f));

            mi = new MobSwitch("LEVER_1_OC.MDS", "MOBNAME_SECRETSWITCH", null, "EVT_CITY_REICH03_01", false, false);
            mi.Spawn(mapName, new Vec3f(12928.15f, 1437.156f, -4194.157f), new Vec3f(0.08715579f, 0f, 0.9961964f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_SECRETSWITCH", null, "EVT_CITY_SALANDRIL_01", false, false);
            mi.Spawn(mapName, new Vec3f(12521.84f, 853.9437f, 3077.144f), new Vec3f(-0.9455196f, 0f, 0.3255686f));

            mi = new MobSwitch("LEVER_1_OC.MDS", "", null, "EVT_CITY_PRISON_03", false, false);
            mi.Spawn(mapName, new Vec3f(3750.92f, 870.762f, 5221.294f), new Vec3f(0.4539902f, 0f, -0.8910067f));

            mi = new MobSwitch("LEVER_1_OC.MDS", "", null, "EVT_CITY_PRISON_02", false, false);
            mi.Spawn(mapName, new Vec3f(4083.061f, 870.762f, 5406.928f), new Vec3f(0.4539908f, 0f, -0.8910076f));

            mi = new MobSwitch("LEVER_1_OC.MDS", "", null, "EVT_CITY_PRISON_01", false, false);
            mi.Spawn(mapName, new Vec3f(4214.702f, 873.9993f, 5922.649f), new Vec3f(0.9396938f, 0f, 0.3420207f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "", null, "EVT_CITY_RICH_02", false, false);
            mi.Spawn(mapName, new Vec3f(13388.44f, 1304.109f, -4120.9f), new Vec3f(-0.3746096f, 0f, -0.9271908f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_SECRETSWITCH", null, "EVT_CITY_RICHTER_01", false, true);
            mi.Spawn(mapName, new Vec3f(16457.89f, 1292.559f, -2737.87f), new Vec3f(-0.9702972f, 0f, 0.2419223f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_FARM_01", false, false);
            mi.Spawn(mapName, new Vec3f(16302.24f, 2642.944f, -8560.794f), new Vec3f(-0.9993907f, 0f, -0.03489948f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_FARM_01", false, false);
            mi.Spawn(mapName, new Vec3f(17189.39f, 2566.444f, -9247.174f), new Vec3f(0.05233596f, 0f, -0.9986295f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_FARM_01", false, false);
            mi.Spawn(mapName, new Vec3f(16472.76f, 2541.315f, -9853.419f), new Vec3f(0.743145f, 0f, 0.6691308f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_FOREST_01", false, false);
            mi.Spawn(mapName, new Vec3f(29562.94f, 4389.255f, 25980.25f), new Vec3f(-0.4226177f, 0f, -0.9063069f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_FOREST_01", false, false);
            mi.Spawn(mapName, new Vec3f(28844.43f, 4389.36f, 25458.08f), new Vec3f(0.8910062f, 0f, 0.4539906f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_FOREST_01", false, false);
            mi.Spawn(mapName, new Vec3f(29957.41f, 4330.895f, 24914.91f), new Vec3f(0.4694713f, 1.02444E-09f, 0.8829476f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_SWITCH", null, "EVT_CASTLEMINE_CRAWLERSPERRE", false, false);
            mi.Spawn(mapName, new Vec3f(62544.04f, 3953.164f, -25044.59f), new Vec3f(0.7880105f, 0f, -0.615662f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_SWITCH", null, "EVT_CASTLEMINE_CRAWLERSPERRE", false, false);
            mi.Spawn(mapName, new Vec3f(62410.63f, 3976.948f, -24482.69f), new Vec3f(-0.9563055f, 0f, 0.2923718f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_BIGFARM_01", false, false);
            mi.Spawn(mapName, new Vec3f(58979.88f, 2081.988f, -7514.291f), new Vec3f(0.8829468f, 0f, 0.4694712f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_BIGFARM_01", false, false);
            mi.Spawn(mapName, new Vec3f(60433.68f, 2094.416f, -7450.486f), new Vec3f(-0.9816266f, 0f, -0.190809f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "MOBNAME_ADDON_ORNAMENTSWITCH", null, "EVT_ORNAMENT_CODEMASTER_BIGFARM_01", false, false);
            mi.Spawn(mapName, new Vec3f(59634.19f, 2073.416f, -6560.407f), new Vec3f(-0.03489929f, 0f, -0.9993912f));

            mi = new MobSwitch("LEVER_1_OC.MDS", "MOBNAME_LEVER", null, "EVT_DOOR_PALSECRETCHAMBERTRIGGER", true, false);
            mi.Spawn(mapName, new Vec3f(48112.8f, 3609.399f, 16965.62f), new Vec3f(0.8660253f, 0f, 0.5000004f));

            mi = new MobSwitch("LEVER_1_OC.MDS", "MOBNAME_LIBRARYLEVER", null, "EVT_SECRETLIBRARY_BOOKSHELF", false, false);
            mi.Spawn(mapName, new Vec3f(50172.97f, 4469.027f, 22008.59f), new Vec3f(0.8571678f, 0f, 0.5150383f));

            mi = new MobSwitch("TOUCHPLATE_STONE.MDS", "", null, "EVT_XARDAS_SECRET_01", false, false);
            mi.Spawn(mapName, new Vec3f(30257.15f, 5154.084f, -14557.68f), new Vec3f(0.3255683f, 0f, 0.9455191f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(14429.88f, 1316.39f, -3771.224f), new Vec3f(-0.9111881f, 0f, 0.4119914f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(15000.22f, 1324.155f, -4018.451f), new Vec3f(0.5049797f, 0f, 0.8631313f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(14489.48f, 1328.953f, -4459.431f), new Vec3f(0.5150383f, 0f, 0.8571675f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(14891.67f, 1320.953f, -4377.331f), new Vec3f(-0.8908501f, 0f, 0.4543029f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(14285.1f, 1331.776f, -4087.684f), new Vec3f(-0.9203699f, 0f, 0.3910534f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(15114.18f, 1336.752f, -3796.235f), new Vec3f(0.4591156f, 0f, 0.8883778f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(12255.43f, 901.8385f, 3774.455f), new Vec3f(-0.3053625f, 0f, -0.9522372f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_THIEFGUILDKEY_HOTEL_MIS"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7856.65f, 271f, 2944.945f), new Vec3f(-0.9027369f, 0f, -0.4301962f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(1727.963f, -190.0467f, 616.6371f), new Vec3f(0.01745239f, 0f, 0.9998478f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(524.097f, -190.0779f, -6041.944f), new Vec3f(-0.7771468f, 0f, -0.6293212f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3264.222f, -188.3195f, 2254.931f), new Vec3f(-0.2923718f, 0f, 0.9563049f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3481.048f, -122.4646f, -2782.998f), new Vec3f(1f, 0f, 5.7742E-07f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3004.526f, -112.1509f, -2880.741f), new Vec3f(-4.209578E-07f, 0f, -1.000002f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7363.361f, 572.5247f, 3045.776f), new Vec3f(-0.9046795f, 0f, -0.426096f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8419.35f, 580.1456f, 3068.085f), new Vec3f(0.9189975f, 0f, 0.3942631f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8138.208f, 566.7748f, 3687.667f), new Vec3f(0.9189975f, 0f, 0.3942631f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8461.652f, 571.7748f, 3794.647f), new Vec3f(0.9046783f, 0f, 0.4260957f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7393.769f, 569.997f, 3712.455f), new Vec3f(0.9189977f, 0f, 0.394263f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7670.883f, 568.997f, 3839.827f), new Vec3f(0.9189975f, 0f, 0.3942631f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8380.004f, 265.2694f, -2988.415f), new Vec3f(-0.8396204f, 0f, -0.5431748f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(13984.33f, 1328.473f, -3811.114f), new Vec3f(0.4694721f, 0f, 0.8829482f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(6004.312f, -639.0705f, 4915.921f), new Vec3f(0.001047894f, 0f, -0.9999996f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_CITY_TOWER_04"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(10678.06f, 268.8279f, 5057.788f), new Vec3f(-0.829038f, 0f, -0.559193f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_CITY_TOWER_03"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(9860.903f, 269f, 6316.498f), new Vec3f(-0.8384824f, 0f, -0.5449335f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(17054.84f, 1299.588f, -3059.763f), new Vec3f(0.9945225f, 0f, -0.1045286f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(15514.37f, 1540.773f, 369.8245f), new Vec3f(-0.6946583f, 0f, 0.7193399f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(15258.9f, 1536.104f, 667.0801f), new Vec3f(-0.669131f, 0f, 0.7431453f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3451.916f, -187.7467f, 1754.569f), new Vec3f(-0.2923717f, 0f, 0.956305f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3971.845f, -187.3723f, 877.9774f), new Vec3f(-0.08715561f, 0f, -0.9961947f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4000.896f, -117.049f, -3570.586f), new Vec3f(-0.9659259f, 0f, 0.2588191f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4793.801f, -117.636f, -3558.963f), new Vec3f(0.4225563f, -0.01744975f, 0.9061692f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(990.4994f, -187.6794f, -5718.75f), new Vec3f(-0.7771458f, 0f, -0.6293203f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(850.6344f, -186.983f, -5155.641f), new Vec3f(0.7660444f, 0f, -0.6427876f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(1295.852f, -186.4995f, -5516.934f), new Vec3f(-0.7547101f, 0f, 0.6560595f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3583.696f, -117.5346f, -5475.215f), new Vec3f(0.5735771f, 0f, 0.8191525f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(2406.543f, -186.6691f, -4860.956f), new Vec3f(0.4694718f, 0f, 0.882948f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4032.85f, -117.4567f, -5347.055f), new Vec3f(0.913546f, 0f, -0.4067369f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3419.135f, -117.1861f, -3093.337f), new Vec3f(0.2079115f, 0f, 0.9781477f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(1869.684f, -186.7481f, -5255.608f), new Vec3f(-0.4694714f, 0f, -0.8829477f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4422.283f, -117.896f, -3399.545f), new Vec3f(-0.4067366f, 0f, -0.9135462f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4104.695f, -116.3862f, -4849.672f), new Vec3f(0.4383714f, 0f, 0.8987948f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3951.252f, -194.1021f, 286.0517f), new Vec3f(0.08715577f, 0f, 0.996195f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3619.194f, -186.4887f, 1425.161f), new Vec3f(-0.9925466f, 0f, -0.1218692f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3200.289f, -184.615f, -122.2002f), new Vec3f(-0.03489953f, 0f, -0.9993914f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7432.884f, 269.1692f, -2787.005f), new Vec3f(-0.4848104f, 0f, 0.87462f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(6865.07f, 122.8833f, 49.05592f), new Vec3f(-0.8290382f, 0f, -0.5591935f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5970f, 267f, -5372f), new Vec3f(-0.1161509f, 0f, 0.9932328f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(6763f, 268f, -4995f), new Vec3f(0.987745f, 0f, 0.15609f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8111.936f, 269f, -2714.586f), new Vec3f(-0.5284389f, 0f, 0.8489719f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(5955.929f, 268.2761f, 24.80677f), new Vec3f(-0.8480485f, 0f, -0.5299201f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4737.49f, 166.928f, -1209.849f), new Vec3f(0.8480489f, 0f, 0.5299199f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(2686.089f, -184.836f, -453.7289f), new Vec3f(-0.9993911f, 0f, 0.03489953f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3126.629f, -186.9781f, 1428.349f), new Vec3f(0.03489955f, 0f, -0.9993913f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5703.592f, 747.9456f, 8921.647f), new Vec3f(0.8829475f, 0f, 0.4694719f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5357.321f, 747.7728f, 9555.05f), new Vec3f(-0.8829477f, 0f, -0.4694719f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5404.458f, 747.7313f, 8770.82f), new Vec3f(0.8910067f, 0f, 0.4539905f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5068.679f, 747.9446f, 9378.532f), new Vec3f(-0.8746195f, 0f, -0.4848099f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4535.999f, 746.1268f, 9081.469f), new Vec3f(-0.8660254f, 0f, -0.5f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4176.95f, 748.7969f, 8855.206f), new Vec3f(-0.9063078f, 0f, -0.4226186f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3906.915f, 746.513f, 8705.955f), new Vec3f(-0.8746198f, 0f, -0.4848099f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(2158.558f, -190.0427f, 613.6368f), new Vec3f(0.03489948f, 0f, 0.9993911f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3053.671f, -117.6591f, -5779.844f), new Vec3f(-0.8480483f, 0f, 0.5299194f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(10563.96f, 1299.257f, 958.0833f), new Vec3f(0.9563048f, 0f, 0.2923715f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(12339.61f, 1298.736f, -4337.99f), new Vec3f(-0.9986297f, 0f, -0.05233604f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(3665.007f, -123.9364f, -7193.73f), new Vec3f(0.743145f, 0f, 0.6691315f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7129.165f, 264.7772f, -2520.655f), new Vec3f(0.8660251f, 0f, 0.5000005f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8357.689f, 270.7241f, 3493.219f), new Vec3f(-0.4260962f, 0f, 0.9046795f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_CITY_TOWER_01"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8827f, 269f, -6082f), new Vec3f(0.1391733f, 0f, 0.9902686f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_CITY_TOWER_02"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7319f, 270f, -5884f), new Vec3f(-0.136235f, 0f, -0.9906811f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_CITY_TOWER_06"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5245.266f, 190.9999f, -2877.972f), new Vec3f(0.4817551f, 0f, -0.8763093f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_CITY_TOWER_05"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7806.896f, 268f, -1181.836f), new Vec3f(0.193721f, 0f, 0.9810584f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8203f, -644f, 4996f), new Vec3f(0.004013953f, 0f, 1.000002f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7820f, -638f, 4919f), new Vec3f(1.000004f, 0f, 0.0001748827f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(7295.864f, -639f, 4592.959f), new Vec3f(-0.01850003f, 0f, 0.999831f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(10281.19f, 1302.316f, -2558.006f), new Vec3f(-0.6560596f, 0f, -0.7547104f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(12623.05f, 1298.365f, -4839.863f), new Vec3f(0.06975636f, 0f, -0.997564f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(15077.78f, 897.1324f, -4314.593f), new Vec3f(0.4848102f, 0f, 0.8746209f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(11432.36f, 897.942f, 3706.599f), new Vec3f(-0.8480484f, 0f, -0.5299191f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8948.871f, 569.8574f, 3238.153f), new Vec3f(0.897104f, 0f, 0.4418194f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8692.429f, 571.1705f, 3118.869f), new Vec3f(0.9189975f, 0f, 0.394263f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2318.377f, 97.50841f, -3516.771f), new Vec3f(6.023339E-09f, 0f, 1f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2140.787f, 98.014f, -2717.231f), new Vec3f(8.003043E-09f, 0f, 1.000001f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2233.192f, 97.17695f, -2555.94f), new Vec3f(0f, 0f, 1f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(2282.667f, -185.1014f, -3268.993f), new Vec3f(0f, 0f, 0.9999999f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "", true, ItemInstance.getItemInstance("ITKE_THIEFGUILDKEY_MIS"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-542f, -649f, 5259f), new Vec3f(-0.9999998f, 0f, -3.799864E-08f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(9130.047f, 275.518f, 3359.683f), new Vec3f(0.889257f, 0f, 0.4574088f));

            mi = new MobDoor("BEDHIGH_NW_EDEL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(8249.955f, 269.7159f, 3804.263f), new Vec3f(-0.4260955f, 0f, 0.9046782f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(12356.86f, 1293.201f, -4759.283f), new Vec3f(0.9998503f, 0f, -0.01745191f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, false, false);
            mi.Spawn(mapName, new Vec3f(12512.87f, 1291.685f, -4198.818f), new Vec3f(-0.9998483f, 0f, -0.01745296f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, false, false);
            mi.Spawn(mapName, new Vec3f(16574.81f, 1297.533f, -2955.097f), new Vec3f(0.9961951f, -3.72529E-09f, -0.0871563f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(10067.06f, 1298.56f, -2791.089f), new Vec3f(0.7771469f, 0f, -0.6293211f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(10363.8f, 1292.694f, 1593.398f), new Vec3f(0.9743726f, 0f, 0.2249513f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(11673.03f, 895.7369f, 3325.875f), new Vec3f(0.8480483f, 0f, 0.5299184f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5124.367f, 743.7598f, 8638.06f), new Vec3f(0.8910067f, 0f, 0.4539902f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4827.231f, 740.7341f, 9224.708f), new Vec3f(0.8910067f, 0f, 0.4539902f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "", true, ItemInstance.getItemInstance("ITKE_FINGERS"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(5372.243f, -639.2376f, 4364.467f), new Vec3f(0.9998472f, -1.862645E-09f, 0.01745251f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(14849.2f, 1326.125f, -3516.774f), new Vec3f(-0.474549f, 0f, -0.8802294f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(4937.793f, 163.1324f, -589.9237f), new Vec3f(-0.5544102f, 0f, 0.8322452f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_BROMOR"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(1563.198f, -183.9112f, -3353.519f), new Vec3f(0.008377133f, 0f, -0.9999663f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(15118f, 1792f, -16699f), new Vec3f(-0.5070893f, 0f, 0.8618981f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14663f, 1792f, -16878f), new Vec3f(0.8615482f, 0f, 0.5076935f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(14466f, 1789f, -16537f), new Vec3f(-0.8588759f, 0f, -0.5121959f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(14236f, 1589f, -13917f), new Vec3f(0.0960201f, 3.924653E-09f, 0.9953839f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(13864f, 1589f, -13811f), new Vec3f(-0.9952789f, 0f, 0.09706175f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(32222.34f, -1925.866f, 1952.245f), new Vec3f(-0.9335815f, 0f, -0.3583684f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-1487.066f, 2407.303f, 16756.94f), new Vec3f(0.8386701f, 0f, 0.5446396f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(31382.46f, 3444.988f, 8465.648f), new Vec3f(0.1391732f, -5.184603E-10f, 0.9902678f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(31102.87f, 3444.988f, 8691.413f), new Vec3f(-0.9961944f, 0f, 0.0871565f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "", true, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(36975.77f, 3776.136f, -2855.862f), new Vec3f(-0.08715575f, 0f, 0.9961948f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(36565.75f, 4105.281f, -1931.66f), new Vec3f(-0.01745241f, 0f, 0.9998477f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(36495.51f, 4112.405f, -2504.933f), new Vec3f(0.9999995f, 0f, -3.027179E-08f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "", true, ItemInstance.getItemInstance("ITKE_ORLAN_HOTELZIMMER"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(37202.48f, 4075.722f, -2159.848f), new Vec3f(-0.9986295f, 0f, -0.05233638f));

            mi = new MobDoor("DOOR_NW_NORMAL_01.MDS", "", true, ItemInstance.getItemInstance("ITKE_ORLAN_HOTELZIMMER"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(37234.85f, 4074.722f, -2626.023f), new Vec3f(-0.9986295f, 0f, -0.05233646f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_ORLAN_TELEPORTSTATION"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(35145.89f, 4098.57f, -11038.57f), new Vec3f(-0.8290384f, 0f, 0.5591932f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(65995.89f, 3840f, -25237.79f), new Vec3f(-0.8971987f, 0f, 0.4416363f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50475.08f, 2961.946f, -18657.95f), new Vec3f(0.1320859f, 0f, 0.9912407f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50486f, 2963f, -18570f), new Vec3f(0.1125115f, 0f, 0.9936504f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(72542.44f, 3157.786f, -13101.23f), new Vec3f(0.9545633f, 0f, -0.2980078f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(72455.43f, 3155.786f, -13430.75f), new Vec3f(-0.9545633f, 0f, 0.2980077f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(75673.01f, 3832.218f, -13202.45f), new Vec3f(-0.08127633f, 0f, 0.9966917f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(76107.78f, 3833.218f, -13188.69f), new Vec3f(-0.06386926f, 0f, 0.9979584f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(72875.58f, 3156.415f, -13883.05f), new Vec3f(-0.3852953f, 0f, 0.9227933f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(73279.73f, 3154.415f, -13117.25f), new Vec3f(-0.9545636f, 0f, 0.2980067f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(72489.98f, 3154.415f, -13786.63f), new Vec3f(0.2980067f, 0f, 0.9545636f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(73119.55f, 3158.415f, -13507.45f), new Vec3f(-0.9545636f, 0f, 0.2980067f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(75143.34f, 3512.217f, -12342.1f), new Vec3f(-0.08128124f, 0f, 0.9966912f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(75159.39f, 3511.52f, -13205.65f), new Vec3f(-0.1850187f, 0f, 0.9827356f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(75193.35f, 3512.537f, -13506.58f), new Vec3f(-0.08128124f, 0f, 0.9966912f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(74719.16f, 3510.367f, -12429.23f), new Vec3f(0.07563868f, 0f, 0.9971353f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(74716.45f, 3511.783f, -13519.59f), new Vec3f(-0.08127888f, 0f, 0.9966915f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(74682.43f, 3512.786f, -12954.71f), new Vec3f(0.02334902f, 0f, 0.999727f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(74689.14f, 3512.985f, -13228.86f), new Vec3f(-0.08127887f, 0f, 0.9966916f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(74620.11f, 3831.978f, -12596.87f), new Vec3f(-0.04644428f, 0f, 0.998921f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(74343.88f, 3829.605f, -9604.017f), new Vec3f(0.06975641f, 0f, -0.9975641f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(74396.81f, 3829.433f, -10193.06f), new Vec3f(0.08193913f, 0.0002127f, -0.9966373f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(56901f, 1902f, -1674f), new Vec3f(0.2596634f, 0f, 0.9657055f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(56895f, 1903f, -2073f), new Vec3f(-0.9715508f, 0f, 0.2368392f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(64551f, 3985f, -22136f), new Vec3f(-0.02324547f, 0f, 0.9997312f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(67362.13f, 3968.876f, -21902.86f), new Vec3f(-0.9805062f, 0f, -0.1964905f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(52992f, 1641f, -11006f), new Vec3f(-0.913972f, 0f, 0.4057805f));

            mi = new MobDoor("BEDHIGH_PSI.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(70342.36f, 1679.538f, -24022.07f), new Vec3f(-0.2306931f, 0f, -0.973026f));

            mi = new MobDoor("BEDHIGH_PSI.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(70538.33f, 1675.293f, -24319.64f), new Vec3f(0.9918113f, 0f, -0.1277231f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(59974f, 4146.328f, -33015.1f), new Vec3f(0.9510576f, 0f, 0.3090174f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(61573.11f, 4012.496f, -32671.91f), new Vec3f(0.9702961f, 0f, -0.241922f));

            mi = new MobDoor("BEDHIGH_PSI.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(57625.45f, 1913.875f, -28367.31f), new Vec3f(0.04644403f, 0f, -0.9989208f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(49369.1f, 4998.205f, 21430.5f), new Vec3f(0.5150372f, 0f, -0.857168f));

            mi = new MobDoor("DOOR_NW_RICH_01.MDS", "", true, ItemInstance.getItemInstance("ITKE_KDFPLAYER"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50380.18f, 4978.516f, 18681.87f), new Vec3f(-0.5299211f, 0f, 0.8480482f));

            mi = new MobDoor("DOOR_NW_RICH_01.MDS", "MOBNAME_VORRATSKAMMER", true, ItemInstance.getItemInstance("ITKE_KLOSTERSTORE"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50316.17f, 4235.498f, 19384.27f), new Vec3f(0.8480474f, 0f, 0.5299203f));

            mi = new MobDoor("DOOR_NW_DRAGONISLE_02.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_MONASTARYSECRETLIBRARY_MIS"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(47889.15f, 3456.856f, 19338.17f), new Vec3f(0.4848112f, -1.299319E-10f, -0.8746201f));

            mi = new MobDoor("DOOR_NW_RICH_01.MDS", "MOBNAME_Door", true, ItemInstance.getItemInstance("ITKE_INNOS_MIS"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(47149.16f, 4882.624f, 18703.85f), new Vec3f(0.8480474f, 0f, 0.5299203f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(47988f, 4991f, 22128f), new Vec3f(-0.8433917f, 0f, -0.5372988f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50942.3f, 4991.607f, 18201.62f), new Vec3f(0.9135459f, 0f, 0.4067368f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50402.91f, 4991.679f, 17774.62f), new Vec3f(0.857168f, 0f, 0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(49955.79f, 4991.547f, 17503.43f), new Vec3f(0.8571671f, 0f, 0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50096.28f, 4991.191f, 18290.57f), new Vec3f(-0.8571667f, 0f, -0.5150384f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(49651.11f, 4992.204f, 18014.21f), new Vec3f(-0.8571669f, 0f, -0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(49208.91f, 4991.191f, 17050.16f), new Vec3f(0.8571669f, 0f, 0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(48744.96f, 4991.963f, 16796.61f), new Vec3f(0.8571669f, 0f, 0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(48442.61f, 4992.097f, 17297.15f), new Vec3f(-0.8571671f, 0f, -0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(48895.87f, 4991.238f, 17563.13f), new Vec3f(-0.8571671f, 0f, -0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(46197.53f, 4991.637f, 21032.54f), new Vec3f(-0.8571671f, 0f, -0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(46639.88f, 4992.601f, 21306.74f), new Vec3f(-0.8571671f, 0f, -0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(46497.7f, 4992.371f, 20527.55f), new Vec3f(0.8571669f, 0f, 0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(46940.11f, 4992.211f, 20800.46f), new Vec3f(0.8571669f, 0f, 0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(47392f, 4992f, 21755f), new Vec3f(-0.8616288f, 0f, -0.5075387f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(47842f, 4991.972f, 22041.68f), new Vec3f(-0.8571671f, 0f, -0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(48147.78f, 4992.557f, 21516.14f), new Vec3f(0.8571672f, 0f, 0.5150385f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(47699f, 4991.257f, 21245.24f), new Vec3f(0.8480477f, 0f, 0.5299197f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(50114.64f, 4996.316f, 21833.57f), new Vec3f(-0.5446379f, 0f, 0.8386712f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(49485.59f, 4996.365f, 22865.9f), new Vec3f(-0.5299191f, 0f, 0.8480495f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(51276.32f, 4990.464f, 19865.04f), new Vec3f(-0.4539895f, 0f, 0.8910069f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(51890.27f, 4989.464f, 18828.84f), new Vec3f(-0.544638f, 0f, 0.838671f));

            mi = new MobDoor("DOOR_NW_RICH_01.MDS", "MOBNAME_BIBLIOTHEK", true, ItemInstance.getItemInstance("ITKE_KLOSTERBIBLIOTHEK"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(46604.68f, 4988.83f, 20189.71f), new Vec3f(0.8290369f, 0f, 0.5591939f));

            mi = new MobDoor("DOOR_NW_RICH_01.MDS", "MOBNAME_SCHATZKAMMER", true, ItemInstance.getItemInstance("ITKE_KLOSTERSCHATZ"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(49838.6f, 4238.091f, 18897.59f), new Vec3f(-0.54464f, 0f, 0.8386697f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", true, ItemInstance.getItemInstance("ITKE_PASS_MIS"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(36771.46f, 3174.908f, -26467.8f), new Vec3f(0.615662f, 0f, 0.7880117f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-9650.195f, 184.8078f, -18514.31f), new Vec3f(0.9961953f, 0f, -0.08715668f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-9706.358f, 190.3399f, -18990.46f), new Vec3f(0.9945226f, 0f, -0.1045294f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10335.35f, 191.1898f, -18484.51f), new Vec3f(0.9993908f, 0f, -0.0349004f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10663.39f, 191.956f, -18474.83f), new Vec3f(0.9993908f, 0f, -0.0349004f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10600.48f, 187.8936f, -18956.58f), new Vec3f(0.9993908f, 0f, -0.0349004f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10327.51f, 186.657f, -18967.78f), new Vec3f(0.9993907f, 0f, -0.03489953f));

            mi = new MobDoor("DOOR_NW_RICH_01.MDS", "", true, ItemInstance.getItemInstance("ITKE_SHIP_LEVELCHANGE_MIS"), null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(-9841.165f, 567.0841f, -18812.7f), new Vec3f(0f, 0f, 1f));

            mi = new MobDoor("BEDHIGH_NW_MASTER_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(-10326.91f, 723.3566f, -19592.96f), new Vec3f(0.05756399f, 0f, -0.9983402f));

            mi = new MobDoor("BEDHIGH_PC.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(80532.7f, 5004.334f, 26205.66f), new Vec3f(0.9335808f, 0f, 0.3583681f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(59107.32f, 6939.804f, 24047.58f), new Vec3f(0.9505152f, 0f, -0.3106806f));

            mi = new MobDoor("DOOR_NW_POOR_01.MDS", "MOBNAME_DOOR", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(57441f, 6946f, 25594f), new Vec3f(0.9905056f, -0.001199421f, 0.137439f));

            mi = new MobDoor("BEDHIGH_PSI.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(83234.1f, 3493.196f, 28919.49f), new Vec3f(0f, 0f, 1f));

            mi = new MobDoor("BEDHIGH_PSI.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(83071.12f, 3493.196f, 28649.4f), new Vec3f(-0.9999998f, 0f, -3.799864E-08f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, false);
            mi.Spawn(mapName, new Vec3f(17414.35f, 3166.885f, -20818.1f), new Vec3f(-0.8160374f, 0.08715574f, 0.5713957f));

            mi = new MobDoor("BEDHIGH_NW_NORMAL_01.ASC", "MOBNAME_BED", false, null, null, null, null, true, true);
            mi.Spawn(mapName, new Vec3f(30703.7f, 5149.067f, -15089.55f), new Vec3f(-0.6293211f, 0f, 0.7771466f));



		}
    }
}
