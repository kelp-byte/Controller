using System;
using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using Java.Util;
using Android.Bluetooth;
using System.Threading.Tasks;
using System.Threading;
using Controller.Schnittstellen;
using Xamarin.Forms;
using System.Text;
using Controller.Model;
using System.Collections.Generic;
using Acr.UserDialogs;

[assembly: Dependency(typeof(Controller.Droid.AndroidBluetooth))]

namespace Controller.Droid
{
    public class AndroidBluetooth : IBluetooth
    {

        public List<Data> outputData;
        public List<Data> inputData;
        //Variablen für die Bluetooth-Verwaltung Adapter und Socket
        private BluetoothAdapter mBluetoothAdapter = null;
        private BluetoothSocket btSocket = null;
        BluetoothDevice device;
        private bool isConnected = false;

        //Streams I/0
        private Stream outStream = null;
        private Stream inStream = null;

        //MAC Addresse des Bluetooth Gerät
        private static string address = "98:D3:31:F6:47:00";

        //ID unico zu Komunication
        private static UUID MY_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

        //Candel Thread
        readonly CancellationTokenSource tokenSource = new CancellationTokenSource();


        public AndroidBluetooth()
        {
            inputData = new List<Data>();
            outputData = new List<Data>();
        }

        public IBluetooth GetBluetooth()
        {
            return this;
        }

        public Model.Enum.ConnectedState CheckBt()
        {
            //asignamos el sensor bluetooth con el que vamos a trabajar
            //Zuweisung des Bluetooth-Sensor
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            //Verificamos que este habilitado
            //Prüfen, ob der Sensor vorhanden ist
            if (!mBluetoothAdapter.Enable())
            {
                return Model.Enum.ConnectedState.notEnabled;
            }
            //verificamos que no sea nulo el sensor
            //Prüfen, ob der Sensor nicht null ist
            if (mBluetoothAdapter == null)
            {
                return Model.Enum.ConnectedState.dontExist;

            }
            return Model.Enum.ConnectedState.Enabled;


        }

        public bool Connect()
        {
            {
                //Verbindung mit dem Bluetooth Gerät
                device = mBluetoothAdapter.GetRemoteDevice(address);
                System.Console.WriteLine("Geräteadresse" + device);

                //Adapter auf nicht vervügbar setzen
                mBluetoothAdapter.CancelDiscovery();
                try
                {
                    //??Einfügen der Kommunikationsbuchse mit dem Arduino??
                    btSocket = device.CreateRfcommSocketToServiceRecord(MY_UUID);
                    //Verbinden mit dem Socket
                    btSocket.Connect();
                    System.Console.WriteLine("Connected");
                    isConnected = true;

                }
                catch (System.Exception e)
                {
                 
                    //Schließt den Sochet
                    Console.WriteLine(e.Message);
                    try
                    {
                        btSocket.Close();
                    }
                    catch (System.Exception)
                    {
                        System.Console.WriteLine("Fehler beim Schließen");
                    }
                    System.Console.WriteLine("Socket erstellt");
                    isConnected = false;

                }
            }
            return isConnected;
        }
        public void Disconnect()
        {
            if (btSocket.IsConnected)
            {
                try
                {
                    btSocket.Close();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            isConnected = false;
        }

        public void Write(string data)
        {
            if (isConnected)
            {
                Java.Lang.String data_send = new Java.Lang.String(data);
                //Extrahieren des Ausgabestom
                try
                {
                    outStream = btSocket.OutputStream;
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine("Error:" + e.Message);
                }

                //Erstellen der Zeichenkette
                Java.Lang.String message = data_send;

                //Konvertieren in Bytes
                byte[] msgBuffer = message.GetBytes();

                try
                {
                    //Senden der daten
                    outStream.Write(msgBuffer, 0, msgBuffer.Length);
                    outputData.Add(new Data(data));
                    System.Console.WriteLine("Daten gesendet: " + data);

                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine("Error beim Senden:" + e.Message);
                }
            }
        }

        public void preperListenData()
        {
            if (isConnected)
            {
                //Instream extrahieren
                try
                {
                    inStream = btSocket.InputStream;
                }
                catch (System.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void listenToData(Label label)
        {
            if (isConnected)
            {
                //Buffer zum lesen
                byte[] buffer = new byte[1024];
                //anzah der Bytes
                int bytes;

                var tets = Task.Factory.StartNew(() =>
                    {
                        string velocity = "";
                        var builder = new StringBuilder();

                        while (true)
                        {

                            try
                            {
                                //Lesen der Nachricht und Speichern der Länge
                                buffer = new byte[1024];
                                bytes = inStream.Read(buffer, 0, buffer.Length);
                                if (bytes > 0)
                                {
                                    //Umwanden in Text
                                    string valor = System.Text.Encoding.ASCII.GetString(buffer);
                                    inputData.Add(new Data(valor));
                                    char[] chars = valor.ToCharArray();
                                    for (int i = 0; i < chars.Length && chars[i] != '\n' && chars[i] != '\0'; i++)
                                    {
                                        if (chars[i] == '\r')
                                        {
                                            velocity = builder.ToString();
                                            MainActivity.main.RunOnUiThread(() =>
                                            {
                                                //Schreiben in das Label
                                                Console.WriteLine("Bekommen:" + velocity);
                                                label.Text = velocity;



                                            });
                                            builder = new StringBuilder();
                                        }
                                        else
                                        {
                                            builder.Append(chars[i]);
                                        }
                                    }

                                    //Ausführen in dem HauptThread 

                                }


                            }
                            catch
                            {
                            }
                            if (tokenSource.IsCancellationRequested)
                            {
                                break;
                            }
                        }
                    });
            }
        }

        public List<Data> getInputData()
        {
            return inputData;
        }
        public List<Data> getOutputData()
        {
            return outputData;
        }
    }
}