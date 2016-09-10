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

namespace GUC
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

        struct ActiveSound
        {
            public long startTime;
            public int idPtr;
            public zTSound3DParams sndParams;
            public SoundInstance sfx;
            public zCVob sndVob;

            public ActiveSound(int id, zTSound3DParams sndParams, SoundInstance sfx, zCVob sndVob = null)
            {
                this.startTime = GameTime.Ticks;
                this.idPtr = id;
                this.sndParams = sndParams;
                this.sfx = sfx;
                this.sndVob = sndVob;
            }
        }

        static bool CanPlay(SoundInstance sound)
        {
            for (int i = 0; i < vobSounds.Count; i++)
            {
                if (vobSounds[i].sfx == sound || vobSounds[i].sfx.sfx.Address == sound.sfx.Address)
                {
                    if (GameTime.Ticks - vobSounds[i].startTime < 50 * TimeSpan.TicksPerMillisecond)
                        return false;
                }
            }

            for (int i = 0; i < locSounds.Count; i++)
            {
                if (vobSounds[i].sfx == sound || vobSounds[i].sfx.sfx.Address == sound.sfx.Address)
                {
                    if (GameTime.Ticks - vobSounds[i].startTime < 50 * TimeSpan.TicksPerMillisecond)
                        return false;
                }
            }

            return true;
        }

        static readonly List<ActiveSound> vobSounds = new List<ActiveSound>();
        static readonly List<ActiveSound> locSounds = new List<ActiveSound>();
        static readonly List<zCVob> sndVobs = new List<zCVob>();

        public static void PlaySound3D(SoundInstance sound, BaseVob vob, float range = -1.0f, float volume = 1.0f)
        {
            if (sound == null)
                throw new ArgumentNullException("Sound is null!");
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (!CanPlay(sound)) // don't play too many sounds at once
                return;

            var param = zTSound3DParams.Create();
            param.Volume = volume;
            param.Radius = range;
            param.IsAmbient = true;
            int idPtr = Process.Alloc(4).ToInt32();
            Process.Write(zCSndSys_MSS.PlaySound3D(sound.sfx, vob.gVob, 0, param), idPtr);

            vobSounds.Add(new ActiveSound(idPtr, param, sound));
        }

        public static void PlaySound3D(SoundInstance sound, Vec3f location, float range = -1.0f, float volume = 1.0f)
        {
            if (sound == null)
                throw new ArgumentNullException("Sound is null!");

            if (!CanPlay(sound)) // don't play too many sounds at once
                return;

            var param = zTSound3DParams.Create();
            param.Volume = volume;
            param.Radius = range;
            param.IsAmbient = true;

            zCVob vob;
            if (sndVobs.Count == 0)
            {
                vob = zCVob.Create();
            }
            else
            {
                int index = sndVobs.Count - 1;
                vob = sndVobs[index];
                sndVobs.RemoveAt(index);
            }

            Gothic.oCGame.GetWorld().AddVob(vob);
            vob.TrafoObjToWorld.Position = location.ToArray();
            vob.SetPositionWorld(location.X, location.Y, location.Z);

            int idPtr = Process.Alloc(4).ToInt32();
            Process.Write(zCSndSys_MSS.PlaySound3D(sound.sfx, vob, 0, param), idPtr);

            locSounds.Add(new ActiveSound(idPtr, param, sound, vob));
        }

        internal static void Update3DSounds()
        {
            try
            {
                for (int i = vobSounds.Count - 1; i >= 0; i--)
                {
                    var tup = vobSounds[i];

                    if (!zCSndSys_MSS.UpdateSound3D(tup.idPtr, tup.sndParams) && !zCSndSys_MSS.IsSoundActive(tup.idPtr))
                    {
                        Process.Free(new IntPtr(tup.idPtr), 4);
                        tup.sndParams.Free();
                        vobSounds.RemoveAt(i);
                    }
                }

                for (int i = locSounds.Count - 1; i >= 0; i--)
                {
                    var tup = locSounds[i];

                    if (!zCSndSys_MSS.UpdateSound3D(tup.idPtr, tup.sndParams) && !zCSndSys_MSS.IsSoundActive(tup.idPtr))
                    {
                        Process.Free(new IntPtr(tup.idPtr), 4);

                        tup.sndVob.RemoveVobFromWorld();
                        sndVobs.Add(tup.sndVob);
                        //tup.Item2.refCtr--;
                        //tup.Item2.Dispose();

                        tup.sndParams.Free();
                        locSounds.RemoveAt(i);
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
