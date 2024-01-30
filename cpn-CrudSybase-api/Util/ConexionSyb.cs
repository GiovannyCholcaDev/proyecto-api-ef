using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using AdoNetCore.AseClient;
using System.Reflection;
using log4net;

namespace cpn_CrudSybase_api.Util
{
    public class ConexionSyb
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        #region "variables privadas"
        private static ConexionSyb? _instance = null;

        public static ConexionSyb Instance
        {
            get
            {
                // The first call will create the one and only instance.
                if (_instance == null)
                {
                    _instance = new ConexionSyb();
                }

                // Every call afterwards will return the single instance created above.
                return _instance;
            }
        }

        private Exception? _Exception;
        private AseCommand? command;
        private AseConnection? conexion = null;

        #endregion

        #region "Propiedades"
            public Exception Exception
            {
                get { return _Exception; }
                set { _Exception = value; }
            }
        #endregion

        #region "metodos"
        // PVI cierra la conexion a la base de result
        private void CloseConexion()
        {
            try
            {
                if (conexion != null && conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                    conexion.Dispose();
                }
            }
            catch (Exception ex)
            {
               // log.Error(ex.StackTrace);
                Exception = ex; 
            }
        }
        
        // PVI Ejecucion de la base de result con BAG
        public async Task<DataSet> GetDs(string commandtext, Dictionary<string, object> bag, string _connectionString)
        {
            _Exception = null;
            conexion = null;
            command = new AseCommand();
            DataSet resultado = new DataSet();

            try
            {
                var returnValue = new AseParameter 
                {
                    ParameterName = "@o_return",
                    AseDbType = AseDbType.Integer,
                    Direction = ParameterDirection.Output
                };
                command.CommandText = commandtext;
                command.CommandType = CommandType.Text;

                if (bag != null && bag.Count > 0)
                {
                    foreach (KeyValuePair<string, object> entry in bag)
                    {
                        // do something with entry.Value or entry.Key
                        command.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                }
                try
                {
                    conexion = new AseConnection(_connectionString);
                    await conexion.OpenAsync();
                    command.Connection = conexion;
                    command.CommandTimeout = 60;
                    var adapter = new AseDataAdapter(command);
                    adapter.Fill(resultado);
                }
                catch (Exception ex)
                {
                    DataTable tablaUno = new DataTable("Table");
                    tablaUno.Columns.Add("SSN");
                    DataTable tablaDos = new DataTable("Table1");
                    tablaDos.Columns.Add("CODE");
                    tablaDos.Columns.Add("MSG");
                    DataRow row = tablaDos.NewRow();
                    row["CODE"] = 300;
                    row["MSG"] = ex.Message;
                    tablaDos.Rows.Add(row);
                    resultado.Tables.Add(tablaUno);
                    resultado.Tables.Add(tablaDos);
                    log.Error(ex.Message);
                    Exception = ex;
                    CloseConexion();
                    try
                    {
                        if (command != null) { command.Dispose(); }
                    }
                    catch (Exception)
                    { log.Error(ex.StackTrace); }
                }
                finally
                {
                    CloseConexion();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                Exception = ex; 
            }

            return resultado;
        }
        
        // Devuelve solo el primer SELECT del SP
        public DataTable getDt(string commandtext, CommandType commandtype, string[] paramtername,
                                                  object[] parametervalue, string _connectionString)
        {
            _Exception = null;
            conexion = null;
            command = new AseCommand();
            DataTable resultado = new DataTable();
            try
            {
                command.CommandText = commandtext;
                command.CommandType = commandtype;
                if (paramtername != null && paramtername.Length > 0)
                {
                    for (int i = 0; i < paramtername.Length; i++)
                    { command.Parameters.AddWithValue(paramtername[i], parametervalue[i]); }
                }
                try
                {
                    conexion = new AseConnection(_connectionString);
                    conexion.Open();
                    command.Connection = conexion;
                    command.CommandTimeout = 999999999;
                    var adapter = new AseDataAdapter(command);

                    adapter.Fill(resultado);
                }
                catch (Exception ex)
                {
                    log.Error(ex.StackTrace);
                    Exception = ex;
                    CloseConexion();
                    try
                    {
                        if (command != null) { command.Dispose(); }
                    }
                    catch (Exception)
                    { log.Error(ex.StackTrace); }
                }
            }
            catch (Exception ex)
            { log.Error(ex.StackTrace);  Exception = ex; }
            return resultado;
        }

        // PVI Solo ejecucion par UPDATE o INSERTS
        public async Task<Boolean> exec(string commandtext, CommandType commandtype, Dictionary<string, object> bag, string _connectionString)
        {
            _Exception = null;
            conexion = null;
            command = new AseCommand();
            DataTable resultado = new DataTable();
            try
            {
                command.CommandText = commandtext;
                command.CommandType = commandtype;
                if (bag != null && bag.Count > 0)
                {
                    if (bag != null && bag.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> entry in bag)
                        {
                            // do something with entry.Value or entry.Key
                            command.Parameters.AddWithValue(entry.Key, entry.Value);
                        }
                    }
                }
                try
                {
                     conexion = new AseConnection(_connectionString);
                     await conexion.OpenAsync();
                     command.Connection = conexion;
                     command.CommandTimeout = 999999999;
                     var adapter = new AseDataAdapter(command);
                     adapter.Fill(resultado);
                     return true;
                }
                catch (Exception ex)
                {
                    log.Error(ex.StackTrace);
                    Exception = ex;
                    CloseConexion();
                    try
                    {
                        if (command != null) { command.Dispose(); }
                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            { log.Error(ex.StackTrace);  Exception = ex; return false; }
            finally
            {
                CloseConexion();
            }
        }
        #endregion

        public DataSet getInsert(string commandtext, Dictionary<string, object> bag, string _connectionString)
        {
            _Exception = null;
            conexion = null;
            command = new AseCommand();
            DataSet resultado = new DataSet();
            try
            {
                var returnValue = new AseParameter
                {
                    ParameterName = "@o_return",
                    AseDbType = AseDbType.Integer,
                    Direction = ParameterDirection.Output
                };
                command.CommandText = commandtext;
                command.CommandType = CommandType.Text;
                //command.Parameters.Add(returnValue);

                if (bag != null && bag.Count > 0)
                {
                    foreach (KeyValuePair<string, object> entry in bag)
                    {
                        // do something with entry.Value or entry.Key
                        command.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                }
                try
                {
                    conexion = new AseConnection(_connectionString);
                    //conexion.OpenAsync();
                    conexion.Open();
                    command.Connection = conexion;
                    command.CommandTimeout = 60;
                    var adapter = new AseDataAdapter(command);

                    adapter.Fill(resultado);
                }
                catch (Exception ex)
                {
                    log.Error(ex.StackTrace);
                    Exception = ex;
                    CloseConexion();
                    try
                    {
                        if (command != null) { command.Dispose(); }
                    }
                    catch (Exception)
                    { log.Error(ex.StackTrace); }
                }
                finally
                {
                    CloseConexion();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                Exception = ex;
            }

            return resultado;
        }

        public async Task<DataSet> getDs2(string commandtext, Dictionary<string, object> bag, string _connectionString)
        {
            _Exception = null;
            conexion = null;
            command = new AseCommand();
            DataSet resultado = new DataSet();

            try
            {
                var returnValue = new AseParameter
                {
                    ParameterName = "@o_return",
                    AseDbType = AseDbType.Integer,
                    Direction = ParameterDirection.Output
                };
                command.CommandText = commandtext;
                command.CommandType = CommandType.StoredProcedure;

                if (bag != null && bag.Count > 0)
                {
                    foreach (KeyValuePair<string, object> entry in bag)
                    {
                        // do something with entry.Value or entry.Key
                        command.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                }
                try
                {
                    conexion = new AseConnection(_connectionString);
                    await conexion.OpenAsync();
                    command.Connection = conexion;
                    command.CommandTimeout = 60;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    Exception = ex;
                    CloseConexion();
                    try
                    {
                        if (command != null) { command.Dispose(); }
                    }
                    catch (Exception)
                    { log.Error(ex.StackTrace); }
                }
                finally
                {
                    CloseConexion();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                Exception = ex;
            }

            return resultado;
        }

        
        /*************
         **@autor gcholca 
         *by INSERT, UPDATE, DELETE
         ***************/
        public async Task<bool> exec(string commandtext, Dictionary<string, object> bag, string _connectionString)
        {
            _Exception = null;
            conexion = null;
            DataTable resultado = new DataTable();
            try
            {
                conexion = new AseConnection(_connectionString);
                await conexion.OpenAsync();
                //command.Connection = conexion;
                //command.CommandTimeout = 999999999;
                var command = conexion.CreateCommand();
                command.CommandText = commandtext;

                if (bag != null && bag.Count > 0)
                {
                    foreach (KeyValuePair<string, object> entry in bag)
                    {
                        // do something with entry.Value or entry.Key
                        command.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                }

                var recordsModified = command.ExecuteNonQuery();

                if (recordsModified > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                Exception = ex;
                CloseConexion();
                try
                {
                    if (command != null) { command.Dispose(); }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            finally
            {
                CloseConexion();
            }
        }

        /*************
        **@autor gcholca 
        *by Stored Procedure
        ***************/
        public async Task<DataSet> ExecSP(string nameSP, Dictionary<string, object> bag, string _connectionString)
        {
            _Exception = null;
            conexion = null;
            DataTable resultado = new DataTable();
            try
            {
                conexion = new AseConnection(_connectionString);
                await conexion.OpenAsync();
                //command.Connection = conexion;
                //command.CommandTimeout = 999999999;
                var command = conexion.CreateCommand();
                command.CommandText = nameSP;
                command.CommandType = CommandType.StoredProcedure;

                if (bag != null && bag.Count > 0)
                {
                    foreach (KeyValuePair<string, object> entry in bag)
                    {
                        // do something with entry.Value or entry.Key
                        command.Parameters.AddWithValue(entry.Key, entry.Value);
                    }
                }

                // Create a DataSet to store the results
                var dataSet = new DataSet();

                var adapter = new AseDataAdapter(command);

                adapter.Fill(dataSet);

                return dataSet;          
          
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                Exception = ex;
                CloseConexion();
                try
                {
                    if (command != null) { command.Dispose(); }
                    return null!;
                }
                catch (Exception)
                {
                    return null!;
                }
            }
            finally
            {
                CloseConexion();
            }
        }

    }

}