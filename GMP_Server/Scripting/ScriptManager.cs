using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using Network;
using GMP_Server.Net.Message;

namespace GMP_Server.Scripting
{
    public class ScriptManager
    {
        private Assembly assembly;
        private bool startuped = false;
        private bool initalised = false;


        private List<Listener.IConnectionListener> connectionListener = new List<Listener.IConnectionListener>();
        private List<Listener.IChatListener> chatListener = new List<Listener.IChatListener>();
        private List<Listener.IDamageListener> damageListener = new List<Listener.IDamageListener>();
        private List<Listener.ITakeDropListener> takeDropListener = new List<Listener.ITakeDropListener>();
        private List<Listener.ILevelChangeListener> levelChangeListener = new List<Listener.ILevelChangeListener>();
        private List<Listener.IKeyListener> keyListener = new List<Listener.IKeyListener>();
        private List<Listener.ITextBoxListener> textBoxListener = new List<Listener.ITextBoxListener>();
        private List<Listener.IEquipmentListener> equipmentListener = new List<Listener.IEquipmentListener>();
        private List<Listener.ISlotListener> slotListener = new List<Listener.ISlotListener>();
        private List<Listener.ITriggerListener> triggerListener = new List<Listener.ITriggerListener>();
        private List<Listener.IUseItemListener> useItemListener = new List<Listener.IUseItemListener>();
        private List<Timer> timerList = new List<Timer>();



        public ScriptManager()
        {
            
        }

        public void init()
        {
            if (!Program.config.useScripts)
                return;

            if (initalised)
                return;

            if (Program.config.useScriptedFile)
            {
                load();

            }
            else
            {
                compile(Program.config.generateScriptedFile);
            }
        }

        private void load()
        {
            assembly = System.Reflection.Assembly.LoadFile(Path.GetFullPath("scripts/_compiled/ServerScripts.dll"));
        }

        private void compile(bool file)
        {
            System.CodeDom.Compiler.CodeDomProvider CodeDomProvider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp");

            System.CodeDom.Compiler.CompilerParameters CompilerParameters = new System.CodeDom.Compiler.CompilerParameters();
            CompilerParameters.ReferencedAssemblies.Add("System.dll");
            CompilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            CompilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
            CompilerParameters.ReferencedAssemblies.Add("RakNetSwig.dll");
            CompilerParameters.ReferencedAssemblies.Add("Network.dll");
            CompilerParameters.ReferencedAssemblies.Add("GMP_Server.exe");
            CompilerParameters.CompilerOptions = "/t:library";
            if (!file)
            {
                CompilerParameters.GenerateInMemory = true;
            }
            else
            {
                CompilerParameters.GenerateInMemory = false;
                CompilerParameters.OutputAssembly = "scripts/_compiled/ServerScripts.dll";
            }

            List<String> fileList = new List<string>();
            getFileList(fileList, "scripts/server");
            //getFileList(fileList, "scripts/both");

            System.CodeDom.Compiler.CompilerResults CompilerResults = CodeDomProvider.CompileAssemblyFromFile(CompilerParameters, fileList.ToArray());
            if (CompilerResults.Errors.Count > 0)
            {

                foreach (CompilerError col in CompilerResults.Errors)
                {

                    Log.Logger.log(Log.Logger.LOG_ERROR, col.FileName + ":" + col.Line + " \t" + col.ErrorText);
                }
                return;
            }


            assembly = CompilerResults.CompiledAssembly;
        }


