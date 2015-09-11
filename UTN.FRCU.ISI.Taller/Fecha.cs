using System;

namespace UTN.FRCU.ISI.Taller
{
    /// <summary>
    /// Representa una fecha entre el 01/01/1900 y el 31/12/2499.
    /// </summary>
    public class Fecha
    {

        /// <summary>
        /// Año base a partir del cual se representan las fechas, tomando el 01 de enero de dicho año.
        /// </summary>
        private const int AÑO_BASE = 1900;

        /// <summary>
        /// Año máximo que soporta la clase. Va hasta el 31 de diciembre.
        /// </summary>
        private const int AÑO_MAXIMO = 2499;

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
        /// Cantidad de días transcurridos desde la fecha base.
        /// </summary>
        private readonly long iCantidadDias;

        /// <summary>
        /// Componente día de la fecha.
        /// </summary>
        private readonly int iDia;

        /// <summary>
        /// Componente mes de la fecha.
        /// </summary>
        private readonly int iMes;

        /// <summary>
        /// Componente año de la fecha.
        /// </summary>
        private readonly int iAño;

        /// <summary>
        /// Constructor que recibe los tres componentes de la fecha.
        /// </summary>
        /// <param name="pDia">Componente día.</param>
        /// <param name="pMes">Componente mes.</param>
        /// <param name="pAño">Componente año.</param>
        /// <exception cref="ArgumentException">Si <paramref name="pDia"/> no está comprendido entre 1 y la cantidad de días máximos del <paramref name="pMes"/> y <paramref name="pAño"/>.</exception>
        /// <exception cref="ArgumentException">Si <paramref name="pMes"/> no está comprendido entre 1 y 12.</exception>
        /// <exception cref="ArgumentException">Si <paramref name="pAño"/> no está comprendido entre 1900 y 2499.</exception>
        public Fecha(int pDia, int pMes, int pAño)
        {

            if (pAño < AÑO_BASE || pAño > AÑO_MAXIMO)
            {
                throw new ArgumentException(String.Format("El año debe estar comprendido entre {0} y {1}.", AÑO_BASE, AÑO_MAXIMO));
            }

            if (pMes < 1 || pMes > 12)
            {
                throw new ArgumentException("El mes debe estar comprendido entre 1 y 12.");
            }

            if (pDia < 1 || pDia > Fecha.CalcularCantidadDiasMes(pMes, pAño))
            {
                throw new ArgumentException("El día debe estar comprendido entre 1 y la cantidad máxima de días para el mes y año proporcionados.");
            }

            this.iDia = pDia;
            this.iMes = pMes;
            this.iAño = pAño;

            // Por lo menos, desde la fecha base han pasado la cantidad de días
            // del componente día de la fecha.
            long mCantidadDias = pDia;

            // Se calculan la cantidad de días de los meses transcurridos desde enero hasta
            // el mes anterior al componente mes provisto, ya que dicho mes no está
            // completo y sus días ya fueron considerados.
            for (int bMes = 1; bMes < pMes; bMes++)
            {
                mCantidadDias += Fecha.CalcularCantidadDiasMes(bMes, pAño);
            }

            // Se calculan la cantidad de días transcurridos desde el año base
            // hasta el año anterior al del componente año, ya que se han sumado
            // los días y meses de dicho año.
            for (int bAño = AÑO_BASE; bAño < pAño; bAño++)
            {
                mCantidadDias += Fecha.CalcularCantidadDiasAño(bAño);
            }

            this.iCantidadDias = mCantidadDias;
        }

