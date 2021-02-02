using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadForm
{
    public partial class Form1 : Form
    {
        private delegate void AtualizarControle(Control controle, string propriedade, object valor); // Delegate
        Thread t; 
        public Form1()
        {
            InitializeComponent();
            t = new Thread(new ThreadStart(Tarefa));
            t.IsBackground = true;
        }

        private void btnPrincipal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Principal");
        }

        private void btnContador_Click(object sender, EventArgs e)
        {
            if (!t.IsAlive)
            {
                t.Start();
            }
        }

        private void Tarefa()
        {
            while (true)
            {
              //  lblResultado.Text = DateTime.Now.Second.ToString();
                lblResultado.Invoke(new Action(() => lblResultado.Text = DateTime.Now.Second.ToString())); // new Action() é uma maneira de criar um Delegate anonimo e o '() =>' é o uso de um método LAMBDA para chamar a tarefa que vamos exexutar
                // Forma mais complicada:
                // DefinirValorPropriedade(lblResultado, "Text", DateTime.Now.Second.ToString());
            }
        }

        private void DefinirValorPropriedade(Control controle, string propriedade, object valor)
        {
            if (controle.InvokeRequired) // verifica se o controle passado como parametro precisa de um Invoke, que é um método que é executado quando o controle precisa ser executado em thread diferente.
            {
                AtualizarControle d = new AtualizarControle(DefinirValorPropriedade);
                controle.Invoke(d, new object[] { controle, propriedade, valor } );
            }
            else
            {
                Type t = controle.GetType();
                PropertyInfo[] props = t.GetProperties();

                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propriedade.ToUpper())
                    {
                        p.SetValue(controle, valor, null);
                    }
                }
            }
        }
    }
}