        private void getFileList(List<String> filelist, String dir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    getFileList(filelist, d);
                }
                foreach (string f in Directory.GetFiles(dir, "*.cs"))
                {
                    if (f.ToLower().Trim().EndsWith(".cs"))
                        filelist.Add(f);
                }
            }
            catch (Exception excpt)
            {
                 Log.Logger.log(Log.Logger.LOG_ERROR, excpt.ToString());
            }
        }

        public void EnableChat(Player player)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.getID(), (byte)CommandoType.EnableChatBox, null, new int[] { 1 }, null);
        }

        public void DisableChat(Player player)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.getID(), (byte)CommandoType.EnableChatBox, null, new int[] { 0 }, null);
        }

        public void EnablePlayerKey(Player player)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.getID(), (byte)CommandoType.EnablePlayerKey, null, new int[] { 1 }, null);
        }

        public void DisablePlayerKey(Player player)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.getID(), (byte)CommandoType.EnablePlayerKey, null, new int[] { 0 }, null);
        }

        public void addTimerListener(long time, Listener.ITimerListener listener)
        {
            timerList.Add(new Timer() { startTime = DateTime.Now.Ticks, time = time, listener = listener });
        }

        public void update()
        {
            foreach (Timer timer in this.timerList)
            {
                if (timer.startTime + timer.time < DateTime.Now.Ticks)
                {
                    timer.listener.OnTick();
                    timer.startTime = DateTime.Now.Ticks;
                }
            }
        }

        public void addUseItemListener(Listener.IUseItemListener kl)
        {
            this.useItemListener.Add(kl);
        }

        public void OnUseItem(Player player, string instance)
        {
            Listener.IUseItemListener[] list = useItemListener.ToArray();
            foreach (Listener.IUseItemListener listern in list)
            {
                listern.OnUseItem(player, instance);
            }
        }

        public void addTriggerListener(Listener.ITriggerListener kl)
        {
            this.triggerListener.Add(kl);
        }

        public void OnTrigger(Player pl, int vobType, String vobName, float[] pos)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnTrigger(pl, vobType, vobName, pos);
            }
        }

        public void OnUnTrigger(Player pl, int vobType, String vobName, float[] pos)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnUnTrigger(pl, vobType, vobName, pos);
            }
        }

        public void OnStartInteraction(Player pl, int vobType, String vobName, float[] pos)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnStartInteraction(pl, vobType, vobName, pos);
            }
        }

        public void OnStopInteraction(Player pl, int vobType, String vobName, float[] pos)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnStopInteraction(pl, vobType, vobName, pos);
            }
        }

        public void OnPickLock(Player pl, int vobType, String vobName, float[] pos, int ch)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnPickLock(pl, vobType, vobName, pos, ch);
            }
        }

        public void OnOpenContainer(Player pl, int vobType, String vobName, float[] pos)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnOpenContainer(pl, vobType, vobName, pos);
            }
        }

        public void OnCloseContainer(Player pl, int vobType, String vobName, float[] pos, bool hasKey)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnCloseContainer(pl, vobType, vobName, pos, hasKey);
            }
        }

        public void OnInsertItemToContainer(Player pl, int vobType, String vobName, float[] pos, String item, int amount)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnInsertItemToContainer(pl, vobType, vobName, pos, item, amount);
            }
        }
        public void OnRemoveItemToContainer(Player pl, int vobType, String vobName, float[] pos, String item, int amount)
        {
            Listener.ITriggerListener[] list = triggerListener.ToArray();
            foreach (Listener.ITriggerListener listern in list)
            {
                listern.OnRemoveItemToContainer(pl, vobType, vobName, pos, item, amount);
            }
        }






        public void addSlotListener(Listener.ISlotListener kl)
        {
            this.slotListener.Add(kl);
        }

        public void OnRightHandSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnRightHandSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnLeftHandSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnLeftHandSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnSwordSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnSwordSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnLongSwordSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnLongSwordSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnBowSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnBowSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnCrossBowSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnCrossBowSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnTorsoSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnTorsoSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnHelmetSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnHelmetSlotChanged(pl, oldItem, newItem);
            }
        }

        public void OnShieldSlotChanged(Player pl, String oldItem, String newItem)
        {
            if (oldItem == newItem)
                return;
            Listener.ISlotListener[] list = slotListener.ToArray();
            foreach (Listener.ISlotListener listern in list)
            {
                listern.OnShieldSlotChanged(pl, oldItem, newItem);
            }
        }


        public void addEquipmentListener(Listener.IEquipmentListener kl)
        {
            this.equipmentListener.Add(kl);
        }

        public void OnWeaponChanged(Player pl, String oldWeapon, String newWeapon)
        {
            if (oldWeapon == newWeapon)
                return;
            Listener.IEquipmentListener[] list = equipmentListener.ToArray();
            foreach (Listener.IEquipmentListener listern in list)
            {
                listern.OnWeaponChanged(pl, oldWeapon, newWeapon);
            }
        }

        public void OnRangeWeaponChanged(Player pl, String oldWeapon, String newWeapon)
        {
            if (oldWeapon == newWeapon)
                return;
            Listener.IEquipmentListener[] list = equipmentListener.ToArray();
            foreach (Listener.IEquipmentListener listern in list)
            {
                listern.OnRangeWeaponChanged(pl, oldWeapon, newWeapon);
            }
        }

        public void OnArmorChanged(Player pl, String oldWeapon, String newWeapon)
        {
            if (oldWeapon == newWeapon)
                return;
            Listener.IEquipmentListener[] list = equipmentListener.ToArray();
            foreach (Listener.IEquipmentListener listern in list)
            {
                listern.OnArmorChanged(pl, oldWeapon, newWeapon);
            }
        }






        public void addTextBoxListener(Listener.ITextBoxListener kl)
        {
            textBoxListener.Add(kl);
        }

        public void OnTextBoxMessageReceived(TextBox tb, Player pl, String message)
        {
            Listener.ITextBoxListener[] list = textBoxListener.ToArray();
            foreach (Listener.ITextBoxListener listern in list)
            {
                listern.OnMessageReceived(tb, pl, message);
            }

            tb.OnTextBoxMessageReceived(tb, pl, message);
        }

        public void addKeyListener(Listener.IKeyListener kl)
        {
            keyListener.Add(kl);
        }

        public void OnKeyAction(Player pl, int key, bool pressed, byte times)
        {
            Listener.IKeyListener[] list = keyListener.ToArray();
            foreach (Listener.IKeyListener listern in list)
            {
                listern.OnKeyAction(pl, key, pressed, times);
            }
        }

        public void addConnectionListener(Listener.IConnectionListener conListener)
        {
            connectionListener.Add(conListener);
        }

        public void OnPlayerConnected(Player pl)
        {
            Listener.IConnectionListener[] list = connectionListener.ToArray();
            foreach(Listener.IConnectionListener listern in list){
                listern.OnPlayerConnected( pl );
            }
        }

        public void OnPlayerDiconnected(Player pl)
        {
            Listener.IConnectionListener[] list = connectionListener.ToArray();
            foreach(Listener.IConnectionListener listern in list){
                listern.OnPlayerDisconnected(pl);
            }
        }

        public void addChatListener(Listener.IChatListener chListener)
        {
            chatListener.Add(chListener);
        }

        public bool OnChatMessageReceived(Player pl, ref String name, ref String content, ref byte type)
        {
            Listener.IChatListener[] list = chatListener.ToArray();
            bool send = true;
            foreach (Listener.IChatListener listern in list)
            {
                if(!listern.OnChatMessageReceived(pl, ref name, ref content, ref type)){
                    send = false;
                }
            }
            return send;
        }

        public void addDamageListener(Listener.IDamageListener dmgListener)
        {
            damageListener.Add(dmgListener);
        }

        public void OnDamage(ref int attackID, ref int victimID, ref byte DamageMode, ref byte WeaponMode, byte itemType, string weapon)
        {
            Listener.IDamageListener[] list = damageListener.ToArray();
            foreach (Listener.IDamageListener listern in list)
            {
                listern.OnDamageReceived(ref attackID, ref victimID, ref DamageMode, ref WeaponMode, itemType, weapon);
            }
        }

        public void addTakeDropListener(Listener.ITakeDropListener tdListener)
        {
            this.takeDropListener.Add(tdListener);
        }

        public void OnTakeItem(Player pl, String itmCode, int amount)
        {
            Listener.ITakeDropListener[] list = takeDropListener.ToArray();
            foreach (Listener.ITakeDropListener listern in list)
            {
                listern.OnTakeItem(pl, itmCode, amount);
            }
        }

        public void OnDropItem(Player pl, String itmCode, int amount)
        {
            Listener.ITakeDropListener[] list = takeDropListener.ToArray();
            foreach (Listener.ITakeDropListener listern in list)
            {
                listern.OnDropItem(pl, itmCode, amount);
            }
        }

        public void addLevelChangeListener(Listener.ILevelChangeListener lvlChListener)
        {
            this.levelChangeListener.Add(lvlChListener);
        }

        public void OnLevelChange(Player pl, String world, String oldworld)
        {
            Listener.ILevelChangeListener[] list = levelChangeListener.ToArray();
            foreach (Listener.ILevelChangeListener listern in list)
            {
                listern.OnLevelChanged(pl, world, oldworld);
            }
        }


        public void Startup()
        {
            if (startuped)
                return;
            if (!Program.config.useScripts)
                return;

            startuped = true;

            Listener.IServerStartup listener = (Listener.IServerStartup)assembly.CreateInstance("GMP_Server.Scripts.Startup");
            listener.OnServerInit();
            
        }
    }
}
