using Cindalnet.SQLBot.View;
using Cindalnet.SQLBot.Database;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cindalnet.SQLBot.Presenter
{
    public class PresenterManage : PresenterForm, IPresenter
    {
        protected IFormManage View;
        public event EventHandler ViewClosed;

        public PresenterManage()
        {
            MaterialForm = new CFormManage();
            View = (IFormManage)MaterialForm;
            FormControl = View.FormControl;

            View.PropertyChanged += view_PropertyChanged;
            View.ViewClosed += view_Closed;
            View.AddToDatabase += View_AddToDatabase;
        }

        void View_AddToDatabase(object sender, EventArgs e)
        {
            try
            {
                // Initialize database context
                TramwajeDataContext databaseContext = new TramwajeDataContext();

                // Parse XML file..
                XDocument TramXMLFile = XDocument.Load(View.FilePath);
                foreach (var tramXML in TramXMLFile.Root.Descendants("linia"))
                {
                    Linie linia = new Linie();
                    linia.Nazwa = tramXML.Attribute("nazwa").Value;
                    linia.Typ = tramXML.Attribute("typ").Value;
                    foreach (var wariantXML in tramXML.Descendants("wariant"))
                    {
                        WariantTrasy wariant = new WariantTrasy();
                        wariant.Linie = linia;
                        wariant.Nazwa = wariantXML.Attribute("nazwa").Value;
                        // BISKUPIN - POŚWIĘTNE
                        int pPos = wariant.Nazwa.IndexOf("-");
                        wariant.Start = wariant.Nazwa.Substring(0, pPos - 1);
                        wariant.Koniec = wariant.Nazwa.Substring(pPos + 1);
                        foreach(var przystanekXML in wariantXML.Descendants("przystanek"))
                        {
                            Przystanek przystanek = new Przystanek();
                            przystanek.WariantTrasy = wariant;
                            przystanek.Nazwa = przystanekXML.Attribute("nazwa").Value;
                            przystanek.Ulica = przystanekXML.Attribute("ulica").Value;
                            foreach(var dzienXML in przystanekXML.Descendants("dzien"))
                            {
                                foreach (var godzXML in dzienXML.Descendants("godz"))
                                {
                                    foreach (var minXML in godzXML.Descendants("min"))
                                    {
                                        Przyjazd przyjazd = new Przyjazd();
                                        przyjazd.Dzień = dzienXML.Attribute("nazwa").Value;
                                        przyjazd.Godzina = int.Parse(godzXML.Attribute("h").Value);
                                        przyjazd.Minuta = int.Parse(minXML.Attribute("m").Value);
                                        przyjazd.Przystanek = przystanek;

                                        przystanek.Przyjazds.Add(przyjazd);
                                    }
                                }
                            }

                            wariant.Przystaneks.Add(przystanek);
                        }
                       linia.WariantTrasies.Add(wariant);
                    }

                    if (databaseContext.Linies.Count(l => l.Nazwa == linia.Nazwa) > 0)
                    {
                        databaseContext.Linies.DeleteAllOnSubmit(databaseContext.Linies.Where(l => l.Nazwa == linia.Nazwa));
                    }

                    databaseContext.Linies.InsertOnSubmit(linia);
                }
                databaseContext.SubmitChanges();

                View.Message = "Dane zostały pomyślnie zapisane.";
            }catch(Exception ex)
            {
                View.Message = "Wystąpił błąd podczas przetwarzania pliku: " + ex.Message;
            }
        }

        public void view_Closed(object sender, EventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed(this, e);
        }

        public void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //
        }

        public void view_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //
        }

        public void view_OK(object sender, EventArgs e)
        {
            //
        }

        public void view_Cancel(object sender, EventArgs e)
        {
            //
        }
    }
}
