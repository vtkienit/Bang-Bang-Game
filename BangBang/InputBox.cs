using SplashKitSDK;

public class InputBox
{
    public string Label { get; set; }
    public string Text { get; set; } = "";
    public float X, Y, Width, Height;
    public bool IsActive { get; set; } = false;
    private bool _isPassword;

    private int _cursorIndex = 0;
    private DateTime _lastBlinkTime;
    private bool _showCursor = true;

    private const string FontName = "Arial";
    private const int FontSize = 14;

    // Backspace holding
    private DateTime _backspaceStartTime;
    private bool _backspaceHeld = false;
    private TimeSpan _initialBackspaceDelay = TimeSpan.FromMilliseconds(400);
    private TimeSpan _backspaceRepeatInterval = TimeSpan.FromMilliseconds(50);
    private DateTime _lastBackspaceRepeat;

    public InputBox(string label, float x, float y, float width, float height, bool isPassword = false)
    {
        Label = label;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        _isPassword = isPassword;
        _lastBlinkTime = DateTime.Now;
    }

    public void Draw()
    {
        SplashKit.FillRectangle(Color.White, X, Y, Width, Height);
        SplashKit.DrawRectangle(Color.Black, X, Y, Width, Height);

        string displayText = _isPassword ? new string('*', Text.Length) : Text;

        SplashKit.DrawText(Label, Color.White, "RussoOne", FontSize, X, Y - 20);
        SplashKit.DrawText(displayText, Color.Black, FontName, FontSize, X + 5, Y + 10);

        // Cursor blinking
        if ((DateTime.Now - _lastBlinkTime).TotalMilliseconds > 500)
        {
            _showCursor = !_showCursor;
            _lastBlinkTime = DateTime.Now;
        }

        if (IsActive && _showCursor)
        {
            if (_cursorIndex < 0) _cursorIndex = 0;
            if (_cursorIndex > displayText.Length) _cursorIndex = displayText.Length;
            string textBeforeCursor = displayText.Substring(0, _cursorIndex);
            float cursorX = X + 5 + SplashKit.TextWidth(textBeforeCursor, FontName, FontSize);
            float cursorY = Y + 8;
            SplashKit.DrawLine(Color.Black, cursorX, cursorY, cursorX, cursorY + 24);
        }
    }

    public void HandleInput()
    {
        // Click to activate
        if (SplashKit.MouseClicked(MouseButton.LeftButton))
        {
            IsActive = SplashKit.PointInRectangle(SplashKit.MousePosition(), SplashKit.RectangleFrom(X, Y, Width, Height));
        }

        if (!IsActive) return;

        // Letters
        for (KeyCode key = KeyCode.AKey; key <= KeyCode.ZKey; key++)
        {
            if (SplashKit.KeyTyped(key))
            {
                string letter = key.ToString().Replace("Key", "");
                bool isShift = SplashKit.KeyDown(KeyCode.LeftShiftKey) || SplashKit.KeyDown(KeyCode.RightShiftKey);
                letter = isShift ? letter.ToUpper() : letter.ToLower();
                InsertAtCursor(letter);
                break;
            }
        }

        // Numbers
        for (KeyCode key = KeyCode.Num0Key; key <= KeyCode.Num9Key; key++)
        {
            if (SplashKit.KeyTyped(key))
            {
                string number = key.ToString().Replace("Num", "").Replace("Key", "");
                InsertAtCursor(number);
                break;
            }
        }

        // Space
        if (SplashKit.KeyTyped(KeyCode.SpaceKey))
            InsertAtCursor(" ");

        // Left/right arrows
        if (SplashKit.KeyTyped(KeyCode.LeftKey))
            _cursorIndex = Math.Max(0, _cursorIndex - 1);

        if (SplashKit.KeyTyped(KeyCode.RightKey))
            _cursorIndex = Math.Min(Text.Length, _cursorIndex + 1);

        // Backspace hold support
        if (SplashKit.KeyDown(KeyCode.BackspaceKey))
        {
            if (!_backspaceHeld)
            {
                _backspaceHeld = true;
                _backspaceStartTime = DateTime.Now;
                _lastBackspaceRepeat = DateTime.Now;
                PerformBackspace();
            }
            else
            {
                var timeHeld = DateTime.Now - _backspaceStartTime;
                var timeSinceLast = DateTime.Now - _lastBackspaceRepeat;

                if (timeHeld >= _initialBackspaceDelay && timeSinceLast >= _backspaceRepeatInterval)
                {
                    PerformBackspace();
                    _lastBackspaceRepeat = DateTime.Now;
                }
            }
        }
        else
        {
            _backspaceHeld = false;
        }
    }

    private void PerformBackspace()
    {
        if (_cursorIndex > 0)
        {
            Text = Text.Remove(_cursorIndex - 1, 1);
            _cursorIndex--;
        }
    }

    private void InsertAtCursor(string character)
    {
        _cursorIndex = Math.Clamp(_cursorIndex, 0, Text.Length);
        Text = Text.Insert(_cursorIndex, character);
        _cursorIndex++;
    }
}