using System;
using System.Linq;
using Exercises.DictionarySuggestService;

namespace Exercises
{
    public class Exercise7
    {
        public static void Run()
        {
            var svc = new DictServiceSoapClient("DictServiceSoap");

//            svc.BeginMatchInDict("wn", "react", "prefix", 
//                iar => {
//                    var words = svc.EndMatchInDict(iar); 
//                    foreach (var word in words) 
//                        Console.WriteLine(word.Word);
//                }, 
//                null);

//            var matchInDict = Observable.FromAsyncPattern<string, string, string, DictionaryWord[]>(svc.BeginMatchInDict, svc.EndMatchInDict);


//            Func<string, IObservable<DictionaryWord[]>> suggestionForPrefix = prefix => matchInDict("wn", prefix, "prefix");
            Func<string, IObservable<DictionaryWord[]>> suggestionForPrefix = prefix => Observable.Return(new[] { new DictionaryWord() { Word = "test" } });

            var input = "reactive";
            for (int len = 4; len <= input.Length; len++)
            {
                var term = input.Substring(0, len);
                suggestionForPrefix(term).Subscribe(
                        words => Console.WriteLine(term + " -> " + words.Length + " => " + string.Join(";", from w in words select w.Word)));
            }


            Console.ReadLine();
        }
    }
}