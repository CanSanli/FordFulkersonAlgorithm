using System;
using System.Collections.Generic;

namespace FordFulkerson
{
    class Program
    {
        static int v = 5;
        static int[,] sonGraph = new int[v, v];
        static bool Control(int[,] graph, int source, int sink, int[] parent)
        {
            bool[] visited = new bool[v];  //Ziyaret edilen node'lar için bir adet array oluşturduk (Default olarak hepsi "false" değeri aldı)
            List<int> kuyruk = new List<int>();   //kuyruk oluşturduk
            kuyruk.Add(source);
            visited[source] = true;   //kaynak node'u ziyaret edildi olarak işaretlendi 
            parent[source] = -1;

            while (kuyruk.Count != 0)
            {
                int u = kuyruk[0];  //kuyruktaki değeri u değişkenine aldım ve kuyruktan sildim
                kuyruk.RemoveAt(0);
                for (int ver = 0; ver < v; ver++)
                {
                    if (visited[ver] == false && graph[u, ver] > 0) //bağlantı varsa diye bakıyoruz
                    {
                        if (ver == sink)
                        {
                            parent[ver] = u;      //eğer elimizdeki node'dan hedef node'a yol var ise daha fazla kontrole gerek yok 
                            return true;        //Hedef node'un parenti ver değeri ile tutulur
                        }
                        //Elimizdeki node'dan Hedef node'a yol yok ise, fakat kendi aralarında iki node arasında yol var ise o parent atanır
                        kuyruk.Add(ver);
                        parent[ver] = u;
                        visited[ver] = true;
                    }
                }
            }

            return false;   //Girilen balangıç noktasından (source) hedef noktaya (sink) yol bulunamadıysa false döndür;
        }

        static int FordFulkerson(int[,] graph, int source, int sink) //Gönderilen grafikte source ve sink (başlangıç ve hedef) arasındaki max akışı döndürür.
        {
            int[,] rGraph = new int[v, v];
            for (int u = 0; u < v; u++)
            {
                for (int ver = 0; ver < v; ver++)
                {
                    rGraph[u, ver] = graph[u, ver];     //gönderdiğim grafiği rGraph değişkenine kopyaladım
                }

            }

            int[] parent = new int[v];
            int max_flow = 0;

            while (Control(rGraph, source, sink, parent))
            {
                int path_flow = int.MaxValue;
                for (int i = sink; i != source; i = parent[i])
                {
                    int u = parent[i];
                    path_flow = Math.Min(path_flow, rGraph[u, i]);  //yoldaki minimum kalan kapasitesini buluyoruz


                }
                for (int i = sink; i != source; i = parent[i])  //yol boyunca kenarların ve ters kenarların kalan kapasitelerini güncelledik
                {
                    int u = parent[i];
                    rGraph[u, i] -= path_flow;
                    rGraph[i, u] += path_flow;
                }
                max_flow += path_flow;

            }
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    sonGraph[i, j] = rGraph[i, j];
                }
            }
            return max_flow;
        }

        static void Main(string[] args)
        {
            int[,] graph = new int[,] { { 0, 20, 30, 10, 0 }, { 0, 0, 40, 0, 30 }, { 0, 0, 0, 10, 20 }, { 0, 0, 5, 0, 20 }, { 0, 0, 0, 0, 0 } };
            Console.WriteLine($"Maksimum akış: {FordFulkerson(graph, 0, 4)}");
            Console.WriteLine();


            Console.WriteLine("SON GRAPH");
            Console.WriteLine("↓");
            Console.WriteLine("BAĞLANTI      AKIŞ MİKTAR");
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    if (sonGraph[i, j] > 0)
                    {
                        Console.WriteLine($"({i + 1},{j + 1})              {sonGraph[i, j]}");
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("BAĞLANTI         AKIŞ FARKLARI           AKIŞ MİKTAR         YÖN");
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    if (graph[i, j] > 0)
                    {
                        graph[i, j] -= sonGraph[i, j];
                        graph[j, i] -= sonGraph[j, i];
                        Console.WriteLine($"({i + 1},{j + 1})            ({graph[i, j]},{graph[j, i]})                  {graph[i, j]}               {i + 1} => {j + 1}");
                    }
                }
            }
        }
    }
}
