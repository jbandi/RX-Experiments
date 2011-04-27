using System;
using System.Linq;
using System.Windows.Forms;

namespace Exercises
{
    public class Exercise3
    {
        public static void Run()
        {
            var lbl = new Label();
            var form = new Form { Controls = { lbl } };

            #region classic
//            form.MouseMove += (sender, args) => { 
//                lbl.Text = args.Location.ToString(); 
//            };
//            Application.Run(form);
            #endregion

            #region RX

            var moves = Observable.FromEvent<MouseEventArgs>(form, "MouseMove");

            var locations = from m in moves
                            where m.EventArgs.X > 100
                            select new {X = m.EventArgs.X, Y = m.EventArgs.Y + 100};

            using (locations.Delay(TimeSpan.FromSeconds(1)).ObserveOn(lbl).Subscribe(l =>
                                       {
                                           lbl.Text = l.X + "-" + l.Y;
                                       }))
            {
                Application.Run(form);
            }
            #endregion


        }
    }
}