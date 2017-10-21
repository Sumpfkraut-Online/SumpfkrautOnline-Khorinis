using System;
using System.Collections.Generic;
using System.Text;
using WinApi.Kernel.Exception;
using WinApi.Kernel;
using KFile = WinApi.Kernel.File;
using WinApi.SetupApi.Structures;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using WinApi.HID.Structures;
using System.IO;

namespace WinApi
{
    abstract class HIDDevice : IDisposable
    {
        const Int64 INVALID_HANDLE_VALUE = -1;
        static IntPtr INVALID_HANDLE_VALUE_PTR = new IntPtr(-1);

        #region Statics
        public static Guid HidGuid()
        {
            Guid guid = Guid.Empty;
            HID.HID.HidD_GetHidGuid(ref guid);
            return guid;
        }

        public static List<T> FindKnownHidDevices<T>() where T : HIDDevice, new()
        {
            List<T> rList = new List<T>();
            Guid hidGuid;
            IntPtr hardwareDeviceInfoPtr;
            bool done;
            uint index;
            
            hidGuid = HidGuid();

            hardwareDeviceInfoPtr = SetupApi.ClassDev.SetupDiGetClassDevs(ref hidGuid, null, IntPtr.Zero, (int)SetupApi.ClassDev.DiGetClassFlags.DIGCF_PRESENT | (int)SetupApi.ClassDev.DiGetClassFlags.DIGCF_DEVICEINTERFACE);

            if (hardwareDeviceInfoPtr == INVALID_HANDLE_VALUE_PTR)
            {
                Error.GetLastError();
            }

            done = true;
            index = 0;

            while (done)
            {
                DeviceInterfaceData did = new DeviceInterfaceData();
                did.size = (uint)Marshal.SizeOf(did);

                done = SetupApi.ClassDev.SetupDiEnumDeviceInterfaces(hardwareDeviceInfoPtr, IntPtr.Zero, ref hidGuid, index, ref did);
                if (done)
                {
                    String path = getDevicePath(hardwareDeviceInfoPtr, ref did);
                    T newDevice = new T();
                    newDevice.Initialise(path);
                    rList.Add(newDevice);
                }
                index++;
            }

            return rList;
        }

        public static T FindDevice<T>(int vendorid, int productid) where T : HIDDevice, new()
        {
            T rValue = null;

            string strSearch = string.Format("vid_{0:x4}&pid_{1:x4}", vendorid, productid).ToLower();

            List<T> list = FindKnownHidDevices<T>();
            foreach(T obj in list)
            {
                if(obj.Path.ToLower().IndexOf(strSearch) >= 0)
                    return obj;
            }

            return rValue;
        }

        public static String getDevicePath(IntPtr hardwareDeviceInfoPtr, ref DeviceInterfaceData did)
        {
            uint size = 0;
            if (!SetupApi.ClassDev.SetupDiGetDeviceInterfaceDetail(hardwareDeviceInfoPtr, ref did, IntPtr.Zero, 0, ref size, IntPtr.Zero))
            {
                DeviceInterfaceData da = new DeviceInterfaceData();
                da.size = (uint)Marshal.SizeOf(da);

                DeviceInterfaceDetailData detail = new DeviceInterfaceDetailData();
                if (IntPtr.Size == 8)
                    detail.size = 8;
                else
                    detail.size = 4 + (uint)Marshal.SystemDefaultCharSize;
                
                if(SetupApi.ClassDev.SetupDiGetDeviceInterfaceDetail(hardwareDeviceInfoPtr, ref did, ref detail, size, out size, ref da  ))
                {
                    return detail.DevicePath;
                }
                Error.GetLastError();
            }
            
            return null;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposer called by both dispose and finalise
        /// </summary>
        /// <param name="bDisposing">True if disposing</param>
        protected virtual void Dispose(bool bDisposing)
        {
            try
            {
                if (bDisposing)	// if we are disposing, need to close the managed resources
                {
                    if (File != null)
                    {
                        File.Close();
                        File = null;
                    }
                }
                if (Handle != null)	// Dispose and finalize, get rid of unmanaged resources
                {

                    KFile.CloseHandle(Handle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion



        public String Path { get; protected set; }
        protected SafeFileHandle Handle;
        public HidCaps HidCap { get; protected set;}
        protected System.IO.FileStream File;

        public event EventHandler OnDeviceRemoved;

        public IntPtr DeviceControl
        {
            get
            {
                return IntPtr.Zero;//Kernel.DeviceIoControls.DeviceIoControl(Handle, DeviceIoControls.EIOControlCode.)
            }

        }

        public void Initialise(String path)
        {
            this.Path = path;
        }

        public void Initialise()
        {
            
            IntPtr parsedData;
            HidCaps caps;
            this.Handle = KFile.CreateFile(Path, KFile.EFileAccess.GenericRead | KFile.EFileAccess.GenericWrite, 0, IntPtr.Zero, KFile.ECreationDisposition.OpenExisting, KFile.EFileAttributes.Overlapped, IntPtr.Zero);

            if (Handle == null || Handle.IsInvalid)
            {
                Handle = null;
                Kernel.Error.GetLastError();
            }

            if (!HID.HID.HidD_GetPreparsedData(Handle, out parsedData))
            {
                Kernel.Error.GetLastError();
            }

            try
            {
                if ((uint)HID.Structures.ntStatus.HIDP_STATUS_SUCCESS != HID.HID.HidP_GetCaps(parsedData, out caps))
                    Kernel.Error.GetLastError();
                HidCap = caps;

                File = new System.IO.FileStream(Handle, System.IO.FileAccess.Read | System.IO.FileAccess.Write, HidCap.InputReportByteLength, true);
                
                BeginAsyncRead();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                HID.HID.HidD_FreePreparsedData(ref parsedData);
            }
        }

        private void BeginAsyncRead()
        {
            byte[] arrInputReport = new byte[HidCap.InputReportByteLength];
            // put the buff we used to receive the stuff as the async state then we can get at it when the read completes

            File.BeginRead(arrInputReport, 0, HidCap.InputReportByteLength, new AsyncCallback(ReadCompleted), arrInputReport);
        }


        protected void ReadCompleted(IAsyncResult iResult)
        {
            byte[] arrBuff = (byte[])iResult.AsyncState;
            try
            {
                File.EndRead(iResult);	// call end read : this throws any exceptions that happened during the read
                try
                {
                    InputReport oInRep = CreateInputReport();	// Create the input report for the device
                    oInRep.SetData(arrBuff);	// and set the data portion - this processes the data received into a more easily understood format depending upon the report type
                    HandleDataReceived(oInRep);	// pass the new input report on to the higher level handler
                }
                finally
                {
                    BeginAsyncRead();	// when all that is done, kick off another read for the next report
                }
            }
            catch
            {
                HandleDeviceRemoved();
                if (OnDeviceRemoved != null)
                {
                    OnDeviceRemoved(this, new EventArgs());
                }
                Dispose();
            }
        }

        protected void Write(OutputReport oOutRep)
        {
            try
            {
                File.Write(oOutRep.Buffer, 0, oOutRep.BufferLength);
            }
            catch (IOException ex)
            {
                throw new Exception(ex.ToString());
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx.ToString());
            }
        }

        protected abstract void HandleDataReceived(InputReport oInRep);
        protected abstract void HandleDeviceRemoved();
        protected abstract InputReport CreateInputReport();

    }

    
}
