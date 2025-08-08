using System.Windows.Controls;
using SharpVectors.Dom;

namespace SimpleCode.Commands;

public class TabKeyPressedCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        TextBox? textBox = parameter as TextBox;

        if (textBox == null) return;

        int caretIndex = textBox.CaretIndex;
        textBox.Text = textBox.Text.Insert(caretIndex, "    ");
        textBox.CaretIndex = caretIndex + 4;
    }
}