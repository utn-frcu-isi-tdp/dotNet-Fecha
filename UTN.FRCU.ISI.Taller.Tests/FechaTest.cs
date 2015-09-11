using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UTN.FRCU.ISI.Taller.Tests
{
    [TestClass]
    public class FechaTest
    {
        [TestMethod]
        public void EsBisiestoTest()
        {
            Fecha fecha = new Fecha(10, 9, 2000);

            Assert.AreEqual(fecha.EsBisiesto(), true);
        }

        [TestMethod]
        public void AgregarDiasTest()
        {
            Fecha fecha = new Fecha(1, 3, 2005);

            fecha = fecha.AgregarDias(60);

            Fecha otraFecha = new Fecha(30, 4, 2005);

            Assert.AreEqual(fecha.CompararFecha(otraFecha), 0);
        }

        [TestMethod]
        public void AgregarMesesTest()
        {
            Fecha fecha = new Fecha(22, 10, 2008);

            fecha = fecha.AgregarMeses(12);

            Fecha otraFecha = new Fecha(22, 10, 2009);

            Assert.AreEqual(fecha.CompararFecha(otraFecha), 0);
        }

        [TestMethod]
        public void AgregarAñosTest()
        {
            Fecha fecha = new Fecha(1, 12, 1985);

            fecha = fecha.AgregarAños(29);

            Fecha otraFecha = new Fecha(1, 12, 2014);

            Assert.AreEqual(fecha.CompararFecha(otraFecha), 0);
        }

        [TestMethod]
        public void MenorQueUnaFechaTest()
        {
            Fecha fecha = new Fecha(1, 3, 2005);

            Fecha otraFecha = new Fecha(25, 4, 2010);

            Assert.AreEqual(fecha.CompararFecha(otraFecha), -1);
        }

        [TestMethod]
        public void MayorQueUnaFecha()
        {
            Fecha fecha = new Fecha(26, 1, 2014);

            Fecha otraFecha = new Fecha(21, 6, 2008);

            Assert.AreEqual(fecha.CompararFecha(otraFecha), 1);
        }

        [TestMethod]
        public void DiaDeLaSemanaTest()
        {
            Fecha fecha = new Fecha(1, 10, 2014);

            Assert.AreEqual(fecha.DiaSemana, "Miércoles");
        }

        [TestMethod]
        public void DiferenciaFechasTest()
        {
            Assert.AreEqual(new Fecha(5, 3, 2013).DiferenciaFecha(new Fecha(2, 8, 2000)), 4598);
        }
    }
}
