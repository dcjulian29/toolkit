using System;

namespace ToolKit.Data
{
    public interface IDatabase
    {
        void InitializeDatabase(Action initialization);
    }
}
