V t�to slo�ce se nach�z� souborov� p��lohy k bakal��sk� pr�ci:  


                "Vyu��v�n� .NET assembly z ne��zen�ho C++" 
                ==========================================
                 
                     (Vojt�ch Kinkor; Plze�, 2015)


Popis slo�ek:
=============

  CppCliBridgeGenerator/ 
      � Adres�� obsahuj�c� projekt n�stroje pro IDE Microsoft Visual Studio 2010. 

  CppCliBridgeGenerator_Examples/ 
      � Adres�� obsahuj�c� p�ipraven� p��klady pou�iteln� pro otestov�n� funk�nosti n�stroje.
      - V podadres���ch se nach�z� readme soubor k jednotliv�m p��klad�m (anglicky).

  CppCliBridgeGenerator_PerformanceTests/ 
      � Adres�� obsahuj�c� n�stroje v podob� pou�it� pro testov�n� funk�nosti v kapitole 7. 

  CppCliBridgeGenerator_Release/ 
      � Adres�� obsahuj�c� spou�t�c� soubor n�stroje (tj. zkompilovan� projekt z prvn� zm�n�n� slo�ky).

  InteropMethods/ 
      � Adres�� obsahuj�c� p��klady pou��van� v kapitole 3. 



Popis soubor�:
==============

  Kinkor_A12B0082P_BP.pdf 
      � Text bakal��sk� pr�ce ve form�tu PDF.
      - Obsahuje u�ivatelskou p��ru�ku n�stroje (p��loha A, strana 39). 

  readme.txt 
      � Tento soubor.



Dodate�n� informace:
====================

 Ve slo�k�ch s uk�zkami jsou um�st�n� d�vkov� soubory (.bat) slou��c� pro rychl� vyzkou�en� dan� uk�zky.
 Tyto soubory je nutn� spou�t�t ze zapisovateln�ho m�dia (tedy nap��klad z pevn�ho disku).

 Pro vyzkou�en� v�t�iny uk�zek je pot�eba IDE Microsoft Visual Studio 2010 nebo nov�j�� a .NET Framework 4.

 Soubory __vcvars.bat slou�� pro nastaven� v�vojov�ho prost�ed�. V p��pad�, �e se nepoda�� automatick�
 nastaven�, budete upozorn�ni s ��dost� o ru�n� nastaven� cesty k souboru vcvarsall.bat (kter� je sou��st�
 IDE Visual Studio).

 Po spu�t�n� souboru __vcvars.bat v samostatn�m konzolov�m okn� se zp��stupn� n�stroj msbuild, kter� 
 lze pou��t pro kompilaci vygenerovan�ch C++/CLI most�.

