using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServicio
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServicioNegocio
    {
        [OperationContract] string RegistrarUsuario(TB_USUARIO reg);
        [OperationContract] string EnviarCorreoConfimacion();
        [OperationContract] List<TB_TIPO_USUARIO> ListarTipoUsuario();
        [OperationContract] List<TB_TIPO_DOCUMENTO> ListarTipoDocumento();
        [OperationContract] List<TB_DISTRITO> ListarDistritos();
    }

    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class TB_USUARIO
    {
        [DataMember] public int COD_USU { get; set; }
        [DataMember] public int ID_TIPO_USU { get; set; }
        [DataMember] public string NOM_USU { get; set; }
        [DataMember] public string APE_USU { get; set; }
        [DataMember] public string NUMERO_CONTACTO_USU { get; set; }
        [DataMember] public string EMAIL_USU { get; set; }
        [DataMember] public int ID_DOC { get; set; }
        [DataMember] public string NUM_DOC_USU { get; set; }
        [DataMember] public int ID_DISTRITO { get; set; }
        [DataMember] public int ID_ESTADO { get; set; }
        [DataMember] public string LOGIN_USU { get; set; }
        [DataMember] public string PASSWORD_USU { get; set; }
    }

    [DataContract]
    public class TB_TIPO_USUARIO
    {
        [DataMember] public int ID_TIPO_USU	 { get; set; }
        [DataMember] public string DES_TIPO_USU { get; set; }
    }

    [DataContract]
    public class TB_TIPO_DOCUMENTO
    {
        [DataMember] public int ID_DOC	{ get; set; }
        [DataMember] public string DES_DOC { get; set; }
    }

    [DataContract]
    public class TB_DISTRITO
    {
        [DataMember] public int ID_DISTRITO	{ get; set; }
        [DataMember] public string NOM_DISTRITO { get; set; }
    }

    [DataContract]
    public class tablaConfirmacion
    {
        [DataMember] public string NOM_CONF { get; set; }
        [DataMember] public string APE_CONF { get; set; }
        [DataMember] public string EMAIL_CONF { get; set; }
        [DataMember] public string USU_CONF  { get; set; }
        [DataMember] public string PASS_CONF  { get; set; }
    }
}
