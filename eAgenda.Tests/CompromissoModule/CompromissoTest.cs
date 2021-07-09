using eAgenda.Controladores.CompromissoModule;
using eAgenda.Controladores.Shared;
using eAgenda.Dominio.ContatoModule;
using eAgenda.Dominio.CompromissoModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using eAgenda.Controladores.ContatoModule;
using FluentAssertions;
using System.Collections.Generic;

namespace eAgenda.Tests.CompromissoModule
{
    [TestClass]
    public class CompromissoTest
    {
        readonly ControladorContato controladorCont = null;
        readonly ControladorCompromisso controladorComp = null;

        public CompromissoTest()
        {
            controladorCont = new ControladorContato();
            controladorComp = new ControladorCompromisso();
            Db.Update("DELETE FROM [TBCOMPROMISSO]");
            Db.Update("DELETE FROM [TBCONTATO]");
        }

        [TestMethod]
        public void DeveInserirCompromisso()
        {
            //arrange
            Contato cont1 = new Contato("Rech", "rech@gmail.com", "49991371647", "Rech Technologies", "CEO");
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), cont1);
            controladorCont.InserirNovo(cont1);

            //action
            controladorComp.InserirNovo(c1);

            //assert
            Compromisso compromissoInserido = controladorComp.SelecionarPorId(c1.Id);
            compromissoInserido.ToString().Should().Be(c1.ToString());
        }

        [TestMethod]
        public void DeveInserirCompromissoSemContato()
        {
            //arrange
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            
            //action
            controladorComp.InserirNovo(c1);

            //assert
            Compromisso compromissoInserido = controladorComp.SelecionarPorId(c1.Id);
            compromissoInserido.Contato.Should().BeNull();
        }

        [TestMethod]
        public void DeveExcluirCompromisso()
        {
            //arrange            
            Contato cont1 = new Contato("Rech", "rech@gmail.com", "49991371647", "Rech Technologies", "CEO");
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), cont1);
            controladorCont.InserirNovo(cont1);
            controladorComp.InserirNovo(c1);

            //action        
            controladorComp.Excluir(c1.Id);

            //assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => controladorComp.SelecionarPorId(c1.Id));
        }

        [TestMethod]
        public void DeveEditarCompromisso()
        {
            //arrange 
            Contato cont1 = new Contato("Rech", "rech@gmail.com", "49991371647", "Rech Technologies", "CEO");
            Compromisso cNormal = new Compromisso("NORMAL", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), cont1);
            controladorComp.InserirNovo(cNormal);
            Compromisso cEditado = new Compromisso("EDITADO", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), cont1);

            controladorComp.Editar(cNormal.Id, cEditado);

            Compromisso compromissoSelecionado = controladorComp.SelecionarPorId(cNormal.Id);
            compromissoSelecionado.Assunto.Should().Be("EDITADO");
        }

        [TestMethod]
        public void DeveAdicionarContatoNoCompromisso()
        {
            Contato cont1 = new Contato("Rech", "rech@gmail.com", "49991371647", "Rech Technologies", "CEO");
            controladorCont.InserirNovo(cont1);
            Compromisso cSemContato = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            controladorComp.InserirNovo(cSemContato);
            Compromisso cComContato = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), cont1);
            controladorComp.Editar(cSemContato.Id, cComContato);

            Compromisso compromissoSelecionado = controladorComp.SelecionarPorId(cSemContato.Id);
            compromissoSelecionado.Contato.Nome.Should().Be("Alexandre Rech");
        }

        [TestMethod]
        public void DeveBuscarTodosCompromissos()
        {
            //arrange   
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 4, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c2 = new Compromisso("Palestra", "Meet", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 1, 8, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c3 = new Compromisso("Atividade", "Casa", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 7, 10, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);

            controladorComp.InserirNovo(c1);
            controladorComp.InserirNovo(c2);
            controladorComp.InserirNovo(c3);

            //action  
            List<Compromisso> compromissos = controladorComp.SelecionarTodos();

            //assert
            compromissos.Should().HaveCount(3);
            compromissos[0].Assunto.Should().Be("Aula");
            compromissos[1].Assunto.Should().Be("Palestra");
            compromissos[2].Assunto.Should().Be("Atividade");
        }

        [TestMethod]
        public void DeveBuscarCompromissosPassados()
        {
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 4, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c2 = new Compromisso("Palestra", "Meet", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 5, 1, 8, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c3 = new Compromisso("Atividade", "Casa", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 7, 10, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);

            controladorComp.InserirNovo(c1);
            controladorComp.InserirNovo(c2);
            controladorComp.InserirNovo(c3);

            List<Compromisso> compromissos = controladorComp.SelecionarCompromissosPassados(new DateTime(2021, 6, 10, 00, 00, 00));

            compromissos.Should().HaveCount(2);
            compromissos[0].Assunto.Should().Be("Aula");
            compromissos[1].Assunto.Should().Be("Palestra");
        }

        [TestMethod]
        public void DeveBuscarCompromissosFuturosDoDia()
        {
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 6, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c2 = new Compromisso("Palestra", "Meet","meet.google.com/awc-iwhc-vbe", new DateTime(2021, 7, 2, 8, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c3 = new Compromisso("Atividade", "Casa", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 7, 2, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);

            controladorComp.InserirNovo(c1);
            controladorComp.InserirNovo(c2);
            controladorComp.InserirNovo(c3);

            List<Compromisso> compromissos = controladorComp.SelecionarCompromissosFuturos(new DateTime(2021, 7, 2, 00, 00, 00), new DateTime(2021, 7, 2, 23, 59, 59));

            compromissos.Should().HaveCount(2);
            compromissos[0].Assunto.Should().Be("Palestra");
            compromissos[1].Assunto.Should().Be("Atividade");
        }

        [TestMethod]
        public void DeveBuscarCompromissosFuturosDoMes()
        {
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 6, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c2 = new Compromisso("Palestra", "Meet", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 7, 5, 8, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            Compromisso c3 = new Compromisso("Atividade", "Casa", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 7, 5, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);

            controladorComp.InserirNovo(c1);
            controladorComp.InserirNovo(c2);
            controladorComp.InserirNovo(c3);

            List<Compromisso> compromissos = controladorComp.SelecionarCompromissosFuturos(new DateTime(2021, 7, 2, 00, 00, 00), new DateTime(2021, 7, 30, 23, 59, 59));

            compromissos.Should().HaveCount(2);
            compromissos[0].Assunto.Should().Be("Palestra");
            compromissos[1].Assunto.Should().Be("Atividade");
        }

        [TestMethod]
        public void DeveBuscarCompromissosPorId()
        {
            Compromisso c1 = new Compromisso("Aula", "NDD", "meet.google.com/awc-iwhc-vbe", new DateTime(2021, 6, 29, 10, 00, 00), new TimeSpan(13, 30, 00), new TimeSpan(17, 30, 00), null);
            controladorComp.InserirNovo(c1);

            Compromisso compromissoSelecionado = controladorComp.SelecionarPorId(c1.Id);
            compromissoSelecionado.Assunto.Should().Be("Aula");
        }
    }
}
