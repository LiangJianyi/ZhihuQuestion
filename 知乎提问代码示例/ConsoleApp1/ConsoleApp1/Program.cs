using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp {
    [Serializable]
    public class MyData {
        private readonly int[] _collection;
        public int[] DataCollection => _collection;
        public MyData(int capacity) => this._collection = new int[capacity];
    }

    class Program {
        static void Main(string[] args) {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SerializationOverview.dat";

            PutMyDataSerializeBinary(path);
            var data = ReadMyDataFromBinary(path);
            if (data != null) {
                Console.WriteLine($"data.DataCollection.Count: {data.DataCollection.Length}");
                DateTime startime = DateTime.Now;
                int[] orderArr = data.DataCollection.OrderBy(e => e).ToArray();
                Console.WriteLine($"Orderby time consuming: {(DateTime.Now - startime).Seconds} seconds.");
                int i = 1;
                foreach (var item in orderArr) {
                    if (i <= 10) {
                        Console.Write($"{item} ");
                        i += 1;
                    }
                    else {
                        break;
                    }
                }
            }
            Console.ReadKey();
        }

        static void PutMyDataSerializeBinary(string path) {
            Console.WriteLine("Writing data...");

            const int CAPACITY = 200000000;
            MyData data = new MyData(CAPACITY);
            Random random = new Random();
            DateTime startime = DateTime.Now;
            for (int i = 0; i < CAPACITY; i++) {
                int v = random.Next(-10000, CAPACITY + 1);
                data.DataCollection[i] = v;
            }
            System.Diagnostics.Debug.WriteLine($"data.DataCollection add finish. Time consuming:{(DateTime.Now - startime).Seconds} seconds.");

            System.Diagnostics.Debug.WriteLine($"path is {path}");
            Stream SaveFileStream = null;
            try {
                SaveFileStream = File.Create(path);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(SaveFileStream, data);
            }
            finally {
                SaveFileStream?.Close();
            }

            Console.WriteLine("Write finish.");
        }

        static MyData ReadMyDataFromBinary(string path) {
            if (File.Exists(path)) {
                Console.WriteLine($"Reading {path}");
                Stream openFileStream = null;
                MyData data = null;
                try {
                    openFileStream = File.OpenRead(path);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    data = (MyData)deserializer.Deserialize(openFileStream);
                }
                catch (SerializationException e) {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                }
                finally {
                    openFileStream.Close();
                }
                Console.WriteLine($"Read finish.");
                return data;
            }
            else {
                Console.WriteLine($"Not found file: {path}");
                return null;
            }
        }
    }
}
