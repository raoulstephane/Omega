using Omega.Crawler;
using Omega.DataManager;
using System.Collections.Generic;

namespace Omega.Model
{
    public class Livefusion
    {
        public List<string> SelectSong(List<string> playlistIdSource, MetaDonnees askedDonnees)
        {
            Requests r = new Requests();
            List<string> FilteredList = new List<string>();
            List<CleanTrack> cleanTracks = new List<CleanTrack>();
            for(int i= 0; i < playlistIdSource.Count; i++)
            {
                CleanTrack analysedSong = r.GetSongCleanTrack(playlistIdSource[i]);
                if(askedDonnees.acousticness == null || askedDonnees.acousticness == analysedSong.Acousticness)
                {
                    if (askedDonnees.danceability == null || askedDonnees.danceability == analysedSong.Danceability)
                    {
                        if (askedDonnees.energy == null || askedDonnees.energy == analysedSong.Energy)
                        {
                            if (askedDonnees.instrumentalness == null || askedDonnees.instrumentalness == analysedSong.Instrumentalness)
                            {
                                if (askedDonnees.liveness == null || askedDonnees.liveness == analysedSong.Liveness)
                                {
                                    if (askedDonnees.loudness == null || askedDonnees.loudness == analysedSong.Loudness)
                                    {
                                        if (askedDonnees.mode == null || askedDonnees.mode == analysedSong.Mode)
                                        {
                                            if (askedDonnees.speechiness == null || askedDonnees.speechiness == analysedSong.Speechiness)
                                            {
                                                if (askedDonnees.tempo == null || askedDonnees.tempo == analysedSong.Tempo)
                                                {
                                                    if (askedDonnees.valence == null || askedDonnees.valence == analysedSong.Vanlence)
                                                    {
                                                        FilteredList.Add(analysedSong.Id);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }               
            return FilteredList;
        }
    }
}
