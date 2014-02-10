// NetInject.h

#include "windows.h"
#include <mscoree.h>
#include <corerror.h>

struct NETINJECTPARAMS
{
	char* dllName;
	char* typeName;
	char* methodName;
	char* ptrAddress;
};

wchar_t * convertToChar(char * c)
{
	int len = strlen(c)+1;
	wchar_t *dllName = new wchar_t[len];
	if ( dllName == 0 )
		return dllName;
	memset(dllName,0,len);
	::MultiByteToWideChar(  CP_ACP, NULL,c, -1, dllName,len );

	return dllName;
}


EXTERN_C __declspec(dllexport) void LoadNetDllEx(NETINJECTPARAMS* params)
{
	ICLRRuntimeHost* pCLR = NULL;
	DWORD result;


	wchar_t* dllName = convertToChar(params->dllName);
	wchar_t* typeName = convertToChar(params->typeName);
	wchar_t* methodName = convertToChar(params->methodName);
	wchar_t* ptrAddress = convertToChar(params->ptrAddress);
	wchar_t* x = L"wks";
	// start the .NET Runtime in the current native process
	HRESULT hr = CorBindToRuntimeEx(NULL, x, 0, CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (PVOID*)&pCLR);
	hr = pCLR->Start();

	// execute the method "Int32 Test.Program.InjectedMain(String arg)"

	hr = pCLR->ExecuteInDefaultAppDomain(dllName, typeName, methodName, ptrAddress, &result);
	pCLR->Stop();
	pCLR->Release();

	
	free(dllName);
	free(typeName);
	free(methodName);
	free(ptrAddress);
	free(x);
	//free(pCLR);
}

void LoadNETDLL()
{
	ICLRRuntimeHost* pCLR = NULL;
	DWORD result;

	// start the .NET Runtime in the current native process
	HRESULT hr = CorBindToRuntimeEx(NULL, L"wks", 0, CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (PVOID*)&pCLR);
	hr = pCLR->Start();

	// execute the method "Int32 Test.Program.InjectedMain(String arg)"
	hr = pCLR->ExecuteInDefaultAppDomain(L"GUC.dll", L"Injection.Program", L"InjectedMain", L"injected.txt", &result);
	pCLR->Stop();
	pCLR->Release();
	//free(pCLR);
}

int WINAPI DllMain(HINSTANCE hInst,DWORD reason,LPVOID reserved)
{
	
	if(reason==DLL_PROCESS_ATTACH)
	{
		CreateThread(0, 0, (LPTHREAD_START_ROUTINE) LoadNETDLL, 0, 0, 0);
	}
	return true;
}