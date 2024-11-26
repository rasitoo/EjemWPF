using System.Windows.Input;

namespace WpfAppText;

//Se crea el comando en su propia clase
public static class CustomCommands
{
    public static readonly RoutedUICommand FullScreen = new RoutedUICommand(
        "Pantalla Completa",
        "FullScreen",
        typeof(CustomCommands),
        new InputGestureCollection
        {
            new KeyGesture(Key.Enter, ModifierKeys.Alt) // Para la entrada: Alt + Enter
        }
    );
}