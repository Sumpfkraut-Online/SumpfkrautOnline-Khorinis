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

void test(wchar_t* t1, wchar_t* t2, wchar_t* t3){
	std::wstring str;
	str.append(L"Error: DLL: ");
	str.append(t1);
	str.append(L" , Class: ");
	str.append(t2);
	str.append(L" , Method: ");
	str.append(t3);
	str.append(L" !");
	MessageBoxW(NULL,(LPCWSTR)str.c_str(),L"Error!", MB_ICONWARNING | MB_CANCELTRYCONTINUE | MB_DEFBUTTON2 );
}

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
	// execute the method "Int32 Test.Program.InjectedMain(String arg)"

	hr = pClrRuntimeHost->ExecuteInDefaultAppDomain(dllName, typeName, methodName, ptrAddress, &result);
	/*pClrRuntimeHost->Stop();
	pRuntimeInfo->Release();
	pMetaHost->Release();
	pClrRuntimeHost->Release();*/
	

	if(hr != S_OK){
		//test(dllName, typeName, methodName);
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

	// execute the method "Int32 Test.Program.InjectedMain(String arg)"
	hr = pClrRuntimeHost->ExecuteInDefaultAppDomain(L"UntoldChapter\\DLL\\GUC.dll", L"GUC.Program", L"InjectedMain", L"injected.txt", &result);
	pClrRuntimeHost->Stop();

	pRuntimeInfo->Release();
	pMetaHost->Release();
	pClrRuntimeHost->Release();

	delete pMetaHost;
	delete pRuntimeInfo;

	if(hr != S_OK){
		test(L"UntoldChapter\DLL\GUC.dll", L"GUC.Program", L"InjectedMain");
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