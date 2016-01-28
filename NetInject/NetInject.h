// NetInject.h

#include "windows.h"
#include <mscoree.h>
#include <corerror.h>

#include <metahost.h>
#include <string>

struct NETINJECTPARAMS
{
	char* dllName;
	char* typeName;
	char* methodName;
	char* ptrAddress;
};

void Error(wchar_t* text){
	MessageBoxW(NULL,text,L"Error!", MB_ICONWARNING | MB_CANCELTRYCONTINUE | MB_DEFBUTTON2 );
}

wchar_t * convertToChar(char * c)
{
	int len = strlen(c)+1;
	wchar_t *dllName = new wchar_t[len];
	if ( dllName == 0 )
		return dllName;
	memset(dllName,0,len);
	::MultiByteToWideChar(CP_ACP, NULL, c, -1, dllName, len);

	return dllName;
}


EXTERN_C __declspec(dllexport) void LoadNetDllEx(NETINJECTPARAMS* params)
{
	static ICLRMetaHost *pMetaHost = NULL;
    static ICLRRuntimeInfo *pRuntimeInfo = NULL;
    static ICLRRuntimeHost *pClrRuntimeHost = NULL;
	HRESULT hr;
	DWORD result;

	wchar_t* dllName = convertToChar(params->dllName);
	wchar_t* typeName = convertToChar(params->typeName);
	wchar_t* methodName = convertToChar(params->methodName);
	wchar_t* ptrAddress = convertToChar(params->ptrAddress);

	// start the .NET Runtime in the current native process
	if(pMetaHost == NULL){
		hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&pMetaHost);
		hr = pMetaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*)&pRuntimeInfo);
		hr = pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)&pClrRuntimeHost);
		hr = pClrRuntimeHost->Start();

		//test(dllName, typeName, methodName);
	}

	hr = pClrRuntimeHost->ExecuteInDefaultAppDomain(dllName, typeName, methodName, ptrAddress, &result);
	/*pClrRuntimeHost->Stop();
	pRuntimeInfo->Release();
	pMetaHost->Release();
	pClrRuntimeHost->Release();*/

	if(hr != S_OK){
		Error((wchar_t *)(std::wstring(L"LoadNetDllEx failed on Method: ") + methodName).c_str());
	}
	
	delete[] dllName;
	delete[] typeName;
	delete[] methodName;
	delete[] ptrAddress;

	/*delete pMetaHost;
	delete pRuntimeInfo;*/
}

void LoadNETDLL()
{
	ICLRMetaHost *pMetaHost = NULL;
    ICLRRuntimeInfo *pRuntimeInfo = NULL;
    ICLRRuntimeHost *pClrRuntimeHost = NULL;
	HRESULT hr;
	DWORD result;

	// start the .NET Runtime in the current native process
	hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&pMetaHost);
    hr = pMetaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*)&pRuntimeInfo);
    hr = pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)&pClrRuntimeHost);
    hr = pClrRuntimeHost->Start();

	LPCWSTR project = convertToChar(getenv("GUCProject"));
	std::wstring projectDll = std::wstring(L"Multiplayer\\UntoldChapters\\") + project + std::wstring(L"\\GUC.dll");
	hr = pClrRuntimeHost->ExecuteInDefaultAppDomain(projectDll.c_str(), L"GUC.Client.Injection", L"Main", project, &result);
	pClrRuntimeHost->Stop();

	pRuntimeInfo->Release();
	pMetaHost->Release();
	pClrRuntimeHost->Release();

	delete pMetaHost;
	delete pRuntimeInfo;

	if(hr != S_OK){
		Error((wchar_t *)(std::wstring(L"DLL-Injection of ") + projectDll + std::wstring(L" failed!")).c_str());
	}
}

int WINAPI DllMain(HINSTANCE hInst, DWORD reason, LPVOID reserved)
{
	if(reason==DLL_PROCESS_ATTACH)
	{
		CreateThread(0, 0, (LPTHREAD_START_ROUTINE) LoadNETDLL, 0, 0, 0);		
	}
	return true;
}