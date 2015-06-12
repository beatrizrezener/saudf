using System;
using System.Collections.Generic;
using SQLite;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace saudfhub
{
    class Conexao
    {
        static string nomeBase = Path.Combine(ApplicationData.Current.LocalFolder.Path, "saudf.sqlite");

        private static async Task<bool> VerificarBanco(string nomeBanco)
        {
            bool bancoCriado = true;

            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(nomeBanco);
            }
            catch (Exception)
            {
                bancoCriado = false;
            }

            return bancoCriado;
        }

        private static void CriarBaseDeDados()
        {
            using (var db = new SQLiteConnection(nomeBase))
            {
                db.CreateTable<Unidade>();
                inserirRegistrosDeUnidadesNaBaseDeDados();
            }
        }

        public static void CriaBaseDeDadosSeNaoExistir()
        {
            if (!VerificarBanco(nomeBase).Result)
            {
                CriarBaseDeDados();
            }
        }

        public static SQLiteConnection Conn()
        {
            CriaBaseDeDadosSeNaoExistir();

            SQLiteConnection sConn = new SQLiteConnection(nomeBase);

            return sConn;
        }

        public static void inserirRegistrosDeUnidadesNaBaseDeDados()
        {
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSG 01 GAMA','-16.0115647315974','-48.0734467506395','(61) 34843540','QUADRA','GAMA SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU SAMAMBAIA QUADRA 317','-15.8906614780421','-48.1121778488145','(61) 34597151','QN 317 CJ 01 LT 01 AE','SAMAMBAIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS SERRA AZUL','-15.6358623504634','-47.8468751907335','(61) 34854898','DF 420 KM 01 QUADRA 03 AE 01','SOBRADINHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU BRAZLANDIA 01','-15.6698405742641','-48.200690746306','(61) 33917848','QUADRA 03 LOTE 06','BRAZLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU QUADRA 18','-15.7706701755519','-47.7785968780504','(61) 33694979','QUADRA','PARANOA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 03 CEILANDIA','-15.8309447765346','-48.0963635444627','(61) 33711106','AREA ESPECIAL LOTE D','CEILANDIA SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 07 CEILANDIA','-15.7956469058986','-48.1309747695909','(61) 35852088','AREA ESPECIAL DE','SETOR O','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU SANTA MARIA 02','-16.0073912143703','-47.9895901679979','(61) 33945616','QUADRA 212313','SANTA MARIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSP 03 PLANALTINA','-15.6149840354915','-47.6578116416917','(61) 33891520','RUA ALEXANDRE SALGADO QUADRA 20 LOTE','SETOR TRADICIONAL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSG 04 GAMA','-16.0253727436061','-48.0566239356981','(61) 35566878','ENTRE QUADRA','GAMA DF','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU AREAL QS 08','-15.8595371246333','-48.0239868164049','(61) 33560436','QS 08 CJ 410 A LT 15','AREAL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSRF 03 RIACHO FUNDO I','-15.8856081962581','-48.0239224433885','(61) 33998403','NA 07 POSTO DE SAUDE AREA ESPECIAL','RIACHO FUNDO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU POSTO URBANO N 01','-15.9051239490504','-47.7558517456041','(61) 3355570','RUA DA ESCOLA N 540','VILA NOVA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS MORRO AZUL','-15.8955860137935','-47.7880382537828','(61) 33356115','QD 11 CJ N CS 19','MORRO AZUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS JOAO CANDIDO','-15.8979463577266','-47.7611732482896','(61) 33390006','RUA 14','JOAO CANDIDO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS MORRO DA CRUZ','-15.916571617126','-47.7717304229722','Nao disponivel','CHACARA 10','MORRO DA CRUZ','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS JARDIM MORUMBI','-15.5236172676082','-47.618780136107','Nao disponivel','Q N LT 12','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 08 CEILANDIA','-15.8069336414333','-48.1298375129686','(61) 35853654','AREA ESPECIAL A B C D','CEILANDIA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS CHAPADINHA','-15.6651949882503','-48.1980943679796','Nao disponivel','GLEBA 02 RESERVA A','BRAZLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 05 BRASILIA','-15.846877098083','-47.8549861907945','(61) 33662530','SHIS QI 2123','LAGO SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 09 CRUZEIRO','-15.8008074760432','-47.9421901702867','(61)2330040','SHICES','CRUZEIRO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSSM 01 SANTA MARIA','-16.027153730392','-48.0263257026658','(61) 33936473','QR 207307 AE SEM N','SANTA MARIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSSA 03 SAMAMBAIA','-15.890103578567','-48.142154216765','(61) 33595500','QN 429 CONJUNTO F','SAMAMBAIA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSG 05 GAMA','-16.0134530067439','-48.0677175521837','(61) 35565111','QUADRA 38 AREA ESPECIAL LESTE SC','GAMA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSF 04 RIACHO FUNDO II','-15.9227192401881','-48.0437922477708','(61) 34045299','QC 06 CJ 16 LOTE 01','RIACHO FUNDO II','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS RIACHO FUNDO II QN 15 D','-15.9012722969051','-48.0437493324266','(61) 33330109','QN8C AE S N','RIACHO FUNDO II','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 07 TAGUATINGA','-15.8012902736659','-48.0939602851854','(61) 33712888','QNM 36 AE N','TAGUATINGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSS 03 SOBRADINHO','-15.6446492671962','-47.8243660926805','(61) 34856775','AR 17 CHACARA','SOBRADINHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSS 02 SOBRADINHO','-15.6548202037807','-47.8094744682298','(61) 34872196','QUADRA 3 AREA ESPECIAL CONJUNTOS','SOBRADINHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU 2 RIACHO FUNDO II','-15.9094369411464','-48.049221038817','(61) 34043931','AREA ESPECIAL QC','RIACHO FUNDO II','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS PONTE ALTA','-15.967179536819','-48.1253743171678','Nao disponivel','DF 180 KM 65','GAMA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR ENGENHO DAS LAGES','-16.0436546802516','-48.2544851303087','(61) 33594180','RUA LIBANIO','GAMA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 15 ASA NORTE','-15.7951962947841','-47.8480339050279','(61) 33061198','RUA','VILA PLANALTO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 04 TAGUATINGA','-15.8229839801784','-48.0675458908067','(61) 35611310','SETOR C NORTE','BRASILIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSNB 02 NUCLEO BANDEIRANTE','-15.8703088760371','-47.9656219482408','(61) 35520544','TERCEIRA AVENIDA CENTRO DE SAUDE AREA ESPECIAL','NUCLEO BANDEIRANTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS PONTE ALTA NORTE','-15.980322360992','-48.0721378326402','Nao disponivel','DF 457','GAMA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR ALMECEGAS','-15.5390453338619','-48.1686115264879','(61) 3515865','PSR ALMECEGAS','BRAZLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU CONDOMINIO PRIVE','-15.7851541042323','-48.1365966796861','(61) 33740979','AE 01 ENTRE RUAS 13 E 15','PRIVE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR TAQUARA','-15.6333196163173','-47.5213623046861','(61) 34858973','NUCLEO RURAL TAQUARA 201','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU RIACHO FUNDO II','-15.905767679214','-48.0458307266221','(61) 34341125','QN 07 A B AREA ESPECIAL LOTES 1 E 2','RIACHO FUNDO II','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CANTINHO DA SAUDE GRANJA DO TORTO','-15.7087004184718','-47.9135441780076','(61) 34687323','AREA ESPECIAL VILA WESLIAN RORIZ','GRANJA TORTO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS VILA BOA','-15.876585245132','-47.793123722075','(61) 33352482','RUA 07','SAO SEBASTIAO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSP 04 ESTANCIA DE PLANALTINA','-15.6201660633083','-47.6802349090562','(61) 34885853','ESTANCIA NOVA PLANALTINA QD 02 R A AREA ESPECIAL','ESTANCIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR CAUB I','-15.9475350379939','-48.0149316787706','(61) 33387810','CAUB I AREA ESPECIAL','RIACHO FUNDO II','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CERPIS','-15.6247580051418','-47.6515674591051','(61) 33889678','AV WL 04 SETOR HOSPITALAR OESTE','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS SITIO DO GAMA','-15.9931969642635','-47.9881095886217','(61) 33945650','PRACA ESQUADRAO OLIMPIAS','SANTA MARIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS BASEVI','-15.6459367275233','-47.8892111778245','(61) 30343792','AR 01','VILA BASEVI','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSGU 01 GUARA','-15.8183705806728','-47.9864573478685','(61) 35683296','QI 06 AREA ESPECIAL LT A','GUARA I','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 01 TAGUATINGA','-15.795539617538','-48.059670925139','(61) 33541441','QNG AREA ESPECIAL','TAGUATINGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 06 TAGUATINGA','-15.8364593982692','-48.0630397796617','(61) 33515043','QSC 01 AREA ESP','TAGUATINGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CENTRO DE SAUDE DA ESTRUTURAL','-15.7834482192989','-47.9972720146165','(61) 34657846','AREA ESPECIAL 02 AVENIDA CENTRAL CENTRO DE SAUDE','ESTRUTURAL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR JARDIM II','-16.016532182693','-47.3805141448961','Nao disponivel','NUCLEO RURAL DE JARDIM','PARANOA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSF EQUIPE 01 PSR PADDF','-16.0085391998286','-47.5556516647325','Nao disponivel','BR 251','PARANOA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU SANTA MARIA 01','-16.0103952884669','-48.0024218559251','(61) 33943988','ENTRE Q 212 E 213','SANTA MARIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS CASA GRANDE','-15.9644651412959','-48.0997538566575','(61) 34043846','NUCLEO RURAL CASA GRANDE 12 MA','RECANTO DAS EMAS','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS SAMAMBAIA QUADRA 501','-15.88415980339','-48.0870294570909','(61) 33570465','QR 501 CONJUNTO 07 LOTE','SAMAMBAIA SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('POSTO DE SAUDE N 01 JARDIM RORIZ','-15.6028389930721','-47.6482844352708','(61) 33880448','AREA ESPECIAL ENTREEEQUADRAS 03 04','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSG 06 GAMA','-16.0116505622859','-48.0741977691636','(61)5560980','ENTRE QUADRA','SETOR OESTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 07 BRASILIA','-15.8326828479762','-47.9096817970262','(61) 33452873','SGAS Q 612','ASA SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSS 01 SOBRADINHO','-15.6504642963405','-47.7819657325731','(61) 35912779','SOBRADINHO','SOBRADINHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR VARGEM BONITA','-15.9336304664607','-47.9396367073045','(61) 33801095','AREA RURAL VARGEM BONITA','NUCLEO BANDEIRANTES','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSCA 01 CANDANGOLANDIA','-15.8500099182124','-47.9514169692979','(61) 33015444','E Q 5 7 CENTRO DE SAUDE AREA ESPECIAL','CANDANGOLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS ENGENHO VELHO','-15.6003499031062','-47.8716588020311','Nao disponivel','LOTE 01 RUA 01','SOBRADINHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSG 08 GAMA','-16.0136997699733','-48.067417144774','(61) 35561032','AEREA ESPECIAL N','GAMADF','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSSM 02 SANTA MARIA','-15.78','-47.93','(61) 33945616','EQ 217218 E 317318 DE SANTA MARIA','SANTA MARIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSPA 02 ITAPOA','-15.7402324676509','-47.7632975578294','(61) 34674723','QD 378 AREA ESPECIAL','ITAPOA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR SAO JOSE','-15.7040226459499','-47.3735404014574','(61)3883264','NUCLEO RURAL RIO PRETO DF','COLONIA SAO JOSE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 08 TAGUATINGA','-15.8210635185237','-48.0925440788255','(61) 34752912','EQNL 24 AREA ESP NORTE','TAGUATINGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSRE 01 RECANTO DAS EMAS','-15.9196507930751','-48.1016635894761','(61) 33314773','QUADRA 307 AREA ESPECIAL N 01','RECANTO DAS EMAS','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 14 ASA NORTE','-15.783298015594','-47.9358386993394','(61) 2341666','AREA ESPECIAL SETOR ESCOLAR LOTE','CRUZEIRO VELHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR NOVA BETANIA','-15.9908902645106','-47.8134870529161','Nao disponivel','DF 140 KM 75','SAO SEBASTIAO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 08 BRASILIA','-15.8254301548','-47.9240155220018','(61) 324521775','AV W3 SUL EQS','ASA SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR DVO','-16.0458755493159','-48.0442428588853','(61) 33921572','RUA DAS AVENCAS LOTE 01','GAMA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU RECANTO DAS EMAS 01','-15.9301865100856','-48.1056547164903','Nao disponivel','AVENIDA MONJOLO QUADRA 311 CJ','RECANTO DAS EMAS','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS SAO FRANCISCO SS','-15.914597511291','-47.7606368064867','(61) 33398836','CJ 05 CS 01','SAO FRANCISCO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS RAJADINHA','-15.7465732097621','-47.6625108718858','Nao disponivel','RAJADINHA 2 KM 11 BR 130 CH 3 COQUEIRO','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS VEREDAS II QD 04','-15.6693577766414','-48.199081420897','(61) 34795787','QD 04 LT 06 LJS 1 E 2','BRAZLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSGU 02 GUARA','-15.8327364921565','-47.9731106758104','(61) 35673055','QE 23 AREA ESPECIAL','GUARA II','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 06 CEILANDIA','-15.8400750160213','-48.1144094467149','(61) 33761335','EQNP','CEILANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS VALE DOS PINHEIROS','-15.621024370193','-47.8215980529771','(61) 34854292','QD 45 A CONJUNTO A','COND VALE DOS PINHE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 12 CEILANDIA','-15.8047878742213','-48.1435489654527','(61) 33746556','AREA ESPECIAL SN','CEILANDIA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU SANTA MARIA 03','-15.78','-47.93','(61) 33932901','QR 202 E 303','SANTA MARIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSG 03 GAMA','-16.0057604312892','-48.0526328086839','(61) 35566689','AREA ESPECIAL','GAMA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 10 CEILANDIA','-15.8079314231868','-48.1206536293016','(61) 3713040','QNN','CEILANDIA SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU RIACHO FUNDO I QN 01','-15.879642963409','-48.0010056495653','(61) 33990491','QN 01 CONJUNTO 20 CASA','RIACHO FUNDO I','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 02 TAGUATINGA','-15.8105170726772','-48.0632328987108','(61) 33543020','CND 02 AREA ESPECIAL','TAGUATINGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 03 TAGUATINGA','-15.78','-47.93','(61) 33363028','EQNL','TAGUATINGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU ARAPOANGA','-15.6407654285426','-47.6402807235704','(61) 34891557','QUADRA 08 CONJUNTO I','ARAPOANGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSG 02 GAMA','-16.036520004272','-48.06130170822','(61) 34847220','QUADRA','SETOR SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSSA 04 SAMAMBAIA','-15.8784091472621','-48.0685973167405','(61) 33581335','QN 512 CONJUNTO 2 LOTE','SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 02 CEILANDIA','-15.8078455924983','-48.1206536293016','(61) 35852288','AREA ESPECIAL SN','CEILANDIA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR BOA ESPERANCA','-15.8289062976833','-48.2464814186082','(61) 35062082','NUCLEO RURAL BOA ESPERANCA','N R B ESPERANCA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('ESF 402 EQUIPE DE SAUDE DA FAMILIA','-15.9021520614619','-48.0599713325486','(61) 34346904','AV RECANTO DAS EMAS QD 102 AREA ESPECIAL','RECANTO DAS EMAS','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS LAGO OESTE','-15.6190824508662','-47.9482841491685','(61) 33021017','ROD DF 01 KM 13 RUA 08','SOBRADINHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR INCRA 8','-15.7429790496822','-48.1703066825853','(61) 35401282','QUADRA 16 LOTE','BRAZLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS BOSQUE 02','-15.9093296527858','-47.7532553672777','(61) 33395219','RUA 20 CJ A CS 24','BOSQUE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSPA 01 PARANOA','-15.7696187496181','-47.7799057960496','(61) 3691467','QD 21 AREA ESPECIAL SN','PARANOA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR TABATINGA','-15.8205592632289','-47.5693845748888','Nao disponivel','NUCLEO RURAL DE TABATINGA SN','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 13 ASA NORTE','-15.7430434226985','-47.8914213180528','(61) 32722948','EQN 114 115','ASA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS VILA NOVA I','-15.9091258049007','-47.7532982826219','Nao disponivel','RUA 53 CS 81','BOSQUE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU ITAPUA','-15.7454466819759','-47.7696919441209','Nao disponivel','AREA ESPECIAL ENTRE QUADRA 61 318','ITAPUA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('POSTO DE SAUDE DO VALE','-15.6774258613582','-47.6525330543504','Nao disponivel','CR 71 CASA 177','VALE DO AMANHECER','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 12 ASA NORTE','-15.761497020721','-47.8804993629442','(61) 32743455','EQN 208408','ASA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR SANTOS DUMONT','-15.6793248653407','-47.6483917236314','(61) 34890488','NUCLEO RURAL DE SANTOS DUMONT','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 05 CEILANDIA','-15.8031249046321','-48.1117057800279','(61) 33711672','QNM','CEILANDIA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS SETOR TRADICIONAL','-15.8900606632228','-47.7776527404771','(61) 33358778','RUA 06','SAO SEBASTIAO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS SAO FRANCISCO 01','-15.9419775009151','-48.2359457015977','(61) 33597003','QUADRA 03 LOTE 07 DF 280 KM','RECANTO DAS EMAS','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSBZ 01 BRAZLANDIA','-15.6770718097682','-48.1948328018174','(61) 33911533','QUADRA NORTE','BRAZLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 11 CEILANDIA','-15.7983720302577','-48.1335496902452','(61) 35852288','EQN0 1718','EXPSANSAO SETOR O','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR CAPAO SECO','-15.9630489349361','-47.557003498076','(61) 35064000','NUCLEO RURAL DE CAPAO SECO','PARANOA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSBZ 02 BRAZLANDIA','-15.6744217872615','-48.1990170478807','(61) 33911771','QUADRA 45 AREA ESPECIAL','BRAZLANDIA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSSB 01 SAO SEBASTIAO','-15.9040081501002','-47.7722668647752','(61) 33356221','CENTRO DE MULTIPLAS FUNCOES','CENTRO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU LUCIO COSTA GUARA','-15.8104312419887','-47.9887318611131','(61) 3823555','QE23 AREA ESPECIAL','GUARA I','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CENTRO DE SAUDE DO VARJAO','-15.710706710815','-47.8762292861925','Nao disponivel','Q','VARJAO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSSA 01 SAMAMBAIA','-15.8626055717464','-48.0791759490953','(61) 3582811','QDA SERVICO 408 AREA ESPECIAL','NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR CATINGUEIRO','-15.563560724258','-47.9352378845201','Nao disponivel','ROD DF 205 OESTE KM 13','SOBRADINHO','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSSA 02 SAMAMBAIA','-15.8758127689357','-48.1102466583238','(61) 34599316','QS 611 AREA ESPECIAL','NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS VILA ESTRUTURAL QUADRA 15','-15.7802188396449','-47.9998683929429','(61) 34655811','QD 15 CONJUNTO L CASA','ESTRUTURAL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS MINI CHACARAS','-15.6340706348415','-47.8378629684434','(61) 35951883','QMS 30 A AREA ESPECIAL','COND MINI CHACARA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UBS BICA DO DER','-15.6353259086604','-47.670278549193','Nao disponivel','CONDOMINIO CACHOEIRA LOTE','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR PIPIRIPAU','-15.5343568325038','-47.5109553337083','Nao disponivel','NUCLEO RURAL DO PIPIRIPAU','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR RIO PRETO','-15.7626235485072','-47.493059635161','(61)3883264','NUCLEO RURAL RIO PRETO','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSGU 03 GUARA','-15.8489906787868','-47.9703211784349','(61) 33011187','QE 38 AREA ESPECIAL','GUARA II','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSR CARIRU','-15.9070014953609','-47.5177145004259','Nao disponivel','NUCLEO RURAL DE CARIRU','PARANOA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('PSU POSTO URBANO N 02','-15.9076666831966','-47.7775454521165','(61) 33358173','QD 301 CJ 06 LT 01','RESIDENCIAL OESTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CST 05 TAGUATINGA','-15.8512651920314','-48.0473542213426','(61) 35612072','SETOR D SUL AREA ESP N','TAGUATINGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 11 ASA NORTE','-15.7734704017635','-47.892365455626','(61) 32748118','SGAN','ASA NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSB 10 ASA NORTE','-15.7255876064296','-47.8740835189805','(61) 35772092','SHIN QI','LAGO NORTE','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSC 04 CEILANDIA','-15.8355689048762','-48.1050109863267','(61) 33761946','AREA ESPECIAL SN','CEILANDIA SUL','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSP 02 PLANALTINA','-15.6122589111324','-47.6433491706834','(61) 33891866','ENTRE QUADRAS 110 AREA ESPECIAL','PLANALTINA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSRE 02 RECANTO DAS EMAS','-15.9021949768062','-48.0602717399583','(61) 33336326','QUADRA 102 AREA ESPECIAL N','RECANTO DAS EMAS','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('CSP 05 ARAPOANGA PLANALTINA','-15.6389522552486','-47.6473617553697','(61) 34891472','QUADRA 12 CONJ A AREA ESPECIAL C E FUTEBOL','ARAPOANGA','UBS')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital de Base','-15.800896','-47.888969','(61) 33151200','SHIS, Quadra 101 Bloco B - Area Especial','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Alvorada Brasilia','-15.813201','-47.912131','(61) 34429200','Seps 710/910 Cj D, s/n','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Prontonorte','-15.735378','-47.895309','(61) 34489100','SHLN BL G, 516 qd 516 lt 7 - Asa Norte ','ASA NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Santa Lucia','-15.828578','-47.929166','(61) 34450000','SHLS Q 716, 0 cj C - Asa Sul ','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Santa Helena','-15.735693','-47.897239','(61) 32150000','SHLN 516 cj D  - Asa Norte','ASA NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Brasilia','-15.845447','-47.882990','(61) 37049000','SHIS QI 15 Area Especial','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Regional de Saude de Brazlandia','-15.675074','-48.203996','(61) 33191832','Area Especial numero 6, Setor tradicional - Brazlandia','BRAZLANDIA','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Rede SARAH de Hospitais de Reabilitacao','-15.798894','-47.890001','(61) 33191111','SMHS Quadra 301 Bloco A - Asa Sul','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Santa Luzia','-15.827313','-47.930522','(61) 34456000','SHLS 716 Conjunto E - Asa Sul','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Daher ','-15.841474','-47.885285','(61) 32484848','SHLS 716 Conjunto E - Asa Sul','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Materno Infantil de Brasilia','-15.821942','-47.896106','(61) 34457500','Av. L2 Sul Quadra 608, Modulo A','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Regional da Asa Norte - HRAN','-15.785673','-47.882863','(61) 33254300','SMHN 101 Area Especial - Asa Norte','ASA NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Vida - UTI Movel','-15.815309','-47.908292','(61) 32483030','SAAN Quadra 1 Lote 25/35 - Asa Norte','ASA NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Santa Marta','-15.859855','-48.042606','(61) 34513000','Setor E, Area Especial 01 e 17, Taguatinga Sul','TAGUATINGA','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Militar de Brasilia','-15.772759','-47.930484','(61) 33626300','Smu Hgub, bl Sn','ASA NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Naval de Brasilia','-15.814717','-47.914739','(61) 34457300','SEPS 711/911, s/n bl A','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Regional de Sobradinho','-15.648612','-47.792290','(61) 34856775','Quadra 12 - Area Especial - Setor Central Sobradinho','SOBRADINHO','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital das Forcas Armadas','-15.801392','-47.936691','(61) 39662555',' Estrada Contorno do Bosque, Cruzeiro Novo','CRUZEIRO NOVO','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Planalto','-15.820160','-47.926535','(61) 39624409','Setor de Grandes Areas Sul 915','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Home Hospital de Medicina Especializada','-15.833847','-47.911386','(61) 38782878','Sgas 613, Conjunto C, Asa Sul','ASA SUL','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Pronto Socorro Queimaduras','-15.760302','-47.876796','(61) 32622200','R. SGAN 608 Md F - Asa norte','ASA  NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital de Apoio de Brasilia','-15.759256','-47.915299','(61) 39054700','Sain Lt 04 - Srpn','ASA NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital da Crianca de Brasilia','-15.759768','-47.917464','(61) 30258350','Lt 04, Shcnw Trecho 2 - SRPN','ASA NORTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Anchieta','-15.823967','-48.066628','(61) 33539000','Area Especial 8, ae 8/9 sl 118 B','TAGUATINGA','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Centro de Saude No 02','-15.870194','-47.965691','(61) 33865694','Area Especial 03, 3a Avenida - NUcleo Bandeirante','NUCLEO BANDEIRANTE','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Posto de Saude - Riacho Fundo I','-15.885181','-48.023552','(61) 33993064','Riacho Fundo I Ac 4 Qn 9','RIACHO FUNDO I','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Posto de Saude - Riacho Fundo II','-15.905819','-48.046297','(61) 34341125','Riacho Fundo II-1A Etapa Qn 7B Conjunto 3','RIACHO FUNDO II','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('Hospital Regional do Guara','-15.817710','-47.986099','(61) 33531500','Area Especial - QI O6 Lote C - Guara I','GUARA I','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UPA do Recanto das Emas','-15.911784','-48.057594','(61) 34346890','Quadra 400/600 Area Especial','RECANTO DAS EMAS','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UPA de Sao Sebastiao','-15.900793','-47.777748','(61) 33357664','Quadra 102 Conjunto 1 Lote 1','SAO SEBASTIAO','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UPA de Samambaia','-15.883017','-48.099695','(61) 34593282','QS 107 Conjunto 4 Area Especial','SAMAMBAIA','Hospital')");
            Conexao.Conn().Query<Unidade>("INSERT INTO Unidade (Nome, Latitude, Longitude, Telefone, Endereco, Bairro, Tipo) VALUES('UPA do Nucleo Bandeirante','-15.875977','-47.982235','(61) 33860447',' DF-075, KM 180, Area Especial EPNB','NUCLEO BANDEIRANTE','Hospital')");

        }
    }
}
