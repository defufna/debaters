using System.Security.Cryptography;
using System.Text.Json;
using System.IO;
using VeloxDB.Client;
using Debaters.API;

string connString = "address=localhost:7568";
var api = ConnectionFactory.Get<IDebateAPI>(connString);
var userApi = ConnectionFactory.Get<IUserAPI>(connString);
MD5 md5 = MD5.Create();

string[] adjectives = new string[]
{
    "Bold",
    "Vibrant",
    "Genuine",
    "Radiant",
    "Whimsical",
    "Noble",
    "Enchanting",
    "Brilliant",
    "Elegant",
    "Adventurous",
    "Gracious",
    "Harmonious",
    "Exuberant",
    "Daring",
    "Lively",
    "Spirited",
    "Gentle",
    "Charming",
    "Serene",
    "Versatile",
    "Vivid",
    "Energetic",
    "Inspiring",
    "Clever",
    "Graceful",
    "Majestic",
    "Playful",
    "Harmonious",
    "Joyful",
    "Peaceful",
    "Rhythmic",
    "Whimsical",
    "Wonderous",
    "Sunny",
    "Spirited",
    "Zestful",
    "Zealous",
    "Calm",
    "Cozy",
    "Delightful",
    "Adorable",
    "Flourishing",
    "Jovial",
    "Blissful",
    "Soothing",
    "Vivacious",
    "Galactic",
    "Jedi",
    "Sith",
    "Rebel",
    "Imperial",
    "Wookiee",
    "Droid",
    "Skywalker",
    "Forceful",
    "Vader",
    "Falcon",
    "Mandalorian",
    "Ewok",
    "XWing",
    "Stormtrooper",
    "Lightsaber",
    "Yoda",
    "Rogue",
    "Empire",
    "Clone",
    "Tatooine",
    "Coruscant",
    "Freaky",
    "Crazy",
    "Zany",
    "Weird",
    "Bizarre",
    "Frenetic",
    "Nutty",
    "Outlandish",
    "Unconventional",
    "Whacky",
    "Kooky",
    "Eccentric",
    "Quirky",
    "Insane",
    "Goofy",
    "Hysterical",
    "Futuristic",
    "Bender",
    "Zoidberg",
    "Nibbler",
    "Leela",
    "Fry",
    "Robot",
    "Spacey",
    "Hermes",
    "Amy",
    "Zapp",
    "Kif",
    "Simpson",
    "Bart",
    "Homer",
    "Marge",
    "Lisa",
    "Maggie",
    "Springfield",
    "Ned",
    "Krusty",
    "Apu",
    "Milhouse",
    "Montgomery",
    "Cletus",
    "Chief",
    "Moe",
    "Nelson",
    "Arrakis",
    "Atreides",
    "Harkonnen",
    "Fremen",
    "Bene",
    "Gesserit",
    "Sandworm",
    "Melange",
    "ShaiHulud",
    "Kwisatz",
    "Haderach",
    "Liberal",
    "Just",
    "Free",
    "Equality",
    "Republican",
    "Democratic",
    "Patriotic",
    "Constitutional",
    "Federal",
    "Civil",
    "Anarchy",
    "Punk",
    "Infected",
    "Sorrow",
    "Generated",
    "Chaothic",
    "Protest",
    "Stranger",
    "Atheist",
    "Infidelic",
    "Sane",
    "Honest",
    "Unholy",
    "Suffer",
    "Requiem"
};

