using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tracker.Core.StaticTypes
{
    public class Counties
    {
        public static readonly County ALAMEDA = new County("Alameda", "ALA", "AA", 1, "NWIC");
        public static readonly County ALPINE = new County("Alpine", "ALP", "AP", 2, "CCaIC");
        public static readonly County AMADOR = new County("Amador", "AMA", "AM", 3, "NCIC");
        public static readonly County BUTTE = new County("Butte", "BUT", "BT", 4, "NEIC");
        public static readonly County CALAVERAS = new County("Calaveras", "CAL", "CA", 5, "CCaIC");
        public static readonly County COLUSA = new County("Colusa", "COL", "CO", 6, "NWIC");
        public static readonly County CONTRA_COSTA = new County("Contra Costa", "CCO", "CC", 7, "NWIC");
        public static readonly County DEL_NORTE = new County("Del Norte", "DNO", "DN", 8, "NWIC");
        public static readonly County EL_DORADO = new County("El Dorado", "ELD", "ED", 9, "NCIC");
        public static readonly County FRESNO = new County("Fresno", "FRE", "FR", 10, "SSJVIC");
        public static readonly County GLENN = new County("Glenn", "GLE", "GL", 11, "NEIC");
        public static readonly County HUMBOLDT = new County("Humboldt", "HUM", "HU", 12, "NWIC");
        public static readonly County IMPERIAL = new County("Imperial", "IMP", "IM", 13, "SCIC");
        public static readonly County INYO = new County("Inyo", "INY", "IN", 14, "EIC");
        public static readonly County KERN = new County("Kern", "KER", "KE", 15, "SSJVIC");
        public static readonly County KINGS = new County("Kings", "KIN", "KI", 16, "SSJVIC");
        public static readonly County LAKE = new County("Lake", "LAK", "LK", 17, "NWIC");
        public static readonly County LASSEN = new County("Lassen", "LAS", "LS", 18, "NEIC");
        public static readonly County LOS_ANGELES = new County("Los Angeles", "LAN", "LA", 19, "SCCIC");
        public static readonly County MADERA = new County("Madera", "MAD", "MA", 20, "SSJVIC");
        public static readonly County MARIN = new County("Marin", "MRN", "MR", 21, "NWIC");
        public static readonly County MARIPOSA = new County("Mariposa", "MRP", "MP", 22, "CCaIC");
        public static readonly County MENDOCINO = new County("Mendocinco", "MEN", "MD", 23, "NWIC");
        public static readonly County MERCED = new County("Merced", "MER", "ME", 24, "CCaIC");
        public static readonly County MODOC = new County("Modoc", "MOD", "MO", 25, "NEIC");
        public static readonly County MONO = new County("Mono", "MNO", "MN", 26, "EIC");
        public static readonly County MONTEREY = new County("Monterey", "MNT", "MT", 27, "NWIC");
        public static readonly County NAPA = new County("Napa", "NAP", "NP", 28, "NWIC");
        public static readonly County NEVADA = new County("Nevada", "NEV", "NV", 29, "NCIC");
        public static readonly County ORANGE = new County("Orange", "ORA", "OR", 30, "SCCIC");
        public static readonly County PLACER = new County("Placer", "PLA", "PL", 31, "NCIC");
        public static readonly County PLUMAS = new County("Plumas", "PLU", "PU", 32, "NEIC");
        public static readonly County RIVERSIDE = new County("Riverside", "RIV", "RI", 33, "EIC");
        public static readonly County SACRAMENTO = new County("Sacramento", "SAC", "SA", 34, "NCIC");
        public static readonly County SAN_BENITO = new County("San Benito", "SBN", "SN", 35, "NWIC");
        public static readonly County SAN_BERNARDINO = new County("San Bernardino", "SBR", "SB", 36, "SCCIC");
        public static readonly County SAN_DIEGO = new County("San Diego", "SDI", "SD", 37, "SCIC");
        public static readonly County SAN_FRANSCISCO = new County("San Franscisco", "SFR", "SF", 38, "NWIC");
        public static readonly County SAN_JOAQUIN = new County("San Joaquin", "SJO", "SJ", 39, "CCaIC");
        public static readonly County SAN_LUIS_OBISPO = new County("San Luis Obispo", "SLO", "SL", 40, "CCoIC");
        public static readonly County SAN_MATEO = new County("San Mateo", "SMA", "SM", 41, "NWIC");
        public static readonly County SANTA_BARBARA = new County("Santa Barbara", "SBA", "SR", 42, "CCoIC");
        public static readonly County SANTA_CLARA = new County("Santa Clara", "SCL", "SC", 43, "NWIC");
        public static readonly County SANTA_CRUZ = new County("Santa Cruz", "SCR", "SZ", 44, "NWIC");
        public static readonly County SHASTA = new County("Shasta", "SHA", "SH", 45, "NEIC");
        public static readonly County SIERRA = new County("Sierra", "SIE", "SE", 46, "NEIC");
        public static readonly County SISKIYOU = new County("Siskiyou", "SIS", "SI", 47, "NEIC");
        public static readonly County SOLANO = new County("Solano", "SOL", "SO", 48, "NWIC");
        public static readonly County SONOMA = new County("Sonoma", "SON", "SX", 49, "NWIC");
        public static readonly County STANISLAUS = new County("Stanislaus", "STA", "ST", 50, "CCaIC");
        public static readonly County SUTTER = new County("Sutter", "SUT", "SU", 51, "NEIC");
        public static readonly County TEHAMA = new County("Tehama", "TEH", "TE", 52, "NEIC");
        public static readonly County TRINITY = new County("Trinity", "TRI", "TR", 53, "NEIC");
        public static readonly County TULARE = new County("Tulare", "TUL", "TU", 54, "SSJVIC");
        public static readonly County TUOLUMNE = new County("Tuolumne", "TUO", "TO", 55, "CCaIC");
        public static readonly County VENTURA = new County("Ventura", "VEN", "VN", 56, "SCCIC");
        public static readonly County YOLO = new County("Yolo", "YOL", "YL", 57, "NWIC");
        public static readonly County YUBA = new County("Yuba", "YUB", "YU", 58, "NCIC");

        public static IEnumerable<County> Values
        {
            get
            {
                yield return ALAMEDA;
                yield return ALPINE;
                yield return AMADOR;
                yield return BUTTE;
                yield return CALAVERAS;
                yield return COLUSA;
                yield return CONTRA_COSTA;
                yield return DEL_NORTE;
                yield return EL_DORADO;
                yield return FRESNO;
                yield return GLENN;
                yield return HUMBOLDT;
                yield return IMPERIAL;
                yield return INYO;
                yield return KERN;
                yield return KINGS;
                yield return LAKE;
                yield return LASSEN;
                yield return LOS_ANGELES;
                yield return MADERA;
                yield return MARIN;
                yield return MARIPOSA;
                yield return MENDOCINO;
                yield return MERCED;
                yield return MODOC;
                yield return MONO;
                yield return MONTEREY;
                yield return NAPA;
                yield return NEVADA;
                yield return ORANGE;
                yield return PLACER;
                yield return PLUMAS;
                yield return RIVERSIDE;
                yield return SACRAMENTO;
                yield return SAN_BENITO;
                yield return SAN_BERNARDINO;
                yield return SAN_DIEGO;
                yield return SAN_FRANSCISCO;
                yield return SAN_JOAQUIN;
                yield return SAN_LUIS_OBISPO;
                yield return SAN_MATEO;
                yield return SANTA_BARBARA;
                yield return SANTA_CLARA;
                yield return SANTA_CRUZ;
                yield return SHASTA;
                yield return SIERRA;
                yield return SISKIYOU;
                yield return SOLANO;
                yield return SONOMA;
                yield return STANISLAUS;
                yield return SUTTER;
                yield return TEHAMA;
                yield return TRINITY;
                yield return TULARE;
                yield return TUOLUMNE;
                yield return VENTURA;
                yield return YOLO;
                yield return YUBA;
            }
        }

        public static IEnumerable<County> regionCounties = from c in Values
                                                          where c.ICCurator is "NEIC"
                                                          select c;

        public static County Parse(string name)
        {
            return Values.First(c => c.Name == name);
        }
    }

    public class County : BindableBase
    {
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public string ReportAbbr { get; private set; }
        public int Number { get; private set; }
        public string ICCurator { get; private set; }
        public bool IsChecked { get; set; }

        public County(string name, string abbreviation, string reportAbbr, int number, string icCurator)
        {
            Name = name;
            Abbreviation = abbreviation;
            ReportAbbr = reportAbbr;
            Number = number;
            ICCurator = icCurator;
            IsChecked = false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
