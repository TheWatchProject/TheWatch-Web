namespace TheWatch.Catalog.Experience;

/// <summary>Web, Blazor, dashboards, clients, mobile, MAUI, wearable, and presentation APIs.</summary>
public static class ExperienceCatalog
{
    /// <summary>Gets metadata describing this generated catalog.</summary>
    public static TheWatch.ApiCatalog.ApiCatalogDescriptor Descriptor { get; } = new(
        "Experience",
        746,
        223474,
        0,
        0,
        "2b3407d993acb2c9efb819d35228bb0e946310d51961e9a6cd13e929d6a16b73");

    /// <summary>Streams the catalog without loading the full data set into memory.</summary>
    public static IEnumerable<TheWatch.ApiCatalog.ApiCatalogEntry> ReadAll() =>
        TheWatch.ApiCatalog.ApiCatalogReader.Read(
            typeof(ExperienceCatalog).Assembly,
            "TheWatch.Catalog.Experience.Catalog.tsv.gz");
}