        /// <summary>
        /// Constructor interno al que se le proporcionan la cantidad de días.
        /// </summary>
        /// <param name="pCantidadDias">Cantidad de días.</param>
        /// <exception cref="ArgumentException">Si <paramref name="pCantidadDias"/> no está comprendido entre 1 y 219.146.</exception>
        private Fecha(long pCantidadDias)
        {

            if (pCantidadDias < 1 || pCantidadDias > 219146)
            {
                throw new ArgumentException("La cantidad de días debe estar comprendida entre 1 y 219.146.");
            }

            this.iCantidadDias = pCantidadDias;

            int mAñoActual = AÑO_BASE;

            int mCantidadDiasAñoActual = Fecha.CalcularCantidadDiasAño(mAñoActual);

            // Se van sumando años al año base siempre y cuando la cantidad de días
            // sea suficiente para el año que se quiera sumar.
            while (pCantidadDias > mCantidadDiasAñoActual)
            {
                mAñoActual++;
                pCantidadDias -= mCantidadDiasAñoActual;
                mCantidadDiasAñoActual = Fecha.CalcularCantidadDiasAño(mAñoActual);
            }

            this.iAño = mAñoActual;

            int mMesActual = 1;

            int mCantidadDiasMesActual = Fecha.CalcularCantidadDiasMes(mMesActual, this.iAño);

            // Se van sumando meses siempre y cuando haya suficientes días para
            // el mes que se esté tratando.
            while (pCantidadDias > mCantidadDiasMesActual)
            {
                mMesActual++;
                pCantidadDias -= mCantidadDiasMesActual;
                mCantidadDiasMesActual = Fecha.CalcularCantidadDiasMes(mMesActual, this.iAño);
            }

            this.iMes = mMesActual;

            // El componente día es lo que quedó de quitar los días transcurridos
            // en los años y en los meses.
            this.iDia = (int)pCantidadDias;
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
            if (pMes == 2 && !Fecha.EsBisiesto(pAño))
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

            if (Fecha.EsBisiesto(pAño))
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
            return (pAño % 4 == 0 && pAño % 100 != 0) || pAño % 400 == 0;
        }

        /// <summary>
        /// Agrega la cantidad de días indicados como parámetro a la fecha, devolviendo el resultado en una nueva instancia de la clase.
        /// </summary>
        /// <param name="pCantidadDias">Cantidad de días a agregar.</param>
        /// <returns>Resultado de agregar a la instancia la cantidad de días indicada en <paramref name="pCantidadDias"/>.</returns>
        public Fecha AgregarDias(int pCantidadDias)
        {
            return new Fecha(this.iCantidadDias + pCantidadDias);
        }

        /// <summary>
        /// Agrega la cantidad de meses indicados como parámetro a la fecha, devolviendo el resultado en una nueva instancia de la clase.
        /// </summary>
        /// <param name="pCantidadMeses">Cantidad de meses a agregar.</param>
        /// <returns>Resultado de agregar a la instancia la cantidad de meses indicada en <paramref name="pCantidadMeses"/>.</returns>
        public Fecha AgregarMeses(int pCantidadMeses)
        {

            int mCantidadDias = 0;

            int mMesActual = this.iMes;
            int mAñoActual = this.iAño;

            for (int bIndice = 1; bIndice <= pCantidadMeses; bIndice++)
            {

                // En la próxima iteración luego de diciembre, vuelvo a enero e
                // incremento el año.
                if (mMesActual == 12)
                {
                    mMesActual = 1;
                    mAñoActual++;
                }
                else
                {
                    mMesActual++;
                }

                mCantidadDias += Fecha.CalcularCantidadDiasMes(mMesActual, mAñoActual);
            }

            return new Fecha(this.iCantidadDias + mCantidadDias);
        }

        /// <summary>
        /// Agrega la cantidad de años indicados como parámetro a la fecha, devolviendo el resultado en una nueva instancia de la clase.
        /// </summary>
        /// <param name="pAños">Cantidad de años a agregar.</param>
        /// <returns>Resultado de agregar a la instancia la cantidad de años indicada en <paramref name="pAños"/>.</returns>
        public Fecha AgregarAños(int pAños)
        {
            int mCantidadDias = 0;

            for (int bIndice = 1; bIndice <= pAños; bIndice++)
            {

                mCantidadDias += Fecha.CalcularCantidadDiasAño(this.iAño + bIndice);
            }

            return new Fecha(this.iCantidadDias + mCantidadDias);
        }

