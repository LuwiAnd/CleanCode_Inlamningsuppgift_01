# Rapport inlämningsuppgift 1

## Problem med den ursprungliga koden

### Problem 1 - Controllers skapar nya instanser av service:ar

De controllers som ingår i originalkoden skapar egna service:ar. Detta bryter mot Dependency Inversion Principle (DIP), då högnivåobjekt (controllers) är beronde av lågnivåobjekt (service-klasser). Till exempel i ProductController:

```csharp
private readonly ProductService _service = new ProductService();
```

Detta löste jag genom Dependency Injection (DI) i Program.cs 

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

och genom att byta ut koden ovan mot att i controllern skriva:

```csharp
private readonly IProductService _service;

public ProductController(IProductService service)
{
    _service = service;
}
```

I Program.cs har jag även lagt till repositories som singleton. I vanliga fall brukar man lägga till dem som Scoped för att de ska fungera med EF Core, men i detta projekt har jag ingen riktig databas och data ska sparas i minnet och ligga kvar där under hela programmets körning, så då är AddSingleton att föredra.

```csharp
builder.Services.AddSingleton<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
```

### Problem 2 - Service:ar hade eget datalager via statiska listor

Exempel på problemet:
```csharp
private static readonly List<Product> Products = new List<Product>();
```

Detta bryter mot SRP genom att service:arna har hand om både affärslogik och databashantering. Dessutom skapar det hårda kopplingar mellan service:arna och hur data sparas, vilket kräver uppdateringar av service:ar när databasen ändras, vilket strider mot Open/Closed Principle (OCP) så att det kräver mycket jobb att försöka byta ut databasen. Att skapa hårda kopplingar kallas Tight coupling anti pattern.

Lösningen var att införa repository pattern för samtliga service:ar. Nu är service:arna kopplade till respektive repository via ett interface, vilket skapar lösa kopplingar och därmed är enkelt att underhålla om man vill byta till att använda en riktig databas i framtiden.

### Problem 3 - avsaknad av DTO:er

Det fanns inga DTO:er i den ursprungliga koden, vilket är en säkerhetsrisk då hackare får information om hur våra databasobjekt ser ut och ju mer information de har dessto större risk att de kan utnyttja den för att komma åt våra data. Detta kallas för Encapsulation anti pattern.

Lösningen var helt enkelt att införa DTO:er och se till att returnera dem istället för entities.

### Problem 4 - CartService saknade en riktig datamodell

CartService hade 
```csharp
private static readonly Dictionary<int, List<CartItem>> Carts = 
    new Dictionary<int, List<CartItem>>();
```
Data för olika kundvagnar är utspridda i en lista med alla kundvagnars items. Detta är Encapsulation anti pattern.

Lösning:
Dels flyttades denna data till repository:t, men jag skapade även Cart-klassen för att ha hand om alla CartItems för en användare på ett ställe.

### Problem 5 - För stora klasser God objects

Från början fanns bara åtta filer, som var för sig gjorde alldeles för många saker för en enskild klass. Nackdelen är att det är svårt att sätta sig in i så stora klasser och det är svårt att bygga mockar, stubar och fakes för testning av dem. Vidare är det svårt att arbeta i team i så stora filer, eftersom man då oftare arbetar i samma fil och riskerar merge-konflikter.
Detta bryter mot SRP och OCP, för att varje klass har många ansvar respektive ändringar i en del av koden kan påverka många andra delar.

Lösning:
Dessa filer har var och en brutits upp i flera klasser och interface för att följa SRP.


### Problem 6 - Det fanns List<object>

Det fanns flera ställen i koden som använde List<object>, vilket är en väldigt abstrakt datatyp som är väldigt svårt att basera några användbara test på och som säkerligen kan leda till problem när nästa utvecklare väljer att skicka med ett helt orelaterat objekt till det man tänkt använda det till.
Detta bryter mot Liskov Substitution Principle (LSP) när listan kan innehålla vad som helst.

Lösning:
På alla ställen där jag stötte på List<object> bytte jag ut det mot listor av en mer specifik objekttyp, som till exempel List<OrderItem>.


### Problem 7 - Controllers returnerar anonyma objekt

Att returnera anonyma objekt gör koden mindre testbar och gör att de som anropar api:et inte vet vad de ska förvänta sig i retur och hur de ska hantera det. 
Detta bryter mot OCP, eftersom anonyma klasser inte går att återanvända eller utöka.

Lösning: 
Gör DTO-klasser som kan returneras istället för anonyma klasser. Då kan man dessutom återanvända funktioner för att hantera dem/data ur dem om man förväntar sig en viss DTO i retur vid olika anrop och man glömmer inte att uppdatera koden för att hantera dem om funktionerna ger felmeddelanden om att data som skickas in längre är den DTO som de förväntar sig.

### Problem 8 - Entities-klasser inne i andra klasser

I många klasser definieras andra klasser, vilket gör koden svåröverskådlig. 
Detta bryter mot SRP för att klassen både hanterar logik och datamodeller. Det är även Encapsulation anti pattern att gömma klasser på fel ställe.

Lösning:
Jag har flyttat ut alla klasser som definieras inuti andra klasser till egna filer och grupperat dessa i mappar, så att de är lätta att komma åt och återanvändas. Skulle den ursprunliga klassen, som tidigare innehöll andra klasser, tas bort, så kan man fortsätta att använda de utflyttade klasserna om man behöver.


### Problem 9 - Dålig namngivning

Många namn i den ursprungliga koden var väldigt korta eller intetsägande, som exempelvis "p", "o" eller "existing", där "existing" låter som en bool men det var ett eventuellt redan existerande CartItem. Flera metodnamn var även de kort eller intetsägande. Detta bryter mot principen om Clean Code genom självförklarande kod. Namnen ska vara meningsfulla för att öka läsbarheten.

