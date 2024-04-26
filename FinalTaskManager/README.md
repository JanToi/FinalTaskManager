Projektia varten tarvitset SQL-relaatiotietokannan (suositeltava Microsoft SQL Server Manager Studio) sekä Visual Studio Coden - Visual Studio suositeltava backendin käynnistämiseksi

1. Kloonaa repositorio itsellesi.
2. Avaa projektikansio Visual Studio Codella.
3. Navigoi "Frontend" kansioon terminaalissa komennolla "cd Frontend".
4. Asenna tarvittavat lisäosat terminaalikomennolla "npm i"
5. Kirjaudu itsesi Server Manager Studioon parametreilla:
Server type: Database Engine
Server name: Koneesi nimi (jos tyhjä eikä näy alapuolella, näet sen command promptista komennolla "whoami")
!!!KOPIOI KONEESI NIMI LEIKEPÖYDÄLLE!!!
-Jos kirjautuminen ei onnistu, avaa lisäasetukset Options napista, ja vaihda Encryption Optionaliksi!-
-Jos palvelimet ovat alhaalla, käynnistä SQL Server Configuration Manager, ja käynnistä palvelimet. Jos et voi käynnistää, paina "Properties", josta löytyy "Services" valikko, josta voit vaihtaa käynnistysvaihtoehdon.
6. Navigoi Backend kansioon, jonka sisältä löytyy Api-kansio.
7. Avaa appsettings.json
8. Vaihda "taskDBCon" Data Source oman koneesi nimeksi, joka on nyt myös tietokantasi palvelimen nimi.
-Esimerkiksi:
"taskDBCon": "Data Source=TIETOKONEESINIMI\\SQLEXPRESS
9. Tallenna appsettings.json
10. Etsi projektin Backend kansiosta tiedosto "LuoSQLtietokanta.sql". Kopioi tiedoston sisältö leikepöydälle.
11. Tee SQL Server Manager Studiossa uusi Query, liitä sisältö sinne, ja juokse query "Execute" napilla.
12. Jos kaikki meni hyvin, nyt on luotu uusi tietokanta tarvittavalla schemalla ja relaatioilla.
13. Backend-kansiossa on Api.sln tiedosto. Avaa kyseinen tiedosto Visual Studiolla (muistathan päivittää Visual Studion uusimpaan versioon!)
14. Käynnistä projekti Visual Studiossa.
15. Mene takaisin Visual Studio Codeen, navigoi Frontend kansioon terminaalissa. 
16. Käynnistä Frontend terminaalikomennolla "npm start"

Jos et käytä Visual Studiota, saat uudessa terminaalissa backendin käynnistettyä terminaalikomennolla "dotnet run" Api-kansiossa.

Projektin tehneet Janne Toivanen, Martin Nipuli, Kasper Kauppi sekä Tatu Ruohoaho.