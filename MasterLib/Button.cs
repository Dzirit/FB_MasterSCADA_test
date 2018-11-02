using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using FB;
using FB.VisualFB;
using InSAT.Library.Interop;

namespace MasterLib
{
    [Serializable,
              ComVisible(true),
              Guid("742CA4EB-85C2-4B65-B920-CF4570C208CB"),
              CatID(CatIDs.CATID_OTHER),
              DisplayName("УберКнопка"),
              VisualControls(typeof(buttonControl))]
    //Класс button отвечает за невизуальную часть ВФБ(представленную в окне "Объекты" MAsterSCADA
    public class Button :VisualFBBase
    {   //константы просто для удобства
        const int RUN = 1;
        const int BLOCK = 2;
        const int OUT = 3;
        public const int VisuRUN = 1;
        public const int VisuBLOCK = 2;
        public const int VisuOUT = 3;
        
        //пустое свойство для вкладки "Настройка" ВФБ в MasterSCADA
        bool _uberProp = true;
        [DispId(1),
         DisplayName("УберСвойство"),
         Description("Пустое свойство")]
        public bool UberProp
        {
            get { return _uberProp; }
            set { _uberProp = value; }
        }
        protected override void UpdateData()
        {
            if (!IsValueExistOnAllPins()) return;

            VisualPins.SetPinValue(VisuRUN, GetPinValue(RUN));
            VisualPins.SetPinValue(VisuBLOCK, GetPinValue(BLOCK));
        }
        public override void OnVisualPinChanged(int pinId)
        {
            if (pinId == VisuOUT) PinByID(OUT).Value = VisualPins.PinByID(VisuOUT).Value;
        }
    }
}