Lösning:
Jag har bytt ut många metodnamn mot längre, mer förklarande namn, som exempelvis OrderService.Get har bytts ut till OrderService.GetOrderById för att det ska vara lättare att förstå att det är **en order** som ska hämtas mha **ett orderId** till skillnad mot metoden med namnet OrderService.GetForUser som nu har fått namnet OrderService.GetOrdersForUser och som istället returnerar en lista av **flera ordrar** mha **ett userId**.


### Problem 10 - Långa funktioner

Vissa metoder är ganska långa och därför svåra att överblicka. Detta bryter mot SRP.

Lösning:
Jag har för vissa långa funktioner brutit ut en del av koden och lagt i nya funktioner för att de ska vara enklare att läsa och testa. Jag tror dock att det finns flera långa funktioner kvar som jag inte hunnit bryta isär till mindre.

### Problem 11 - Upprepad kod

Många funktioner gör samma sak som varandra, vilket bryter mot "Don't repeat yourself" (DRY). 

Lösning:
Man kan bryta ut kod som upprepas i flera funktioner och lägga i en hjälpfunktion. Jag tror dock inte att jag hunnit göra detta på så många ställen.

### Problem 12 - Dålig kodformatering

Jag har delat in långa metodanrop i flera rader med en parameter per rad för att det ska bli mer lättläst och för att man snabbt ska kunna se alla parametrar som används.

Jag har ökat antalet tomma kodrader på vissa ställen för att öka läsbarheten. I funktioner har jag gärna en tom rad innan return till exempel, men jag grupperar gärna kodrader som hör ihop för att man snabbt ska se att nästa kluster av kod hanterar något annat.

En del i konceptet clean code är att koden ska vara lättläst. Även om man följt SOLID-principerna till punkt och pricka, så blir koden svårläst om man inte har radbrytningar och indenteringar där det behövs. Därför bröt formateringen av koden mot clean code innan jag uppdaterade den.

## Problem 13 - Dåligt namngivna routes

Jag tror att jag behållit alla api-routes med samma namn, även om jag tycker att "me" inte är ett bra namn på användarens egen kundvagn. Jag hade hellre döpt den till "my-cart" eller liknande, eftersom "me" låter som att användaren vill hämta sin profil.

## Reflektion

### Vad blev bättre

Efter ovanstående uppdateringar har min kod blivit mer modulär, vilket gör den mer
 - förvaltningsbar
 - enklare att testa
 - skalbar
 - testbar

 Den är också mer typsäker efter att jag bytt ut variabler av typen List<object> till något mer specifikt.

### Vad kan göras framöver?

### Onödig funktion
Jag märkte i efterhand att jag i OrderService råkat skapa både 

```csharp
private decimal CalculateOrderTotalFromItems(List<OrderItem> items)
```
och 
```csharp
public decimal CalculateOrderTotal(Order order)
    => CalculateOrderTotalFromItems(order.Items);
```
som använder den, när det hade räckt med att bara skapa den förstnämnda. Nu har jag skapat test för den sistnämnda som jag inte orkar/hinner ta bort nu, så jag har låtit det vara kvar.




### Unit of Work

Jag skulle kunna lägga till unit of work för att undvika att min uppdateringsfunktion skulle kunna råka ta bort ett objekt utan att lägga till den uppdaterade versionen av det. Om servern skulle krascha precis under en sådan uppdatering, mellan att den tar bort den gamla versionen av objektet och att den lägger till den nya, så skulle det kunna leda till att det försvinner ur databasen.

### Implementera en färdig lösning för autentisering

Appen implementerar i sin nuvarande form ett eget sätt att skapa och hantera tokens för autentisering. Detta kan leda till att andra utvecklare kommer att behöva lägga mer tid på att kontrollera att det fungerar och hur det fungerar än om jag hade bytt ut lösningen mot en känd lösning man kan ladda ned som ett paket och som andra utvecklare med stor sannolikhet redan använt och litar på, till exempel JWT.

### Skapa Mappingsprogram för att konvertera till DTO:er

Jag har lagt metoder som konverterar från Entities till DTO:er direkt i service-programmen, eftersom jag bara gjorde någon enstaka sådan funktion till att börja med. Nu inser jag dock att det vore bättre att skapa en mapp som heter "Mappings" eller liknande och lägga program enbart för detta i den mappen, så att det är lätt att hitta dem och programmen blir ytterligare lite mer modulära om man till exempel skulle vilja byta ut ett helt service-program i framtiden. 










## Testplan

Min testplan går ut på att testa den viktigaste affärslogiken, alltså att de viktigaste delarna i de services jag gjort fungerar.

### ProductService

1. CreateProduct skapar produkt korrekt
2. CreateProduct kastar ArgumentException vid ogiltiga värden.
3. Search returnerar rätt produkter.
4. ChangeProductStock uppdaterar antal i lager.

### CartService

1. AddToCart skapar ny cart om ingen finns (med mock)
2. AddToCart ökar quantity om produkten redan finns
3. RemoveFromCart tar bort rätt produkt
4. ClearCart tar bort hela kundvagnen (med mock)

### OrderService

1. CalculateOrderTotal räknar korrekt summa
2. CreateOrderFromCart skapar order korrekt (stubbar ut produktpriser)
3. CreateOrderFromCart returnerar null vid tom cart

### UserService

1. Login returnerar token vid korrekt lösenord
2. Login returnerar null vid fel lösenord
    1. Med Fake
    2. Med Stub
3. Register returnerar false om användarnamnet redan finns
4. GetUserByToken returnerar rätt användare
5. GetUserByToken returnerar null vid okänt token
