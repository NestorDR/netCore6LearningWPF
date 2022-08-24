﻿using CommonLibrary;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LearningWPF.Common
{
    internal class AppSettings : CommonBase
    {
        #region Instance Property

        private static AppSettings? _instance;
        public static AppSettings Instance
        {
            get => _instance ??= new AppSettings();
            set => _instance = value;
        }

        #endregion

        #region Properties

        public IConfiguration? Configuration { get; private set; }

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                RaisePropertyChanged(nameof(ConnectionString));
            }
        }

        #endregion

        #region LoadSettings Method

        public void LoadSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        #endregion
    }
}
