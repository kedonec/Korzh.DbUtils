﻿using Microsoft.Extensions.Logging;

namespace Korzh.DbUtils
{
    public interface IDbUtilsOptions
    {
        string InitialDataFolder { get; set; }

        IDbSeeder DbWriter { get; set; }

        IDatasetImporter DatasetImporter { get; set; }

        IDataUnpacker Unpacker { get; set; }

        ILoggerFactory LoggerFactory { get; set; }

    }
}