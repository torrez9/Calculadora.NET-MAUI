namespace CalculadoraV2
{
    public partial class NewPage1 : ContentPage
    {
        private string _entradaActual = string.Empty;
        private string _operacionCompleta = string.Empty;
        private List<string> _historial = new List<string>();

        public NewPage1()
        {
            InitializeComponent();
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string textoBoton = button.Text;

                if (double.TryParse(textoBoton, out double _))
                {
                    OnNumberClicked(textoBoton);
                }
                else
                {
                    OnOperatorClicked(textoBoton);
                }
            }
        }

        private void OnNumberClicked(string numero)
        {
            _entradaActual += numero;
            _operacionCompleta += numero;
            EtiquetaResultado.Text = _operacionCompleta;
        }

        private void OnOperatorClicked(string op)
        {
            switch (op)
            {
                case "C":
                    Clear();
                    break;
                case "+/-":
                    ToggleSign();
                    break;
                case "%":
                    ApplyPercentage();
                    break;
                case ".":
                    AddDecimalPoint();
                    break;
                case "=":
                    CalculateResult();
                    break;
                case "?":
                    DeleteLastCharacter();
                    break;
                default:
                    SetOperation(op);
                    break;
            }
        }

        private void Clear()
        {
            _entradaActual = string.Empty;
            _operacionCompleta = string.Empty;
            EtiquetaResultado.Text = "0";
        }

        private void ToggleSign()
        {
            if (!string.IsNullOrEmpty(_entradaActual))
            {
                if (_entradaActual.StartsWith("-"))
                {
                    // Si ya es negativo, quita el signo negativo
                    _entradaActual = _entradaActual.Substring(1);
                    _operacionCompleta = _operacionCompleta.Substring(0, _operacionCompleta.Length - _entradaActual.Length - 1) + _entradaActual;
                }
                else
                {
                    // Si es positivo, añade el signo negativo
                    _entradaActual = "-" + _entradaActual;
                    _operacionCompleta = _operacionCompleta.Substring(0, _operacionCompleta.Length - _entradaActual.Length + 1) + _entradaActual;
                }
                EtiquetaResultado.Text = _operacionCompleta;
            }
        }

        private void ApplyPercentage()
        {
            if (double.TryParse(_entradaActual, out double numero))
            {
                string originalEntrada = _entradaActual;
                numero /= 100;
                _entradaActual = numero.ToString();
                _operacionCompleta = _operacionCompleta.Substring(0, _operacionCompleta.Length - originalEntrada.Length) + _entradaActual;
                EtiquetaResultado.Text = _operacionCompleta;
            }
        }

        private void AddDecimalPoint()
        {
            if (!_entradaActual.Contains("."))
            {
                _entradaActual += ".";
                _operacionCompleta += ".";
                EtiquetaResultado.Text = _operacionCompleta;
            }
        }

        private void SetOperation(string op)
        {
            if (!string.IsNullOrEmpty(_entradaActual))
            {
                if (_operacionCompleta.EndsWith(" ") && _operacionCompleta.Length > 2)
                {
                    _operacionCompleta = _operacionCompleta.Substring(0, _operacionCompleta.Length - 3) + $" {op} ";
                }
                else
                {
                    _operacionCompleta += $" {op} ";
                }

                _entradaActual = string.Empty;
                EtiquetaResultado.Text = _operacionCompleta;
            }
        }

        private void CalculateResult()
        {
            try
            {
                var result = new System.Data.DataTable().Compute(_operacionCompleta, null);
                _entradaActual = result.ToString();

                _historial.Add($"{_operacionCompleta} = {_entradaActual}");
                UpdateHistorial();

                _operacionCompleta = _entradaActual;
                EtiquetaResultado.Text = _entradaActual;
            }
            catch (Exception)
            {
                DisplayAlert("Error", "Error en la expresión", "OK");
                Clear();
            }
        }

        private void UpdateHistorial()
        {
            EtiquetaHistorial.Text = string.Join("\n", _historial);
        }

        private void DeleteLastCharacter()
        {
            if (!string.IsNullOrEmpty(_operacionCompleta))
            {
                _operacionCompleta = _operacionCompleta.Substring(0, _operacionCompleta.Length - 1);

                if (string.IsNullOrEmpty(_operacionCompleta))
                {
                    _operacionCompleta = "0";
                }

                EtiquetaResultado.Text = _operacionCompleta;
            }
        }

        // Método para manejar los clics en el historial (opcional)
        private void OnHistorialClicked(object sender, EventArgs e)
        {
            if (sender is Label label)
            {
                _operacionCompleta = label.Text.Split('=')[0].Trim();
                _entradaActual = string.Empty;
                EtiquetaResultado.Text = _operacionCompleta;
            }
        }
    }
}
