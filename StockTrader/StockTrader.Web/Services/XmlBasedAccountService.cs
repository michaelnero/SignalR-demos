using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using StockTrader.Web.Configuration;
using StockTrader.Web.Models;

namespace StockTrader.Web.Services {
    [RegisterInContainer(typeof(IAccountLocator), RegistrationType.Singleton)]
    [RegisterInContainer(typeof(IAccountPersister), RegistrationType.Singleton)]
    public class XmlBasedAccountService : IAccountLocator, IAccountPersister {
        private readonly DataContractSerializer serializer = new DataContractSerializer(typeof(Account));
        private readonly ConcurrentDictionary<string, Account> accounts = new ConcurrentDictionary<string, Account>();
        private readonly ConcurrentDictionary<string, ReaderWriterLockSlim> fileSyncs = new ConcurrentDictionary<string, ReaderWriterLockSlim>(); 
        private readonly IConfigurationSource configurationSource;

        public XmlBasedAccountService(IConfigurationSource configurationSource) {
            this.configurationSource = configurationSource;
        }

        public Account GetAccount(string id) {
            var account = this.accounts.GetOrAdd(id, this.LoadAccountFromDisk);
            return account;
        }

        public void SaveAccount(Account account) {
            string dataPath = this.GetPathFor(account.ID);

            var fileSync = this.fileSyncs.GetOrAdd(account.ID, key => new ReaderWriterLockSlim());
            
            fileSync.EnterWriteLock();

            try {
                File.Delete(dataPath);
                using (var fileStream = File.OpenWrite(dataPath)) {
                    this.serializer.WriteObject(fileStream, account);
                    fileStream.Flush(true);
                }
            } finally {
                fileSync.ExitWriteLock();
            }
        }

        private Account LoadAccountFromDisk(string accountID) {
            string dataPath = this.GetPathFor(accountID);

            var fileSync = this.fileSyncs.GetOrAdd(accountID, key => new ReaderWriterLockSlim());

            fileSync.EnterReadLock();

            try {
                if (File.Exists(dataPath)) {
                    using (var fileStream = File.OpenRead(dataPath)) {
                        return (Account)this.serializer.ReadObject(fileStream);
                    }
                }
            } finally {
                fileSync.ExitReadLock();
            }

            return null;
        }

        private string GetPathFor(string accountID) {
            var settings = this.configurationSource.GetSection<StockTraderSettings>(StockTraderSettings.SectionName);

            string dataPath = Path.Combine(settings.DataPath, accountID + ".xml");
            return dataPath;
        }
    }
}