using BuroManagementProject.Models;
using MySql.Data.MySqlClient;

namespace BuroManagementProject.Data
{
    public class KisilerData
    {
        private readonly string _connectionString;
        public readonly List<Kisiler>? kisilerList;

        public KisilerData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Kisiler> GetKisiler()
        {
                           

                using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            
            var cmd = new MySqlCommand("SELECT * FROM Kisiler", conn);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (kisilerList != null)
                {
                 kisilerList.Add(new Kisiler
                                {
                Ad = reader["Ad"].ToString(),
                 Soyad = reader["Soyad"].ToString(),
                  Telefon = reader["Telefon"].ToString(),
              Tc = reader["Tc_Kimlik_No"].ToString(),
                Eposta = reader["E_posta"].ToString(),
                       Sifre = reader["Sifre"].ToString(),
              BaroNo = GetNullableString(reader["Baro_No"]),
           AdresID = GetNullableString(reader["Adres_ID"]),
                    
                                });
                }
               
            }

            return kisilerList;
        }
        public string MuvekkilKayit(Kisiler k)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                var tcKontrolCmd = new MySqlCommand("SELECT COUNT(*) FROM kisiler WHERE Tc_Kimlik_No = @TC", conn, transaction);
                tcKontrolCmd.Parameters.AddWithValue("@TC", k.Tc);
                if (Convert.ToInt32(tcKontrolCmd.ExecuteScalar()) > 0)
                    return "Bu TC kimlik numarası zaten kayıtlı!";

                var mailKontrolCmd = new MySqlCommand("SELECT COUNT(*) FROM kisiler WHERE E_posta = @Eposta", conn, transaction);
                mailKontrolCmd.Parameters.AddWithValue("@Eposta", k.Eposta);
                if (Convert.ToInt32(mailKontrolCmd.ExecuteScalar()) > 0)
                    return "Bu e-posta adresi zaten kayıtlı!";

                var telefonKontrolCmd = new MySqlCommand("SELECT COUNT(*) FROM kisiler WHERE Telefon = @Telefon", conn, transaction);
                telefonKontrolCmd.Parameters.AddWithValue("@Telefon", k.Telefon);
                if (Convert.ToInt32(telefonKontrolCmd.ExecuteScalar()) > 0)
                    return "Bu telefon numarası zaten kayıtlı!";

