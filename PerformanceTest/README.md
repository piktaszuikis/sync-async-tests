# Apie
Čia yra dvi C# http serverio versijas: vieną parašytą su asp.net ir naudojantį kestrel, kitą naudojantį HttpListener.

## asp.net
Šiai programai reikalingas prisijungimas prie postgresql DB. Prisijungimui naudojama *UNIX socket* ir numatytasis portas bei vartotojas 'postgres' be slaptažodžio.

API:
	* / - tas pats kaip ir */sync/get*.
	* sync/get, async/get - gražina "OK".
	* sync/primes?num=?, async/primes?num=? - suranda ir gržina *num* ilgio pirminių skaičių JSON sąrašą.
	* sync/db-fast?num=?, async/db-fast?num=? - kreipiasi į DB ir gražina  *num* ilgio pirminių skaičių JSON sąrašą.
	* sync/db-slow?num=?, async/db-slow?num=? - duomenų bazėje įvykdo lėtą užklausą `SELECT pg_sleep(3)` ir gražina  *num* ilgio pirminių skaičių JSON sąrašą.

## HttpListener
Yra vienintelis API - `/`, kuris gražina "OK" ir viskas.

# Kompiliavimas
Sukompiliuoti .net5 versiją, bus reikalingas openssl-1.1 paketas. Kompiliavimui taip pat reikalingas bent aštuntos versijos dotnet SDK.

Kompiliavimui galima panaudoti `./build.sh` skriptą, kuris sukompiliuos abu projektus šioms dotnet versijoms:
* .net5
* .net6
* .net7
* .net8

# Paleidimas
Paleidimui galima naudoti `run-all-asp.sh` ir `run-all-http-listeners.sh`, kurie paleis projektus visoms .net versojoms.

