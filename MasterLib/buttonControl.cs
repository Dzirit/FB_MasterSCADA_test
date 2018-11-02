using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using FB;
using FB.VisualFB;

namespace MasterLib
{
    [ComVisible(true),
     Guid("AA06D670-F54C-45F7-B6F6-D12C408292ED")]

    public partial class buttonControl : VisualControlBase
    {
        public buttonControl()
        {
            InitializeComponent();
        }

        //объявление локальных переменных
        bool mouseButtonPressed;
        bool buttonRun;

        //список свойств контрола изменяемых в MasterSCADA
        //-реакция на нажатие
        //-источник задания сотояния внешнего вида
        //-цвета и тексты в разных состояниях
        //-шрифт
        
        bool _mode = false;
        [DispId(1),
            DisplayName("Обработка нажатия"),
            Description("FALSE - реверс, TRUE - моментальное"),
            Category("Поведение")]

        public bool Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        bool _externalRun = false;
        [DispId(2),
            DisplayName("Внешний источник состояния"),
            Description("Используется внешний источник состояния(внешнего вида) кнопки?"),
            Category("Поведение")]

        public bool ExternalRun
        {
            get { return _externalRun; }
            set { _externalRun = value; }
        }

        Color _colorTrue = Color.IndianRed;
        [DispId(3),
            DisplayName("Правдивый цвет"),
            Description("Цвет кнопки с состоянием TRUE"),
            Category("Внешний вид")]

        public Color ColorTrue
        {
            get { return _colorTrue; }
            set { _colorTrue = value; }
        }

        Color _colorFalse = Color.YellowGreen;
        [DispId(4),
            DisplayName("Ложный цвет"),
            Description("Цвет кнопки с состоянием FALSE"),
            Category("Внешний вид")]

        public Color ColorFalse
        {
            get { return _colorFalse; }
            set { _colorFalse = value; }
        }

        string _textTrue = "Стоп";
        [DispId(5),
            DisplayName("Правдивый текст"),
            Description("Текст кнопки с состоянием TRUE"),
            Category("Внешний вид")]

        public string TextTrue
        {
            get { return _textTrue; }
            set { _textTrue = value; }
        }

        string _textFalse = "Пуск";
        [DispId(6),
            DisplayName("Ложный текст"),
            Description("Текст кнопки с состоянием FALSE"),
            Category("Внешний вид")]

        public string TextFalse
        {
            get { return _textFalse; }
            set { _textFalse = value; }
        }

        Font _textFont = new Font("Calibri Light", 28);
        [DispId(7),
            DisplayName("Шрифт"),
            Description("Параметры шрифта"),
            Category("Внешний вид")]

        public Font TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }
        //обработка нажатия кнопок
        private void button1_Click(object sender, EventArgs e)
        {
            if (!_mode)
            {
                buttonRun = !buttonRun;
                FBConnector.SetPinValue(Button.VisuOUT, !FBConnector.GetPinBool(Button.VisuOUT));

            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) mouseButtonPressed = true;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) mouseButtonPressed = false;
        }
        //обработка событий по таймеру
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FBConnector.DesignMode) return;

            if((_externalRun && FBConnector.GetPinBool(Button.VisuRUN) || (!_externalRun && buttonRun)))
            {
                button1.BackColor = _colorTrue;
                button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(_colorTrue.A - 22, _colorTrue);
                button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(_colorTrue.A - 44, _colorTrue);
                button1.Text = _textTrue;
            }
            else
            {
                button1.BackColor = _colorFalse;
                button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(_colorTrue.A - 22, _colorFalse);
                button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(_colorTrue.A - 44, _colorFalse);
                button1.Text = _textFalse;
            }
            if (FBConnector.GetPinBool(Button.VisuBLOCK)) button1.BackColor = Color.Silver;
            button1.Enabled = !FBConnector.GetPinBool(Button.VisuBLOCK);

            if (_mode)
            {
                FBConnector.SetValue<bool>(Button.VisuOUT, mouseButtonPressed);
                buttonRun = mouseButtonPressed;
            }
        }
    }
}
