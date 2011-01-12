using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Spark;
using Spark.FileSystem;

public class SparkCompiler
{
  private readonly ISparkViewEngine _engine;

  private SparkCompiler(ISparkViewEngine engine)
  {
    _engine = engine;
  }

  public static SparkCompiler Create()
  {
    var settings = new SparkSettings();
    settings.AddNamespace("System");
    settings.AddAssembly(Assembly.GetExecutingAssembly());
    settings.AddViewFolder(ViewFolderType.FileSystem, new Dictionary<string, string> { { "basePath", @"D:\Users\Ben Hall\Documents\Visual Studio 2010\Projects\TestableUI.Spike\TestableUI.Spike\" } });
    settings.SetPageBaseType(typeof(ViewDataPage));

    var engine = new SparkViewEngine(settings);

    return new SparkCompiler(engine);
  }

  public string Compile(string page)
  {
    var descriptor = new SparkViewDescriptor().AddTemplate(page);
    var sparkView = _engine.CreateInstance(descriptor);

    StringWriter writer = new StringWriter();
    sparkView.RenderView(writer);

    return writer.GetStringBuilder().ToString();
  }
}

public abstract class ViewDataPage : AbstractSparkView
{
  public string model { get; set; }
}