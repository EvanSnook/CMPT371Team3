echo "Setting up environment variables"
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat"

echo "cleaning house"
rd TestResults /s /q
rd HtmlReport /s /q
rd tmp /s /q
mkdir tmp


echo "Copying dependencies";
copy "C:\Program Files\Unity\Editor\Data\Managed\nunit.framework.dll" tmp\ 
copy "C:\Program Files\Unity\Editor\Data\Managed\nunit.core.dll" tmp\
copy "C:\Program Files\Unity\Editor\Data\Managed\nunit.core.interfaces.dll" tmp\
copy "C:\Program Files\Unity\Editor\Data\Managed\Mono.Cecil.dll" tmp\
copy "C:\Program Files\Unity\Editor\Data\Managed\Mono.Cecil.Mdb.dll" tmp\
copy "C:\Program Files\Unity\Editor\Data\Managed\UnityEngine.dll" tmp\
copy "..\Luxsonic Project\Assets\UnityTestTools\UnitTesting\Editor\NSubstitute\NSubstitute.dll" tmp\
copy "C:\Program Files\Unity\Editor\Data\UnityExtensions\Unity\VR\Win64\OVRPlugin.dll" tmp\


echo "Generating DLL";
csc.exe @code.rsp

echo "Generating .coverage file";
vstest.console.exe /UseVsixExtensions:true /Enablecodecoverage /InIsolation tmp\code.dll | wtee tmp\vsoutput.txt

echo "Converting .coverage file";
start C:\Python32\python findAndMoveCoverageFile.py

CoverageConverter\CodeCoverageConverter.exe tmp\raw.coverage tmp\code.dll tmp\converted.coveragexml

ReportGenerator_2.5.5\ReportGenerator.exe -reports:tmp\converted.coveragexml -targetdir:HtmlReport