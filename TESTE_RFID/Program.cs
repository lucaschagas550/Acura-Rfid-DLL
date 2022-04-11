using System;
using ThingMagic;

namespace TESTE_RFID
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SDKUHFReader MyReader;

            MyReader = new SDKUHFReader(@"C:\Apps\Config\ACU100.UHF_cnx", @"C:\Apps\Config\ACU100.UHF_cfg", false, false);
            MyReader.TagDataType = UHFTagReturnType.EPC;
            MyReader.SetMode(UHFReaderMode.READING);
            MyReader.OnDeviceException += MyReader_OnDeviceException;
            MyReader.OnAgentException += MyReader_OnAgentException;
            MyReader.OnReadEPC += MyReader_OnReadEPC;
            MyReader.OnReadingStatus += MyReader_OnReadingStatus;
            MyReader.OnTagEnter += MyReader_OnTagEnter;
            MyReader.OnTagLeave += MyReader_OnTagLeave;
            MyReader.OnConnectionParam += MyReader_OnConnectionParam;
            MyReader.OnNoNewTag += MyReader_OnNoNewTag;
            MyReader.OnNoTag += MyReader_OnNoTag;
            MyReader.OnPing += MyReader_OnPing;
            Connect();
        }
    }
}
