using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MyWCFLib
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class MyWCFLib : IService1
    {

        static object locker = new object();
        static Dictionary<string, int> wordListStream = new Dictionary<string, int>();
        public static void GetOnThread(object paramObj)
        {
            Regex reg_exp = new Regex("[^a-zA-Zа-яА-Я]");
            String p = (String)paramObj;
            p = reg_exp.Replace(p, " ");
            string[] words = p.Split(
            new char[] { ' ' },
            StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].ToLower();
                lock (locker)
                {
                    if (wordListStream.ContainsKey(words[i]))
                    {
                        wordListStream[words[i]]++;
                    }
                    else
                    {

                        wordListStream.Add(words[i], 1);
                    }
                }
            }
        }

        public Dictionary<string, int> DoWork(string allText)
        {
            int i = allText.Length / 2;
            while (allText[i] != ' ')
            {
                i++;
            }

            Thread myThread1 = new Thread(new ParameterizedThreadStart(GetOnThread));
            myThread1.Start(allText.Substring(0, allText.Length - i));


            Thread myThread2 = new Thread(new ParameterizedThreadStart(GetOnThread));
            myThread2.Start(allText.Substring(i));

            myThread1.Join();
            myThread2.Join();

            wordListStream = wordListStream.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return wordListStream;
        }
    }
}
