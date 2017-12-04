using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;

namespace WinApi.Calls
{
    public abstract class AbstractCall
    {
        int address;
        public int Address { get { return this.address; } }

        public AbstractCall(int address)
        {
            if (address == 0)
                throw new ArgumentNullException("address");

            this.address = address;
        }

        #region Marshal Extension

        delegate Delegate GetDelegateHandler(IntPtr ptr, Type type);
        static GetDelegateHandler GetDelegate;

        static AbstractCall()
        {
            MethodInfo method = typeof(Marshal).GetMethod("GetDelegateForFunctionPointerInternal", BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null)
                throw new Exception("GetDelegate method not found!");

            GetDelegate = (GetDelegateHandler)Delegate.CreateDelegate(typeof(GetDelegateHandler), method);
            if (GetDelegate ==  null)
                throw new Exception("Created GetDelegate is null!");
        }

        internal static TDelegate GetDelegateFromFunction<TDelegate>(int ptr)
        {
            return (TDelegate)(object)GetDelegate(new IntPtr(ptr), typeof(TDelegate));
        }

        #endregion
    }

    /* public static class DelegateCreator
     {
         private const MethodAttributes CtorAttributes = MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public;
         private const MethodImplAttributes ImplAttributes = MethodImplAttributes.Runtime | MethodImplAttributes.Managed;
         private const MethodAttributes InvokeAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual;
         private static readonly Type[] _DelegateCtorSignature = new Type[] { typeof(object), typeof(IntPtr) };
         public static Type NewActionType(Type returnType, params Type[] parameters)
         {
             var DefineDelegateType = (Func<string, TypeBuilder>)Delegate.CreateDelegate(typeof(Func<string, TypeBuilder>), typeof(Expression).Assembly.GetType("System.Linq.Expressions.Compiler.AssemblyGen").GetMethod("DefineDelegateType", BindingFlags.NonPublic | BindingFlags.Static));
             if (DefineDelegateType == null)
                 throw new Exception();

             TypeBuilder builder = DefineDelegateType("Delegate");
             builder.DefineConstructor(CtorAttributes, CallingConventions.Standard, _DelegateCtorSignature).SetImplementationFlags(ImplAttributes);
             builder.DefineMethod("Invoke", InvokeAttributes, returnType, parameters).SetImplementationFlags(ImplAttributes);

             var conInfo = typeof(UnmanagedFunctionPointerAttribute).GetConstructor(new Type[] { typeof(CallingConvention) });
             var attributeBuilder = new CustomAttributeBuilder(conInfo, new object[] { CallingConvention.ThisCall });
             builder.SetCustomAttribute(attributeBuilder);

             return builder.CreateType();
         }

         [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
         public delegate void TestHandler(int self, float arg);
         public static TestHandler GetCall(int address)
         {
             TestHandler result = null;
             try
             {
                 Type t = NewActionType(null, typeof(int), typeof(float));
                 Delegate del = Marshal.GetDelegateForFunctionPointer(new IntPtr(address), t);
                 result = (TestHandler)Delegate.CreateDelegate(typeof(TestHandler), null, del.Method);
             }
             catch (Exception e)
             {
                 System.IO.File.WriteAllText("D:\\debug.txt", e.ToString());
                 if (e.InnerException != null)
                     System.IO.File.WriteAllText("D:\\debug2.txt", e.InnerException.ToString());

             }
             return result;
         }
     }*/
}
