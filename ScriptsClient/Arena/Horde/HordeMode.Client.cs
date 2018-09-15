using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Scripting;

namespace GUC.Scripts.Arena
{
    static partial class HordeMode
    {
        public static bool IsPlaying { get { return ArenaClient.Client.HordeClass != null && ArenaClient.Client.IsIngame; } }

        public static void ReadGameInfo(PacketReader stream)
        {
            string name = stream.ReadString();
            activeDef = HordeDef.GetDef(name);
            HordePhase phase = (HordePhase)stream.ReadByte();
            if (phase == HordePhase.Stand)
            {
                int index = stream.ReadByte();
                StartStand(activeDef.Stands[index]);
            }
            else
            {
                Endstand();
            }
            SetPhase(phase);
        }

        static HordeStand ActiveStand;
        static SoundInstance StandSFXLoop;
        static int messageIndex = 0;
        static GUCTimer messageTimer = new GUCTimer(NextMessage);
        static void StartStand(HordeStand stand)
        {
            ActiveStand = stand;

            if (IsPlaying)
            {
                if (!string.IsNullOrWhiteSpace(stand.SFXStart))
                    SoundHandler.PlaySound3D(new SoundDefinition(stand.SFXStart), stand.Position, 2500, 1.0f);

                if (!string.IsNullOrWhiteSpace(stand.SFXStart))
                    StandSFXLoop = SoundHandler.PlaySound3D(new SoundDefinition(stand.SFXLoop), stand.Position, 2500, 0.5f, true);

                if (stand.Messages != null && stand.Messages.Length > 0)
                {
                    messageIndex = 0;
                    NextMessage();
                    if (stand.Messages.Length > 1 && stand.Duration > 0)
                    {
                        messageTimer.SetInterval(stand.Duration * TimeSpan.TicksPerSecond / (stand.Messages.Length - 1));
                        messageTimer.Start();
                    }
                }
            }
        }

        static void NextMessage()
        {
            if (ActiveStand == null || messageIndex >= ActiveStand.Messages.Length)
            {
                messageTimer.Stop();
                return;
            }

            Log.Logger.Log(ActiveStand.Messages[messageIndex]);
            ChatMenu.Menu.AddMessage(ChatMode.Private, ActiveStand.Messages[messageIndex++]);
        }

        static void Endstand()
        {
            if (ActiveStand == null)
                return;

            if (StandSFXLoop != null)
                SoundHandler.StopSound(StandSFXLoop);

            var stand = ActiveStand;
            ActiveStand = null;
            if (IsPlaying)
            {
                if (!string.IsNullOrWhiteSpace(stand.SFXStart))
                    SoundHandler.PlaySound3D(new SoundDefinition(stand.SFXStop), stand.Position);
            }
            messageTimer.Stop();
        }

        public static void ReadStartMessage(PacketReader stream)
        {
            string name = stream.ReadString();
            activeDef = HordeDef.GetDef(name);
        }

        public static void ReadPhaseMessage(PacketReader stream)
        {
            HordePhase phase = (HordePhase)stream.ReadByte();
            SetPhase(phase);
            if (phase == HordePhase.Stand)
            {
                int index = stream.ReadByte();
                StartStand(activeDef.Stands[index]);
            }
            else
            {
                Endstand();
            }
        }

        static void SetPhase(HordePhase phase)
        {
            string screenMsg;
            switch (phase)
            {
                case HordePhase.WarmUp:
                    screenMsg = "warmup";
                    break;
                case HordePhase.Fight:
                    screenMsg = "Fight!";
                    break;
                case HordePhase.Stand:
                    screenMsg = "Stand";
                    break;
                case HordePhase.Victory:
                    screenMsg = "Sieg!";
                    HordeBoardScreen.Instance.Open();
                    break;
                case HordePhase.Lost:
                    screenMsg = "Niederlage!";
                    HordeBoardScreen.Instance.Open();
                    break;
                default: return;
            }

            Phase = phase;
            ScreenScrollText.AddText(screenMsg, GUI.GUCView.Fonts.Menu);
            OnPhaseChange?.Invoke(phase);
        }
    }
}
