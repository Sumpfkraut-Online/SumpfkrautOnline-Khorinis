using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;
using System.Globalization;
using System.Threading.Tasks;

using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.Types;
using GUC.Server.Scripts.AI.NPC_Def.Monster;
using GUC.Server.Scripts.AI.NPC_Def.Orc;
using GUC.Server.Scripts.AI;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Server.Scripts;


namespace GUC.Server.Scripts.Sumpfkraut
{
    /**
     * A class for periodical respawning of plants and more (in the future).
     * TODO: Kalkor wants to distribute monsters sorted by XP
     * TODO: Byte padding with the final version of Zenobject
     */
    public class CRespawn
    {
        //Stopwatch sw;

        const string MAPNAME = @"NEWWORLD\NEWWORLD.ZEN";
        
        System.Timers.Timer RespawnTimer;
        ArrayList ObjectsToRespawn;
        String[] PlantList;
        String[] MonsterList;
        Random Rng = new Random();
        
        public CRespawn()
        {
            //Stopwatch sw = new Stopwatch();

            PlantList = Enum.GetNames(typeof(BalancingConstants.PlantList));
            MonsterList = Enum.GetNames(typeof(BalancingConstants.MonsterList));

            //normally: ArrayList with the ZenObjects e.g.
            //ObjectsToRespawn = WorldParser::GetObjects(ZENTYPE.PlantContainer);
            ObjectsToRespawn = new ArrayList();
            this.FillArrayListWithJunk(ref this.ObjectsToRespawn);

            this.RespawnTimer = new System.Timers.Timer();
            this.RespawnTimer.Elapsed += new ElapsedEventHandler(RespawnObjects);
            this.RespawnTimer.Interval = BalancingConstants.RESPAWNINTERVALLMILLISECONDS;
            this.RespawnTimer.Start();
        }
        public void StopRespawn()
        {
            this.RespawnTimer.Stop();
        }

        private void FillArrayListWithJunk(ref ArrayList _ObjectsToRespawn)
        {
            //todo schleife mit random positionen

            //Das X steht für West/Ost Richtung, z.B. "5000" und der Spieler bewegt sich um 5000 Schritte nach Osten, "-5000" und der Spieler bewegt um 5000 Schritte nach Westen.
            //Das Y steht für die Höhe, "1000" bedeutet, dass der Spieler 1000 Schritte über dem Boden erscheint.
            //Das Z steht für Norden/Süden. z.B. "5000" und der Spieler bewegt sich um 5000 Schritte nach Norden, "-5000" und der Spieler bewegt sich 5000 Schritte nach Süden. Der Nullpunkt (goto position 0) ist dabei der Hafen von Khorinis.

            double[] Direction;
            double[] Position;
            ZenObject Zobj;

            for (int i = 0; i <= 20 ; ++i)
            {
                Position = new[] { (double)Rng.Next(6000), -90.0d, (double)Rng.Next(6000) };//{ -245.0d, -15.0d, 110.0d };
                Direction = new[] { 0.0d, 0.0d, 0.0d };
                Zobj = new ZenObject();
                Zobj.setKlasse(ZENCLASS.MonsterContainer);
                Zobj.setName("Container");
                Zobj.setPosition(Position);
                Zobj.setDirection(Direction);
                _ObjectsToRespawn.Add(Zobj);
            }

        }

        private void RespawnObjects(object source, ElapsedEventArgs e)
        {
            foreach (ZenObject ZenObj in ObjectsToRespawn)
            {
                if (ZenObj.getKlasse() == ZENCLASS.PlantContainer /*&& Rng.NextDouble() <= 0.5d*/) //TODO: random chance that nothing spawns?
                {
                    double[] Position = ZenObj.getPosition();
                    double[] Direction = ZenObj.getDirection();

                    Vec3f VectorPos = new Vec3f((float)Position[0], (float)Position[1], (float)Position[2]);
                    Vec3f VectorDir = new Vec3f((float)Direction[0], (float)Direction[1], (float)Direction[2]);

                    Item item = new Item(ItemInstance.getItemInstance(this.PlantList.ElementAt(Rng.Next(PlantList.Count()))), 1);
                    item.Spawn(MAPNAME, VectorPos, VectorDir);
                }
                else if (ZenObj.getKlasse() == ZENCLASS.MonsterContainer)
                {
                    double[] Position = ZenObj.getPosition();
                    double[] Direction = ZenObj.getDirection();

                    Vec3f VectorPos = new Vec3f((float)Position[0], (float)Position[1], (float)Position[2]);
                    Vec3f VectorDir = new Vec3f((float)Direction[0], (float)Direction[1], (float)Direction[2]);

                    //this.MonsterList.ElementAt(Rng.Next(MonsterList.Count())));
                    //NPCInstance.InstanceDict["Mud"];
                    //NPCInstance.InstanceDict[3];
                }
                //else if (ZenObj.getKlasse() == ZENCLASS.OreContainer)
                //{
                //    //TODO
                //}
                //else if (ZenObj.getKlasse() == ZENCLASS.EventContainer)
                //{
                //    //TODO
                //}

            }
        }
    }
}



//Everything down below  is just for testing purposes. It will be replaced later with the real parser.
#region ParserStuff
public enum ZENTYPE { Waypoint, Vobs, zCVob, oCMob, oCItem, Container, RessourceContainer, PlantContainer, OreContainer, EventContainer, ALL }; // Don't change the order of this! The external parser depends on it.
public enum ZENCLASS { oCWaypoint, zCVob, oCMob, oCItem, PlantContainer, OreContainer, EventContainer, MonsterContainer};

public class ZenObject
{
    private ZENCLASS klasse;
    private String name;
    private double[] position = new double[3]; //zSlang seems to understand only double not float
    private double[] direction = new double[3];
    String visual;
    String itemInstance;

    public void setKlasse(ZENCLASS klasse)
    {
        this.klasse = klasse;
    }

    public ZENCLASS getKlasse()
    {
        return klasse;
    }

    public void setName(String name)
    {
        this.name = name;
    }

    public String getName()
    {
        return name;
    }

    public void setPosition(double[] position)
    {
        this.position = position;
    }

    public double[] getPosition()
    {
        return position;
    }

    public void setDirection(double[] direction)
    {
        this.direction = direction;
    }

    public double[] getDirection()
    {
        return direction;
    }

    public void setVisual(String visual)
    {
        this.visual = visual;
    }

    public String getVisual()
    {
        return visual;
    }

    public void setItemInstance(String itemInstance)
    {
        this.itemInstance = itemInstance;
    }

    public String getItemInstance()
    {
        return itemInstance;
    }
}
#endregion