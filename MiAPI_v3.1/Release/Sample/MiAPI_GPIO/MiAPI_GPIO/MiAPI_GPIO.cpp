// MiAPI_GPIO.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <Windows.h>
#include "MiAPI.h"

#pragma comment(lib, "MiAPI.lib")
#pragma warning(disable:4996)

void usage(void)
{
	int Index;
	static const TCHAR *Str[] = {
		_T(" MiAPI GPIO tool v0.01"),
		_T("============================================"),
		_T("Options:"),
		_T("  -h             : Help information."),
		_T("  -v             : Show current product name and BIOS version."),
		_T("  -get pin       : Get GPIOS status from pin_num."),
		_T("  -set pin dir level : set GPIO pin as configuration."),
		_T("       dir   :  0  --> Output ; 1 --> Input."),
		_T("       level :  0  --> Low    ; 1 --> High."),
		NULL
	};

	
	for (Index = 0; Str[Index] != NULL; Index++) {
		_tprintf(_T("%s\n"), Str[Index]);
	}
}


int _tmain(int argc, _TCHAR* argv[])
{
	int Major, Minor;
	char BIOSVersion[80];
	char ProductName[80];
	MIAPI_GPIO_STATUS gpio_status;
	DWORD size;
	BYTE pin;
	int errorcode = MiAPI_OK;
	

	//-- Start the MiAPI libary
	if (MiAPI_Start() != MiAPI_OK)
	{
		_tprintf(_T("Error: Failed to initialize MiAPI library.\n"));
		return MiAPI_INIT_FAIL;
	}
	
	//-- Call the GPIO functions
	if (argc == 1 || (_tcscmp(argv[1], _T("-h")) == 0))
	{
		usage();
	}
	else if (_tcscmp(argv[1], _T("-v")) == 0)
	{
		//--  Call MiAPI functions to get Product name, BIOS version and MiAPI version.

		MiAPI_GetProductName(ProductName, &size);
		_tprintf(_T("Product name : %s\n"), ProductName);

		MiAPI_GetBIOSVersion(BIOSVersion, &size);
		_tprintf(_T("BIOS version : %s\n"), BIOSVersion);

		errorcode = MiAPI_GetMiAPIVersion(&Major, &Minor);
		_tprintf(_T("MAPI DLL version : %d.%d \n"), Major, Minor);
		if(errorcode == MiAPI_OLD_VERSION ) _tprintf("[Note] This MiAPI DLL only supports less legacy features for this mother board!\n");
	}
	else if (_tcscmp(argv[1], _T("-get")) == 0)
	{			
		pin = _ttoi(argv[2]);
		gpio_status.Direction = 0;
		gpio_status.VoltageLevel = 0;
				
		if (MiAPI_GPIO_GetStatus((BYTE)pin, &gpio_status) == MiAPI_OK)
		{
			_tprintf(_T("GPIO %d : %s(%d) - %s(%d)\n"), pin,
				gpio_status.Direction ? _T("Input(1)") : _T("Output(0)"), gpio_status.Direction,
				gpio_status.VoltageLevel ? _T("High(1)") : _T("Low(0)"), gpio_status.VoltageLevel);
		}
		else
			_tprintf(_T("Fail to get GPIO %d status!\n"), pin);
	}
	else if (_tcscmp(argv[1], _T("-set")) == 0)
	{
		pin = _ttoi(argv[2]);
		gpio_status.Direction = _ttoi(argv[3]);
		gpio_status.VoltageLevel = _ttoi(argv[4]);
		
		if (MiAPI_GPIO_SetStatus(pin, gpio_status) == MiAPI_OK)
		{
			_tprintf(_T("Set GPIO %d as Dir=%d, level=%d"), pin, gpio_status.Direction, gpio_status.VoltageLevel);
		}
		else
			_tprintf(_T("Fail to set GPIO %d !\n"), pin);
	}


	//-- It must free the resource when MiAPI  exits. 	
	MiAPI_Exit();

	return 0;
	
}

