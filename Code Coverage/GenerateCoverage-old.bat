echo "Setting up environment variables"
call VsDevCmd.bat

echo "cleaning house"
rd TestResults /s /q
rd HtmlReport /s /q
rd tmp /s /q
mkdir tmp


"..\Luxsonic Project\packages\NUnitTestAdapter.2.0.0\lib\"

echo "Copying dependencies";
copy "..\Luxsonic Project\packages\packages\NUnit.3.6.0\lib\net35\nunit.framework.dll" tmp\ 
copy "..\Luxsonic Project\packages\NUnitTestAdapter.2.0.0\lib\nunit.core.dll" tmp\
copy "..\Luxsonic Project\packages\NUnitTestAdapter.2.0.0\lib\nunit.core.interfaces.dll" tmp\
copy "..\Luxsonic Project\packages\NUnitTestAdapter.2.0.0\lib\nunit.util.dll" tmp\
copy "..\Luxsonic Project\packages\NUnitTestAdapter.2.0.0\lib\NUnit.VisualStudio.TestAdapter.dll" tmp\
copy "C:\Path\To\UnityEngine.dll" tmp\
copy "C:\Path\To\PlayMaker\PlayMaker.dll" tmp\
copy "C:\Path\To\HOTween.dll" tmp\
copy "..\Luxsonic Project\Assets\UnityTestTools\UnitTesting\Editor\NSubstitute\NSubstitute.dll" tmp\

echo "Generating DLL";
csc.exe @full.rsp

echo "Generating .coverage file";
vstest.console.exe /UseVsixExtensions:true /Enablecodecoverage /InIsolation tmp\full.dll | wtee tmp\vsoutput.txt

echo "Converting .coverage file";
findAndMoveCoverageFile.py

CoverageConverter\CodeCoverageConverter.exe tmp\raw.coverage tmp\full.dll tmp\converted.coveragexml

ReportGen\ReportGenerator.exe -reports:tmp\converted.coveragexml -targetdir:HtmlReport