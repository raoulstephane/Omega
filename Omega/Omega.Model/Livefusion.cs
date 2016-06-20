using Omega.DataManager;
using System;
using System.Collections.Generic;

namespace Omega.Model
{
    public class Livefusion
    {
        Requests cr = new Requests();
        public List<string> PlaylistAnalyser(List<string> playlistIdSource, MetaDonnees askedDonnees, double ratio)
        {         
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
                && Compare(askedDonnees.valence, analysedSong.Valence, ratio))
                    FilteredList.Add(analysedSong.Id);
            }
            return FilteredList;
        }

        public List<string> PlaylistAnalyser(List<string> playlistIdSource, MetaDonnees askedDonneesMax, MetaDonnees askedDonneesMin)
        {
            List<string> FilteredList = new List<string>();
            List<CleanTrack> cleanTracks = new List<CleanTrack>();
            for (int i = 0; i < playlistIdSource.Count; i++)
            {
                CleanTrack analysedSong = cr.GetSongCleanTrack(playlistIdSource[i]);
                if (Compare(askedDonneesMax.acousticness, analysedSong.Acousticness, askedDonneesMin.acousticness)
                && Compare(askedDonneesMax.danceability, analysedSong.Danceability, askedDonneesMin.danceability)
                && Compare(askedDonneesMax.energy, analysedSong.Energy, askedDonneesMin.energy)
                && Compare(askedDonneesMax.instrumentalness, analysedSong.Instrumentalness, askedDonneesMin.instrumentalness)
                && Compare(askedDonneesMax.liveness, analysedSong.Liveness, askedDonneesMin.liveness)
                && Compare(askedDonneesMax.loudness, analysedSong.Loudness, askedDonneesMin.loudness)
                && Compare(askedDonneesMax.mode, analysedSong.Mode, askedDonneesMin.mode)
                && Compare(askedDonneesMax.speechiness, analysedSong.Speechiness, askedDonneesMin.speechiness)
                && Compare(askedDonneesMax.tempo, analysedSong.Tempo, askedDonneesMin.tempo)
                && Compare(askedDonneesMax.valence, analysedSong.Valence, askedDonneesMin.valence))
                    FilteredList.Add(analysedSong.Id);
            }
            return FilteredList;
        }

        public static bool Compare(string asked, string analysed, double ratio)
        {
            ratio = Double.Parse(asked) * ratio / 100;
            if (asked != null) asked = asked.Replace(".", ",");
            if (analysed != null) analysed = analysed.Replace(".", ",");

            return (string.IsNullOrEmpty(asked) || string.IsNullOrEmpty(analysed) 
                || (Double.Parse(analysed) > Double.Parse(asked) - ratio && Double.Parse(analysed) < Double.Parse(asked) + ratio));
        }

        public static bool Compare(string askedMax, string analysed, string askedMin)
        {

            if (askedMax != null) askedMax = askedMax.Replace(".", ",");
            if (askedMin != null) askedMin = askedMin.Replace(".", ",");
            if (analysed != null) analysed = analysed.Replace(".", ",");

            return (string.IsNullOrEmpty(askedMax) || string.IsNullOrEmpty(askedMin) || string.IsNullOrEmpty(analysed) 
                || (Double.Parse(analysed) > Double.Parse(askedMin) && Double.Parse(analysed) < Double.Parse(askedMax)));
        }
    }
}