string[] nouns = new string[]
{
    // Generic
    "Glimmer",
    "Journey",
    "Sparkle",
    "Whisper",
    "Champion",
    "Voyager",
    "Legend",
    "Gaze",
    "Talisman",
    "Charm",
    "Cherub",
    "Petal",
    "Cascade",
    "Harmony",
    "Beacon",
    "Oracle",
    "Paragon",
    "Gleam",
    "Quest",
    "Enigma",
    "Marvel",
    "Cascade",
    "Rhapsody",
    "Symphony",
    "Glimpse",
    "Chronicle",
    "Labyrinth",
    "Mystique",
    "Thrive",
    "Whisper",
    "Fable",
    "Serenade",
    "Eclipse",
    "Spectacle",
    "Realm",
    "Odyssey",
    "Essence",
    "Twilight",
    "Memento",
    "Crescent",
    "Reverie",
    "Tide",
    "Zephyr",
    "Eon",
    "Luminance",
    "Talisman",
    "Vortex",
    "Galaxy",
    "Haven",
    "Sage",
    "Harmony",
    "Solstice",
    "Riddle",

    // Star Wars
    "Falcon",
    "Jedi",
    "Saber",
    "Wookiee",
    "Empire",
    "Rebellion",
    "Cruiser",
    "Alderaan",
    "Nebula",
    "Force",
    "Cantina",
    "Droid",
    "Kyber",
    "Aurebesh",
    "Theed",
    "Gungan",
    "Jawa",
    "Ewok",
    "Hoth",
    "Dagobah",
    "Chewbacca",
    "Tatooine",
    "Palpatine",
    "Padme",
    "Jango",
    "Sidious",
    "Maul",
    "Kenobi",
    "Grievous",
    "Poe",
    "Finn",
    "Rey",
    "Han",
    "Leia",
    "Anakin",
    "Luke",

    // Freakazoid
    "Frenzy",
    "Madness",
    "Havoc",
    "Chaos",
    "Nonsense",
    "Lunacy",
    "Mayhem",
    "Zaniness",
    "Absurdity",
    "Ruckus",
    "Wackiness",
    "Jumble",
    "Mirth",
    "Craziness",
    "Riot",
    "Foolery",

    // Futurama
    "Nibbler",
    "Robot",
    "Planet",
    "Squishy",
    "Bender",
    "Leela",
    "Fry",
    "Zoidberg",
    "Professor",
    "Kif",
    "Nixon",
    "Mom",
    "Lrrr",
    "Kang",
    "Nibblonian",
    "Slurm",
    "Hypnotoad",
    "Nudar",
    "Hermes",
    "Zapp",
    "Zappman",
    "Doop",
    "Hedonism",
    "Nixonbuck",
    "Mombil",
    "Kifster",
    "Donut",
    "Apu",
    "Nelson",
    "Duff",
    "Krusty",
    "Abe",
    "Moe",
    "Maggie",
    "Jebediah",
    "Krusty",
    "Kwik",
    "Bartman",
    "Flanders",
    "Barney",
    "Itchy",
    "Scratchy",
    "Marge",
    "Selma",
    "Duffman",
    "Neddy",
    "Cletus",
    "Lenny",
    "Carl",
    "Apu",
    "Wiggum",

    // Dune
    "Arrakis",
    "Shield",
    "Kwisatz",
    "Haderach",
    "Gom",
    "Jabar",
    "Bene",
    "Gesserit",
    "Paul",
    "Atreides",
    "Leto",
    "Harkonnen",
    "Chani",
    "ShaiHulud",
    "MuadDib",
    "Melange",
    "Thufir",
    "Hawat",
    "Stilgar",
    "Harah",
    "Gurney",
    "Halleck",

    "Leslie",
    "Ron",
    "Ann",
    "Tom",
    "Andy",
    "April",
    "Ben",
    "Chris",
    "Jerry",
    "Donna",
    "Garry",
    "JeanRalphio",
    "Perd",
    "Tammy",
    "Orin",
    "Bobby",
    "Marcia",
    "MonaLisa",
    "Macklin",

    // 30 Rock
    "Liz",
    "Jack",
    "Tracy",
    "Jenna",
    "Kenneth",
    "Pete",
    "Frank",
    "Cerie",
    "Jonathan",
    "Hazel",
    "Dennis",
    "Dotcom",
    "Grizz",
    "Toofer",
    "Elisa",
    "Avery",
    "Devon",
    "Paul",
    "Lenny",
    "Connie",

    // Terminator
    "John",
    "Sarah",
    "Kyle",
    "Derek",
    "Cameron",
    "Marcus",
    "Miles",
    "Catherine",
    "T800",
    "T1000",
    "TX",
    "Connor",
    "Carl",
    "Grace",
    "Dani",
    "Savannah",
    "Martin",
    "Cromartie",
    "Rosie",
    "Riley"


};

string[] comments = JsonSerializer.Deserialize<string[]>(File.ReadAllText("./comments.json"));

Random random = new Random();

string RandomUsername() => $"{adjectives[random.Next(adjectives.Length)]}{nouns[random.Next(nouns.Length)]}";
string GeneratePass(string username, string salt) => Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(username+salt)));

List<string> RegisterUsers(int count, string salt)
{
    List<string> usernames = new(count);
    DatabaseTask<RegisterResult>[] tasks = new DatabaseTask<RegisterResult>[count];
    for(int i = 0; i < count; i++)
    {
        string username = RandomUsername();
        string password = GeneratePass(username, salt);
        usernames.Add(username);
        tasks[i] = userApi.Register(username, password, "test@example.com");
    }

    int exceptions = 0;
    List<string> result = new(count);
    for(int i = 0; i < count; i++)
    {
        var task = tasks[i];
        try
        {
            if(task.GetResult() == RegisterResult.Success)
            {
                result.Add(usernames[i]);
            }else if(task.GetResult() == RegisterResult.InvalidUsername)
            {
                Console.WriteLine(usernames[i]);
            }
        }
        catch(VeloxDB.Protocol.DbAPIMismatchException e)
        {            
            exceptions++;
        }
    }
    Console.WriteLine(exceptions);
    return result;
}

List<string> Login(List<string> users)
{
    List<DatabaseTask<string?>> tasks = new(users.Count);
    foreach(string user in users)
    {
        tasks.Add(userApi.Login(user, GeneratePass(user, "mySalt")));
    }

    return new List<string>(tasks.Select(t=>t.GetResult()).Where(sid=>sid!=null));
}

string Pick(IReadOnlyList<string> list) => list[random.Next(list.Count)];

void CreatePosts(List<string> sids, int count)
{
    
}

public async Task<SubmitCommentResultDTO> CreateComment(long parent)
{
    var sid = Pick(sids);
    var content = Pick(comments);
    return await api.SubmitComment(sid, parent, content);
}

public async Task<bool> CreatePost(int commentNum)
{
    var sid = Pick(sids);
    var text = Pick(comments);
    var content = Pick(comments);
    var result = await api.SubmitPost(sid, "Test", text, content);

    if(result.Code != ResultCode.Success)
        return false;

    Stack<long> parents = new();
    parents.Push(result.Id);

    for(int i = 0; i < commentNum; i++)
    {
        var comment = await CreateComment(parents.Peek());
        int next = random.Next(100);

        if(next < Math.Max(0, 50 - (15*parents.Count)))
            parents.Push(comment.Id);

        if(next > 50 && parents.Count > 1)
            parents.Pop();
    }

    return true;
}