                // Kayıt
                var kisiEkleCmd = new MySqlCommand(@"
            INSERT INTO kisiler (Ad, Soyad, Telefon, Tc_Kimlik_No, E_posta, Sifre) 
            VALUES (@Ad, @Soyad, @Telefon, @TC, @Eposta, @Sifre); 
            SELECT LAST_INSERT_ID();", conn, transaction);

                kisiEkleCmd.Parameters.AddWithValue("@Ad", k.Ad);
                kisiEkleCmd.Parameters.AddWithValue("@Soyad", k.Soyad);
                kisiEkleCmd.Parameters.AddWithValue("@Telefon", k.Telefon);
                kisiEkleCmd.Parameters.AddWithValue("@TC", k.Tc);
                kisiEkleCmd.Parameters.AddWithValue("@Eposta", k.Eposta);
                kisiEkleCmd.Parameters.AddWithValue("@Sifre", k.Sifre);

                var Kisi_ID = Convert.ToInt32(kisiEkleCmd.ExecuteScalar());

                var rolEkleCmd = new MySqlCommand("INSERT INTO kisi_rol (Kisi_ID, Rol_ID) VALUES (@Kisi_ID, @Otomatik_Rol_ID)", conn, transaction);
                rolEkleCmd.Parameters.AddWithValue("@Kisi_ID", Kisi_ID);
                rolEkleCmd.Parameters.AddWithValue("@Otomatik_Rol_ID", 2);
                rolEkleCmd.ExecuteNonQuery();

                transaction.Commit();
                return "Başarılı";
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return "Kayıt işlemi başarısız: " + e.Message;
            }
        }


        public string AvukatKayit(Kisiler k, AdresAvukat a)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // TC kontrolü
                var tcKontrolCmd = new MySqlCommand("SELECT COUNT(*) FROM kisiler WHERE Tc_Kimlik_No = @TC", conn, transaction);
                tcKontrolCmd.Parameters.AddWithValue("@TC", k.Tc);
                if (Convert.ToInt32(tcKontrolCmd.ExecuteScalar()) > 0)
                    return "Bu TC kimlik numarası zaten kayıtlı!";

                // E-posta kontrolü
                var mailKontrolCmd = new MySqlCommand("SELECT COUNT(*) FROM kisiler WHERE E_posta = @Eposta", conn, transaction);
                mailKontrolCmd.Parameters.AddWithValue("@Eposta", k.Eposta);
                if (Convert.ToInt32(mailKontrolCmd.ExecuteScalar()) > 0)
                    return "Bu e-posta adresi zaten kayıtlı!";

                // Telefon kontrolü
                var telefonKontrolCmd = new MySqlCommand("SELECT COUNT(*) FROM kisiler WHERE Telefon = @Telefon", conn, transaction);
                telefonKontrolCmd.Parameters.AddWithValue("@Telefon", k.Telefon);
                if (Convert.ToInt32(telefonKontrolCmd.ExecuteScalar()) > 0)
                    return "Bu telefon numarası zaten kayıtlı!";

                // Adres tablosuna ekleme
                var adresCmd = new MySqlCommand(@"
            INSERT INTO adres_avukat (Il, Ilce, Adres) 
            VALUES (@Il, @Ilce, @Adres); 
            SELECT LAST_INSERT_ID();", conn, transaction);

                adresCmd.Parameters.AddWithValue("@Il", a.Il);
                adresCmd.Parameters.AddWithValue("@Ilce", a.Ilce);
                adresCmd.Parameters.AddWithValue("@Adres", a.Adres);

                var adresID = Convert.ToInt32(adresCmd.ExecuteScalar());

                // Kisiler tablosuna ekleme
                var kisiEkleCmd = new MySqlCommand(@"
            INSERT INTO kisiler 
            (Ad, Soyad, Telefon, Tc_Kimlik_No, E_posta, Sifre, Baro_No, Adres_ID) 
            VALUES (@Ad, @Soyad, @Telefon, @TC, @Eposta, @Sifre, @BaroNo, @AdresID);
            SELECT LAST_INSERT_ID();", conn, transaction);

                kisiEkleCmd.Parameters.AddWithValue("@Ad", k.Ad);
                kisiEkleCmd.Parameters.AddWithValue("@Soyad", k.Soyad);
                kisiEkleCmd.Parameters.AddWithValue("@Telefon", k.Telefon);
                kisiEkleCmd.Parameters.AddWithValue("@TC", k.Tc);
                kisiEkleCmd.Parameters.AddWithValue("@Eposta", k.Eposta);
                kisiEkleCmd.Parameters.AddWithValue("@Sifre", k.Sifre);
                kisiEkleCmd.Parameters.AddWithValue("@BaroNo", k.BaroNo);
                kisiEkleCmd.Parameters.AddWithValue("@AdresID", adresID);

                var kisiID = Convert.ToInt32(kisiEkleCmd.ExecuteScalar());

                // kisi_rol tablosuna ekleme
                var rolEkleCmd = new MySqlCommand("INSERT INTO kisi_rol (Kisi_ID, Rol_ID) VALUES (@KisiID, @Otomatik_Rol_Id)", conn, transaction);
                rolEkleCmd.Parameters.AddWithValue("@KisiID", kisiID);
                rolEkleCmd.Parameters.AddWithValue("@Otomatik_Rol_ID", 1); // 1 = Avukat
                rolEkleCmd.ExecuteNonQuery();

                transaction.Commit();
                return "Başarılı";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return "Kayıt işlemi başarısız: " + ex.Message;
            }
        }

        public int? GetRol_ID_By_Mail_Password(string mail, string password)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var getRolCmd = new MySqlCommand(
                "SELECT Rol_ID FROM kisi_rol WHERE Kisi_ID = " +
                "(SELECT Kisi_ID FROM kisiler WHERE E_posta = @mail AND Sifre = @password);", conn);

            getRolCmd.Parameters.AddWithValue("@mail", mail);
            getRolCmd.Parameters.AddWithValue("@password", password);

            var result = getRolCmd.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : (int?)null;
            /*
             * ExecuteScalar(): SQL sorgusunun döndürdüğü tek bir değeri alır
             *  Eğer null değilse, int'e çevirir ve döner. Eğer null ise, null(int? null) değerini döner.
             */
        }




        public static string GetNullableString(object obj)
        {
            return obj != DBNull.Value ? obj.ToString() : null;
        }
       

    }
}

