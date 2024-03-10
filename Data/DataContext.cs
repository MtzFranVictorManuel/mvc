using MySqlConnector;

namespace mvc;

public class DataContext : IDataContext
{
    private readonly MySqlConnection _sqlConnection;

    public DataContext(MySqlConnection mySqlConnection)
    {
        _sqlConnection = mySqlConnection;
    }

    public async Task<List<Producto>> ObtenProductosAsync()
    {
        await _sqlConnection.OpenAsync();

        List<Producto> productos = new();
        using var command = new MySqlCommand(@"Select producto.id, producto.nombre, producto.precio, fabricante.nombre as fabricante_nombre from fabricante inner join producto on fabricante.id = producto.id_fabricante", _sqlConnection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            Producto item = new()
            {
                ProductoId = Convert.ToInt32(reader["id"]),
                Nombre = reader["nombre"].ToString(),
                Precio = Convert.ToDecimal(reader["precio"]),
                Fabricante = reader["fabricante_nombre"].ToString()
            };
            productos.Add(item);
        }
        return productos;
    }
}