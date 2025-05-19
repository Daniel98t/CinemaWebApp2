using CinemaWebApp.Models;

namespace CinemaWebApp.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(CinemaContext context)
        {
            // Kontrollera att filmer finns, annars lägg till några
            if (!context.Films.Any())
            {
                context.Films.AddRange(
                    new Film { Title = "Titanic", Genre = "Drama", Description = "Titanics undergång 1912 är en av 1900-talets mest omskrivna katastrofer. Titantic imponerar både genom den fantastiska tekniken och kärlekshistorien mellan överklasstjejen från första klass och konstnären från tredje klass.", Length = 120, Price = 99.99M },
                    new Film { Title = "Scream", Genre = "Skräck", Description = "En psykopatisk mördare ställer in siktet på ett föredetta offers dotter, medan en TV-reporter kommer närmare mördarens identitet.", Length = 150, Price = 129.99M },
                    new Film { Title = "Harry Potter", Genre = "Fantasi", Description = "Harry Potter, en föräldralös pojke med magiska talanger, börjar på internatet Hogwarts, en skola för häxkonster och trolldom. Harry lär känna Hermione och Ron och listar ut mysteriet kring sina föräldrars dödsfall.", Length = 200, Price = 129.99M },
                    new Film { Title = "LasseMajas detektivbyrå - Maskoten som försvann", Genre = "Familj", Description = "En ny film i den populära svenska barnfilmsserien, där Lasse och Maja löser mysterier i Valleby. ", Length = 150, Price = 129.99M },
                    new Film { Title = "Wicked", Genre = "Musikal", Description = "Wicked, även känd som Wicked: Part One, är en musikalisk fantasyfilm från 2024. Filmen är regisserad av Jon M. Chu och för manus har Winnie Holzman, Gregory Maguire och Stephen Schwartz svarat. Filmen är en filmatisering av musikalföreställningen Wicked. ", Length = 161, Price = 129.99M },
                    new Film { Title = "Heretic", Genre = "Skräck", Description = "Filmens handling kretsar kring två mormonmissionärer, som tillsammans försöker omvända en man, vilket visar sig vara ännu farligare än vad de hade trott.", Length = 110, Price = 129.99M },
                    new Film { Title = "Gladiator 2", Genre = "Action", Description = "Ungefär 20 år har passerat sedan gladiatorn Maximus Decimus Meridius fick sätta livet till på Colosseum. Lucius, son till Lucilla och Maximus, lever tillsammans med sin familj i den romerska provinsen Numudien. Romerska soldater ledda av general Marcus Acacius intar staden och dödar Lucius fru och tvingar honom till slaveri. Inspirerad av historien om Maximus antar Lucius ett liv som gladiator.", Length = 148, Price = 129.99M },
                    new Film { Title = "Den vilda roboten", Genre = "Familj", Description = "fter ett skeppsbrott blir en intelligent robot vid namn Roz strandsatt på en obebodd ö. För att överleva den svåra miljön ansluter sig Roz till öns djur och tar hand om en föräldralös gåsunge.", Length = 102, Price = 129.99M },
                    new Film { Title = "Red One", Genre = "Action/Komedi", Description = "Efter att jultomten (kodnamn: Red One) har kidnappats måste Nordpolens säkerhetschef slå sig ihop med världens mest ökända prisjägare i ett försök att hitta tomten och rädda julen.", Length = 123, Price = 129.99M }
                );
                context.SaveChanges();
            }

            // Kontrollera att salonger finns, annars lägg till några
            if (!context.Salonger.Any())
            {
                context.Salonger.AddRange(
                    new Salong { Number = 1, Seats = 50 },
                    new Salong { Number = 2, Seats = 100 },
                    new Salong { Number = 3, Seats = 150 }
                );
                context.SaveChanges();
            }

            // Kontrollera att föreställningar finns, annars lägg till några
            if (!context.Föreställningar.Any())
            {
                var salonger = context.Salonger.ToList();
                var filmer = context.Films.ToList();

                // Kontrollera att salonger och filmer finns innan föreställningar skapas
                var titanicFilm = filmer.FirstOrDefault(f => f.Title == "Titanic");
                var screamFilm = filmer.FirstOrDefault(f => f.Title == "Scream");
                var harryPotterFilm = filmer.FirstOrDefault(f => f.Title == "Harry Potter");
                var lasseMajaFilm = filmer.FirstOrDefault(f => f.Title == "LasseMajas detektivbyrå - Maskoten som försvann");
                var wickedFilm = filmer.FirstOrDefault(f => f.Title == "Wicked");
                var hereticFilm = filmer.FirstOrDefault(f => f.Title == "Heretic");
                var gladiatorFilm = filmer.FirstOrDefault(f => f.Title == "Gladiator 2");
                var robotFilm = filmer.FirstOrDefault(f => f.Title == "Den vilda roboten");
                var redOneFilm = filmer.FirstOrDefault(f => f.Title == "Red One");

                if (salonger.Count < 3 || filmer.Count < 9)
                {
                    throw new InvalidOperationException("Det saknas nödvändiga filmer eller salonger för att skapa föreställningar.");
                }

                context.Föreställningar.AddRange(
                    new Föreställning { Film = titanicFilm, Salong = salonger.First(s => s.Number == 1), Time = DateTime.Now.AddDays(1).AddHours(15) },
                    new Föreställning { Film = titanicFilm, Salong = salonger.First(s => s.Number == 2), Time = DateTime.Now.AddDays(2).AddHours(18) },
                    new Föreställning { Film = screamFilm, Salong = salonger.First(s => s.Number == 3), Time = DateTime.Now.AddDays(1).AddHours(20) },
                    new Föreställning { Film = screamFilm, Salong = salonger.First(s => s.Number == 1), Time = DateTime.Now.AddDays(3).AddHours(22) },
                    new Föreställning { Film = harryPotterFilm, Salong = salonger.First(s => s.Number == 2), Time = DateTime.Now.AddDays(2).AddHours(14) },
                    new Föreställning { Film = harryPotterFilm, Salong = salonger.First(s => s.Number == 3), Time = DateTime.Now.AddDays(4).AddHours(19) },
                    new Föreställning { Film = lasseMajaFilm, Salong = salonger.First(s => s.Number == 1), Time = DateTime.Now.AddDays(3).AddHours(13) },
                    new Föreställning { Film = wickedFilm, Salong = salonger.First(s => s.Number == 2), Time = DateTime.Now.AddDays(5).AddHours(16) },
                    new Föreställning { Film = hereticFilm, Salong = salonger.First(s => s.Number == 3), Time = DateTime.Now.AddDays(1).AddHours(21) },
                    new Föreställning { Film = gladiatorFilm, Salong = salonger.First(s => s.Number == 1), Time = DateTime.Now.AddDays(2).AddHours(20) },
                    new Föreställning { Film = robotFilm, Salong = salonger.First(s => s.Number == 2), Time = DateTime.Now.AddDays(3).AddHours(12) },
                    new Föreställning { Film = redOneFilm, Salong = salonger.First(s => s.Number == 3), Time = DateTime.Now.AddDays(4).AddHours(17) }
                );

                context.SaveChanges();
            }
        }
    }
}
