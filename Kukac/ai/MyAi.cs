using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kukac.enums;
using Kukac.interfaces;
using System.Windows;

namespace Kukac.ai
{
    public class MyAi : Ai
    {
        Adat _adat;
        Test _kukac;
        Point kukacFej, etel;        

        public MyAi()
        {}

        public void initAdat(Adat adat)
        {
            _adat = adat;
        }
        public void initKukac(Test kukac)
        {
            _kukac = kukac;
        }
        public enum Tengely
        {
            X,
            Y
        }
        public Iranyok setIrany()
        {
            // kukac fejének pozíciója  
            kukacFej = _kukac.getFej();
            etel = _adat.getEtel();
            Iranyok kukacIrany = _kukac.getIrany();
            Iranyok joIrany = kukacIrany;

            #region Veszely elkerules
            if (kukacIrany == Iranyok.JOBB)
                joIrany = JobbIranyuKukac();
            else if (kukacIrany == Iranyok.FEL)
                joIrany = FelIranyuKukac();
            else if (kukacIrany == Iranyok.BAL)
                joIrany = BalIranyuKukac();
            else if (kukacIrany == Iranyok.LE)
                joIrany = LeIranyuKukac();
            #endregion Veszely elkerules
            return joIrany;
        }
        // A függvény visszaadja, hogy az adott koordinátán lévő pont a játék végét jelenti-e a kukac számára
        private bool IsThisPalyaElemHalaltOkoz(Point point)
        {
            if (_adat.getPalyaElem((int)point.X, (int)point.Y) == PalyaElemek.FAL ||
                _adat.getPalyaElem((int)point.X, (int)point.Y) == PalyaElemek.TEST)
                return true;
            else return false;
        }

        private Iranyok JobbIranyuKukac()
        {
            var etelIrany = MerreVanAzEtel(Tengely.X);
            if (etelIrany != null && !IsThisPalyaElemHalaltOkoz(etelIrany.Item2))
                return etelIrany.Item1;
            // ha a kukac fejétől x irányban jobbra veszély van
            else if (IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X + 1, kukacFej.Y)))
            {
                if (!IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X, kukacFej.Y - 1)))
                    return Iranyok.FEL;                
                else return Iranyok.LE;
            }
            // Ha nincs veszély akkor ugyanarra folytathatja az útját
            else return Iranyok.JOBB;
        }

        private Iranyok FelIranyuKukac()
        {
            var etelIrany = MerreVanAzEtel(Tengely.Y);
            if (etelIrany != null && !IsThisPalyaElemHalaltOkoz(etelIrany.Item2))
                return etelIrany.Item1;
            else if (IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X, kukacFej.Y - 1)))
            {
                if (!IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X + 1, kukacFej.Y)))
                    return Iranyok.JOBB;                
                else return Iranyok.BAL;                
            }
            // Ha nincs veszély akkor ugyanarra folytathatja az útját
            else return Iranyok.FEL;
        }

        private Iranyok BalIranyuKukac()
        {
            var etelIrany = MerreVanAzEtel(Tengely.X);
            if (etelIrany != null && !IsThisPalyaElemHalaltOkoz(etelIrany.Item2))
                return etelIrany.Item1;
            else if (IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X - 1, kukacFej.Y)))
            {
                if (!IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X, kukacFej.Y - 1)))
                    return Iranyok.FEL;
                else return Iranyok.LE;
            }
            // Ha nincs veszély akkor ugyanarra folytathatja az útját
            else return Iranyok.BAL;
        }

        private Iranyok LeIranyuKukac()
        {
            var etelIrany = MerreVanAzEtel(Tengely.Y);
            if (etelIrany != null && !IsThisPalyaElemHalaltOkoz(etelIrany.Item2))
                return etelIrany.Item1;
            else if (IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X, kukacFej.Y + 1)))
            {
                if (!IsThisPalyaElemHalaltOkoz(new Point(kukacFej.X + 1, kukacFej.Y)))
                    return Iranyok.JOBB;
                else return Iranyok.BAL;
            }
            // Ha nincs veszély akkor ugyanarra folytathatja az útját
            else return Iranyok.LE;
        }
        
        /* Visszaadja, hogy a kukac fejétől merre található az étel
         * param tengely -> meg kell adni, hogy melyik tengelyen mozog a kukac
         * null értékkel tér vissza ha változatlan az irány
         */
        private Tuple<Iranyok,Point> MerreVanAzEtel(Tengely tengely)
        {
            switch (tengely)
            {
                case Tengely.X:
                    {
                        if ((int)etel.Y < (int)kukacFej.Y)
                            return new Tuple<Iranyok, Point>(Iranyok.FEL, new Point(kukacFej.X, kukacFej.Y - 1));
                        else if ((int)etel.Y > (int)kukacFej.Y)
                            return new Tuple<Iranyok, Point>(Iranyok.LE, new Point(kukacFej.X, kukacFej.Y + 1));
                        else return null;
                        break;
                    }
                case Tengely.Y:
                    {
                        if ((int)etel.X < (int)kukacFej.X)
                            return new Tuple<Iranyok, Point>(Iranyok.BAL, new Point(kukacFej.X - 1, kukacFej.Y));
                        else if ((int)etel.X > (int)kukacFej.X)
                            return new Tuple<Iranyok, Point>(Iranyok.JOBB, new Point(kukacFej.X + 1, kukacFej.Y));
                        else return null;                        
                        break;
                    }
            }
            return null;
        }
    }
}
