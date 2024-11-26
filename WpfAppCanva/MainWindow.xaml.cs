using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfAppCanva;

public partial class MainWindow : Window
{

    private Stack<Stroke> _redoStrokes = new Stack<Stroke>(); //Pila para las strokes borradas
    public MainWindow()
    {
        InitializeComponent();
    }
    private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {   //Sacado de https://stackoverflow.com/questions/6246009/inkcanvas-load-save-operations#:~:text=You%20can%20do%20this%20by%20using%20the%20StrokeCollection.Save,this%20again%20and%20have%20the%20individual%20strokes%20accessible.
        // y de https://learn.microsoft.com/en-us/windows/apps/design/input/save-and-load-ink

        //cuadro de diálogo para guardar archivo
        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
        dlg.FileName = "InkSample"; // Nombre predeterminado del archivo
        dlg.DefaultExt = ".isf"; // Extensión predeterminada
        // Mostrar el cuadro de diálogo y devolver el booleano de aceptar/cancelar
        bool? result = dlg.ShowDialog();

        // Procesar el resultado del cuadro de diálogo
        if (result == true)
        {
            // Obtener la ruta del archivo
            string filename = dlg.FileName;

            using (var fs = new FileStream(filename, FileMode.Create))
            {
                Canvas.Strokes.Save(fs);
            }
        }
    }

    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        if (Canvas.Strokes.Count > 0)
        {
            e.CanExecute = true;
        }
        else
            e.CanExecute = false;
    }
    private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {   //Sacado de https://stackoverflow.com/questions/6246009/inkcanvas-load-save-operations#:~:text=You%20can%20do%20this%20by%20using%20the%20StrokeCollection.Save,this%20again%20and%20have%20the%20individual%20strokes%20accessible.

        //cuadro de diálogo para abrir archivo
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        // Mostrar el cuadro de diálogo y devolver el booleano de aceptar/cancelar
        bool? result = dlg.ShowDialog();

        // Procesar el resultado del cuadro de diálogo
        if (result == true)
        {
            // Obtener la ruta del archivo
            string filename = dlg.FileName;

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                StrokeCollection strokes = new StrokeCollection(fs);
                Canvas.Strokes = strokes;
            }
        }
    }

    private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        MessageBox.Show("""
                        Si necesita ayuda con el funcionamiento del programa acuda a la página web

                        Versión: 0.000000000000000002 Mk7
                        Autor:   Rodrigo Tapiador Cano
                        """, "Ayuda");
    }


    private void ShowTools_Click(object sender, RoutedEventArgs e)
    {
        if (ToolPanel.Height == 0)
        {
            Storyboard sb = (Storyboard)FindResource("ExpandPanel");
            sb.Begin();
        }
        else
        {
            Storyboard sb = (Storyboard)FindResource("CollapsePanel");
            sb.Begin();
        }
    }

    private void ColorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ColorPicker.SelectedItem is ComboBoxItem selectedItem)
        {
            string color = selectedItem.Tag.ToString();
            Canvas.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(color);
        }
    }

    private void Tool_Checked(object sender, RoutedEventArgs e)
    {
        if (Canvas != null)
        {
            if (DrawTool.IsChecked == true)
            {
                Canvas.EditingMode = InkCanvasEditingMode.Ink;
            }
            else if (SelectTool.IsChecked == true)
            {
                Canvas.EditingMode = InkCanvasEditingMode.Select;
            }
        }
    }
    private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (Canvas.Strokes.Count > 0)
        {
            Stroke lastStroke = Canvas.Strokes[Canvas.Strokes.Count - 1];
            Canvas.Strokes.Remove(lastStroke);
            _redoStrokes.Push(lastStroke);
        }
    }

    private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = Canvas.Strokes.Count > 0;
    }

    private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (_redoStrokes.Count > 0)
        {
            Stroke stroke = _redoStrokes.Pop();
            Canvas.Strokes.Add(stroke);
        }
    }

    private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = _redoStrokes.Count > 0;
    }
}
