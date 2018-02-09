# SBJson
SBJson is a simple and ~~navie~~ brief json parser.

# Getting Start

It's the very best way without words, just being like a old Chinese saying gose:

__Men Sheng Fa Da Cai!__

```json
{
   "foo" : "bar",
   "headers" : {
      "data" : [
         233,
         {
            "readme" : "bingo"
         },
         false
      ]
   }
}
```

However, you do asked me how to use this $hit, I'm afried that you would become unhappy if I keep slience. Being the developer, I have to submit you some ~~__Ren Sheng Jing Yan__~~ samples.

```CSharp
using Lizho.SimpleBriefJson;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var JsonText = "{\"foo\" : \"bar\"    ,  \"headers\" : {\"data\" : [233,{\"readme\" : \"bingo\" },false   ]  }   }";
            var SBJson = new SBJsonHelper(JsonText);

            Console.Write("SBJson.JsonText   => ");
            Console.WriteLine(JsonText);
            Console.Write("SBJson.ToString() => ");
            Console.WriteLine(SBJson);
            Console.WriteLine();

            Console.Write("SBJson[\"foo\"] => ");
            Console.WriteLine(SBJson["foo"]);
            Console.WriteLine();

            Console.Write("SBJson[\"headers\"] => ");
            Console.WriteLine(SBJson["headers"]);
            Console.WriteLine();

            Console.Write("SBJson[\"headers\"][\"data\"] => ");
            Console.WriteLine(SBJson["headers"]["data"]);
            Console.WriteLine();

            Console.Write("SBJson[\"headers\"][\"data\"][0] => ");
            Console.WriteLine(SBJson["headers"]["data"][0]);
            Console.WriteLine();

            Console.Write("SBJson[\"headers\"][\"data\"][1] => ");
            Console.WriteLine(SBJson["headers"]["data"][1]);
            Console.WriteLine();

            Console.Write("SBJson[\"headers\"][\"data\"][1][\"readme\"] => ");
            Console.WriteLine(SBJson["headers"]["data"][1]["readme"]);
            Console.WriteLine();

            Console.Write("SBJson[\"headers\"][\"data\"][2] => ");
            Console.WriteLine(SBJson["headers"]["data"][2]);
            Console.ReadKey(true);
        }
    }
}
```

I'm ~~angry~~ EXCITED!