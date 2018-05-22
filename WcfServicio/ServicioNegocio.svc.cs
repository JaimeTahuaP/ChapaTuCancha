using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace WcfServicio
{
    public class Service1 : IServicioNegocio
    {
        SqlConnection cn = new SqlConnection("server=.; database=DB_CHAPA_TU_CANCHA; uid=SA; pwd=sql");

        public List<TB_DISTRITO> ListarDistritos()
        {
            List<TB_DISTRITO> listaDist = new List<TB_DISTRITO>();
            SqlCommand cmd = new SqlCommand("Select*from TB_DISTRITO", cn);
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                TB_DISTRITO dist = new TB_DISTRITO();
                dist.ID_DISTRITO = Convert.ToInt32(dr["ID_DISTRITO"].ToString());
                dist.NOM_DISTRITO = dr["NOM_DISTRITO"].ToString();
                listaDist.Add(dist);
            }

            cn.Close();
            return listaDist;
        }

        public List<TB_TIPO_DOCUMENTO> ListarTipoDocumento()
        {
            List<TB_TIPO_DOCUMENTO> listaTipDoc = new List<TB_TIPO_DOCUMENTO>();
            SqlCommand cmd = new SqlCommand("Select*from TB_TIPO_DOCUMENTO", cn);
            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                TB_TIPO_DOCUMENTO tDoc = new TB_TIPO_DOCUMENTO();
                tDoc.ID_DOC = Convert.ToInt32(dr["ID_DOC"].ToString());
                tDoc.DES_DOC = dr["DES_DOC"].ToString();
                listaTipDoc.Add(tDoc);
            }

            cn.Close();
            return listaTipDoc;
        }

        public List<TB_TIPO_USUARIO> ListarTipoUsuario()
        {
            List<TB_TIPO_USUARIO> listaTipUsu = new List<TB_TIPO_USUARIO>();
            SqlCommand cmd = new SqlCommand("SP_LISTAR_TIPO_USUARIO", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                TB_TIPO_USUARIO tUsu = new TB_TIPO_USUARIO();
                tUsu.ID_TIPO_USU = Convert.ToInt32(dr["ID_TIPO_USU"].ToString());
                tUsu.DES_TIPO_USU = dr["DES_TIPO_USU"].ToString();
                listaTipUsu.Add(tUsu);
            }

            cn.Close();
            return listaTipUsu;
        }

        public string RegistrarUsuario(TB_USUARIO reg)
        {
            string msg = "";
            cn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR_USUARIO", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID_TIPO_USU", 1);
                cmd.Parameters.AddWithValue("@NOM_USU", reg.NOM_USU);
                cmd.Parameters.AddWithValue("@APE_USU", reg.APE_USU);
                cmd.Parameters.AddWithValue("@NUMERO_CONTACTO_USU", reg.NUMERO_CONTACTO_USU);
                cmd.Parameters.AddWithValue("@EMAIL_USU", reg.EMAIL_USU);
                cmd.Parameters.AddWithValue("@ID_DOC", reg.ID_DOC);
                cmd.Parameters.AddWithValue("@NUM_DOC_USU", reg.NUM_DOC_USU);
                cmd.Parameters.AddWithValue("@ID_DISTRITO", reg.ID_DISTRITO);
                cmd.Parameters.AddWithValue("@ID_ESTADO", 1);
                cmd.Parameters.AddWithValue("@LOGIN_USU", reg.LOGIN_USU);
                cmd.Parameters.AddWithValue("@PASSWORD_USU", reg.PASSWORD_USU);

                cmd.ExecuteNonQuery();
                msg = "Registro Agregado!!";
            }
            catch (SqlException ex)
            {
                msg = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            return msg;
        }

        public string EnviarCorreoConfimacion()
        {
            #region Enviar Correo
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_confirmacion_registro", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();
                da.Fill(dt);

                var nombreUsuario = "";
                nombreUsuario = Convert.ToString(dt.Rows[0]["NOM_USU"]);
                if (dt != null && nombreUsuario != "" && nombreUsuario != " " && nombreUsuario != null)
                {
                    MailMessage oMail = new MailMessage();
                    SmtpClient oSmtp = new SmtpClient();
                    oSmtp.Host = "pod51009.outlook.com";
                    oSmtp.Port = 587;
                    oSmtp.Credentials = new NetworkCredential("i201521340@cibertec.edu.pe", "B87197c7");
                    oSmtp.EnableSsl = true;
                    oMail.From = new MailAddress("i201521340@cibertec.edu.pe");
                    oMail.To.Add(new MailAddress(Convert.ToString(dt.Rows[0]["EMAIL_USU"])));
                    oMail.CC.Add(new MailAddress("i201520724@cibertec.edu.pe"));
                    oMail.CC.Add(new MailAddress("i201521289@cibertec.edu.pe"));
                    oMail.CC.Add(new MailAddress("a0000661@cibertec.edu.pe"));
                    oMail.CC.Add(new MailAddress("i201520464@cibertec.edu.pe"));
                    oMail.Subject = "Confirmación de registro de usuario - Chapa tu cancha";
                    oMail.IsBodyHtml = true;
                    oMail.Body = bodyMensaje(dt);
                    oSmtp.Send(oMail);
                    oSmtp = null;
                    oMail = null;
                    return "Correcto";
                }
                else
                {
                    return "Incorrecto";
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message);
                throw ex;
            }
            #endregion
        }

        public string bodyMensaje(DataTable dataset)
        {
            if (dataset != null)
            {
                #region variables fecha y omailboy
                string oMailBody = "";
                //string vfecha = "";
                string nombre = "", apellido = "", correo = "", usuario = "", password = "";
                #endregion

                #region oList_tablaConfirmacion
                List<tablaConfirmacion> oList_tablaConfirmacion = new List<tablaConfirmacion>();
                var objtablaConfirmacion = (from DataRow dr in dataset.Rows
                                           select new
                                           {
                                               NOM_USU = dr["NOM_USU"],
                                               APE_USU = dr["APE_USU"],
                                               EMAIL_USU = dr["EMAIL_USU"],
                                               LOGIN_USU = dr["LOGIN_USU"],
                                               PASSWORD_USU = dr["PASSWORD_USU"]
                                               
                                           }).Distinct().ToList();
                foreach (var oitem in objtablaConfirmacion)
                {
                    nombre = Convert.ToString(oitem.NOM_USU);
                    apellido = Convert.ToString(oitem.APE_USU);
                    correo = Convert.ToString(oitem.EMAIL_USU);
                    usuario = Convert.ToString(oitem.LOGIN_USU);
                    password = Convert.ToString(oitem.PASSWORD_USU);
                }
                #endregion

                #region recuperando la plantilla de correo
                oMailBody = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~\plantilla.htm"), Encoding.UTF8);
                #endregion

                //#region llenando variable fecha
                //DateTime thisDay = DateTime.Today;
                //vfecha = thisDay.ToString("d");
                //#endregion

                //#region realizando replace a la variables fecha
                ////oMailBody = oMailBody.Replace("@fecha@", vfecha);
                //#endregion
                oMailBody = oMailBody.Replace("@NOM_CONF@", nombre);
                oMailBody = oMailBody.Replace("@APE_CONF@", apellido);
                oMailBody = oMailBody.Replace("@USU_CONF@", usuario);
                oMailBody = oMailBody.Replace("@PASS_CONF@", password);
                oMailBody = oMailBody.Replace("@EMAIL_CONF@", correo);

                return oMailBody;
            }
            else
            {
                return "Incorrecto";
            }
        }
    }
}
