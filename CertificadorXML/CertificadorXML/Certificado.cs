
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using CertificadorXML;

namespace CertificadorXML
{
    class Certificado
    {
        /// <summary>
        /// Certificado digital
        /// </summary>
        public X509Certificate2 x509Cert { get; set; }
        /// <summary>
        /// Utilizado para certificados A3
        /// </summary>
        public string CertificadoPIN { get; set; }
        /// <summary>
        /// Type do Provider do Certificado selecionado
        /// </summary>
        public string ProviderTypeCertificado { get; set; }
        /// <summary>
        /// Provider utilizado pelo certificado se utilizar a opção para salvar o PIN
        /// </summary>
        public string ProviderCertificado { get; set; }

        #region AssinarXML
        /// <summary>
        /// Assina o XML e sobrepondo-o
        /// </summary>
        /// <param name="strDirXML">Nome do arquivo XML a ser assinado</param>
        /// <param name="objSchema">Objeto de schema a ser utilizado na assinatura</param>
        public void AssinarXML(string strDirXML, clsSchemaXML objSchema)
        {
            if (!String.IsNullOrEmpty(objSchema.InfSchemas.TagAssinatura))
            {
                if (!Assinado(strDirXML, objSchema.InfSchemas.TagAssinatura))
                    this.Assinar(strDirXML, objSchema.InfSchemas.TagAssinatura, objSchema.InfSchemas.TagAtributoId);
            }

            //Assinar o lote
            if (!String.IsNullOrEmpty(objSchema.InfSchemas.TagLoteAssinatura))
                if (!Assinado(strDirXML, objSchema.InfSchemas.TagLoteAssinatura))
                    this.Assinar(strDirXML, objSchema.InfSchemas.TagLoteAssinatura, objSchema.InfSchemas.TagLoteAtributoId);
        }
        #endregion

        #region Assinar
        /// <summary>
        /// O método assina digitalmente o arquivo XML passado por parâmetro e 
        /// grava o XML assinado com o mesmo nome, sobreponto o XML informado por parâmetro.
        /// Disponibiliza também uma propriedade com uma string do xml assinado (this.vXmlStringAssinado)
        /// </summary>
        /// <param name="strDirXML">Diretorio do arquivo XML a ser assinado</param>
        /// <param name="tagAssinatura">Nome da tag onde é para ficar a assinatura</param>
        /// <param name="tagAtributoId">Nome da tag que tem o atributo ID, tag que vai ser assinada</param>
        private void Assinar(string strDirXML, string tagAssinatura, string tagAtributoId)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.PreserveWhitespace = false;
                xmlDoc.Load(strDirXML);

