using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network.Savings;
using Gothic.zTypes;
using Gothic.zClasses;
using WinApi;
using Network.Types;
using GMP.Helper;
using Network;
using RakNet;
using GMP.Modules;

namespace GMP.Network.Messages
{
    public class LevelDataMessage : Message
    {
        public class LDM_CONTAINER
        {
            public class item{ public String code; public int amount;}
            public String objectname;
            public float[] pos;
            public List<item> itemList = new List<item>();

            public void addItem(String code, int amount)
            {
                itemList.Add(new item() { code = code, amount = amount });
            }
        }

        public class LDM_MobInter
        {
            public string name;
            public float[] pos;
            public int type;
            public bool triggered;
        }
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            Process Process = Process.ThisProcess();
            int itemCount = 0;
            int containerCount = 0;
            int mobInterCount = 0;

            stream.Read(out itemCount);
            stream.Read(out containerCount);
            stream.Read(out mobInterCount);

            zERROR.GetZErr(Process).Report(2, 'G', "Level-Data-Message: Item-Count: "+itemCount + " | Container-Count: "+containerCount, 0, "LevelDataMessage.cs", 0);

            World worlds = new World();
            
            //Items...
            for (int i = 0; i < itemCount; i++)
            {
                String code = "";
                int amount = 0;
                float[] pos = new float[3];

                for (int p = 0; p < 3; p++)
                    stream.Read(out pos[p]);
                stream.Read(out code);
                stream.Read(out amount);

                //TODO: Item der Welt hinzufügen
                oCItem itm = oCObjectFactory.GetFactory(Process).CreateItem(code);
                itm.Amount = amount;

                zVec3 vecPos = zVec3.Create(Process);
                vecPos.X = pos[0]; 
                vecPos.Y = pos[1];
                vecPos.Z = pos[2];

                itm.SetPositionWorld(vecPos);
                vecPos.Dispose();

     
                oCGame.Game(Process).World.AddVob(itm);
                itm.BeginMovement();
            }
            #region othercode


            //Container:

            List<LDM_CONTAINER> containerList = new List<LDM_CONTAINER>();
            for (int i = 0; i < containerCount; i++)
            {
                String objectName = "";
                float[] pos = new float[3];
                int contentCount = 0;

                stream.Read(out objectName);
                for (int p = 0; p < 3; p++)
                    stream.Read(out pos[p]);
                stream.Read(out contentCount);


                LDM_CONTAINER conObj = new LDM_CONTAINER();
                conObj.objectname = objectName;
                conObj.pos = pos;

                for (int cC = 0; cC < contentCount; cC++)
                {
                    String code = "";
                    int amount = 0;

                    stream.Read(out code);
                    stream.Read(out amount);

                    conObj.addItem(code, amount);
                }
                containerList.Add(conObj);
            }

            zCTree<zCVob> vobtree = oCGame.Game(Process).World.GlobalVobTree.FirstChild;
            do
            {

                zCVob vob = vobtree.Data;
                if (vob.VobType != zCVob.VobTypes.MobContainer)
                    continue;

                String vobName = vob.ObjectName.Value;
                float[] vobPos = new float[] { vob.TrafoObjToWorld.get(3), vob.TrafoObjToWorld.get(7), vob.TrafoObjToWorld.get(11) };


                foreach (LDM_CONTAINER container in containerList)
                {
                    if (new Vec3f(vobPos).getDistance((Vec3f)container.pos) < World.ContainerMinDistance
                        && vobName.Trim().ToLower() == container.objectname.Trim().ToLower())
                    {
                        oCMobContainer mc = new oCMobContainer(Process, vob.Address);

                        foreach (LDM_CONTAINER.item item in container.itemList)
                        {
                            oCItem itm = oCObjectFactory.GetFactory(Process).CreateItem(item.code);
                            itm.Amount = item.amount;
                            mc.Insert(itm);
                        }

                        containerList.Remove(container);
                        break;
                    }
                }
            } while ((vobtree = vobtree.Next).Address != 0);







