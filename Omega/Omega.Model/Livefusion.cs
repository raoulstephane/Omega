using Omega.DataManager;
using System;
using System.Collections.Generic;

namespace Omega.Model
{
    public class Livefusion
    {
        public List<string> SelectSong(List<string> playlistIdSource, MetaDonnees askedDonnees, double ratio)
        {
            Requests cr = new Requests();
            List<string> FilteredList = new List<string>();
            List<CleanTrack> cleanTracks = new List<CleanTrack>();
            for (int i = 0; i < playlistIdSource.Count; i++)
            {
                CleanTrack analysedSong = cr.GetSongCleanTrack(playlistIdSource[i]);
                if (Compare(askedDonnees.acousticness, analysedSong.Acousticness, ratio)
                && Compare(askedDonnees.danceability, analysedSong.Danceability, ratio)
                && Compare(askedDonnees.energy, analysedSong.Energy, ratio)
                && Compare(askedDonnees.instrumentalness, analysedSong.Instrumentalness, ratio)
                && Compare(askedDonnees.liveness, analysedSong.Liveness, ratio)
                && Compare(askedDonnees.loudness, analysedSong.Loudness, ratio)
                && Compare(askedDonnees.mode, analysedSong.Mode, ratio)
                && Compare(askedDonnees.speechiness, analysedSong.Speechiness, ratio)
                && Compare(askedDonnees.tempo, analysedSong.Tempo, ratio)
                && Compare(askedDonnees.valence, analysedSong.Vanlence, ratio))
                    FilteredList.Add(analysedSong.Id);
            }
            return FilteredList;
        }

        public static bool Compare(string asked, string analysed, double ratio)
        {
            if (asked != null) asked = asked.Replace(".", ",");
            if (analysed != null) analysed = analysed.Replace(".", ",");

            return (asked == null || analysed == null || (Double.Parse(analysed) > Double.Parse(asked) - ratio && Double.Parse(analysed) < Double.Parse(asked) + ratio));
        }
    }
}
