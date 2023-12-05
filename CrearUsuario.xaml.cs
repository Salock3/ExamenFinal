using Microsoft.Maui.Controls;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace ProyectoAnashe;
public class User
{

}
public partial class PaguinaPrincipal : ContentPage
{

    List<Cliente> clie;
    string appData;
    string fileName;
    string clave;
    private const string ClaveCorrecta = "contrasena";
    int count;
    List<Cliente> MISCLIENTES = new List<Cliente>();
    private object datos;
    public PaguinaPrincipal()
    {
        InitializeComponent();
        clie = new List<Cliente>();
        appData = FileSystem.Current.AppDataDirectory;
        fileName = "Cliente.json";
    }


    private void Cliked_Usuario(object sender, EventArgs e)
    {
        string nombreCliente = EntryUsuario.Text;
        string Clave = EntryRut.Text;
        string rut = EntryClave.Text;
        string apellidoPaternoCliente = EntryApellidoPaterno.Text;
        string apellidoMaternoCliente = EntryApellidoMaterno.Text;
        Fecha MisFechas = new Fecha();
        int dia = int.Parse(EnntryDia.Text);
        int mes = int.Parse(EntryMes.Text);
        int Anhio = int.Parse(EntryAño.Text);
        Cliente MisCliente = new Cliente(Int32.Parse(Clave), nombreCliente, rut, apellidoPaternoCliente, apellidoMaternoCliente, MisFechas);
        MISCLIENTES.Add(MisCliente);
        EntryUsuario.Text = "";
        EntryClave.Text = "";
        EntryRut.Text = "";
        EntryApellidoPaterno.Text = "";
        EntryApellidoMaterno.Text = "";
        EnntryDia.Text = "";
        EntryMes.Text = "";
        EntryAño.Text = "";
        DisplayAlert("Éxito", "Creacion de sesion éxitosa", "Aceptar");
        Navigation.PushAsync(new CrearUsuari1());
        if (EntryRut.Text == string.Empty || EntryClave.Text == string.Empty)
        {
            var alert = DisplayAlert("URL", appData, "Ok");
            var user = new Cliente(Int32.Parse(Clave), nombreCliente, rut, apellidoPaternoCliente, apellidoMaternoCliente, MisFechas);
            clie.Add(user);
            string usersJson = JsonConvert.SerializeObject(clie, Formatting.Indented);
            File.WriteAllText(appData + '\\' + fileName, usersJson);
        }
        else
        {
            DisplayAlert("Ingresar Usuarios", "Campos sin datos\nUsuario no agregado", "ok");
        }
        
        var strConect = "Data source = Creacion_Usuario.db";
        SqliteConnection connection = new SqliteConnection(strConect);


        connection.Open();


        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"Create table if NOT EXISTS Cliente(rut integer(15) primary key,
                Clave VARCHAR(8),
                nombreCliente VARCHAR(100),
                ApellidoPaternoCliente VARCHAR(100),
                ApellidoMaternoCliente VARCHAR(100),
                Dia INTEGER,
                Mes INTEGER,
                Año INTEGER,
                Sexo VARCHAR(15))";

        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Cliente VALUES (@Clave, @rut, @nombreCliente, @ApellidoPaternoCliente, @ApellidoMaternoCliente, @Dia, @Mes, @Año, @Sexo)";
        command.Parameters.Add("@rut", SqliteType.Integer);
        command.Parameters.Add("@Clave", SqliteType.Text);
        command.Parameters.Add("@nombreCliente", SqliteType.Text);
        command.Parameters.Add("@ApellidoPaternoCliente", SqliteType.Text);
        command.Parameters.Add("@ApellidoMaternoCliente", SqliteType.Text);
        command.Parameters.Add("@Dia", SqliteType.Integer);
        command.Parameters.Add("@Mes", SqliteType.Integer);
        command.Parameters.Add("@Año", SqliteType.Integer);
        command.Parameters.Add("@Sexo", SqliteType.Text); 
        command.Parameters["@rut"].Value = Convert.ToInt32(rut);
        command.Parameters["@Clave"].Value = Convert.ToString(Clave);
        command.Parameters["@nombreCliente"].Value = Convert.ToString(nombreCliente); 
        command.Parameters["@ApellidoPaternoCliente"].Value = Convert.ToString(apellidoPaternoCliente); 
        command.Parameters["@ApellidoMaternoCliente"].Value = Convert.ToString(apellidoMaternoCliente); 
        command.Parameters["@Dia"].Value = Convert.ToInt32(dia);
        command.Parameters["@Mes"].Value = Convert.ToInt32(mes);
        command.Parameters["@Año"].Value = Convert.ToInt32(Anhio);
        command.Parameters["@Sexo"].Value = SexoUsuario.Text; 
        int filas = command.ExecuteNonQuery();



        connection.Close();


    }
    
    private void cli(object sender, EventArgs e)
    {
        string json = File.ReadAllText(appData + '\\' + fileName);
        DisplayAlert("Json", json, "Ok");
        List<User> users = new List<User>();
        users = JsonConvert.DeserializeObject<List<User>>(json);
        DisplayAlert("Json", json, "Ok");
    }

    

}