        /// <summary>
        /// Indica si la fecha pertenece a un año bisiesto.
        /// </summary>
        /// <returns>true si la fecha pertenece a un año bisiesto, false en caso contrario.</returns>
        public bool EsBisiesto()
        {
            return Fecha.EsBisiesto(this.iAño);
        }

        /// <summary>
        /// Día de la semana de la fecha.
        /// </summary>
        /// <returns></returns>
        public String DiaSemana
        {
            get
            {
                // Se puede encontrar la teoría en http://www.zurditorium.com/como-calcular-que-dia-de-la-semana-era-tal-fecha.

                // Se extraen los primeros dos dígitos del año para calcular el siglo,
                // y se le restan 20 unidades para obtener el índice en la que está
                // la clave en el array correspondiente.
                int mClaveSiglo = CLAVE_SIGLO[Convert.ToInt32(Convert.ToString(this.iAño).Substring(0, 2)) - 19];

                // Se obtienen los dos últimos dígitos del año.
                int mAño = Convert.ToInt32(Convert.ToString(this.iAño).Substring(2, 2));

                // Para obtener el índice del día de la semana, se utiliza la fórmula
                // N = (D + M + A + [A/4] + S) mod 7, donde N es el índice del día de la
                // semana, D es el día, M es la clave del mes, A son los últimos dos
                // dígitos del año y S es la clave del siglo.
                int mDiaSemana = (int)((this.iDia + CLAVE_MES[this.iMes - 1] + mAño + (mAño / 4) + mClaveSiglo) % 7);

                return DIA_SEMANA[mDiaSemana];
            }
        }

        /// <summary>
        /// Componente día de la fecha.
        /// </summary>
        public int Dia
        {
            get { return this.iDia; }
        }

        /// <summary>
        /// Componente mes de la fecha.
        /// </summary>
        public int Mes
        {
            get { return this.iMes; }
        }

        /// <summary>
        /// Componente año de la fecha.
        /// </summary>
        public int Año
        {
            get { return this.iAño; }
        }

        /// <summary>
        /// Compara la fecha con la provista como parámetro.
        /// </summary>
        /// <param name="pFecha">Fecha con la que se quiere comparar.</param>
        /// <returns>-1 si la fecha es menor que la fecha provista, 0 si son iguales, y 1 si la fecha es mayor a la fecha proporcionada como parámetro.</returns>
        /// <exception cref="ArgumentNullException">Si <paramref name="pFecha"/> es nula.</exception>
        public int CompararFecha(Fecha pFecha)
        {

            if (pFecha == null)
            {
                throw new ArgumentNullException("La fecha proporcionada es nula.");
            }

            int mResultado;

            if (this.iCantidadDias < pFecha.iCantidadDias)
            {
                mResultado = -1;
            }
            else if (this.iCantidadDias == pFecha.iCantidadDias)
            {
                mResultado = 0;
            }
            else
            {
                mResultado = 1;
            }

            return mResultado;
        }

        /// <summary>
        /// Devuelve una fecha que es el resultado de restar la fecha con otra provista como parámetro.
        /// </summary>
        /// <param name="pFecha">Fecha que se quiere restar.</param>
        /// <returns>Cantidad de días de diferencia entre las dos fechas.</returns>
        /// <exception cref="ArgumentNullException">Si <paramref name="pFecha"/> es nula.</exception>
        public long DiferenciaFecha(Fecha pFecha)
        {

            if (pFecha == null)
            {
                throw new ArgumentNullException("La fecha proporcionada es nula.");
            }

            return Math.Abs(this.iCantidadDias - pFecha.iCantidadDias);
        }

        public override String ToString()
        {
            return String.Format("{0}/{1}/{2}", this.iDia.ToString("D2"), this.iMes.ToString("D2"), this.iAño);
        }

    }
}
