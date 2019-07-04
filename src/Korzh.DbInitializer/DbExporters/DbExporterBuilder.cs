﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Logging;

using Korzh.DbUtils.DbSavers;

namespace Korzh.DbUtils.Exporters
{

    public static class DbType
    {
        public const string MsSqlServer = "mssql";

        public const string MySql = "mysql";
    }

    public class DbExportBuilderException : Exception
    {
        public DbExportBuilderException(string message) : base(message)
        {

        }
    }

    public class DbExporterBuilder
    {
        private Type _dbExporterType;
        private string _connectionString;
        private IDbSaver _saver;
        private ILogger _logger;

        public DbExporterBuilder()
        {

        }

        public DbExporterBuilder(ILogger logger)
        {
            _logger = logger;
        }

        public DbExporterBuilder SetConnectionString(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
            return this;
        }

        public DbExporterBuilder UseDbExporter(string dbType)
        {
            switch (dbType)
            {
                case DbType.MsSqlServer:
                    _dbExporterType = typeof(MsSqlServerExporter);
                    break;
                case DbType.MySql:
                    _dbExporterType = typeof(MySqlExporter);
                    break;
                default:
                    throw new DbExportBuilderException("Unknown Database type");
            }

            return this;

        }

        public DbExporterBuilder UseDbExporter<T>() where T : DbExporterBase
        {
            _dbExporterType = typeof(T);
            return this;

        }

        public DbExporterBuilder UseXmlZipFileDbSaver(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            _saver = new XmlZipFileDbSaver(fileName, _logger);
            return this;
        }

        public DbExporterBuilder UseJsonZipFileDbSaver(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            _saver = new JsonZipFileDbSaver(fileName, _logger);
            return this;
        }

        public DbExporterBuilder UseDbSaver(IDbSaver saver)
        {
            _saver = saver;
            return this;
        }

        public IDbExporter Build()
        {
            if (_connectionString == null)
            {
                throw new DbExportBuilderException("Connection string is not defined");
            }

            if (_dbExporterType == null)
            {
                throw new DbExportBuilderException("DbExporter is not defined");
            }

            if (_saver == null)
            {
                _saver = new XmlZipFileDbSaver("result", _logger);
            }

            return (IDbExporter)
                Activator.CreateInstance(_dbExporterType, _connectionString, _saver);
        }

    }
}
