using Controller.Model;
using System.Collections.Generic;

namespace Controller.Schnittstellen
{
    public interface IBluetooth
    {

        IBluetooth GetBluetooth();


        /// <summary>
        /// Checkt, ob der Bliuetooth Adapter vorhanden uns verfügbar ist
        /// </summary>
        /// <returns>
        /// <para>Enabled</para>
        /// <para>not Enabled</para>
        /// <para>Not Exist</para>
        /// </returns>
        Enum.ConnectedState CheckBt();

        /// <summary>
        ///  Versucht, sich mit dem Bluetooth Modul zu verbinden
        /// </summary>
        /// <returns>erfolgreich true, andernfalls false</returns>
        bool Connect();
        /// <summary>
        /// Hebt die Verbindung auf
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sendet Daten an das Modul
        /// </summary>
        /// <param name="data">zusendene Daten</param>      
        void Write(string data);

        /// <summary>
        /// Startet das hören auf die Daten
        /// </summary>     
        void preperListenData();
        /// <summary>
        /// Startet den Thread, der die gesendeten Daten ließt
        /// </summary>
        /// <param name="label">Label, wo die Daten reingeschrieben werden</param>
        void listenToData(Xamarin.Forms.Label label);


        /// <summary>
        /// Gibt alle reingekommenen Daten zurück
        /// </summary>
        /// <returns>Liste mit Logs</returns>
        List<Data> getInputData();
        /// <summary>
        /// Gibt alle geschriebenden Daten zurück
        /// </summary>
        /// <returns>Liste mit Logs</returns>
        List<Data> getOutputData();
    }
}
