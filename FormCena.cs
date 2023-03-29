using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibraryFilosofos;
using System.Collections.ObjectModel;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Configuration;

namespace WindowsFormsApplicationCenaFilosofos
{
    public partial class FormCena : Form
    {
        Int32 TiempoComer = 0;
        Int32 TiempoPensar = 0;
        Int32 Comiendo = 0;
        Int32 Pensando = 0;
        Int32 Contador = 0;
        Int32 nFilosofos = 5;
        Int32 Tiempo = 0;
        bool bd = false;
        bool bd2 = false;
        bool bd3 = false;                
        List<ClassLibraryFilosofos.Filosofo> ListaFilosofos = new List<ClassLibraryFilosofos.Filosofo>();        
        string[] NombreFilosofos = { "Platon ", "Aristoteles","Anaximenes","Anaximandro" ,"Leibniz"};
        
        public FormCena()
        {
            InitializeComponent();
            NombrarFilosofos();            
            labelTime.Text = TiempoComer.ToString();
        }
        //Funcion para nombrar a los filosofos
        private void NombrarFilosofos()
        {
            labelFilosofo0.Text = "Platon";
            labelFilosofo1.Text = "Aristoteles";
            labelFilosofo2.Text = "Anaximenes";
            labelFilosofo3.Text = "Anaximandro";
            labelFilosofo4.Text="Leibniz";
        }
        //funcion del boton comezar la simulacion de la cena de filosofos
        private void b_Comenzar_Click_1(object sender, EventArgs e)
        {
            if (Tiempo_Comer.Text != string.Empty || Tiempo_Pensar.Text != string.Empty)
            {
                if (bd3 == false)
                {
                    b_Comenzar.Enabled = false;
                    buttonPausar.Enabled = true;
                    button1.Enabled = true;
                  
                    bd3 = true;
                    bd = true;
                    TiempoComer = Convert.ToInt32(Tiempo_Comer.Text);
                    TiempoPensar = Convert.ToInt32(Tiempo_Pensar.Text);

                    for (int i = 0; i < nFilosofos; i++)
                    {
                        ClassLibraryFilosofos.Filosofo oFilosofo = new ClassLibraryFilosofos.Filosofo();
                        oFilosofo.Id_Filosofo = i;
                        oFilosofo.Nombre_Filosofo = NombreFilosofos[i];
                        oFilosofo.TiempoComer = 5;
                        oFilosofo.TiempoPensar = 5;
                        ListaFilosofos.Add(oFilosofo);
                    }
                    DefinirEstado();
                }
                else
                {
                    Inicializar();
                    NombrarFilosofos();
                }
            }
            else
            {
                MessageBox.Show("Ingreso los Tiempos para Comer y Pensar");                
            }
        }
        //fucion para inicializar los componentes que loconforman
        private void Inicializar()
        {
            timerCenaFilosofos.Start();
            buttonPausar.Enabled = true;
            b_Comenzar.Enabled = false;
            button1.Enabled = true;
        }

