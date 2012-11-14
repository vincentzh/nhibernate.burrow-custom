del result.txt
path %SystemRoot%\Microsoft.NET\Framework\v4.0.30319
MSBuild src/NHibernate.Burrow.sln /t:Build /p:RunCodeAnalysis=true;Configuration=Release;outdir=../../OutputDll/
PAUSE