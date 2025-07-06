using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using System.Web.UI.WebControls;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {

        private const string SessionKey = "Beneficiarios";

        private List<BeneficiarioModel> GetBeneficiarios()
        {
            if (Session[SessionKey] == null)
                Session[SessionKey] = new List<BeneficiarioModel>();
            return (List<BeneficiarioModel>)Session[SessionKey];
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            Session[SessionKey] = new List<BeneficiarioModel>();
            return View();
        }

        public ActionResult Beneficiario()
        {
            var model = new BeneficiarioModel();
            return PartialView("Beneficiario", model);
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBenef = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                ModelState.AddModelError("Erro", "CPF já Cadastrado");

                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {

                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                List<BeneficiarioModel> listaBenef = GetBeneficiarios();

                if (listaBenef.Count > 0)
                {
                    foreach (BeneficiarioModel Benef in listaBenef)
                    {
                        Beneficiario bnf = ConverterBeneficiario(Benef);
                        bnf.IdCliente = model.Id;
                        boBenef.Incluir(bnf);
                    }

                }



                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBenef = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if (bo.VerificarExistencia(model.CPF, model.Id))
            {
                ModelState.AddModelError("Erro", "CPF já Cadastrado");

                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

                var excluidos = (List<BeneficiarioModel>)Session["BeneficiariosExcluidos"];

                if (excluidos != null && excluidos.Count > 0)
                {
                    foreach (BeneficiarioModel Benef in excluidos)
                    {
                        Beneficiario bnf = ConverterBeneficiario(Benef);
                        bnf.IdCliente = model.Id;
                        boBenef.Excluir(bnf.Id);
                    }

                    Session["BeneficiariosExcluidos"] = new List<BeneficiarioModel>();

                }

                List<BeneficiarioModel> listaBenef = GetBeneficiarios();

                if (listaBenef.Count > 0)
                {
                    foreach (BeneficiarioModel Benef in listaBenef)
                    {
                        Beneficiario bnf = ConverterBeneficiario(Benef);
                        bnf.IdCliente = model.Id;
                        boBenef.Incluir(bnf);

                    }

                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBenef = new BoBeneficiario();
            List<Beneficiario> beneficiarios = boBenef.Consultar(id);
            Session[SessionKey] = ConverterParaModel(beneficiarios);
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };


            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult IncluirBeneficiario(BeneficiarioModel beneficiario)
        {
            var lista = GetBeneficiarios();
            lista.Add(beneficiario);
            return Json(lista);
        }

        [HttpPost]
        public JsonResult ExcluirBeneficiario(string cpf)
        {
            var lista = GetBeneficiarios();
            lista.RemoveAll(x => x.CPF == cpf);
            return Json(lista);
        }

        [HttpPost]
        public JsonResult AlterarBeneficiario(BeneficiarioModel beneficiario)
        {
            var lista = GetBeneficiarios();

            var item = lista.FirstOrDefault(x => x.CPF == beneficiario.CPFOriginal);
            if (item != null)
            {
                item.CPF = beneficiario.CPF;
                item.Nome = beneficiario.Nome;
            }

            Session["Beneficiarios"] = lista;
            return Json(lista);
        }

        [HttpGet]
        public JsonResult ListarBeneficiarios()
        {
            var lista = GetBeneficiarios();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public Beneficiario ConverterBeneficiario(BeneficiarioModel model)
        {
            Beneficiario benef = new Beneficiario();

            benef.Id = model.Id;
            benef.IdCliente = model.IdCliente;
            benef.CPF = model.CPF;
            benef.Nome = model.Nome;

            return benef;
        }

        public List<BeneficiarioModel> ConverterParaModel(List<Beneficiario> benef)
        {
            List<BeneficiarioModel> listaModel = new List<BeneficiarioModel>();
            foreach (var item in benef)
            {
                BeneficiarioModel model = new BeneficiarioModel();

                model.Id = item.Id;
                model.IdCliente = item.IdCliente;
                model.CPF = item.CPF;
                model.Nome = item.Nome;

                listaModel.Add(model);
            }
            

            return listaModel;
        }

        [HttpPost]
        public JsonResult ExcluirBeneficiarioSession(string cpf)
        {
            // Recupera a lista principal da sessão
            var lista = Session["Beneficiarios"] as List<BeneficiarioModel> ?? new List<BeneficiarioModel>();

            // Encontra o item a ser excluído
            var beneficiarioExcluido = lista.FirstOrDefault(x => x.CPF == cpf);

            if (beneficiarioExcluido != null)
            {
                // Remove da lista principal
                lista.Remove(beneficiarioExcluido);

                // Recupera a lista de excluídos da sessão
                var excluidos = Session["BeneficiariosExcluidos"] as List<BeneficiarioModel> ?? new List<BeneficiarioModel>();

                // Adiciona o excluído à lista de excluídos (evitando duplicidade por CPF)
                if (!excluidos.Any(x => x.CPF == cpf))
                    excluidos.Add(beneficiarioExcluido);

                // Atualiza as sessões
                Session["Beneficiarios"] = lista;
                Session["BeneficiariosExcluidos"] = excluidos;
            }

            return Json(lista);
        }

        [HttpPost]
        public JsonResult ValidarCpfBeneficiario(string cpf)
        {
            bool cpfBeneficiarioValido = CPFModel.ValidaCpfBeneficiarios(cpf);

            return Json(cpfBeneficiarioValido);
        }

    }
}