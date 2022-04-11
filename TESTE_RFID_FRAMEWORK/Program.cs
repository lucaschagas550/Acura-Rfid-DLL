using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ThingMagic;

namespace TESTE_RFID_FRAMEWORK
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            try
            {
                int timeout = 2000;

                //CONEXAO
                using (Reader reader = Reader.Create("eapi:///com5"))
                {
                    reader.Connect();

                    if ((reader is SerialReader) || (reader is LlrpReader))
                    {
                        if (Reader.Region.UNSPEC == (Reader.Region)reader.ParamGet("/reader/region/id"))
                        {
                            Reader.Region[] supportedRegions = (Reader.Region[])reader.ParamGet("/reader/region/supportedRegions");
                            if (supportedRegions.Length < 1)
                            {
                                throw new FAULT_INVALID_REGION_Exception();
                            }
                            else
                            {
                                reader.ParamSet("/reader/region/id", supportedRegions[0]);
                            }
                        }
                    }

                    //ANTENAS
                    int[] antennaList = new int[1];
                    antennaList[0] = 2;

                    //string str = "1,1";
                    //antennaList = Array.ConvertAll<string, int>(str.Split(','), int.Parse);
                    SimpleReadPlan plan = new SimpleReadPlan(antennaList, TagProtocol.GEN2, null, null, 1000);
                    reader.ParamSet("/reader/read/plan", plan);

                    //LEITURA
                    reader.TagRead += TagRead;
                    TagReadData[] tags = new TagReadData[0];

                    //POTENCIA DA LEITURA
                    int readPowerMax = (int)reader.ParamGet("/reader/radio/powerMax");
                    int readPowerMin = (int)reader.ParamGet("/reader/radio/powerMin");
                    reader.ParamSet("/reader/radio/readPower", 2150);

                    Console.WriteLine($"Read power Max: {readPowerMax}");
                    Console.WriteLine($"Read power Min: {readPowerMin}");

                    //LE TODAS AS TAGS DURANTE O PERIODO DEFINIDO E DEPOIS FAZ A SUA LEITURA EM FORMA DE LISTA OU ARRAY
                    Console.WriteLine($"{DateTime.Now}: Inicio a leitura");
                    tags = reader.Read(10000);

                    using (StreamWriter file = File.CreateText(@"C:/AcuraRfid/teste.txt"))
                    {
                        foreach (var item in tags)
                        {
                            file.WriteLine(item.Tag + "\n");
                            Console.WriteLine($"{DateTime.Now}: {item.Tag}");
                        }

                        reader.StopReading();

                        Console.WriteLine($"{DateTime.Now}: Parou a leitura");

                        //Console.WriteLine($"{DateTime.Now}: Inicio a leitura");
                        //reader.Read(timeout);
                        //reader.StartReading();

                        //await Task.Delay(100000);

                        //foreach (var item in tags)
                        //{
                        //    file.WriteLine(item.Tag + "\n");
                        //    Console.WriteLine($"{DateTime.Now}: {item.Tag}");
                        //}
                        Console.WriteLine($"{DateTime.Now}: Terminou a leitura");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        //FUNCIONA SOMENTE SE O STARTREADING FOR ATIVADO
        private static void TagRead(object sender, TagReadDataEventArgs e)
        {
            Console.WriteLine($"vvv {DateTime.Now}: {e.TagReadData.Tag}");
        }

        //AO PARAR E INICIAR A LEITURA ELE LE NOVAMENTE A TAG, NÃO GUARDA CACHE
    }
}