                if (xmlDoc.GetElementsByTagName(tagAssinatura).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAssinatura.Trim() + " não existe no XML. (Código do Erro: 5)");
                }
                else if (xmlDoc.GetElementsByTagName(tagAtributoId).Count == 0)
                {
                    throw new Exception("A tag de assinatura " + tagAtributoId.Trim() + " não existe no XML. (Código do Erro: 4)");
                }
                else
                {
                    XmlDocument xmlAss;

                    XmlNodeList lists = xmlDoc.GetElementsByTagName(tagAssinatura);

                    foreach (XmlNode nodes in lists)
                    {
                        foreach (XmlNode childNodes in nodes.ChildNodes)
                        {
                            if (!childNodes.Name.Equals(tagAtributoId))
                                continue;

                            Reference reference = new Reference();
                            reference.DigestMethod = SignedXml.XmlDsigSHA1Url;
                            reference.Uri = String.Empty;

                            //pega o uri que deve ser assinada                                       
                            XmlElement childElemen = (XmlElement)childNodes;

                            if (childElemen.GetAttributeNode("Id") != null)
                            {
                                reference.Uri = "#" + childElemen.GetAttributeNode("Id").Value;
                            }
                            else if (childElemen.GetAttributeNode("id") != null)
                            {
                                reference.Uri = "#" + childElemen.GetAttributeNode("id").Value;
                            }

                            // Create a SignedXml object.
                            SignedXml signedXml = new SignedXml(xmlDoc);

                            //A3
                            if (!String.IsNullOrEmpty(CertificadoPIN) && IsA3(this.x509Cert))
                            {
                                signedXml.SigningKey = LerDispositivo(this.CertificadoPIN, Convert.ToInt32(this.ProviderTypeCertificado), this.ProviderCertificado);
                            }
                            else
                            {
                                signedXml.SigningKey = this.x509Cert.PrivateKey;
                            }

                            // Add an enveloped transformation to the reference.
                            //XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                             XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                            reference.AddTransform(env);

                            XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                            reference.AddTransform(c14);

                            // Add the reference to the SignedXml object.
                            signedXml.AddReference(reference);

                            // Create a new KeyInfo object
                            KeyInfo keyInfo = new KeyInfo();

                            // Load the certificate into a KeyInfoX509Data object
                            // and add it to the KeyInfo object.
                            keyInfo.AddClause(new KeyInfoX509Data(this.x509Cert));

                            // Add the KeyInfo object to the SignedXml object.
                            signedXml.KeyInfo = keyInfo;
                            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
                            signedXml.ComputeSignature();

                            // Get the XML representation of the signature and save
                            // it to an XmlElement object.
                            XmlElement xmlDigitalSignature = signedXml.GetXml();

                            //Gravar o elemento no documento XML
                            nodes.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
                        }
                    }

                    xmlAss = new XmlDocument();
                    xmlAss.PreserveWhitespace = false;
                    xmlAss = xmlDoc;

                    // Atualizar a string do XML já assinada
                    string StringXMLAssinado = xmlAss.OuterXml;

                    // Gravar o XML Assinado no HD
                    StreamWriter sw = File.CreateText(strDirXML);
                    sw.Write(StringXMLAssinado);
                    sw.Close();
                }
            }
            catch (System.Security.Cryptography.CryptographicException ex)
            {
                throw new Exception("O certificado deverá ser reiniciado.\r\n Retire o certificado.\r\nAguarde o LED terminar de piscar.\r\n Recoloque o certificado e informe o PIN novamente.\r\n" + ex.Message.ToString());
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region IsA3
        /// <summary>
        /// Retorna true se o certificado for do tipo A3.
        /// </summary>
        /// <param name="_x509cert">Certificado que deverá ser validado se é A3 ou não.</param>
        private bool IsA3(X509Certificate2 _x509cert)
        {
            if (_x509cert == null)
                return false;

            bool result = false;

            try
            {
                RSACryptoServiceProvider service = _x509cert.PrivateKey as RSACryptoServiceProvider;

                if (service != null)
                {
                    if (service.CspKeyContainerInfo.Removable &&
                    service.CspKeyContainerInfo.HardwareDevice)
                        result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region LerDispositivo
        /// <summary>
        /// Ler o dispositivo certificado A3
        /// </summary>
        /// <param name="PIN">Codigo PIN do certificado</param>
        /// <param name="providerType">Type do Provider do Certificado selecionado</param>
        /// <param name="provider">Provider utilizado pelo certificado se utilizar a opção para salvar o PIN</param>
        private RSACryptoServiceProvider LerDispositivo(string PIN, int providerType, string provider)
        {
            CspParameters csp = new CspParameters(providerType, provider);

            SecureString ss = new SecureString();

            foreach (char a in PIN)
            {
                ss.AppendChar(a);
            }

            csp.KeyPassword = ss;
            csp.KeyNumber = 1;
            csp.Flags = CspProviderFlags.NoPrompt;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
            rsa.PersistKeyInCsp = false;
            return rsa;
        }
        #endregion

        #region Assinado
        /// <summary>
        /// Verificar se o XML já tem assinatura
        /// </summary>
        /// <param name="strDirXML">Arquivo XML a ser verificado se tem assinatura</param>
        /// <param name="tagAssinatura">Tag de assinatura onde vamos pesquisar</param>
        private bool Assinado(string strDirXML, string tagAssinatura)
        {
            bool retorno = false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(strDirXML);

                if (doc.GetElementsByTagName(tagAssinatura)[0].LastChild.Name == "Signature")
                    retorno = true;
            }

            catch { }
            return retorno;
        }
        #endregion

        #region GetCertificado
        /// <summary>
        /// Busca o certificado pelo numero do serial
        /// </summary>
        /// <param name="strSerialNumber">Numero serial do certificado</param>
        public X509Certificate2 GetCertificado(string strSerialNumber, byte[] arquivocertificado = null, string certpass = "")
        {
            byte[] buffer = null;

            if (arquivocertificado != null && certpass != String.Empty)
            {
                try
                {
                    string sCaminho = Environment.CurrentDirectory + "\\certificadodigital.pfx";

                    if (!File.Exists(sCaminho))
                    {
                        buffer = arquivocertificado;
                        File.WriteAllBytes(sCaminho, buffer);
                        return SignFile(sCaminho, certpass);
                    }
                    else
                    {
                        buffer = arquivocertificado;

                        FileStream stream = File.OpenRead(sCaminho);
                        byte[] fileBytes = new byte[stream.Length];

                        stream.Read(fileBytes, 0, fileBytes.Length);
                        stream.Close();
                        stream.Dispose();

                        if (!ArraysEqual(buffer, fileBytes))
                        {
                            File.WriteAllBytes(sCaminho, buffer);
                            return SignFile(sCaminho, certpass);
                        }
                        else
                        {
                            return SignFile(sCaminho, certpass);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ExcecaoCertificadoDigital(String.Format("Falha ao acessar certificado digital.\n{0}", ex.Message));
                }

            }
            else
            {
                X509Store stores = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                stores.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificados = stores.Certificates;

                foreach (X509Certificate2 certificado in certificados)
                {
                    if (certificado.GetSerialNumberString().ToLower() == strSerialNumber.Trim().ToLower())
                    {
                        stores.Close();
                        return certificado;
                    }
                }

                stores.Close();
            }

            throw new ExcecaoCertificadoDigital(String.Format("Certificado não encontrado.\nNumero do serial: {0}", strSerialNumber));
        }

        #endregion

        #region LeituraArquivoCertificado
        public static X509Certificate2 SignFile(string CertFile, string CertPass)
        {

            FileStream fs = new FileStream(CertFile, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            fs.Dispose();
            X509Certificate2 cert = new X509Certificate2(buffer, CertPass);
            return cert;
        }
        public static bool ArraysEqual(byte[] array1, byte[] array2, int bytesToCompare = 0)
        {
            bool areEqual = array1.SequenceEqual(array2);

            if (areEqual)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion 
    }
}
