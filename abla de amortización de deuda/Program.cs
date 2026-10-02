using Spectre.Console;

class Program
{
    static void Main()
    {
        Console.WriteLine("==================================");
        Console.WriteLine("   TABLA DE AMORTIZACIÓN");
        Console.WriteLine("==================================");
        Console.WriteLine();

        // Datos del préstamo
        decimal monto = AnsiConsole.Ask<decimal>(
            "Monto del préstamo: ");

        decimal tasaAnual = AnsiConsole.Ask<decimal>(
            "Tasa de interés anual (%): ");

        int plazo = AnsiConsole.Ask<int>(
            "Plazo del préstamo (meses): ");

        // Calcular tasa mensual
        decimal tasaMensual = (tasaAnual / 12) / 100;

        // Calcular cuota fija
        decimal cuota;

        if (tasaMensual == 0)
        {
            cuota = monto / plazo;
        }
        else
        {
            double i = (double)tasaMensual;
            double potencia = Math.Pow(1 + i, plazo);

            cuota = monto * tasaMensual *
                    (decimal)(potencia / (potencia - 1));
        }

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine("DATOS DEL PRÉSTAMO");
        Console.WriteLine("==================================");
        Console.WriteLine($"Monto:        {monto:C2}");
        Console.WriteLine($"Tasa anual:   {tasaAnual:F2}%");
        Console.WriteLine($"Tasa mensual: {tasaMensual:P4}");
        Console.WriteLine($"Plazo:        {plazo} meses");
        Console.WriteLine($"Cuota fija:   {cuota:C2}");
        Console.WriteLine();

        // Crear tabla
        var tabla = new Table();

        tabla.AddColumn("Cuota");
        tabla.AddColumn("Pago");
        tabla.AddColumn("Interés");
        tabla.AddColumn("Capital");
        tabla.AddColumn("Saldo");

        decimal saldo = monto;

        // Calcular cada cuota
        for (int numero = 1; numero <= plazo; numero++)
        {
            // Interés del período
            decimal interes = saldo * tasaMensual;

            // Abono a capital
            decimal abonoCapital = cuota - interes;

            // Ajustar la última cuota
            if (numero == plazo)
            {
                abonoCapital = saldo;
                cuota = abonoCapital + interes;
            }

            // Calcular nuevo saldo
            saldo = saldo - abonoCapital;

            if (saldo < 0)
            {
                saldo = 0;
            }

            tabla.AddRow(
                numero.ToString(),
                cuota.ToString("C2"),
                interes.ToString("C2"),
                abonoCapital.ToString("C2"),
                saldo.ToString("C2")
            );
        }

        AnsiConsole.Write(tabla);

        Console.WriteLine();
        Console.WriteLine("Proceso terminado.");
        Console.ReadKey();
    }
}