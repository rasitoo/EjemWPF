using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace WpfAppText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string stringInicial;
        public MainWindow()
        {
            InitializeComponent();
            stringInicial = StringFromRichTextBox(CuadroTexto);

        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //cuadro de diálogo para guardar archivo
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Documento"; // Nombre predeterminado del archivo
            dlg.DefaultExt = ".txt"; // Extensión predeterminada

            // Mostrar el cuadro de diálogo y devolver el booleano de aceptar/cancelar
            bool? result = dlg.ShowDialog();

            // Procesar el resultado del cuadro de diálogo
            if (result == true)
            {
                // Obtener la ruta del archivo
                string filename = dlg.FileName;

                string content = StringFromRichTextBox(CuadroTexto); // Método sacado de stackoverflow https://stackoverflow.com/questions/24127380/reading-in-lines-of-text-from-a-rich-text-box#:~:text=According%20to%20MSDN%2C%20to%20extract%20text%20from%20a,to%20the%20start%20of%20content%20in%20the%20RichTextBox.

                // Guardar el contenido en el archivo
                System.IO.File.WriteAllText(filename, content);
            }
        }
        string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
              // TextPointer to the start of content in the RichTextBox.
              rtb.Document.ContentStart,
              // TextPointer to the end of content in the RichTextBox.
              rtb.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string 
            // representing the plain text content of the TextRange. 
            return textRange.Text;
        }
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            String s = StringFromRichTextBox(CuadroTexto);
            if (!s.Equals(stringInicial))
                e.CanExecute = true; 
        }
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //cuadro de diálogo para abrir archivo
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Mostrar el cuadro de diálogo y devolver el booleano de aceptar/cancelar
            bool? result = dlg.ShowDialog();

            // Procesar el resultado del cuadro de diálogo
            if (result == true)
            {
                // Obtener la ruta del archivo
                string filename = dlg.FileName;
                // Leer el archivo
                string content = System.IO.File.ReadAllText(filename);
                //Escribir en el textBox
                StringToRichTextBox(CuadroTexto, content);
            }
        }
        void StringToRichTextBox(RichTextBox rtb, string contenido)
        {
            TextRange textRange = new TextRange(
              rtb.Document.ContentStart,
              rtb.Document.ContentEnd
            );

            textRange.Text = contenido;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("""
                            Si necesita ayuda con el funcionamiento del programa acuda a la página web

                            Versión: 0.000000000000000001 Mk2
                            Autor:   Rodrigo Tapiador Cano
                            """, "Ayuda");
        }
        private void FullScreen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
        private void FullScreen_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
                e.CanExecute = true;
        }
    }
}