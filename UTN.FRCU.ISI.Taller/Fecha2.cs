using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTN.FRCU.ISI.Taller
{
    /// <summary>
    /// Representa una fecha.
    /// </summary>
    public class Fecha2
    {        
        
        /// <summary>
        /// Cantidad de días por mes, donde la posición 0 del array se corresponde a enero, la 1 a febrero y
        /// así sucesivamente. Para febrero se consideran la cantidad de días de los años bisiestos.
        /// </summary>
        private static readonly int[] CANTIDAD_DIAS_MES = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// Clave del mes para calcular el día de la semana. La posición 0 del array se corresponde con enero,
        /// la 1 con febrero y así sucesivamente.
        /// </summary>
        private static readonly int[] CLAVE_MES = { 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5 };

        /// <summary>
        /// Clave del siglo para calcular el día de la semana. La posición 0 del array se corresponde con el siglo XX,
        /// la posición 1 con el siglo XXI y así sucesivamente hasta el siglo XXV inclusive.
        /// </summary>
        private static readonly int[] CLAVE_SIGLO = { 0, 6, 4, 2, 0, 6 };

        /// <summary>
        /// Nombres de días de la semana.
        /// </summary>
        private static readonly String[] DIA_SEMANA = { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

        /// <summary>
        /// Representación interna de la fecha utilizando <see cref="System.DateTime"/>.
        /// </summary>
        private readonly DateTime iFecha;

        /// <summary>
        /// Constructor que recibe los tres componentes de la fecha.
        /// </summary>
        /// <param name="pDia">Componente día.</param>
        /// <param name="pMes">Componente mes.</param>
        /// <param name="pAño">Componente año.</param>
        /// <exception cref="ArgumentException">Si <paramref name="pDia"/> no está comprendido entre 1 y la cantidad de días máximos del <paramref name="pMes"/> y <paramref name="pAño"/>.</exception>
        /// <exception cref="ArgumentException">Si <paramref name="pMes"/> no está comprendido entre 1 y 12.</exception>
        /// <exception cref="ArgumentException">Si <paramref name="pAño"/> no está comprendido entre 1900 y 2499.</exception>
        public Fecha2(int pDia, int pMes, int pAño)
        {
            if (pMes < 1 || pMes > 12)
            {
                throw new ArgumentException("El mes debe estar comprendido entre 1 y 12.");
            }

            if (pDia < 1 || pDia > Fecha2.CalcularCantidadDiasMes(pMes, pAño))
            {
                throw new ArgumentException("El día debe estar comprendido entre 1 y la cantidad máxima de días para el mes y año proporcionados.");
            }

            this.iFecha = new DateTime(pAño, pMes, pDia);
        }
        

        /// <summary>
        /// Calcula la cantidad de días para un mes y año determinados.
        /// </summary>
        /// <param name="pMes">Mes para el que se desea calcular la cantidad de días.</param>
        /// <param name="pAño">Año para el que se desea calcular la cantidad de días.</param>
        /// <returns>Cantidad de días para el mes <paramref name="pMes"/> y el año <paramref name="pAño"/>.</returns>
        private static int CalcularCantidadDiasMes(int pMes, int pAño)
        {
            int mCantidadDias = CANTIDAD_DIAS_MES[pMes - 1];

            // Si el mes es febrero y el año no es bisiesto entonces debo quitarle
            // una unidad al número recuperado.
            if (pMes == 2 && !Fecha2.EsBisiesto(pAño))
            {
                mCantidadDias -= 1;
            }

            return mCantidadDias;
        }

        /// <summary>
        /// Devuelve la cantidad de días que tiene un año.
        /// </summary>
        /// <param name="pAño">Año para el cual se desean obtener la cantidad de días.</param>
        /// <returns>Cantidad de días que posee el año <paramref name="pAño"/>.</returns>
        private static int CalcularCantidadDiasAño(int pAño)
        {
            int mCantidadDias = 365;

            if (Fecha2.EsBisiesto(pAño))
            {
                mCantidadDias++;
            }

            return mCantidadDias;
        }

        /// <summary>
        /// Indica si un año es bisiesto.
        /// </summary>
        /// <param name="pAño">Año que se quiere determinar si es bisiesto.</param>
        /// <returns>true si <paramref name="pAño"/> es bisiesto, false en caso contrario.</returns>
        private static bool EsBisiesto(int pAño)
        {
            return DateTime.IsLeapYear(pAño);
        }

        /// <summary>
        /// Agrega la cantidad de días indicados como parámetro a la fecha, devolviendo el resultado en una nueva instancia de la clase.
        /// </summary>
        /// <param name="pCantidadDias">Cantidad de días a agregar.</param>
        /// <returns>Resultado de agregar a la instancia la cantidad de días indicada en <paramref name="pCantidadDias"/>.</returns>
        public Fecha2 AgregarDias(int pCantidadDias)
        {
            DateTime mNuevaFecha = this.iFecha.AddDays(pCantidadDias);
            return new Fecha2(mNuevaFecha.Day, mNuevaFecha.Month, mNuevaFecha.Year);
        }

        /// <summary>
        /// Agrega la cantidad de meses indicados como parámetro a la fecha, devolviendo el resultado en una nueva instancia de la clase.
        /// </summary>
        /// <param name="pCantidadMeses">Cantidad de meses a agregar.</param>
        /// <returns>Resultado de agregar a la instancia la cantidad de meses indicada en <paramref name="pCantidadMeses"/>.</returns>
        public Fecha2 AgregarMeses(int pCantidadMeses)
        {
            DateTime mNuevaFecha = this.iFecha.AddMonths(pCantidadMeses);
            return new Fecha2(mNuevaFecha.Day, mNuevaFecha.Month, mNuevaFecha.Year);
        }

        /// <summary>
        /// Agrega la cantidad de años indicados como parámetro a la fecha, devolviendo el resultado en una nueva instancia de la clase.
        /// </summary>
        /// <param name="pAños">Cantidad de años a agregar.</param>
        /// <returns>Resultado de agregar a la instancia la cantidad de años indicada en <paramref name="pAños"/>.</returns>
        public Fecha2 AgregarAños(int pAños)
        {
            DateTime mNuevaFecha = this.iFecha.AddYears(pAños);
            return new Fecha2(mNuevaFecha.Day, mNuevaFecha.Month, mNuevaFecha.Year);
        }

        /// <summary>
        /// Indica si la fecha pertenece a un año bisiesto.
        /// </summary>
        /// <returns>true si la fecha pertenece a un año bisiesto, false en caso contrario.</returns>
        public bool EsBisiesto()
        {
            return Fecha2.EsBisiesto(this.iFecha.Year);
        }

        /// <summary>
        /// Día de la semana de la fecha.
        /// </summary>
        /// <returns>Nombre del día de la semana correspondiente a la fecha.</returns>
        public String DiaSemana
        {
            get
            {
                // Se puede encontrar la teoría en http://www.zurditorium.com/como-calcular-que-dia-de-la-semana-era-tal-fecha.

                // Se extraen los primeros dos dígitos del año para calcular el siglo,
                // y se le restan 20 unidades para obtener el índice en la que está
                // la clave en el array correspondiente.
                int mClaveSiglo = CLAVE_SIGLO[Convert.ToInt32(Convert.ToString(this.iFecha.Year).Substring(0, 2)) - 19];

                // Se obtienen los dos últimos dígitos del año.
                int mAño = Convert.ToInt32(Convert.ToString(this.iFecha.Year).Substring(2, 2));

                // Para obtener el índice del día de la semana, se utiliza la fórmula
                // N = (D + M + A + [A/4] + S) mod 7, donde N es el índice del día de la
                // semana, D es el día, M es la clave del mes, A son los últimos dos
                // dígitos del año y S es la clave del siglo.
                int mDiaSemana = (int)((this.iFecha.Day + CLAVE_MES[this.iFecha.Month - 1] + mAño + (mAño / 4) + mClaveSiglo) % 7);

                return DIA_SEMANA[mDiaSemana];
            }
        }

        /// <summary>
        /// Componente día de la fecha.
        /// </summary>
        public int Dia
        {
            get { return this.iFecha.Day; }
        }

        /// <summary>
        /// Componente mes de la fecha.
        /// </summary>
        public int Mes
        {
            get { return this.iFecha.Month; }
        }

        /// <summary>
        /// Componente año de la fecha.
        /// </summary>
        public int Año
        {
            get { return this.iFecha.Year; }
        }

        /// <summary>
        /// Compara la fecha con la provista como parámetro.
        /// </summary>
        /// <param name="pFecha">Fecha con la que se quiere comparar.</param>
        /// <returns>-1 si la fecha es menor que la fecha provista, 0 si son iguales, y 1 si la fecha es mayor a la fecha proporcionada como parámetro.</returns>
        /// <exception cref="ArgumentNullException">Si <paramref name="pFecha"/> es nula.</exception>
        public int CompararFecha(Fecha2 pFecha)
        {

            if (pFecha == null)
            {
                throw new ArgumentNullException("La fecha proporcionada es nula.");
            }

            return this.iFecha.CompareTo(pFecha.iFecha);
        }

        /// <summary>
        /// Devuelve una fecha que es el resultado de restar la fecha con otra provista como parámetro.
        /// </summary>
        /// <param name="pFecha">Fecha que se quiere restar.</param>
        /// <returns>Cantidad de días de diferencia entre las dos fechas.</returns>
        /// <exception cref="ArgumentNullException">Si <paramref name="pFecha"/> es nula.</exception>
        public long DiferenciaFecha(Fecha2 pFecha)
        {

            if (pFecha == null)
            {
                throw new ArgumentNullException("La fecha proporcionada es nula.");
            }

            return Math.Abs((this.iFecha - pFecha.iFecha).Days);
        }

        public override String ToString()
        {
            return this.iFecha.ToString("dd/MM/YYYY");
        }

    }
}
