using System;
using System.Linq;
using Exercises.DictionarySuggestService;
using System.Windows.Forms;

namespace Exercises
{
    public class Exercise8
    {
        public static void Run()
        {
            // The Web Service
            var svc = new DictServiceSoapClient("DictServiceSoap");
            var matchInDict = Observable.FromAsyncPattern<string, string, string, DictionaryWord[]>(svc.BeginMatchInDict, svc.EndMatchInDict);
            Func<string, IObservable<DictionaryWord[]>> suggestionForPrefix = prefix => matchInDict("wn", prefix, "prefix");

            // The GUI
            var textBox = new TextBox();
            var listBox = new ListBox {Top = textBox.Height + 10};
            var form = new Form {Controls = {textBox, listBox}};

            var input = (from evt in Observable.FromEvent<EventArgs>(textBox, "TextChanged")
                         select ((TextBox) evt.Sender).Text)
                .Where(w => w.Length > 3)
                .Throttle(TimeSpan.FromSeconds(1))
                .Do(x => Console.WriteLine("After Throttle:" + x));

            #region Stubbing

            const string INPUT = "reactive";
            var rand = new Random();

            IObservable<string> test_input = Observable.GenerateWithTime(
                    3, 
                    len => len <= INPUT.Length, 
                    len => len + 1,
                    len => INPUT.Substring(0, len), 
                    _ => TimeSpan.FromMilliseconds(rand.Next(200, 1200))
                )
               .ObserveOn(textBox)
               .Do(term => textBox.Text = term)
               .Throttle(TimeSpan.FromSeconds(1));

                Func<string, IObservable<DictionaryWord[]>> test_suggestionForPrefix = term => Observable.Return(
                                                        (from i in Enumerable.Range(0, rand.Next(0, 50)) 
                                                            select new DictionaryWord { Word = term + i }).ToArray())
                                                            .Delay(TimeSpan.FromSeconds(rand.Next(1, 10)));
            #endregion

            // replace with test_ to use the stubbed observables
            var suggestions = from term in input
                             from words in suggestionForPrefix(term).TakeUntil(input)
                             select words;

            using (suggestions.ObserveOn(listBox).Subscribe(words =>
                                          {
                                              listBox.Items.Clear();
                                              listBox.Items.AddRange((from word in words select word.Word).ToArray());
                                          },
                                          ex => Console.WriteLine(ex)))
                Application.Run(form);

        }
    }
}