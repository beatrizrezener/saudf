using System;
using System.Collections.Generic;
using SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Diagnostics;

namespace saudfhub
{
    class Conexao
    {
        static string nomeBanco = "saudf.sqlite";
        static string caminhoBanco = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "saudf.sqlite");

        private static async Task<bool> BancoJaExiste()
        {
            bool isDatabaseExisting = false;
            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(nomeBanco).AsTask().ConfigureAwait(false);
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            return isDatabaseExisting;
        }

        private static async Task<bool> FazCopiaDoBancoBemSucedida()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///DataModel/saudf.sqlite")).AsTask().ConfigureAwait(false);
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                await file.CopyAsync(folder).AsTask().ConfigureAwait(false);
                return true;
            }
            catch(Exception) 
            {
                return false;
            }
        }

        public static SQLiteConnection Conn()
        {

            if (BancoJaExiste().Result)
            {
                SQLiteConnection sConn = new SQLiteConnection(caminhoBanco);
                return sConn;
            }

            if (FazCopiaDoBancoBemSucedida().Result)
            {
                SQLiteConnection sConn = new SQLiteConnection(caminhoBanco);
                return sConn;
            }
            else
            {
                return null;
            }
        }
    }
}
