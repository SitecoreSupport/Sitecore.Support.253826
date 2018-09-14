namespace Sitecore.Support.ContentSearch.Azure
{
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.Azure;
  using Sitecore.ContentSearch.Diagnostics;

  public class CloudSearchDocumentBuilder : Sitecore.ContentSearch.Azure.CloudSearchDocumentBuilder
  {
    public CloudSearchDocumentBuilder(IIndexable indexable, IProviderUpdateContext context)
           : base(indexable, context)
    {}

    protected override void AddField(IIndexableDataField field)
    {
      var fieldName = field.Name;
      var fieldValue = this.Index.Configuration.FieldReaders.GetFieldValue(field);

      var indexConfiguration = this.Index.Configuration.FieldMap.GetFieldConfiguration(field);
      var cloudConfiguration = indexConfiguration as CloudSearchFieldConfiguration;

      if (this.IsTextField(field) && this.IsFieldSearchable(cloudConfiguration))
      {
        this.AddField(BuiltinFields.Content, fieldValue, true);
      }

      // Based on SMAR's comments in issue 96467, the append must be false.
      this.AddField(fieldName, fieldValue, cloudConfiguration, false);
    }
  }
}
