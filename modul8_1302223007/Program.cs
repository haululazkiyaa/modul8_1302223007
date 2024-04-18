using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        int tf_amount = 0;
        int fee_amount = 0;
        int total_amount = 0;
        int tf_methods = 0;
        string confirm;

        ReadWriteConfig rwconf = new ReadWriteConfig();

        bool isLangEn = rwconf.config.lang == "en";

        if (isLangEn)
        {
            Console.Write("Please insert the amount of money to transfer: ");
        } else
        {
            Console.Write("Masukkan jumlah uang yang akan di-transfer: ");
        }
        tf_amount = Convert.ToInt32(Console.ReadLine());

        if (tf_amount <= rwconf.config.transfer.threshold )
        {
            fee_amount = rwconf.config.transfer.low_fee;
        } else
        {
            fee_amount = rwconf.config.transfer.high_fee;
        }

        total_amount = tf_amount + fee_amount;

        if (isLangEn)
        {
            Console.Write($"Transfer fee\t= {fee_amount}\nTotal amount\t= {total_amount}\n");
        } else
        {
            Console.Write($"Biaya transfer\t= {fee_amount}\nTotal biaya\t= {total_amount}\n");
        }

        void showMethods() {
            for (int i = 0; i < rwconf.config.methods.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {rwconf.config.methods[i]}");
            }
        }

        if (isLangEn)
        {
            Console.WriteLine("Select transfer methods: ");
            showMethods();
            Console.Write("Select option: ");
        }
        else
        {
            Console.WriteLine("Pilih metode transfer: ");
            showMethods();
            Console.Write("Pilih opsi: ");
        }
        tf_methods = Convert.ToInt32(Console.ReadLine());

        if (isLangEn)
        {
            Console.Write($"Please type {rwconf.config.confirmation.en} to confirm the transaction: ");
        }
        else
        {
            Console.Write($"Ketik {rwconf.config.confirmation.id} untuk mengonfirmasi transaksi: ");
        }
        confirm = Convert.ToString(Console.ReadLine());

        if (
            (isLangEn && (confirm == rwconf.config.confirmation.en)) ||
            (!isLangEn && (confirm == rwconf.config.confirmation.id))
            )
        {
            if (isLangEn)
            {
                Console.Write($"The transfer is completed");
            }
            else
            {
                Console.Write($"Proses transfer berhasil");
            }
        } else
        {
            if (isLangEn)
            {
                Console.Write($"Transfer is canceled");
            }
            else
            {
                Console.Write($"Transfer dibatalkan");
            }
        }
    }
}

class Transfer
{
    public int threshold { get; set; }
    public int low_fee { get; set; }
    public int high_fee { get; set; }

    public Transfer(int threshold, int low_fee, int high_fee)
    {
        this.threshold = threshold;
        this.low_fee = low_fee;
        this.high_fee = high_fee;
    }
}

class Confirmation
{
    public string en { get; set; }
    public string id { get; set; }

    public Confirmation(string en, string id)
    {
        this.en = en;
        this.id = id;
    }
}

class BankTransferConfig
{
    public string lang { get; set; }
    public Transfer transfer { get; set; }
    public List<string> methods { get; set; }
    public Confirmation confirmation { get; set; }

    public BankTransferConfig() { }

    public BankTransferConfig(string lang, Transfer transfer, List<string> methods, Confirmation confirmation)
    {
        this.lang = lang;
        this.transfer = transfer;
        this.methods = methods;
        this.confirmation = confirmation;
    }
}

class ReadWriteConfig
{
    public BankTransferConfig config;

    public const String filePath = "../../../banktf_config.json";

    public ReadWriteConfig()
    {
        try
        {
            ReadConfigFile();
        }
        catch (Exception)
        {
            SetDefault();
            WriteNewConfigFile();
        }
    }

    private BankTransferConfig ReadConfigFile()
    {
        String configJsonData = File.ReadAllText(filePath);
        config = JsonSerializer.Deserialize<BankTransferConfig>(configJsonData);
        return config;
    }

    private void SetDefault()
    {
        Transfer tfData = new Transfer(250000000, 6500, 15000);
        Confirmation cfData = new Confirmation("yes", "ya");
        List<string> mtData = new List<string>{"RTO(real - time)", "SKN", "RTGS", "BI FAST"};
        config = new BankTransferConfig("en", tfData, mtData, cfData);
    }

    private void WriteNewConfigFile()
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        String jsonString = JsonSerializer.Serialize(config, options);
        File.WriteAllText(filePath, jsonString);
    }
}