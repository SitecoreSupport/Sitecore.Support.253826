namespace Sitecore.Support.ContentSearch.Azure
{
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.Azure;
  using Sitecore.ContentSearch.Diagnostics;

  public class CloudSearchDocumentBuilder : Sitecore.ContentSearch.Azure.CloudSearchDocumentBuilder
  {
    public CloudSearchDocumentBuilder(IIndexable indexable, IProviderUpdateContext context)
           : base(indexable, context)
    { }

    public override void AddField(IIndexableDataField field)
    {
      if (string.IsNullOrEmpty(field.Name))
      {
        CrawlingLog.Log.Warn($"[Index={base.Index.Name}] '{field.Id}' field of '{base.Indexable.Id}' item is skipped: the field name is missed.", null);
      }

      else
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
}