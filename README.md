Quick POC for dumping bundled drivers in RSRC section. Very quickly put together.
ResourceExtractor base project (Copyright © ruilopes.com 2018) https://github.com/rgl/ResourceExtractor
Was missing some functionality so I added to it.


This lists and extract the resources embedded inside an PE file (`.exe`, `.dll`, `.sys`).

General usage:
```
	C:\TOOLING\ResourceExtractor.exe

	USAGE:

	ResourceExtractor list <executable>
	ResourceExtractor extract <executable> <id> <extract to file name>
	ResourceExtractor dump <executable> <output directory>
	ResourceExtractor finddrivers <directory to recurse>
```

To list the resources use, e.g.:
```
	C:\TOOLING\ResourceExtractor.exe  list accesschk.exe
	BINRES/RCACCESSCHK64/1033       810416
	BINRES/101/1033 16264
	RT_VERSION/1/1033       916
	RT_MANIFEST/1/1033      891
```
The first column is the resource id and the second the resource size.


To extract a specific resource into a file use, e.g.:
```
	C:\TOOLING\ResourceExtractor.exe extract accesschk.exe BINRES/101/1033 binres_101_1033
```

To dump all 
```
```


To find and list imports of PE's:
```
	C:\Users\p4\Downloads\ResourceExtractor-master>C:\Users\p4\Downloads\ResourceExtractor-master\bin\Debug\net461\ResourceExtractor.exe finddrivers OK\
                [+] Found PE file OK\accesschk.exe
	NETAPI32.dll - NetShareEnum - 222 - 708
	NETAPI32.dll - NetShareGetInfo - 224 - 712
	NETAPI32.dll - NetApiBufferFree - 81 - 716
	[...SNIP...]
	                [+] Found PE file OK\OUTDIR\Sysmon\BINRES_1002_1033
	FLTMGR.SYS - FltSetCallbackDataDirty - 233 - 0
	FLTMGR.SYS - FltRegisterFilter - 210 - 4
	FLTMGR.SYS - FltUnregisterFilter - 263 - 8
	FLTMGR.SYS - FltStartFiltering - 253 - 12
	FLTMGR.SYS - FltGetFileNameInformation - 111 - 16
	FLTMGR.SYS - FltReleaseFileNameInformation - 216 - 20
	FLTMGR.SYS - FltParseFileNameInformation - 185 - 24
	FLTMGR.SYS - FltGetVolumeName - 151 - 28
	FLTMGR.SYS - FltCreateFile - 51 - 32
	FLTMGR.SYS - FltReadFile - 206 - 36
	FLTMGR.SYS - FltWriteFile - 266 - 40
	FLTMGR.SYS - FltQueryInformationFile - 199 - 44
	FLTMGR.SYS - FltSetInformationFile - 240 - 48
	FLTMGR.SYS - FltSetSecurityObject - 247 - 52
	FLTMGR.SYS - FltClose - 40 - 56
	FLTMGR.SYS - FltAllocateContext - 11 - 60
	FLTMGR.SYS - FltSetStreamContext - 248 - 64
	FLTMGR.SYS - FltSetStreamHandleContext - 249 - 68
	FLTMGR.SYS - FltDeleteContext - 62 - 72
	FLTMGR.SYS - FltDeleteStreamHandleContext - 68 - 76
	FLTMGR.SYS - FltGetStreamContext - 136 - 80
	FLTMGR.SYS - FltGetStreamHandleContext - 137 - 84
	FLTMGR.SYS - FltReleaseContext - 213 - 88
	FLTMGR.SYS - FltGetVolumeProperties - 152 - 92
	FLTMGR.SYS - FltQueryVolumeInformation - 202 - 96
	HAL.dll - ExReleaseFastMutex - 1 - 104
	HAL.dll - ExAcquireFastMutex - 0 - 108
	ntoskrnl.exe - wcsncpy - 3139 - 116
	ntoskrnl.exe - wcsstr - 3144 - 120
	ntoskrnl.exe - _wcsupr - 3064 - 124
	ntoskrnl.exe - RtlInitUnicodeString - 2230 - 128
	ntoskrnl.exe - RtlCompareUnicodeString - 2049 - 132
	ntoskrnl.exe - RtlCopyUnicodeString - 2070 - 136
	ntoskrnl.exe - RtlAppendUnicodeStringToString - 2018 - 140
```

Misc early dev powershell loop
```powershell
/*
$a=@(gci . -recurse -force -include "*.exe"| select fullname)
Write-host "[+]" $a.Count "Files"

ForEach ($execpath in $a)
{
	try 
	{
		Write-host "[+] Listing resources of" $execpath.Fullname
		C:\Users\p4\Downloads\ResourceExtractor-master\bin\Release\net461\ResourceExtractor.exe dump $execpath.Fullname OUTDIR
	} 
	catch 
	{
		Write-host "[-] Failed for" $execpath.Fullname
	}
}
*/
```

using:
- Vestris.ResourceLib (https://github.com/resourcelib/resourcelib) 
- PeNet (https://github.com/secana/PeNet)