            //List<LDM_MobInter> mobInterList = new List<LDM_MobInter>();
            //for (int i = 0; i < mobInterCount; i++)
            //{
            //    int vobType = 0;
            //    String mobName = "";
            //    float[] pos = new float[3];
            //    bool triggered = false;

            //    stream.Read(out mobName);
            //    stream.Read(out vobType);

            //    for (int p = 0; p < 3; p++)
            //        stream.Read(out pos[p]);
            //    stream.Read(out triggered);

            //    LDM_MobInter mobInterObj = new LDM_MobInter();
            //    mobInterObj.name = mobName;
            //    mobInterObj.pos = pos;
            //    mobInterObj.triggered = triggered;
            //    mobInterObj.type = vobType;

            //    mobInterList.Add(mobInterObj);
            //}
            #endregion


            //Container:
            //for (int i = 0; i < containerCount; i++)
            //{
            //    String objectName = "";
            //    float[] pos = new float[3];
            //    int contentCount = 0;

            //    stream.Read(out objectName);
            //    for (int p = 0; p < 3; p++)
            //        stream.Read(out pos[p]);
            //    stream.Read(out contentCount);


            //    //Container finden:
            //    zCVob vob = ObjectHelper.findVob(objectName, pos, zCVob.VobTypes.MobContainer, World.ContainerMinDistance);

            //    if (vob == null)
            //    {
            //        zERROR.GetZErr(Process).Report(2, 'G', "Level-Data-Message Container nicht gefunden! " + objectName, 0, "LevelDataMessage.cs", 0);

            //        for (int cC = 0; cC < contentCount; cC++)
            //        {
            //            String code = "";
            //            int amount = 0;

            //            stream.Read(out code);
            //            stream.Read(out amount);
            //        }

            //        continue;
            //    }

            //    for (int cC = 0; cC < contentCount; cC++)
            //    {
            //        String code = "";
            //        int amount = 0;

            //        stream.Read(out code);
            //        stream.Read(out amount);

            //        //TODO: Items dem Container hinzufügen!
            //        oCMobContainer mc = new oCMobContainer(Process, vob.Address);

            //        oCItem itm = oCObjectFactory.GetFactory(Process).CreateItem(code);
            //        itm.Amount = amount;
            //        mc.Insert(itm);
            //    }
            //}

            for (int i = 0; i < mobInterCount; i++)
            {
                int vobType = 0;
                String mobName = "";
                float[] pos = new float[3];
                bool triggered = false;

                stream.Read(out mobName);
                stream.Read(out vobType);

                for (int p = 0; p < 3; p++)
                    stream.Read(out pos[p]);
                stream.Read(out triggered);
                zCVob vob = ObjectHelper.findVob(mobName, pos, (Gothic.zClasses.zCVob.VobTypes)vobType, World.MobInterMinDistance);

                if (vob == null)
                {
                    zERROR.GetZErr(Process).Report(2, 'G', "Level-Data-Message MobInter nicht gefunden! " + mobName, 0, "LevelDataMessage.cs", 0);
                    continue;
                }
                if (triggered)
                    new oCMobInter(Process, vob.Address).OnTrigger(new zCVob(Process, 0), new zCVob(Process, 0));
                else
                    new oCMobInter(Process, vob.Address).OnUnTrigger(new zCVob(Process, 0), new zCVob(Process, 0));
            }


            zERROR.GetZErr(Process).Report(2, 'G', "Level-Data-Message empfangen", 0, "LevelDataMessage.cs", 0);
        }

        public override void Write(RakNet.BitStream stream, Client client)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.LevelDataMessage);
            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Level-Data-Message beantragt!", 0, "LevelDataMessage.cs", 0);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.LevelDataMessage))
                StaticVars.sStats[(int)NetWorkIDS.LevelDataMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.LevelDataMessage] += 1;
        }
    }
}
