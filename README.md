**Feladat: Anonim szavazó rendszer**

Készítsünk egy online anonim szavazó rendszert. A kliens-szerver architektúrájú webalkalmazáson keresztül a rendszer felhasználóinak (vagy egy részüknek) kérhetjük ki a véleményét a kérdés és a válasz opciók megadásával. A szavazások minden esetben titkosak legyenek, amelyet a programnak garantálnia kell (külön tárolandó a szavazatukat gyakorló felhasználók és a leadott szavazat).

**Általános követelmények**

A feladat részeként egy REST architektúrát követő WebAPI webszolgáltatást kell megvalósítani ASP.NET keretrendszerben, C# programozási nyelven. A webszolgáltatáshoz egy látogatói és egy adminisztrációs kliens alkalmazást is fejleszteni kell.

• A WebAPI szolgáltatást MVC architektúrának megfelelően kell elkészíteni.

• A látogatói kliens alkalmazás szabadon választható technológiával, webes alkalmazásként, asztali alkalmazásként vagy mobil alkalmazásként is elkészíthető. Az alkalmazást a választott platformnak megfelelően, legalább kétrétegű architketúrában kell elkészíteni.

• Az adminisztrációs klienst a Blazor szoftverkönyvtár használatával, MV(VM) architektúrában kell elkészíteni legalább 1 támogatott platformra.

• A látogatói és az adminisztrációs alkalmazást együttesen is be kell tudni mutatni, a két alkalmazásnak funkcionalitásban egymást kiegészítve kell együtt működőképesnek lennie. A feladatkiírás alapfeladatból (2+1 pont) és opcionális választható részfeladatokból áll. A beadandó értékelése az elfogadott funkcionalitások összpontszámának egész része. Jeles (5+1) érdemjegy teljesítéséhez kötelező legalább egy SignalR alapú funkció megvalósítása.

**Alapfeladat (2+1 pont)**

Látogatói felület: készítsünk REST architektúrájú webalkalmazást és hozzá webes felületű, asztali grafikus vagy mobil kliens alkalmazást, amelyen keresztül a felhasználók az aktív kérdésekben szavazhatnak, valamint megtekinthetőek a korábbi szavazások eredményei.

• A felhasználók email cím és jelszó megadásával regisztrálhatnak, valamint jelentkezhetnek be. A portál további funkciói csak bejelentkezést követően érhetőek el.

• Bejelentkezést követő megjelenő felületen látható az aktív szavazások listája. Aktív az a szavazás, amely már elkezdődött, de még nem fejeződött be. A szavazásokat a befejező dátumuk szerint növekvő sorrendben kell listázni a kérdés szövegének, valamint a kezdő és befejező időpontnak a feltüntetésével. Vizuálisan legyen egyértelműen jelölve, hogy egy aktív szavazáson már szavazott-e a felhasználó vagy sem.

• Egy aktív szavazás kiválasztásával az alkalmazás jelenítse meg a kérdést és a válasz opciókat. Utóbbiak közül pontosan egyet kiválasztva lehet a szavazatot érvényesen leadni.

• A bejelentkezett felhasználók egy másik felületen kilistázhatják a már lezárult szavazásokat. Lezárultnak tekintendő az a szavazás, amelynek befejező időpontja elmúlt. A szavazások listáját lehessen szűrni a kérdés szövegének részlete vagy időintervallum alapján.

• Egy lezárult szavazás kiválasztásával a weboldal jelenítse meg annak eredményét, azaz válasz opciónkként a szavazatok száma és százalékos értéke.
Adminisztrációs felület: készítsünk Blazor keretrendszerre épülő kliens alkalmazást, amelyen keresztül új szavazásokat lehet kiírni a rendszerben. Bármely felhasználó írhat ki új szavazást.

• A felhasználók bejelentkezhetnek (email cím és jelszó megadásával) a programba. Sikeres bejelentkezést követően látja az általa kiírt korábbi szavazások listáját.

• Egy szavazást kiválasztva megjelenítésre kerül a feltett kérdés és a válasz opciók, valamint a szavazás kezdete és vége. Továbbá kerüljön megjelenítésre a szavazáshoz rendelt felhasználók listája, jelölve, hogy mely felhasználók szavaztak már és melyek nem.

• Legyen lehetőség új szavazás kiírására a kérdés, a dinamikus számú (legalább 2) válasz opció, továbbá a kezdő és a befejező időpont megadásával. A kezdő és a vég időpont létező kell legyen, továbbá mindkettőnek jövőbelinek kell lennie és a vég időpontnak legalább 15 perccel követnie kell a kezdőidőpontot.

• A szavazás a kiírása után már nem módosítható.
