Quick POC for dumping bundled drivers in RSRC section. Very quickly put together.

This lists and extract the resources embedded inside an PE file (`.exe`, `.dll`, `.sys`).

General usage:

	C:\TOOLING\ResourceExtractor.exe

	USAGE:

	ResourceExtractor list <executable>
	ResourceExtractor extract <executable> <id> <extract to file name>
	ResourceExtractor dump <executable> <output directory>
	ResourceExtractor finddrivers <directory to recurse>



To list the resources use, e.g.:

	C:\TOOLING\ResourceExtractor.exe  list accesschk.exe
	BINRES/RCACCESSCHK64/1033       810416
	BINRES/101/1033 16264
	RT_VERSION/1/1033       916
	RT_MANIFEST/1/1033      891

The first column is the resource id and the second the resource size.



To extract a specific resource into a file use, e.g.:

	C:\TOOLING\ResourceExtractor.exe extract accesschk.exe BINRES/101/1033 binres_101_1033


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



using Vestris.ResourceLib and PeNet