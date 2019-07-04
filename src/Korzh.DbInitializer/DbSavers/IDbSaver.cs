﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Korzh.DbUtils.DbSavers
{
    public interface IDbSaver
    {
        void Start();
        void StartSaveTable(string tableName);
        void SaveTableData(IDataReader dataReader);
        void EndSaveTable();
        void End();
    }

}