        private void EstadoPensar()
        {
            foreach (ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
            {
                oFilosofo.Pensando = true;
            }
        }

        private void DefinirEstado()
        {            
            foreach(ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
            {
                oFilosofo.Comiendo = false;
                oFilosofo.Pensando = false;
                oFilosofo.CogerTenedores = false;
                oFilosofo.SoltarTenedores = false;
                oFilosofo.Hambriento = false;
            }
        }

        private void ReestablecerEstados(ClassLibraryFilosofos.Filosofo oFilosofo)
        {
            oFilosofo.Comiendo = false;
            oFilosofo.Pensando = false;
            oFilosofo.Hambriento = false;
        }

        public void Sincronizar()
        {
            if (bd2 == false)
            {
                Random Aleatorio = new Random();
                int i = Aleatorio.Next(0, 5);
                Contador = i;
                // funcion para poner a pensar a los filosofos
                //El metodo restablece estados sirve para mantener los filosofos en un inico estado
                foreach (ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                {
                    ReestablecerEstados(oFilosofo);
                    //Restabecer tenedores
                    ReestablecerTenedores(Convert.ToInt32(oFilosofo.Id_Filosofo));

                    oFilosofo.Pensando = true;                    
                }
                if (TiempoPensar == Tiempo)
                {
                    //metodo para poner los filosofos hambriendos                 
                    foreach (ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                    {
                        ReestablecerEstados(oFilosofo);
                        oFilosofo.Hambriento = true;                        
                    }
   
                    // metod0o para poner a los filosofos a comer, de manera intercalados
                    // cada flosofo lo identifica por un ID
                    if (i < 3)
                    {
                        foreach(ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                        {
                            if(oFilosofo.Id_Filosofo==i)
                            {
                                ReestablecerEstados(oFilosofo);
                                //se llama la funcion llamar tenedor
                                LlamarTenedores(i);
                                oFilosofo.Comiendo = true;
                                AgregarSuceso(oFilosofo.Nombre_Filosofo + " esta comiendo");
                                foreach(ClassLibraryFilosofos.Filosofo ooFilosofo in ListaFilosofos)
                                {
                                    if(ooFilosofo.Id_Filosofo==i+2)
                                    {
                                        ReestablecerEstados(ooFilosofo);
                                        //se llama la funcion llamar tenedor
                                        LlamarTenedores(i+2);
                                        ooFilosofo.Comiendo = true;
                                        AgregarSuceso(ooFilosofo.Nombre_Filosofo+" esta comiendo");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (ClassLibraryFilosofos.Filosofo ooFilosofo in ListaFilosofos)
                        {
                            if(ooFilosofo.Id_Filosofo==i)
                            {
                                ReestablecerEstados(ooFilosofo);
                                //se llama la funcion llamar tenedor
                                LlamarTenedores(i);
                                ooFilosofo.Comiendo = true;
                                AgregarSuceso(ooFilosofo.Nombre_Filosofo + " esta comiendo");
                                foreach (ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                                {
                                    if(oFilosofo.Id_Filosofo==i-2)
                                    {
                                        ReestablecerEstados(oFilosofo);
                                        //se llama la funcion llamar tenedor
                                        LlamarTenedores(i-2);
                                        oFilosofo.Comiendo = true;
                                        AgregarSuceso(oFilosofo.Nombre_Filosofo + " esta comiendo");
                                    }
                                }
                            }
                        }
                    }                    
                    bd2 = true;
                }
            }
            else
            {
                if (Comiendo < TiempoComer)
                    Comiendo++;
                if (Pensando < TiempoPensar)
                    Pensando++;
                // funcion para poner hambrientos a los que piensan
                if(Pensando==TiempoPensar)
                {
                    foreach(ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                    {
                        if (oFilosofo.Pensando)
                        {
                            ReestablecerEstados(oFilosofo);
                            oFilosofo.Hambriento = true;
                        }
                    }
                    Pensando = 0;
                }                
                // funcion para poner a pensar o hambrientos a los que comian
                if(Comiendo==TiempoComer)
                {
                    foreach(ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                    {
                        if(oFilosofo.Comiendo)
                        {
                            //funcion para reestablecer los tenedores
                            ReestablecerTenedores(Convert.ToInt32(oFilosofo.Id_Filosofo));
                            // si no hay tiempo para pensar, pasar a hambrientos
                            if (TiempoPensar > 0)
                            {
                                ReestablecerEstados(oFilosofo);
                                oFilosofo.Pensando = true;
                            }
                            else
                            {
                                ReestablecerEstados(oFilosofo);
                                oFilosofo.Hambriento = true;
                            }
                        }
                    }
                    Comiendo = 0;
                    Contador++;
                    if (Contador > 4)
                        Contador = 0;
                    if (Contador < 3)
                    {
                        foreach(ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                        {
                            if (oFilosofo.Id_Filosofo == Contador)
                            {
                                ReestablecerEstados(oFilosofo);
                                //se llama la funcion llamar tenedor
                                LlamarTenedores(Contador);
                                oFilosofo.Comiendo = true;
                                AgregarSuceso(oFilosofo.Nombre_Filosofo + " esta comiendo");
                                foreach (ClassLibraryFilosofos.Filosofo ooFilosofo in ListaFilosofos)
                                {
                                    if (ooFilosofo.Id_Filosofo == Contador + 2)
                                    {
                                        ReestablecerEstados(ooFilosofo);
                                        //se llama la funcion llamar tenedor
                                        LlamarTenedores(Contador+2);
                                        ooFilosofo.Comiendo = true;
                                        AgregarSuceso(ooFilosofo.Nombre_Filosofo + " esta comiendo");
                                    }
                                }
                            }
                        }                        
                    }
                    else
                    {
                        foreach (ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
                        {
                            if (oFilosofo.Id_Filosofo == Contador)
                            {
                                ReestablecerEstados(oFilosofo);
                                //se llama la funcion llamar tenedores
                                LlamarTenedores(Contador);
                                oFilosofo.Comiendo = true;
                                AgregarSuceso(oFilosofo.Nombre_Filosofo + " esta comiendo");
                                foreach (ClassLibraryFilosofos.Filosofo ooFilosofo in ListaFilosofos)
                                {
                                    if (ooFilosofo.Id_Filosofo == Contador -3)
                                    {
                                        ReestablecerEstados(ooFilosofo);
                                        //se llama la funcion llamar tenedores
                                        LlamarTenedores(Contador-3);
                                        ooFilosofo.Comiendo = true;
                                        AgregarSuceso(ooFilosofo.Nombre_Filosofo + " esta comiendo");
                                    }
                                }
                            }
                        }  
                    }
                }
            }
        }

        private void LlamarTenedores(int NumFilosofo)
        {
            switch (NumFilosofo)
            {
                case 0:
                    MoverTenedoresComerF1();
                    break;
                case 1:
                    MoverTenedoresComerF2();
                    break;
                case 2:
                    MoverTenedoresComerF3();
                    break;
                case 3:
                    MoverTenedoresComerF4();
                    break;
                case 4:
                    MoverTenedoresComerF5();
                    break;
                default:
                    break;
            }
        }

        private void ReestablecerTenedores(int NumFiloso)
        {
            switch (NumFiloso)
            {
                case 0:
                    ReestablecerTenedoresF1();
                    break;
                case 1:
                    ReestablecerTenedoresF2();
                    break;
                case 2:
                    ReestablecerTenedoresF3();
                    break;
                case 3:
                    ReestablecerTenedoresF4();
                    break;
                case 4:
                    ReestablecerTenedoresF5();
                    break;
                default:
                    break;
            }
        }

        private void timerCenaFilosofos_Tick(object sender, EventArgs e)
        {
            if(bd)
            {               
                labelTime.Text = Tiempo.ToString();
                Sincronizar();
                ComprobarEstados();
                Tiempo = Tiempo + 1;
            }            
        }

        private void ComprobarEstados()
        {
            string NameLabel = string.Empty;
            string NamePicture = string.Empty;

            foreach(ClassLibraryFilosofos.Filosofo oFilosofo in ListaFilosofos)
            {
                NameLabel = "labelFilosofo" + oFilosofo.Id_Filosofo.ToString();
                int LabelIndex = panelComerdorFilosofos.Controls.IndexOfKey(NameLabel);

                if(oFilosofo.Pensando)
                {
                    Label oLabel=(Label)panelComerdorFilosofos.Controls[LabelIndex];
                    oLabel.Text = "PENSANDO";
                }
                if(oFilosofo.Comiendo)
                {
                    Label oLabel = (Label)panelComerdorFilosofos.Controls[LabelIndex];
                    oLabel.Text = "COMIENDO";
                }
                if(oFilosofo.Hambriento)
                {
                    Label oLabel = (Label)panelComerdorFilosofos.Controls[LabelIndex];
                    oLabel.Text = "HAMBRIENTO";
                }
            }
        }

        public void AgregarSuceso(string Suceso)
        {
            listBoxSucesos.Items.Add(Suceso);
        }

        private void buttonReiniciar_Click(object sender, EventArgs e)
        {
            timerCenaFilosofos.Stop();
            b_Comenzar.Enabled = true;
            buttonPausar.Enabled = false;            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MoverTenedoresComerF5();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ReestablecerTenedoresF5();
        }

        private void MoverTenedoresComerF5()
        {
            pictureBoxT4F5.Visible = true;
            pictureBoxT5F5.Visible = true;

            pictureBoxT5.Visible = false;
            pictureBoxT4.Visible = false;
        }

        private void MoverTenedoresComerF4()
        {
            pictureBoxT3F4.Visible = true;
            pictureBoxT4F4.Visible = true;

            pictureBoxT4.Visible = false;
            pictureBoxT3.Visible = false;
        }
        private void MoverTenedoresComerF3()
        {
            pictureBoxT3F3.Visible = true;
            pictureBoxT2F3.Visible = true;

            pictureBoxT3.Visible = false;
            pictureBoxT2.Visible = false;
        }
        private void MoverTenedoresComerF2()
        {
            pictureBoxT1F2.Visible = true;
            pictureBoxT2F2.Visible = true;

            pictureBoxT1.Visible = false;
            pictureBoxT2.Visible = false;
        }
        private void MoverTenedoresComerF1()
        {
            pictureBoxT1F1.Visible = true;
            pictureBoxT5F1.Visible = true;

            pictureBoxT5.Visible = false;
            pictureBoxT1.Visible = false;
        }

        private void ReestablecerTenedoresF2()
        {
            pictureBoxT1F2.Visible = false;
            pictureBoxT2F2.Visible = false;

            pictureBoxT1.Visible = true;
            pictureBoxT2.Visible = true;
        }

        private void ReestablecerTenedoresF1()
        {
            pictureBoxT1F1.Visible = false;
            pictureBoxT5F1.Visible = false;

            pictureBoxT5.Visible = true;
            pictureBoxT1.Visible = true;
        }

        private void ReestablecerTenedoresF3()
        {
            pictureBoxT3F3.Visible = false;
            pictureBoxT2F3.Visible = false;

            pictureBoxT3.Visible = true;
            pictureBoxT2.Visible = true;
        }

        private void ReestablecerTenedoresF4()
        {
            pictureBoxT3F4.Visible = false;
            pictureBoxT4F4.Visible = false;

            pictureBoxT4.Visible = true;
            pictureBoxT3.Visible = true;
        }

        private void ReestablecerTenedoresF5()
        {
            pictureBoxT4F5.Visible = false;
            pictureBoxT5F5.Visible =false;

            pictureBoxT5.Visible = true;
            pictureBoxT4.Visible = true;
        }

        private void iniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerCenaFilosofos.Start();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerCenaFilosofos.Stop();
        }

        private void b_terminar_Click(object sender, EventArgs e)
        {
            Tiempo_Pensar.Text = " ";
            Tiempo_Comer.Text = " ";
            listBoxSucesos.Text = " ";
            labelTime.Text = "";
            timerCenaFilosofos.Stop();
        }
        private void b_salir0_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
