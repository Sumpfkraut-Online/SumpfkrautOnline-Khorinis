using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Log;
using Gothic.Sound;
using GUC.WorldObjects;
using GUC.Types;
using Gothic.Objects;

namespace GUC.Client
{
    public class SoundInstance
    {
        public zCSndFX_MSS sfx { get; private set; }

        public SoundInstance(string sfxName)
        {
            this.sfx = zCSndSys_MSS.LoadSoundFX(sfxName);
        }
    }

    public static class SoundHandler
    {
        public enum MusicType
        {
            Normal,
            Threat,
            Fight
        }

        static MusicType currentMusicType = MusicType.Normal;

        public static MusicType CurrentMusicType
        {
            get { return SoundHandler.currentMusicType; }
            set
            {
                if (SoundHandler.currentMusicType == value)
                    return;

                SoundHandler.currentMusicType = value;
                Process.Write(new byte[] { 0xB8, (byte)value, 0x00, 0x00, 0x00, 0xC3 }, 0x6C2D10);

                Logger.Log("SoundHandler: Play music type " + value);
            }
        }

        public static float MasterVolume
        {
            get { return zCSndSys_MSS.GetMasterVolume(); }
            set { zCSndSys_MSS.SetMasterVolume(value); }
        }

        public static void PlaySound(SoundInstance sound, float volume = 1.0f)
        {
            if (sound == null)
                throw new ArgumentNullException("Sound is null!");
            zCSndSys_MSS.PlaySound(sound.sfx, false, 0, volume, 0);
        }

        static readonly List<Tuple<int, zTSound3DParams>> vobSounds = new List<Tuple<int, zTSound3DParams>>();

        public static void PlaySound3D(SoundInstance sound, BaseVob vob, float range = -1.0f, float volume = 1.0f)
        {
            if (sound == null)
                throw new ArgumentNullException("Sound is null!");
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            var param = zTSound3DParams.Create();
            param.Volume = volume;
            param.Radius = range;
            param.IsAmbient = true;            
            int idPtr = Process.Alloc(4).ToInt32();
            Process.Write(zCSndSys_MSS.PlaySound3D(sound.sfx, vob.gvob, 0, param), idPtr);

            vobSounds.Add(new Tuple<int, zTSound3DParams>(idPtr, param));
        }

        internal static void Update3DSounds()
        {
            try
            {
                for (int i = vobSounds.Count - 1; i >= 0; i--)
                {
                    var tup = vobSounds[i];

                    if (!zCSndSys_MSS.UpdateSound3D(tup.Item1, tup.Item2))
                    {
                        Process.Free(new IntPtr(tup.Item1), 4);
                        tup.Item2.Free();
                        vobSounds.RemoveAt(i);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning(e);
            }
        }
    }
}
